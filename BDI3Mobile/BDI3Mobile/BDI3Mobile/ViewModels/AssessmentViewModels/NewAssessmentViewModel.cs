using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AssessmentViewModels
{
    public class NewAssessmentViewModel : BindableObject
    {
        private readonly ILocationService _locationService;
        private readonly IUserPermissionService userPermissionService;

        public ICommand ActionClickedCommand { get; set; }
        public ICommand UpdateListviewBoundCommand { get; set; }
        private bool actionIconClicked;
        public ICommand HyperLinkClickedCommand { get; set; }
        public ICommand SelectLocationCommand => new Command(() =>
        {
            ShowLocation = !ShowLocation;
            Cover = ShowLocation;
        });


        public ICommand UnCoverCommand
        {
            get
            {
                return new Command(UnCover);
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

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }
        private string childID;
        public string ChildID
        {
            get { return childID; }
            set
            {
                childID = value;
                OnPropertyChanged(nameof(ChildID));
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
        private DateTime minimumDate;
        public DateTime MinimumDate
        {
            get
            {
                return minimumDate;
            }
            set
            {
                minimumDate = value;
                OnPropertyChanged(nameof(MinimumDate));
            }
        }

        public int OfflineStudentId { get; set; }

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
        private string enrollment;
        public string Enrollment
        {
            get { return enrollment; }
            set
            {
                enrollment = value;
                OnPropertyChanged(nameof(Enrollment));
            }
        }
        private bool _showLocation;
        public bool ShowLocation
        {
            get
            {
                return _showLocation;
            }
            set
            {
                _showLocation = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowLocation));
            }
        }
        private bool cover;
        public bool Cover
        {
            get { return cover; }
            set
            {
                cover = value;
                OnPropertyChanged(nameof(Cover));
            }
        }


        List<ChildRecordsNewAssessment> _childTestRecords = new List<ChildRecordsNewAssessment>();
        public List<ChildRecordsNewAssessment> ChildTestRecords
        {
            get
            {
                return _childTestRecords;
            }
            set
            {
                _childTestRecords = value;
                OnPropertyChanged("ChildTestRecords");
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
                    UpdateChildRecords(value);
                _selectAll = value;
                OnPropertyChanged(nameof(SelectAll));
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
        #region LocationTree
        private bool isLocationEmpty;
        public bool IsLocationEmpty
        {
            get
            {
                return isLocationEmpty;
            }
            set
            {
                isLocationEmpty = value;
                OnPropertyChanged(nameof(IsLocationEmpty));
            }
        }
        private string _locationName;
        public string LocationName
        {
            get { return _locationName; }
            set
            {

                _locationName = value;
                OnPropertyChanged(nameof(LocationName));
            }
        }
        private Location selectedLocation;
        public Location SelectedLocation
        {
            get
            {
                return selectedLocation;
            }
            set
            {
                selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));

            }
        }

        private bool _isTabStop = true;

        public bool IsTabStop
        {
            get
            {
                return _isTabStop;
            }
            set
            {
                _isTabStop = value;
                OnPropertyChanged(nameof(IsTabStop));
                MessagingCenter.Send<String, bool>("Tab", "Tab", IsTabStop);

            }
        }

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
                OnPropertyChanged(nameof(_selectedLocations));
            }
        }
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

        public void ReloadClicked()
        {
            FirstName = "";
            LastName = "";
            LocationName = "";
            ChildID = "";
            SearchResult = "";
            SearchErrorVisible = false;
            SelectAll = false;
            DOBIsValid = false;
            EnrollmentDateIsValid = false;
            IsTableBottomLineVisible = false;


            SelectedLocations.Clear();
            SelectedLocations = new List<string>();

            foreach (var item in LocationsObservableCollection)
            {
                if (item.IsSelected)
                    item.IsSelected = false;
            }
            //SearchClicked(null, null);
            LoadChildRecordsFromDBAsync();
            SearchErrorColor = Color.Black;
            return;
        }
        #endregion
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

        public ICommand AscendingButtonCommand { get; set; }
        public ICommand DescendingButtonCommand { get; set; }



        private readonly IStudentsService _studentService;

        public NewAssessmentViewModel()
        {
            IsTableBottomLineVisible = true;
            _locationService = DependencyService.Get<ILocationService>();
            _studentService = DependencyService.Get<IStudentsService>();
            userPermissionService = DependencyService.Get<IUserPermissionService>();
            ActionClickedCommand = new Command<ChildRecordsNewAssessment>(ActionClicked);
            AscendingButtonCommand = new Command<string>(AscendingClicked);
            DescendingButtonCommand = new Command<string>(DecendingClicked);
            UpdateListviewBoundCommand = new Command<double>(UpdateChildRecordTableBounds);
            HyperLinkClickedCommand = new Xamarin.Forms.Command<ChildRecordsNewAssessment>(HyperlinkClicked);
            GetLocations();
            LoadChildRecordsFromDBAsync();


            MessagingCenter.Subscribe<ItemGroup>(this, "LocationSelected", (arg1) =>
            {
                Cover = false;
                if (SelectedLocation != null && SelectedLocation.LocationId == arg1.TreeViewId)
                {
                    LocationName = "";
                    SelectedLocation = null;
                }
                else
                {
                    LocationName = arg1.Name;
                    SelectedLocation = new Location() { LocationId = arg1.TreeViewId, LocationName = arg1.Name };
                }
                ShowLocation = false;
            });

            if (Device.RuntimePlatform == Device.Android)
            {
                MinimumDate = DateTime.SpecifyKind(new DateTime(1919, 01, 01), DateTimeKind.Unspecified);
            }
        }
        public int LocalTestFormID { get; set; }
        private void UpdateChildRecordTableBounds(double listviewheight)
        {
            double rowHeight = 0.0;
            double totalRowHeight = 0.0;
            if (ChildTestRecords.Any())
            {
                rowHeight = ChildTestRecords.FirstOrDefault().RowHeight;
                totalRowHeight = ChildTestRecords.Count() * rowHeight;
            }
            if (totalRowHeight > listviewheight)
            {
                IsTableBottomLineVisible = true;
            }
            else if (ChildTestRecords.Count > 0)
            {
                ChildTestRecords[ChildTestRecords.Count - 1].IsTableSepartorVisble = true;
                IsTableBottomLineVisible = false;
            }
        }

        private void UpdateChildRecords(bool value)
        {
            foreach (var childRecord in ChildRecords)
            {
                childRecord.isChecked = value;
            }
        }

        public Action ClearData { get; set; }

        private async void HyperlinkClicked(ChildRecordsNewAssessment child)
        {
            string firstName = child.FirstName;
            string lastName = child.LastName;
            int maxRows = 1000;
            int pageNo = 1;
            int pageSize = 25;
            string sortColumn = "name";
            string isortOrder = "ASC";
            ClearData?.Invoke();
            Application.Current.MainPage = new Views.ChildInformationpageView(child.OfflineStudentId);

        }
        private void DecendingClicked(string obj)
        {


            List<ChildRecordsNewAssessment> reorderedList = new List<ChildRecordsNewAssessment>();
            if (obj.Equals("NameDecending"))
            {
                var list = ChildTestRecords.OrderByDescending(a => a.LastName);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("DOBDecending"))
            {
                var list = ChildTestRecords.OrderByDescending(a => a.DOBDateField);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("LocationDecending"))
            {
                var list = ChildTestRecords.OrderByDescending(a => a.Location);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("EnrollmentDecending"))
            {
                var list = ChildTestRecords.OrderByDescending(a => a.EnrollmentDateField);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            ChildTestRecords.Clear();
            ChildTestRecords = reorderedList;
        }

        private void AscendingClicked(string obj)
        {
            List<ChildRecordsNewAssessment> reorderedList = new List<ChildRecordsNewAssessment>();
            if (obj.Equals("NameAscending"))
            {
                var list = ChildTestRecords.OrderBy(a => a.LastName);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("DOBAscending"))
            {
                var list = ChildTestRecords.OrderBy(a => a.DOBDateField);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("LocationAscending"))
            {
                var list = ChildTestRecords.OrderBy(a => a.Location);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("EnrollmentAscending"))
            {
                var list = ChildTestRecords.OrderBy(a => a.EnrollmentDateField);
                reorderedList = new List<ChildRecordsNewAssessment>(list);
            }
            else if (obj.Equals("ChildAscendingDecending"))
            {
                bool isAscending = (ChildTestRecords.SequenceEqual(ChildTestRecords.OrderBy(a => a.ChildID))) ? true : false;
                reorderedList = isAscending ? new List<ChildRecordsNewAssessment>(ChildTestRecords.OrderByDescending(a => a.ChildID)) : new List<ChildRecordsNewAssessment>(ChildTestRecords.OrderBy(a => a.ChildID));
            }
            ChildTestRecords.Clear();
            ChildTestRecords = reorderedList;
        }

        public async Task LoadChildRecordsFromDBAsync()
        {
            int addedBy;
            IsTableBottomLineVisible = true;
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
                            if (childRecord.SelectedLocationId.HasValue && AllLocations != null && AllLocations.Any() && AllLocations.Where(p => p.IsEnabled).Any(p => p.LocationId == childRecord.SelectedLocationId.Value))
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
                                    Enrollment = childRecord.EnrollmentDate != DateTime.MinValue ? childRecord.EnrollmentDate.ToString("MM/dd/yyyy") : "",
                                    Location = AllLocations != null && AllLocations.Any() && childRecord.SelectedLocationId != null ? AllLocations.FirstOrDefault(p => p.LocationId == childRecord.SelectedLocationId.Value)?.LocationName : "",
                                });
                            }
                        }
                        var list = initialChildRecords.OrderBy(a => a.LastName + a.FirstName);
                        initialChildRecords = new List<ChildRecordsNewAssessment>(list);
                    }
                }
            }
            ChildTestRecords.Clear();
            ChildTestRecords = initialChildRecords;
			
			SearchErrorVisible = true;
            SearchResult = string.Format(ErrorMessages.RecordsFoundMessage, ChildTestRecords.Count(), ChildTestRecords.Count() == 1 ? "Match" : "Matches");
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

        public async void SearchClicked(string dob, string enrollmentDate)
        {
            var searchChildRecords = new List<ChildRecordsNewAssessment>();
            var isMorethan1000 = false;
            await LoadChildRecordsFromDBAsync();

            if (!string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) || (SelectedLocations != null && SelectedLocations.Any()) || !string.IsNullOrEmpty(dob) || !string.IsNullOrEmpty(enrollmentDate) || !string.IsNullOrEmpty(ChildID))
            {
                IEnumerable<ChildRecordsNewAssessment> query = ChildTestRecords;

                if (!string.IsNullOrEmpty(FirstName))
                    query = query.Where(p => !string.IsNullOrEmpty(p.FirstName) && p.FirstName.ToLower().Contains(FirstName.ToLower()));
                if (!string.IsNullOrEmpty(LastName))
                    query = query.Where(p => !string.IsNullOrEmpty(p.LastName) && p.LastName.ToLower().Contains(LastName.ToLower()));
                if (!string.IsNullOrEmpty(ChildID))
                    query = query.Where(p => !string.IsNullOrEmpty(p.ChildID) && p.ChildID.ToLower().Contains(ChildID.ToLower()));
                if (!string.IsNullOrEmpty(dob))
                    query = query.Where(p => !string.IsNullOrEmpty(p.DOB) && p.DOB == dob);
                if (!string.IsNullOrEmpty(enrollmentDate))
                    query = query.Where(p => !string.IsNullOrEmpty(p.Enrollment) && p.Enrollment == enrollmentDate);
                if (SelectedLocations.Count > 0)
                    query = query.Where(p => !string.IsNullOrEmpty(p.Location) && SelectedLocations.Contains(p.Location));

                searchChildRecords = new List<ChildRecordsNewAssessment>(query);
            }
            else
            {
                searchChildRecords = ChildTestRecords;
            }


            if (!searchChildRecords.Any())
            {
                SearchResult = ErrorMessages.RecordMatchesFoundMessage;
                SearchErrorColor = Color.Red;
                SearchErrorVisible = true;
            }
            else
            {

                SearchResult = string.Format(ErrorMessages.RecordsFoundMessage, searchChildRecords.Count(), searchChildRecords.Count() == 1 ? "Match" : "Matches");
                SearchErrorColor = Color.Black;
                SearchErrorVisible = true;
                if (searchChildRecords.Count > 1000)
                {
                    isMorethan1000 = true;
                    SearchResult = ErrorMessages.SearchError;
                    SearchErrorColor = Color.Red;
                    SearchErrorVisible = true;
                }
                else if (searchChildRecords.Count > 5)
                {
                    searchChildRecords[searchChildRecords.Count - 1].IsTableSepartorVisble = false;
                }
                else
                {
                    searchChildRecords[searchChildRecords.Count - 1].IsTableSepartorVisble = true;
                    IsTableBottomLineVisible = false;
                }
                if (SelectAll)
                {
                    int index = 0;
                    foreach (var record in searchChildRecords)
                    {
                        index++;
                    }
                }
            }
            var list = searchChildRecords.OrderBy(a => a.LastName);
            var reorderedList = new List<ChildRecordsNewAssessment>(list);
            ChildTestRecords.Clear();
            if (!isMorethan1000)
            {
                ChildTestRecords = reorderedList;
            }
        }

        private async void ActionClicked(ChildRecordsNewAssessment child)
        {
            if (actionIconClicked)
                return;
            FullName = child.FirstName + " " + child.LastName;
            actionIconClicked = true;
            await PopupNavigation.Instance.PushAsync(new AssessmentConfigPopupView(child.OfflineStudentId));
            actionIconClicked = false;
        }

        void UnCover(object obj)
        {
            ShowLocation = false;
            Cover = false;
        }

        private bool dOBIsValid;
        public bool DOBIsValid
        {
            get
            {
                return dOBIsValid;
            }
            set
            {
                dOBIsValid = value;
                OnPropertyChanged(nameof(DOBIsValid));
            }
        }

        private bool enrollmentDateIsValid;
        public bool EnrollmentDateIsValid
        {
            get
            {
                return enrollmentDateIsValid;
            }
            set
            {
                enrollmentDateIsValid = value;
                OnPropertyChanged("EnrollmentDateIsValid");
            }
        }

        private bool assessmentDateIsValid;
        public bool AssessmentDateIsValid
        {
            get
            {
                return assessmentDateIsValid;
            }
            set
            {
                assessmentDateIsValid = value;
                OnPropertyChanged("AssessmentDateIsValid");
            }
        }
    }
}
