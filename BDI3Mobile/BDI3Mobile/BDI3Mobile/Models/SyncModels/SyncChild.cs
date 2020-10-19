using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BDI3Mobile.Models.SyncModels
{
    public class SyncChild
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("localChildUserId")]
        public int LocalChildUserId { get; set; }

        [JsonProperty("userId")]
        public int? UserId { get; set; }

        [JsonProperty("organizationId")]
        public int OrganizationId { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("genderId")]
        public int GenderId { get; set; }

        [JsonProperty("dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("locationId")]
        public int LocationId { get; set; }

        [JsonProperty("childId")]
        public string ChildId { get; set; }

        [JsonProperty("enrollmentDate")]
        public string EnrollmentDate { get; set; }

        [JsonProperty("parentGuardianName1")]
        public string ParentGuardianName1 { get; set; }

        [JsonProperty("parentGuardianEmail1")]
        public string ParentGuardianEmail1 { get; set; }

        [JsonProperty("parentGuardianName2")]
        public string ParentGuardianName2 { get; set; }

        [JsonProperty("parentGuardianEmail2")]
        public string ParentGuardianEmail2 { get; set; }

        [JsonProperty("primaryLanguageId")]
        public int? PrimaryLanguageId { get; set; }

        [JsonProperty("raceId")]
        public List<int> RaceIds { get; set; }

        [JsonProperty("ethnicityId")]
        public int? EthnicityId { get; set; }

        [JsonProperty("ifsp")]
        public bool Ifsp { get; set; }

        [JsonProperty("iep")]
        public bool Iep { get; set; }

        [JsonProperty("fundingResourceIds")]
        public List<int> FundingResourceIds { get; set; }

        [JsonProperty("freeLunch")]
        public bool FreeLunch { get; set; }

        [JsonProperty("createdDate")]
        public string CreatedDate { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("skipUpdate")]
        public bool SkipUpdate { get; set; }

        [JsonProperty("researchCodeNames")]
        public List<SyncResearchCodeName> researchCodeNames { get; set; }

        [JsonProperty("researchCodeValues")]
        public List<SyncResearchCodeValue> ResearchCodeValues { get; set; }

        [JsonProperty("testRecords")]
        public List<SyncTestRecord> TestRecords { get; set; }

        [JsonProperty("ifspEligibilityDate")]
        public string IfspEligibilityDate { get; set; }

        [JsonProperty("ifspExitDate")]
        public string IfspExitDate { get; set; }

        [JsonProperty("iepEligibilityDate")]
        public string IepEligibilityDate { get; set; }

        [JsonProperty("iepExitDate")]
        public string IepExitDate { get; set; }

        [JsonProperty("primaryDiagnosesId")]
        public int? PrimaryDiagnosesId { get; set; }

        [JsonProperty("secondaryDiagnosesId")]
        public int? SecondaryDiagnosesId { get; set; }
    }
    public class SyncTestRecord
    {
        [JsonProperty("localFormInstanceId")]
        public int LocalFormInstanceId { get; set; }

        [JsonProperty("assessmentId")]
        public int AssessmentId { get; set; }

        [JsonProperty("formParameters")]
        public string FormParameters { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("additionalNotes")]
        public string AdditionalNotes { get; set; }

        [JsonProperty("createdByUserId")]
        public int CreatedByUserId { get; set; }

        [JsonProperty("createDate")]
        public string CreateDate { get; set; }

        [JsonProperty("contentCategories")]
        public List<SyncContentCategory> ContentCategories { get; set; }
    }


    public class SyncContentCategory
    {
        [JsonProperty("contentCategoryId")]
        public int ContentCategoryId { get; set; }

        [JsonProperty("examinerId")]
        public int ExaminerId { get; set; }

        [JsonProperty("testDate")]
        public string TestDate { get; set; }

        [JsonProperty("order")]
        public int Order { get; set; }

        [JsonProperty("itemScore")]
        public int ItemScore { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("rawScore")]
        public int? RawScore { get; set; }

        [JsonProperty("createDate")]
        public string CreateDate { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("ageInMonths")]
        public int? AgeInMonths { get; set; }

        [JsonProperty("itemLevelResponse")]
        public SyncItemLevelResponse ItemLevelResponse { get; set; }

        [JsonProperty("timeTaken")]
        public int? TimeTaken { get; set; }


    }

    public class SyncItemLevelResponse
    {
        [JsonProperty("sectionId")]
        public int? SectionId { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("createdOn")]
        public string CreatedOn { get; set; }

        [JsonProperty("createdBy")]
        public int CreatedBy { get; set; }
        [JsonProperty("suggestedStartingPoint")]
        public int? SuggestedStartingPoint { get; set; }

    }

    public class SyncResearchCodeValue
    {
        [JsonProperty("researchCodeValueId")]
        public int ResearchCodeValueId { get; set; }

        [JsonProperty("researchCodeId")]
        public int ResearchCodeId { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class SyncResearchCodeName
    {
        [JsonProperty("researchCodeId")]
        public int ResearchCodeId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class SyncTestRecordResult
    {
        [JsonProperty("childUserId")]
        public int? ChildUserId { get; set; }

        [JsonProperty("formInstanceId")]
        public Guid? FormInstanceId { get; set; }

        [JsonProperty("localFormInstanceId")]
        public int LocalFormInstanceId { get; set; }

        [JsonProperty("statusCode")]
        public SyncTestRecordStatusCode StatusCode { get; set; }

        [JsonProperty("researchCodeValues")]
        public List<SyncResearchCodeValue> ResearchCodeValues { get; set; }
        [JsonProperty("localChildUserId")]
        public int LocalChildUserId { get; set; }
    }
    public enum SyncTestRecordStatusCode
    {
        Unknown = 0,
        Success = 1,
        ChildDoesNotExists = 2,
        ChildIsDeleted = 3,
        InvalidAssessment = 4,
        InvalidLocation = 5,
        LocationIsDeleted = 6,
        InvalidRace = 7,
        InvalidGender = 8,
        InvalidEthnicity = 9,
        InvalidLanguage = 10,
        InvalidTestRecordCreator = 11,
        InvalidFundingSource = 12,
        InvalidContentCategory = 13,
        InvalidExaminer = 14,
        InvalidTestRecordItemLevelCreator = 15,
        InvalidUser = 16,
        EmptyRecordForm = 17,
        NoLicenseAvailable = 18,
        MissingExaminerId = 19,
        MissingTestRecordItemLevelCreator = 20,
        InvalidAge7YearsAndMore = 21,
        MissingTestRecord = 22,
        NoSufficientLicense = 23,
        UnableToValidateTestRecord = 24,
        UnableToSaveChild = 25,
        UnableToSaveResearchCodes = 26,
        UnableToSaveTestRecord = 27,
        InvalidResponseData = 28
    }
}
