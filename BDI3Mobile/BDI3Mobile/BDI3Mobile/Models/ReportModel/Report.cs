using System.Collections.Generic;

namespace BDI3Mobile.Models.ReportModel
{
    public class ReportTypes
    {
        public int value { get; set; }
        public string text { get; set; }
        public bool selected { get; set; }
        public bool disabled { get; set; }
    }

    public class ProgramLabel
    {
        public int labelId { get; set; }
        public string labelname { get; set; }
        public bool selected { get; set; }
        public bool disabled { get; set; }
    }
    public class ReportTypeProgramLabelResponse
    {
        public List<ReportTypes> reportTypes;
        public List<ProgramLabel> programLabels;
    }
    public class ReportLocations
    {
        public int LocationID { get; set; }
        public string ParentLocationID { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public bool canHaveChildren { get; set; }
        public int depth { get; set; }
        public bool enabled { get; set; }
        public List<ReportLocations> children { get; set; }

    }
    public class ReportChild
    {
        public string lastName  { get; set; }
        public string firstName { get; set; }
        public string middleName { get; set; }
        public string userId  { get; set; }
    }
   
    public class ReportRecordForms
    {
        public string formInstanceId { get; set; }
        public int assessmentId { get; set; }
        public string description { get; set; }
        public string programLabelId { get; set; }
    }
    public class ResponseCriteria
    {
        public string criteriaId { get; set; }
    }
    }
