using System.Collections.Generic;
using Newtonsoft.Json;

namespace BDI3Mobile.Models.Requests
{
    public class StaffRequestModel
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        public string Email { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("userID")]
        public int UserID { get; set; }
        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
        [JsonProperty("SortColumnName")]
        public string sortColumnName { get; set; }
        [JsonProperty("includeDeleted")]
        public bool IncludeDeleted { get; set; }
        [JsonProperty("searchText")]
        public string SearchText { get; set; }
        [JsonProperty("paging")]
        public string Paging { get; set; }
        [JsonProperty("pagesize")]
        public int Pagesize { get; set; }
        [JsonProperty("currentpage")]
        public int Currentpage { get; set; }
        public int OrganizationID { get; set; }
        [JsonProperty("location")]
        public List<StaffLocation> Location { get; set; }
        [JsonProperty("locationName")]
        public string LocationName { get; set; }
    }
    public class StaffLocation
    {

    }
}
