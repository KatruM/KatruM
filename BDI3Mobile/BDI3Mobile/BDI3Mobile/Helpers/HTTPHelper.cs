using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;
using BDI3Mobile.Common;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace BDI3Mobile.Helpers
{
    public class HTTPHelper
    {
        private readonly static string reservedCharacters = "!*'();:@&=+$,/?%#[]";
        public async Task<string> SendPostRequest(string serviceName, object parameters, bool logDatatoAppCenter = true)
        {
            string url = GetBaseAddress() + serviceName;
            string jsonData = JsonConvert.SerializeObject(parameters);
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
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/78.0.3904.108 Safari/537.36");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
                    var postData = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, postData);

                    if (logDatatoAppCenter)
                    {
                        Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                        {
                            { "RequestURI:", url},
                            { "StatusCode:", response.StatusCode + "" },
                            {"User Name:", username + "" },
                            {"User ID:", userID + "" }
                        });
                    }
                    else
                    {
                        if (logDatatoAppCenter)
                        {
                            Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                        {
                            { "RequestURI:", url},
                            { "PayLoad:", jsonData},
                            { "StatusCode:", response.StatusCode + "" },
                            {"User Name:", username + "" },
                            {"User ID:", userID + "" }
                        });
                        }
                    }

                    

                    if (!response.IsSuccessStatusCode)
                    {
                        Analytics.TrackEvent("API Requests", new Dictionary<string, string>
                        {
                            { "RequestURI:", url},
                            { "PayLoad:", jsonData},
                            { "StatusCode:", response.StatusCode + "" },
                            {"UserName:", username + "" },
                            {"UserID:", userID + "" }
                        });
                        throw new Exception($"Http request failed. Status code : {response.StatusCode}.");
                    }
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>
                {
                    {"StackTrace", ex.StackTrace },
                    {"Error Message", ex.Message },
                    {"RequestURI:", url },
                    { "PayLoad:", jsonData },
                    {"UserName:", username + "" },
                    {"UserID:", userID + "" }
                });

                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        public string GetBaseAddress()
        {
            return APIConstants.BaseUri;
        }
    }
}
