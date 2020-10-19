using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BDI3Mobile.Models.Responses
{
    public class ProgramNoteModel
    {
        [JsonProperty("labelId")]
        public int? LabelId { get; set; }
        [JsonProperty("labelName")]
        public string LabelName { get; set; }
        [JsonProperty("organizationId")]
        public int OrganizationId { get; set; }
        [JsonProperty("deleteType")]
        public bool DeleteType { get; set; }
        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }
        [JsonProperty("pageNo")]
        public int PageNo { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
        [JsonProperty("sortColumn")]
        public string SortColumn { get; set; }
        [JsonProperty("sortOrder")]
        public string SortOrder { get; set; }
        [JsonProperty("maxRows")]
        public int MaxRows { get; set; }
        [JsonProperty("totalRows")]
        public int TotalRows { get; set; }
        public string updatedOn { get; set; }
        public int DownLoadedBy { get; set; }

    }

    public class ProgramNotesResponse
    {
        public List<ProgramNoteModel> data { get; set; }
    }

    public class ProgramNote : BindableObject
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
