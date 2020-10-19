using SQLite;
using System;

namespace BDI3Mobile.Models.DBModels.DigitalTestRecord
{
    public class StudentTestFormResponses
    {
        public int LocalTestFormResponseID { get; set; }
        public int LocalFormInstanceId { get; set; }
        [Ignore]
        public int UserId { get; set; }
        public int ContentCategoryId { get; set; }
        public int? SectionId { get; set; }
        public string Response { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int? SuggestedStartingPointId { get; set; }
    }
}
