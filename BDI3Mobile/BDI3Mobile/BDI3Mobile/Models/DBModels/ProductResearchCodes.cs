using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class ProductResearchCodes
    {
        [PrimaryKey]
        public int ResearchCodeId { get; set; }
        public int OrganizationId { get; set; }
        public string ValueName { get; set; }
        public int Sequence { get; set; }
        public int ProductID { get; set; }
        public string ModifiedDate { get; set; }
        public int DownloadedBy { get; set; }
    }
    public class ProductResearchCodeValues
    {
        public string ModifiedDate { get; set; }
        public int DownloadedBy { get; set; }
        public int ResearchCodeValueId { get; set; }
        public int ResearchCodeId { get; set; }
        public int OrganizationId { get; set; }
        public string value { get; set; }
        public int UserId { get; set; }
        public int OfflineStudentID { get; set; }
        public int ProductID { get; set; }
        [Ignore]
        public ProductResearchCodes ResearchCode { get; set; }
    }
}
