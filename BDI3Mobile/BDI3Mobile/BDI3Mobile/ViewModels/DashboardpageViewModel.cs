using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;
using BDI3Mobile.Helpers;
using BDI3Mobile.IServices;
using BDI3Mobile.Views.AddChildViews;
using BDI3Mobile.Views;
using BDI3Mobile.Views.AssesmentViews;
using Rg.Plugins.Popup.Services;
using Plugin.Connectivity;
using BDI3Mobile.Models;
using BDI3Mobile.Models.DBModels;
using System.Linq;
using Acr.UserDialogs;
using BDI3Mobile.Models.Responses;
using System.Threading.Tasks;
using BDI3Mobile.Views.PopupViews;
using BDI3Mobile.Common;
using BDI3Mobile.Models.Requests;
using Newtonsoft.Json.Linq;

namespace BDI3Mobile.ViewModels
{
    public class DashboardpageViewModel : BindableObject
    {
        private readonly IUsersService userService = DependencyService.Get<IUsersService>();
        private readonly IUserSyncService userSyncService = DependencyService.Get<IUserSyncService>();
        private BDIWebServices services = new BDIWebServices();
        private readonly IProductResearchCodeValuesService _productResearchCodeValuesService;
        private readonly IProductResearchCodesService _productResearchCodesService;
        private readonly IStudentsService _studentService;
        private readonly IProductService _productService;
        private readonly IAssessmentsService _assessmentService;
        private readonly IContentCategoryLevelsService _contentCategoryLevelsService;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IContentCategoryItemsService _contentCategoryItemsService;
        private readonly IContentItemsService _contentItemService;
        private readonly IContentItemAttributesService _contentItemAttributeService;
        private readonly IContentRubricsService _contentRubricsService;
        private readonly IContentRubricPointsService _contentRubicPointsService;
        private readonly IContentItemTallyService _contentItemTalliesService;
        private readonly IContentItemTalliesScoresService _contentItemTalliesScoresService;
        private readonly IContentGroupService _contentGroupService;
        private readonly IContentGroupItemsService _contentGroupItemsService;
        private readonly ILocationService _locationService;
        private readonly IProgramNoteService _programNoteService;
        private readonly IExaminerService _examinerService;
        private readonly IOrgRecordFormService _orgRecordFormService;
        private readonly IUserPermissionService _userPermissionService;


        private readonly ITokenService _tokenService;
        private bool isInternetAvailable;
        private bool userTappedBrowseRecord;
        private bool userTappedAddNewRecord;
        private bool userTappedResumeAssessment;
        private bool userTappedNewAssessment;
        private bool userTappedBasicReport;
        private bool userTappedFullReport;
        private string userFullName;
        public bool IsInternetAvailable
        {
            get
            {
                return isInternetAvailable;
            }
            set
            {
                isInternetAvailable = value;
                OnPropertyChanged(nameof(IsInternetAvailable));
            }
        }
        public string DeviceID { get; set; }
        public string UserFullName
        {
            get { return userFullName; }
            set
            {
                userFullName = value;
                OnPropertyChanged(nameof(userFullName));
            }

        }

        private bool showInfo { get; set; } = false;
        public bool ShowInfo
        {
            get { return showInfo; }
            set
            {
                showInfo = value;
                OnPropertyChanged(nameof(ShowInfo));
            }

        }

        private string pendingString;
        public string PendingString
        {
            get
            {
                return pendingString;
            }
            set
            {
                pendingString = value;
                OnPropertyChanged(nameof(PendingString));
            }
        }

        private string failedString;
        public string FailedString
        {
            get
            {
                return failedString;
            }
            set
            {
                failedString = value;
                OnPropertyChanged(nameof(FailedString));
            }
        }

        private string currentTime = string.Format("{0}: {1} {2}", "Last Connection", DateTime.Now.Month + "/" + DateTime.Now.Day + "/" + DateTime.Now.Year, DateTime.Now.ToShortTimeString());
        public string CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }

        public ICommand NewAssessmentCommand { get; set; }
        public ICommand ResumeAssessmentCommand { get; set; }
        public ICommand BrowseUpdateRecordCommand { get; set; }
        public ICommand AddNewRecordCommand { get; set; }
        public ICommand ReportToFamilyCommand { get; set; }
        public ICommand OpenFullReportCommand { get; set; }
        public ICommand ShowInfoCommand { get; set; }
        public ICommand HomeCommand { get; set; }


        public DashboardpageViewModel()
        {
            _locationService = DependencyService.Get<ILocationService>();
            _tokenService = DependencyService.Get<ITokenService>();
            _studentService = DependencyService.Get<IStudentsService>();
            _productResearchCodesService = DependencyService.Get<IProductResearchCodesService>();
            _productResearchCodeValuesService = DependencyService.Get<IProductResearchCodeValuesService>();
            _productService = DependencyService.Get<IProductService>();
            _assessmentService = DependencyService.Get<IAssessmentsService>();
            _contentCategoryLevelsService = DependencyService.Get<IContentCategoryLevelsService>();
            _contentCategoryService = DependencyService.Get<IContentCategoryService>();
            _contentCategoryItemsService = DependencyService.Get<IContentCategoryItemsService>();
            _programNoteService = DependencyService.Get<IProgramNoteService>();
            _examinerService = DependencyService.Get<IExaminerService>();
            _contentItemService = DependencyService.Get<IContentItemsService>();
            _contentItemAttributeService = DependencyService.Get<IContentItemAttributesService>();
            _contentRubricsService = DependencyService.Get<IContentRubricsService>();
            _contentRubicPointsService = DependencyService.Get<IContentRubricPointsService>();
            _contentItemTalliesService = DependencyService.Get<IContentItemTallyService>();
            _contentItemTalliesScoresService = DependencyService.Get<IContentItemTalliesScoresService>();
            _contentGroupService = DependencyService.Get<IContentGroupService>();
            _contentGroupItemsService = DependencyService.Get<IContentGroupItemsService>();
            _orgRecordFormService = DependencyService.Get<IOrgRecordFormService>();
            _userPermissionService = DependencyService.Get<IUserPermissionService>();

            var userID = Convert.ToInt32(Application.Current.Properties["UserID"]);
            var user = userService.GetUserByID(userID);
            if (user != null)
            {
                DeviceID = user.DeviceId;
            }
            CheckInternet();
            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                IsInternetAvailable = false;
                string userFirstName = "", userLastName = "";
                if (Application.Current.Properties.ContainsKey("FirstName") && Application.Current.Properties["FirstName"] != null)
                {
                    userFirstName = Application.Current.Properties["FirstName"].ToString();
                }
                if (Application.Current.Properties.ContainsKey("LastName") && Application.Current.Properties["LastName"] != null)
                {
                    userLastName = Application.Current.Properties["LastName"].ToString();
                }

                UserFullName = "" + userFirstName + "  " + "" + userLastName + "";
            }
            else
            {
                IsInternetAvailable = true;
                _tokenService = DependencyService.Get<ITokenService>();
                var tokenModel = _tokenService.GetTokenResposne();
                if (tokenModel != null)
                {
                    UserFullName = "" + tokenModel.FirstName + "  " + "" + tokenModel.LastName + "";
                }
            }

            FailedString = "Failed: 0";
            PendingString = "Pending: 0";

            NewAssessmentCommand = new Command(NewAssessment);
            ResumeAssessmentCommand = new Command(ResumeAssessment);
            BrowseUpdateRecordCommand = new Command(BrowseUpdate);
            AddNewRecordCommand = new Command(AddNew);
            ReportToFamilyCommand = new Command(Report);
            OpenFullReportCommand = new Command(OpenFullReport);
            ShowInfoCommand = new Command(openInfo);

            HomeCommand = new Command(async () =>
            {
                var popUpNavigationInstance = PopupNavigation.Instance;
                await popUpNavigationInstance.PushAsync(new LogoutpopupView());
                //await App.Current.MainPage.Navigation.PushModalAsync(new LogoutpopupView());
                //if (!Application.Current.Properties.ContainsKey("RememberMe") || Application.Current.Properties["RememberMe"].ToString() == "False")
                //{
                //Application.Current.Properties.Clear();
                //}
                //App.LogoutAction?.Invoke(false);
            });

        }

        void openInfo(object obj)
        {
            ShowInfo = true;
        }


        async void NewAssessment()
        {
            if (userTappedNewAssessment)
                return;

            userTappedNewAssessment = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new NewAssessmentView());
            userTappedNewAssessment = false;
        }

        async void ResumeAssessment()
        {
            if (userTappedResumeAssessment)
                return;

            userTappedResumeAssessment = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new ResumeSyncAssesmentView());
            userTappedResumeAssessment = false;
        }

        async void BrowseUpdate()
        {
            if (userTappedBrowseRecord)
                return;

            userTappedBrowseRecord = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new SearchEditChildView());
            userTappedBrowseRecord = false;
        }

        async void AddNew()
        {
            if (userTappedAddNewRecord)
                return;

            userTappedAddNewRecord = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new ChildTabbedPage(0));
            userTappedAddNewRecord = false;
        }


        async void Report()
        {
            if (userTappedBasicReport)
                return;
            userTappedBasicReport = true;
            await PopupNavigation.Instance.PushAsync(new BasicReport(), false);
            await Task.Delay(500);
            userTappedBasicReport = false;
        }

        async void OpenFullReport()
        {
            if (userTappedFullReport)
                return;
            userTappedFullReport = true;
            await PopupNavigation.Instance.PushAsync(new FullReport(), false);
            await Task.Delay(500);
            userTappedFullReport = false;
        }


        public ICommand iconhelp => new Command<string>((url) => { Xamarin.Essentials.Launcher.TryOpenAsync(new System.Uri(url)); });

        public void CheckInternet()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                IsInternetAvailable = args.IsConnected ? true : false;
            };
        }

        #region FirstTime and Download
        public List<BDI3Mobile.Models.DBModels.Location> GenerateSubLocations(IEnumerable<LocationResponseModel> subItems, List<Models.DBModels.Location> locations, int parentlocationid, int downloadedBy)
        {
            if (subItems != null && subItems.Any())
            {
                foreach (var item in subItems)
                {
                    var subLocation = new Models.DBModels.Location();
                    subLocation.LocationId = item.value;
                    subLocation.LocationName = item.text;
                    subLocation.IsEnabled = item.enabled;
                    subLocation.DownloadedBy = downloadedBy;
                    subLocation.updatedOn = item.updatedOn;
                    subLocation.isDeleted = item.isDeleted;
                    subLocation.ParentLocationId = item.parentLocationID;
                    subLocation.UserId = int.Parse(Application.Current.Properties["UserID"].ToString());
                    locations.Add(subLocation);
                    GenerateSubLocations(item.subItems, locations, item.value, downloadedBy);
                }
            }
            return null;
        }
        private string downloadProgressMessage;
        public string DownloadProgressMessage
        {
            get
            {
                return downloadProgressMessage;
            }
            set
            {
                downloadProgressMessage = value;
                OnPropertyChanged(nameof(DownloadProgressMessage));
            }
        }
        public async Task<bool> CheckFirstTimeAndDownload()
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            var username = Convert.ToString(Application.Current.Properties["UserName"]);
            var password = Convert.ToString(Application.Current.Properties["PassID"]);
            try
            {
                var response = await services.LoginUser(new { username = username.Trim(), password = password.Trim() });
                if (response != null)
                {
                    if (!string.IsNullOrEmpty(response.StatusCode))
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Download Failed!");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                if (!string.IsNullOrEmpty(ex.Message) && ex.Message == "User Don't have BDI product")
                {
                    await UserDialogs.Instance.AlertAsync("You Don't have BDI product access. Please contact Admin");
                }
                else
                {
                    await UserDialogs.Instance.AlertAsync("Download Failed!");
                }
                return false;
            }
            ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
            var lstlocalLocations = default(List<Location>);
            var needtoDeleteLocations = default(List<Location>);
            UserDialogs.Instance.HideLoading();
            await PopupNavigation.Instance.PushAsync(new SyncingPopupView() { BindingContext = this });
            double totalTaskCount = 11;
            var percentageCompleted = 100;
            DownloadProgressMessage = "0%";

            var userResponse = _tokenService.GetTokenResposne();
            int uID, organizationID;
            int.TryParse(userResponse.UserID, out uID);
            int.TryParse(userResponse.OrganizationID, out organizationID);

            var contentSyncData = userSyncService.GetContentSyncData(ContentTypes.Content.ToString());
            var contentImageSyncData = userSyncService.GetContentSyncData(ContentTypes.Images.ToString());
            var userLastSync = userSyncService.GetUserSyncTable(uID);
            var childRecords = await services.GetChildRecords(modifiedSince: userLastSync.LastSyncDatetime);
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / totalTaskCount) + "%";
            totalTaskCount -= 1;
            if (childRecords != null && childRecords.StatusCode != 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            var exisingRecords = _studentService.GetStudentsByDownloaded(uID);
            if (exisingRecords != null && exisingRecords.Any())
            {
                if (childRecords != null && childRecords.Childrens != null && childRecords.Childrens.Any())
                {
                    var newlyAddedRecords = childRecords.Childrens.Where(p => !exisingRecords.Select(q => q.UserId).Contains(p.ChildUserID)).ToList();
                    if (newlyAddedRecords != null && newlyAddedRecords.Any())
                    {
                        try
                        {
                            var needtoInsert = new List<Students>();
                            GenerateStudentList(newlyAddedRecords, needtoInsert, uID);
                            _studentService.InsertAll(needtoInsert);
                            var lstResearchCodeValues = new List<ProductResearchCodeValues>();
                            foreach (var item in childRecords.Childrens)
                            {
                                if (item.ResearchCodes != null && item.ResearchCodes.Any())
                                {
                                    foreach (var innerItem in item.ResearchCodes)
                                    {
                                        var ProductResearchCodeValues = new ProductResearchCodeValues();
                                        ProductResearchCodeValues.OrganizationId = organizationID;
                                        ProductResearchCodeValues.value = innerItem.value;
                                        ProductResearchCodeValues.ResearchCodeValueId = innerItem.ResearchCodeValueId;
                                        ProductResearchCodeValues.ResearchCodeId = innerItem.ResearchCodeId;
                                        ProductResearchCodeValues.OfflineStudentID = needtoInsert.FirstOrDefault(p => p.UserId == item.ChildUserID.ToString()).OfflineStudentID;
                                        lstResearchCodeValues.Add(ProductResearchCodeValues);
                                    }

                                }
                            }
                            if (lstResearchCodeValues != null && lstResearchCodeValues.Any())
                            {
                                _productResearchCodeValuesService.InsertAll(lstResearchCodeValues);
                            }
                        }
                        catch (Exception ex)
                        {
                            Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                        }
                    }

                    var needtoUpdateRecords = exisingRecords.Where(p => childRecords.Childrens.Select(q => q.ChildUserID).Contains(p.UserId)).ToList();
                    if (needtoUpdateRecords != null && needtoUpdateRecords.Any())
                    {
                        foreach (var item in needtoUpdateRecords)
                        {
                            var serverRecord = childRecords.Childrens.FirstOrDefault(p => p.ChildUserID.ToString() == item.UserId);
                            if (serverRecord.isDeleteStatus == 1)
                            {
                                var needtoInsert = new List<Students>();
                                GenerateStudentList(new List<Child>() { serverRecord }, needtoInsert, uID);
                                needtoInsert.FirstOrDefault().OfflineStudentID = item.OfflineStudentID;
                                _studentService.Update(needtoInsert.FirstOrDefault());
                            }
                            else
                            {
                                DateTime dateTime;
                                DateTime.TryParse(serverRecord.updatedOnUTC, out dateTime);

                                DateTime itemdateTime;
                                DateTime.TryParse(item.updatedOn, out itemdateTime);

                                if (serverRecord != null && dateTime > itemdateTime)
                                {
                                    var needtoInsert = new List<Students>();
                                    GenerateStudentList(new List<Child>() { serverRecord }, needtoInsert, uID);
                                    needtoInsert.FirstOrDefault().OfflineStudentID = item.OfflineStudentID;
                                    _studentService.Update(needtoInsert.FirstOrDefault());
                                    _productResearchCodeValuesService.DeleteByStudentId(item.OfflineStudentID);
                                    var lstResearchCodeValues = new List<ProductResearchCodeValues>();
                                    foreach (var innerItem in serverRecord.ResearchCodes)
                                    {
                                        var ProductResearchCodeValues = new ProductResearchCodeValues();
                                        ProductResearchCodeValues.OrganizationId = organizationID;
                                        ProductResearchCodeValues.value = innerItem.value;
                                        ProductResearchCodeValues.ResearchCodeValueId = innerItem.ResearchCodeValueId;
                                        ProductResearchCodeValues.ResearchCodeId = innerItem.ResearchCodeId;
                                        ProductResearchCodeValues.OfflineStudentID = needtoInsert.FirstOrDefault(p => p.UserId == serverRecord.ChildUserID.ToString()).OfflineStudentID;
                                        lstResearchCodeValues.Add(ProductResearchCodeValues);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                }
            }
            else
            {
                var studentsToInsert = new List<Students>();
                if (childRecords != null)
                {
                    GenerateStudentList(childRecords.Childrens, studentsToInsert, uID);
                }
                if (studentsToInsert != null && studentsToInsert.Any())
                {
                    try
                    {
                        _studentService.InsertAll(studentsToInsert);
                        var lstResearchCodeValues = new List<ProductResearchCodeValues>();
                        foreach (var childItem in childRecords.Childrens)
                        {
                            if (childItem.ResearchCodes != null && childItem.ResearchCodes.Any())
                            {
                                foreach (var innerItem in childItem.ResearchCodes)
                                {
                                    var ProductResearchCodeValues = new ProductResearchCodeValues();
                                    ProductResearchCodeValues.OrganizationId = organizationID;
                                    ProductResearchCodeValues.value = innerItem.value;
                                    ProductResearchCodeValues.ResearchCodeValueId = innerItem.ResearchCodeValueId;
                                    ProductResearchCodeValues.ResearchCodeId = innerItem.ResearchCodeId;
                                    ProductResearchCodeValues.OfflineStudentID = studentsToInsert.FirstOrDefault(p => p.UserId == childItem.ChildUserID.ToString()).OfflineStudentID;
                                    lstResearchCodeValues.Add(ProductResearchCodeValues);
                                }
                            }
                        }
                        if (lstResearchCodeValues != null && lstResearchCodeValues.Any())
                        {
                            _productResearchCodeValuesService.InsertAll(lstResearchCodeValues);
                        }
                    }
                    catch (Exception ex)
                    {
                        Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
                    }
                }
            }

            if (childRecords != null && childRecords.ResearchCodes != null && childRecords.ResearchCodes.Any())
            {
                _productResearchCodesService.DeleteAll(organizationID);
                if (childRecords.ResearchCodes != null && childRecords.ResearchCodes.Any())
                {
                    _productResearchCodesService.InsertAll(childRecords.ResearchCodes);
                }
            }
            var locations = await new BDIWebServices().GetLocationRequestModel(uID);
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / totalTaskCount) + "%";
            totalTaskCount -= 2;
            if (locations == null || !locations.Any())
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                if (locations != null && locations.Any())
                {
                    var totalLocations = new List<Models.DBModels.Location>();
                    foreach (var item in locations)
                    {
                        var location = new Models.DBModels.Location();
                        location.LocationId = item.value;
                        location.LocationName = item.text;
                        location.IsEnabled = item.enabled;
                        location.DownloadedBy = uID;
                        location.isDeleted = item.isDeleted;
                        location.updatedOn = item.updatedOn;
                        location.ParentLocationId = item.parentLocationID;
                        location.UserId = uID;
                        totalLocations.Add(location);
                        GenerateSubLocations(item.subItems, totalLocations, 0, uID);
                    }
                    lstlocalLocations = _locationService.GetAllByDownloadedByLocations(uID);
                    _locationService.DeleteByDownloadedBy(uID);
                    needtoDeleteLocations = lstlocalLocations.Where(p => !totalLocations.Select(q => q.LocationId).Contains(p.LocationId)).ToList();
                    _locationService.InsertAll(totalLocations);
                }
            }
            percentageCompleted = 50;
            totalTaskCount = 1.5;
            DownloadProgressMessage = (Convert.ToInt32(percentageCompleted / totalTaskCount) + percentageCompleted) + "%";
            totalTaskCount -= 1;
            var examiners = await new BDIWebServices().GetExaminer(new Models.Requests.StaffRequestModel());
            DownloadProgressMessage = Convert.ToInt32(percentageCompleted / totalTaskCount) + "%";
            totalTaskCount -= 1;
            if (examiners == null || !examiners.Any())
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                _examinerService.DeleteByDownloadedBy(uID);
                if (examiners != null && examiners.Any())
                {
                    var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
                    foreach (var item in examiners)
                    {
                        item.DownloadedBy = uID;
                        item.UserID = KeyEncryption.Decrypt(item.UserID);
                        item.OrganizationId = orgId;
                    }
                    _examinerService.InsertAll(examiners);
                }
                commonDataService.SearchStaffResponseModel = _examinerService.GetExamainer() ?? new List<SearchStaffResponse>();
            }
            var programNotes = await new BDIWebServices().GetProgramNote(organizationID);
            if (programNotes == null)
            {
                await PopupNavigation.Instance.PopAllAsync();
                await UserDialogs.Instance.AlertAsync("Download Failed!");
                return false;
            }
            else
            {
                DownloadProgressMessage = "100%";
                _programNoteService.DeleteByDownloadedBy(uID);
                if (programNotes != null && programNotes.Any())
                {
                    var totalProgram = new List<ProgramNoteModel>();
                    foreach (var item in programNotes)
                    {
                        var programNote = new ProgramNoteModel();
                        programNote.LabelId = item.LabelId;
                        programNote.LabelName = item.LabelName;
                        programNote.DeleteType = item.DeleteType;
                        programNote.DownLoadedBy = uID;
                        programNote.updatedOn = item.updatedOn;
                        programNote.OrganizationId = item.OrganizationId;
                        totalProgram.Add(programNote);
                    }
                    _programNoteService.InsertAll(totalProgram);
                }
                commonDataService.ProgramNoteModels = _programNoteService.GetProgramNote() ?? new List<ProgramNoteModel>();
            }
            try
            {
                var organizationRecordForms = await services.GetOrgRecordForms();
                _orgRecordFormService.DeleteAll();
                if (organizationRecordForms != null && organizationRecordForms.Any())
                {
                    organizationRecordForms.ForEach((item) =>
                    {
                        item.DownloadedBy = uID;
                        item.OrganizationId = organizationID;
                    });
                    _orgRecordFormService.Insert(organizationRecordForms);
                }
                commonDataService.OrgRecordFormList = _orgRecordFormService.GetRecordForms() ?? new List<OrganizationRecordForms>();
            }
            catch (Exception ex)
            {

            }
            var localLocations = _locationService.GetLocations();
            if (needtoDeleteLocations != null && needtoDeleteLocations.Any())
            {
                foreach (var item in needtoDeleteLocations)
                {
                    var deletedLocation = lstlocalLocations.FirstOrDefault(p => p.LocationId == item.LocationId);
                    if (deletedLocation != null)
                    {
                        deletedLocation.isDeleted = true;
                    }
                }
                var locationIDs = needtoDeleteLocations.Select(p => p.LocationId).ToList();
                var locaDownlodedBtStudents = _studentService.GetStudentsByDownloaded(uID);
                if (locaDownlodedBtStudents != null && locaDownlodedBtStudents.Any())
                {
                    foreach (var item in locaDownlodedBtStudents)
                    {
                        if (item.SelectedLocationId.HasValue && locationIDs.Contains(item.SelectedLocationId.Value))
                        {
                            var locationID = CheckRecursiveLOcationsUpdate(item.SelectedLocationId.Value, lstlocalLocations);
                            item.SelectedLocationId = locationID;
                            _studentService.Update(item);
                        }
                    }
                }
            }
            var userpermissions = default(List<string>);
            if (JObject.Parse(_tokenService.GetTokenResposne().Perms).ToObject<Dictionary<int, string>>().ContainsKey(10))
            {
                userpermissions = JObject.Parse(_tokenService.GetTokenResposne().Perms).ToObject<Dictionary<int, string>>().Where(p => p.Key == 10).Select(p => p.Value).ToList();
                foreach (var item in userpermissions)
                {
                    await _userPermissionService.DeleteAllAsync();
                    var allUserPermissionList = new List<Models.DBModels.UserPermissions>();
                    var splitids = item.Split(',').ToList();
                    foreach (var perId in splitids)
                    {
                        var userPermissions = new Models.DBModels.UserPermissions()
                        {
                            UserId = uID,
                            PermissionId = Convert.ToInt32(perId)
                        };
                        allUserPermissionList.Add(userPermissions);
                    }
                    await _userPermissionService.InsertAllAsync(allUserPermissionList);
                }
            }
            await PopupNavigation.Instance.PopAllAsync();
            var date = DateTime.Now.ToUniversalTime().ToString("s") + "Z";
            userLastSync.LastSyncDatetime = date;
            userSyncService.UpdateUserSync(userLastSync);
            return true;
        }

        private int CheckRecursiveLOcationsUpdate(int locationID, List<Location> lstLocation)
        {
            var location = lstLocation.FirstOrDefault(p => p.LocationId == locationID);
            if (location != null && location.isDeleted)
            {
                var parentLocation = lstLocation.FirstOrDefault(p => p.LocationId == location.ParentLocationId);
                if (parentLocation != null && parentLocation.isDeleted)
                {
                    return CheckRecursiveLOcationsUpdate(parentLocation.LocationId, lstLocation);
                }
                else
                {
                    return parentLocation.LocationId;
                }
            }
            return location.LocationId;
        }
        public void GenerateStudentList(List<Child> childRecords, List<Students> students, int addedBy)
        {
            foreach (var childRecord in childRecords)
            {
                int gender = 0, language = 0;
                if (childRecord.Language != null && childRecord.Language.Count() > 0)
                {
                    language = childRecord.Language.FirstOrDefault().value;
                }
                if (childRecord.Gender != null && childRecord.Gender.Count() > 0)
                {
                    gender = childRecord.Gender.FirstOrDefault().value;
                }
                var selectedEthnicityIds = "";
                if (childRecord.Ethnicity != null && childRecord.Ethnicity.Any())
                {
                    selectedEthnicityIds = string.Join(",", childRecord.Ethnicity.Select(p => p.value));
                }
                var selectedFundingSourceIds = "";
                if (childRecord.FundingSources != null && childRecord.FundingSources.Any())
                {
                    selectedFundingSourceIds = string.Join(",", childRecord.FundingSources.Select(p => p.value));
                }
                var selectedRaceIds = "";
                if (childRecord.Race != null && childRecord.Race.Any())
                {
                    selectedRaceIds = string.Join(",", childRecord.Race.Select(p => p.value));
                }
                var selectedPrimaryDiagnosesIds = "";
                if (childRecord.Diagnoses != null && childRecord.Diagnoses.Any())
                {
                    selectedPrimaryDiagnosesIds = string.Join(",", childRecord.Diagnoses.Select(p => p.value));
                }
                var selectedSecondaryDiagnosesIds = "";
                if (childRecord.SecondaryDiagnoses != null && childRecord.SecondaryDiagnoses.Any())
                {
                    selectedSecondaryDiagnosesIds = string.Join(",", childRecord.SecondaryDiagnoses.Select(p => p.value));
                }
                var selectedLanguageIds = "";
                if (childRecord.Language != null && childRecord.Language.Any())
                {
                    selectedLanguageIds = string.Join(",", childRecord.Language.Select(p => p.value));
                }
                int? locationID = null;
                if (childRecord.Location != null && childRecord.Location.Any())
                {
                    locationID = childRecord.Location.FirstOrDefault().value;
                }
                var childToAdd = new Models.DBModels.Students()
                {
                    isDeleteStatus = childRecord.isDeleteStatus,
                    updatedOn = childRecord.updatedOn,
                    updatedOnUTC = childRecord.updatedOnUTC,
                    DownloadedBy = addedBy,
                    SelectedEthnictyIds = selectedEthnicityIds,
                    SelectedFundingSourceIds = selectedFundingSourceIds,
                    SelectedLanguageIds = selectedLanguageIds,
                    SelectedPrimaryDiagnosesIds = selectedPrimaryDiagnosesIds,
                    SelectedRaceIds = selectedRaceIds,
                    SelectedSecondaryDiagnosesIds = selectedSecondaryDiagnosesIds,
                    StudentID = childRecord.ChildUserID.ToString(),
                    FirstName = childRecord.FirstName,
                    LastName = childRecord.LastName,
                    MiddleName = childRecord.MiddleName,
                    Gender = gender,
                    ChildID = childRecord.ChildId,
                    Birthdate = CovertToDateFromString(childRecord.DOB),
                    EnrollmentDate = CovertToDateFromString(childRecord.EnrollmentDate),
                    UserId = childRecord.ChildUserID.ToString(),
                    AddedBy = childRecord.AddedBy,
                    ParentGuardian1 = childRecord.Parent1Name,
                    ParentGuardian2 = childRecord.Parent2Name,
                    ParentEmailAddress1 = childRecord.Parent1Email,
                    ParentEmailAddress2 = childRecord.Parent2Email,
                    IsIFSP = Convert.ToByte(childRecord.IFSP),
                    IsIEP = Convert.ToByte(childRecord.IEP),
                    IEPEligibilityDate = CovertToDateFromString(childRecord.IEPInitialDate),
                    IEPExitDate = CovertToDateFromString(childRecord.IEPExitDate),
                    IFSPEligibilityDate = CovertToDateFromString(childRecord.IFSPInitialDate),
                    IFSPExitDate = CovertToDateFromString(childRecord.IFSPExitDate),
                    IsFreeLunch = Convert.ToByte(childRecord.FreeLunch),
                    IsSynced = true,
                    SelectedLocationId = locationID
                };
                students.Add(childToAdd);
            }
        }

        private DateTime CovertToDateFromString(string obj)
        {
            if (obj == null)
            {
                return default(DateTime);
            }
            else
            {
                DateTime dateTime;
                bool isSucess = DateTime.TryParse(obj, out dateTime);
                if (isSucess)
                {
                    return dateTime;
                }
            }
            return default(DateTime);
        }
        #endregion
    }
}
