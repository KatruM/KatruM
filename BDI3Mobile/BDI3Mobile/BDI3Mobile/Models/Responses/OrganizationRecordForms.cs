using Newtonsoft.Json;
using SQLite;

namespace BDI3Mobile.Models.Responses
{
    public class OrganizationRecordForms
    {
        [JsonProperty("productID")]
        public int ProductID { get; set; }

        [JsonProperty("assessmentId")]
        public int AssessmentID { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("lockTestRecord")]
        public int LockTestRecord { get; set; }

        [JsonProperty("userid")]
        public string UserID { get; set; }

        [JsonProperty("toDate")]
        public string ToDate { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("internalUserId")]
        public string InternalUserId { get; set; }

        [JsonProperty("internalCaseloadId")]
        public string InternalCaseloadId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("assessmentIds")]
        public string AssessmentIds { get; set; }

        [JsonProperty("caseFolderId")]
        public string CaseFolderId { get; set; }

        [Ignore]
        public bool IsChecked { get; set; }
        public int DownloadedBy { get; set; }

        public int OrganizationId { get; set; }

    }
}
