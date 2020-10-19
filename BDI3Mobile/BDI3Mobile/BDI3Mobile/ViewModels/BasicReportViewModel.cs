using Acr.UserDialogs;
using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Views.ItemAdministrationView;
using BDI3Mobile.Views.PopupViews;
using BDI3Mobile.Views.Reports;
using System.Collections.Generic;
using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BDI3Mobile.ViewModels
{
    public class BasicReportViewModel : BindableObject
    {

        private readonly ILocationService _locationService;
        private readonly IStudentsService _studentService;
        private readonly IClinicalTestFormService _clinicalTestFormService;
        private readonly ICommonDataService _commonDataService;
        public IStudentTestFormsService _studentTestFormsService;
        #region Properties
        private bool runReport;
        public bool RunReport
        {
            get
            {
                return runReport;
            }
            set
            {
                runReport = value;
                OnPropertyChanged(nameof(RunReport));
            }
        }
        public int TotalAgeinMonths;
        public int OfflineStudentID { get; set; }
        public int selectedCount { get; set; }
        public bool isLocationPopupOpen { get; set; }
        public bool isChildRecordPopupOpen { get; set; }
        public bool isBatteryTypePopupOpen { get; set; }
        public bool isRecordFormPopupOpen { get; set; }
        
        private bool isChildRecordButtonEnabled;
        public bool IsChildRecordButtonEnabled
        {
            get
            {
                return isChildRecordButtonEnabled;
            }
            set
            {
                isChildRecordButtonEnabled = value;
                OnPropertyChanged(nameof(IsChildRecordButtonEnabled));
                if(value)
                 GetChildRecords();
            }
        }

        private bool isBatteryTypeButtonEnabled;

        public bool IsBatteryTypeButtonEnabled
        {
            get
            {
                return isBatteryTypeButtonEnabled;
            }
            set
            {
                isBatteryTypeButtonEnabled = value;
                OnPropertyChanged(nameof(IsBatteryTypeButtonEnabled));
                if (value)
                    GetBatteryTypes();
            }
        }

        private bool isRecordFormButtonEnabled;

        public bool IsRecordFormButtonEnabled
        {
            get
            {
                return isRecordFormButtonEnabled;
            }
            set
            {
                isRecordFormButtonEnabled = value;
                OnPropertyChanged(nameof(IsRecordFormButtonEnabled));
                if (value)
                    GetRecordForms();
            }
        }
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

        private string Age { get; set; }
        private List<Location> AllLocations;
        private List<Location> locations;
        public List<Location> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                locations = value;
                OnPropertyChanged(nameof(Locations));

            }
        }

        List<ChildRecord> _childRecords = new List<ChildRecord>();
        public List<ChildRecord> ChildRecords
        {
            get
            {
                return _childRecords;
            }

            set
            {
                _childRecords = value;
                OnPropertyChanged(nameof(ChildRecords));
            }
        }
        List<BatteryTypes> _batteryTypes = new List<BatteryTypes>();
        public List<BatteryTypes> BatteryTypeList
        {
            get
            {
                return _batteryTypes;
            }

            set
            {
                _batteryTypes = value;
                OnPropertyChanged(nameof(BatteryTypeList));
            }
        }

        private ObservableCollection<LocationNew> locationsObservableCollection;
        public ObservableCollection<LocationNew> LocationsObservableCollection
        {
            get
            {
                return locationsObservableCollection;
            }
            set
            {
                locationsObservableCollection = value;
                OnPropertyChanged(nameof(LocationsObservableCollection));

            }
        }

        private List<string> _selectedLocations;
        public List<string> SelectedLocations
        {
            get { return _selectedLocations; }
            set
            {
                _selectedLocations = value;
                OnPropertyChanged(nameof(SelectedLocations));
            }
        }
        private List<string> _selectedLocationsReport;
        public List<string> SelectedLocationsReport
        {
            get { return _selectedLocationsReport; }
            set
            {
                _selectedLocationsReport = value;
                OnPropertyChanged(nameof(SelectedLocationsReport));
            }
        }
        private string _locationSeleted;
        public string LocationsSelected
        {
            get { return _locationSeleted; }
            set
            {
                _locationSeleted = value;
                OnPropertyChanged(nameof(LocationsSelected));
            }
        }
        private string _selectedChild;       
        public string SelectedChild
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                OnPropertyChanged(nameof(SelectedChild));
                if (value == null)
                    _selectedChild = "Select child";
            }
        }      
        private string _selectedRecordForm;
        public string SelectedRecordForm
        {
            get { return _selectedRecordForm; }
            set
            {
                _selectedRecordForm = value;
                OnPropertyChanged(nameof(SelectedRecordForm));
                if (value == null)
                    _selectedRecordForm = "Select record form";
            }
        }
        private int _selectedRecordFormID;
        public int SelectedRecordFormID
        {
            get { return _selectedRecordFormID; }
            set
            {
                _selectedRecordFormID = value;
                OnPropertyChanged(nameof(SelectedRecordFormID));
            }
        }
        private int _selectedAssessmentID;
        public int SelectedAssessmentID
        {
            get { return _selectedAssessmentID; }
            set
            {
                _selectedAssessmentID = value;
                OnPropertyChanged(nameof(SelectedAssessmentID));
            }
        }
        private string _selectedAssessmentType;

        public string SelectedAssessmentType
        {
            get { return _selectedAssessmentType; }
            set
            {
                _selectedAssessmentType = value;
                OnPropertyChanged(nameof(SelectedAssessmentType));
                if (value == null)
                    _selectedAssessmentType = "Select battery type";
            }
        }

        List<ChildInformationRecord> _childTestRecordsRecords = new List<ChildInformationRecord>();
        public List<ChildInformationRecord> ChildTestRecords
        {
            get
            {
                return _childTestRecordsRecords;
            }

            set
            {
                _childTestRecordsRecords = value;
                OnPropertyChanged(nameof(ChildTestRecords));
            }
        }
        #endregion

        public BasicReportViewModel()
        {
            _locationService = DependencyService.Get<ILocationService>();
            _studentService = DependencyService.Get<IStudentsService>();
            _clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
            _commonDataService = DependencyService.Get<ICommonDataService>();
           _studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            GetLocations();
        }
        /// <summary>
        /// Fetch locations
        /// </summary>
        public void GetLocations()
        {
            var locations = _locationService.GetLocationTree();
            if (locations != null && locations.Any())
            {
                AllLocations = locations.ToList();
                Locations = new List<Location>();
                LocationsObservableCollection = new ObservableCollection<LocationNew>();
                var parentLocation = locations.Where(p => p.ParentLocationId == 0).ToList();
                if (parentLocation != null && parentLocation.Any())
                {
                    foreach (var item in parentLocation)
                    {
                        var location = new Location();
                        var locationnew = new LocationNew();

                        location.LocationId = item.LocationId;
                        location.LocationName = item.LocationName;
                        location.IsSelected = item.IsSelected;
                        location.IsEnabled = item.IsEnabled;
                        location.ParentLocationId = item.ParentLocationId;
                        Locations.Add(location);

                        locationnew.LocationId = item.LocationId;
                        locationnew.LocationName = item.LocationName;
                        locationnew.IsSelected = item.IsSelected;
                        locationnew.IsEnabled = item.IsEnabled;
                        locationnew.ParentLocationId = item.ParentLocationId;
                        LocationsObservableCollection.Add(locationnew);
                        locations.Remove(item);
                        GenerateSubLocation(locationnew, locations);
                    }
                }
            }
        }
        /// <summary>
        /// Generate sub locations
        /// </summary>
        /// <param name="location"></param>
        /// <param name="lstLocation"></param>
        private void GenerateSubLocation(LocationNew location, List<Location> lstLocation)
        {

            var subLocation = lstLocation.Where(p => p.ParentLocationId == location.LocationId);
            if (subLocation != null && subLocation.Any())
            {
                location.SubLocations = new List<LocationNew>();
                foreach (var item in subLocation)
                {
                    item.Level = location.Level + 1;
                    var locationnew = new LocationNew();
                    locationnew.LocationId = item.LocationId;
                    locationnew.LocationName = item.LocationName;
                    locationnew.IsSelected = item.IsSelected;
                    locationnew.IsEnabled = item.IsEnabled;
                    locationnew.ParentLocationId = item.ParentLocationId;
                    locationnew.Level = item.Level;
                    location.SubLocations.Add(locationnew);
                    location.HasSubLocations = true;
                    GenerateSubLocation(locationnew, lstLocation);
                }
            }
        }
        /// <summary>
        /// Fetch child records
        /// </summary>
        public async void GetChildRecords()
        {
            int addedBy;

            if (ChildRecords.Any())
                ChildRecords.Clear();

            var initialChildRecords = new List<ChildRecord>();

            if (Application.Current.Properties.ContainsKey("UserID"))
            {
                var isSuccess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
                if (isSuccess)
                {
                    var childRecords = (_studentService.GetStudentsByOfflineID(addedBy));
                    var testRecords = (_clinicalTestFormService.GetStudentTestForms()).Where(x => x.FormStatus != "Not started").Select(x => x.LocalStudentId).ToList();
                    var filteredChildRecord = childRecords.FindAll(x => testRecords.Contains(x.OfflineStudentID));
                    foreach (var childRecord in filteredChildRecord)
                    {
                        if (childRecord.SelectedLocationId.HasValue && AllLocations != null && AllLocations.Any() && AllLocations.Where(p => p.IsEnabled).Any(p => p.LocationId == childRecord.SelectedLocationId.Value))
                        {
                            string birthdate = (childRecord.Birthdate.Month < 10 ? "0" + childRecord.Birthdate.Month : childRecord.Birthdate.Month + "") + "/" + (childRecord.Birthdate.Day < 10 ? "0" + childRecord.Birthdate.Day : childRecord.Birthdate.Day + "") + "/" + childRecord.Birthdate.Year;
                            initialChildRecords.Add(new ChildRecord()
                            {
                                Name = childRecord.FirstName + " " + childRecord.LastName,
                                selected = false,
                                Location = AllLocations != null && AllLocations.Any() && childRecord.SelectedLocationId != null ? AllLocations.FirstOrDefault(p => p.LocationId == childRecord.SelectedLocationId.Value)?.LocationName : "",
                                LocationID = (childRecord.SelectedLocationId == null) ? 0 : Convert.ToInt32(childRecord.SelectedLocationId),
                                OfflineStudentId = childRecord.OfflineStudentID,
                                DOB = birthdate
                            });
                        }
                    }
                    var list = initialChildRecords.OrderBy(a => a.LastName);
                    initialChildRecords = new List<ChildRecord>(list);                    

                    var searchChildRecords = new List<ChildRecord>();
                    IEnumerable<ChildRecord> query = initialChildRecords;
                    
                    if (SelectedLocations.Count > 0)
                        query = query.Where(p => !string.IsNullOrEmpty(p.Location) && SelectedLocations.Contains(p.Location));

                    searchChildRecords = new List<ChildRecord>(query);
                    foreach (var item in searchChildRecords)
                    {
                        ChildRecords.Add(item);
                    }
                }
            }

            if (ChildRecords.Count == 0)
            {
                SelectedChild = "No results found";
                IsChildRecordButtonEnabled = false;
                IsBatteryTypeButtonEnabled = false;
            }  
            else
            {
                SelectedChild = null;
            }

        }
        /// <summary>
        /// Fetch Assessment types
        /// </summary>
        public void GetBatteryTypes()
        {
            if (BatteryTypeList.Any())
                BatteryTypeList.Clear();

            if (OfflineStudentID != 0)
            {
                CalculateAgeDiff();
                var testRecords = (_clinicalTestFormService.GetStudentTestFormsByStudentID(OfflineStudentID));
                var filteredBatteryTypeList = testRecords.OrderBy(x => x.assessmentId).Select(x => x.assessmentId).Distinct();
                foreach (var item in filteredBatteryTypeList)
                {
                    if (item == AssignmentTypes.BattelleDevelopmentalCompleteID)
                        BatteryTypeList.Add(new BatteryTypes()
                        {
                            assessmentId = item,
                            Description = AssignmentTypes.BattelleDevelopmentalCompleteString,
                            selected = false
                        });
                    if (item == AssignmentTypes.BattelleDevelopmentalScreenerID)
                        BatteryTypeList.Add(new BatteryTypes()
                        {
                            assessmentId = item,
                            Description = AssignmentTypes.BattelleDevelopmentScreenerString,
                            selected = false
                        });
                    if (item == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID)
                        BatteryTypeList.Add(new BatteryTypes()
                        {
                            assessmentId = item,
                            Description = AssignmentTypes.BattelleEarlyAcademicSurveyString,
                            selected = false
                        });
                }
            }
            BatteryTypeList.OrderBy(x => x.assessmentId);

            if (!BatteryTypeList.Any())
            {
                IsBatteryTypeButtonEnabled = false;
                IsRecordFormButtonEnabled = false;
                SelectedAssessmentType = "No results found";
            }
            else
            {
                SelectedAssessmentType = null;
            }

        }
        /// <summary>
        /// Fetch record forms
        /// </summary>
        public void GetRecordForms()
        {
                int assessmentID;
                var studentTestRecordForms = new List<ChildInformationRecord>();
                var testRecords = (_clinicalTestFormService.GetStudentTestFormsByStudentID(OfflineStudentID));
          
                if (SelectedAssessmentType == AssignmentTypes.BattelleDevelopmentalCompleteString)
                    assessmentID = AssignmentTypes.BattelleDevelopmentalCompleteID;
                else if (SelectedAssessmentType == AssignmentTypes.BattelleDevelopmentScreenerString)
                    assessmentID = AssignmentTypes.BattelleDevelopmentalScreenerID;
                else
                    assessmentID = AssignmentTypes.BattelleDevelopmentalAcademicSurveyID;

                var sortedRecords = testRecords.Where(x => x.assessmentId == assessmentID);
                var recordForm = "";
                foreach (var testRecord in sortedRecords)
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
                        AssesmentId = testRecord.assessmentId,
                        LocalTestInstance = testRecord.LocalTestRecodId,
                        RecordForm = recordForm,
                        InitialTestDate = dateOfTesting.Date.ToString("d"),
                        RecordFormName = recordForm + " (" + dateOfTesting.Date.ToString("d") + ")"
                    }) ;
                    }

                }
                
                var sortedRecordsByDate =  studentTestRecordForms.OrderByDescending(x => x.InitialTestDate);
                studentTestRecordForms = new List<ChildInformationRecord>();

                foreach (var dateSortedRecord in sortedRecordsByDate)
                    {
                        studentTestRecordForms.Add(dateSortedRecord);
                    }

                ChildTestRecords.Clear();
                ChildTestRecords = studentTestRecordForms;
                ChildTestRecords.OrderByDescending(x => x.InitialTestDate);
                if(!ChildTestRecords.Any())
                {
                    SelectedRecordForm = "No results found";
                    IsRecordFormButtonEnabled = false;
                }
                else
                {
                    SelectedRecordForm = null;
                }
        }

        #region Commands
        public  ICommand OpenLocationPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isLocationPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectLocationPopupView && (popup as SelectLocationPopupView).Title == "LocationPopView")
                            {
                                return;
                            }
                        }
                    }
                    isLocationPopupOpen = true;

                    var item = (e as BasicReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectLocationPopupView(this, null, SelectedLocationsReport));
                });
            }
        }

        public  ICommand OpenChildRecordPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isChildRecordPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectChildPopupView && (popup as SelectChildPopupView).Title == "ChildRecordPopView")
                            {
                                return;
                            }
                        }
                    }
                    isChildRecordPopupOpen = true;

                    var item = (e as BasicReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectChildPopupView(this, null));
                });
            }
        }

        public  ICommand OpenBatteryTypeRecordPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isBatteryTypePopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectBatteryTypePopupView && (popup as SelectBatteryTypePopupView).Title == "BatteryTypePopView")
                            {
                                return;
                            }
                        }
                    }
                    isBatteryTypePopupOpen = true;

                    await PopupNavigation.Instance.PushAsync(new SelectBatteryTypePopupView(this, null));
                });
            }
        }

        public  ICommand OpenRecordFormsPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isRecordFormPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectRecordFormPopupView && (popup as SelectRecordFormPopupView).Title == "RecordFormPopView")
                            {
                                return;
                            }
                        }
                    }
                    isRecordFormPopupOpen = true;

                    var item = (e as BasicReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectRecordFormPopupView(this, null));
                });
            }
        }

        public  ICommand RunReportCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (SelectedRecordFormID != 0)
                    {
                        if (PopupNavigation.Instance.PopupStack.Count == 1)
                        {
                            await PopupNavigation.Instance.PopAsync(false);
                        }
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(200);
                        _commonDataService.StudentTestForms = _studentTestFormsService.GetStudentTestForms(SelectedRecordFormID);
                        _commonDataService.LocaInstanceID = SelectedRecordFormID;
                        _commonDataService.IsCompleteForm = SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID;
                        _commonDataService.IsAcademicForm = SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID;
                        _commonDataService.IsScreenerForm = SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalScreenerID;
                        var navigationParams = new AdminstrationNavigationParams
                        {
                            LocalInstanceID = SelectedRecordFormID,
                            TestDate = TestDate,
                            FullName = FullName,
                            OfflineStudentID = OfflineStudentID
                        };
                        if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID)
                        {
                            navigationParams.IsDevelopmentCompleteForm = true;
                        }
                        UserDialogs.Instance.HideLoading();
                        if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID)
                        {                           
                            await PopupNavigation.Instance.PushAsync(new DevelopmentalReport(), false);
                        }
                        else if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalScreenerID)
                        {                           
                            await PopupNavigation.Instance.PushAsync(new ScreenerReport(Age), false);
                        }
                        else
                        {                            
                            await PopupNavigation.Instance.PushAsync(new BAESReport(), false);
                        }
                    }
                    else
                    {
                        RunReport = false;
                    }
                });
            
            }
        }
        #endregion
        private void CalculateAgeDiff()
        {
            DateTime Now = DateTime.Now;
            var splittedDate = DOB.Split('/');
            DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
            int Years = new DateTime(DateTime.Now.Subtract(itemdateTime).Ticks).Year - 1;
            DateTime PastYearDate = itemdateTime.AddYears(Years);
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
            TotalAgeinMonths = Months + (Years * 12);
            _commonDataService.TotalAgeinMonths = TotalAgeinMonths;
            if (Years > 0)
                Age = Years + " years, " + Months + " months";
            else
                Age = Months + " months";
        }

    }
}
