using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using JWT;
using JWT.Serializers;
using BDI3Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Forms;
using BDI3Mobile.Models.Responses;
using BDI3Mobile.Models.Requests;
using BDI3Mobile.Models.DBModels;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using System.Threading;
using System.IO;
using System.Linq;
using PCLStorage;
using System.IO.Compression;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.ReportModel;
using Xamarin.Essentials;
using BDI3Mobile.Models.SyncModels;

namespace BDI3Mobile.Helpers
{
    public class BDIWebServices
    {
        private HttpClient httpClient;
        private ITokenService _tokenservice;
        /// <summary>
        /// The file service.
        /// </summary>
        private readonly ISaveImageService _fileService;
        /// <summary>
        /// The size of the buffer.
        /// </summary>
        private int bufferSize = 4095;
        public BDIWebServices()
        {
            _tokenservice = DependencyService.Get<ITokenService>();
            _fileService = DependencyService.Get<ISaveImageService>();
            httpClient = DependencyService.Get<HttpClient>();
        }

        public async Task<LoginResponse> LoginUser(object parameters)
        {
            LoginResponse response;
            try
            {
                var result = await new HTTPHelper().SendPostRequest(APIConstants.LoginUri, parameters, false);
                response = JsonConvert.DeserializeObject<LoginResponse>(result) as LoginResponse;
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, urlEncoder);
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(decoder.Decode(response.token));
                tokenResponse.Token = response.token;
                _tokenservice.SetTokenResponse(tokenResponse);
                var userproducts = JsonConvert.DeserializeObject<List<ProductUserRoles>>(tokenResponse.UserRoles).Where(p => p.productId == 10);
                if (!userproducts.Any())
                    throw new Exception($"User Don't have BDI product");
                return response;
            }
            catch (Exception ex)
            {
                response = new LoginResponse();
                response.StatusCode = ex.Message;
            }
            return response;
        }


        public async Task<string> ForGotPassWord(object parameters)
        {
            try
            {
                var result = await new HTTPHelper().SendPostRequest(APIConstants.ForgotPasswordUri, parameters);
                return "Success";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return "Failed";
            }
        }

        public async Task<List<ProductResearchCodeValues>> GetResearchCodes()
        {
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.GetAsync(APIConstants.BaseUri + APIConstants.ResearchCodesUri);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var researchCodes = JsonConvert.DeserializeObject<List<ProductResearchCodeValues>>(resposeString);
                    return researchCodes;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return default(List<ProductResearchCodeValues>);
        }

        public async Task<Child> SaveChild(object parameters)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.PostAsync(APIConstants.BaseUri + APIConstants.AddChildUri, new StringContent(JsonConvert.SerializeObject(parameters), UnicodeEncoding.UTF8, "application/json"));
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (response != null && responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var child = JsonConvert.DeserializeObject<Child>(response);
                    return child;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return default(Child);
        }

        public async Task<ChildResponse> GetChildRecords(string modifiedSince = null, int? deleteType = null)
        {
            var uri = APIConstants.BaseUri + APIConstants.LatestUpdatesUri;
            object username = "";
            try
            {
                username = Application.Current.Properties["UserName"];
            }
            catch (Exception ex)
            {

            }

            object userID = "";
            try
            {
                userID = Application.Current.Properties["UserID"];
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (!string.IsNullOrEmpty(modifiedSince))
                {
                    uri += "?modifiedSince=" + modifiedSince;
                }
                if (deleteType.HasValue)
                {
                    uri += "?deleteType=" + deleteType.Value;
                }
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.GetAsync(uri);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", uri},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });

                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var childRecords = JsonConvert.DeserializeObject<ChildResponse>(resposeString);
                    return childRecords;
                }
                else
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", uri},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });
                    return new ChildResponse() { StatusCode = -1 };
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    {"StackTrace", ex.StackTrace },
                    {"Error Message", ex.Message },
                    {"RequestURI:", uri },
                    {"UserName:", username + "" },
                    {"UserID:", userID + "" }
                });
                return new ChildResponse() { StatusCode = -1 };
            }
        }

        public async Task<SearchChildModel> SearchChild(object parameters)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                string jsonData = JsonConvert.SerializeObject(parameters);
                var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(APIConstants.BaseUri + APIConstants.SearchChildUri, postData);
                var result = await response.Content.ReadAsStringAsync();
                var searchChildResponse = JsonConvert.DeserializeObject<SearchChildResponse>(result);
                if (searchChildResponse != null)
                {
                    var datas = searchChildResponse.data;
                    if (datas.Count > 0)
                    {
                        return datas[0];
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<ChildInfoResponse> GetChildInfo(string id)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.StudentInformationUri, id);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.GetAsync(apiURL);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var childInfo = JsonConvert.DeserializeObject<ChildInfoResponse>(resposeString);
                    return childInfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }


        public async Task<List<LocationResponseModel>> GetLocationRequestModel(int id)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.GetLocationUri + id);
            object username = "";
            try
            {
                username = Application.Current.Properties["UserName"];
            }
            catch (Exception ex)
            {
            }

            object userID = "";
            try
            {
                userID = Application.Current.Properties["UserID"];
            }
            catch (Exception ex)
            {
            }
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);

                var responseMessage = await httpClient.GetAsync(apiURL);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });

                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var childInfo = JsonConvert.DeserializeObject<List<LocationResponseModel>>(resposeString, settings);
                    HelperMethods.LocationModelCollection = childInfo;
                    return childInfo;
                }
                else
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });
                }
                return null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    {"StackTrace", ex.StackTrace },
                    {"Error Message", ex.Message },
                    {"RequestURI:", apiURL },
                    {"UserName:", username + "" },
                    {"UserID:", userID + "" }
                });
                return null;
            }
        }

        public async Task<BDI3Mobile.Models.DBModels.ContentCategoriesModel> GetDomainRequestModel(int id)
        {
            object username = "";
            try
            {
                username = Application.Current.Properties["UserName"];
            }
            catch (Exception ex)
            {
            }

            object userID = "";
            try
            {
                userID = Application.Current.Properties["UserID"];
            }
            catch (Exception ex)
            {
            }
            JsonSerializerSettings settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
            };
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.GetDomainUri);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.GetAsync(apiURL);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });
                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var childInfo = JsonConvert.DeserializeObject<BDI3Mobile.Models.DBModels.ContentCategoriesModel>(resposeString, settings);
                    return childInfo;
                }
                else
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });
                    return new BDI3Mobile.Models.DBModels.ContentCategoriesModel() { StatusCode = (int)responseMessage.StatusCode };
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    {"StackTrace", ex.StackTrace },
                    {"Error Message", ex.Message },
                    {"RequestURI:", apiURL },
                    {"UserName:", username + "" },
                    {"UserID:", userID + "" }
                });
                return new BDI3Mobile.Models.DBModels.ContentCategoriesModel() { StatusCode = -1 };
            }
        }

        public async Task<ChildInfoResponse> GetChildDetails(string id)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.StudentDetailsUri, id);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);

                var responseMessage = await httpClient.GetAsync(apiURL);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var childInfo = JsonConvert.DeserializeObject<ChildInfoResponse>(resposeString);
                    return childInfo;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<List<SearchStaffResponse>> GetExaminer(StaffRequestModel model)
        {
            string apiURL = APIConstants.BaseUri + APIConstants.SearchStaffUri;
            object username = "";
            try
            {
                username = Application.Current.Properties["UserName"];
            }
            catch (Exception ex)
            {
            }

            object userID = "";
            try
            {
                userID = Application.Current.Properties["UserID"];
            }
            catch (Exception ex)
            {
            }
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                string jsonData = JsonConvert.SerializeObject(model);
                var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiURL, postData);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", response.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });

                    var result = await response.Content.ReadAsStringAsync();
                    var searchStaffResponse = JsonConvert.DeserializeObject<List<SearchStaffResponse>>(result);
                    if (searchStaffResponse != null)
                    {
                        return searchStaffResponse;
                    }
                }
                else
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", response.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });
                }

                return null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    {"StackTrace", ex.StackTrace },
                    {"Error Message", ex.Message },
                    {"RequestURI:", apiURL },
                    {"UserName:", username + "" },
                    {"UserID:", userID + "" }
                });
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<List<ProgramNoteModel>> GetProgramNote(int organizationID)
        {
            string apiURL = APIConstants.BaseUri + APIConstants.GetProgramNote;
            object username = "";
            try
            {
                username = Application.Current.Properties["UserName"];
            }
            catch (Exception ex)
            {
            }

            object userID = "";
            try
            {
                userID = Application.Current.Properties["UserID"];
            }
            catch (Exception ex)
            {
            }
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);

                var responseMessage = await httpClient.PostAsync(apiURL, new StringContent(JsonConvert.SerializeObject(new ProgramNoteModel() { LabelId = null, LabelName = null, OrganizationId = 0, DeleteType = false, CreatedBy = 0, PageNo = 1, PageSize = 25, SortColumn = "labelName", SortOrder = "ASC", MaxRows = 1000, TotalRows = 0 }), UnicodeEncoding.UTF8, "application/json"));
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });

                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var progromNotes = JsonConvert.DeserializeObject<ProgramNotesResponse>(resposeString);
                    return progromNotes.data;
                }
                else
                {
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                    {
                        { "RequestURI:", apiURL},
                        { "StatusCode:", responseMessage.StatusCode + "" },
                        {"User Name:", username + "" },
                        {"User ID:", userID + "" }
                    });
                }
                return null;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    {"StackTrace", ex.StackTrace },
                    {"Error Message", ex.Message },
                    {"RequestURI:", apiURL },
                    {"UserName:", username + "" },
                    {"UserID:", userID + "" }
                });
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
        public async Task<bool> DownloadImageFileAsync()
        {
            var cts = new CancellationTokenSource();
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);

                // Step 1 : Get call
                var response = await httpClient.GetAsync(string.Format(APIConstants.BaseUri + APIConstants.GetImageUri), HttpCompletionOption.ResponseHeadersRead, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                    //throw new Exception(string.Format("The request returned with HTTP status code {0}", response.StatusCode));
                }

                // Step 2 : Filename
                var fileName = response.Content.Headers?.ContentDisposition?.FileName ?? "tmp.zip";

                // Step 3 : Get total of data
                var totalData = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                var canSendProgress = totalData != -1L;//&& progress != null;


                IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                var zipFile = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                // Step 4 : Get total of data
                //var filePath = Path.Combine(_fileService.GetStorageFolderPath(), fileName);

                // Step 5 : Download data
                using (var fileStream = OpenStream(zipFile.Path))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        var totalRead = 0L;
                        var buffer = new byte[bufferSize];
                        var isMoreDataToRead = true;

                        do
                        {
                            cts.Token.ThrowIfCancellationRequested();

                            var read = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);

                            if (read == 0)
                            {
                                isMoreDataToRead = false;
                            }
                            else
                            {
                                // Write data on disk.
                                await fileStream.WriteAsync(buffer, 0, read);

                                totalRead += read;

                                //if (canSendProgress)
                                //{
                                //    progress.Report((totalRead * 1d) / (totalData * 1d) * 100);
                                //}
                            }
                        } while (isMoreDataToRead);
                    }
                }
                try
                {
                    zipFile = await rootFolder.GetFileAsync("Product_10.zip");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
                if (zipFile != null)
                {
                    try
                    {
                        var imagepath = await rootFolder.CheckExistsAsync("Images");
                        if (imagepath != ExistenceCheckResult.NotFound)
                        {
                            var imagefolder = await rootFolder.GetFolderAsync("Images");
                            await imagefolder.DeleteAsync();
                        }

                    }
                    catch (Exception imagefolderex)
                    {
                        System.Diagnostics.Debug.WriteLine(imagefolderex.ToString());
                    }
                    var imagesFolder = await rootFolder.CreateFolderAsync("Images", PCLStorage.CreationCollisionOption.OpenIfExists);
                    ZipFile.ExtractToDirectory(zipFile.Path, imagesFolder.Path);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
                // Manage the exception as you need here.
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
            return false;
        }
        /// <summary>
        /// Opens the stream.
        /// </summary>
        /// <returns>The stream.</returns>
        /// <param name="path">Path.</param>
        private Stream OpenStream(string path)
        {
            return new FileStream(path, FileMode.OpenOrCreate, System.IO.FileAccess.Write, FileShare.None, bufferSize);
        }

        /// <summary>
        /// Gets the list of records purchased by an Organization
        /// </summary>
        /// <returns></returns>
        public async Task<List<OrganizationRecordForms>> GetOrgRecordForms()
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.GetOrgRecordForms);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);

                var responseMessage = await httpClient.GetAsync(apiURL);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var orgRecords = JsonConvert.DeserializeObject<List<OrganizationRecordForms>>(resposeString);
                    return orgRecords;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
        public async Task<CommonData> GetCommonData()
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.GetCommonData);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.GetAsync(apiURL);
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var resposeString = await responseMessage.Content.ReadAsStringAsync();
                    var commonData = JsonConvert.DeserializeObject<CommonData>(resposeString);
                    return commonData;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<ReportTypeProgramLabelResponse> GetReportParamters(object parameters)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.PostAsync(APIConstants.BaseUri + APIConstants.GetReportParameters, new StringContent(JsonConvert.SerializeObject(parameters), UnicodeEncoding.UTF8, "application/json"));
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var reportResponse = JsonConvert.DeserializeObject<ReportTypeProgramLabelResponse>(response);
                    return reportResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<List<ReportLocations>> GetLocationsForReport()
        {

            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var responseMessage = await httpClient.GetAsync(APIConstants.BaseUri + APIConstants.GetLocations);
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var locationResponse = JsonConvert.DeserializeObject<List<ReportLocations>>(response);
                    return locationResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;

            }
        }

        public async Task<List<ReportChild>> GetChildData(object selectedLocations)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.GetChildren);

            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                string jsonData = JsonConvert.SerializeObject(selectedLocations);
                var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var responseMessage = await httpClient.PostAsync(apiURL, postData);
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var chilResponse = JsonConvert.DeserializeObject<List<ReportChild>>(response);
                    return chilResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<List<ReportRecordForms>> GetBatteryTypes(object parameters)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.GetBatteryTypes);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                string jsonData = JsonConvert.SerializeObject(parameters);
                var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var responseMessage = await httpClient.PostAsync(apiURL, postData);
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var reportResponse = JsonConvert.DeserializeObject<List<ReportRecordForms>>(response);
                    return reportResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }

        public async Task<ResponseCriteria> SaveReportCriteria(object parameters)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.SaveReportCriteria);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                string jsonData = JsonConvert.SerializeObject(parameters);
                var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var responseMessage = await httpClient.PostAsync(apiURL, postData);
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var reportResponse = JsonConvert.DeserializeObject<ResponseCriteria>(response);
                    return reportResponse;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;
            }
        }
        public async Task<string> ExecuteReport(string criteriaID, string reportName, string format)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.ExecuteReport + "/" + criteriaID);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                var beforetime = DateTime.Now;
                var responseMessage = await httpClient.GetAsync(apiURL);
                var response = await responseMessage.Content.ReadAsStringAsync();
                var aftertime = DateTime.Now;
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    object username = "";
                    if (Application.Current.Properties.ContainsKey("UserName"))
                    {
                        username = Application.Current.Properties["UserName"];
                    }
                    object userID = "";
                    if (Application.Current.Properties.ContainsKey("UserID"))
                    {
                        userID = Application.Current.Properties["UserID"];
                    }
                    Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                        {
                            { "RequestURI:", apiURL},
                            { "StatusCode:", responseMessage.StatusCode + "" },
                            {"User Name:", username + "" },
                            {"User ID:", userID + "" },
                            {"Time Taken:", (aftertime - beforetime).TotalSeconds + "" }
                        });
                    var fileBytes = await responseMessage.Content.ReadAsByteArrayAsync();
                    if (fileBytes != null && fileBytes.Length > 0)
                    {
                        IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;

                        string fileName = reportName + format;
                        IFile reportFile = await rootFolder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
                        using (Stream stream = await reportFile.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                        {
                            stream.Write(fileBytes, 0, fileBytes.Length);
                            stream.Dispose();
                        }
                        return reportFile.Name;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return null;

            }
        }

        public async Task<List<SyncTestRecordResult>> SyncTestRecords(object parameters, Action<string> Error)
        {
            string apiURL = string.Format(APIConstants.BaseUri + APIConstants.SyncTestRecord);
            try
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _tokenservice.GetTokenResposne().Token);
                string jsonData = JsonConvert.SerializeObject(parameters);
                var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                var responseMessage = await httpClient.PostAsync(apiURL, postData);
                var response = await responseMessage.Content.ReadAsStringAsync();
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var reportResponse = JsonConvert.DeserializeObject<List<SyncTestRecordResult>>(response);
                    return reportResponse;
                }
                if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound && (Connectivity.NetworkAccess == NetworkAccess.None || Connectivity.NetworkAccess == NetworkAccess.Unknown))
                {
                    Error?.Invoke("nointernet");
                }
                else
                {
                    Error?.Invoke("Commit Failed with Status Code: " + responseMessage.StatusCode);
                }
                return null;
            }
            catch (Exception ex)
            {
                if (ex != null && ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) &&
                    (ex.InnerException.Message.Contains("The server name or address could not be resolved") || ex.InnerException.Message.Contains("An internal error occurred in the Microsoft Internet extensions")))
                {
                    throw ex;
                }
                Error?.Invoke(ex.Message);
                return null;
            }
        }
    }
}
