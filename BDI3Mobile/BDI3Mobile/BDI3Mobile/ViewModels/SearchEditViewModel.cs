using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using System.Threading.Tasks;
using BDI3Mobile.Views.AddChildViews;
using Rg.Plugins.Popup.Services;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using BDI3Mobile.Views.PopupViews;
using BDI3Mobile.Models.Common;

namespace BDI3Mobile.ViewModels
{
    public class SearchEditViewModel : BaseclassViewModel
    {
        private readonly IUserPermissionService userPermissionService;
        private readonly ILocationService _locationService;
        private bool isAscending = false;
        public ICommand HyperLinkClickedCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand EditChildCommand { get; set; }
        public ICommand AscendingButtonCommand { get; set; }
        public ICommand DescendingButtonCommand { get; set; }
        public ICommand UpdateListviewBoundCommand { get; set; }
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
        private readonly IStudentsService _studentService;

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

        List<string> _countryNames = new List<string>();
        public List<string> CountryNames
        {
            get
            {
                return _countryNames;
            }
            set
            {
                _countryNames = value;
                OnPropertyChanged(nameof(CountryNames));
            }
        }

        string _childID;
        public string ChildID
        {
            get
            {
                return _childID;
            }
            set
            {
                _childID = value;
                OnPropertyChanged(nameof(ChildID));
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

        string _firstName;
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

        private string _selectedCountry;
        public string SelectedCountry
        {
            get { return _selectedCountry; }
            set
            {
                _selectedCountry = value;
                OnPropertyChanged(nameof(SelectedCountry));
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

        #endregion
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
        private bool isTableViewVisble = false;
        public bool IsTableViewVisble
        {
            get { return isTableViewVisble; }
            set
            {
                isTableViewVisble = value;
                OnPropertyChanged(nameof(IsTableViewVisble));
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
        public void CheckBoxChanged(bool isChecked)
        {
            if (ChildRecords.Where(p => p.IsImageVisible == false && p.IsCheckboxVisible == false).Count() > 0)
            {
                var count = ChildRecords.Where(p => (p.isChecked == true)).Count();
                _selectAll = count == ChildRecords.Count();
                OnPropertyChanged(nameof(SelectAll));
            }
            else
            {
                var count = ChildRecords.Where(p => p.isChecked).Count();
                _selectAll = count == ChildRecords.Count();
                OnPropertyChanged(nameof(SelectAll));
            }
        }

        public void CheckBoxChanged()
        {
            if (ChildRecords.Where(p => p.IsImageVisible == false && p.IsCheckboxVisible == false).Count() > 0)
            {
                Console.Write("");
            }
            else
            {
                var count = ChildRecords.Where(p => p.isChecked).Count();
                _selectAll = count == ChildRecords.Count();
                OnPropertyChanged(nameof(SelectAll));
            }
        }

        private ITokenService _tokenservice;
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
        public Action SetExaminer { get; set; }
        public Action SetProgramNotes { get; set; }

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
        public ICommand ActionClickedCommand { get; set; }
        private bool actionIconClicked;
        private bool editIconClicked { get; set; }
        private async void ActionClicked(ChildRecord child)
        {
            if (actionIconClicked || editIconClicked)
            {
                return;
            }

            FullName = child.FirstName + " " + child.LastName;
            actionIconClicked = true;
            await PopupNavigation.Instance.PushAsync(new AssessmentConfigPopupView(child.OfflineStudentId));
            await Task.Delay(300);
            actionIconClicked = false;
        }
        public int LocalTestFormID { get; set; }

        public SearchEditViewModel()
        {
            IsTableBottomLineVisible = true;
            SearchErrorVisible = false;
            userPermissionService = DependencyService.Get<IUserPermissionService>();
            _tokenservice = DependencyService.Get<ITokenService>();
            _locationService = DependencyService.Get<ILocationService>();
            _studentService = DependencyService.Get<IStudentsService>();
            EditChildCommand = new Command<ChildRecord>(EditChildClicked);
            HyperLinkClickedCommand = new Xamarin.Forms.Command<ChildRecord>(HyperlinkClicked);
            AscendingButtonCommand = new Command<string>(AscendingClicked);
            DescendingButtonCommand = new Command<string>(DecendingClicked);
            UpdateListviewBoundCommand = new Command<double>(UpdateChildRecordTableBounds);
            GetLocations();
            PopulateCountryNames();

            ActionClickedCommand = new Command<ChildRecord>(ActionClicked);

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

            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                MinimumDate = DateTime.SpecifyKind(new DateTime(1919, 01, 01), DateTimeKind.Unspecified);
            }            
        }
        void UnCover(object obj)
        {
            ShowLocation = false;
            Cover = false;
        }
        private void UpdateChildRecordTableBounds(double listviewheight)
        {
            double rowHeight = 0.0;
            double totalRowHeight = 0.0;
            if (ChildRecords.Any())
            {
                rowHeight = ChildRecords.FirstOrDefault().RowHeight;
                totalRowHeight = ChildRecords.Count() * rowHeight;
            }
            if (totalRowHeight > listviewheight)
            {
                IsTableBottomLineVisible = true;
            }
            else if (ChildRecords.Count > 0)
            {
                ChildRecords[ChildRecords.Count - 1].IsTableSepartorVisble = true;
                IsTableBottomLineVisible = false;
            }
        }
        private void DecendingClicked(string obj)
        {
            List<ChildRecord> reorderedList = new List<ChildRecord>();
            if (obj.Equals("NameDecending"))
            {
                var list = ChildRecords.OrderByDescending(a => a.LastName);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("DOBDecending"))
            {
                var list = ChildRecords.OrderByDescending(a => a.DOBDateField);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("LocationDecending"))
            {
                var list = ChildRecords.OrderByDescending(a => a.Location);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("EnrollmentDecending"))
            {
                var list = ChildRecords.OrderByDescending(a => a.EnrollmentDateField);
                reorderedList = new List<ChildRecord>(list);
            }
            ChildRecords.Clear();
            ChildRecords = reorderedList;
        }

        private void AscendingClicked(string obj)
        {
            List<ChildRecord> reorderedList = new List<ChildRecord>();
            if (obj.Equals("NameAscending"))
            {
                var list = ChildRecords.OrderBy(a => a.LastName);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("DOBAscending"))
            {
                var list = ChildRecords.OrderBy(a => a.DOBDateField);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("LocationAscending"))
            {
                var list = ChildRecords.OrderBy(a => a.Location);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("EnrollmentAscending"))
            {
                var list = ChildRecords.OrderBy(a => a.EnrollmentDateField);
                reorderedList = new List<ChildRecord>(list);
            }
            else if (obj.Equals("ChildAscendingDecending"))
            {
                bool isAscending = (ChildRecords.SequenceEqual(ChildRecords.OrderBy(a => a.ChildID))) ? true : false;
                reorderedList = isAscending ? new List<ChildRecord>(ChildRecords.OrderByDescending(a => a.ChildID)) : new List<ChildRecord>(ChildRecords.OrderBy(a => a.ChildID));
            }
            ChildRecords.Clear();
            ChildRecords = reorderedList;
        }

        public async Task LoadChildRecordsFromDB()
        {
            int addedBy;
            IsTableBottomLineVisible = true;
            var initialChildRecords = new List<ChildRecord>();

            if (Application.Current.Properties.ContainsKey("UserID"))
            {
                var isSuccess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
                if (isSuccess)
                {
                    var childRecords = (_studentService.GetStudentsByOfflineID(addedBy));
                    var hasPermission = await userPermissionService.GetStudentEditPermissionsAsync() || Application.Current.Properties["UserTypeID"].ToString() == "1" || Application.Current.Properties["UserTypeID"].ToString() == "6";
                    foreach (var childRecord in childRecords)
                    {
                        if (childRecord.SelectedLocationId.HasValue && AllLocations != null && AllLocations.Any() && AllLocations.Where(p => p.IsEnabled).Any(p => p.LocationId == childRecord.SelectedLocationId.Value))
                        {
                            initialChildRecords.Add(new ChildRecord()
                            {
                                IsChildEditEnable = hasPermission || childRecord.AddedBy == addedBy,
                                EditIconImage = hasPermission || childRecord.AddedBy == addedBy ? "icon_edit.png" : "icon_edit_gray.png",
                                AddRfIconImage = hasPermission || childRecord.AddedBy == addedBy ? "icon_add_record.png" : "icon_add_record_gray.png",
                                FirstName = childRecord.FirstName,
                                LastName = childRecord.LastName,
                                ChildID = childRecord.ChildID,
                                ChildUserID = childRecord.UserId,
                                DOB = childRecord.Birthdate.ToString("MM/dd/yyyy"),
                                Enrollment = childRecord.EnrollmentDate.Date != DateTime.MinValue.Date ? childRecord.EnrollmentDate.ToString("MM/dd/yyyy") : "",
                                Location = AllLocations != null && AllLocations.Any() && childRecord.SelectedLocationId != null ? AllLocations.FirstOrDefault(p => p.LocationId == childRecord.SelectedLocationId.Value)?.LocationName : "",
                                LocationID = (childRecord.SelectedLocationId == null) ? 0 : Convert.ToInt32(childRecord.SelectedLocationId),
                                OfflineStudentId = childRecord.OfflineStudentID
                            });
                        }
                    }
                    var list = initialChildRecords.OrderBy(a => a.LastName + a.FirstName);
                    initialChildRecords = new List<ChildRecord>(list);
                }
            }

            SearchResult = string.Format(ErrorMessages.RecordsFoundMessage, initialChildRecords.Where(p => p.IsImageVisible == true && p.IsCheckboxVisible == true).Count(), initialChildRecords.Where(p => p.IsImageVisible == true && p.IsCheckboxVisible == true).Count() == 1 ? "Match" : "Matches");
            SearchErrorColor = Color.Black;
            SearchErrorVisible = true;
            ChildRecords.Clear();
            ChildRecords = initialChildRecords;
        }
        public async void SearchClicked(string dob, string enrollmentDate)
        {
            var searchChildRecords = new List<ChildRecord>();
            var isMorethan1000 = false;
            IsTableViewVisble = true;
            await LoadChildRecordsFromDB();

            if (!string.IsNullOrEmpty(FirstName) || !string.IsNullOrEmpty(LastName) || (SelectedLocations != null && SelectedLocations.Any()) || !string.IsNullOrEmpty(SelectedCountry) || !string.IsNullOrEmpty(dob) || !string.IsNullOrEmpty(enrollmentDate) || !string.IsNullOrEmpty(ChildID))
            {
                IEnumerable<ChildRecord> query = ChildRecords;

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

                searchChildRecords = new List<ChildRecord>(query);
            }
            else
            {
                searchChildRecords = ChildRecords;
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
                        searchChildRecords[index].isChecked = true;
                        index++;
                    }
                }
            }
            var list = searchChildRecords.OrderBy(a => a.LastName);
            var reorderedList = new List<ChildRecord>(list);
            ChildRecords.Clear();
            if (!isMorethan1000)
            {
                ChildRecords = reorderedList;
            }
        }

        public void ReloadClicked()
        {
            FirstName = "";
            LastName = "";
            SelectedCountry = "";
            LocationName = "";
            ChildID = "";
            SearchResult = "";
            SearchErrorVisible = false;
            SelectAll = false;
            DOBIsValid = false;
            EnrollmentDateIsValid = false;
            IsTableBottomLineVisible = false;
            SearchErrorVisible = false;

            SelectedLocations.Clear();
            SelectedLocations = new List<string>();

            foreach (var item in LocationsObservableCollection)
            {
                if (item.IsSelected)
                    item.IsSelected = false;
            }
            SearchClicked(null, null);
            return;
        }
        public void ReloadAscendingOrder()
        {
            List<ChildRecord> reorderedList = new List<ChildRecord>();
            if (!isAscending)
            {
                var list = ChildRecords.OrderBy(a => a.LastName);
                reorderedList = new List<ChildRecord>(list);

                List<ChildRecord> toRemove = reorderedList.Where(W => W.IsImageVisible == false && W.IsCheckboxVisible == false).ToList();
                if (toRemove.Count() > 0)
                {
                    foreach (var item in toRemove)
                    {
                        bool isSuccess = reorderedList.Remove(item);
                    }
                    if (reorderedList.Count < 20)
                    {
                        var rowsToAdd = 20 - reorderedList.Count;
                        for (int i = 0; i < rowsToAdd; i++)
                        {
                            reorderedList.Insert(reorderedList.Count, new ChildRecord() { IsCheckboxVisible = false, IsImageVisible = false });
                        }
                    }
                }
                isAscending = true;
            }
            else
            {
                var list = ChildRecords.OrderByDescending(a => a.LastName);
                reorderedList = new List<ChildRecord>(list);
                isAscending = false;
            }

            ChildRecords.Clear();
            ChildRecords = reorderedList;
        }

        private void PopulateCountryNames()
        {
            _countryNames = new List<string> { "Afghanistan", "Albania", "Algeria", "American Samoa", "Andorra", "Angola", "Anguilla", "Antarctica", "Antigua and Barbuda", "Argentina", "Armenia", "Aruba", "Australia", "Austria", "Azerbaijan", "Bahamas", "Bahrain", "Bangladesh", "Barbados", "Belarus", "Belgium", "Belize", "Benin", "Bermuda", "Bhutan", "Bolivia", "Bosnia and Herzegovina", "Botswana", "Bouvet Island", "Brazil", "British Indian Ocean Territory", "Brunei Darussalam", "Bulgaria", "Burkina Faso", "Burundi", "Cambodia", "Cameroon", "Canada", "Cape Verde", "Cayman Islands", "Central African Republic", "Chad", "Chile", "China", "Christmas Island", "Cocos (Keeling) Islands", "Colombia", "Comoros", "Congo", "Congo, the Democratic Republic of the", "Cook Islands", "Costa Rica", "Cote D'Ivoire", "Croatia", "Cuba", "Cyprus", "Czech Republic", "Denmark", "Djibouti", "Dominica", "Dominican Republic", "Ecuador", "Egypt", "El Salvador", "Equatorial Guinea", "Eritrea", "Estonia", "Ethiopia", "Falkland Islands (Malvinas)", "Faroe Islands", "Fiji", "Finland", "France", "French Guiana", "French Polynesia", "French Southern Territories", "Gabon", "Gambia", "Georgia", "Germany", "Ghana", "Gibraltar", "Greece", "Greenland", "Grenada", "Guadeloupe", "Guam", "Guatemala", "Guinea", "Guinea-Bissau", "Guyana", "Haiti", "Heard Island and Mcdonald Islands", "Holy See (Vatican City State)", "Honduras", "Hong Kong", "Hungary", "Iceland", "India", "Indonesia", "Iran, Islamic Republic of", "Iraq", "Ireland", "Israel", "Italy", "Jamaica", "Japan", "Jordan", "Kazakhstan", "Kenya", "Kiribati", "Korea, Democratic People's Republic of", "Korea, Republic of", "Kuwait", "Kyrgyzstan", "Lao People's Democratic Republic", "Latvia", "Lebanon", "Lesotho", "Liberia", "Libyan Arab Jamahiriya", "Liechtenstein", "Lithuania", "Luxembourg", "Macao", "Macedonia, the Former Yugoslav Republic of", "Madagascar", "Malawi", "Malaysia", "Maldives", "Mali", "Malta", "Marshall Islands", "Martinique", "Mauritania", "Mauritius", "Mayotte", "Mexico", "Micronesia, Federated States of", "Moldova, Republic of", "Monaco", "Mongolia", "Montserrat", "Morocco", "Mozambique", "Myanmar", "Namibia", "Nauru", "Nepal", "Netherlands", "Netherlands Antilles", "New Caledonia", "New Zealand", "Nicaragua", "Niger", "Nigeria", "Niue", "Norfolk Island", "Northern Mariana Islands", "Norway", "Oman", "Pakistan", "Palau", "Palestinian Territory, Occupied", "Panama", "Papua New Guinea", "Paraguay", "Peru", "Philippines", "Pitcairn", "Poland", "Portugal", "Puerto Rico", "Qatar", "Reunion", "Romania", "Russian Federation", "Rwanda", "Saint Helena", "Saint Kitts and Nevis", "Saint Lucia", "Saint Pierre and Miquelon", "Saint Vincent and the Grenadines", "Samoa", "San Marino", "Sao Tome and Principe", "Saudi Arabia", "Senegal", "Serbia and Montenegro", "Seychelles", "Sierra Leone", "Singapore", "Slovakia", "Slovenia", "Solomon Islands", "Somalia", "South Africa", "South Georgia and the South Sandwich Islands", "Spain", "Sri Lanka", "Sudan", "Suriname", "Svalbard and Jan Mayen", "Swaziland", "Sweden", "Switzerland", "Syrian Arab Republic", "Taiwan, Province of China", "Tajikistan", "Tanzania, United Republic of", "Thailand", "Timor-Leste", "Togo", "Tokelau", "Tonga", "Trinidad and Tobago", "Tunisia", "Turkey", "Turkmenistan", "Turks and Caicos Islands", "Tuvalu", "Uganda", "Ukraine", "United Arab Emirates", "United Kingdom", "United States", "United States Minor Outlying Islands", "Uruguay", "Uzbekistan", "Vanuatu", "Venezuela", "Viet Nam", "Virgin Islands, British", "Virgin Islands, US", "Wallis and Futuna", "Western Sahara", "Yemen", "Zambia", "Zimbabwe", };

        }

        private async void HyperlinkClicked(ChildRecord child)
        {
            string firstName = child.FirstName;
            string lastName = child.LastName;
            int maxRows = 1000;
            int pageNo = 1;
            int pageSize = 25;
            string sortColumn = "name";
            string isortOrder = "ASC";
            Application.Current.MainPage = new Views.ChildInformationpageView(child.OfflineStudentId);
        }

        private async void EditChildClicked(ChildRecord childRecord)
        {
            if (editIconClicked || actionIconClicked)
            {
                return;
            }

            editIconClicked = true;
            await App.Current.MainPage.Navigation.PushModalAsync(new ChildTabbedPage(childRecord.OfflineStudentId));
            await Task.Delay(300);
            editIconClicked = false;
        }
        private void UpdateChildRecords(bool value)
        {
            foreach (var childRecord in ChildRecords)
            {
                childRecord.isChecked = value;
            }
        }
    }
}
