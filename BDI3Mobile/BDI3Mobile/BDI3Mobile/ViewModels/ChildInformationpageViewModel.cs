using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using BDI3Mobile.Helpers;
using Xamarin.Essentials;
using BDI3Mobile.Common;
using Acr.UserDialogs;
using BDI3Mobile.IServices;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BDI3Mobile.Models.Responses;
using BDI3Mobile.LookUps.ChildPageLookUps;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using BDI3Mobile.Views.AcademicSurvey;
using BDI3Mobile.Services.DigitalTestRecordService;
using Newtonsoft.Json;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using Newtonsoft.Json.Linq;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.SyncModels;
using BDI3Mobile.Models.DBModels;

namespace BDI3Mobile.ViewModels
{
    public class ChildInformationpageViewModel : BaseclassViewModel
    {
        private readonly IUserPermissionService userPermissionService;
        public IStudentTestFormsService studentTestFormsService;
        private readonly ICommonDataService commonDataService;
        private readonly ILocationService locationService;
        private readonly IClinicalTestFormService clinicalTestFormService;
        public ICommand HyperLinkClickedCommand { get; set; }
        public bool editIconClicked;
        private bool userTappedAddNewRecord;
        private readonly IStudentsService _studentService;
        List<ChildInformationRecord> _childInformationRecords = new List<ChildInformationRecord>();
        public List<ChildInformationRecord> ChildInformationRecords
        {
            get
            {
                return _childInformationRecords;
            }

            set
            {
                _childInformationRecords = value;
                Height = ChildInformationRecords.Count * 40;
                OnPropertyChanged(nameof(ChildInformationRecords));
                if (value != null && value.Any())
                {
                    foreach (var item in value)
                    {
                        item.ItemDelete = new Action<ChildInformationRecord>(async(record)=> 
                        {
                            UserDialogs.Instance.ShowLoading("Loading...");
                            await Task.Delay(300);
                            clinicalTestFormService.DeleteTestFormByLocalID(record.LocalTestInstance);
                            studentTestFormResponsesService.DeleteAll(record.LocalTestInstance);
                            studentTestFormsService.DeleteAll(record.LocalTestInstance);
                            LoadTestRecordsFromDB(OfflineStudentId);
                            UserDialogs.Instance.HideLoading();
                        });

                        item.ItemSync = new Action<ChildInformationRecord>(async(record) =>
                        {
                            isIndividual = true;
                            SelectedRecord = record;
                            var testRecords = studentTestFormsService.GetStudentTestRecords(record.LocalTestInstance + "");
                            if (testRecords != null && testRecords.Any())
                            {
                                var basalCeilingReached = testRecords.Where(p => p.LocalformInstanceId == record.LocalTestInstance).Any(p => !p.BaselCeilingReached && p.IsBaselCeilingApplied && p.TSOStatus != "Not Started");
                                if (basalCeilingReached)
                                {
                                    await PopupNavigation.Instance.PushAsync(new FormNoBasalCeiling("The test record you selected has a one or more subdomains with scores that do not meet basal and ceiling requirements. You will not receive scores for the subdomains that are not complete.") { CloseWhenBackgroundIsClicked = false, BindingContext = this });
                                }
                                else
                                {
                                    await PopupNavigation.Instance.PushAsync(new FormCommitPopUp() { CloseWhenBackgroundIsClicked = false, BindingContext = this });
                                }
                            }
                        });
                    }
                }
            }
        }

        private ChildInformationRecord SelectedRecord;
        private bool isIndividual = false;

        private bool isTableBottomLineVisible = true;
        public bool IsTableBottomLineVisible
        {
            get
            {
                return isTableBottomLineVisible;
            }
            set
            {
                isTableBottomLineVisible = value;
                OnPropertyChanged(nameof(IsTableBottomLineVisible));
            }
        }

        int height;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelectAll;
        public bool IsSelectAll
        {
            get
            {
                return _isSelectAll;
            }
            set
            {
                if (_isSelectAll != value)
                    UpdateRecords(value);
                if (value)
                {
                    if (ChildInformationRecords.Count > 0 && (ChildInformationRecords.Any(p => p.Status == "In-Progress") || ChildInformationRecords.Any(q => q.Status == "Saved")) && Connectivity.NetworkAccess == NetworkAccess.Internet)
                    {
                        _isSelectAll = EnableSync = value;
                    }
                    else
                    {
                        _isSelectAll = EnableSync = !value;
                    }
                }
                else 
                {
                    _isSelectAll = value;
                }
                OnPropertyChanged(nameof(IsSelectAll));
            }
        }
        private readonly ITokenService _tokenService;

        private string userFullName;
        public string UserFullName
        {
            get { return userFullName; }
            set
            {

                userFullName = value;
                OnPropertyChanged(nameof(UserFullName));
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

        private bool _IsStartEnabled;
        public bool IsStartEnabled
        {
            get { return _IsStartEnabled; }
            set
            {
                _IsStartEnabled = value;
                OnPropertyChanged(nameof(IsStartEnabled));
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

        private string currentTime = string.Format("{0}: {1} {2}", "Last Connection", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
        public string CurrentTime
        {
            get { return currentTime; }
            set
            {
                currentTime = value;
                OnPropertyChanged(nameof(CurrentTime));
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string gender;
        public string Gender
        {
            get { return gender; }
            set
            {
                gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private string dateofbirth;
        public string DateofBirth
        {
            get { return dateofbirth; }
            set
            {
                dateofbirth = value;
                OnPropertyChanged(nameof(DateofBirth));
            }
        }
        private string editIconImage = "iconeditwhite.png";
        public string EditIconImage
        {
            get
            {
                return editIconImage;
            }
            set
            {
                editIconImage = value;
                OnPropertyChanged(nameof(EditIconImage));
            }
        }
        private string plusIconImage = "iconpluswhite.png";
        public string PlusIconImage
        {
            get
            {
                return plusIconImage;
            }
            set
            {
                plusIconImage = value;
                OnPropertyChanged(nameof(PlusIconImage));
            }
        }
        private bool isenablepermission = false;
        public bool Isenablepermission
        {
            get
            {
                return isenablepermission;
            }
            set
            {
                isenablepermission = value;
                OnPropertyChanged(nameof(Isenablepermission));
            }
        }
        private bool enableSync;
        public bool EnableSync
        {
            get
            {
                return enableSync;
            }
            set
            {
                enableSync = value;
                OnPropertyChanged(nameof(EnableSync));
            }
        }

        
        
        private ChildInfoResponse childInformation;
        public ChildInfoResponse ChildInformation
        {
            get { return childInformation; }
            set { childInformation = value; OnPropertyChanged(nameof(childInformation)); }
        }
        public ICommand EditCommand { get; set; }
        public ICommand AddNewRecordCommand { get; set; }


        #region Adding for AssessmentConfigPopup
        private string fullName;
        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string dOB;
        public string DOB
        {
            get { return dOB; }
            set
            {
                dOB = value;
                OnPropertyChanged(nameof(DOB));
            }
        }

        private string testDate;
        public string TestDate
        {
            get { return testDate; }
            set
            {
                testDate = value;
                OnPropertyChanged(nameof(TestDate));
            }
        }
        #endregion

        private readonly IUsersService userservice = DependencyService.Get<IUsersService>();
        private readonly IStudentTestFormResponsesService studentTestFormResponsesService = DependencyService.Get<IStudentTestFormResponsesService>();
        private readonly IProductResearchCodeValuesService productResearchCodeValuesService;
        private readonly IProductResearchCodesService productResearchCodesService;
        public ChildInformationpageViewModel(int offlineStudentId)
        {
            productResearchCodeValuesService = DependencyService.Get<IProductResearchCodeValuesService>();
            productResearchCodesService = DependencyService.Get<IProductResearchCodesService>();

            userPermissionService = DependencyService.Get<IUserPermissionService>();
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            commonDataService = DependencyService.Get<ICommonDataService>();
            locationService = DependencyService.Get<ILocationService>();
            clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
            OfflineStudentId = offlineStudentId;
            EditCommand = new Command(EditClicked);
            AddNewRecordCommand = new Command(AddNew);
            HyperLinkClickedCommand = new Xamarin.Forms.Command<ChildInformationRecord>(HyperlinkClicked);

            _studentService = DependencyService.Get<IStudentsService>();
            _tokenService = DependencyService.Get<ITokenService>();
            var tokenModel = _tokenService.GetTokenResposne();
            if (tokenModel != null)
            {
                UserFullName = "" + tokenModel.FirstName + "  " + "" + tokenModel.LastName + "";
            }

            FailedString = "Failed: 0";
            PendingString = "Pending: 0";
            Task.Run(async () => { await LoadData(OfflineStudentId); });
            LoadTestRecordsFromDB(OfflineStudentId);

            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                foreach(var child in ChildInformationRecords)
                {
                    child.EnableRow = false;
                }
                EnableSync = false;
            }
            else
            {
                if (ChildInformationRecords.Any(p => p.Status != "Not started") && ChildInformationRecords.Any(p => p.IsSelect))
                    EnableSync = true;
            }
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            commonDataService.ClearAddChildContent = ResetContent;
        }
        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.None || e.NetworkAccess == NetworkAccess.Unknown )
            { 
                EnableSync = false;
                if(IsSelectAll)
                    IsSelectAll = false;
                foreach (var child in ChildInformationRecords)
                {
                        if(child.IsSelect)
                            child.IsSelect = false;
                        if (child.EnableRow)
                        child.EnableRow = false;
                }
             }
            else if (ChildInformationRecords != null && ChildInformationRecords.Any())
            {
                if(ChildInformationRecords.Any(p => p.IsSelect))
                    EnableSync = true;
                foreach (var child in ChildInformationRecords)
                {
                    if (child.Status != "Not started")
                    {
                        if (!child.EnableRow)
                        {
                            child.EnableRow = true;
                        }
                    }
                }
            }
        }

        private async void EditClicked()
        {
            if (editIconClicked)
                return;

            editIconClicked = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new Views.AddChildViews.ChildTabbedPage(OfflineStudentId));
            editIconClicked = false;
        }
        public int LocalTestFormID { get; set; }

        async void AddNew()
        {
            if (userTappedAddNewRecord)
                return;
            userTappedAddNewRecord = true;
            await PopupNavigation.Instance.PushAsync(new AssessmentConfigPopupView(OfflineStudentId));
            userTappedAddNewRecord = false;
        }

        public int OfflineStudentId { get; set; }
        private async Task LoadData(int offlinestudentid)
        {
            int addedBy = 0;
            if (Application.Current.Properties.ContainsKey("UserID"))
            {
                addedBy = Convert.ToInt32(Application.Current.Properties["UserID"].ToString());
            }
            var offlineData = _studentService.GetStudentById(offlinestudentid);
            if (offlineData != null)
            {
                Isenablepermission = await userPermissionService.GetStudentEditPermissionsAsync() || Application.Current.Properties["UserTypeID"].ToString() == "1" || Application.Current.Properties["UserTypeID"].ToString() == "6" || offlineData.AddedBy == addedBy;
                if (!Isenablepermission)
                {
                    EditIconImage = "icon_edit_gray.png";
                    PlusIconImage = "icon_plus_gray.png";
                }
                Name = offlineData.FirstName + " " + offlineData.LastName;
                var gender = GenderLookUp.GetGenders().FirstOrDefault(p => p.value == offlineData.Gender);
                Gender = gender?.text;
                var year = offlineData.Birthdate.Year;
                var month = offlineData.Birthdate.Month < 10 ? "0" + offlineData.Birthdate.Month : "" + offlineData.Birthdate.Month;
                var day = offlineData.Birthdate.Day < 10 ? "0" + offlineData.Birthdate.Day : offlineData.Birthdate.Day + "";
                DateofBirth = month + "/" + day + "/" + year;
                Number = offlineData.ChildID;
                Location = "-";
                if (offlineData.SelectedLocationId != null)
                {
                    var location = locationService.GetLocationById(offlineData.SelectedLocationId.Value);
                    Location = location?.LocationName ?? "-";
                }
            }
        }
        private void UpdateRecords(bool value)
        {
            foreach (var child in ChildInformationRecords)
            {
                if (child.Status != "Not started" && Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
                {
                    child.IsSelect = value;
                    child.EnableRow = true;
                }
                else
                {
                    child.IsSelect = false;
                    child.EnableRow = false;
                }
            }
               
            }

        public void CheckBoxChanged(bool isChecked)
        {
            var count = ChildInformationRecords.Count(p => p.IsSelect);
            if (count > 0)
                EnableSync = true;
            else
                EnableSync  = false;

            var sortedRecordCount = ChildInformationRecords.Count(p => p.Status == "Saved") + ChildInformationRecords.Count(p => p.Status == "In-Progress");
            if (count == 1 && sortedRecordCount == 1)
                _isSelectAll = true;
            else if (count == sortedRecordCount)
                _isSelectAll = true;
            else if (count == 0 && sortedRecordCount >= 1)
                _isSelectAll = false;
            else if (count <= 0)
                _isSelectAll = false;
            else
                _isSelectAll = false;
            //_isSelectAll = count == ChildInformationRecords.Count;
            OnPropertyChanged(nameof(IsSelectAll));
        }
      
        private async void HyperlinkClicked(ChildInformationRecord child)
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            await Task.Delay(200);
            LocalTestFormID = child.LocalTestInstance;
            this.FullName = this.Name;
            this.DOB = this.DateofBirth;
            commonDataService.DOB = this.DateofBirth;
            this.TestDate = child.InitialTestDate;
            commonDataService.StudentTestFormOverview = null;
            commonDataService.StudentTestForms = studentTestFormsService.GetStudentTestForms(LocalTestFormID);
            commonDataService.StudentTestFormOverview = clinicalTestFormService.GetStudentTestFormsByID(LocalTestFormID);
            commonDataService.LocaInstanceID = LocalTestFormID;
            commonDataService.IsCompleteForm = child.AssesmentId == AssignmentTypes.BattelleDevelopmentalCompleteID;
            commonDataService.IsAcademicForm = child.AssesmentId == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID;
            commonDataService.IsScreenerForm = child.AssesmentId == AssignmentTypes.BattelleDevelopmentalScreenerID;
            var navigationParams = new AdminstrationNavigationParams
            {
                LocalInstanceID = LocalTestFormID,
                DOB = DOB,
                TestDate = TestDate,
                FullName = FullName,
                OfflineStudentID = OfflineStudentId
            };
            if (child.AssesmentId == AssignmentTypes.BattelleDevelopmentalCompleteID)
            {
                navigationParams.IsDevelopmentCompleteForm = true;
            }
            if (child.AssesmentId == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID)
                await Application.Current.MainPage.Navigation.PushModalAsync(new AcademicformWithMatAndItems(navigationParams));
            else
                await Application.Current.MainPage.Navigation.PushModalAsync(new AdministrationView(navigationParams));
            ResetContent?.Invoke();
        }

        public void LoadTestRecordsFromDB(int offlineStudentId)
        {
            var studentTestRecordForms = new List<ChildInformationRecord>();

            var testRecords = (clinicalTestFormService.GetStudentTestFormsByStudentID(offlineStudentId));
            var recordForm = "";
            foreach (var testRecord in testRecords)
            {
                var obj = testRecord.formParameters != null ? JObject.Parse(testRecord.formParameters) : null;
                if (obj != null)
                {
                    var dateOfTesting = (DateTime)obj.SelectToken("TestDate");

                    if (testRecord.assessmentId == AssignmentTypes.BattelleDevelopmentalCompleteID)
                        recordForm = AssignmentTypes.BattelleDevelopmentalCompleteString;
                    else if (testRecord.assessmentId == AssignmentTypes.BattelleDevelopmentalScreenerID)
                        recordForm = AssignmentTypes.BattelleDevelopmentScreenerString;
                    else
                        recordForm = AssignmentTypes.BattelleEarlyAcademicSurveyString;


                    var formParametersObj = JsonConvert.DeserializeObject<FormParamterClass>(testRecord.formParameters);
                    studentTestRecordForms.Add(new ChildInformationRecord()
                    {
                        Status = testRecord.FormStatus,
                        AssesmentId = testRecord.assessmentId,
                        LocalTestInstance = testRecord.LocalTestRecodId,
                        RecordForm = recordForm,
                        InitialTestDate = dateOfTesting.Date.ToString("MM/dd/yyyy"),
                        SyncStatus = testRecord.SyncStausDesc,
                        StatusCode = testRecord.SyncStausCode,
                        EnableRow = ((testRecord.FormStatus == "Not started" && Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet) || (testRecord.FormStatus == "Saved" && Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet) || (testRecord.FormStatus == "In-Progress" && Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet) ? false : true)
                    });
                }
            }
            ChildInformationRecords.Clear();
            IsSelectAll = false;
            ChildInformationRecords = studentTestRecordForms;
            IsTableBottomLineVisible = ChildInformationRecords.Any();
        }

        public Action ResetContent { get; set; }

        #region Sync
        public ICommand DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var selectedRecords = ChildInformationRecords.Where(p => p.IsSelect);
                    if (selectedRecords != null && selectedRecords.Any())
                    {
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(300);
                        foreach (var item in selectedRecords)
                        {
                            clinicalTestFormService.DeleteTestFormByLocalID(item.LocalTestInstance);
                            studentTestFormResponsesService.DeleteAll(item.LocalTestInstance);
                            studentTestFormsService.DeleteAll(item.LocalTestInstance);
                        }
                        LoadTestRecordsFromDB(OfflineStudentId);
                        UserDialogs.Instance.HideLoading();
                    }
                });
            }
        }
        public ICommand SyncCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var basalCeilingPopup = false;
                    isIndividual = false;
                    SelectedRecord = null;
                    var selectedRecords = ChildInformationRecords.Where(p => p.IsSelect);
                    if (selectedRecords != null && selectedRecords.Any())
                    {
                        var testRecords = studentTestFormsService.GetStudentTestRecords(string.Join(",", selectedRecords.Select(p => p.LocalTestInstance)));
                        if (testRecords != null && testRecords.Any())
                        {
                            foreach (var item in selectedRecords)
                            {
                                var basalCeilingReached = testRecords.Where(p => p.LocalformInstanceId == item.LocalTestInstance).Any(p => !p.BaselCeilingReached && p.IsBaselCeilingApplied && p.TSOStatus != "Not Started");
                                if (basalCeilingReached)
                                {
                                    basalCeilingPopup = true;
                                    break;
                                }
                            }
                            if (basalCeilingPopup)
                            {
                                await PopupNavigation.Instance.PushAsync(new FormNoBasalCeiling(selectedRecords.Count() == 1 ? "The test record you selected has a one or more subdomains with scores that do not meet basal and ceiling requirements. You will not receive scores for the subdomains that are not complete." : "One or more of the test records you selected has  one or more subdomains with scores that do not meet basal and ceiling requirements. You will not receive scores for the subdomains that are not complete.") { CloseWhenBackgroundIsClicked = false, BindingContext = this });
                            }
                            else
                            {
                                await PopupNavigation.Instance.PushAsync(new FormCommitPopUp() { CloseWhenBackgroundIsClicked = false, BindingContext = this });
                            }
                        }
                    }
                });
            }
        }
        public Command CancelCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                });
            }
        }
        public Command CommitContinue
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        await CommitTestRecord();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                        if (ex != null)
                        {
                            await PopupNavigation.Instance.PushAsync(new FailuretoCommitInternet() { BindingContext = this, CloseWhenBackgroundIsClicked = false });
                        }
                    }

                });
            }
        }

        public async void ShowError(string errormessage)
        {
            LoadTestRecordsFromDB(OfflineStudentId);
            UserDialogs.Instance.HideLoading();
            if (string.IsNullOrEmpty(errormessage) && Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                await PopupNavigation.Instance.PushAsync(new FailuretoCommitInternet() { BindingContext = this, CloseWhenBackgroundIsClicked = false });
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new GenericFailuretoCommitView() { BindingContext = new GenericFailuretoCommitViewModel() { ErrorMessage = errormessage } });
            }
        }

        public async Task CommitTestRecord()
        {
            await PopupNavigation.Instance.PopAllAsync();
            UserDialogs.Instance.ShowLoading("Loading...");
            await Task.Delay(300);
            var username = Convert.ToString(Application.Current.Properties["UserName"]);
            var password = Convert.ToString(Application.Current.Properties["PassID"]);
            try
            {
                var response = await new BDIWebServices().LoginUser(new { username = username.Trim(), password = password.Trim() });
                if (response != null)
                {
                    if (!string.IsNullOrEmpty(response.StatusCode))
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Sync Failed!");
                        return;
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
                    await UserDialogs.Instance.AlertAsync("Sync Failed!");
                }
                return;
            }
            UserDialogs.Instance.HideLoading();
            UserDialogs.Instance.ShowLoading("Syncing Data to server...");
            var lstStudents = new List<Students>();

            var selectedRecords = new List<ChildInformationRecord>();
            if (!isIndividual)
            {
                selectedRecords = ChildInformationRecords.Where(p => p.IsSelect).ToList();
            }
            else
            {
                selectedRecords = new List<ChildInformationRecord>();
                selectedRecords.Add(SelectedRecord);
            }
                
            if (selectedRecords != null && selectedRecords.Any())
            {
                var groupedRecords = selectedRecords;
                if (groupedRecords != null && groupedRecords.Any())
                {
                    var lstSync = new List<SyncChild>();
                    var orgID = Convert.ToInt32(Application.Current.Properties["OrgnazationID"]);
                    var researchCodes = productResearchCodesService.GetResearchCodesByOrg(orgID);
                    var localStudent = _studentService.GetStudentById(OfflineStudentId);
                    var researchCodesValues = productResearchCodeValuesService.GetProductResearchCodes(OfflineStudentId);
                    var syncChild = new SyncChild();
                    int addedBy;
                    var isSuccess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
                    if (localStudent != null)
                    {
                        lstStudents.Add(localStudent);
                        localStudent.LocalTestFormIds = string.Join(",", selectedRecords.ToList().Select(p => p.LocalTestInstance));
                        var data = localStudent;
                        var userRecord = userservice.GetUserByID(addedBy);
                        syncChild.DeviceId = userRecord.DeviceId;
                        lstSync.Add(syncChild);
                        syncChild.SkipUpdate = localStudent.IsSynced;
                        syncChild.LocalChildUserId = data.OfflineStudentID;
                        syncChild.ChildId = data.ChildID;
                        if (!string.IsNullOrEmpty(data.UserId))
                        {
                            syncChild.UserId = Convert.ToInt32(data.UserId);
                        }
                        syncChild.OrganizationId = orgID;
                        syncChild.DateOfBirth = data.Birthdate.Month + "/" + data.Birthdate.Day + "/" + data.Birthdate.Year;
                        syncChild.EnrollmentDate = data.EnrollmentDate == DateTime.MinValue ? null : data.EnrollmentDate.Month + "/" + data.EnrollmentDate.Day + "/" + data.EnrollmentDate.Year;
                        if (!string.IsNullOrEmpty(data.SelectedEthnictyIds))
                        {
                            syncChild.EthnicityId = Convert.ToInt32(data.SelectedEthnictyIds);
                        }
                        syncChild.FirstName = data.FirstName;
                        syncChild.LastName = data.LastName;
                        syncChild.MiddleName = data.MiddleName;
                        syncChild.ParentGuardianEmail1 = data.ParentEmailAddress1;
                        syncChild.ParentGuardianEmail2 = data.ParentEmailAddress2;
                        syncChild.ParentGuardianName1 = data.ParentGuardian1;
                        syncChild.ParentGuardianName2 = data.ParentGuardian2;
                        if (!string.IsNullOrEmpty(data.SelectedLanguageIds))
                        {
                            syncChild.PrimaryLanguageId = Convert.ToInt32(data.SelectedLanguageIds);
                        }
                        if (!string.IsNullOrEmpty(data.SelectedRaceIds))
                        {
                            syncChild.RaceIds = data.SelectedRaceIds.Split(',').Select(p => Convert.ToInt32(p)).ToList();
                        }
                        syncChild.FreeLunch = Convert.ToBoolean(data.IsFreeLunch);
                        if (!string.IsNullOrEmpty(data.SelectedFundingSourceIds))
                        {
                            syncChild.FundingResourceIds = data.SelectedFundingSourceIds.Split(',').Select(p => Convert.ToInt32(p)).ToList();
                        }
                        syncChild.GenderId = data.Gender;
                        syncChild.Iep = Convert.ToBoolean(data.IsIEP);
                        syncChild.Ifsp = Convert.ToBoolean(data.IsIFSP);
                        syncChild.LocationId = data.SelectedLocationId.Value;
                        syncChild.CreatedDate = data.updatedOn;
                        syncChild.researchCodeNames = new List<SyncResearchCodeName>();
                        syncChild.ResearchCodeValues = new List<SyncResearchCodeValue>();
                        if (Convert.ToBoolean(data.IsIEP))
                        {
                            syncChild.IepEligibilityDate = data.IEPEligibilityDate == DateTime.MinValue ? null : data.IEPEligibilityDate.Month + "/" + data.IEPEligibilityDate.Day + "/" + data.IEPEligibilityDate.Year;
                            syncChild.IepExitDate = data.IEPExitDate == DateTime.MinValue ? null : data.IEPExitDate.Month + "/" + data.IEPExitDate.Day + "/" + data.IEPExitDate.Year;
                        }
                        if (Convert.ToBoolean(data.IsIFSP))
                        {
                            syncChild.IfspEligibilityDate = data.IFSPEligibilityDate == DateTime.MinValue ? null : data.IFSPEligibilityDate.Month + "/" + data.IFSPEligibilityDate.Day + "/" + data.IFSPEligibilityDate.Year;
                            syncChild.IfspExitDate = data.IFSPExitDate == DateTime.MinValue ? null : data.IFSPExitDate.Month + "/" + data.IFSPExitDate.Day + "/" + data.IFSPExitDate.Year;
                        }

                        if (Convert.ToBoolean(data.IsIEP) || Convert.ToBoolean(data.IsIFSP))
                        {
                            if (!string.IsNullOrEmpty(data.SelectedPrimaryDiagnosesIds))
                            {
                                syncChild.PrimaryDiagnosesId = Convert.ToInt32(data.SelectedPrimaryDiagnosesIds);
                            }
                            if (!string.IsNullOrEmpty(data.SelectedSecondaryDiagnosesIds))
                            {
                                syncChild.SecondaryDiagnosesId = Convert.ToInt32(data.SelectedSecondaryDiagnosesIds);
                            }
                        }

                        foreach (var codes in researchCodes)
                        {
                            syncChild.researchCodeNames.Add(new SyncResearchCodeName()
                            {
                                Name = codes.ValueName,
                                ResearchCodeId = codes.ResearchCodeId
                            });
                        }

                        foreach (var codevalues in researchCodesValues)
                        {
                            syncChild.ResearchCodeValues.Add(new SyncResearchCodeValue()
                            {
                                ResearchCodeId = codevalues.ResearchCodeId,
                                ResearchCodeValueId = codevalues.ResearchCodeValueId,
                                Value = codevalues.value
                            });
                        }
                    }
                    syncChild.TestRecords = new List<SyncTestRecord>();
                    foreach (var inneritem in selectedRecords)
                    {   
                        var testRecord = clinicalTestFormService.GetStudentTestFormsByID(inneritem.LocalTestInstance);
                        if (testRecord != null)
                        {
                            var newSyncTestRecord = new SyncTestRecord();
                            syncChild.TestRecords.Add(newSyncTestRecord);
                            newSyncTestRecord.AdditionalNotes = testRecord.additionalNotes;
                            newSyncTestRecord.Notes = testRecord.notes;
                            newSyncTestRecord.AssessmentId = testRecord.assessmentId;
                            newSyncTestRecord.CreateDate = Convert.ToString(testRecord.createDate.Value);
                            newSyncTestRecord.CreatedByUserId = addedBy;
                            newSyncTestRecord.FormParameters = testRecord.formParameters;
                            newSyncTestRecord.LocalFormInstanceId = testRecord.LocalTestRecodId;
                            newSyncTestRecord.ContentCategories = new List<SyncContentCategory>();

                            var records = studentTestFormsService.GetStudentTestForms(testRecord.LocalTestRecodId);
                            if (records != null && records.Any())
                            {
                                foreach (var record in records)
                                {
                                    var newContentCatgory = new SyncContentCategory();
                                    newContentCatgory.ContentCategoryId = record.contentCategoryId;
                                    newContentCatgory.CreateDate = Convert.ToString(record.createDate.Value);
                                    newContentCatgory.ExaminerId = record.examinerId.Value;
                                    newContentCatgory.Order = commonDataService.TotalCategories.FirstOrDefault(p => p.contentCategoryId == record.contentCategoryId).sequenceNo;
                                    newContentCatgory.Notes = record.Notes;
                                    newContentCatgory.TimeTaken = record.TimeTaken;
                                    newContentCatgory.RawScore = record.rawScore;
                                    newContentCatgory.ItemScore = record.rawScoreEnabled ? 0 : 1;
                                    newContentCatgory.Status = record.TSOStatus == "Saved" ? 1 : 0;
                                    newContentCatgory.AgeInMonths = CalculateAgeDiff(lstStudents.FirstOrDefault().Birthdate);
                                    newContentCatgory.TestDate = record.testDate;
                                    var testformResponse = studentTestFormResponsesService.GetStudentTestFormResponses(testRecord.LocalTestRecodId);
                                    var itemResponse = testformResponse.FirstOrDefault(p => p.ContentCategoryId == record.contentCategoryId);
                                    if (itemResponse != null)
                                    {
                                        newContentCatgory.ItemLevelResponse = new SyncItemLevelResponse();
                                        newContentCatgory.ItemLevelResponse.CreatedBy = addedBy;
                                        newContentCatgory.ItemLevelResponse.SectionId = JsonConvert.DeserializeObject<FormJsonClass>(newSyncTestRecord.FormParameters).sectionId;
                                        newContentCatgory.ItemLevelResponse.Response = itemResponse.Response;
                                        newContentCatgory.ItemLevelResponse.CreatedOn = Convert.ToString(itemResponse.CreatedOn.Value);
                                        newSyncTestRecord.ContentCategories.Add(newContentCatgory);
                                    }
                                }
                            }
                        }
                    }
                    if (lstSync != null && lstSync.Any())
                    {
                        var response = await new BDIWebServices().SyncTestRecords(lstSync, ShowError);
                        if (response != null && response.Any())
                        {
                            foreach (var item in response)
                            {
                                if (item.StatusCode == SyncTestRecordStatusCode.Success)
                                {
                                    clinicalTestFormService.DeleteTestFormByLocalID(item.LocalFormInstanceId);
                                    studentTestFormsService.DeleteAll(item.LocalFormInstanceId);
                                    studentTestFormResponsesService.DeleteAll(item.LocalFormInstanceId);
                                    if (lstStudents.Any(p => p.LocalTestFormIds.Split(',').Contains(item.LocalFormInstanceId + "")))
                                    {
                                        var student = lstStudents.FirstOrDefault(p => p.LocalTestFormIds.Split(',').Contains(item.LocalFormInstanceId + ""));
                                        if (student != null && string.IsNullOrEmpty(student.UserId))
                                        {
                                            student.UserId = item.ChildUserId.Value.ToString();
                                            student.updatedOn = DateTime.Now.ToUniversalTime().ToString();
                                            student.updatedOnUTC = DateTime.Now.ToUniversalTime().ToString();
                                            _studentService.Update(student);
                                        }
                                    }
                                    if (item.ResearchCodeValues != null && item.ResearchCodeValues.Any())
                                    {
                                        var student = lstStudents.FirstOrDefault(p => p.LocalTestFormIds.Split(',').Contains(item.LocalFormInstanceId + ""));
                                        var localCodeValues = productResearchCodeValuesService.GetProductResearchCodes(student.OfflineStudentID);
                                        var lstResearchCodeValues = new List<ProductResearchCodeValues>();
                                        foreach (var innerItem in localCodeValues)
                                        {
                                            innerItem.ResearchCodeValueId = item.ResearchCodeValues.FirstOrDefault(p => p.ResearchCodeId == innerItem.ResearchCodeId).ResearchCodeValueId;
                                        }
                                        productResearchCodeValuesService.DeleteByStudentId(item.LocalChildUserId);
                                        productResearchCodeValuesService.InsertAll(localCodeValues);
                                    }
                                }
                                else
                                {
                                    if (lstStudents.Any(p => p.LocalTestFormIds.Split(',').Contains(item.LocalFormInstanceId + "")))
                                    {
                                        var student = lstStudents.FirstOrDefault(p => p.LocalTestFormIds.Split(',').Contains(item.LocalFormInstanceId + ""));
                                        if (item.ChildUserId.HasValue)
                                        {
                                            if (student != null && string.IsNullOrEmpty(student.UserId))
                                            {
                                                student.UserId = item.ChildUserId.Value.ToString();
                                                student.updatedOn = DateTime.Now.ToUniversalTime().ToString();
                                                student.updatedOnUTC = DateTime.Now.ToUniversalTime().ToString();
                                                _studentService.Update(student);
                                            }
                                        }
                                    }
                                    if (item.ResearchCodeValues != null && item.ResearchCodeValues.Any() && item.ChildUserId.HasValue)
                                    {
                                        var student = lstStudents.FirstOrDefault(p => p.LocalTestFormIds.Split(',').Contains(item.LocalFormInstanceId + ""));
                                        var localCodeValues = productResearchCodeValuesService.GetProductResearchCodes(student.OfflineStudentID);
                                        var lstResearchCodeValues = new List<ProductResearchCodeValues>();
                                        foreach (var innerItem in localCodeValues)
                                        {
                                            innerItem.ResearchCodeValueId = item.ResearchCodeValues.FirstOrDefault(p => p.ResearchCodeId == innerItem.ResearchCodeId).ResearchCodeValueId;
                                        }
                                        productResearchCodeValuesService.DeleteByStudentId(item.LocalChildUserId);
                                        productResearchCodeValuesService.InsertAll(localCodeValues);
                                    }
                                    clinicalTestFormService.UpdateSyncStatus((int)item.StatusCode, item.LocalFormInstanceId);
                                }
                            }
                            LoadTestRecordsFromDB(OfflineStudentId);
                            UserDialogs.Instance.HideLoading();
                        }
                    }
                }
            }
        }
        private int CalculateAgeDiff(DateTime DOB)
        {
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(Convert.ToDateTime(DOB)).Ticks).Year - 1;
            DateTime PastYearDate = Convert.ToDateTime(DOB).AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            return Months + (Years * 12);
        }
        #endregion
    }
}
