using BDI3Mobile.Models;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Models.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface ICommonDataService
    {
        Action ClearAddChildContent { get; set; }
        string DOB { get; set; }
        List<StudentTestForms> StudentTestForms { get; set; }
        int TotalAgeinMonths { get; set; }
        int LocaInstanceID { get; set; }
        bool IsCompleteForm { get; set; }
        bool IsScreenerForm { get; set; }
        bool IsAcademicForm { get; set; }
        Task GenerateData();
        List<ProgramNoteModel> ProgramNoteModels { get; set; }
        List<SearchStaffResponse> SearchStaffResponseModel { get; set; }
        List<OrganizationRecordForms> OrgRecordFormList { get; set; }
        List<ContentCategory> TotalCategories { get; set; }
        List<ContentCategory> BattleCategories { get; set; }
        List<ContentCategory> ScreenerCategories { get; set; }
        List<ContentCategory> AcademicCategories { get; set; }

        List<MenuContentModel> BattleDevelopmentMenuList { get; set; }
        List<MenuContentModel> ScreenerMenuList { get; set; }
        List<MenuContentModel> AcademicMenuList { get; set; }

        Task GenerateAcademicMentList();
        Task GenerateBattleMenuList();
        Task GenerateScreenerMenuList();

        #region MasterContent
        List<ContentGroup> ContentGroups { get; set; }
        List<ContentGroupItem> ContentGroupItems { get; set; }
        List<ContentBasalCeilings> ContentBasalCeilings { get; set; }
        List<ContentCategoryItem> ContentCategoryItems { get; set; }
        List<ContentItemAttribute> ContentItemAttributes { get; set; }
        List<ContentItemTallyScore> ContentItemTallyScores { get; set; }
        List<ContentItemTally> ContentItemTallies { get; set; }
        List<ContentRubricPoint> ContentRubricPoints { get; set; }
        List<ContentRubric> ContentRubrics { get; set; }
        List<ContentItem> ContentItems { get; set; }
        List<ContentCategoryLevel> ContentCategoryLevels { get; set; }
        #endregion
        Action ResetTestDate { get; set; }
        StudentTestFormOverview StudentTestFormOverview { get; set; }

        List<Race> Races { get; set; }
        List<Language> Languages { get; set; }
        List<Diagnostics> PrimaryDiagnostics { get; set; }
        List<Diagnostics> SecondaryDiagnostics { get; set; }
        List<FundingSource> fundingSources { get; set; }

        Action<string> ObservationNotes { get; set; }
    }
}
