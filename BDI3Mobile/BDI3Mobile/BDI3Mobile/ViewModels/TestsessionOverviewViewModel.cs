using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Models.Responses;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Views.PopupViews;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using ClinicalScoring.BundleParsers;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;

namespace BDI3Mobile.ViewModels
{
    public class TestSessionOverViewModel : BaseclassViewModel
    {
        public readonly ICommonDataService commonDataService;
        private readonly IClinicalTestFormService clinicalTestFormService;
        public IStudentTestFormsService studentTestFormsService;
        public EventHandler<TestSessionModel> DatePickerSelectedEvent;
        public bool IsProgramLabelClicked { get; set; }
        public string SelectedItemDomainCode { get; set; }
        private TestSessionModel SelectedItem { get; set; }
        public bool IsDevelopmentalBasicReport { get; set; }
        public bool IsScreenerBasicReport { get; set; }
        public bool IsAcademicBasicReport { get; set; }
        public string Title { get; set; }
        public bool ProgramLabelChange { get; set; }
        private string _programLabel;
        public string ProgramLabel
        {
            get { return _programLabel; }
            set
            {
                _programLabel = value;
                OnPropertyChanged(nameof(ProgramLabel));
            }
        }
        private List<ProgramNote> _programNoteList;
        public List<ProgramNote> ProgramNoteList
        {
            get { return _programNoteList; }
            set
            {
                _programNoteList = value;
                OnPropertyChanged(nameof(ProgramNoteList));
            }
        }
        private DateTime _minimumDate = DateTime.Now;
        public DateTime MinimumDate
        {
            get { return _minimumDate; }
            set
            {
                _minimumDate = value;
                OnPropertyChanged(nameof(MinimumDate));
            }
        }
        private DateTime _maximumDate = DateTime.Now;
        public DateTime MaximumDate
        {
            get { return _maximumDate; }
            set
            {
                _maximumDate = value;
                OnPropertyChanged(nameof(MaximumDate));
            }
        }
        private List<Examiner> _examinerList;
        public List<Examiner> ExaminerList
        {
            get { return _examinerList; }
            set
            {
                _examinerList = value;
                OnPropertyChanged(nameof(ExaminerList));
            }
        }
        public bool IsOverviewChanged { get; set; }
        private List<TestSessionModel> _testSessionRecords = new List<TestSessionModel>();
        public List<TestSessionModel> TestSessionRecords
        {
            get
            {
                return _testSessionRecords;
            }

            set
            {
                _testSessionRecords = value;
                OnPropertyChanged(nameof(TestSessionRecords));
            }
        }
        private bool isDateVisible;
        public bool IsDateVisible
        {
            get
            {
                return isDateVisible;
            }
            set
            {
                isDateVisible = value;
                OnPropertyChanged(nameof(IsDateVisible));
            }
        }

        public async void NavigateandRelaodQuestions(string domain)
        {
            var navigationHeaader = new ItemNavigationHeaderData();
            var selectedDomain = TestSessionRecords.Where(p => !string.IsNullOrEmpty(p.DomainCode)).FirstOrDefault(p => p.DomainCode == domain);
            navigationHeaader.HeaderColor = Color.FromHex("#5c2d91");
            if (selectedDomain != null)
            {
                if (selectedDomain.ParentDomainCode.ToUpper() == "ADP")
                {
                    navigationHeaader.HeaderColor = Color.FromHex("#D73648");
                }
                else if (selectedDomain.ParentDomainCode.ToUpper() == "MOT")
                {
                    navigationHeaader.HeaderColor = Color.FromHex("#0066AD");
                }
                else if (selectedDomain.ParentDomainCode.ToUpper() == "COM")
                {
                    navigationHeaader.HeaderColor = Color.FromHex("#5C2D91");
                }
                else if (selectedDomain.ParentDomainCode.ToUpper() == "SE")
                {
                    navigationHeaader.HeaderColor = Color.FromHex("#008550");
                }
                else if (selectedDomain.ParentDomainCode.ToUpper() == "COG")
                {
                    navigationHeaader.HeaderColor = Color.FromHex("#CC4B00");
                }
                else if (selectedDomain.ParentDomainCode == "Academic")
                {
                    // TODO : Need to check for the Academic Domain Because at this time it is not coming from the API 06/01/2020
                    navigationHeaader.HeaderColor = Color.FromHex("#BFD730");
                }
            }
            if (selectedDomain != null)
            {
                navigationHeaader.ContentCategoryId = selectedDomain.ContentCategoryId;
                navigationHeaader.TestDate = selectedDomain.TestDate;
                navigationHeaader.Title = selectedDomain.Domain + " (" + domain + ")";
                navigationHeaader.TestDate = selectedDomain.TestDate;
            }
            await SaveTestSessionOverView();
            MessagingCenter.Send<ItemNavigationHeaderData>(navigationHeaader, "Administrationpage");
        }

        private void NavigateQuestionsForAcademicForm(string domain)
        {
        }

        public ICommand ShowDatePickerCommand
        {
            get
            {
                return new Command((e) =>
                {

                    SelectedItem = (e as TestSessionModel);
                    DatePickerSelectedEvent?.Invoke(null, SelectedItem);
                });
            }
        }
        public ICommand SearchErrorContinueCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAsync();
                });
            }
        }

        public ICommand SelectExaminerCommand
        {
            get
            {
                return new Command((e) =>
                {

                    var item = (e as TestSessionModel);
                    SelectedItemDomainCode = item.DomainCode;
                    PopupNavigation.Instance.PushAsync(new SelectExaminerPopupView(this, new Thickness(0, 0, 0, 0)));
                });
            }
        }
        public virtual void UpdateSelectedExaminer(string selectedText)
        {
            IsOverviewChanged = true;
            TestSessionRecords.Where(p => p.DomainCode == SelectedItemDomainCode).FirstOrDefault().Examiner = selectedText;
            TestSessionRecords.Where(p => p.DomainCode == SelectedItemDomainCode).FirstOrDefault().ExaminerID = Convert.ToInt32(ExaminerList.FirstOrDefault(p => p.text == selectedText)?.id);
        }
        public TestSessionOverViewModel(bool isReport = false)
        {
            IsDevelopmentalBasicReport = isReport;
            if (IsDevelopmentalBasicReport)
            {
                Title = "BDI-3 Developmental Complete Basic Report";
                IsDateVisible = false;
            }
            else
            {
                Title = "Test Session Overview";
                IsDateVisible = true;
            }
            commonDataService = DependencyService.Get<ICommonDataService>();
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            LocalformInstanceId = commonDataService.LocaInstanceID;
            GetExaminerDetails();
            clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
            ProgramLabel = "Select a program label";
            GetProgramNotes();
            if (IsDevelopmentalBasicReport)
            {
                if (ProgramLabel == "Select a program label")
                    ProgramLabel = string.Empty;
            }

            var domains = default(List<ContentCategory>);
            if (commonDataService.IsCompleteForm)
            {
                domains = new List<ContentCategory>(commonDataService.BattleCategories);
                if (domains != null && domains.Any())
                {
                    var parentCategories = domains.Where(p => p.parentContentCategoryId == 0).OrderBy(p => p.sequenceNo);
                    if (parentCategories != null && parentCategories.Any())
                    {
                        var bdi3ScoringParser = Bdi3ScoringParser.GetInstance();
                        foreach (var parentCategory in parentCategories)
                        {
                            TestSessionRecords.Add(new TestSessionModel() { Domain = parentCategory.name, IsDateVisible = false });
                            var childCategories = domains.Where(p => p.parentContentCategoryId == parentCategory.contentCategoryId).OrderBy(p => p.sequenceNo);
                            if (childCategories != null && childCategories.Any())
                            {
                                foreach (var childCategory in childCategories)
                                {
                                    var sessionmodel = new TestSessionModel() { ContentCategoryId = childCategory.contentCategoryId, ParentDomainCode = parentCategory.code, DomainCode = childCategory.code, LoadDomainBasedQuestionsAction = NavigateandRelaodQuestions, Domain = childCategory.name, TestDate = "mm/dd/yyyy", Examiner = "Select Examiner", Status = "Complete", IsDateVisible = true, DomainMargin = new Thickness(17, 0, 0, 0) };
                                    if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
                                    {
                                        var testform = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == childCategory.contentCategoryId);
                                        if (testform != null)
                                        {
                                            sessionmodel.IsScoreSelected = testform.IsScoreSelected;
                                            sessionmodel.ContentCategoryId = childCategory.contentCategoryId;
                                            sessionmodel.TestDate = testform.testDate;
                                            sessionmodel.Notes = testform.Notes;
                                            sessionmodel.Status = testform.TSOStatus;
                                            sessionmodel.RawScore = testform.rawScore != null && testform.rawScore.HasValue ? testform.rawScore.Value.ToString() : "";
                                            sessionmodel.PropertyChanged -= Sessionmodel_PropertyChanged;
                                            sessionmodel.PropertyChanged += Sessionmodel_PropertyChanged;
                                            sessionmodel.EnablerawScore = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == childCategory.contentCategoryId).rawScoreEnabled;
                                            if (testform.rawScore != null && testform.rawScore.HasValue)
                                            {
                                                var aescore = bdi3ScoringParser.GetAeFromRs(Convert.ToDouble(testform.rawScore), null, childCategory.name);
                                                sessionmodel.AE = aescore > 0 ? (aescore / 12 > 0 ? aescore / 12 + " yrs, " : "") + aescore % 12 + " mos" : "--";
                                                var ss = bdi3ScoringParser.GetSsFromRs(Convert.ToDouble(testform.rawScore), childCategory.name, commonDataService.TotalAgeinMonths);
                                                if (ss > ScoringParser.NO_SCORE)
                                                {
                                                    sessionmodel.SS = ss.ToString();
                                                    sessionmodel.PR = bdi3ScoringParser.GetPrFromSs((double)ss)?.FirstOrDefault()?.StringValue;
                                                }
                                                else
                                                {
                                                    sessionmodel.SS = "--";
                                                    sessionmodel.PR = "--";
                                                }
                                            }
                                            if (commonDataService.SearchStaffResponseModel != null && commonDataService.SearchStaffResponseModel.Any() && testform.examinerId != null && testform.examinerId.HasValue)
                                            {
                                                sessionmodel.ExaminerID = Convert.ToInt32(testform.examinerId);
                                                sessionmodel.Examiner = commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.UserID == testform.examinerId.Value.ToString()).FirstNameLastName;
                                            }
                                        }
                                    }
                                    TestSessionRecords.Add(sessionmodel);
                                }
                            }
                        }
                        bdi3ScoringParser = null;
                        GC.Collect();
                        GC.Collect();
                        GC.SuppressFinalize(this);
                    }
                }
            }
            else if (commonDataService.IsScreenerForm)
            {
                domains = new List<ContentCategory>(commonDataService.ScreenerCategories);
                foreach (var item in domains)
                {
                    var sessionModel = new TestSessionModel()
                    {
                        ContentCategoryId = item.contentCategoryId,
                        ParentDomainCode = item.code,
                        DomainCode = item.code,
                        Domain = item.name,
                        TestDate = "mm/dd/yyyy",
                        Examiner = "Select Examiner",
                        Status = "Complete",
                        RawScore = "23",
                        IsDateVisible = true
                    };
                    if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
                    {
                        var testform = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId);
                        if (testform != null)
                        {
                            sessionModel.IsScoreSelected = testform.IsScoreSelected;
                            sessionModel.TestDate = testform.testDate;
                            sessionModel.Notes = testform.Notes;
                            sessionModel.Status = testform.TSOStatus;
                            sessionModel.RawScore = testform.rawScore != null && testform.rawScore.HasValue ? testform.rawScore.Value.ToString() : "";
                            sessionModel.PropertyChanged -= Sessionmodel_PropertyChanged;
                            sessionModel.PropertyChanged += Sessionmodel_PropertyChanged;
                            sessionModel.EnablerawScore = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId).rawScoreEnabled;
                            if (commonDataService.SearchStaffResponseModel != null && commonDataService.SearchStaffResponseModel.Any() && testform.examinerId != null && testform.examinerId.HasValue)
                            {
                                sessionModel.ExaminerID = Convert.ToInt32(testform.examinerId);
                                sessionModel.Examiner = commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.UserID == testform.examinerId.Value.ToString()).FirstNameLastName;
                            }
                        }
                    }
                    Records.Add(sessionModel);
                }
            }
            if (commonDataService.IsAcademicForm)
            {
                domains = domains = new List<ContentCategory>(commonDataService.AcademicCategories);
                var records = new List<TestSessionModel>();
                if (domains != null && domains.Any())
                {
                    var parentDomains = domains.Where(p => p.parentContentCategoryId == 0).OrderBy(p => p.sequenceNo);
                    if (parentDomains != null && parentDomains.Any())
                    {
                        foreach (var item in parentDomains)
                        {
                            records.Add(new TestSessionModel() { Domain = item.name, IsDateVisible = false });
                            var childRecords = domains.Where(p => p.parentContentCategoryId == item.contentCategoryId).OrderBy(p => p.sequenceNo);
                            if (childRecords != null && childRecords.Any())
                            {
                                foreach (var subitem in childRecords)
                                {
                                    var sessionmodel = new TestSessionModel() { DomainVisibility = true, ContentCategoryId = subitem.contentCategoryId, ParentDomainCode = item.code, DomainCode = subitem.code, LoadDomainBasedQuestionsAction = NavigateandRelaodQuestions, Domain = subitem.name, TestDate = "mm/dd/yyyy", Examiner = "Select Examiner", Status = "Complete", IsDateVisible = true, DomainMargin = new Thickness(17, 0, 0, 0) };
                                    records.Add(sessionmodel);
                                    var testform = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == subitem.contentCategoryId);
                                    if (testform != null)
                                    {
                                        sessionmodel.IsScoreSelected = testform.IsScoreSelected;
                                        sessionmodel.ContentCategoryId = subitem.contentCategoryId;
                                        sessionmodel.TestDate = testform.testDate;
                                        sessionmodel.Notes = testform.Notes;
                                        sessionmodel.Status = testform.TSOStatus;
                                        if (sessionmodel.DomainCode == "PA" || sessionmodel.DomainCode == "PWR")
                                        {
                                            sessionmodel.RawScore = "";
                                        }
                                        else
                                        {
                                            sessionmodel.RawScore = testform.rawScore != null && testform.rawScore.HasValue ? testform.rawScore.Value.ToString() : "";
                                        }
                                        sessionmodel.PropertyChanged -= Sessionmodel_PropertyChanged;
                                        sessionmodel.PropertyChanged += Sessionmodel_PropertyChanged;
                                        sessionmodel.EnablerawScore = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == subitem.contentCategoryId).rawScoreEnabled;
                                        if (commonDataService.SearchStaffResponseModel != null && commonDataService.SearchStaffResponseModel.Any() && testform.examinerId != null && testform.examinerId.HasValue)
                                        {
                                            sessionmodel.ExaminerID = Convert.ToInt32(testform.examinerId);
                                            sessionmodel.Examiner = commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.UserID == testform.examinerId.Value.ToString()).FirstNameLastName;
                                        }
                                        if (sessionmodel.ContentCategoryId == 162 || sessionmodel.Domain.ToLower() == "fluency")
                                        {
                                            sessionmodel.IsFluency = true;
                                            if (testform.TimeTaken != null && testform.TimeTaken.HasValue)
                                            {
                                                sessionmodel.Timer = new TimeSpan(0, 0, 0, testform.TimeTaken.Value);
                                            }
                                            else
                                            {
                                                sessionmodel.Timer = new TimeSpan(0, 0, 0, 210);
                                            }
                                        }
                                        else
                                        {
                                            sessionmodel.IsNotFluency = true;
                                        }
                                    }
                                    var areas = domains.Where(p => p.parentContentCategoryId == subitem.contentCategoryId).OrderBy(p => p.sequenceNo);
                                    if (areas != null && areas.Any())
                                    {
                                        sessionmodel.DomainColor = Color.Black;
                                        sessionmodel.IsDateVisible = false;
                                        sessionmodel.DomainVisibility = false;
                                        sessionmodel.LoadDomainBasedQuestionsAction = null;
                                        foreach (var area in areas)
                                        {
                                            var sessionmodel1 = new TestSessionModel() { DomainVisibility = true, ContentCategoryId = area.contentCategoryId, ParentDomainCode = subitem.code, DomainCode = area.code, LoadDomainBasedQuestionsAction = NavigateandRelaodQuestions, Domain = area.name, TestDate = "mm/dd/yyyy", Examiner = "Select Examiner", Status = "Complete", IsDateVisible = true, DomainMargin = new Thickness(27, 0, 0, 0) };
                                            records.Add(sessionmodel1);
                                            var testform1 = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == area.contentCategoryId);
                                            if (testform1 != null)
                                            {
                                                sessionmodel1.IsScoreSelected = testform1.IsScoreSelected;
                                                sessionmodel1.ContentCategoryId = area.contentCategoryId;
                                                sessionmodel1.TestDate = testform1.testDate;
                                                sessionmodel1.Status = testform1.TSOStatus;
                                                sessionmodel1.Notes = testform1.Notes;
                                                sessionmodel1.IsNotFluency = true;
                                                sessionmodel1.RawScore = testform1.rawScore != null && testform1.rawScore.HasValue ? testform1.rawScore.Value.ToString() : "";
                                                sessionmodel1.PropertyChanged -= Sessionmodel_PropertyChanged;
                                                sessionmodel1.PropertyChanged += Sessionmodel_PropertyChanged;
                                                sessionmodel1.EnablerawScore = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == area.contentCategoryId).rawScoreEnabled;
                                                if (commonDataService.SearchStaffResponseModel != null && commonDataService.SearchStaffResponseModel.Any() && testform1.examinerId != null && testform1.examinerId.HasValue)
                                                {
                                                    sessionmodel1.ExaminerID = Convert.ToInt32(testform1.examinerId);
                                                    sessionmodel1.Examiner = commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.UserID == testform1.examinerId.Value.ToString()).FirstNameLastName;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                AcademicTestSessionRecords = new List<TestSessionModel>(records);

                /*foreach(var record in AcademicTestSessionRecords)
                {
                    if(record.Domain == "Fluency")
                    {
                        record.IsFluency = true;
                        record.Timer = new TimeSpan(0,0,0,210);
                    }
                    else
                    {
                        record.IsNotFluency = true;
                    }

                }*/
            }

            domains = null;
        }

        public void Sessionmodel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RawScore")
            {
                if (!string.IsNullOrEmpty((sender as TestSessionModel).RawScore) || (sender as TestSessionModel).IsScoreSelected)
                {
                    (sender as TestSessionModel).Status = "In-Progress";
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (sender as TestSessionModel).ContentCategoryId).TSOStatus = "In-Progress";
                }
                else
                {
                    (sender as TestSessionModel).Status = "Not Started";
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (sender as TestSessionModel).ContentCategoryId).TSOStatus = "Not Started";
                }
            }
        }

        public void GetProgramNotes()
        {
            ProgramNoteList = new List<ProgramNote>();
            if (commonDataService.ProgramNoteModels != null && commonDataService.ProgramNoteModels.Any())
            {
                //ProgramNoteList = new ObservableCollection<string>(ProgramNoteModels.Select(p => p.LabelName).ToList());                
                foreach (var item in commonDataService.ProgramNoteModels)
                {
                    ProgramNoteList.Add(new ProgramNote() { text = item.LabelName, selected = false, id = item.LabelId });
                }
                if (commonDataService.StudentTestFormOverview != null && !string.IsNullOrEmpty(commonDataService.StudentTestFormOverview.formParameters))
                {
                    var menuItem = JsonConvert.DeserializeObject<FormParamterClass>(commonDataService.StudentTestFormOverview.formParameters);
                    if (menuItem != null && menuItem.ProgramLabelId != null)
                    {
                        var label = commonDataService.ProgramNoteModels.FirstOrDefault(p => p.LabelId.Value == menuItem.ProgramLabelId.Value);
                        var pLabel = ProgramNoteList.FirstOrDefault(p => p.id == menuItem.ProgramLabelId);
                        if (pLabel != null)
                        {
                            pLabel.selected = true;
                        }
                        ProgramLabel = label == null ? "Select a program label" : label.LabelName;
                        OnPropertyChanged(nameof(ProgramLabel));

                    }
                }
            }

        }
        private void GetExaminerDetails()
        {
            ExaminerList = new List<Examiner>();
            if (commonDataService.SearchStaffResponseModel != null && commonDataService.SearchStaffResponseModel.Any())
            {
                foreach (var item in commonDataService.SearchStaffResponseModel)
                {
                    ExaminerList.Add(new Examiner() { text = item.FirstNameLastName, selected = false, id = Convert.ToInt32(item.UserID) });
                }
            }
        }
        public int LocalformInstanceId { get; set; }
        public async Task SaveTestSessionOverView()
        {
            await Task.Delay(0);
            if (commonDataService.StudentTestFormOverview != null)
            {
                var model = JsonConvert.DeserializeObject<FormParamterClass>(commonDataService.StudentTestFormOverview.formParameters);
                model.ProgramLabelId = null;
                var programSelected = ProgramNoteList.FirstOrDefault(p => p.selected);
                if (programSelected != null && model.ProgramLabelId != programSelected.id)
                {
                    model.ProgramLabelId = programSelected.id;
                }
                commonDataService.StudentTestFormOverview.formParameters = JsonConvert.SerializeObject(model);
            }
            IsOverviewChanged = false;
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
            {
                if (commonDataService.IsCompleteForm)
                {
                    foreach (var item in commonDataService.StudentTestForms)
                    {
                        var str = item.rawScore.HasValue ? item.rawScore.Value + "" : "";
                        var testrecord = TestSessionRecords.FirstOrDefault(p => p.ContentCategoryId == item.contentCategoryId);
                        if (testrecord != null && (testrecord.TestDate != item.testDate || testrecord.RawScore != str || testrecord.ExaminerID != item.examinerId))
                        {
                            IsOverviewChanged = true;
                            break;
                        }
                    }
                }
                else if (commonDataService.IsAcademicForm)
                {
                    foreach (var item in commonDataService.StudentTestForms)
                    {
                        var str = item.rawScore != null && item.rawScore.HasValue ? item.rawScore.Value + "" : "";
                        var testrecord = AcademicTestSessionRecords.FirstOrDefault(p => p.ContentCategoryId == item.contentCategoryId);
                        if (testrecord != null && (testrecord.TestDate != item.testDate || testrecord.RawScore != str || testrecord.ExaminerID != item.examinerId))
                        {
                            IsOverviewChanged = true;
                            break;
                        }
                    }

                    if (!IsOverviewChanged)
                    {
                        var testRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == 162);
                        if (testRecord != null)
                        {
                            var overviewrecord = AcademicTestSessionRecords.FirstOrDefault(p => p.ContentCategoryId == 162);
                            if (overviewrecord != null)
                            {
                                if (!overviewrecord.ShowTimerErrorMessage && !testRecord.TimeTaken.HasValue)
                                {
                                    IsOverviewChanged = true;
                                }
                                else if (overviewrecord.ShowTimerErrorMessage && testRecord.TimeTaken.HasValue)
                                {
                                    IsOverviewChanged = true;
                                }
                                else if (!overviewrecord.ShowTimerErrorMessage && testRecord.TimeTaken.HasValue && testRecord.TimeTaken.Value != Convert.ToInt32(overviewrecord.Timer.TotalSeconds))
                                {
                                    IsOverviewChanged = true;
                                }
                                else if (!overviewrecord.ShowTimerErrorMessage && !testRecord.TimeTaken.HasValue)
                                {
                                    IsOverviewChanged = true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var item in commonDataService.StudentTestForms)
                    {
                        var str = item.rawScore.HasValue ? item.rawScore.Value + "" : "";
                        var testrecord = Records.FirstOrDefault(p => p.ContentCategoryId == item.contentCategoryId);
                        if (testrecord != null && (testrecord.TestDate != item.testDate || testrecord.RawScore != str || testrecord.ExaminerID != item.examinerId))
                        {
                            IsOverviewChanged = true;
                            break;
                        }
                    }
                }
            }
            if (IsOverviewChanged)
            {
                //studentTestFormsService.DeleteAll(LocalformInstanceId);
                if (commonDataService.IsCompleteForm)
                {
                    if (TestSessionRecords != null && TestSessionRecords.Any())
                    {
                        var lstStudentTestForms = new List<StudentTestForms>();
                        foreach (var item in TestSessionRecords)
                        {
                            if (!item.IsDateVisible)
                            {
                                continue;
                            }
                            var StudentTestForms = new StudentTestForms();
                            StudentTestForms.IsBaselCeilingApplied = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).IsBaselCeilingApplied;
                            StudentTestForms.BaselCeilingReached = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).BaselCeilingReached;
                            StudentTestForms.LocalStudentTestFormId = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).LocalStudentTestFormId;
                            StudentTestForms.LocalformInstanceId = LocalformInstanceId;
                            StudentTestForms.testDate = item.TestDate;
                            StudentTestForms.Notes = item.Notes;
                            StudentTestForms.TSOStatus = item.Status;
                            StudentTestForms.rawScore = !string.IsNullOrEmpty(item.RawScore) ? Convert.ToInt32(item.RawScore) : default(int?);
                            StudentTestForms.rawScoreEnabled = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).rawScoreEnabled;
                            if (item.Examiner != null && item.Examiner != "Select Examiner")
                                StudentTestForms.examinerId = Convert.ToInt32(commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.FirstNameLastName == item.Examiner).UserID);
                            StudentTestForms.contentCategoryId = item.ContentCategoryId;
                            StudentTestForms.createDate = DateTime.Now;
                            lstStudentTestForms.Add(StudentTestForms);
                        }
                        //studentTestFormsService.InsertAll(lstStudentTestForms);
                        commonDataService.StudentTestForms = new List<StudentTestForms>(lstStudentTestForms);
                    }
                }
                else if (commonDataService.IsAcademicForm)
                {
                    if (AcademicTestSessionRecords != null && AcademicTestSessionRecords.Any())
                    {
                        var lstStudentTestForms = new List<StudentTestForms>();
                        foreach (var item in AcademicTestSessionRecords)
                        {
                            if (!item.DomainVisibility)
                            {
                                continue;
                            }
                            var StudentTestForms = new StudentTestForms();
                            StudentTestForms.IsBaselCeilingApplied = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).IsBaselCeilingApplied;
                            StudentTestForms.BaselCeilingReached = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).BaselCeilingReached;
                            StudentTestForms.LocalformInstanceId = LocalformInstanceId;
                            StudentTestForms.LocalStudentTestFormId = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).LocalStudentTestFormId;
                            StudentTestForms.testDate = item.TestDate;
                            if (item.ContentCategoryId == 162)
                            {
                                if (!item.ShowTimerErrorMessage)
                                {
                                    StudentTestForms.TimeTaken = Convert.ToInt32(item.Timer.TotalSeconds);
                                }
                                else
                                {
                                    StudentTestForms.TimeTaken = null;
                                }
                            }
                            StudentTestForms.Notes = item.Notes;
                            StudentTestForms.TSOStatus = item.Status;
                            StudentTestForms.rawScore = !string.IsNullOrEmpty(item.RawScore) ? Convert.ToInt32(item.RawScore) : default(int?);
                            StudentTestForms.rawScoreEnabled = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).rawScoreEnabled;
                            if (item.Examiner != null && item.Examiner != "Select Examiner")
                                StudentTestForms.examinerId = Convert.ToInt32(commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.FirstNameLastName == item.Examiner).UserID);
                            StudentTestForms.contentCategoryId = item.ContentCategoryId;
                            StudentTestForms.createDate = DateTime.Now;
                            lstStudentTestForms.Add(StudentTestForms);
                        }
                        //studentTestFormsService.InsertAll(lstStudentTestForms);
                        commonDataService.StudentTestForms = new List<StudentTestForms>(lstStudentTestForms);
                    }
                }
                else
                {
                    var lstStudentTestForms = new List<StudentTestForms>();
                    foreach (var item in Records)
                    {
                        if (!item.IsDateVisible)
                        {
                            continue;
                        }
                        var StudentTestForms = new StudentTestForms();
                        StudentTestForms.IsBaselCeilingApplied = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).IsBaselCeilingApplied;
                        StudentTestForms.BaselCeilingReached = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).BaselCeilingReached;
                        StudentTestForms.LocalformInstanceId = LocalformInstanceId;
                        StudentTestForms.LocalStudentTestFormId = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).LocalStudentTestFormId;
                        StudentTestForms.testDate = item.TestDate;
                        StudentTestForms.Notes = item.Notes;
                        StudentTestForms.TSOStatus = item.Status;
                        StudentTestForms.rawScore = !string.IsNullOrEmpty(item.RawScore) ? Convert.ToInt32(item.RawScore) : default(int?);
                        StudentTestForms.rawScoreEnabled = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.ContentCategoryId).rawScoreEnabled;
                        if (item.Examiner != null && item.Examiner != "Select Examiner")
                            StudentTestForms.examinerId = Convert.ToInt32(commonDataService.SearchStaffResponseModel.FirstOrDefault(p => p.FirstNameLastName == item.Examiner).UserID);
                        StudentTestForms.contentCategoryId = item.ContentCategoryId;
                        StudentTestForms.createDate = DateTime.Now;
                        lstStudentTestForms.Add(StudentTestForms);
                    }
                    commonDataService.StudentTestForms = new List<StudentTestForms>(lstStudentTestForms);
                }
                commonDataService.ResetTestDate?.Invoke();
            }
        }
        private List<TestSessionModel> _records = new List<TestSessionModel>();
        public List<TestSessionModel> Records
        {
            get
            {
                return _records;
            }

            set
            {
                _records = value;
                OnPropertyChanged(nameof(Records));
            }
        }

        private List<TestSessionModel> _academicTestSessionrecords = new List<TestSessionModel>();
        public List<TestSessionModel> AcademicTestSessionRecords
        {
            get
            {
                return _academicTestSessionrecords;
            }

            set
            {
                _academicTestSessionrecords = value;
                OnPropertyChanged(nameof(AcademicTestSessionRecords));
            }
        }
    }
    public class ItemNavigationHeaderData
    {
        public int ContentCategoryId { get; set; }
        public string TestDate { get; set; }
        public string Title { get; set; }
        public Color HeaderColor { get; set; }
    }
}