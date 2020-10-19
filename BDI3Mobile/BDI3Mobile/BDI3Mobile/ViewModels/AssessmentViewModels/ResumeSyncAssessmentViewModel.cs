using Acr.UserDialogs;
using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Models.SyncModels;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Views.AcademicSurvey;
using BDI3Mobile.Views.PopupViews;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AssessmentViewModels
{
    public class ResumeSyncAssessmentViewModel : BindableObject
    {
        private bool cover = false;
        public ICommand TestRecordNavigCommand { get; set; }
        public ICommand UpdateListviewBoundCommand { get; set; }
        public ICommand StatusSelectionCommand => new Command(StatusSelected);
        public ICommand UnCoverCommand => new Command(UnCover);
        List<ChildAssessmentRecord> _childAssessmentsRecords = new List<ChildAssessmentRecord>();
        public List<ChildAssessmentRecord> ChildAssessmentRecords
        {
            get
            {
                return _childAssessmentsRecords;
            }

            set
            {
                _childAssessmentsRecords = value;
                OnPropertyChanged(nameof(ChildAssessmentRecords));
            }
        }

        public bool Cover
        {
            get => cover;
            set
            {
                cover = value;
                OnPropertyChanged(nameof(Cover));
            }
        }

        public string TestDate { get; set; }
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string _testName;
        public string TestName
        {
            get { return _testName; }
            set
            {
                _testName = value;
                OnPropertyChanged(nameof(TestName));
            }
        }
        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private Color _searchErrorColor;
        public Color SearchErrorColor
        {
            get
            {
                return _searchErrorColor;
            }
            set
            {
                _searchErrorColor = value;
                OnPropertyChanged(nameof(SearchErrorColor));

            }

        }
        private bool _searchErrorVisible;
        public bool SearchErrorVisible
        {
            get { return _searchErrorVisible; }
            set
            {
                _searchErrorVisible = value;
                OnPropertyChanged(nameof(SearchErrorVisible));
            }
        }
        private string _searchResult;
        public string SearchResult
        {
            get { return _searchResult; }
            set
            {
                _searchResult = value;
                OnPropertyChanged(nameof(SearchResult));
            }
        }

        private bool testDateIsValid;
        public bool TestDateIsValid
        {
            get
            {
                return testDateIsValid;
            }
            set
            {
                testDateIsValid = value;
                OnPropertyChanged("TestDateIsValid");
            }
        }
        bool _selectAll;
        public bool SelectAll
        {
            get
            {
                return _selectAll;
            }
            set
            {
               if (_selectAll != value)
               {
                   UpdateChildRecords(value);
               }
               if(value)
                {
                    if (ChildAssessmentRecords.Count > 0)
                        _selectAll = value;
                    else
                        _selectAll = !value;
                }
               else
                {
                    _selectAll = value;
                }
               OnPropertyChanged(nameof(SelectAll)); 
            }
        }
        bool _showTestSelection;
        public bool ShowTestSelection
        {
            get
            {
                return _showTestSelection;
            }
            set
            {
                _showTestSelection = value;
                Cover = value;
                OnPropertyChanged(nameof(ShowTestSelection));
            }
        }
        public List<TestRecordStatus> TestRecordStatusList { get; set; }

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
        public int LocalTestFormID { get; set; }

        #region CallService
        private readonly IUsersService userservice = DependencyService.Get<IUsersService>();
        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
        private readonly IProductResearchCodeValuesService productResearchCodeValuesService;
        private readonly IProductResearchCodesService productResearchCodesService;
        private readonly IStudentsService _studentService;
        private readonly IClinicalTestFormService clinicalTestFormService;
        private readonly ILocationService _locationService;
        private readonly IUserPermissionService userPermissionService;
        private readonly IStudentTestFormsService studentTestFormsService;
        private readonly IStudentTestFormResponsesService studentTestFormResponsesService = DependencyService.Get<IStudentTestFormResponsesService>();
        #endregion
        public ResumeSyncAssessmentViewModel()
        {
            productResearchCodeValuesService = DependencyService.Get<IProductResearchCodeValuesService>();
            productResearchCodesService = DependencyService.Get<IProductResearchCodesService>();
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            _locationService = DependencyService.Get<ILocationService>();
            _studentService = DependencyService.Get<IStudentsService>();
            userPermissionService = DependencyService.Get<IUserPermissionService>();
            clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
            UpdateListviewBoundCommand = new Command<double>(UpdateChildRecordTableBounds);
            LoadAssessmentFromLocalDB();
            ShowTestSelection = false;

            commonDataService.ClearAddChildContent = ResetContent;

            List<TestRecordStatus> StatusList = new List<TestRecordStatus>()
            {
                new TestRecordStatus() { StatusText = "In-Progress", Selected = false },
                new TestRecordStatus() { StatusText = "Saved", Selected = false },
            };
            TestRecordStatusList = StatusList;

            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            TestRecordNavigCommand = new Command<ChildAssessmentRecord>(TestNavig);
        }

        public Action ResetContent { get; set; }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.None || e.NetworkAccess == NetworkAccess.Unknown || (ChildAssessmentRecords == null || !ChildAssessmentRecords.Any() || !ChildAssessmentRecords.Any(p => p.IsSelected)))
            {
                EnableSync = false;
            }
            else if (ChildAssessmentRecords != null && ChildAssessmentRecords.Any() && ChildAssessmentRecords.Any(p => p.IsSelected))
            {
                EnableSync = true;
            }
        }

        void UnCover()
        {
            ShowTestSelection = false;
        }

        private async void TestNavig(ChildAssessmentRecord child)
        {
            ShowTestSelection = false;
            UserDialogs.Instance.ShowLoading("Loading...");
            await Task.Delay(200);
            //UserDialogs.Instance.HideLoading();
            LocalTestFormID = child.LocalTestInstance;
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
                DOB = child.DOB,
                TestDate = TestDate,
                FullName = child.FullName,
                OfflineStudentID = child.OfflineStudentID,
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

        private async void LoadAssessmentFromLocalDB()
        {
            IsTableBottomLineVisible = true;
            ChildAssessmentRecords = new List<ChildAssessmentRecord>();
            SelectAll = false;
            int addedBy;
            var initialChildRecords = new List<ChildRecordsNewAssessment>();
            if (Application.Current.Properties.ContainsKey("UserID"))
            {
                var isSuccess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
                if (isSuccess)
                {
                    var childRecords = (_studentService.GetStudentsByOfflineID(addedBy));
                    var hasPermission = await userPermissionService.GetStudentEditPermissionsAsync() || Application.Current.Properties["UserTypeID"].ToString() == "1" || Application.Current.Properties["UserTypeID"].ToString() == "6";
                    if (childRecords != null)
                    {
                        foreach (var childRecord in childRecords)
                        {
                            initialChildRecords.Add(new ChildRecordsNewAssessment()
                            {
                                IsChildEditEnable = hasPermission || childRecord.AddedBy == addedBy,
                                AddRfIconImage = hasPermission || childRecord.AddedBy == addedBy ? "icon_add_record.png" : "icon_add_record_gray.png",
                                OfflineStudentId = childRecord.OfflineStudentID,
                                FirstName = childRecord.FirstName,
                                LastName = childRecord.LastName,
                                ChildID = childRecord.ChildID,
                                ChildUserID = childRecord.UserId,
                                LocationID = (childRecord.SelectedLocationId == null) ? 0 : Convert.ToInt32(childRecord.SelectedLocationId),
                                DOB = childRecord.Birthdate.ToString("MM/dd/yyyy"),
                                Enrollment = childRecord.EnrollmentDate.Date != DateTime.MinValue.Date ? childRecord.EnrollmentDate.ToString("MM/dd/yyyy") : "",
                            });
                        }
                        var list = initialChildRecords.OrderBy(a => a.LastName);
                        initialChildRecords = new List<ChildRecordsNewAssessment>(list);
                    }
                }
            }


            var studentTestRecordForms = new List<ChildAssessmentRecord>();

            if (initialChildRecords != null)
            {
                foreach (var rec in initialChildRecords)
                {
                    var testRecords = (clinicalTestFormService.GetStudentTestFormsByStudentID(rec.OfflineStudentId).Where(s => s.FormStatus != "Not started").ToList());
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
                            studentTestRecordForms.Add(new ChildAssessmentRecord()
                            {
                                OfflineStudentID = rec.OfflineStudentId,
                                ChildFirstName = rec.FirstName,
                                ChildLastName = rec.LastName,
                                Status = testRecord.FormStatus,
                                TestDate = formParametersObj.TestDate.Value.ToString("MM/dd/yyyy"),
                                DOB = rec.DOB,
                                FullName = rec.FirstName + " " + rec.LastName,
                                AssessmentName = recordForm,
                                AssesmentId = testRecord.assessmentId,
                                LocalTestInstance = testRecord.LocalTestRecodId,
                                RecordForm = recordForm,
                                SyncStatus = testRecord.SyncStausDesc,
                                StatusCode = testRecord.SyncStausCode,
                                InitialTestDate = dateOfTesting.Date.ToString("d")
                            });
                        }
                    }
                }
                ChildAssessmentRecords.Clear();
                if (studentTestRecordForms != null && studentTestRecordForms.Any())
                {
                    if (!string.IsNullOrEmpty(FirstName))
                        studentTestRecordForms = new List<ChildAssessmentRecord>((studentTestRecordForms.Where(p => p.ChildFirstName.ToLower().Contains(FirstName.ToLower()))));
                    if (!string.IsNullOrEmpty(LastName))
                        studentTestRecordForms = new List<ChildAssessmentRecord>(studentTestRecordForms.Where(p => p.ChildLastName.ToLower().Contains(LastName.ToLower())));
                    if (!string.IsNullOrEmpty(TestName))
                        studentTestRecordForms = new List<ChildAssessmentRecord>(studentTestRecordForms.Where(p => p.AssessmentName.ToLower().Contains(TestName.ToLower())));
                    if (!string.IsNullOrEmpty(Status))
                        studentTestRecordForms = new List<ChildAssessmentRecord>(studentTestRecordForms.Where(p => p.Status.ToLower().Contains(Status.ToLower())));
                    if (!string.IsNullOrEmpty(TestDate))
                        studentTestRecordForms = new List<ChildAssessmentRecord>(studentTestRecordForms.Where(p => p.TestDate == TestDate));
                }
                ChildAssessmentRecords = studentTestRecordForms;
            }

            if (!ChildAssessmentRecords.Any())
            {
                SearchResult = ErrorMessages.RecordMatchesFoundMessage;
                SearchErrorColor = Color.Red;
                SearchErrorVisible = true;
            }
            else
            {

                SearchResult = string.Format(ErrorMessages.RecordsFoundMessage, ChildAssessmentRecords.Count(), ChildAssessmentRecords.Count() == 1 ? "Match" : "Matches");
                SearchErrorColor = Color.Black;
                SearchErrorVisible = true;
            }

            if (ChildAssessmentRecords.Any())
            {
                if (ChildAssessmentRecords.Count > 5)
                {
                    ChildAssessmentRecords[ChildAssessmentRecords.Count - 1].IsTableSepartorVisble = false;
                }
                else
                {
                    ChildAssessmentRecords[ChildAssessmentRecords.Count - 1].IsTableSepartorVisble = true;
                    IsTableBottomLineVisible = false;
                }
            }
            else
            {
                IsTableBottomLineVisible = false;
            }

            if(string.IsNullOrEmpty(TestName) && string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) && string.IsNullOrEmpty(Status) && string.IsNullOrEmpty(TestDate) && !ChildAssessmentRecords.Any())
            {
                SearchErrorVisible = false;
            }

        }

        public void Search(string dob)
        {
            TestDate = dob;
            LoadAssessmentFromLocalDB();
        }
        public void CheckBoxChanged()
        {
            EnableSync = false;
            if (ChildAssessmentRecords != null && ChildAssessmentRecords.Any())
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet && (ChildAssessmentRecords.Any(p => p.IsSelected)))
                {
                    EnableSync = true;
                }
            }
            _selectAll = false;
            if (ChildAssessmentRecords != null && ChildAssessmentRecords.Any())
            {
                var selectedRecords = ChildAssessmentRecords.Where(p => p.IsSelected);
                if (selectedRecords.Count() == ChildAssessmentRecords.Count())
                {
                    _selectAll = true;
                }
            }
            OnPropertyChanged(nameof(SelectAll));

        }
        private void UpdateChildRecords(bool value)
        {
            if (ChildAssessmentRecords != null && ChildAssessmentRecords.Any())
            {
                foreach (var childTestRecord in ChildAssessmentRecords)
                {
                    childTestRecord.IsSelected = value;
                }
                CheckBoxChanged();
            }
            else
            {
                EnableSync = false;
            }
        }
        private void UpdateChildRecordTableBounds(double listviewheight)
        {
            double rowHeight = 0.0;
            double totalRowHeight = 0.0;
            if (ChildAssessmentRecords.Any())
            {
                rowHeight = ChildAssessmentRecords.FirstOrDefault().RowHeight;
                totalRowHeight = ChildAssessmentRecords.Count() * rowHeight;
            }
            if (totalRowHeight > listviewheight)
            {
                IsTableBottomLineVisible = true;
            }
            else if (ChildAssessmentRecords.Count > 0)
            {
                ChildAssessmentRecords[ChildAssessmentRecords.Count - 1].IsTableSepartorVisble = true;
                IsTableBottomLineVisible = false;
            }
        }
        void StatusSelected(object obj)
        {
            var status = obj as TestRecordStatus;
            foreach (var item in TestRecordStatusList)
            {
                if (item.StatusText == status.StatusText)
                {
                    if (item.Selected)
                    {
                        item.Selected = !item.Selected;
                        Status = "";
                    }
                    else
                    {
                        item.Selected = !item.Selected;
                        Status = item.StatusText;
                    }
                }
                else
                {
                    item.Selected = false;
                }
            }
        }

        #region Sync
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
        public ICommand DeleteCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var selectedRecords = ChildAssessmentRecords.Where(p => p.IsSelected);
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
                        LoadAssessmentFromLocalDB();
                        UserDialogs.Instance.HideLoading();
                        if (ChildAssessmentRecords.Count == 0)
                            _selectAll = false;
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
                    var selectedRecords = ChildAssessmentRecords.Where(p => p.IsSelected);
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
                    if (ChildAssessmentRecords.Count == 0)
                        _selectAll = false;
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
            LoadAssessmentFromLocalDB();
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
            var selectedRecords = ChildAssessmentRecords.Where(p => p.IsSelected);
            if (selectedRecords != null && selectedRecords.Any())
            {
                var groupedRecords = selectedRecords.GroupBy(p => p.OfflineStudentID);
                if (groupedRecords != null && groupedRecords.Any())
                {
                    var lstSync = new List<SyncChild>();
                    var orgID = Convert.ToInt32(Application.Current.Properties["OrgnazationID"]);
                    var researchCodes = productResearchCodesService.GetResearchCodesByOrg(orgID);
                    foreach (var item in groupedRecords)
                    {
                        var localStudent = _studentService.GetStudentById(item.Key);
                        var researchCodesValues = productResearchCodeValuesService.GetProductResearchCodes(item.Key);
                        if (localStudent != null)
                        {
                            lstStudents.Add(localStudent);
                            localStudent.LocalTestFormIds = string.Join(",", item.ToList().Select(p => p.LocalTestInstance));
                            int addedBy;
                            var isSuccess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
                            var data = localStudent;
                            var syncChild = new SyncChild();
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
                            if (item != null && item.ToList().Any())
                            {
                                syncChild.TestRecords = new List<SyncTestRecord>();
                                foreach (var inneritem in item.ToList())
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
                                                newContentCatgory.RawScore = record.rawScore;
                                                newContentCatgory.TimeTaken = record.TimeTaken;
                                                newContentCatgory.ItemScore = record.rawScoreEnabled ? 0 : 1;
                                                newContentCatgory.Status = record.TSOStatus == "Saved" ? 1 : 0;
                                                newContentCatgory.AgeInMonths = CalculateAgeDiff(data.Birthdate);
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
                                        if (student != null && string.IsNullOrEmpty(student.UserId) && item.ChildUserId.HasValue)
                                        {
                                            student.UserId = item.ChildUserId.Value.ToString();
                                            student.updatedOn = DateTime.Now.ToUniversalTime().ToString();
                                            student.updatedOnUTC = DateTime.Now.ToUniversalTime().ToString();
                                            _studentService.Update(student);
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
                                }
                                else
                                {
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
                                    clinicalTestFormService.UpdateSyncStatus((int)item.StatusCode, item.LocalFormInstanceId);
                                }
                            }
                            LoadAssessmentFromLocalDB();
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
