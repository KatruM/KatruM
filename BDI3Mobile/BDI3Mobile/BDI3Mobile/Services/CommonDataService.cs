using BDI3Mobile.IServices;
using BDI3Mobile.Models;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Models.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class CommonDataService : ICommonDataService
    {
        private readonly IOrgRecordFormService _orgRecordFormService;
        private readonly IExaminerService examinerService;
        private readonly IProgramNoteService programNoteService;
        private readonly IContentCategoryService _contentcategoryservice;
        public CommonDataService()
        {
            examinerService = DependencyService.Get<IExaminerService>();
            programNoteService = DependencyService.Get<IProgramNoteService>();
            _orgRecordFormService = DependencyService.Get<IOrgRecordFormService>();
            _contentcategoryservice = DependencyService.Get<IContentCategoryService>();
        }
        private List<StudentTestForms> studentTestForms;
        public List<StudentTestForms> StudentTestForms
        {
            get
            {
                return studentTestForms;
            }
            set
            {
                if (value != null)
                {
                    studentTestForms = new List<StudentTestForms>(value);
                }
                else
                {
                    studentTestForms = new List<StudentTestForms>();
                }
            }
        }
        public int TotalAgeinMonths { get; set; }
        public int LocaInstanceID { get; set; }
        public bool IsCompleteForm { get; set; }
        public bool IsScreenerForm { get; set; }
        public bool IsAcademicForm { get; set; }
        public List<ProgramNoteModel> ProgramNoteModels { get; set; }
        public List<SearchStaffResponse> SearchStaffResponseModel { get; set; }
        public List<OrganizationRecordForms> OrgRecordFormList { get; set; }
        public List<ContentCategory> TotalCategories { get; set; }
        public List<ContentCategory> BattleCategories { get; set; }
        public List<ContentCategory> ScreenerCategories { get; set; }
        public List<ContentCategory> AcademicCategories { get; set; }
        public List<MenuContentModel> BattleDevelopmentMenuList { get; set; }
        public List<MenuContentModel> ScreenerMenuList { get; set; }
        public List<MenuContentModel> AcademicMenuList { get; set; }
        public List<ContentGroup> ContentGroups { get; set; }
        public List<ContentGroupItem> ContentGroupItems { get; set; }
        public List<ContentBasalCeilings> ContentBasalCeilings { get; set; }
        public List<ContentCategoryItem> ContentCategoryItems { get; set; }
        public List<ContentItemAttribute> ContentItemAttributes { get; set; }
        public List<ContentItemTallyScore> ContentItemTallyScores { get; set; }
        public List<ContentItemTally> ContentItemTallies { get; set; }
        public List<ContentRubricPoint> ContentRubricPoints { get; set; }
        public List<ContentRubric> ContentRubrics { get; set; }
        public List<ContentItem> ContentItems { get; set; }
        public List<ContentCategoryLevel> ContentCategoryLevels { get; set; }

        public List<Race> Races { get; set; }
        public List<Language> Languages { get; set; }
        public List<Diagnostics> PrimaryDiagnostics { get; set; }
        public List<Diagnostics> SecondaryDiagnostics { get; set; }
        public List<FundingSource> fundingSources { get; set; }

        public async Task GenerateData()
        {
            await Task.Delay(0);
            ProgramNoteModels = programNoteService.GetProgramNote() ?? new List<ProgramNoteModel>();
            SearchStaffResponseModel = examinerService.GetExamainer() ?? new List<SearchStaffResponse>();
            OrgRecordFormList = _orgRecordFormService.GetRecordForms() ?? new List<OrganizationRecordForms>();
            if (TotalCategories != null && TotalCategories.Any())
            {
                GenerateBattleCategories();
                GenerateScreenerCategories();
                GenerateAcademicCategories();
            }
        }
        private void GenerateBattleCategories()
        {
            BattleCategories = TotalCategories.Where(p => p.contentCategoryLevelId == 1 || p.contentCategoryLevelId == 2).ToList() ?? new List<ContentCategory>();
        }
        private void GenerateScreenerCategories()
        {
            ScreenerCategories = TotalCategories.Where(p => p.contentCategoryLevelId == 4).ToList() ?? new List<ContentCategory>();
        }
        private void GenerateAcademicCategories()
        {
            AcademicCategories = TotalCategories.Where(p => p.contentCategoryLevelId == 6 || p.contentCategoryLevelId == 7 || p.contentCategoryLevelId == 8).ToList() ?? new List<ContentCategory>();
        }

        public async Task GenerateAcademicMentList()
        {
            var localList = new List<MenuContentModel>();
            await Task.Delay(0);
            if (TotalCategories != null && TotalCategories.Any())
            {
                var sequence = 0;
                foreach (var item in TotalCategories.OrderBy(p => p.sequenceNo))
                {
                    sequence += 1;
                    var MenuContentModel = new MenuContentModel();
                    if (item.contentCategoryLevelId == 6)
                    {
                        MenuContentModel.ContentLevelID = item.contentCategoryLevelId;
                        MenuContentModel.ContentCatgoryId = item.contentCategoryId;
                        MenuContentModel.Code = item.code;
                        MenuContentModel.Text = item.name;
                        MenuContentModel.ShowImage = true;
                        MenuContentModel.SequenceNumber = sequence;
                        //MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        localList.Add(MenuContentModel);
                    }
                    else if (item.contentCategoryLevelId == 7 || item.contentCategoryLevelId == 8)
                    {
                        MenuContentModel.ContentLevelID = item.contentCategoryLevelId;
                        MenuContentModel.ContentCatgoryId = item.contentCategoryId;
                        MenuContentModel.Code = item.code;
                        MenuContentModel.Text = item.name + " (" + item.code + ")";
                        MenuContentModel.ShowImage = true;
                        MenuContentModel.ParentID = item.parentContentCategoryId;
                        MenuContentModel.SequenceNumber = sequence;
                        //MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        localList.Add(MenuContentModel);
                    }
                    else if (item.contentCategoryLevelId == 9)
                    {
                        if (ContentCategoryItems != null && ContentCategoryItems.Any())
                        {
                            var filteredContentCategoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == item.contentCategoryId).ToArray();
                            if (filteredContentCategoryItems.Length > 0)
                            {
                                if (ContentItems != null && ContentItems.Any())
                                {
                                    var filteredContentItems = ContentItems.Where(p => filteredContentCategoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => !p.sampleItem).ThenBy(p => p.sequenceNo).ToArray();
                                    if (filteredContentItems.Length > 0)
                                    {
                                        var startingpoint = new MenuContentModel();
                                        startingpoint.Code = filteredContentItems.FirstOrDefault().itemCode;
                                        startingpoint.ContentItemId = filteredContentItems.FirstOrDefault().contentItemId;
                                        startingpoint.Text = "Starting Point for developmental ages " + item.name;
                                        startingpoint.ContentCatgoryId = item.contentCategoryId;
                                        startingpoint.ParentID = item.parentContentCategoryId;
                                        startingpoint.ContentLevelID = item.contentCategoryLevelId;
                                        startingpoint.IsStartingPoint = true;
                                        startingpoint.SequenceNumber = sequence;
                                        //startingpoint.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                        localList.Add(startingpoint);
                                        foreach (var contentItem in filteredContentItems)
                                        {
                                            sequence += 1;
                                            var menuItem = new MenuContentModel();
                                            menuItem.ContentLevelID = item.contentCategoryLevelId;
                                            menuItem.ContentCatgoryId = item.contentCategoryId;
                                            menuItem.ParentID = item.parentContentCategoryId;
                                            menuItem.Code = contentItem.itemCode;
                                            menuItem.Text = contentItem.alternateItemText;
                                            menuItem.ContentItemId = contentItem.contentItemId;
                                            menuItem.SequenceNumber = sequence;
                                            menuItem.HtmlFilePath = contentItem.HtmlFilePath;
                                            //menuItem.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                            localList.Add(menuItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            AcademicMenuList = new List<MenuContentModel>(localList);
        }

        public async Task GenerateBattleMenuList()
        {
            
            var localList = new List<MenuContentModel>();
            await Task.Delay(0);
            if (TotalCategories != null && TotalCategories.Any())
            {
                var sequence = 0;
                foreach (var item in TotalCategories)
                {
                    sequence += 1;
                    var MenuContentModel = new MenuContentModel();
                    if (item.contentCategoryLevelId == 1)
                    {
                        MenuContentModel.ContentLevelID = item.contentCategoryLevelId;
                        MenuContentModel.ContentCatgoryId = item.contentCategoryId;
                        MenuContentModel.Code = item.code;
                        MenuContentModel.Text = item.name;
                        MenuContentModel.ShowImage = true;
                        MenuContentModel.SequenceNumber = sequence;
                        // MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        localList.Add(MenuContentModel);
                    }
                    else if (item.contentCategoryLevelId == 2)
                    {
                        MenuContentModel.ContentLevelID = item.contentCategoryLevelId;
                        MenuContentModel.ContentCatgoryId = item.contentCategoryId;
                        MenuContentModel.Code = item.code;
                        MenuContentModel.Text = item.name + " (" + item.code + ")";
                        MenuContentModel.ShowImage = true;
                        MenuContentModel.ParentID = item.parentContentCategoryId;
                        MenuContentModel.SequenceNumber = sequence;
                        //MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        localList.Add(MenuContentModel);
                    }
                    else if (item.contentCategoryLevelId == 3)
                    {
                        if (ContentCategoryItems != null && ContentCategoryItems.Any())
                        {
                            var filteredContentCategoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == item.contentCategoryId).ToArray();
                            if (filteredContentCategoryItems.Length > 0)
                            {
                                if (ContentItems != null && ContentItems.Any())
                                {
                                    var filteredContentItems = ContentItems.Where(p => filteredContentCategoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).ToArray();
                                    if (filteredContentItems.Length > 0)
                                    {
                                        var startingpoint = new MenuContentModel();
                                        startingpoint.Code = filteredContentItems.FirstOrDefault().itemCode;
                                        startingpoint.ContentItemId = filteredContentItems.FirstOrDefault().contentItemId;
                                        startingpoint.Text = "Starting Point for developmental ages " + item.name;
                                        startingpoint.ContentCatgoryId = item.contentCategoryId;
                                        startingpoint.ParentID = item.parentContentCategoryId;
                                        startingpoint.ContentLevelID = item.contentCategoryLevelId;
                                        startingpoint.IsStartingPoint = true;
                                        startingpoint.SequenceNumber = sequence;
                                        //startingpoint.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                        localList.Add(startingpoint);
                                        foreach (var contentItem in filteredContentItems)
                                        {
                                            sequence += 1;
                                            var menuItem = new MenuContentModel();
                                            menuItem.ContentLevelID = item.contentCategoryLevelId;
                                            menuItem.ContentCatgoryId = item.contentCategoryId;
                                            menuItem.ParentID = item.parentContentCategoryId;
                                            menuItem.Code = contentItem.itemCode;
                                            menuItem.Text = !string.IsNullOrEmpty(contentItem.alternateItemText) ? contentItem.alternateItemText : contentItem.itemText;
                                            //menuItem.Text = contentItem.itemText;
                                            menuItem.ContentItemId = contentItem.contentItemId;
                                            menuItem.SequenceNumber = sequence;
                                            menuItem.HtmlFilePath = contentItem.HtmlFilePath;
                                            //menuItem.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                            localList.Add(menuItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            BattleDevelopmentMenuList = new List<MenuContentModel>(localList);
        }

        public async Task GenerateScreenerMenuList()
        {
            var localList = new List<MenuContentModel>();
            await Task.Delay(0);
            if (TotalCategories != null && TotalCategories.Any())
            {
                var sequence = 0;
                foreach (var item in TotalCategories)
                {
                    sequence += 1;
                    var MenuContentModel = new MenuContentModel();
                    if (item.contentCategoryLevelId == 4)
                    {
                        MenuContentModel.ShowStatus = true;
                        MenuContentModel.ContentLevelID = item.contentCategoryLevelId;
                        MenuContentModel.ContentCatgoryId = item.contentCategoryId;
                        MenuContentModel.Code = item.name;
                        MenuContentModel.Text = item.name;
                        MenuContentModel.ShowImage = true;
                        MenuContentModel.SequenceNumber = sequence;
                        //MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        localList.Add(MenuContentModel);
                    }
                    else if (item.contentCategoryLevelId == 5)
                    {
                        if (ContentCategoryItems != null && ContentCategoryItems.Any())
                        {
                            var filteredContentCategoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == item.contentCategoryId).ToArray();
                            if (filteredContentCategoryItems.Length > 0)
                            {
                                if (ContentItems != null && ContentItems.Any())
                                {
                                    var filteredContentItems = ContentItems.Where(p => filteredContentCategoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).ToArray();
                                    if (filteredContentItems.Length > 0)
                                    {
                                        var startingpoint = new MenuContentModel();
                                        startingpoint.Code = filteredContentItems.FirstOrDefault().itemCode;
                                        startingpoint.ContentItemId = filteredContentItems.FirstOrDefault().contentItemId;
                                        startingpoint.Text = "Starting Point for developmental ages " + item.name;
                                        startingpoint.ContentCatgoryId = item.contentCategoryId;
                                        startingpoint.ParentID = item.parentContentCategoryId;
                                        startingpoint.ContentLevelID = item.contentCategoryLevelId;
                                        startingpoint.IsStartingPoint = true;
                                        startingpoint.SequenceNumber = sequence;
                                        //startingpoint.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                        localList.Add(startingpoint);
                                        foreach (var contentItem in filteredContentItems)
                                        {
                                            sequence += 1;
                                            var menuItem = new MenuContentModel();
                                            menuItem.ContentLevelID = item.contentCategoryLevelId;
                                            menuItem.ContentCatgoryId = item.contentCategoryId;
                                            menuItem.ParentID = item.parentContentCategoryId;
                                            menuItem.Code = contentItem.itemCode;
                                            menuItem.Text = !string.IsNullOrEmpty(contentItem.alternateItemText) ? contentItem.alternateItemText : contentItem.itemText;
                                            menuItem.ContentItemId = contentItem.contentItemId;
                                            menuItem.SequenceNumber = sequence;
                                            menuItem.HtmlFilePath = contentItem.HtmlFilePath;
                                            //menuItem.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                            localList.Add(menuItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            ScreenerMenuList = new List<MenuContentModel>(localList);

        }

        public Action ResetTestDate { get; set; }
        public StudentTestFormOverview StudentTestFormOverview { get; set; }
        public Action<string> ObservationNotes { get; set; }
        public string DOB { get; set; }

        public Action ClearAddChildContent { get; set; }
    }
}
