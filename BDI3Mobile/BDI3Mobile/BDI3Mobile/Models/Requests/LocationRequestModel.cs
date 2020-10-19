using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Models.Requests
{
    public class LocationResponseModel : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
        public IList<LocationResponseModel> subItems { get; set; }
        public int hierarchyID { get; set; }
        public int childHierarchyID { get; set; }
        public int parentLocationID { get; set; }
        public bool enabled { get; set; }
        public bool isDeleted { get; set; }
        public string updatedOn { get; set; }
        public int DownloadedBy { get; set; }
    }
    public class SubItem : BindableObject
    {
        public int value { get; set; }
        public string text { get; set; }
        private bool isselected;
        public bool selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
        public IList<SubItem> subItems { get; set; }
        public int hierarchyID { get; set; }
        public int childHierarchyID { get; set; }
        public int parentLocationID { get; set; }
        public bool enabled { get; set; }
    }
}
