using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BDI3Mobile.Models.DBModels
{
    public class Location
    {
        [PrimaryKey,AutoIncrement]
        public int LocalLocationId { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int ParentLocationId { get; set; }
        public bool IsSelected { get; set; }
        public bool IsEnabled { get; set; }
        public int Level { get; set; } = 0;
        public int UserId { get; set; }
        public bool isDeleted { get; set; }
        public string updatedOn { get; set; }
        public int DownloadedBy
        {
            get; set;
        }
        [Ignore]
        public List<Location> SubLocations { get; set; }

    }

    public class LocationNew : BindableObject
    {


        [PrimaryKey]
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int ParentLocationId { get; set; }
        public bool _isEnabled;
        public bool IsEnabled {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
                IsVisible = !_isEnabled;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private bool _isSeleted;
        public bool IsSelected
        {
            get
            {
                return _isSeleted;
            }
            set
            {
                if (IsEnabled)
                    _isSeleted = value;
                else
                    _isSeleted = false;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        public int Level { get; set; } = 0;
        [Ignore]
        public List<LocationNew> SubLocations { get; set; }

        private bool hasSubLocation;
        public bool HasSubLocations
        {
            get
            {
                return hasSubLocation;
            }
            set
            {
                hasSubLocation = value;
                OnPropertyChanged(nameof(HasSubLocations));
            }
        }

        private string expandImage = "menudownarrow.png";
        public string ExpandImage {
            get
            {
                return expandImage;
            }
            set
            {
                expandImage = value;
                OnPropertyChanged(nameof(ExpandImage));
            }
        }

        private bool isExpanded;
        public bool IsExpanded {
            get
            {
                return isExpanded;
            }
            set
            {
                isExpanded = value;
                ExpandImage = IsExpanded == true ? "menuuparrow.png" : "menudownarrow.png";
                OnPropertyChanged(nameof(IsExpanded));
            } 
        }

        private bool isAddedToCount;
        public bool IsAddedToCount {
            get
            {
                return isAddedToCount;
            }
            set
            {
                isAddedToCount = value;
                OnPropertyChanged(nameof(IsAddedToCount));
            }
        }
        public bool IsVisible { get; set; }

    }
}
