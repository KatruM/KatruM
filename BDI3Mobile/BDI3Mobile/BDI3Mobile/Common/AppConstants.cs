namespace BDI3Mobile.Common
{
    public class AppConstants
    {
        public AppConstants()
        {
        }
        public static readonly string UserID = "UserID";
        public static readonly string Token = "Token";
        public static readonly string OK = "OK";
        public static readonly string Cancel = "Cancel";
    }

    public class APIConstants
    {
        public APIConstants() { }
        //public static readonly string BaseUri = "http://clinical.qa.rsiapps.com/";
        //public static readonly string LoginUri = "identity/api/authentication/login";
        //public static readonly string ForgotPasswordUri = "identity/api/Authentication/ForgotPassword";
        //public const string ResearchCodesUri = "Capi/api/UserInfo/GetOrgResearchCodes";
        //public static readonly string AddChildUri = "Capi/api/child/save";
        //public static readonly string LatestUpdatesUri = "Capi/api/DataSync/LatestUpdates";
        //public static readonly string SearchChildUri = "Capi/api/Child/Search";
        //public static readonly string StudentInformationUri = "Capi/api/Child/GetStudentInformation/{0}";
        //public static readonly string StudentDetailsUri = "Capi/api/Child/GetStudentDetails/{0}";
        //public static readonly string SearchStaffUri = "Capi/api/StaffInfo/SearchExaminers";
        //public static readonly string GetProgramNote = "Capi/api/Organization/GetProgramLabels";
        //public static readonly string GetLocationUri = "capi/api/Organization/GetParentLocations?";
        //public static readonly string GetDomainUri = "Capi/api/content/GetModifiedContentListByProductAssessment?ProductId=10&AssessmentId=0";
        //public static readonly string GetImageUri = "Capi/api/content/DownloadContentImages?ProductId=10&AssessmentId=0";
        //public static readonly string GetOrgRecordForms = "Capi/api/Organization/GetOrgRecordForms";
        //public static readonly string GetCommonData = "Capi/api/DataSync/GetCommonData";
        //public static readonly string GetReportParameters = "Capi/api/reportcenter/ReportCenterData";
        //public static readonly string GetLocations = "Capi/api/organization/GetHierarchyLocations";
        //public static readonly string GetChildren = "Capi/api/child/GetChildrenByLocation";
        //public static readonly string GetBatteryTypes = "Capi/api/reportcenter/BatteryTypesByUserId";
        //public static readonly string SaveReportCriteria = "Capi/api/reportcenter/SaveReportCriteria";
        //public static readonly string ExecuteReport = "Capi/api/reportcenter/ExecuteReport";
        //public static readonly string SyncTestRecord = "Capi/api/DataSync/SyncTestRecord";



        public static readonly string BaseUri = "https://stage.riversidescore.com/";
        public static readonly string LoginUri = "IdentityApi/api/authentication/login";
        public static readonly string ForgotPasswordUri = "IdentityApi/api/Authentication/ForgotPassword";
        public const string ResearchCodesUri = "ClinicalApi/api/UserInfo/GetOrgResearchCodes";
        public static readonly string AddChildUri = "ClinicalApi/api/child/save";
        public static readonly string LatestUpdatesUri = "ClinicalApi/api/DataSync/LatestUpdates";
        public static readonly string SearchChildUri = "ClinicalApi/api/Child/Search";
        public static readonly string StudentInformationUri = "ClinicalApi/api/Child/GetStudentInformation/{0}";
        public static readonly string StudentDetailsUri = "ClinicalApi/api/Child/GetStudentDetails/{0}";
        public static readonly string SearchStaffUri = "ClinicalApi/api/StaffInfo/SearchExaminers";
        public static readonly string GetProgramNote = "ClinicalApi/api/Organization/GetProgramLabels";
        public static readonly string GetLocationUri = "ClinicalApi/api/Organization/GetParentLocations?";
        public static readonly string GetDomainUri = "ClinicalApi/api/content/GetModifiedContentListByProductAssessment?ProductId=10&AssessmentId=0";
        public static readonly string GetImageUri = "ClinicalApi/api/content/DownloadContentImages?ProductId=10&AssessmentId=0";
        public static readonly string GetOrgRecordForms = "ClinicalApi/api/Organization/GetOrgRecordForms";
        public static readonly string GetCommonData = "ClinicalApi/api/DataSync/GetCommonData";
        public static readonly string GetReportParameters = "ClinicalApi/api/reportcenter/ReportCenterData";
        public static readonly string GetLocations = "ClinicalApi/api/organization/GetHierarchyLocations";
        public static readonly string GetChildren = "ClinicalApi/api/child/GetChildrenByLocation";
        public static readonly string GetBatteryTypes = "ClinicalApi/api/reportcenter/BatteryTypesByUserId";
        public static readonly string SaveReportCriteria = "ClinicalApi/api/reportcenter/SaveReportCriteria";
        public static readonly string ExecuteReport = "ClinicalApi/api/reportcenter/ExecuteReport";
        public static readonly string SyncTestRecord = "ClinicalApi/api/DataSync/SyncTestRecord";


    }

    public class APIResponseCodes
    {
        public APIResponseCodes() { }
        public static readonly int Success = 200;
    }
    public class Auth
    {
        public static readonly string AuthError = "Auth Failure";
        public static readonly string AuthMessage = "Authentication Failed. Please check your username or password";
    }
    public class ErrorMessages
    {
        public static readonly string NetworkError = "Network Error";
        public static readonly string RecordMatchesFoundMessage = "Search criteria did not return any results";
        public static readonly string RecordsFoundMessage = "{0} {1} found";
        public static readonly string SearchError = "Search returned over 1000 children. Please narrow your Search criteria.";
        public static readonly string NetworkErrorMessage = "Username and/or password are incorrect. Please establish an internet \nconnection to retrieve your password or contact Riverside support.";
        public static readonly string DuplicateChildMessage1 = "Child {0} {1}, DOB {2} already exists. The child was originally created by Account Holder {3} {4}.";
        public static readonly string DuplicateChildMessage2 = "Please click Cancel and contact {0} {1} or your Account Holder or click Continue to create a second version of this child.";
    }
    public class EncryptionKeys
    {
        public static readonly string GUID = "b8f304afbb8c43b3aa388e4c1994edab";
    }

    public class AssignmentTypes
    {
        public static readonly string BattelleDevelopmentalCompleteString = "BDI-3 Developmental Record Form";
        public static readonly string BattelleDevelopmentScreenerString = "BDI-3 Screening Test Record Form";
        public static readonly string BattelleEarlyAcademicSurveyString = "Battelle Early Academic Survey Record Form";

        public static readonly int BattelleDevelopmentalCompleteID = 40;
        public static readonly int BattelleDevelopmentalScreenerID = 42;
        public static readonly int BattelleDevelopmentalAcademicSurveyID = 43;
    }

    public class NumericConstants
    {
        public static readonly int CaptureModeWebViewHeight = 150;
        public static readonly int ScoringWebViewHeight = 63;
        public static readonly int CaptureModeWebViewHeightRange = 1000;
        public static readonly int ScoringWebViewHeightRange = 200;
    }

    public enum ContentTypes
    {
        Content,
        Images
    }
}
    