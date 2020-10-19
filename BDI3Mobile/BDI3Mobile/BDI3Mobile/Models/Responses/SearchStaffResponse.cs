using Newtonsoft.Json;
using Xamarin.Forms;

namespace BDI3Mobile.Models.Responses
{
    public class SearchStaffResponse
    {
        [JsonProperty("maxRows")]
        public int MaxRows { get; set; }
        [JsonProperty("userID")]
        public string UserID { get; set; }
        [JsonProperty("firstNameLastName")]
        public string FirstNameLastName { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }
        [JsonProperty("locationname")]
        public string Locationname { get; set; }
        [JsonProperty("roleActive")]
        public int RoleActive { get; set; }
        [JsonProperty("checkedval")]
        public bool Checkedval { get; set; }
        [JsonProperty("sortorder")]
        public string Sortorder { get; set; }
        [JsonProperty("sortcolumnname")]
        public string Sortcolumnname { get; set; }
        [JsonProperty("totalPage")]
        public int TotalPage { get; set; }
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
        [JsonProperty("pagesize")]
        public int Pagesize { get; set; }
        public int OrganizationId { get; set; }
        public int DownloadedBy { get; set; }
        public bool deleteType { get; set; }
        public string updatedOn { get; set; }
    }

    public class Examiner : BindableObject
    {
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
        public int? id { get; set; }
    }
}
