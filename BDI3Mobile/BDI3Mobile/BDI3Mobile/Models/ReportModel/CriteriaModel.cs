namespace BDI3Mobile.Models.ReportModel
{
    public class CriteriaModel
    {
        public int reportType { get; set; }
        public string programLabelId { get; set; }
        public string locationIds { get; set; }
        public string childUserId { get; set; }
        public int batteryType { get; set; }
        public string forminstanceIds { get; set; }
        public bool isIncludeDomain { get; set; }
        public bool isItemScores { get; set; }
        public bool isSuggestActivities { get; set; }
        public string notes { get; set; }
        public string selectedScores { get; set; }
        public double selectedDeviations { get; set; }
        public int outputFormat { get; set; }


    }
}
