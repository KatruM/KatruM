using System;
using Newtonsoft.Json;
using SQLite;

namespace BDI3Mobile.Models.DBModels
{
    public class Students
    {
        [PrimaryKey,AutoIncrement]
        public int OfflineStudentID { get; set; }
        public string StudentID { get; set; }
        public int CustomerID { get; set; }
        public DateTime DeleteDate { get; set; }
        public string StudentGuid { get; set; }
        public string ChildID { get; set; }
        public DateTime Birthdate { get; set; }
        public int DisabilityID { get; set; }
        public string UserId { get; set; }
        public string ParentEmailAddress1 { get; set; }
        public string ParentEmailAddress2 { get; set; }
        public DateTime IFSPExitDate { get; set; }
        public DateTime IFSPEligibilityDate { get; set; }
        public DateTime IEPExitDate { get; set; }
        public DateTime IEPEligibilityDate { get; set; }
        public byte IsIFSP { get; set; }
        public byte IsIEP { get; set; }
        public byte IsFreeLunch { get; set; }
        public string ParentGuardian1 { get; set; }
        public string ParentGuardian2 { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddedBy { get; set; }
        public bool IsSynced { get; set; }
        public string MiddleName { get; set; }
        public int Gender { get; set; }
        public string SelectedLanguageIds { get; set; }
        public string SelectedEthnictyIds { get; set; }
        public string SelectedFundingSourceIds { get; set; }
        public string SelectedPrimaryDiagnosesIds { get; set; }
        public string SelectedSecondaryDiagnosesIds { get; set; }
        public string SelectedRaceIds { get; set; }
        public int? SelectedLocationId { get; set; }
        public string LocationName { get; set; }
        public int OrgId { get; set; }
        public int isDeleteStatus { get; set; }
        public string updatedOn { get; set; }
        public string updatedOnUTC { get; set; }
        public int DownloadedBy { get; set; }

        [JsonIgnore, Ignore]
        public string LocalTestFormIds { get; set; }
    }
}
