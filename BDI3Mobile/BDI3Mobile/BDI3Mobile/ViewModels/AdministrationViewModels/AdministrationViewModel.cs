using Acr.UserDialogs;
using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Views;
using BDI3Mobile.Views.ItemAdministrationView;
using BDI3Mobile.Views.PopupViews;
using Newtonsoft.Json;
using PCLStorage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using BDI3Mobile.Helpers;
using BDI3Mobile.Models.DevelopmentalFormModel;

namespace BDI3Mobile.ViewModels.AdministrationViewModels
{
    public class AdministrationViewModel : BaseclassViewModel, INotifyPropertyChanged
    {
        private TimerStopWatch _timer;
        private bool isInProgress;
        public bool IsInProgress
        {
            get { return isInProgress; }
            set { isInProgress = value; OnPropertyChanged(nameof(IsInProgress)); }
        }
        private bool baselObtained;
        private bool ceilingObtained;
        public bool BaselObtained
        {
            get
            {
                return baselObtained;
            }
            set
            {
                baselObtained = value;
                OnPropertyChanged(nameof(BaselObtained));
            }
        }
        public bool CeilingObtained
        {
            get
            {
                return ceilingObtained;
            }
            set
            {
                ceilingObtained = value;
                OnPropertyChanged(nameof(CeilingObtained));
            }
        }
        private bool isNextItemTapped = true;

        public bool IsNextItemTapped
        {
            get
            {
                return isNextItemTapped;
            }
            set
            {
                isNextItemTapped = value;
            }
        }

        private bool isPrevItemTapped = true;

        public bool IsPrevItemTapped
        {
            get
            {
                return isPrevItemTapped;
            }
            set
            {
                isPrevItemTapped = value;
            }
        }
        public int TotalAgeinMonths;
        private string testdate;
        public string TestDate
        {
            get
            {
                return testdate;
            }
            set
            {
                testdate = value;
                OnPropertyChanged(nameof(TestDate));
            }
        }

        private TimeSpan _totalSeconds = new TimeSpan(0, 0, 0, 0);
        public TimeSpan TotalSeconds
        {
            get { return _totalSeconds; }
            set
            {
                _totalSeconds = value;
                OnPropertyChanged(nameof(TotalSeconds));
            }
        }

        public string DOB { get; set; }
        private readonly IClinicalTestFormService clinicalTestFormService;
        public string FormNotes { get; set; }
        public Action<NoteModel> NotesSaveAction
        {
            get
            {
                return new Action<NoteModel>((model) =>
                {
                    if (model.IsFormNote)
                    {
                        FormNotes = model.Notes;
                    }
                    else if (model.IsSubDomainNote)
                    {
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == captureMode.DomainCategoryId).Notes = model.Notes;
                    }
                    else
                    {
                        HasDoneChanges = true;
                        captureMode.Notes = model.Notes;
                    }
                });
            }
        }
        private readonly IStudentTestFormsService studentTestFormsService;
        public int LocaInstanceID { get; set; }
        private List<StudentTestFormResponses> studentTestFormResponses;
        private List<ItemInfo> formItemInfo = new List<ItemInfo>();
        private readonly IStudentTestFormResponsesService studentTestFormResponsesService = DependencyService.Get<IStudentTestFormResponsesService>();
        private readonly IContentCategoryItemsService _contentCategoryItemsService;
        private DateTime datePickerDate;
        public DateTime DatePickerDate
        {
            get
            {
                return datePickerDate;
            }
            set
            {
                datePickerDate = value;
                OnPropertyChanged("DatePickerDate");
            }
        }
        private static string InitialBattelleDevHeader = "ADAPTIVE: SELF-CARE (SC)";
        private static string InitialBattelleScreenerHeader = "ADAPTIVE";
        private static Color InitialHeaderColor = Color.FromHex("#d73648");
        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
        private readonly IContentBasalCeilingsService contentBasalCeilingsService;
        private readonly IProductService _productService;
        private readonly IAssessmentsService _assessmentService;
        private readonly IContentCategoryLevelsService _contentCategoryLevelsService;
        private readonly IContentCategoryService _contentcategoryservice;
        private readonly IContentItemsService _contentItemsService;
        private readonly IContentItemAttributesService _contentItemAttributeService;
        private readonly IContentRubricsService _contentRubricsService;
        private readonly IContentRubricPointsService _contentRubicPointsService;
        private readonly IContentItemTallyService _contentItemTallyService;
        private readonly IContentItemTalliesScoresService _contentItemTalliesScoresService;
        private readonly INavigationService _navigationService;

        public bool popupOpenClicked { get; set; }
        public string CurrentSubDomain { get; set; }
        public bool isTestRecordNavigationOpen { get; set; }

        List<SubDomainContent> TotalSubdomainCollection = new List<SubDomainContent>();
        public List<AdministrationContent> SubdomainCollection = new List<AdministrationContent>();

        //Screener Assessment
        public string CurrentDomain { get; set; }

        public List<DomainContent> TotalDomainCollection = new List<DomainContent>();
        public List<AdministrationContent> domainContentCollection = new List<AdministrationContent>();

        public List<ContentCategory> ScreenerStartingPointCatrgories { get; set; }
        private bool userTappedSave;

        public int OfflineStudentID { get; set; }
        #region Properties
        public List<ContentBasalCeilings> ContentBasalCeilingsItems { get; set; }

        private static string[] images = new string[] { "completed_TickMark.png", "iconStatusNonitem.png", "notStarted.png", "iconStatusSkipped.png" };

        private static string[] status = new string[] { "in-progress", "not started", "complete" };

        public bool IsBattelleDevelopmentCompleteChecked { get; set; }

        #region Tally prop
        private bool tallyLayoutIsvisible;
        public bool TallyLayoutIsVisible
        {
            get { return tallyLayoutIsvisible; }
            set { tallyLayoutIsvisible = value; OnPropertyChanged(nameof(TallyLayoutIsVisible)); }
        }

        private Color tallyBckgrdColor;
        public Color TallyBckgrdColor
        {
            get { return tallyBckgrdColor; }
            set { tallyBckgrdColor = value; OnPropertyChanged(nameof(TallyBckgrdColor)); }
        }

        private Color tallyTextColor;
        public Color TallyTextColor
        {
            get { return tallyTextColor; }
            set { tallyTextColor = value; OnPropertyChanged(nameof(TallyTextColor)); }
        }
        #endregion

        #region Scoring Prop
        private bool scoringLayoutIsvisible;
        public bool ScoringLayoutIsvisible
        {
            get { return scoringLayoutIsvisible; }
            set { scoringLayoutIsvisible = value; OnPropertyChanged(nameof(ScoringLayoutIsvisible)); }
        }

        private Color scoreBckgrdColor;
        public Color ScoreBckgrdColor
        {
            get { return scoreBckgrdColor; }
            set { scoreBckgrdColor = value; OnPropertyChanged(nameof(ScoreBckgrdColor)); }
        }

        private Color scoreTextColor;
        public Color ScoreTextColor
        {
            get { return scoreTextColor; }
            set { scoreTextColor = value; OnPropertyChanged(nameof(ScoreTextColor)); }
        }

        #endregion
        private bool isScoringOnly;
        public bool IsScoringOnly
        {
            get { return isScoringOnly; }
            set { isScoringOnly = value; OnPropertyChanged(nameof(IsScoringOnly)); }
        }

        private bool isTallyAndScoring;
        public bool IsTallyAndScoring
        {
            get { return isTallyAndScoring; }
            set { isTallyAndScoring = value; OnPropertyChanged(nameof(IsTallyAndScoring)); }
        }

        private bool leftImageVisiblity;
        public bool LeftImageVisiblity
        {
            get { return leftImageVisiblity; }
            set { leftImageVisiblity = value; OnPropertyChanged(nameof(LeftImageVisiblity)); }
        }

        private bool rightImageVisiblity;
        public bool RightImageVisiblity
        {
            get { return rightImageVisiblity; }
            set { rightImageVisiblity = value; OnPropertyChanged(nameof(RightImageVisiblity)); }
        }
        private string _administrationHeader;
        public string AdministrationHeader
        {
            get { return _administrationHeader; }
            set
            {
                _administrationHeader = value;
                OnPropertyChanged(nameof(AdministrationHeader));
            }
        }
        private string _assessmentType;
        public string AssessmentType
        {
            get { return _assessmentType; }
            set
            {
                _assessmentType = value;
                OnPropertyChanged(nameof(AssessmentType));
            }
        }


        public AssessmentConfigurationType AssessmentConfigurationType { get; set; }

        private Color _administrationHeaderBackgroundColor;
        public Color AdministrationHeaderBackgroundColor
        {
            get { return _administrationHeaderBackgroundColor; }
            set
            {
                _administrationHeaderBackgroundColor = value;
                OnPropertyChanged(nameof(AdministrationHeaderBackgroundColor));
            }
        }

        private Color gridColor;
        public Color GridColor
        {
            get
            {
                return gridColor;
            }
            set
            {
                gridColor = value;
                OnPropertyChanged(nameof(GridColor));
            }
        }
        private Thickness margin;
        public Thickness Margin
        {
            get
            {
                return margin;
            }
            set
            {
                margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }

        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }

        private bool resetEnabled;
        public bool ResetEnabled
        {
            get { return resetEnabled; }
            set { resetEnabled = value; OnPropertyChanged(nameof(ResetEnabled)); }
        }

        private bool startEnabled = true;
        public bool StartEnabled
        {
            get { return startEnabled; }
            set { startEnabled = value; OnPropertyChanged(nameof(StartEnabled)); }
        }

        private bool isTimerInProgress = true;
        public bool IsTimerInProgress
        {
            get { return isTimerInProgress; }
            set { isTimerInProgress = value; OnPropertyChanged(nameof(IsTimerInProgress)); }
        }

        private string timerStatusText = "Start";
        public string TimerStatusText
        {
            get { return timerStatusText; }
            set
            {
                timerStatusText = value; OnPropertyChanged(nameof(TimerStatusText));
            }
        }
        private string timerReset = "iconrefreshgray.png";
        public string TimerReset
        {
            get { return timerReset; }
            set
            {
                timerReset = value; OnPropertyChanged(nameof(TimerReset));
            }
        }
        private Color timerButtonBckgrd;
        public Color TimerButtonBckgrd
        {
            get { return timerButtonBckgrd; }
            set { timerButtonBckgrd = value; OnPropertyChanged(nameof(TimerButtonBckgrd)); }
        }
        AdministrationContent LastItemScored = default(AdministrationContent);
        public ICommand ContentRubicPointSelection
        {
            get
            {
                return new Command<ContentRubricPoint>(async (rubicPoint) =>
                {
                    HasDoneChanges = true;
                    ContentRubricPointCollection.Where(p => p.contentRubricPointsId != rubicPoint.contentRubricPointsId).ForEach(p => p.IsSelected = false);
                    rubicPoint.IsSelected = !rubicPoint.IsSelected;
                    if (rubicPoint.IsSelected && LastItemScored == null)
                    {
                        LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                    }
                    if (CaptureMode != null)
                    {
                        var selectedRubricPoint = ContentRubricPointCollection.FirstOrDefault(p => p.IsSelected);
                        CaptureMode.SelectedDefaultRubicPoint = null;
                        CaptureMode.SelectedInterviewContentRubicPoint = null;
                        CaptureMode.SelectedObservationContentRubicPoint = null;
                        CaptureMode.SelectedStructuredContentRubicPoint = null;
                        if (CaptureMode.DefaultContentScoring != null && CaptureMode.DefaultContentScoring.Any())
                        {
                            CaptureMode.SelectedDefaultRubicPoint = selectedRubricPoint;
                        }
                        if (CheckInterview)
                        {
                            CaptureMode.SelectedInterviewContentRubicPoint = selectedRubricPoint;
                        }
                        else if (CheckObservation)
                        {
                            CaptureMode.SelectedObservationContentRubicPoint = selectedRubricPoint;
                        }
                        else
                        {
                            CaptureMode.SelectedStructuredContentRubicPoint = selectedRubricPoint;
                        }
                        CheckScoreSelected();
                        var isBaselObtained = baselObtained;
                        CalculateBaselCeiling();
                        if (!BaselObtained)
                        {
                            if (rubicPoint.IsSelected && rubicPoint.points != 2)
                            {
                                var question = GetCurrentMovedQuestion(CurrentQuestion);
                                if (question != -1)
                                {
                                    if (rubicPoint.IsSelected)
                                    {
                                        LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                    }
                                    CurrentQuestion = question;
                                    InitialLoad = true;
                                    await LoadParticularQuestion();
                                    InitialLoad = false;
                                }
                            }
                            else if (rubicPoint.IsSelected)
                            {
                                if (commonDataService.IsCompleteForm)
                                {
                                    var scoreItems = SubdomainCollection.Where(p => p.CaptureModeDesc.SelectedDefaultRubicPoint != null || p.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ||
                                    p.CaptureModeDesc.SelectedObservationContentRubicPoint != null || p.CaptureModeDesc.SelectedStructuredContentRubicPoint != null);
                                    if (scoreItems != null && scoreItems.Any() && scoreItems.Count() == 1 && CurrentQuestion == SubdomainCollection.Count - 1)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = CurrentQuestion - 1;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                    }
                                    else if (LastItemScored != null)
                                    {
                                        var indexOfLastQuestion = SubdomainCollection.IndexOf(LastItemScored);
                                        if (indexOfLastQuestion != -1)
                                        {
                                            if (CurrentQuestion < indexOfLastQuestion)
                                            {
                                                if (rubicPoint.IsSelected)
                                                {
                                                    LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                                }
                                                CurrentQuestion = CurrentQuestion - 1;
                                                InitialLoad = true;
                                                await LoadParticularQuestion();
                                                InitialLoad = false;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    var scoreItems = domainContentCollection.Where(p => p.CaptureModeDesc.SelectedDefaultRubicPoint != null || p.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ||
                                    p.CaptureModeDesc.SelectedObservationContentRubicPoint != null || p.CaptureModeDesc.SelectedStructuredContentRubicPoint != null);
                                    if (scoreItems != null && scoreItems.Any() && scoreItems.Count() == 1 && CurrentQuestion == domainContentCollection.Count - 1)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = CurrentQuestion - 1;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                    }
                                    else if (LastItemScored != null)
                                    {
                                        var indexOfLastQuestion = domainContentCollection.IndexOf(LastItemScored);
                                        if (indexOfLastQuestion != -1)
                                        {
                                            if (CurrentQuestion < indexOfLastQuestion)
                                            {
                                                if (rubicPoint.IsSelected)
                                                {
                                                    LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                                }
                                                CurrentQuestion = CurrentQuestion - 1;
                                                InitialLoad = true;
                                                await LoadParticularQuestion();
                                                InitialLoad = false;
                                            }
                                        }

                                    }
                                }

                            }
                        }
                        else if (BaselObtained && !isBaselObtained && !ceilingObtained)
                        {
                            if (commonDataService.IsCompleteForm)
                            {
                                for (int i = CurrentQuestion; i < SubdomainCollection.Count; i++)
                                {
                                    var item = SubdomainCollection[i].CaptureModeDesc;
                                    if (item.SelectedDefaultRubicPoint == null && item.SelectedInterviewContentRubicPoint == null &&
                                    item.SelectedObservationContentRubicPoint == null && item.SelectedInterviewContentRubicPoint == null)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = i;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = CurrentQuestion; i < domainContentCollection.Count; i++)
                                {
                                    var item = domainContentCollection[i].CaptureModeDesc;
                                    if (item.SelectedDefaultRubicPoint == null && item.SelectedInterviewContentRubicPoint == null &&
                                    item.SelectedObservationContentRubicPoint == null && item.SelectedInterviewContentRubicPoint == null)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = i;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }

        private int GetCurrentMovedQuestion(int currentquestion)
        {
            if (commonDataService.IsCompleteForm)
            {
                if (SubdomainCollection != null && SubdomainCollection.Any())
                {
                    var question = SubdomainCollection[currentquestion - 1];
                    if (question.CaptureModeDesc.SelectedDefaultRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedDefaultRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                    else if (question.CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedInterviewContentRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                    else if (question.CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedObservationContentRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                    else if (question.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedStructuredContentRubicPoint.points != 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                }
            }
            else
            {
                if (domainContentCollection != null && domainContentCollection.Any())
                {
                    var question = domainContentCollection[currentquestion - 1];
                    if (question.CaptureModeDesc.SelectedDefaultRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedDefaultRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                    else if (question.CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedInterviewContentRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                    else if (question.CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedObservationContentRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                    else if (question.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                    {
                        if (question.CaptureModeDesc.SelectedStructuredContentRubicPoint.points == 2)
                        {
                            return GetCurrentMovedQuestion(currentquestion - 1);
                        }
                    }
                }
            }
            return currentquestion - 1;
        }
        private void CheckScoreSelected()
        {
            if (commonDataService.IsCompleteForm)
            {
                if (SubdomainCollection != null && SubdomainCollection.Any())
                {
                    var scoreItems = SubdomainCollection.Any(p => p.CaptureModeDesc.SelectedDefaultRubicPoint != null
                    || p.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || p.CaptureModeDesc.SelectedObservationContentRubicPoint != null || p.CaptureModeDesc.SelectedStructuredContentRubicPoint != null);
                    var selectedSubDomain = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == SubdomainCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId);
                    if (selectedSubDomain != null && scoreItems)
                    {
                        TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == SubdomainCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).subdomainscore = null;
                        selectedSubDomain.rawScore = null;
                        TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == SubdomainCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).BasalCeilingReached = false;
                        selectedSubDomain.BaselCeilingReached = false;
                        selectedSubDomain.rawScoreEnabled = false;
                        selectedSubDomain.TSOStatus = "In-Progress";
                        selectedSubDomain.IsScoreSelected = true;
                    }
                    else
                    {
                        if (!selectedSubDomain.rawScoreEnabled)
                        {
                            BaselObtained = false;
                            CeilingObtained = false;
                            BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                            CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";

                            TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == SubdomainCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).subdomainscore = null;
                            selectedSubDomain.rawScore = null;
                            TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == SubdomainCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).BasalCeilingReached = false;
                            selectedSubDomain.BaselCeilingReached = false;
                            selectedSubDomain.rawScoreEnabled = true;
                            selectedSubDomain.TSOStatus = "Not Started";
                            selectedSubDomain.IsScoreSelected = false;
                        }
                        else if (selectedSubDomain.rawScore.HasValue)
                        {
                            selectedSubDomain.IsScoreSelected = true;
                            selectedSubDomain.TSOStatus = "In-Progress";
                        }
                    }
                }
            }
            else if (commonDataService.IsScreenerForm)
            {
                if (domainContentCollection != null && domainContentCollection.Any())
                {
                    var scoreItems = domainContentCollection.Any(p => p.CaptureModeDesc.SelectedDefaultRubicPoint != null
                    || p.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || p.CaptureModeDesc.SelectedObservationContentRubicPoint != null || p.CaptureModeDesc.SelectedStructuredContentRubicPoint != null);
                    var selectedSubDomain = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == domainContentCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId);
                    if (selectedSubDomain != null && scoreItems)
                    {
                        TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == domainContentCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).rawscore = null;
                        selectedSubDomain.rawScore = null;
                        TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == domainContentCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).BasalCeilingObtained = false;
                        selectedSubDomain.BaselCeilingReached = false;
                        selectedSubDomain.rawScoreEnabled = false;
                        selectedSubDomain.TSOStatus = "In-Progress";
                        selectedSubDomain.IsScoreSelected = true;
                    }
                    else
                    {
                        if (!selectedSubDomain.rawScoreEnabled)
                        {
                            BaselObtained = false;
                            CeilingObtained = false;
                            BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                            CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";

                            TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == domainContentCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).rawscore = null;
                            selectedSubDomain.rawScore = null;
                            TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == domainContentCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).BasalCeilingObtained = false;
                            selectedSubDomain.BaselCeilingReached = false;
                            selectedSubDomain.rawScoreEnabled = true;
                            selectedSubDomain.TSOStatus = "Not Started";
                            selectedSubDomain.IsScoreSelected = false;
                        }
                        else if (selectedSubDomain.rawScore.HasValue)
                        {
                            selectedSubDomain.IsScoreSelected = true;
                            selectedSubDomain.TSOStatus = "In-Progress";
                        }
                    }
                }
            }
        }

        List<ContentRubricPoint> contentRubricPointCollection;
        public List<ContentRubricPoint> ContentRubricPointCollection
        {
            get
            {
                return contentRubricPointCollection;
            }

            set
            {
                contentRubricPointCollection = value;
                OnPropertyChanged("ContentRubricPointCollection");
            }
        }

        private List<ContentItemTally> contentItemTalliesCollection;
        public List<ContentItemTally> ContentItemTalliesCollection
        {
            get
            {
                return contentItemTalliesCollection;
            }
            set
            {
                if (value != null)
                {
                    bool CheckLongString = value.Any(a => a.text.Length > 30);
                    if (CheckLongString)
                    {
                        value.ForEach(x => x.TallyDescriptionWidth = 330);
                    }
                    else
                    {
                        value.ForEach(x => x.TallyDescriptionWidth = 130);
                    }
                }
                contentItemTalliesCollection = value;
                OnPropertyChanged("ContentItemTalliesCollection");
            }
        }

        private List<ImageLocation> imageLocation = new List<ImageLocation>();
        public List<ImageLocation> ImageLoc
        {
            get
            {
                return imageLocation;
            }
            set
            {
                imageLocation = value;
                OnPropertyChanged(nameof(ImageLoc));
            }
        }
        #endregion
        #region Commands
        public ICommand MoreButtonCommand { get; set; }
        public bool ChildTapped { get; set; }
        public Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ResetTimer();
                    if (OriginalStudentTestFormOverView != null && OriginalStudentTestFormOverView.notes != FormNotes)
                    {
                        HasDoneChanges = true;
                    }
                    if (!HasDoneChanges)
                    {
                        if (OriginalStudentTestFormOverView != null && commonDataService.StudentTestFormOverview != null)
                        {
                            var originalFormParamerts = JsonConvert.DeserializeObject<FormParamterClass>(OriginalStudentTestFormOverView.formParameters);
                            var upDatedFormParamters = JsonConvert.DeserializeObject<FormParamterClass>(commonDataService.StudentTestFormOverview.formParameters);
                            if (originalFormParamerts.ProgramLabelId != upDatedFormParamters.ProgramLabelId)
                            {
                                HasDoneChanges = true;
                            }
                        }
                    }

                    try
                    {
                        if (!HasDoneChanges)
                        {
                            if (OriginalStudentTestForms != null && OriginalStudentTestForms.Any())
                            {
                                foreach (var item in OriginalStudentTestForms)
                                {
                                    var newRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId);
                                    if (newRecord != null)
                                    {
                                        var splittedDate = newRecord.testDate.Split('/');
                                        DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));

                                        var itemsplittedDate = item.testDate.Split('/');
                                        DateTime itemtestdateTime = new DateTime(Convert.ToInt32(itemsplittedDate[2]), Convert.ToInt32(itemsplittedDate[0]), Convert.ToInt32(itemsplittedDate[1]));

                                        if (itemdateTime.Date != itemtestdateTime.Date || newRecord.examinerId != item.examinerId || newRecord.rawScore != item.rawScore)
                                        {
                                            HasDoneChanges = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                    }

                    if (HasDoneChanges)
                    {
                        await PopupNavigation.Instance.PushAsync(new SavePopUp() { BindingContext = this });
                    }
                    else
                    {
                        if (ChildTapped)
                        {
                            MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "Administrationpage");
                            
                            App.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                        }
                        else
                        {
                            MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "Administrationpage");
                            _navigationService.ClearModalStack();
                            App.Current.MainPage = new Views.DashboardHomeView();
                        }
                    }
                });
            }
        }



        public Action ResetWebView { get; set; }
        public Command ScoringTabTappedCommand
        {
            get
            {
                return new Command(() =>
                {
                    TallyLayoutIsVisible = false;
                    TallyBckgrdColor = Color.LightGray;
                    ScoringLayoutIsvisible = true;
                    ScoreBckgrdColor = TallyTextColor = Color.White;
                    ScoreTextColor = Color.Black;
                    LeftImageVisiblity = false;
                    RightImageVisiblity = true;
                });
            }
        }

        public Command TallyTabTappedCommand
        {
            get
            {
                return new Command(() =>
                {
                    TallyLayoutIsVisible = true;
                    TallyBckgrdColor = ScoreTextColor = Color.White;
                    ScoringLayoutIsvisible = false;
                    ScoreBckgrdColor = Color.LightGray;
                    TallyTextColor = Color.Black;
                    LeftImageVisiblity = true;
                    RightImageVisiblity = false;
                });
            }
        }
        private bool isRecordToolIconClicked;
        public Command OpenRecordToolCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ResetTimer();

                    if (isSaveIconClicked || isRecordToolIconClicked)
                    {
                        return;
                    }
                    isRecordToolIconClicked = true;
                    await PopupNavigation.Instance.PushAsync(new RecordToolsPOP(this));
                    await Task.Delay(600);
                    isRecordToolIconClicked = false;

                });
            }
        }
        public ICommand SaveCommand { get; set; }
        public ICommand ItemNavigationCommand { get; set; }
        public ICommand ItemLevelCloseCommand { get; set; }
        public ICommand ImageViewExpandCommand { get; set; }
        public ICommand NextItemCommand { get; set; }
        public ICommand PreviousItemCommand { get; set; }
        public ICommand StructureTappedCommand { get; set; }
        public ICommand ObservationTappedCommand { get; set; }
        public ICommand InterviewTappadCommand { get; set; }
        public ICommand InfoIconNavigationCommand { get; set; }
        #endregion
        #region Binding Prop
        private GridLength timerSepertorWidth;
        public GridLength TimerSepertorWidth
        {
            get { return timerSepertorWidth; }
            set { timerSepertorWidth = value; OnPropertyChanged(nameof(TimerSepertorWidth)); }
        }
        private GridLength timerWidth;
        public GridLength TimerWidth
        {
            get { return timerWidth; }
            set { timerWidth = value; OnPropertyChanged(nameof(TimerWidth)); }
        }
        private string itemAbbrevation;
        public string ItemAbbrevation
        {
            get
            {
                return itemAbbrevation;
            }
            set
            {
                itemAbbrevation = value;
                OnPropertyChanged(nameof(ItemAbbrevation));
            }
        }
        private string behaviorTextFilePath;
        public string BehaviorTextFilePath
        {
            get { return behaviorTextFilePath; }
            set
            {
                behaviorTextFilePath = value;// new HtmlToText().Convert(value);
                OnPropertyChanged(nameof(BehaviorTextFilePath));
            }
        }
        private string behaviorText;
        public string BehaviorText
        {
            get
            {
                return behaviorText;
            }
            set
            {
                behaviorText = value;
                OnPropertyChanged(nameof(BehaviorText));
            }
        }
        private string captureModeTextFilePath;
        public string CaptureModeTextFilePath
        {
            get { return captureModeTextFilePath; }
            set
            {
                captureModeTextFilePath = value;// new HtmlToText().Convert(value);
                OnPropertyChanged(nameof(CaptureModeTextFilePath));
            }
        }
        private string captureModeText;
        public string CaptureModeText
        {
            get { return captureModeText; }
            set
            {
                captureModeText = value;// new HtmlToText().Convert(value);
                OnPropertyChanged(nameof(CaptureModeText));
            }
        }
        private CaptureMode captureMode;
        public CaptureMode CaptureMode
        {
            get { return captureMode; }
            set { captureMode = value; OnPropertyChanged(nameof(CaptureMode)); }
        }
        private bool timerVisibilty;
        public bool TimerVisibilty
        {
            get { return timerVisibilty; }
            set { timerVisibilty = value; OnPropertyChanged(nameof(TimerVisibilty)); }
        }
        private int maxTime;
        public int MaxTime
        {
            get { return maxTime; }
            set
            {
                if ((!IsBattelleDevelopmentCompleteChecked && ItemAbbrevation == "ST" && ItemNumber == "84") || ItemAbbrevation == "PC" && ItemNumber == "4")
                {
                    maxTime = 60;
                }
                else
                {
                    maxTime = value;
                }
                OnPropertyChanged(nameof(MaxTime));
            }
        }
        private string materialHtmlFilePath;
        public string MaterialHtmlFilePath
        {
            get
            {
                return materialHtmlFilePath;
            }
            set
            {
                materialHtmlFilePath = value;
                OnPropertyChanged(nameof(MaterialHtmlFilePath));
            }
        }
        private string materialText;
        public string MaterialText
        {
            get { return materialText; }
            set
            {
                if (value != null && value.ToString().ToLower().Contains("timing device"))
                {
                    TimerVisibilty = true;
                    TimerSepertorWidth = new GridLength(1, GridUnitType.Absolute);
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        TimerWidth = new GridLength(1, GridUnitType.Star);
                    }
                    else
                    {
                        TimerWidth = new GridLength(.8, GridUnitType.Star);
                    }

                }

                else
                {
                    TimerVisibilty = false;
                    TimerSepertorWidth = new GridLength(0);
                    TimerWidth = new GridLength(0);
                }
                materialText = value;
                OnPropertyChanged(nameof(MaterialText));
            }
        }
        private string totalScoringtext;
        public string TotalScoringText
        {
            get { return totalScoringtext; }
            set
            {
                totalScoringtext = value;
                OnPropertyChanged(nameof(TotalScoringText));
            }
        }

        private int scoringHeight;
        public int ScoringHeight
        {
            get { return scoringHeight; }
            set
            {
                scoringHeight = value;
                OnPropertyChanged(nameof(ScoringHeight));
            }
        }
        private string scoringNotes;
        public string ScoringNotes
        {
            get { return scoringNotes; }
            set
            {
                ScoringHeight = (value == null) ? 50 : 90;
                scoringNotes = value;
                OnPropertyChanged(nameof(ScoringNotes));
            }
        }

        private bool isNextShown;
        public bool IsNextShown
        {
            get { return isNextShown; }
            set
            {
                isNextShown = value;
                OnPropertyChanged(nameof(IsNextShown));
            }
        }

        public int CurrentQuestion { get; set; }

        private bool isPreviousShown;
        public bool IsPreviousShown
        {
            get { return isPreviousShown; }
            set { isPreviousShown = value; OnPropertyChanged(nameof(IsPreviousShown)); }
        }

        private string scoringText;
        public string ScoringText
        {
            get { return scoringText; }
            set { scoringText = value; OnPropertyChanged(nameof(ScoringText)); }
        }
        private bool structureEnabled;
        //Set it for whole Strucuture StackLayout.
        public bool StructureEnabled
        {
            get { return structureEnabled; }
            set { structureEnabled = value; OnPropertyChanged(nameof(StructureEnabled)); }
        }
        private bool observationEnabled;
        public bool ObservationEnabled
        {
            get { return observationEnabled; }
            set { observationEnabled = value; OnPropertyChanged(nameof(ObservationEnabled)); }
        }
        private bool interviewEnabled;
        public bool InterviewEnabled
        {
            get { return interviewEnabled; }
            set { interviewEnabled = value; OnPropertyChanged(nameof(InterviewEnabled)); }
        }

        //Structure Radio button
        private bool checkStruture;
        public bool CheckStructure
        {
            get { return checkStruture; }
            set { checkStruture = value; OnPropertyChanged(nameof(CheckStructure)); }
        }
        private Color structureBorderColor = Color.Gray;
        public Color StructureBorderColor
        {
            get { return structureBorderColor; }
            set { structureBorderColor = value; OnPropertyChanged(nameof(StructureBorderColor)); }
        }
        private FontAttributes structureFontAttribute;
        public FontAttributes StructureFontAttribute
        {
            get { return structureFontAttribute; }
            set { structureFontAttribute = value; OnPropertyChanged(nameof(StructureFontAttribute)); }
        }

        private string itemNumber;
        public string ItemNumber
        {
            get { return itemNumber; }
            set { itemNumber = value; OnPropertyChanged(nameof(ItemNumber)); }
        }

        //Observation Radio button
        private bool checkObservation;
        public bool CheckObservation
        {
            get { return checkObservation; }
            set { checkObservation = value; OnPropertyChanged(nameof(CheckObservation)); }
        }
        private Color observationBorderColor = Color.Gray;
        public Color ObservationBorderColor
        {
            get { return observationBorderColor; }
            set { observationBorderColor = value; OnPropertyChanged(nameof(ObservationBorderColor)); }
        }
        private FontAttributes observationFontAttribute;
        public FontAttributes ObservationFontAttribute
        {
            get { return observationFontAttribute; }
            set { observationFontAttribute = value; OnPropertyChanged(nameof(ObservationFontAttribute)); }
        }
        //Interview Radio button
        private bool checkInterview;
        public bool CheckInterview
        {
            get { return checkInterview; }
            set { checkInterview = value; OnPropertyChanged(nameof(CheckInterview)); }
        }
        private Color interviewBorderColor = Color.Gray;
        public Color InterviewBorderColor
        {
            get { return interviewBorderColor; }
            set { interviewBorderColor = value; OnPropertyChanged(nameof(InterviewBorderColor)); }
        }
        private FontAttributes interviewFontAttribute;
        public FontAttributes InterviewFontAttribute
        {
            get { return interviewFontAttribute; }
            set { interviewFontAttribute = value; OnPropertyChanged(nameof(InterviewFontAttribute)); }
        }
        private Color structureDisabled;
        //Structure Text color changed.
        public Color StructureDisabled
        {
            get { return structureDisabled; }
            set
            {
                structureDisabled = value;
                OnPropertyChanged(nameof(StructureDisabled));
            }
        }
        private Color observedDisabled;
        public Color ObservedDisabled
        {
            get { return observedDisabled; }
            set
            {
                observedDisabled = value;
                OnPropertyChanged(nameof(ObservedDisabled));
            }
        }
        private Color interviewDisabled;
        public Color InterviewDisabled
        {
            get { return interviewDisabled; }
            set
            {
                interviewDisabled = value;
                OnPropertyChanged(nameof(InterviewDisabled));
            }
        }
        private ImageSource imagePath1;//= Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + "661517.png";
        //string path =;
        public ImageSource ImagePath1
        {
            get { return imagePath1; }
            set
            {
                imagePath1 = value;
                OnPropertyChanged(nameof(ImagePath1));
            }
        }
        private ImageSource imagePath2;// = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + "RC17.png";
        public ImageSource ImagePath2
        {
            get { return imagePath2; }
            set
            {
                imagePath2 = value;
                OnPropertyChanged(nameof(ImagePath2));
            }
        }

        private ImageSource imagePath3;// = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + "RC17.png";
        public ImageSource ImagePath3
        {
            get { return imagePath3; }
            set
            {
                imagePath3 = value;
                OnPropertyChanged(nameof(ImagePath3));
            }
        }

        //SCORING CHECK BOX
        private bool tallyListviewCheckbox;
        public bool TallyListviewCheckbox
        {
            get { return tallyListviewCheckbox; }
            set { tallyListviewCheckbox = value; OnPropertyChanged(nameof(TallyListviewCheckbox)); }
        }

        private bool tallyListviewCheckboxEnabled;
        public bool TallyListviewCheckboxEnabled
        {
            get { return tallyListviewCheckboxEnabled; }
            set { tallyListviewCheckboxEnabled = value; OnPropertyChanged(nameof(TallyListviewCheckboxEnabled)); }
        }
        private Color tallyListviewChecboxkDisabled;
        public Color TallyListviewChecboxkDisabled
        {
            get { return tallyListviewChecboxkDisabled; }
            set
            {
                tallyListviewChecboxkDisabled = value;
                OnPropertyChanged(nameof(TallyListviewChecboxkDisabled));
            }
        }

        private Color tallyListviewCheckboxBorderColor = Color.Gray;
        public Color TallyListviewCheckboxBorderColor
        {
            get { return tallyListviewCheckboxBorderColor; }
            set { tallyListviewCheckboxBorderColor = value; OnPropertyChanged(nameof(TallyListviewCheckboxBorderColor)); }
        }

        private FontAttributes tallyListviewCheckboxFontAttribute;
        public FontAttributes TallyListviewCheckboxFontAttribute
        {
            get { return tallyListviewCheckboxFontAttribute; }
            set { tallyListviewCheckboxFontAttribute = value; OnPropertyChanged(nameof(TallyListviewCheckboxFontAttribute)); }
        }



        #endregion
        #region Constructor
        public AdministrationViewModel(int loaclInstanceID, bool isbattellleDevelopmentCompleteChecked, string dob, string testdate)
        {
            StartCommand = new Command(StartTimerCommand);
            StopCommand = new Command(StopTimerCommand);
            //make sure you put this in the constructor
            _timer = new TimerStopWatch(TimeSpan.FromSeconds(1), CountDown);
            TotalSeconds = _totalSeconds;
            OriginalStudentTestForms = new List<StudentTestForms>(commonDataService.StudentTestForms);
            IsBattelleDevelopmentCompleteChecked = isbattellleDevelopmentCompleteChecked;
            _productService = DependencyService.Get<IProductService>();
            _assessmentService = DependencyService.Get<IAssessmentsService>();
            _contentCategoryLevelsService = DependencyService.Get<IContentCategoryLevelsService>();
            DOB = dob;
            TestDate = testdate;
            clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            LocaInstanceID = loaclInstanceID;
            _contentcategoryservice = DependencyService.Get<IContentCategoryService>();
            SaveCommand = new Command(NewSavedCommand);
            ItemNavigationCommand = new Command(OpenItemLevelNavigation);
            ItemLevelCloseCommand = new Command(CloseTreeView);
            NextItemCommand = new Command(execute: async () =>
            {
                NextItemClicked();
            },
            canExecute: () =>
                 IsNextItemTapped
            );

            PreviousItemCommand = new Command(execute: () =>
            {
                PreviousItemClicked();
            },
            canExecute: () =>
                IsPrevItemTapped
            );
            StructureTappedCommand = new Command(TappedStructured);
            ObservationTappedCommand = new Command(TappedObservation);
            InterviewTappadCommand = new Command(TappedInterview);
            MoreButtonCommand = new Command(MoreButtonClicked);
            InfoIconNavigationCommand = new Command(NavigateToInformationPage);

            if (isbattellleDevelopmentCompleteChecked)
                AdministrationHeader = InitialBattelleDevHeader;
            else
                AdministrationHeader = InitialBattelleScreenerHeader;

            AdministrationHeaderBackgroundColor = InitialHeaderColor;
            ContentRubricPointCollection = new List<ContentRubricPoint>();
            ContentItemTalliesCollection = new List<ContentItemTally>();
            _contentItemsService = DependencyService.Get<IContentItemsService>();
            _contentItemAttributeService = DependencyService.Get<IContentItemAttributesService>();
            _contentRubricsService = DependencyService.Get<IContentRubricsService>();
            _contentRubicPointsService = DependencyService.Get<IContentRubricPointsService>();
            _contentItemTallyService = DependencyService.Get<IContentItemTallyService>();
            _contentItemTalliesScoresService = DependencyService.Get<IContentItemTalliesScoresService>();
            _contentCategoryItemsService = DependencyService.Get<IContentCategoryItemsService>();
            contentBasalCeilingsService = DependencyService.Get<IContentBasalCeilingsService>();
            _navigationService = DependencyService.Get<INavigationService>();


            GridColor = Color.FromHex("#f4f4f4");
            CalculateAgeDiff();
            ContentBasalCeilingsItems = commonDataService.ContentBasalCeilings;
            MessagingCenter.Subscribe<ItemNavigationHeaderData>(this, "Administrationpage", async (value) =>
            {
                if (value == null)
                    return;
                UserDialogs.Instance.ShowLoading("Loading");
                InitialLoad = true;
                AdministrationHeader = value.Title;
                AdministrationHeaderBackgroundColor = value.HeaderColor;

                await PopupNavigation.Instance.PopAsync();

                int startindexof = AdministrationHeader.IndexOf('(');
                int Endindexof = AdministrationHeader.IndexOf(')');
                string domains = string.Empty;
                if (commonDataService.IsCompleteForm)
                {
                    domains = AdministrationHeader.Substring(startindexof + 1, Endindexof - startindexof - 1);
                    var categories = TotalCategories.Where(p => p.contentCategoryLevelId == 2 && p.code == domains).ToList();
                    if (categories != null && categories.Any())
                    {
                        var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == categories.FirstOrDefault().contentCategoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                        if (startingPointCategory != null)
                        {
                            var parentCategory = TotalCategories.Where(p => p.contentCategoryId == startingPointCategory.parentContentCategoryId);
                            var contentCategoryItems = commonDataService.ContentCategoryItems;
                            var splittedcode = parentCategory != null && parentCategory.Any() ? parentCategory.FirstOrDefault().code.Split(' ').FirstOrDefault() : domains;
                            var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                            var position = -1;
                            if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                            {
                                var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                                foreach (var item in TotalSubdomainCollection)
                                {
                                    if (startingContentitem != null)
                                    {
                                        var findContent = item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId);

                                        if (findContent != null)
                                        {
                                            position = item.AdministrationContents.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId));
                                            break;
                                        }
                                    }

                                }
                            }
                            await Initialize(splittedcode, position == -1 ? 0 : position);
                        }
                        else
                        {
                            await Initialize(domains);
                        }
                    }
                    else
                    {
                        await Initialize(domains);
                    }
                }
                else
                {
                    domains = AdministrationHeader.Substring(0, startindexof).Trim();
                    var categories = TotalCategories.Where(p => p.contentCategoryLevelId == 4 && p.name == domains).ToList();
                    if (categories != null && categories.Any())
                    {
                        var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == categories.FirstOrDefault().contentCategoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                        if (startingPointCategory != null)
                        {
                            var contentCategoryItems = commonDataService.ContentCategoryItems;
                            var parentCategory = TotalCategories.Where(p => p.contentCategoryId == startingPointCategory.parentContentCategoryId);
                            var splittedcode = parentCategory != null && parentCategory.Any() ? parentCategory.FirstOrDefault().name : AdministrationHeader;
                            var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                            var position = -1;
                            if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                            {
                                var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                                foreach (var item in TotalDomainCollection)
                                {
                                    var findContent = item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId);
                                    if (findContent != null)
                                    {
                                        position = item.AdministrationContents.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId));
                                        break;
                                    }
                                }
                            }
                            await Initialize(splittedcode, position == -1 ? 0 : position);
                        }
                        else
                        {
                            await Initialize(AdministrationHeader);
                        }
                    }
                    else
                    {
                        await Initialize(domains);
                    }
                }
                InitialLoad = false;
                UserDialogs.Instance.HideLoading();
            });
            commonDataService.ResetTestDate = new Action(() =>
            {
                if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any() && CaptureMode != null)
                {
                    var testOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == CaptureMode.DomainCategoryId);
                    if (testOverview != null)
                    {
                        TestDate = testOverview.testDate;
                    }

                    if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                    {
                        foreach (var item in TotalSubdomainCollection)
                        {
                            var form = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.SubDomainCategoryId);
                            if (form != null)
                            {
                                item.subdomainscore = form.rawScore;
                            }
                        }
                    }
                    if (TotalDomainCollection != null && TotalDomainCollection.Any())
                    {
                        foreach (var item in TotalDomainCollection)
                        {
                            var form = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.DoaminCategoryId);
                            if (form != null)
                            {
                                item.rawscore = form.rawScore;
                            }
                        }
                    }
                }
            });
        }
        public bool InitialLoad { get; set; }
        public async Task LoadTestForm()
        {
            int startindex = AdministrationHeader.IndexOf('(');
            int Endindex = AdministrationHeader.IndexOf(')');
            string domain = IsBattelleDevelopmentCompleteChecked ? AdministrationHeader.Substring(startindex + 1, Endindex - startindex - 1) : AdministrationHeader;
            InitialLoad = true;
            if (IsBattelleDevelopmentCompleteChecked)
            {
                LoadAllDomainsData();
            }
            else
            {
                LoadAllDomainDataForScreener();
            }
            if (commonDataService.IsCompleteForm)
            {
                var startingPointCategory = TotalCategories.Where(p => p.contentCategoryLevelId == 3).Where(p => TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                if (startingPointCategory != null)
                {
                    var contentCategoryItems = commonDataService.ContentCategoryItems;
                    var parentCategory = TotalCategories.Where(p => p.contentCategoryId == startingPointCategory.parentContentCategoryId);
                    var splittedcode = parentCategory != null && parentCategory.Any() ? parentCategory.FirstOrDefault().code.Split(' ').FirstOrDefault() : domain;
                    var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                    var position = -1;
                    if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                    {
                        var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                        foreach (var item in TotalSubdomainCollection)
                        {
                            var findContent = item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId);
                            if (findContent != null)
                            {
                                position = item.AdministrationContents.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId));
                                break;
                            }

                        }
                    }
                    await Initialize(splittedcode, position == -1 ? 0 : position);
                }
                else
                {
                    await Initialize(domain);
                }
            }
            else if (commonDataService.IsScreenerForm)
            {
                var startingPointCategory = TotalCategories.Where(p => p.contentCategoryLevelId == 5).Where(p => TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                if (startingPointCategory != null)
                {
                    var contentCategoryItems = commonDataService.ContentCategoryItems;
                    var parentCategory = TotalCategories.Where(p => p.contentCategoryId == startingPointCategory.parentContentCategoryId);
                    var splittedcode = parentCategory != null && parentCategory.Any() ? parentCategory.FirstOrDefault().name : AdministrationHeader;
                    var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                    var position = -1;
                    if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                    {
                        var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                        foreach (var item in TotalDomainCollection)
                        {
                            var findContent = item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId);
                            if (findContent != null)
                            {
                                position = item.AdministrationContents.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(item.AdministrationContents.FirstOrDefault(p => p.ContentItemID == startingContentitem.contentItemId));
                                break;
                            }

                        }
                    }
                    await Initialize(splittedcode, position == -1 ? 0 : position);
                }
                else
                {
                    await Initialize(AdministrationHeader);
                }
            }
            InitialLoad = false;
            UserDialogs.Instance.HideLoading();
            await LoadTSO();
        }


        private async Task LoadTSO()
        {
            UserDialogs.Instance.HideLoading();
            if (commonDataService.IsCompleteForm)
            {
                await PopupNavigation.Instance.PushAsync(new TestsessionOverviewView(), false);
            }
            else if (commonDataService.IsScreenerForm)
            {
                await PopupNavigation.Instance.PushAsync(new TestSessionOverviewScreenerView(), false);
            }
        }
        private void CalculateAgeDiff()
        {
            var splittedDate = DOB.Split('/');
            DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
            DateTime Now = DateTime.Now;
            int Years = new DateTime(DateTime.Now.Subtract(itemdateTime).Ticks).Year - 1;
            DateTime PastYearDate = itemdateTime.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            TotalAgeinMonths = Months + (Years * 12);
            commonDataService.TotalAgeinMonths = TotalAgeinMonths;
        }
        public StudentTestFormOverview OriginalStudentTestFormOverView { get; set; }
        public List<StudentTestForms> OriginalStudentTestForms { get; set; }
        #endregion

        #region TimerRegion
        /// <summary>
        /// Starts and stop the timer.
        /// </summary>
        private void StartTimerCommand()
        {
            TimerStatusText = (TimerStatusText == "Start") ? "Stop" : "Start";
            TimerButtonBckgrd = (TimerStatusText == "Start") ? Color.FromHex("#478128") : Color.FromHex("#cc1416");

            if (TimerStatusText != "Start")
            {
                TimerReset = "iconrefreshgray.png";
                ResetEnabled = false;
                _timer.Start();
            }

            else
            {
                TimerReset = "iconrefreshblue.png";
                ResetEnabled = true;
                _timer.Stop();
            }

        }

        /// <summary>
        /// Stops and resets the timer.
        /// </summary>
        private void StopTimerCommand()
        {
            StartEnabled = IsTimerInProgress = true;
            TimerButtonBckgrd = Color.FromHex("#478128");
            TotalSeconds = new TimeSpan(0, 0, 0, 0);
            TimerReset = "iconrefreshgray.png";
            _timer.Stop();
        }
        /// <summary>
        /// Counts down the timer
        /// </summary>
        private async void CountDown()
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack)
                {
                    if (popup is TimerLimitPopUp && (popup as TimerLimitPopUp).Title == "TimerPopup")
                    {
                        return;
                    }
                }
            }
            if (TotalSeconds.TotalSeconds == this.MaxTime && IsTimerInProgress)
            {
                //StopTimerCommand();
                _timer.Stop();
                IsTimerInProgress = false;
                TimerStatusText = "Start";
                StartEnabled = false;
                TimerButtonBckgrd = Color.LightGray;
                //TimerButtonBckgrd = Color.FromHex("#478128");
                await PopupNavigation.Instance.PushAsync(new TimerLimitPopUp() { BindingContext = this });
            }
            else
            {
                TotalSeconds = _totalSeconds.Add(new TimeSpan(0, 0, 0, 1));
            }
        }
        #endregion

        private bool isSaveIconClicked { get; set; }
        public async void NewSavedCommand()
        {
            if (isSaveIconClicked || isRecordToolIconClicked)
            {
                return;
            }

            isSaveIconClicked = true;
            SaveTestForm();
            await Task.Delay(600);
            isSaveIconClicked = false;
        }

        private void UpdateItemNavigationPageHeader(string classID, string subDomainName, string hint)
        {
            if (string.IsNullOrEmpty(classID))
                return;
            var adminstrationHeader = classID + (IsBattelleDevelopmentCompleteChecked ? ": " + subDomainName.ToUpper() + " (" + hint + ")" : "");
            if (classID.Contains("ADAPTIVE"))
            {
                AdministrationHeader = adminstrationHeader;
                AdministrationHeaderBackgroundColor = Color.FromHex("#d73648");
            }
            else if (classID.Contains("SOCIAL-EMOTIONAL"))
            {
                AdministrationHeader = adminstrationHeader;
                AdministrationHeaderBackgroundColor = Color.FromHex("#008550");
            }
            else if (classID.Contains("COMMUNICATION"))
            {
                AdministrationHeader = adminstrationHeader;
                AdministrationHeaderBackgroundColor = Color.FromHex("#5c2d91");
            }
            else if (classID.Contains("MOTOR"))
            {
                AdministrationHeader = adminstrationHeader;
                AdministrationHeaderBackgroundColor = Color.FromHex("#0066ad");
            }
            else if (classID.Contains("COGNITIVE"))
            {
                AdministrationHeader = adminstrationHeader;
                AdministrationHeaderBackgroundColor = Color.FromHex("#cc4b00");
            }
        }

        public List<ContentCategory> TotalCategories { get; set; }
        public List<ContentCategory> StartingPointCatrgories { get; set; }
        public List<ContentCategory> SubContentCategories { get; set; } = new List<ContentCategory>();
        public async void LoadAllDomainsData()
        {
            await Task.Delay(0);
            OriginalStudentTestFormOverView = clinicalTestFormService.GetStudentTestFormsByID(LocaInstanceID);
            FormNotes = OriginalStudentTestFormOverView?.notes;
            studentTestFormResponses = studentTestFormResponsesService.GetStudentTestFormResponses(LocaInstanceID);
            if (studentTestFormResponses != null && studentTestFormResponses.Any())
            {
                var formJson = studentTestFormResponses.Where(p => !string.IsNullOrEmpty(p.Response)).Select(p => p.Response);
                foreach (var item in formJson)
                {
                    var jsonItems = JsonConvert.DeserializeObject<List<FormJsonClass>>(item);
                    if (jsonItems != null && jsonItems.Any())
                    {
                        foreach (var jsonitem in jsonItems)
                        {
                            if (jsonitem.items != null && jsonitem.items.Any())
                            {
                                formItemInfo.AddRange(jsonitem.items);
                            }
                        }
                    }
                }
            }
            StartingPointCatrgories = new List<ContentCategory>();
            TotalCategories = commonDataService.TotalCategories;
            var contentitem = commonDataService.ContentItems;
            foreach (var item in TotalCategories)
            {
                if (item.contentCategoryLevelId == 2)
                {
                    var ParentCategory = TotalCategories.FirstOrDefault(p => p.contentCategoryId == item.parentContentCategoryId);
                    item.ParentCategoryTitle = ParentCategory.name;
                    SubContentCategories.Add(item);
                }
                else if (item.contentCategoryLevelId == 3 && string.IsNullOrEmpty(item.code))
                {
                    StartingPointCatrgories.Add(item);
                }
            }
            if (SubContentCategories != null && SubContentCategories.Any())
            {
                var contentItemAttribute = commonDataService.ContentItemAttributes;
                var contentRubrics = commonDataService.ContentRubrics;
                var contentRubicPoints = commonDataService.ContentRubricPoints;
                var contentTally = commonDataService.ContentItemTallies;
                var contentTallyScores = commonDataService.ContentItemTallyScores;

                foreach (var categoryItem in SubContentCategories)
                {
                    if (contentitem != null && contentItemAttribute != null && contentRubrics != null)
                    {
                        var subDomainData = new SubDomainContent();
                        var startingPoint = StartingPointCatrgories.FirstOrDefault(p => p.parentContentCategoryId == categoryItem.contentCategoryId);
                        var tesetSessionOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == categoryItem.contentCategoryId);
                        subDomainData.subdomainscore = tesetSessionOverview.rawScore;
                        subDomainData.BasalCeilingReached = tesetSessionOverview.BaselCeilingReached;
                        subDomainData.CategoryId = startingPoint.contentCategoryId;
                        subDomainData.SubDomainCategoryId = categoryItem.contentCategoryId;
                        subDomainData.SubDomainName = categoryItem.name;
                        subDomainData.SubDomainCode = categoryItem.code;
                        subDomainData.ParentCategoryName = categoryItem.ParentCategoryTitle;
                        subDomainData.AdministrationContents = new List<AdministrationContent>();
                        foreach (var item in contentitem)
                        {
                            var admincontent = new AdministrationContent();
                            var captureModeContent = new CaptureMode();
                            List<ContentRubricPoint> scoring = new List<ContentRubricPoint>();
                            List<ContentItemTally> contentItemTallies = new List<ContentItemTally>();
                            List<ImageSource> ImageSource = new List<ImageSource>();
                            if (item.itemCode.Split(' ')[0] == categoryItem.code)
                            {
                                captureModeContent.DomainCategoryId = categoryItem.contentCategoryId;
                                admincontent.ItemAbbrevation = item.itemCode.Split(' ')[0];
                                admincontent.ItemNumber = item.itemNumber;
                                admincontent.MaxTime = item.maxTime1;
                                admincontent.BehaviorContent = item.itemText;
                                admincontent.BehaviourHtmlFIlePath = item.HtmlFilePath;
                                admincontent.ContentItemID = item.contentItemId;
                                captureModeContent.StructuredCollectionData = new List<CaptureModeContentModel>();
                                captureModeContent.ObservationCollectionData = new List<CaptureModeContentModel>();
                                captureModeContent.InterviewCollectionData = new List<CaptureModeContentModel>();

                                contentItemAttribute.OrderBy(o => o.contentItemAttributeId).ForEach(x =>
                                {
                                    if (item.contentItemId == x.contentItemId)
                                    {
                                        var model = new CaptureModeContentModel() { ContentID = x.contentItemId, ContentDescription = x.value, HTMilFilePath = x.HtmlFilePath };
                                        if (x.name == "Materials")
                                        {
                                            admincontent.MaterialContentFilePath = x.HtmlFilePath;
                                            admincontent.MaterialContent = x.value;
                                        }
                                        if (x.name == "Structured")
                                        {
                                            if (captureModeContent.StructuredContent != null)
                                            {
                                                captureModeContent.StructuredCollectionData.Add(model);
                                            }
                                            else
                                            {
                                                captureModeContent.StructuredContent = model;
                                            }
                                        }
                                        if (x.name == "Observation")
                                        {

                                            if (captureModeContent.ObservationContent != null)
                                            {
                                                captureModeContent.ObservationCollectionData.Add(model);
                                            }
                                            else
                                            {
                                                captureModeContent.ObservationContent = model;
                                            }
                                        }
                                        if (x.name == "Interview")
                                        {
                                            if (captureModeContent.InterviewContent != null)
                                            {
                                                captureModeContent.InterviewCollectionData.Add(model);
                                            }
                                            else
                                            {
                                                captureModeContent.InterviewContent = model;
                                            }
                                        }
                                        if (x.name == "Image")
                                        {
                                            ImageSource.Add(x.value);
                                        }
                                        admincontent.CaptureModeDesc = captureModeContent;
                                    }
                                });

                                contentRubrics.ForEach(x =>
                                {
                                    if (x.contentItemId == item.contentItemId)
                                    {
                                        admincontent.ScoringNotes = x.pointsDesc;
                                        admincontent.ScoringCustomData = x.notes;
                                        if (x.title == "Scoring: Structured")
                                        {
                                            admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Interview")
                                        {
                                            admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Observation")
                                        {
                                            admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Observation and Interview" || x.title == "Scoring: Interview and Observation")
                                        {
                                            admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                            admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Observation and Structured" || x.title == "Scoring: Structured and Observation")
                                        {
                                            admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                            admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Structured and Interview" || x.title == "Scoring: Interview and Structured")
                                        {
                                            admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;

                                            admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Structured and Observation" || x.title == "Scoring: Observation and Structured")
                                        {
                                            admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                            admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Interview and Structured" || x.title == "Scoring: Structured and Interview")
                                        {
                                            admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;

                                            admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                                        }
                                        else if (x.title == "Scoring: Interview and Observation" || x.title == "Scoring: Observation and Interview")
                                        {
                                            admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                            admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                            admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;
                                        }

                                        contentRubicPoints.ForEach(y =>
                                        {
                                            if (x.contentRubricId == y.contentRubricId)
                                            {
                                                var score = new ContentRubricPoint();
                                                score.points = y.points;
                                                score.description = y.description;
                                                score.contentRubricId = y.contentRubricId;
                                                score.contentRubricPointsId = y.contentRubricPointsId;
                                                score.HtmlFilePath = y.HtmlFilePath;
                                                //scoring.Add(score);

                                                if (x.title == "Scoring: Structured")
                                                {
                                                    admincontent.CaptureModeDesc.StructuredContentScoring.Add(score);
                                                }
                                                else if (x.title == "Scoring: Interview")
                                                {
                                                    admincontent.CaptureModeDesc.InterviewContentScoring.Add(score);
                                                }
                                                else if (x.title == "Scoring: Observation")
                                                {
                                                    admincontent.CaptureModeDesc.ObservationContentScoring.Add(score);
                                                }
                                                else
                                                {
                                                    admincontent.CaptureModeDesc.DefaultContentScoring.Add(score);
                                                }
                                            }
                                        });
                                    }
                                });

                                contentTally.ForEach(x =>
                                {
                                    if (x.contentItemId == item.contentItemId)
                                    {
                                        if (item.contentItemId == 98)
                                        {

                                        }
                                        var tally = new ContentItemTally();
                                        tally.contentItemId = item.contentItemId;
                                        tally.contentItemTallyId = x.contentItemTallyId;
                                        tally.text = x.text;
                                        tally.CorrectLayoutAction = CorrectAction;
                                        tally.InCorrectLayoutAction = InCorrectAction;
                                        contentItemTallies.Add(tally);
                                        admincontent.TallyContent.Add(tally);
                                    }
                                });


                                scoring = new List<ContentRubricPoint>(scoring.OrderByDescending(a => a.points));
                                admincontent.Images = ImageSource;

                                if (admincontent.CaptureModeDesc != null &&
                                    (admincontent.CaptureModeDesc.ObservationContent != null || admincontent.CaptureModeDesc.StructuredContent != null || admincontent.CaptureModeDesc.InterviewContent != null))
                                {
                                    if (formItemInfo != null && formItemInfo.Any())
                                    {
                                        var contentItemResponse = formItemInfo.FirstOrDefault(p => p.itemId == admincontent.ContentItemID);
                                        if (contentItemResponse != null)
                                        {
                                            admincontent.CaptureModeDesc.Notes = contentItemResponse.itemNotes;
                                            if (contentItemResponse.captureMode == "O")
                                            {
                                                admincontent.CaptureModeDesc.IsObservationSelected = true;
                                                if (admincontent.CaptureModeDesc.DefaultContentScoring != null && admincontent.CaptureModeDesc.DefaultContentScoring.Any())
                                                {
                                                    var score = admincontent.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                                    if (score != null)
                                                    {
                                                        score.IsSelected = true;
                                                        admincontent.CaptureModeDesc.SelectedDefaultRubicPoint = score;
                                                        admincontent.CaptureModeDesc.SelectedObservationContentRubicPoint = score;
                                                    }
                                                }
                                                else if (admincontent.CaptureModeDesc.ObservationContentScoring != null && admincontent.CaptureModeDesc.ObservationContentScoring.Any())
                                                {
                                                    var score = admincontent.CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                                    if (score != null)
                                                    {
                                                        score.IsSelected = true;
                                                        admincontent.CaptureModeDesc.SelectedObservationContentRubicPoint = score;
                                                    }
                                                }
                                            }
                                            else if (contentItemResponse.captureMode == "I")
                                            {
                                                admincontent.CaptureModeDesc.IsInterViewSelected = true;
                                                if (admincontent.CaptureModeDesc.DefaultContentScoring != null && admincontent.CaptureModeDesc.DefaultContentScoring.Any())
                                                {
                                                    var score = admincontent.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                                    if (score != null)
                                                    {
                                                        score.IsSelected = true;
                                                        admincontent.CaptureModeDesc.SelectedDefaultRubicPoint = score;
                                                        admincontent.CaptureModeDesc.SelectedInterviewContentRubicPoint = score;
                                                    }
                                                }
                                                else if (admincontent.CaptureModeDesc.InterviewContentScoring != null && admincontent.CaptureModeDesc.InterviewContentScoring.Any())
                                                {
                                                    var score = admincontent.CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                                    if (score != null)
                                                    {
                                                        score.IsSelected = true;
                                                        admincontent.CaptureModeDesc.SelectedInterviewContentRubicPoint = score;
                                                    }
                                                }
                                            }
                                            else if (contentItemResponse.captureMode == "S")
                                            {
                                                admincontent.CaptureModeDesc.IsStructredSelected = true;
                                                if (admincontent.CaptureModeDesc.DefaultContentScoring != null && admincontent.CaptureModeDesc.DefaultContentScoring.Any())
                                                {
                                                    var score = admincontent.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                                    if (score != null)
                                                    {
                                                        score.IsSelected = true;
                                                        admincontent.CaptureModeDesc.SelectedDefaultRubicPoint = score;
                                                        admincontent.CaptureModeDesc.SelectedStructuredContentRubicPoint = score;
                                                    }
                                                }
                                                else if (admincontent.CaptureModeDesc.StructuredContentScoring != null && admincontent.CaptureModeDesc.StructuredContentScoring.Any())
                                                {
                                                    var score = admincontent.CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                                    if (score != null)
                                                    {
                                                        score.IsSelected = true;
                                                        admincontent.CaptureModeDesc.SelectedStructuredContentRubicPoint = score;
                                                    }
                                                }
                                            }

                                            if (contentItemResponse.tallyItems != null && contentItemResponse.tallyItems.Any())
                                            {
                                                if (admincontent.TallyContent != null && admincontent.TallyContent.Any())
                                                {
                                                    foreach (var tallyitem in contentItemResponse.tallyItems)
                                                    {
                                                        var tallyContent = admincontent.TallyContent.FirstOrDefault(p => p.contentItemTallyId == tallyitem.itemTallyId);
                                                        if (tallyContent != null)
                                                        {
                                                            if (tallyitem.tallyScore == 0)
                                                            {
                                                                tallyContent.CheckInCorrectVisible = true;
                                                                tallyContent.UncheckInCorrectVisible = false;

                                                                tallyContent.CheckCorrectVisible = false;
                                                                tallyContent.UncheckCorrectVisible = true;
                                                            }
                                                            else
                                                            {
                                                                tallyContent.CheckCorrectVisible = true;
                                                                tallyContent.UncheckCorrectVisible = false;

                                                                tallyContent.CheckInCorrectVisible = false;
                                                                tallyContent.UncheckInCorrectVisible = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    subDomainData.AdministrationContents.Add(admincontent);
                                }
                            }
                        }
                        if (subDomainData != null && subDomainData.AdministrationContents != null && subDomainData.AdministrationContents.Any())
                        {
                            TotalSubdomainCollection.Add(subDomainData);
                        }
                    }
                }
            }
        }

        public List<ContentCategory> ScreenerDomain { get; set; } = new List<ContentCategory>();

        public async void LoadAllDomainDataForScreener()
        {
            await Task.Delay(0);
            OriginalStudentTestFormOverView = clinicalTestFormService.GetStudentTestFormsByID(LocaInstanceID);
            FormNotes = OriginalStudentTestFormOverView?.notes;
            studentTestFormResponses = studentTestFormResponsesService.GetStudentTestFormResponses(LocaInstanceID);
            if (studentTestFormResponses != null && studentTestFormResponses.Any())
            {
                var formJson = studentTestFormResponses.Where(p => !string.IsNullOrEmpty(p.Response)).Select(p => p.Response);
                foreach (var item in formJson)
                {
                    var jsonItems = JsonConvert.DeserializeObject<List<FormJsonClass>>(item);
                    if (jsonItems != null && jsonItems.Any())
                    {
                        foreach (var jsonitem in jsonItems)
                        {
                            if (jsonitem.items != null && jsonitem.items.Any())
                            {
                                formItemInfo.AddRange(jsonitem.items);
                            }
                        }
                    }
                }
            }
            ScreenerStartingPointCatrgories = new List<ContentCategory>();

            TotalCategories = commonDataService.TotalCategories;
            var contentcategorylevels = commonDataService.ContentCategoryLevels;
            var ContentCategory = commonDataService.TotalCategories;
            var contentCategoryItems = commonDataService.ContentCategoryItems;
            var contentitems = commonDataService.ContentItems;
            var contentItemAttribute = commonDataService.ContentItemAttributes;
            var contentRubrics = commonDataService.ContentRubrics;
            var contentRubicPoints = commonDataService.ContentRubricPoints;
            var contentTally = commonDataService.ContentItemTallies;
            var contentTallyScores = commonDataService.ContentItemTallyScores;

            foreach (var Category in ContentCategory)
            {
                if (Category.contentCategoryLevelId == 4)
                {
                    ScreenerDomain.Add(Category);
                }
                else if (Category.contentCategoryLevelId == 5)
                {
                    ScreenerStartingPointCatrgories.Add(Category);
                }
            }

            foreach (var domain in ScreenerDomain)
            {
                var domainContent = new DomainContent();
                domainContent.DomainName = domain.name;
                domainContent.DomainCode = domain.code;
                domainContent.DoaminCategoryId = domain.contentCategoryId;
                var testSessionOverView = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == domain.contentCategoryId);
                domainContent.rawscore = testSessionOverView.rawScore;
                domainContent.BasalCeilingObtained = testSessionOverView.BaselCeilingReached;
                var startingPoint = ScreenerStartingPointCatrgories.FirstOrDefault(p => p.parentContentCategoryId == domain.contentCategoryId);
                domainContent.CategoryId = startingPoint.contentCategoryId;

                var administrationContentCollection = new List<AdministrationContent>();

                var adaptiveitems = from categorylevels in contentcategorylevels
                                    join category in ContentCategory on categorylevels.contentCategoryLevelId equals category.contentCategoryLevelId
                                    join categoryItem in contentCategoryItems on category.contentCategoryId equals categoryItem.contentCategoryId
                                    join conitem in contentitems on categoryItem.contentItemId equals conitem.contentItemId
                                    where categorylevels.assessmentId == 42
                                    where category.parentContentCategoryId == domain.contentCategoryId
                                    select conitem;

                foreach (var item in adaptiveitems)
                {
                    var admincontent = new AdministrationContent();
                    var captureModeContent = new CaptureMode();
                    List<ContentRubricPoint> scoring = new List<ContentRubricPoint>();
                    List<ContentItemTally> contentItemTallies = new List<ContentItemTally>();
                    List<ImageSource> ImageSource = new List<ImageSource>();

                    admincontent.ItemAbbrevation = item.itemCode.Split(' ')[0];
                    admincontent.ItemNumber = item.itemNumber;
                    admincontent.MaxTime = item.maxTime1;
                    admincontent.BehaviorContent = item.itemText;
                    admincontent.BehaviourHtmlFIlePath = item.HtmlFilePath;
                    admincontent.ContentItemID = item.contentItemId;
                    captureModeContent.StructuredCollectionData = new List<CaptureModeContentModel>();
                    captureModeContent.ObservationCollectionData = new List<CaptureModeContentModel>();
                    captureModeContent.InterviewCollectionData = new List<CaptureModeContentModel>();
                    captureModeContent.DomainCategoryId = domain.contentCategoryId;

                    contentItemAttribute.OrderBy(o => o.contentItemAttributeId).ForEach(x =>
                    {
                        if (item.contentItemId == x.contentItemId)
                        {
                            var model = new CaptureModeContentModel() { ContentDescription = x.value, ContentID = x.contentItemId, HTMilFilePath = x.HtmlFilePath };
                            if (x.name == "Materials")
                            {
                                admincontent.MaterialContent = x.value;
                                admincontent.MaterialContentFilePath = x.HtmlFilePath;
                            }
                            if (x.name == "Structured")
                            {

                                if (captureModeContent.StructuredContent != null)
                                {
                                    captureModeContent.StructuredCollectionData.Add(model);
                                }
                                else
                                {
                                    captureModeContent.StructuredContent = model;
                                }
                            }
                            if (x.name == "Observation")
                            {
                                if (captureModeContent.ObservationContent != null)
                                {
                                    captureModeContent.ObservationCollectionData.Add(model);
                                }
                                else
                                {
                                    captureModeContent.ObservationContent = model;
                                }
                            }
                            if (x.name == "Interview")
                            {
                                if (captureModeContent.InterviewContent != null)
                                {
                                    captureModeContent.InterviewCollectionData.Add(model);
                                }
                                else
                                {
                                    captureModeContent.InterviewContent = model;
                                }
                            }
                            if (x.name == "Image")
                            {
                                ImageSource.Add(x.value);
                            }
                            admincontent.CaptureModeDesc = captureModeContent;
                        }
                    });
                    contentRubrics.ForEach(x =>
                    {
                        if (x.contentItemId == item.contentItemId)
                        {
                            admincontent.ScoringNotes = x.pointsDesc;
                            admincontent.ScoringCustomData = x.notes;
                            if (x.title == "Scoring: Structured")
                            {
                                admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Interview")
                            {
                                admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Observation")
                            {
                                admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Observation and Interview" || x.title == "Scoring: Interview and Observation")
                            {
                                admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Observation and Structured" || x.title == "Scoring: Structured and Observation")
                            {
                                admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Structured and Interview" || x.title == "Scoring: Interview and Structured")
                            {
                                admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;

                                admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Structured and Observation" || x.title == "Scoring: Observation and Structured")
                            {
                                admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Interview and Structured" || x.title == "Scoring: Structured and Interview")
                            {
                                admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;

                                admincontent.CaptureModeDesc.StructureScoringNote = x.notes;
                                admincontent.CaptureModeDesc.StructureScoringDesc = x.pointsDesc;
                            }
                            else if (x.title == "Scoring: Interview and Observation" || x.title == "Scoring: Observation and Interview")
                            {
                                admincontent.CaptureModeDesc.ObservationeScoringNote = x.notes;
                                admincontent.CaptureModeDesc.ObservationeScoringDesc = x.pointsDesc;

                                admincontent.CaptureModeDesc.InterviewScoringNote = x.notes;
                                admincontent.CaptureModeDesc.InterviewScoringDesc = x.pointsDesc;
                            }

                            contentRubicPoints.ForEach(y =>
                            {
                                if (x.contentRubricId == y.contentRubricId)
                                {
                                    var score = new ContentRubricPoint();
                                    score.points = y.points;
                                    score.description = y.description;
                                    score.contentRubricId = y.contentRubricId;
                                    score.contentRubricPointsId = y.contentRubricPointsId;
                                    score.HtmlFilePath = y.HtmlFilePath;
                                    if (x.title == "Scoring: Structured")
                                    {
                                        admincontent.CaptureModeDesc.StructuredContentScoring.Add(score);
                                    }
                                    else if (x.title == "Scoring: Interview")
                                    {
                                        admincontent.CaptureModeDesc.InterviewContentScoring.Add(score);
                                    }
                                    else if (x.title == "Scoring: Observation")
                                    {
                                        admincontent.CaptureModeDesc.ObservationContentScoring.Add(score);
                                    }
                                    else
                                    {
                                        admincontent.CaptureModeDesc.DefaultContentScoring.Add(score);
                                    }
                                }
                            });
                        }
                    });
                    contentTally.ForEach(x =>
                    {
                        if (x.contentItemId == item.contentItemId)
                        {
                            var tally = new ContentItemTally();
                            tally.contentItemId = item.contentItemId;
                            tally.contentItemTallyId = x.contentItemTallyId;
                            tally.text = x.text;
                            tally.CorrectLayoutAction = CorrectAction;
                            tally.InCorrectLayoutAction = InCorrectAction;
                            contentItemTallies.Add(tally);
                            admincontent.TallyContent.Add(tally);
                        }
                    });
                    scoring = new List<ContentRubricPoint>(scoring.OrderByDescending(a => a.points));
                    admincontent.Images = ImageSource;


                    if (admincontent.CaptureModeDesc != null &&
                        (admincontent.CaptureModeDesc.ObservationContent != null || admincontent.CaptureModeDesc.StructuredContent != null || admincontent.CaptureModeDesc.InterviewContent != null))
                    {
                        if (formItemInfo != null && formItemInfo.Any())
                        {
                            var contentItemResponse = formItemInfo.FirstOrDefault(p => p.itemId == admincontent.ContentItemID);
                            if (contentItemResponse != null)
                            {
                                admincontent.CaptureModeDesc.Notes = contentItemResponse.itemNotes;
                                if (contentItemResponse.captureMode == "O")
                                {
                                    admincontent.CaptureModeDesc.IsObservationSelected = true;
                                    if (admincontent.CaptureModeDesc.DefaultContentScoring != null && admincontent.CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        var score = admincontent.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                        if (score != null)
                                        {
                                            score.IsSelected = true;
                                            admincontent.CaptureModeDesc.SelectedDefaultRubicPoint = score;
                                            admincontent.CaptureModeDesc.SelectedObservationContentRubicPoint = score;
                                        }
                                    }
                                    else if (admincontent.CaptureModeDesc.ObservationContentScoring != null && admincontent.CaptureModeDesc.ObservationContentScoring.Any())
                                    {
                                        var score = admincontent.CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                        if (score != null)
                                        {
                                            score.IsSelected = true;
                                            admincontent.CaptureModeDesc.SelectedObservationContentRubicPoint = score;
                                        }
                                    }
                                }
                                else if (contentItemResponse.captureMode == "I")
                                {
                                    admincontent.CaptureModeDesc.IsInterViewSelected = true;
                                    if (admincontent.CaptureModeDesc.DefaultContentScoring != null && admincontent.CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        var score = admincontent.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                        if (score != null)
                                        {
                                            score.IsSelected = true;
                                            admincontent.CaptureModeDesc.SelectedDefaultRubicPoint = score;
                                            admincontent.CaptureModeDesc.SelectedInterviewContentRubicPoint = score;
                                        }
                                    }
                                    else if (admincontent.CaptureModeDesc.InterviewContentScoring != null && admincontent.CaptureModeDesc.InterviewContentScoring.Any())
                                    {
                                        var score = admincontent.CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                        if (score != null)
                                        {
                                            score.IsSelected = true;
                                            admincontent.CaptureModeDesc.SelectedInterviewContentRubicPoint = score;
                                        }
                                    }
                                }
                                else if (contentItemResponse.captureMode == "S")
                                {
                                    admincontent.CaptureModeDesc.IsStructredSelected = true;
                                    if (admincontent.CaptureModeDesc.DefaultContentScoring != null && admincontent.CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        var score = admincontent.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                        if (score != null)
                                        {
                                            score.IsSelected = true;
                                            admincontent.CaptureModeDesc.SelectedDefaultRubicPoint = score;
                                            admincontent.CaptureModeDesc.SelectedStructuredContentRubicPoint = score;
                                        }
                                    }
                                    else if (admincontent.CaptureModeDesc.StructuredContentScoring != null && admincontent.CaptureModeDesc.StructuredContentScoring.Any())
                                    {
                                        var score = admincontent.CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                        if (score != null)
                                        {
                                            score.IsSelected = true;
                                            admincontent.CaptureModeDesc.SelectedStructuredContentRubicPoint = score;
                                        }
                                    }
                                }

                                if (contentItemResponse.tallyItems != null && contentItemResponse.tallyItems.Any())
                                {
                                    if (admincontent.TallyContent != null && admincontent.TallyContent.Any())
                                    {
                                        foreach (var tallyitem in contentItemResponse.tallyItems)
                                        {
                                            var tallyContent = admincontent.TallyContent.FirstOrDefault(p => p.contentItemTallyId == tallyitem.itemTallyId);
                                            if (tallyContent != null)
                                            {
                                                if (tallyitem.tallyScore == 0)
                                                {
                                                    tallyContent.CheckInCorrectVisible = true;
                                                    tallyContent.UncheckInCorrectVisible = false;

                                                    tallyContent.CheckCorrectVisible = false;
                                                    tallyContent.UncheckCorrectVisible = true;
                                                }
                                                else
                                                {
                                                    tallyContent.CheckCorrectVisible = true;
                                                    tallyContent.UncheckCorrectVisible = false;

                                                    tallyContent.CheckInCorrectVisible = false;
                                                    tallyContent.UncheckInCorrectVisible = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        administrationContentCollection.Add(admincontent);
                    }


                }

                domainContent.AdministrationContents = administrationContentCollection;
                TotalDomainCollection.Add(domainContent);
            }
        }

        public async Task Initialize(string domain, int currentposition = 0)
        {
            LastItemScored = null;
            TallyBckgrdColor = Color.White;
            TallyTextColor = Color.Black;
            ScoreBckgrdColor = Color.LightGray;
            ScoreTextColor = Color.White;

            CurrentQuestion = currentposition;
            CurrentSubDomain = "";
            IsNextShown = IsPreviousShown = false;
            SubdomainCollection.Clear();

            TimerStatusText = "Start";
            TimerButtonBckgrd = Color.FromHex("#478128");
            TimerReset = "iconrefreshgray.png";
            _timer.Stop();
            StartEnabled = IsTimerInProgress = true;
            TotalSeconds = new TimeSpan(0, 0, 0, 0);

            if (IsBattelleDevelopmentCompleteChecked)
            {
                CheckScoreSelected();
                CalculateBaselCeiling();
                AssessmentType = "BATTELLE DEVELOPMENTAL COMPLETE";
                //TODO: This enum should be passed from constructor instead bool value.
                AssessmentConfigurationType = AssessmentConfigurationType.BattelleDevelopmentComplete;
                if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                {
                    var domaindData = TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCode.ToLower() == domain.ToLower());
                    if (domaindData != null && domaindData.AdministrationContents != null && domaindData.AdministrationContents.Any())
                    {
                        /*AdministrationHeader = value.Title;
                        AdministrationHeaderBackgroundColor = value.HeaderColor;*/

                        SubdomainCollection = new List<AdministrationContent>(domaindData.AdministrationContents.OrderBy(a => Convert.ToInt32(a.ItemNumber)));
                        UpdateItemNavigationPageHeader(domaindData.ParentCategoryName.ToUpper(), domaindData.SubDomainName, domaindData.SubDomainCode);
                        CurrentSubDomain = domaindData.SubDomainCode;
                        var positionofSubdomain = TotalSubdomainCollection.IndexOf(domaindData);
                        IsPreviousShown = positionofSubdomain > 0 || CurrentQuestion > 0;
                        if (positionofSubdomain == 0 && CurrentQuestion == 0)
                        {
                            IsPreviousShown = false;
                        }
                        if (positionofSubdomain == TotalSubdomainCollection.Count - 1 && CurrentQuestion == SubdomainCollection.Count - 1)
                        {
                            IsNextShown = false;
                        }
                        else
                        {
                            IsNextShown = SubdomainCollection.Count > 0;
                        }
                        ItemNumber = SubdomainCollection[CurrentQuestion].ItemNumber;
                        ItemAbbrevation = SubdomainCollection[CurrentQuestion].ItemAbbrevation;
                        BehaviorText = SubdomainCollection[CurrentQuestion].BehaviorContent;
                        BehaviorTextFilePath = SubdomainCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                        MaterialText = SubdomainCollection[CurrentQuestion].MaterialContent;
                        MaterialHtmlFilePath = SubdomainCollection[CurrentQuestion].MaterialContentFilePath;
                        MaxTime = SubdomainCollection[CurrentQuestion].MaxTime;
                        ScoringText = SubdomainCollection[CurrentQuestion].ScoringNotes;
                        ScoringNotes = SubdomainCollection[CurrentQuestion].ScoringCustomData;
                        TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");

                        StructureEnabled = false;
                        StructureDisabled = Color.FromHex("#979797");
                        CheckStructure = false;
                        StructureBorderColor = Color.Gray;
                        StructureFontAttribute = FontAttributes.None;

                        ObservationEnabled = false;
                        ObservedDisabled = Color.FromHex("#979797");
                        CheckObservation = false;
                        ObservationBorderColor = Color.Gray;
                        ObservationFontAttribute = FontAttributes.None;

                        InterviewEnabled = false;
                        InterviewDisabled = Color.FromHex("#979797");
                        CheckInterview = false;
                        InterviewBorderColor = Color.Gray;
                        InterviewFontAttribute = FontAttributes.None;

                        TallyListviewCheckboxEnabled = false;
                        TallyListviewChecboxkDisabled = Color.FromHex("#979797");
                        TallyListviewCheckbox = false;
                        TallyListviewCheckboxBorderColor = Color.Gray;
                        TallyListviewCheckboxFontAttribute = FontAttributes.None;

                        StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");

                        if (SubdomainCollection[CurrentQuestion].TallyContent != null)
                        {
                            if (SubdomainCollection[CurrentQuestion].TallyContent.Count == 0)
                            {
                                ContentItemTalliesCollection = null;
                                IsTallyAndScoring = false;
                                IsScoringOnly = true;
                                //ScoringTabTappedCommand.Execute(new object());
                            }


                            if (SubdomainCollection[CurrentQuestion].TallyContent.Count > 0)
                            {
                                ContentItemTalliesCollection = SubdomainCollection[CurrentQuestion].TallyContent;
                                IsTallyAndScoring = true;
                                IsScoringOnly = false;
                                //TallyTabTappedCommand.Execute(new object());
                            }

                        }
                        if (SubdomainCollection[CurrentQuestion].TallyContent != null && SubdomainCollection[CurrentQuestion].TallyContent.Count > 0 && (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                        {
                            TallyTabTappedCommand.Execute(new object());
                        }
                        else
                        {
                            ScoringTabTappedCommand.Execute(new object());
                        }
                        ObservationStr = "Observation";
                        InterviewStr = "Interview";
                        StructuredStr = "Structured";
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc != null)
                        {
                            CaptureMode = SubdomainCollection[CurrentQuestion].CaptureModeDesc;
                            StructureEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null;
                            ObservationEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null;
                            InterviewEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null;

                            if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                            {
                                ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                                InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                                ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                                InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count == 0)
                            {

                                if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 3)
                                {
                                    ImagePath1 = null;
                                    ImagePath2 = null;
                                    ImagePath3 = null;
                                    string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                    string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                                    string image3FileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();

                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                    if (existfolder == ExistenceCheckResult.FolderExists)
                                    {
                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                        ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                        ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                                        ExistenceCheckResult exist3 = await imageFolder.CheckExistsAsync(image3FileName + ".png");

                                        if (exist1 == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                            ImagePath1 = imageFile1.Path;
                                        }

                                        if (exist2 == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                            ImagePath2 = imageFile2.Path;
                                        }

                                        if (exist3 == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile3 = await imageFolder.GetFileAsync(image3FileName + ".png");
                                            ImagePath3 = imageFile3.Path;
                                        }
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 2)
                                {
                                    ImagePath1 = null;
                                    ImagePath2 = null;
                                    ImagePath3 = null;
                                    string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                    string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                    if (existfolder == ExistenceCheckResult.FolderExists)
                                    {
                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                        ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                        ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");

                                        if (exist1 == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                            ImagePath1 = imageFile1.Path;
                                        }

                                        if (exist2 == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                            ImagePath2 = imageFile2.Path;
                                        }
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count > 0)
                                {
                                    ImagePath1 = null;
                                    ImagePath2 = null;
                                    ImagePath3 = null;
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                    if (existfolder == ExistenceCheckResult.FolderExists)
                                    {
                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                        ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                        if (exist == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                            ImagePath1 = imageFile1.Path;
                                        }
                                    }

                                }
                                else
                                {
                                    ImagePath1 = null;
                                    ImagePath2 = null;
                                    ImagePath3 = null;
                                }

                            }
                            var IsSelected = captureMode.IsStructredSelected || captureMode.IsObservationSelected || captureMode.IsInterViewSelected;
                            if (!IsSelected)
                            {
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    captureMode.IsStructredSelected = true;
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    captureMode.IsObservationSelected = true;
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    captureMode.IsInterViewSelected = true;
                                }
                            }
                            if (captureMode.IsStructredSelected)
                            {
                                captureMode.IsStructredSelected = true;
                                CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                                CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                                StructureEnabled = true;
                                CheckStructure = true;
                                StructureBorderColor = Colors.FrameBlueColor;
                                StructureFontAttribute = FontAttributes.Bold;

                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                                }
                                else
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    StructureEnabled = true;
                                }
                                else
                                {
                                    StructureEnabled = false;
                                    StructureDisabled = Color.FromHex("#979797");
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    ObservationEnabled = true;
                                }
                                else
                                {
                                    ObservationEnabled = false;
                                    ObservedDisabled = Color.FromHex("#979797");
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    InterviewEnabled = true;
                                }
                                else
                                {
                                    InterviewEnabled = false;
                                    InterviewDisabled = Color.FromHex("#979797");
                                }

                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                                {
                                    ImagePath1 = ImagePath2 = ImagePath3 = null;

                                    if (SubdomainCollection[CurrentQuestion].Images.Count > 0)
                                    {
                                        string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                        ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                        if (exist == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                            ImagePath1 = imageFile1.Path;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote))
                                {
                                    ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                    ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                                }
                            }

                            if (captureMode.IsObservationSelected)
                            {
                                captureMode.IsObservationSelected = true;
                                CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                                CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                                ObservationEnabled = true;
                                CheckObservation = true;
                                ObservationBorderColor = Colors.FrameBlueColor;
                                ObservationFontAttribute = FontAttributes.Bold;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                                }
                                else
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    StructureEnabled = true;
                                }
                                else
                                {
                                    StructureEnabled = false;
                                    StructureDisabled = Color.FromHex("#979797");
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    ObservationEnabled = true;
                                }
                                else
                                {
                                    ObservationEnabled = false;
                                    ObservedDisabled = Color.FromHex("#979797");
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    InterviewEnabled = true;
                                }
                                else
                                {
                                    InterviewEnabled = false;
                                    InterviewDisabled = Color.FromHex("#979797");
                                }

                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                                {
                                    ImagePath1 = ImagePath2 = ImagePath3 = null;

                                    if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                                    {
                                        string imageFileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                        ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                        if (exist == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                            ImagePath1 = imageFile1.Path;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote))
                                {
                                    ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                    ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                                }
                            }

                            if (captureMode.IsInterViewSelected)
                            {
                                captureMode.IsInterViewSelected = true;
                                CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                                CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                                InterviewEnabled = true;
                                CheckInterview = true;
                                InterviewBorderColor = Colors.FrameBlueColor;
                                InterviewFontAttribute = FontAttributes.Bold;

                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                                }
                                else
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    StructureEnabled = true;
                                }
                                else
                                {
                                    //Set it for whole Strucuture StackLayout.
                                    StructureEnabled = false;
                                    //Structure Text color changed.
                                    StructureDisabled = Color.FromHex("#979797");
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    ObservationEnabled = true;
                                }
                                else
                                {
                                    ObservationEnabled = false;
                                    ObservedDisabled = Color.FromHex("#979797");
                                }
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    InterviewEnabled = true;
                                }
                                else
                                {
                                    InterviewEnabled = false;
                                    InterviewDisabled = Color.FromHex("#979797");
                                }

                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                                {
                                    ImagePath1 = ImagePath2 = ImagePath3 = null;

                                    if (SubdomainCollection[CurrentQuestion].Images.Count > 2)
                                    {
                                        string imageFileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();
                                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                        ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                        if (exist == ExistenceCheckResult.FileExists)
                                        {
                                            var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                            ImagePath1 = imageFile1.Path;
                                        }
                                    }
                                }

                                if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                                {
                                    ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                    ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                                }
                            }
                            CalculateBaselCeiling();
                        }
                    }
                    else
                    {
                        ItemAbbrevation = "Sample (Content not downloaded)";
                        BehaviorText = "Sample (Content not downloaded)";
                        MaterialText = "Sample (Content not downloaded)";
                    }
                }
            }
            else
            {
                AssessmentType = "BATTELLE DEVELOPMENTAL SCREENER";
                AssessmentConfigurationType = AssessmentConfigurationType.BattelleDevelopmentScreener;
                CheckScoreSelected();
                CalculateBaselCeiling();
                if (TotalDomainCollection != null && TotalDomainCollection.Any())
                {
                    var domaindData = TotalDomainCollection.FirstOrDefault(p => p.DomainName.ToLower() == domain.ToLower());
                    if (domaindData != null && domaindData.AdministrationContents != null && domaindData.AdministrationContents.Any())
                    {

                        //var adaptivedomain = TotalDomainCollection[0];
                        //ItemNumber = adaptivedomain.AdministrationContents[0].ItemNumber;
                        //ItemAbbrevation = adaptivedomain.AdministrationContents[0].ItemAbbrevation;
                        //BehaviorText = adaptivedomain.AdministrationContents[0].BehaviorContent;
                        //MaterialText = adaptivedomain.AdministrationContents[0].MaterialContent;

                        domainContentCollection = new List<AdministrationContent>(domaindData.AdministrationContents.OrderBy(a => Convert.ToInt32(a.ItemNumber)));
                        UpdateItemNavigationPageHeader(domain.ToUpper(), "", "");
                        CurrentDomain = domaindData.DomainName;
                        var positionofdomain = TotalDomainCollection.IndexOf(domaindData);
                        IsPreviousShown = positionofdomain > 0 || CurrentQuestion > 0;
                        if (positionofdomain == 0 && CurrentQuestion == 0)
                        {
                            IsPreviousShown = false;
                        }
                        if (positionofdomain == TotalDomainCollection.Count - 1 && CurrentQuestion == domainContentCollection.Count - 1)
                        {
                            IsNextShown = false;
                        }
                        else
                        {
                            IsNextShown = domainContentCollection.Count > 0;
                        }
                        ItemNumber = domainContentCollection[CurrentQuestion].ItemNumber;
                        ItemAbbrevation = domainContentCollection[CurrentQuestion].ItemAbbrevation;
                        BehaviorText = domainContentCollection[CurrentQuestion].BehaviorContent;
                        BehaviorTextFilePath = domainContentCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                        MaterialText = domainContentCollection[CurrentQuestion].MaterialContent;
                        MaterialHtmlFilePath = domainContentCollection[CurrentQuestion].MaterialContentFilePath;
                        MaxTime = domainContentCollection[CurrentQuestion].MaxTime;
                        ScoringText = domainContentCollection[CurrentQuestion].ScoringNotes;
                        ScoringNotes = domainContentCollection[CurrentQuestion].ScoringCustomData;
                        TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                        StructureEnabled = false;
                        StructureDisabled = Color.FromHex("#979797");
                        CheckStructure = false;
                        StructureBorderColor = Color.Gray;
                        StructureFontAttribute = FontAttributes.None;

                        ObservationEnabled = false;
                        ObservedDisabled = Color.FromHex("#979797");
                        CheckObservation = false;
                        ObservationBorderColor = Color.Gray;
                        ObservationFontAttribute = FontAttributes.None;

                        InterviewEnabled = false;
                        InterviewDisabled = Color.FromHex("#979797");
                        CheckInterview = false;
                        InterviewBorderColor = Color.Gray;
                        InterviewFontAttribute = FontAttributes.None;

                        TallyListviewCheckboxEnabled = false;
                        TallyListviewChecboxkDisabled = Color.FromHex("#979797");
                        TallyListviewCheckbox = false;
                        TallyListviewCheckboxBorderColor = Color.Gray;
                        TallyListviewCheckboxFontAttribute = FontAttributes.None;

                        StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");

                        if (domainContentCollection[CurrentQuestion].TallyContent != null)
                        {
                            if (domainContentCollection[CurrentQuestion].TallyContent.Count == 0)
                            {
                                ContentItemTalliesCollection = null;
                                IsTallyAndScoring = false;
                                IsScoringOnly = true;
                                //ScoringTabTappedCommand.Execute(new object());
                            }


                            if (domainContentCollection[CurrentQuestion].TallyContent.Count > 0)
                            {
                                ContentItemTalliesCollection = domainContentCollection[CurrentQuestion].TallyContent;
                                IsTallyAndScoring = true;
                                IsScoringOnly = false;
                                //TallyTabTappedCommand.Execute(new object());
                            }

                        }
                        if (domainContentCollection[CurrentQuestion].TallyContent != null && domainContentCollection[CurrentQuestion].TallyContent.Count > 0 && (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                        {
                            TallyTabTappedCommand.Execute(new object());
                        }
                        else
                        {
                            ScoringTabTappedCommand.Execute(new object());
                        }
                        ObservationStr = "Observation";
                        InterviewStr = "Interview";
                        StructuredStr = "Structured";
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc != null)
                        {
                            CaptureMode = domainContentCollection[CurrentQuestion].CaptureModeDesc;
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                StructureEnabled = true;
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                ObservationEnabled = true;
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                InterviewEnabled = true;

                            if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any() && !ObservationEnabled && !InterviewEnabled)
                            {
                                ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                                InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                                ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                                InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                            }
                            if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count == 2)
                            {
                                string image1FileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                string image2FileName = (domainContentCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                if (existfolder == ExistenceCheckResult.FolderExists)
                                {
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                    ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");

                                    if (exist1 == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }

                                    if (exist2 == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                        ImagePath2 = imageFile2.Path;
                                    }
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count > 0)
                            {
                                string imageFileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                if (existfolder == ExistenceCheckResult.FolderExists)
                                {
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }

                            }
                            else
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                            }

                            var IsSelected = captureMode.IsStructredSelected || captureMode.IsObservationSelected || captureMode.IsInterViewSelected;
                            if (!IsSelected)
                            {
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    captureMode.IsStructredSelected = true;
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    captureMode.IsObservationSelected = true;
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    captureMode.IsInterViewSelected = true;
                                }
                            }

                            if (captureMode.IsStructredSelected)
                            {
                                captureMode.IsStructredSelected = true;
                                CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                                CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                                StructureEnabled = true;
                                if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote))
                                {
                                    ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                    ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                                }
                                CheckStructure = true;
                                StructureBorderColor = Colors.FrameBlueColor;
                                StructureFontAttribute = FontAttributes.Bold;

                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                                }
                                else
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    StructureEnabled = true;
                                }
                                else
                                {
                                    //Set it for whole Strucuture StackLayout.
                                    StructureEnabled = false;
                                    //Structure Text color changed.
                                    StructureDisabled = Color.FromHex("#979797");
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    ObservationEnabled = true;
                                }
                                else
                                {
                                    ObservationEnabled = false;
                                    ObservedDisabled = Color.FromHex("#979797");
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    InterviewEnabled = true;
                                }
                                else
                                {
                                    InterviewEnabled = false;
                                    InterviewDisabled = Color.FromHex("#979797");
                                }
                                CalculateBaselCeiling();
                            }

                            if (captureMode.IsObservationSelected)
                            {
                                captureMode.IsObservationSelected = true;
                                CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                                CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                                ObservationEnabled = true;
                                CheckObservation = true;
                                if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote))
                                {
                                    ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                    ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                                }
                                ObservationBorderColor = Colors.FrameBlueColor;
                                ObservationFontAttribute = FontAttributes.Bold;


                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                                }
                                else
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    StructureEnabled = true;
                                }
                                else
                                {
                                    //Set it for whole Strucuture StackLayout.
                                    StructureEnabled = false;
                                    //Structure Text color changed.
                                    StructureDisabled = Color.FromHex("#979797");
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    ObservationEnabled = true;
                                }
                                else
                                {
                                    ObservationEnabled = false;
                                    ObservedDisabled = Color.FromHex("#979797");
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    InterviewEnabled = true;
                                }
                                else
                                {
                                    InterviewEnabled = false;
                                    InterviewDisabled = Color.FromHex("#979797");
                                }
                                CalculateBaselCeiling();
                            }

                            if (captureMode.IsInterViewSelected)
                            {
                                captureMode.IsInterViewSelected = true;
                                CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                                CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                                InterviewEnabled = true;
                                CheckInterview = true;
                                if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                                {
                                    ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                    ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                                }
                                InterviewBorderColor = Colors.FrameBlueColor;
                                InterviewFontAttribute = FontAttributes.Bold;

                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                                }
                                else
                                {
                                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                                {
                                    StructureEnabled = true;
                                }
                                else
                                {
                                    //Set it for whole Strucuture StackLayout.
                                    StructureEnabled = false;
                                    //Structure Text color changed.
                                    StructureDisabled = Color.FromHex("#979797");
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                                {
                                    ObservationEnabled = true;
                                }
                                else
                                {
                                    ObservationEnabled = false;
                                    ObservedDisabled = Color.FromHex("#979797");
                                }
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                                {
                                    InterviewEnabled = true;
                                }
                                else
                                {
                                    InterviewEnabled = false;
                                    InterviewDisabled = Color.FromHex("#979797");
                                }
                                CalculateBaselCeiling();
                            }

                        }
                    }
                }
                else
                {
                    ItemAbbrevation = "Sample (Content not downloaded)";
                    BehaviorText = "Sample (Content not downloaded)";
                    MaterialText = "Sample (Content not downloaded)";
                }
            }

            if (CaptureMode != null)
            {
                var studentTestForms = commonDataService.StudentTestForms;
                if (studentTestForms != null && studentTestForms.Any())
                {
                    var testOverview = studentTestForms.FirstOrDefault(p => p.contentCategoryId == CaptureMode.DomainCategoryId);
                    if (testOverview != null)
                    {
                        TestDate = testOverview.testDate;
                    }
                }
            }
        }
        public async void OpenItemLevelNavigation()
        {
            ResetTimer();
            if (popupOpenClicked)
                return;
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack)
                {
                    await PopupNavigation.Instance.PopAsync();
                    if (popup is ItemLevelNavigationPage && (popup as ItemLevelNavigationPage).Title == "ItemLevelNaviagationTitle")
                    {
                        return;
                    }
                }
            }
            popupOpenClicked = true;
            if (AssessmentConfigurationType == AssessmentConfigurationType.BattelleDevelopmentComplete)
            {
                GenerateMenuItems();
            }
            else if (AssessmentConfigurationType == AssessmentConfigurationType.BattelleDevelopmentScreener)
            {
                GenerateScreenerItems();
            }
            await PopupNavigation.Instance.PushAsync(new ItemLevelNavigationPage() { BindingContext = this, Title = "ItemLevelNaviagationTitle" });
            popupOpenClicked = false;
        }

        public async void CloseTreeView()
        {
            var popNavigationInstance = PopupNavigation.Instance;
            await popNavigationInstance.PopAsync();
        }

        public async void NavigateToInformationPage()
        {
            if (isTestRecordNavigationOpen)
                return;
            if (PopupNavigation.Instance.PopupStack.Count > 1)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                {
                    await PopupNavigation.Instance.PopAsync();
                    if (popup is TestRecordInformationPage && (popup as TestRecordInformationPage).Title == "TestRecordLevelNavigationTitle")
                    {
                        return;
                    }
                }
            }
            isTestRecordNavigationOpen = true;

            await PopupNavigation.Instance.PushAsync(new TestRecordInformationPage() { BindingContext = this, Title = "TestRecordLevelNavigationTitle" });
            isTestRecordNavigationOpen = false;

        }

        public async void NavigateToItemLevelPage()
        {
            var popNavigationInstance = PopupNavigation.Instance;
            await popNavigationInstance.PopAsync();

            await PopupNavigation.Instance.PushAsync(new ItemLevelNavigationPage(), false);
        }

        public async void NextItemClicked()
        {
            //CanInitiateNextItemTap(false);
            //UserDialogs.Instance.ShowLoading("Loading");
            //await Task.Delay(2000);
            var positionofSubdomain = -1;
            var positionofdomain = -1;

            ResetTimer();

            if (IsBattelleDevelopmentCompleteChecked)
            {
                if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                {
                    positionofSubdomain = TotalSubdomainCollection.IndexOf(TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCode == CurrentSubDomain));
                    if (positionofSubdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    IsNextShown = positionofSubdomain < TotalSubdomainCollection.Count - 1;
                }

                if (CurrentQuestion < SubdomainCollection.Count - 1)
                {
                    CurrentQuestion += 1;
                    ObservationStr = "Observation";
                    InterviewStr = "Interview";
                    StructuredStr = "Structured";
                    CheckStructure = CheckObservation = CheckInterview = false;
                    StructureFontAttribute = ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                    StructureBorderColor = ObservationBorderColor = InterviewBorderColor = Color.Gray;
                    StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");
                    ItemNumber = SubdomainCollection[CurrentQuestion].ItemNumber;
                    ItemAbbrevation = SubdomainCollection[CurrentQuestion].ItemAbbrevation;// itemCode.Split(' ')[0];
                    BehaviorText = SubdomainCollection[CurrentQuestion].BehaviorContent;// itemText;
                    BehaviorTextFilePath = SubdomainCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                    MaterialText = SubdomainCollection[CurrentQuestion].MaterialContent;
                    MaterialHtmlFilePath = SubdomainCollection[CurrentQuestion].MaterialContentFilePath;
                    MaxTime = SubdomainCollection[CurrentQuestion].MaxTime;
                    ScoringText = SubdomainCollection[CurrentQuestion].ScoringNotes;
                    ScoringNotes = SubdomainCollection[CurrentQuestion].ScoringCustomData;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                    IsPreviousShown = true;
                    IsNextShown = (CurrentQuestion < SubdomainCollection.Count - 1) || positionofSubdomain < TotalSubdomainCollection.Count - 1;

                    if (SubdomainCollection[CurrentQuestion].TallyContent != null)
                    {
                        if (SubdomainCollection[CurrentQuestion].TallyContent.Count == 0)
                        {
                            ContentItemTalliesCollection = null;
                            IsScoringOnly = true;
                            IsTallyAndScoring = false;
                        }

                        if (SubdomainCollection[CurrentQuestion].TallyContent.Count > 0)
                        {
                            ContentItemTalliesCollection = SubdomainCollection[CurrentQuestion].TallyContent;
                            IsScoringOnly = false;
                            IsTallyAndScoring = true;
                        }
                    }

                    if (SubdomainCollection[CurrentQuestion].TallyContent != null && SubdomainCollection[CurrentQuestion].TallyContent.Count > 0 && (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                    {
                        TallyTabTappedCommand.Execute(new object());
                    }
                    else
                    {
                        ScoringTabTappedCommand.Execute(new object());
                    }

                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc != null)
                    {
                        CaptureMode = SubdomainCollection[CurrentQuestion].CaptureModeDesc;
                        StructureEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null;
                        ObservationEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null;
                        InterviewEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null;

                        if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                        {
                            ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                            InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                            ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                            InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                        }
                        var noSelection = !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (!noSelection)
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = false;
                                }
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = false;
                                }
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = false;
                                }
                            }
                        }
                        noSelection = !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (noSelection)
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = true;
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = true;
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = true;
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                            CheckStructure = true;
                            StructureBorderColor = Colors.FrameBlueColor;
                            StructureFontAttribute = FontAttributes.Bold;
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;

                                if (SubdomainCollection[CurrentQuestion].Images.Count > 0)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                            CheckObservation = true;
                            ObservationBorderColor = Colors.FrameBlueColor;
                            ObservationFontAttribute = FontAttributes.Bold;

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                            CheckInterview = true;
                            InterviewBorderColor = Colors.FrameBlueColor;
                            InterviewFontAttribute = FontAttributes.Bold;

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 2)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }

                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                        {
                            StructureEnabled = true;
                        }
                        else
                        {
                            //Set it for whole Strucuture StackLayout.
                            StructureEnabled = false;
                            //Structure Text color changed.
                            StructureDisabled = Color.FromHex("#979797");
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                        {
                            ObservationEnabled = true;
                        }
                        else
                        {
                            ObservationEnabled = false;
                            ObservedDisabled = Color.FromHex("#979797");
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                        {
                            InterviewEnabled = true;
                        }
                        else
                        {
                            InterviewEnabled = false;
                            InterviewDisabled = Color.FromHex("#979797");
                        }
                    }

                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count == 0)
                    {
                        if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 3)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                            string image3FileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();

                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                            if (existfolder == ExistenceCheckResult.FolderExists)
                            {
                                var imageFolder = await rootFolder.GetFolderAsync("Images");
                                ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                                ExistenceCheckResult exist3 = await imageFolder.CheckExistsAsync(image3FileName + ".png");

                                if (exist1 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                    ImagePath1 = imageFile1.Path;
                                }

                                if (exist2 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                    ImagePath2 = imageFile2.Path;
                                }

                                if (exist3 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile3 = await imageFolder.GetFileAsync(image3FileName + ".png");
                                    ImagePath3 = imageFile3.Path;
                                }
                            }
                        }
                        else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 2)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            var imageFolder = await rootFolder.GetFolderAsync("Images");
                            ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                            ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");

                            if (exist1 == ExistenceCheckResult.FileExists)
                            {
                                var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                ImagePath1 = imageFile1.Path;
                            }

                            if (exist2 == ExistenceCheckResult.FileExists)
                            {
                                var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                ImagePath2 = imageFile2.Path;
                            }

                            /*ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image1FileName + ".png";
                            ImagePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image2FileName + ".png";*/
                        }
                        else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count > 0)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            var imageFolder = await rootFolder.GetFolderAsync("Images");
                            ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                            if (exist == ExistenceCheckResult.FileExists)
                            {
                                var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                ImagePath1 = imageFile1.Path;
                            }

                            //ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + imageFileName + ".png";
                        }
                        else
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                        }
                    }
                }
                else
                {
                    if (IsNextShown)
                    {
                        InitialLoad = true;
                        await Initialize(TotalSubdomainCollection[positionofSubdomain + 1].SubDomainCode);
                        InitialLoad = false;
                    }
                }
            }
            else
            {
                if (TotalDomainCollection != null && TotalDomainCollection.Any())
                {
                    positionofdomain = TotalDomainCollection.IndexOf(TotalDomainCollection.FirstOrDefault(p => p.DomainName == CurrentDomain));
                    if (positionofdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    IsNextShown = positionofdomain < TotalDomainCollection.Count - 1;
                }

                if (CurrentQuestion < domainContentCollection.Count - 1)
                {
                    CurrentQuestion += 1;
                    CheckStructure = CheckObservation = CheckInterview = false;
                    StructureFontAttribute = ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                    StructureBorderColor = ObservationBorderColor = InterviewBorderColor = Color.Gray;
                    StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");
                    ItemNumber = domainContentCollection[CurrentQuestion].ItemNumber;
                    ItemAbbrevation = domainContentCollection[CurrentQuestion].ItemAbbrevation;// itemCode.Split(' ')[0];
                    BehaviorText = domainContentCollection[CurrentQuestion].BehaviorContent;// itemText;
                    BehaviorTextFilePath = domainContentCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                    MaterialText = domainContentCollection[CurrentQuestion].MaterialContent;
                    MaterialHtmlFilePath = domainContentCollection[CurrentQuestion].MaterialContentFilePath;
                    MaxTime = domainContentCollection[CurrentQuestion].MaxTime;
                    ScoringText = domainContentCollection[CurrentQuestion].ScoringNotes;
                    ScoringNotes = domainContentCollection[CurrentQuestion].ScoringCustomData;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                    IsPreviousShown = true;
                    IsNextShown = (CurrentQuestion < domainContentCollection.Count - 1) || positionofdomain < TotalDomainCollection.Count - 1;

                    if (domainContentCollection[CurrentQuestion].TallyContent != null)
                    {
                        if (domainContentCollection[CurrentQuestion].TallyContent.Count == 0)
                        {
                            ContentItemTalliesCollection = null;
                            IsScoringOnly = true;
                            IsTallyAndScoring = false;
                        }

                        if (domainContentCollection[CurrentQuestion].TallyContent.Count > 0)
                        {
                            ContentItemTalliesCollection = domainContentCollection[CurrentQuestion].TallyContent;
                            IsScoringOnly = false;
                            IsTallyAndScoring = true;
                        }
                    }

                    if (domainContentCollection[CurrentQuestion].TallyContent != null && domainContentCollection[CurrentQuestion].TallyContent.Count > 0 && (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                    {
                        TallyTabTappedCommand.Execute(new object());
                    }
                    else
                    {
                        ScoringTabTappedCommand.Execute(new object());
                    }
                    ObservationStr = "Observation";
                    InterviewStr = "Interview";
                    StructuredStr = "Structured";
                    if (domainContentCollection[CurrentQuestion].CaptureModeDesc != null)
                    {
                        CaptureMode = domainContentCollection[CurrentQuestion].CaptureModeDesc;
                        StructureEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null;
                        ObservationEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null;
                        InterviewEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null;

                        if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                        {
                            ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                            InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                            ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                            InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                            domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                            domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                        }

                        var noSelection = !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (!noSelection)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = false;
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = false;
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = false;
                                }
                            }
                        }
                        noSelection = !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (noSelection)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = true;
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = true;
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = true;
                            }
                        }

                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                            CheckStructure = true;
                            if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote))
                            {
                                ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                            }
                            TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            StructureBorderColor = Colors.FrameBlueColor;
                            StructureFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                            CheckObservation = true;
                            if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote))
                            {
                                ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                            }
                            TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            ObservationBorderColor = Colors.FrameBlueColor;
                            ObservationFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                            CheckInterview = true;
                            if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                            }
                            TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            InterviewBorderColor = Colors.FrameBlueColor;
                            InterviewFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }

                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                        {
                            StructureEnabled = true;
                        }
                        else
                        {
                            //Set it for whole Strucuture StackLayout.
                            StructureEnabled = false;
                            //Structure Text color changed.
                            StructureDisabled = Color.FromHex("#979797");
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                        {
                            ObservationEnabled = true;
                        }
                        else
                        {
                            ObservationEnabled = false;
                            ObservedDisabled = Color.FromHex("#979797");
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                        {
                            InterviewEnabled = true;
                        }
                        else
                        {
                            InterviewEnabled = false;
                            InterviewDisabled = Color.FromHex("#979797");
                        }
                    }
                    if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count == 2)
                    {
                        string image1FileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                        string image2FileName = (domainContentCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                        ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                        ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                        if (exist1 == ExistenceCheckResult.FileExists)
                        {
                            var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                            ImagePath1 = imageFile1.Path;
                        }

                        if (exist2 == ExistenceCheckResult.FileExists)
                        {
                            var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                            ImagePath2 = imageFile2.Path;
                        }

                        /*ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image1FileName + ".png";
                        ImagePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image2FileName + ".png";*/
                    }
                    else if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count > 0)
                    {

                        string imageFileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                        ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                        if (exist == ExistenceCheckResult.FileExists)
                        {
                            var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                            ImagePath1 = imageFile1.Path;
                        }

                        //ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + imageFileName + ".png";
                    }
                    else
                    {
                        ImagePath1 = null;
                        ImagePath2 = null;
                    }
                }
                else
                {
                    if (IsNextShown)
                    {
                        InitialLoad = true;
                        await Initialize(TotalDomainCollection[positionofdomain + 1].DomainName);
                        InitialLoad = false;
                    }
                }

            }
            // UserDialogs.Instance.HideLoading();
            //await Task.Delay(1500);
            //CanInitiateNextItemTap(true);
            //Dispose();
        }
        #region CaptureModeRadioButton Event handlers
        public async void TappedStructured()
        {
            CaptureMode.IsStructredSelected = true;
            CheckStructure = !CheckStructure;
            StructureBorderColor = (StructureBorderColor.Equals(Colors.FrameBlueColor)) ? Color.Gray : Colors.FrameBlueColor;
            StructureFontAttribute = StructureFontAttribute.Equals(FontAttributes.Bold) ? FontAttributes.None : FontAttributes.Bold;
            if (IsBattelleDevelopmentCompleteChecked)
            {
                if (!string.IsNullOrEmpty(CaptureMode.StructureScoringDesc))
                {
                    ScoringText = CaptureMode.StructureScoringDesc;
                    ScoringNotes = CaptureMode.StructureScoringNote;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                }
            }
            else
            {
                if (captureMode.StructureScoringDesc != null || captureMode.StructureScoringNote != null)
                {
                    ScoringText = captureMode.StructureScoringDesc;
                    ScoringNotes = captureMode.StructureScoringNote;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                }
            }
            if (CheckObservation || CheckInterview)
            {
                CaptureMode.IsObservationSelected = CaptureMode.IsInterViewSelected = false;
                CheckObservation = CheckInterview = false;
                ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                ObservationBorderColor = InterviewBorderColor = Color.Gray;
            }
            if (CaptureMode.StructuredContent != null)
            {
                CaptureModeText = CaptureMode.StructuredContent.ContentDescription;
                CaptureModeTextFilePath = CaptureMode.StructuredContent.HTMilFilePath;
                if (CaptureMode.StructuredContentScoring != null && CaptureMode.StructuredContentScoring.Any())
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>(CaptureMode.StructuredContentScoring.OrderByDescending(p => p.points));
                }
                else if (CaptureMode.DefaultContentScoring != null && CaptureMode.DefaultContentScoring.Any())
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>(CaptureMode.DefaultContentScoring.OrderByDescending(p => p.points));
                }
                else
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                }
                ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                {
                    var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                    if (selectedScore != null)
                    {
                        selectedScore.IsSelected = true;
                    }
                }
                else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                {
                    var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                    if (selectedScore != null)
                    {
                        selectedScore.IsSelected = true;
                    }
                }
            }

            if (this.IsBattelleDevelopmentCompleteChecked && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
            {
                ImagePath1 = null;
                ImagePath2 = null;
                if (SubdomainCollection[CurrentQuestion].Images.Count > 0)
                {
                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                    if (exist == ExistenceCheckResult.FileExists)
                    {
                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                        ImagePath1 = imageFile1.Path;
                    }
                }
            }
        }
        public async void TappedObservation()
        {
            CheckObservation = !CheckObservation;
            ObservationBorderColor = (ObservationBorderColor.Equals(Colors.FrameBlueColor)) ? Color.Gray : Colors.FrameBlueColor;
            ObservationFontAttribute = ObservationFontAttribute.Equals(FontAttributes.Bold) ? FontAttributes.None : FontAttributes.Bold;
            CaptureMode.IsObservationSelected = true;

            if (IsBattelleDevelopmentCompleteChecked)
            {
                if (!string.IsNullOrEmpty(CaptureMode.ObservationeScoringDesc))
                {
                    ScoringText = CaptureMode.ObservationeScoringDesc;
                    ScoringNotes = CaptureMode.ObservationeScoringNote;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                }
            }
            else
            {
                if (captureMode.ObservationeScoringDesc != null || captureMode.ObservationeScoringNote != null)
                {
                    ScoringText = captureMode.ObservationeScoringDesc;
                    ScoringNotes = captureMode.ObservationeScoringNote;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                }
            }

            if (CheckStructure || CheckInterview)
            {
                CaptureMode.IsInterViewSelected = CaptureMode.IsStructredSelected = false;
                CheckStructure = CheckInterview = false;
                StructureFontAttribute = InterviewFontAttribute = FontAttributes.None;
                StructureBorderColor = InterviewBorderColor = Color.Gray;
            }
            if (CaptureMode.ObservationContent != null)
            {
                CaptureModeText = CaptureMode.ObservationContent.ContentDescription;
                CaptureModeTextFilePath = CaptureMode.ObservationContent.HTMilFilePath;
                if (CaptureMode.ObservationContentScoring != null && CaptureMode.ObservationContentScoring.Any())
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>(CaptureMode.ObservationContentScoring.OrderByDescending(p => p.points));
                }
                else if (CaptureMode.DefaultContentScoring != null && CaptureMode.DefaultContentScoring.Any())
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>(CaptureMode.DefaultContentScoring.OrderByDescending(p => p.points));
                }
                else
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                }
                ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                {
                    var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                    if (selectedScore != null)
                    {
                        selectedScore.IsSelected = true;
                    }
                }
                else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                {
                    var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                    if (selectedScore != null)
                    {
                        selectedScore.IsSelected = true;
                    }
                }
            }

            if (this.IsBattelleDevelopmentCompleteChecked && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
            {
                ImagePath1 = null;
                ImagePath2 = null;
                if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                {
                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                    if (exist == ExistenceCheckResult.FileExists)
                    {
                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                        ImagePath1 = imageFile1.Path;
                    }
                }

            }

        }
        void CanInitiateNextItemTap(bool value)
        {
            IsNextItemTapped = value;
            ((Command)NextItemCommand).ChangeCanExecute();
        }

        void CanInitiatePrevItemTap(bool value)
        {
            IsPrevItemTapped = value;
            ((Command)PreviousItemCommand).ChangeCanExecute();
        }
        public async void MoreButtonClicked()
        {
            await PopupNavigation.Instance.PushAsync(new ScoringInstructionView(), false);
        }

        public async void TappedInterview()
        {
            CaptureMode.IsInterViewSelected = true;
            CheckInterview = !CheckInterview;
            InterviewBorderColor = (InterviewBorderColor.Equals(Colors.FrameBlueColor)) ? Color.Gray : Colors.FrameBlueColor;
            InterviewFontAttribute = InterviewFontAttribute.Equals(FontAttributes.Bold) ? FontAttributes.None : FontAttributes.Bold;

            if (IsBattelleDevelopmentCompleteChecked)
            {
                if (!string.IsNullOrEmpty(CaptureMode.InterviewScoringDesc))
                {
                    ScoringText = CaptureMode.InterviewScoringDesc;
                    ScoringNotes = CaptureMode.InterviewScoringNote;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                }
            }
            else
            {
                if (captureMode.InterviewScoringDesc != null || captureMode.InterviewScoringNote != null)
                {
                    ScoringText = captureMode.InterviewScoringDesc;
                    ScoringNotes = captureMode.InterviewScoringNote;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                }
            }
            if (CheckObservation || CheckStructure)
            {
                CaptureMode.IsObservationSelected = CaptureMode.IsStructredSelected = false;
                CheckObservation = CheckStructure = false;
                ObservationFontAttribute = StructureFontAttribute = FontAttributes.None;
                ObservationBorderColor = StructureBorderColor = Color.Gray;
            }
            if (CaptureMode.InterviewContent != null)
            {
                CaptureModeText = CaptureMode.InterviewContent.ContentDescription;
                CaptureModeTextFilePath = CaptureMode.InterviewContent.HTMilFilePath;
                if (CaptureMode.InterviewContentScoring != null && CaptureMode.InterviewContentScoring.Any())
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>(CaptureMode.InterviewContentScoring.OrderByDescending(p => p.points));
                }
                else if (CaptureMode.DefaultContentScoring != null && CaptureMode.DefaultContentScoring.Any())
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>(CaptureMode.DefaultContentScoring.OrderByDescending(p => p.points));
                }
                else
                {
                    ContentRubricPointCollection = new List<ContentRubricPoint>();
                }
                ContentRubricPointCollection.ForEach(p => p.IsSelected = false);

                if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                {
                    var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                    if (selectedScore != null)
                    {
                        selectedScore.IsSelected = true;
                    }
                }
                else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                {
                    var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                    if (selectedScore != null)
                    {
                        selectedScore.IsSelected = true;
                    }
                }
            }

            if (this.IsBattelleDevelopmentCompleteChecked && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
            {
                ImagePath1 = null;
                ImagePath2 = null;
                if (SubdomainCollection[CurrentQuestion].Images.Count > 2)
                {
                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();
                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                    if (exist == ExistenceCheckResult.FileExists)
                    {
                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                        ImagePath1 = imageFile1.Path;
                    }
                }
            }
        }
        #endregion

        void ResetTimer()
        {
            TimerStatusText = "Start";
            TimerButtonBckgrd = Color.FromHex("#478128");
            TimerReset = "iconrefreshgray.png";
            _timer.Stop();
            StartEnabled = IsTimerInProgress = true;
            TotalSeconds = new TimeSpan(0, 0, 0, 0);
        }

        public async void PreviousItemClicked()
        {
            //UserDialogs.Instance.ShowLoading("Loading");
            //await Task.Delay(2000);
            //CanInitiatePrevItemTap(true);
            var positionofSubdomain = -1;
            var positionofdomain = -1;

            ResetTimer();

            if (IsBattelleDevelopmentCompleteChecked)
            {
                if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                {
                    positionofSubdomain = TotalSubdomainCollection.IndexOf(TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCode == CurrentSubDomain));
                    IsPreviousShown = positionofSubdomain > 0;
                    IsNextShown = positionofSubdomain < TotalSubdomainCollection.Count - 1;
                }
                if (CurrentQuestion == 0 && positionofSubdomain > 0)
                {
                    InitialLoad = true;
                    await Initialize(TotalSubdomainCollection[positionofSubdomain - 1].SubDomainCode, TotalSubdomainCollection[positionofSubdomain - 1].AdministrationContents.Count - 1);
                    InitialLoad = false;
                }
                else if (!(CurrentQuestion < 0) && CurrentQuestion < SubdomainCollection.Count)
                {
                    CurrentQuestion -= 1;
                    if (positionofSubdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    else
                    {
                        IsPreviousShown = CurrentQuestion > 0;
                    }
                    IsNextShown = (CurrentQuestion < SubdomainCollection.Count) || positionofSubdomain < TotalSubdomainCollection.Count - 1;
                    ObservationStr = "Observation";
                    InterviewStr = "Interview";
                    StructuredStr = "Structured";
                    CheckStructure = CheckObservation = CheckInterview = false;
                    StructureFontAttribute = ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                    StructureBorderColor = ObservationBorderColor = InterviewBorderColor = Color.Gray;
                    StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");
                    ItemNumber = SubdomainCollection[CurrentQuestion].ItemNumber;
                    ItemAbbrevation = SubdomainCollection[CurrentQuestion].ItemAbbrevation;
                    BehaviorText = SubdomainCollection[CurrentQuestion].BehaviorContent;
                    BehaviorTextFilePath = SubdomainCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                    MaterialText = SubdomainCollection[CurrentQuestion].MaterialContent;
                    MaterialHtmlFilePath = SubdomainCollection[CurrentQuestion].MaterialContentFilePath;
                    MaxTime = SubdomainCollection[CurrentQuestion].MaxTime;
                    ScoringText = SubdomainCollection[CurrentQuestion].ScoringNotes;
                    ScoringNotes = SubdomainCollection[CurrentQuestion].ScoringCustomData;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                    if (SubdomainCollection[CurrentQuestion].TallyContent != null)
                    {
                        if (SubdomainCollection[CurrentQuestion].TallyContent.Count == 0)
                        {
                            IsTallyAndScoring = false;
                            IsScoringOnly = true;
                            ContentItemTalliesCollection = null;
                        }

                        if (SubdomainCollection[CurrentQuestion].TallyContent.Count > 0)
                        {
                            IsTallyAndScoring = true;
                            IsScoringOnly = false;
                            ContentItemTalliesCollection = SubdomainCollection[CurrentQuestion].TallyContent;
                        }
                    }

                    if (SubdomainCollection[CurrentQuestion].TallyContent != null && SubdomainCollection[CurrentQuestion].TallyContent.Count > 0 && (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                    {
                        TallyTabTappedCommand.Execute(new object());
                    }
                    else
                    {
                        ScoringTabTappedCommand.Execute(new object());
                    }

                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc != null)
                    {
                        CaptureMode = SubdomainCollection[CurrentQuestion].CaptureModeDesc;
                        InterviewEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null;
                        StructureEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null;
                        ObservationEnabled = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null;

                        if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                        {
                            ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                            InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                            ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                            InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                        }
                        var noSelection = !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (!noSelection)
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = false;
                                }
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = false;
                                }
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = false;
                                }
                            }
                        }
                        noSelection = !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (noSelection)
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = true;
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = true;
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = true;
                            }
                        }

                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null && CaptureMode.IsStructredSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                            CheckStructure = true;
                            StructureBorderColor = Colors.FrameBlueColor;
                            StructureFontAttribute = FontAttributes.Bold;
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 0)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }
                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null && CaptureMode.IsObservationSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                            CheckObservation = true;
                            ObservationBorderColor = Colors.FrameBlueColor;
                            ObservationFontAttribute = FontAttributes.Bold;

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null && CaptureMode.IsInterViewSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                            CheckInterview = true;
                            InterviewBorderColor = Colors.FrameBlueColor;
                            InterviewFontAttribute = FontAttributes.Bold;

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 2)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }

                            }
                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }

                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                        {
                            StructureEnabled = true;
                        }
                        else
                        {
                            StructureEnabled = false;
                            StructureDisabled = Color.FromHex("#979797");
                        }


                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                        {
                            ObservationEnabled = true;
                        }

                        else
                        {
                            ObservationEnabled = false;
                            ObservedDisabled = Color.FromHex("#979797");
                        }


                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                        {
                            InterviewEnabled = true;
                        }

                        else
                        {
                            InterviewEnabled = false;
                            InterviewDisabled = Color.FromHex("#979797");
                        }
                        CheckInterview = SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected;
                        CheckObservation = SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        CheckStructure = SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected;
                    }

                    /*if (SubdomainCollection[ItemNumber - 2].ScoringValuesandDesc != null)
                    {
                        ContentRubricPointCollection = SubdomainCollection[ItemNumber - 2].ScoringValuesandDesc;
                    }*/

                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count == 0)
                    {
                        if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 3)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                            string image3FileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();

                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                            if (existfolder == ExistenceCheckResult.FolderExists)
                            {
                                var imageFolder = await rootFolder.GetFolderAsync("Images");
                                ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                                ExistenceCheckResult exist3 = await imageFolder.CheckExistsAsync(image3FileName + ".png");

                                if (exist1 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                    ImagePath1 = imageFile1.Path;
                                }

                                if (exist2 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                    ImagePath2 = imageFile2.Path;
                                }

                                if (exist3 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile3 = await imageFolder.GetFileAsync(image3FileName + ".png");
                                    ImagePath3 = imageFile3.Path;
                                }
                            }
                        }
                        else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 2)
                        {
                            string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                            /*ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image1FileName + ".png";
                            ImagePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image2FileName + ".png";*/

                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            ExistenceCheckResult existImageFolder = await rootFolder.CheckExistsAsync("Images");
                            if (existImageFolder == ExistenceCheckResult.FolderExists)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                ImagePath3 = null;
                                var imageFolder = await rootFolder.GetFolderAsync("Images");
                                ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                                if (exist1 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                    ImagePath1 = imageFile1.Path;
                                }

                                if (exist2 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                    ImagePath2 = imageFile2.Path;
                                }
                            }
                        }
                        else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count > 0)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            if (SubdomainCollection[CurrentQuestion].Images.Count > 0)
                            {
                                string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                ExistenceCheckResult existImageFolder = await rootFolder.CheckExistsAsync("Images");
                                if (existImageFolder == ExistenceCheckResult.FolderExists)
                                {
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile.Path;
                                    }
                                }
                            }

                        }
                        else
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                        }

                    }

                }
                else
                {
                    if (IsPreviousShown)
                    {
                        await Initialize(TotalSubdomainCollection[positionofSubdomain - 1].SubDomainCode, TotalSubdomainCollection[positionofSubdomain - 1].AdministrationContents.Count - 1);
                    }
                }
            }
            else
            {
                if (TotalDomainCollection != null && TotalDomainCollection.Any())
                {
                    positionofdomain = TotalDomainCollection.IndexOf(TotalDomainCollection.FirstOrDefault(p => p.DomainName == CurrentDomain));
                    IsPreviousShown = positionofdomain > 0;
                    IsNextShown = positionofdomain < TotalDomainCollection.Count - 1;
                }

                if (CurrentQuestion == 0 && positionofdomain > 0)
                {
                    InitialLoad = true;
                    await Initialize(TotalDomainCollection[positionofdomain - 1].DomainName, TotalDomainCollection[positionofdomain - 1].AdministrationContents.Count - 1);
                    InitialLoad = false;
                }
                else if (!(CurrentQuestion < 0) && CurrentQuestion < domainContentCollection.Count)
                {
                    CurrentQuestion -= 1;
                    if (positionofdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    else
                    {
                        IsPreviousShown = CurrentQuestion > 0;
                    }
                    IsNextShown = (CurrentQuestion < domainContentCollection.Count) || positionofSubdomain < TotalDomainCollection.Count - 1;
                    ObservationStr = "Observation";
                    InterviewStr = "Interview";
                    StructuredStr = "Structured";
                    CheckStructure = CheckObservation = CheckInterview = false;
                    StructureFontAttribute = ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                    StructureBorderColor = ObservationBorderColor = InterviewBorderColor = Color.Gray;
                    StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");
                    ItemNumber = domainContentCollection[CurrentQuestion].ItemNumber;
                    ItemAbbrevation = domainContentCollection[CurrentQuestion].ItemAbbrevation;
                    BehaviorText = domainContentCollection[CurrentQuestion].BehaviorContent;
                    BehaviorTextFilePath = domainContentCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                    MaterialText = domainContentCollection[CurrentQuestion].MaterialContent;
                    MaterialHtmlFilePath = domainContentCollection[CurrentQuestion].MaterialContentFilePath;
                    MaxTime = domainContentCollection[CurrentQuestion].MaxTime;
                    ScoringText = domainContentCollection[CurrentQuestion].ScoringNotes;
                    ScoringNotes = domainContentCollection[CurrentQuestion].ScoringCustomData;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                    if (domainContentCollection[CurrentQuestion].TallyContent != null)
                    {
                        if (domainContentCollection[CurrentQuestion].TallyContent.Count == 0)
                        {
                            IsTallyAndScoring = false;
                            IsScoringOnly = true;
                            ContentItemTalliesCollection = null;
                        }

                        if (domainContentCollection[CurrentQuestion].TallyContent.Count > 0)
                        {
                            IsTallyAndScoring = true;
                            IsScoringOnly = false;
                            ContentItemTalliesCollection = domainContentCollection[CurrentQuestion].TallyContent;
                        }
                    }

                    if (domainContentCollection[CurrentQuestion].TallyContent != null && domainContentCollection[CurrentQuestion].TallyContent.Count > 0 && (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                    {
                        TallyTabTappedCommand.Execute(new object());
                    }
                    else
                    {
                        ScoringTabTappedCommand.Execute(new object());
                    }

                    if (domainContentCollection[CurrentQuestion].CaptureModeDesc != null)
                    {
                        CaptureMode = domainContentCollection[CurrentQuestion].CaptureModeDesc;
                        StructureEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null;
                        ObservationEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null;
                        InterviewEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null;

                        if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                        {
                            ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                            InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                            ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                            InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                            domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                            domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                        }

                        var noSelection = !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (!noSelection)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = false;
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = false;
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = false;
                                }
                            }
                        }
                        noSelection = !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (noSelection)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = true;
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = true;
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = true;
                            }
                        }

                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null && CaptureMode.IsStructredSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                            if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                            }
                            TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            CheckStructure = true;
                            StructureBorderColor = Colors.FrameBlueColor;
                            StructureFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null && CaptureMode.IsObservationSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                            if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                            CheckObservation = true;
                            ObservationBorderColor = Colors.FrameBlueColor;
                            ObservationFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null && CaptureMode.IsInterViewSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                            if (!string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                ScoringNotes = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                            }
                            TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            CheckInterview = true;
                            InterviewBorderColor = Colors.FrameBlueColor;
                            InterviewFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }

                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                        {
                            StructureEnabled = true;
                        }
                        else
                        {
                            StructureEnabled = false;
                            StructureDisabled = Color.FromHex("#979797");
                        }


                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                        {
                            ObservationEnabled = true;
                        }

                        else
                        {
                            ObservationEnabled = false;
                            ObservedDisabled = Color.FromHex("#979797");
                        }


                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                        {
                            InterviewEnabled = true;
                        }

                        else
                        {
                            InterviewEnabled = false;
                            InterviewDisabled = Color.FromHex("#979797");
                        }
                        CheckInterview = domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected;
                        CheckObservation = domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        CheckStructure = domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected;
                    }

                    /*if (SubdomainCollection[ItemNumber - 2].ScoringValuesandDesc != null)
                    {
                        ContentRubricPointCollection = SubdomainCollection[ItemNumber - 2].ScoringValuesandDesc;
                    }*/
                    if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count == 2)
                    {
                        string image1FileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                        string image2FileName = (domainContentCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                        /*ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image1FileName + ".png";
                        ImagePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image2FileName + ".png";*/

                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                        ExistenceCheckResult existImageFolder = await rootFolder.CheckExistsAsync("Images");
                        if (existImageFolder == ExistenceCheckResult.FolderExists)
                        {
                            var imageFolder = await rootFolder.GetFolderAsync("Images");
                            ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                            ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                            if (exist1 == ExistenceCheckResult.FileExists)
                            {
                                var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                ImagePath1 = imageFile1.Path;
                            }

                            if (exist2 == ExistenceCheckResult.FileExists)
                            {
                                var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                ImagePath2 = imageFile2.Path;
                            }
                        }
                    }
                    else if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count > 0)
                    {
                        string imageFileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                        ExistenceCheckResult existImageFolder = await rootFolder.CheckExistsAsync("Images");
                        if (existImageFolder == ExistenceCheckResult.FolderExists)
                        {
                            var imageFolder = await rootFolder.GetFolderAsync("Images");
                            ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                            if (exist == ExistenceCheckResult.FileExists)
                            {
                                var imageFile = await imageFolder.GetFileAsync(imageFileName + ".png");
                                ImagePath1 = imageFile.Path;
                            }
                        }
                    }
                    else
                    {
                        ImagePath1 = null;
                        ImagePath2 = null;
                    }
                }
                else
                {
                    if (IsPreviousShown)
                    {
                        await Initialize(TotalDomainCollection[positionofdomain - 1].DomainName, TotalDomainCollection[positionofdomain - 1].AdministrationContents.Count - 1);
                    }
                }
            }
            //UserDialogs.Instance.HideLoading();
            //await Task.Delay(1500);
            //CanInitiatePrevItemTap(true);
            //Dispose();
        }

        //public async void OpenExpandedImageVIew(ImageLocation imageLocation)
        //{
        //    var image = imageLocation;
        //    await PopupNavigation.Instance.PushAsync(new Views.PopupViews.ImagePopupView(image.ToString()));
        //}
        public void Closepopup()
        {
            var popNavigationInstance = PopupNavigation.Instance;
            AdministrationHeaderBackgroundColor = Color.Aquamarine;
            AdministrationHeader = "Sample";
            popNavigationInstance.PopAsync();
        }

        #region Correct/Incorrect Selection
        public async void CorrectAction(ContentItemTally tally)
        {
            HasDoneChanges = true;
            tally.CheckCorrectVisible = !tally.CheckCorrectVisible;
            tally.UncheckCorrectVisible = !tally.UncheckCorrectVisible;
            tally.CheckInCorrectVisible = false;
            tally.UncheckInCorrectVisible = true;
            await TallyCalculation();
        }
        public async void InCorrectAction(ContentItemTally tally)
        {
            HasDoneChanges = true;
            tally.CheckCorrectVisible = false;
            tally.UncheckCorrectVisible = true;
            tally.CheckInCorrectVisible = !tally.CheckInCorrectVisible;
            tally.UncheckInCorrectVisible = !tally.UncheckInCorrectVisible;
            await TallyCalculation();
        }

        private async Task TallyCalculation()
        {
            ContentRubricPoint rubicPoint = default(ContentRubricPoint);
            if (commonDataService.IsCompleteForm)
            {
                if (ContentItemTalliesCollection != null && ContentItemTalliesCollection.Any())
                {
                    var anycorrectChecked = ContentItemTalliesCollection.Any(p => p.CheckCorrectVisible);
                    var anyIncorretcChecked = ContentItemTalliesCollection.Any(p => p.CheckInCorrectVisible);

                    if (anycorrectChecked || anyIncorretcChecked)
                    {
                        var correcrAnswered = ContentItemTalliesCollection.Where(p => p.CheckCorrectVisible).ToList().Count;
                        var inCorrectAnswered = ContentItemTalliesCollection.Where(p => p.CheckInCorrectVisible).ToList().Count;
                        var contenttally = commonDataService.ContentItemTallyScores.Where(p => p.contentItemId == SubdomainCollection[CurrentQuestion].ContentItemID);
                        if (contenttally != null && contenttally.Any())
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = null;
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint = null;
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint = null;
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint = null;

                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                                {
                                    foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring)
                                    {
                                        item.IsSelected = false;
                                    }
                                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                                    {
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint;
                                    }
                                    else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        if (correcrAnswered < 3)
                                        {
                                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        }
                                        else
                                        {
                                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == 2).IsSelected = true;
                                        }
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                                {
                                    foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring)
                                    {
                                        item.IsSelected = false;
                                    }
                                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                                    {
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint;
                                    }
                                    else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                                {
                                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                                    {
                                        foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint;
                                    }
                                    else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = null;
                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint = null;
                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint = null;
                        SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint = null;
                        rubicPoint = null;
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                        {
                            foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                        {
                            foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                        {
                            foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                        {
                            foreach (var item in SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                    }
                }
            }
            else
            {
                if (ContentItemTalliesCollection != null && ContentItemTalliesCollection.Any())
                {
                    var anycorrectChecked = ContentItemTalliesCollection.Any(p => p.CheckCorrectVisible);
                    var anyIncorretcChecked = ContentItemTalliesCollection.Any(p => p.CheckInCorrectVisible);
                    if (anycorrectChecked || anyIncorretcChecked)
                    {
                        var correcrAnswered = ContentItemTalliesCollection.Where(p => p.CheckCorrectVisible).ToList().Count;
                        var inCorrectAnswered = ContentItemTalliesCollection.Where(p => p.CheckInCorrectVisible).ToList().Count;
                        var contenttally = commonDataService.ContentItemTallyScores.Where(p => p.contentItemId == domainContentCollection[CurrentQuestion].ContentItemID);
                        if (contenttally != null)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = null;
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint = null;
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint = null;
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint = null;
                                rubicPoint = null;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                                {
                                    foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring)
                                    {
                                        item.IsSelected = false;
                                    }
                                    if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                                    {
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint;
                                    }
                                    else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        if (correcrAnswered < 3)
                                        {
                                            domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        }
                                        else
                                        {
                                            domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == 2).IsSelected = true;
                                        }
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                                {
                                    foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring)
                                    {
                                        item.IsSelected = false;
                                    }
                                    if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                                    {
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint;
                                    }
                                    else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                                {
                                    if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                                    {
                                        foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint;
                                    }
                                    else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                                    {
                                        foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                                        {
                                            item.IsSelected = false;
                                        }
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.points == contenttally.FirstOrDefault(q => q.totalPoints == correcrAnswered).score).IsSelected = true;
                                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.IsSelected);
                                        rubicPoint = domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint = null;
                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint = null;
                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint = null;
                        domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint = null;
                        rubicPoint = null;
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                        {
                            foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                        {
                            foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                        {
                            foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                        {
                            foreach (var item in domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring)
                            {
                                item.IsSelected = false;
                            }
                        }
                    }
                }
            }
            var isBaselObtained = baselObtained;
            CheckScoreSelected();
            CalculateBaselCeiling();
            if (ContentItemTalliesCollection != null && ContentItemTalliesCollection.Any())
            {
                var correcrAnswered1 = ContentItemTalliesCollection.Where(p => p.CheckCorrectVisible).ToList().Count;
                var inCorrectAnswered2 = ContentItemTalliesCollection.Where(p => p.CheckInCorrectVisible).ToList().Count;
                if (correcrAnswered1 + inCorrectAnswered2 == ContentItemTalliesCollection.Count)
                {
                    if (!BaselObtained && rubicPoint != null)
                    {
                        if (rubicPoint.IsSelected && LastItemScored == null)
                        {
                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                        }

                        if (!BaselObtained)
                        {
                            if (rubicPoint.IsSelected && rubicPoint.points != 2)
                            {
                                var question = GetCurrentMovedQuestion(CurrentQuestion);
                                if (question != -1)
                                {
                                    if (rubicPoint.IsSelected)
                                    {
                                        LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                    }
                                    CurrentQuestion = question;
                                    InitialLoad = true;
                                    await LoadParticularQuestion();
                                    InitialLoad = false;
                                }
                            }
                            else if (rubicPoint.IsSelected)
                            {
                                if (commonDataService.IsCompleteForm)
                                {
                                    var scoreItems = SubdomainCollection.Where(p => p.CaptureModeDesc.SelectedDefaultRubicPoint != null || p.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ||
                                    p.CaptureModeDesc.SelectedObservationContentRubicPoint != null || p.CaptureModeDesc.SelectedStructuredContentRubicPoint != null);
                                    if (scoreItems != null && scoreItems.Any() && scoreItems.Count() == 1 && CurrentQuestion == SubdomainCollection.Count - 1)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = CurrentQuestion - 1;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                    }
                                    else if (LastItemScored != null)
                                    {
                                        var indexOfLastQuestion = SubdomainCollection.IndexOf(LastItemScored);
                                        if (indexOfLastQuestion != -1)
                                        {
                                            if (CurrentQuestion < indexOfLastQuestion)
                                            {
                                                if (rubicPoint.IsSelected)
                                                {
                                                    LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                                }
                                                CurrentQuestion = CurrentQuestion - 1;
                                                InitialLoad = true;
                                                await LoadParticularQuestion();
                                                InitialLoad = false;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    var scoreItems = domainContentCollection.Where(p => p.CaptureModeDesc.SelectedDefaultRubicPoint != null || p.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ||
                                    p.CaptureModeDesc.SelectedObservationContentRubicPoint != null || p.CaptureModeDesc.SelectedStructuredContentRubicPoint != null);
                                    if (scoreItems != null && scoreItems.Any() && scoreItems.Count() == 1 && CurrentQuestion == domainContentCollection.Count - 1)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = CurrentQuestion - 1;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                    }
                                    else if (LastItemScored != null)
                                    {
                                        var indexOfLastQuestion = domainContentCollection.IndexOf(LastItemScored);
                                        if (indexOfLastQuestion != -1)
                                        {
                                            if (CurrentQuestion < indexOfLastQuestion)
                                            {
                                                if (rubicPoint.IsSelected)
                                                {
                                                    LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                                }
                                                CurrentQuestion = CurrentQuestion - 1;
                                                InitialLoad = true;
                                                await LoadParticularQuestion();
                                                InitialLoad = false;
                                            }
                                        }

                                    }
                                }

                            }
                        }
                        else if (BaselObtained && !isBaselObtained && !ceilingObtained)
                        {
                            if (commonDataService.IsCompleteForm)
                            {
                                for (int i = CurrentQuestion; i < SubdomainCollection.Count; i++)
                                {
                                    var item = SubdomainCollection[i].CaptureModeDesc;
                                    if (item.SelectedDefaultRubicPoint == null && item.SelectedInterviewContentRubicPoint == null &&
                                    item.SelectedObservationContentRubicPoint == null && item.SelectedInterviewContentRubicPoint == null)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = i;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                for (int i = CurrentQuestion; i < domainContentCollection.Count; i++)
                                {
                                    var item = domainContentCollection[i].CaptureModeDesc;
                                    if (item.SelectedDefaultRubicPoint == null && item.SelectedInterviewContentRubicPoint == null &&
                                    item.SelectedObservationContentRubicPoint == null && item.SelectedInterviewContentRubicPoint == null)
                                    {
                                        if (rubicPoint.IsSelected)
                                        {
                                            LastItemScored = commonDataService.IsCompleteForm ? SubdomainCollection[CurrentQuestion] : domainContentCollection[CurrentQuestion];
                                        }
                                        CurrentQuestion = i;
                                        InitialLoad = true;
                                        await LoadParticularQuestion();
                                        InitialLoad = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region MenuRegion

        public void SaveTestDate(string testdate)
        {
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any() && CaptureMode != null)
            {
                var testOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == CaptureMode.DomainCategoryId);
                if (testOverview != null)
                {
                    HasDoneChanges = true;
                    testOverview.testDate = testdate;
                }
            }
        }

        private string interviewStr = "Interview";
        public string InterviewStr
        {
            get
            {
                return interviewStr;
            }
            set
            {
                interviewStr = value;
                OnPropertyChanged(nameof(InterviewStr));
            }
        }

        private string observationStr = "Observation";
        public string ObservationStr
        {
            get
            {
                return observationStr;
            }
            set
            {
                observationStr = value;
                OnPropertyChanged(nameof(ObservationStr));
            }
        }
        private string structuredstr = "Structured";
        public string StructuredStr
        {
            get
            {
                return structuredstr;
            }
            set
            {
                structuredstr = value;
                OnPropertyChanged(nameof(StructuredStr));
            }
        }
        private void GenerateScreenerItems()
        {
            if (TotalMenuList != null && TotalMenuList.Any())
            {
                foreach (var item in TotalMenuList)
                {
                    item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                    if (item.ContentLevelID == 4)
                    {
                        var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                        if (startingPointCategory != null)
                        {
                            var contentCategoryItems = commonDataService.ContentCategoryItems;
                            var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                            if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                            {
                                var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                                if (startingContentitem != null)
                                {
                                    var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                    if (contentItem != null)
                                    {
                                        var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                        if (startPointContentItemPosition != -1)
                                        {
                                            for (int i = startPointContentItemPosition - 1; i >= 0; i--)
                                            {
                                                if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                {
                                                    TotalMenuList[i].BelowStartingPoint = true;
                                                    TotalMenuList[i].ProgressImage = "notStarted.png";
                                                }
                                            }
                                        }
                                    }
                                    var startingLastContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).LastOrDefault();
                                    if (startingLastContentitem != null)
                                    {
                                        var lastContentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingLastContentitem.contentItemId);
                                        if (lastContentItem != null)
                                        {
                                            var startPointContentItemPosition = TotalMenuList.IndexOf(lastContentItem);
                                            if (startPointContentItemPosition != -1)
                                            {
                                                for (int i = startPointContentItemPosition + 1; i <= TotalMenuList.Count - 1; i++)
                                                {
                                                    if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                    {
                                                        TotalMenuList[i].BelowStartingPoint = true;
                                                        TotalMenuList[i].ProgressImage = "iconStatusNonitem.png";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var noStartingPointItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId);
                            if (noStartingPointItems != null && noStartingPointItems.Any())
                            {
                                foreach (var noitem in noStartingPointItems)
                                {
                                    noitem.BelowStartingPoint = true;
                                    noitem.ProgressImage = "iconStatusNonitem.png";
                                }
                            }
                        }
                    }
                }
                foreach (var item in TotalMenuList)
                {
                    item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                    if (item.ContentLevelID == 4)
                    {
                        var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                        if (startingPointCategory != null)
                        {
                            var contentCategoryItems = commonDataService.ContentCategoryItems;
                            var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                            if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                            {
                                var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                                if (startingContentitem != null)
                                {
                                    var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                    if (contentItem != null)
                                    {
                                        var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                        if (startPointContentItemPosition != -1)
                                        {
                                            var find = false;
                                            for (int i = 0; i <= startPointContentItemPosition - 1; i++)
                                            {
                                                if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                {
                                                    foreach (var domaincollection in TotalDomainCollection)
                                                    {
                                                        if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                                                        {
                                                            var contentItemTest = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == TotalMenuList[i].ContentItemId);
                                                            if (contentItemTest != null)
                                                            {
                                                                if (contentItemTest.CaptureModeDesc != null && (contentItemTest.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                                    contentItemTest.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                                                {
                                                                    item.ProgressImage = "notStarted.png";
                                                                    for (int j = i; j <= startPointContentItemPosition - 1; j++)
                                                                    {
                                                                        TotalMenuList[j].ProgressImage = "notStarted.png";
                                                                    }
                                                                    find = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (find)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in TotalMenuList)
                {
                    if (item.ContentLevelID == 5 && !item.IsStartingPoint)
                    {
                        if (TotalDomainCollection != null && TotalDomainCollection.Any())
                        {
                            foreach (var domaincollection in TotalDomainCollection)
                            {
                                if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                                {
                                    var contentItem = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == item.ContentItemId);
                                    if (contentItem != null)
                                    {
                                        if (contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                            contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                        {
                                            item.ProgressImage = "completed_TickMark.png";
                                        }
                                        else if (!item.BelowStartingPoint)
                                        {
                                            item.ProgressImage = "notStarted.png";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in TotalMenuList)
                {
                    if (item.ContentLevelID == 4)
                    {
                        var childItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId && !p.IsStartingPoint);
                        if (childItems != null && childItems.Any())
                        {
                            foreach (var childitem in childItems)
                            {
                                if (TotalDomainCollection != null && TotalDomainCollection.Any())
                                {
                                    var collection = TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == item.ContentCatgoryId).AdministrationContents;
                                    if (collection != null && collection.Any())
                                    {
                                        var contentItem = collection.FirstOrDefault(p => p.ContentItemID == childitem.ContentItemId);
                                        if (contentItem != null)
                                        {
                                            if (!(contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                            {
                                                var indexofContentItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(contentItem);
                                                var belowScored = false;
                                                var aboveScored = false;
                                                for (int i = 0; i < indexofContentItem; i++)
                                                {
                                                    var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                            indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                    {
                                                        belowScored = true;
                                                        break;
                                                    }
                                                }
                                                for (int i = indexofContentItem + 1; i < collection.Count; i++)
                                                {
                                                    var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                            indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                    {
                                                        aboveScored = true;
                                                        break;
                                                    }
                                                }
                                                if (aboveScored && belowScored)
                                                {
                                                    childitem.ProgressImage = "iconStatusSkipped.png";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in commonDataService.StudentTestForms)
                {
                    var subDomain = TotalMenuList.FirstOrDefault(p => p.ContentCatgoryId == item.contentCategoryId);
                    if (subDomain != null)
                    {
                        subDomain.SubDomainStatus = item.TSOStatus;
                    }
                }
                return;
            }
            TotalMenuList = new ObservableCollection<MenuContentModel>(commonDataService.ScreenerMenuList);
            foreach (var item in TotalMenuList)
            {
                item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                if (item.ContentLevelID == 4)
                {
                    var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                    if (startingPointCategory != null)
                    {
                        var contentCategoryItems = commonDataService.ContentCategoryItems;
                        var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                        if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                        {
                            var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                            if (startingContentitem != null)
                            {
                                var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                if (contentItem != null)
                                {
                                    var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                    if (startPointContentItemPosition != -1)
                                    {
                                        for (int i = startPointContentItemPosition - 1; i >= 0; i--)
                                        {
                                            if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                            {
                                                TotalMenuList[i].BelowStartingPoint = true;
                                                TotalMenuList[i].ProgressImage = "notStarted.png";
                                            }
                                        }
                                    }
                                }
                            }
                            var startingLastContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).LastOrDefault();
                            if (startingLastContentitem != null)
                            {
                                var lastContentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingLastContentitem.contentItemId);
                                if (lastContentItem != null)
                                {
                                    var startPointContentItemPosition = TotalMenuList.IndexOf(lastContentItem);
                                    if (startPointContentItemPosition != -1)
                                    {
                                        for (int i = startPointContentItemPosition + 1; i <= TotalMenuList.Count - 1; i++)
                                        {
                                            if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                            {
                                                TotalMenuList[i].BelowStartingPoint = true;
                                                TotalMenuList[i].ProgressImage = "iconStatusNonitem.png";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var noStartingPointItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId);
                        if (noStartingPointItems != null && noStartingPointItems.Any())
                        {
                            foreach (var noitem in noStartingPointItems)
                            {
                                noitem.BelowStartingPoint = true;
                                noitem.ProgressImage = "iconStatusNonitem.png";
                            }
                        }
                    }
                }
            }
            foreach (var item in TotalMenuList)
            {
                item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                if (item.ContentLevelID == 4)
                {
                    var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                    if (startingPointCategory != null)
                    {
                        var contentCategoryItems = commonDataService.ContentCategoryItems;
                        var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                        if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                        {
                            var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                            if (startingContentitem != null)
                            {
                                var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                if (contentItem != null)
                                {
                                    var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                    if (startPointContentItemPosition != -1)
                                    {
                                        var find = false;
                                        for (int i = 0; i <= startPointContentItemPosition - 1; i++)
                                        {
                                            if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                            {
                                                foreach (var domaincollection in TotalDomainCollection)
                                                {
                                                    if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                                                    {
                                                        var contentItemTest = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == TotalMenuList[i].ContentItemId);
                                                        if (contentItemTest != null)
                                                        {
                                                            if (contentItemTest.CaptureModeDesc != null && (contentItemTest.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                                contentItemTest.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                                            {
                                                                item.ProgressImage = "notStarted.png";
                                                                for (int j = i; j <= startPointContentItemPosition - 1; j++)
                                                                {
                                                                    TotalMenuList[j].ProgressImage = "notStarted.png";
                                                                }
                                                                find = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (find)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in TotalMenuList)
            {
                if (item.ContentLevelID == 5 && !item.IsStartingPoint)
                {
                    if (TotalDomainCollection != null && TotalDomainCollection.Any())
                    {
                        foreach (var domaincollection in TotalDomainCollection)
                        {
                            if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                            {
                                var contentItem = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == item.ContentItemId);
                                if (contentItem != null)
                                {
                                    if (contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                        contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                    {
                                        item.ProgressImage = "completed_TickMark.png";
                                    }
                                    else if (!item.BelowStartingPoint)
                                    {
                                        item.ProgressImage = "notStarted.png";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in TotalMenuList)
            {
                if (item.ContentLevelID == 4)
                {
                    var childItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId && !p.IsStartingPoint);
                    if (childItems != null && childItems.Any())
                    {
                        foreach (var childitem in childItems)
                        {
                            if (TotalDomainCollection != null && TotalDomainCollection.Any())
                            {
                                var collection = TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == item.ContentCatgoryId).AdministrationContents;
                                if (collection != null && collection.Any())
                                {
                                    var contentItem = collection.FirstOrDefault(p => p.ContentItemID == childitem.ContentItemId);
                                    if (contentItem != null)
                                    {
                                        if (!(contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                            contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                        {
                                            var indexofContentItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(contentItem);
                                            var belowScored = false;
                                            var aboveScored = false;
                                            for (int i = 0; i < indexofContentItem; i++)
                                            {
                                                var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                        indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                {
                                                    belowScored = true;
                                                    break;
                                                }
                                            }
                                            for (int i = indexofContentItem + 1; i < collection.Count; i++)
                                            {
                                                var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                        indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                {
                                                    aboveScored = true;
                                                    break;
                                                }
                                            }
                                            if (aboveScored && belowScored)
                                            {
                                                childitem.ProgressImage = "iconStatusSkipped.png";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in commonDataService.StudentTestForms)
            {
                var subDomain = TotalMenuList.FirstOrDefault(p => p.ContentCatgoryId == item.contentCategoryId);
                if (subDomain != null)
                {
                    subDomain.SubDomainStatus = item.TSOStatus;
                }
            }
            MenuList = new ObservableCollection<MenuContentModel>(TotalMenuList.Where(p => p.ContentLevelID == 4).OrderBy(p => p.SequenceNumber));
        }
        private void GenerateMenuItems()
        {
            if (TotalMenuList != null && TotalMenuList.Any())
            {
                foreach (var item in TotalMenuList)
                {
                    item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                    if (item.ContentLevelID == 2)
                    {
                        var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                        if (startingPointCategory != null)
                        {
                            var contentCategoryItems = commonDataService.ContentCategoryItems;
                            var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                            if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                            {
                                var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                                if (startingContentitem != null)
                                {
                                    var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                    var startingLastContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).LastOrDefault();
                                    if (contentItem != null)
                                    {
                                        var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                        if (startPointContentItemPosition != -1)
                                        {
                                            for (int i = startPointContentItemPosition - 1; i >= 0; i--)
                                            {
                                                if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                {
                                                    TotalMenuList[i].BelowStartingPoint = true;
                                                    TotalMenuList[i].ProgressImage = "notStarted.png";
                                                }
                                            }
                                        }
                                    }
                                    if (startingLastContentitem != null)
                                    {
                                        var lastContentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingLastContentitem.contentItemId);
                                        if (lastContentItem != null)
                                        {
                                            var startPointContentItemPosition = TotalMenuList.IndexOf(lastContentItem);
                                            if (startPointContentItemPosition != -1)
                                            {
                                                for (int i = startPointContentItemPosition + 1; i <= TotalMenuList.Count - 1; i++)
                                                {
                                                    if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                    {
                                                        TotalMenuList[i].BelowStartingPoint = true;
                                                        TotalMenuList[i].ProgressImage = "iconStatusNonitem.png";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var noStartingPointItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId);
                                if (noStartingPointItems != null && noStartingPointItems.Any())
                                {
                                    foreach (var noitem in noStartingPointItems)
                                    {
                                        noitem.BelowStartingPoint = true;
                                        noitem.ProgressImage = "iconStatusNonitem.png";
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in TotalMenuList)
                {
                    item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                    if (item.ContentLevelID == 2)
                    {
                        var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                        if (startingPointCategory != null)
                        {
                            var contentCategoryItems = commonDataService.ContentCategoryItems;
                            var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                            if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                            {
                                var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                                if (startingContentitem != null)
                                {
                                    var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                    if (contentItem != null)
                                    {
                                        var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                        if (startPointContentItemPosition != -1)
                                        {
                                            var find = false;
                                            for (int i = 0; i <= startPointContentItemPosition - 1; i++)
                                            {
                                                if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                {
                                                    foreach (var domaincollection in TotalSubdomainCollection)
                                                    {
                                                        if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                                                        {
                                                            var contentItemTest = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == TotalMenuList[i].ContentItemId);
                                                            if (contentItemTest != null)
                                                            {
                                                                if (contentItemTest.CaptureModeDesc != null && (contentItemTest.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                                    contentItemTest.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                                                {
                                                                    item.ProgressImage = "notStarted.png";
                                                                    for (int j = i; j <= startPointContentItemPosition - 1; j++)
                                                                    {
                                                                        TotalMenuList[j].ProgressImage = "notStarted.png";
                                                                    }
                                                                    find = true;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (find)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in TotalMenuList)
                {
                    if (item.ContentLevelID == 3 && !item.IsStartingPoint)
                    {
                        if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                        {
                            foreach (var domaincollection in TotalSubdomainCollection)
                            {
                                if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                                {
                                    var contentItem = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == item.ContentItemId);
                                    if (contentItem != null)
                                    {
                                        if (contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                            contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                        {
                                            item.ProgressImage = "completed_TickMark.png";
                                        }
                                        else if (!item.BelowStartingPoint)
                                        {
                                            item.ProgressImage = "notStarted.png";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in TotalMenuList)
                {
                    item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                    if (item.ContentLevelID == 2)
                    {
                        var childItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId && !p.IsStartingPoint);
                        if (childItems != null && childItems.Any())
                        {
                            foreach (var childitem in childItems)
                            {
                                if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                                {
                                    var collection = TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == item.ContentCatgoryId).AdministrationContents;
                                    if (collection != null && collection.Any())
                                    {
                                        var contentItem = collection.FirstOrDefault(p => p.ContentItemID == childitem.ContentItemId);
                                        if (contentItem != null)
                                        {
                                            if (!(contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                            {
                                                var indexofContentItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(contentItem);
                                                var belowScored = false;
                                                var aboveScored = false;
                                                for (int i = 0; i < indexofContentItem; i++)
                                                {
                                                    var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                            indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                    {
                                                        belowScored = true;
                                                        break;
                                                    }
                                                }
                                                for (int i = indexofContentItem + 1; i < collection.Count; i++)
                                                {
                                                    var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                            indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                    {
                                                        aboveScored = true;
                                                        break;
                                                    }
                                                }
                                                if (aboveScored && belowScored)
                                                {
                                                    childitem.ProgressImage = "iconStatusSkipped.png";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in commonDataService.StudentTestForms)
                {
                    var subDomain = TotalMenuList.FirstOrDefault(p => p.ContentCatgoryId == item.contentCategoryId);
                    if (subDomain != null)
                    {
                        subDomain.SubDomainStatus = item.TSOStatus;
                    }
                }
                return;
            }
            TotalMenuList = new ObservableCollection<MenuContentModel>(commonDataService.BattleDevelopmentMenuList);
            foreach (var item in TotalMenuList)
            {
                item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                if (item.ContentLevelID == 2)
                {
                    var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                    if (startingPointCategory != null)
                    {
                        var contentCategoryItems = commonDataService.ContentCategoryItems;
                        var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                        if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                        {
                            var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                            if (startingContentitem != null)
                            {
                                var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                if (contentItem != null)
                                {
                                    var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                    if (startPointContentItemPosition != -1)
                                    {
                                        for (int i = startPointContentItemPosition - 1; i >= 0; i--)
                                        {
                                            if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                            {
                                                TotalMenuList[i].BelowStartingPoint = true;
                                                TotalMenuList[i].ProgressImage = "notStarted.png";
                                            }
                                        }
                                    }
                                }
                            }
                            var startingLastContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).LastOrDefault();
                            if (startingLastContentitem != null)
                            {
                                var lastContentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingLastContentitem.contentItemId);
                                if (lastContentItem != null)
                                {
                                    var startPointContentItemPosition = TotalMenuList.IndexOf(lastContentItem);
                                    if (startPointContentItemPosition != -1)
                                    {
                                        for (int i = startPointContentItemPosition + 1; i <= TotalMenuList.Count - 1; i++)
                                        {
                                            if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                            {
                                                TotalMenuList[i].BelowStartingPoint = true;
                                                TotalMenuList[i].ProgressImage = "iconStatusNonitem.png";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var noStartingPointItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId);
                        if (noStartingPointItems != null && noStartingPointItems.Any())
                        {
                            foreach (var noitem in noStartingPointItems)
                            {
                                noitem.BelowStartingPoint = true;
                                noitem.ProgressImage = "iconStatusNonitem.png";
                            }
                        }
                    }
                }
            }
            foreach (var item in TotalMenuList)
            {
                item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                if (item.ContentLevelID == 2)
                {
                    var startingPointCategory = TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                    if (startingPointCategory != null)
                    {
                        var contentCategoryItems = commonDataService.ContentCategoryItems;
                        var startingPointCatgoryItems = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                        if (startingPointCatgoryItems != null && startingPointCatgoryItems.Any())
                        {
                            var startingContentitem = commonDataService.ContentItems.Where(p => startingPointCatgoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                            if (startingContentitem != null)
                            {
                                var contentItem = TotalMenuList.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId);
                                if (contentItem != null)
                                {
                                    var startPointContentItemPosition = TotalMenuList.IndexOf(contentItem);
                                    if (startPointContentItemPosition != -1)
                                    {
                                        var find = false;
                                        for (int i = 0; i <= startPointContentItemPosition - 1; i++)
                                        {
                                            if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                            {
                                                foreach (var domaincollection in TotalSubdomainCollection)
                                                {
                                                    if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                                                    {
                                                        var contentItemTest = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == TotalMenuList[i].ContentItemId);
                                                        if (contentItemTest != null)
                                                        {
                                                            if (contentItemTest.CaptureModeDesc != null && (contentItemTest.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                                contentItemTest.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItemTest.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                                            {
                                                                item.ProgressImage = "notStarted.png";
                                                                for (int j = i; j <= startPointContentItemPosition - 1; j++)
                                                                {
                                                                    TotalMenuList[j].ProgressImage = "notStarted.png";
                                                                }
                                                                find = true;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            if (find)
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in TotalMenuList)
            {
                if (item.ContentLevelID == 3 && !item.IsStartingPoint)
                {
                    if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                    {
                        foreach (var domaincollection in TotalSubdomainCollection)
                        {
                            if (domaincollection.AdministrationContents != null && domaincollection.AdministrationContents.Any())
                            {
                                var contentItem = domaincollection.AdministrationContents.FirstOrDefault(p => p.ContentItemID == item.ContentItemId);
                                if (contentItem != null)
                                {
                                    if (contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                        contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                                    {
                                        item.ProgressImage = "completed_TickMark.png";
                                    }
                                    else if (!item.BelowStartingPoint)
                                    {
                                        item.ProgressImage = "notStarted.png";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in TotalMenuList)
            {
                item.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                if (item.ContentLevelID == 2)
                {
                    var childItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId && !p.IsStartingPoint);
                    if (childItems != null && childItems.Any())
                    {
                        foreach (var childitem in childItems)
                        {
                            if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                            {
                                var collection = TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == item.ContentCatgoryId).AdministrationContents;
                                if (collection != null && collection.Any())
                                {
                                    var contentItem = collection.FirstOrDefault(p => p.ContentItemID == childitem.ContentItemId);
                                    if (contentItem != null)
                                    {
                                        if (!(contentItem.CaptureModeDesc != null && (contentItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                            contentItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || contentItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                        {
                                            var indexofContentItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(contentItem);
                                            var belowScored = false;
                                            var aboveScored = false;
                                            for (int i = 0; i < indexofContentItem; i++)
                                            {
                                                var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                        indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                {
                                                    belowScored = true;
                                                    break;
                                                }
                                            }
                                            for (int i = indexofContentItem + 1; i < collection.Count; i++)
                                            {
                                                var indexItem = collection.OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                if ((indexItem.CaptureModeDesc != null && (indexItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ||
                                                        indexItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || indexItem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)))
                                                {
                                                    aboveScored = true;
                                                    break;
                                                }
                                            }
                                            if (aboveScored && belowScored)
                                            {
                                                childitem.ProgressImage = "iconStatusSkipped.png";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (var item in commonDataService.StudentTestForms)
            {
                var subDomain = TotalMenuList.FirstOrDefault(p => p.ContentCatgoryId == item.contentCategoryId);
                if (subDomain != null)
                {
                    subDomain.SubDomainStatus = item.TSOStatus;
                }
            }
            MenuList = new ObservableCollection<MenuContentModel>(TotalMenuList.Where(p => p.ContentLevelID == 1).OrderBy(p => p.SequenceNumber));
        }
        private async void AddorRemoveCompletedForm(MenuContentModel model)
        {
            model.ImageName = "menudownarrow.png";
            if (model.ContentLevelID == 1)
            {
                if (model.IsVisible)
                {
                    var menuItems = TotalMenuList.Where(p => p.ParentID == model.ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                    if (menuItems != null && menuItems.Any())
                    {
                        for (int i = 0; i < menuItems.Count(); i++)
                        {
                            var submenuItems = TotalMenuList.Where(p => p.ParentID == menuItems.ElementAt(i).ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                            if (submenuItems != null && submenuItems.Any())
                            {
                                foreach (var subitem in submenuItems)
                                {
                                    MenuList.Remove(subitem);
                                }
                            }
                            MenuList.Remove(menuItems.ElementAt(i));
                        }
                    }
                }
                else
                {
                    model.ImageName = "menuuparrow.png";
                    var menuItems = TotalMenuList.Where(p => p.ParentID == model.ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                    if (menuItems != null && menuItems.Any())
                    {
                        var indexofItem = MenuList.IndexOf(model);
                        for (int i = 0; i < menuItems.Count(); i++)
                        {
                            var itemmenu = menuItems.ElementAt(i);
                            itemmenu.ImageName = "menudownarrow.png";
                            itemmenu.IsVisible = false;
                            MenuList.Insert(indexofItem + 1 + i, itemmenu);
                        }
                    }
                }
            }
            else if (model.ContentLevelID == 2)
            {
                if (model.IsVisible)
                {
                    var menuItems = TotalMenuList.Where(p => p.ParentID == model.ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                    if (menuItems != null && menuItems.Any())
                    {
                        for (int i = 0; i < menuItems.Count(); i++)
                        {
                            MenuList.Remove(menuItems.ElementAt(i));
                        }
                    }
                }
                else
                {
                    model.ImageName = "menuuparrow.png";
                    var menuItems = TotalMenuList.Where(p => p.ParentID == model.ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                    if (menuItems != null && menuItems.Any())
                    {
                        var indexofItem = MenuList.IndexOf(model);
                        for (int i = 0; i < menuItems.Count(); i++)
                        {
                            var itemmenu = menuItems.ElementAt(i);
                            itemmenu.ImageName = "menudownarrow.png";
                            itemmenu.IsVisible = false;
                            MenuList.Insert(indexofItem + 1 + i, itemmenu);
                        }
                    }
                }
            }
            else
            {
                TimerStatusText = "Start";
                TimerButtonBckgrd = Color.FromHex("#478128");
                TimerReset = "iconrefreshgray.png";
                _timer.Stop();
                StartEnabled = IsTimerInProgress = true;
                TotalSeconds = new TimeSpan(0, 0, 0, 0);

                await PopupNavigation.Instance.PopAllAsync();
                var splittedcode = model.Code.Split(' ').FirstOrDefault();
                var contentItem = SubdomainCollection.Where(p => p.ContentItemID == model.ContentItemId).FirstOrDefault();
                var position = -1;
                if (contentItem != null)
                {
                    position = SubdomainCollection.IndexOf(contentItem);
                }

                if (CurrentSubDomain == splittedcode)
                {
                    if (position != -1)
                    {
                        CurrentQuestion = position;
                        InitialLoad = true;
                        await LoadParticularQuestion();
                        InitialLoad = false;
                    }
                }
                else
                {
                    var findmodel = TotalSubdomainCollection.Where(p => p.SubDomainCode == splittedcode).Select(p => p.AdministrationContents).FirstOrDefault();
                    if (findmodel != null && findmodel.Any())
                    {
                        var orderList = findmodel.OrderBy(p => Convert.ToInt32(p.ItemNumber));
                        position = -1;
                        foreach (var item in orderList)
                        {
                            position += 1;
                            if (item.ContentItemID == model.ContentItemId)
                            {
                                break;
                            }
                        }
                    }
                    UserDialogs.Instance.ShowLoading("Loading...");
                    await Task.Delay(600);
                    await Initialize(splittedcode, position == -1 ? 0 : position);
                    await Task.Delay(600);
                    UserDialogs.Instance.HideLoading();
                }
            }
            model.IsVisible = !model.IsVisible;
        }

        private async void AddorRemoveScreenForm(MenuContentModel model)
        {
            model.ImageName = "menudownarrow.png";
            if (model.ContentLevelID == 4)
            {
                if (model.IsVisible)
                {
                    var menuItems = TotalMenuList.Where(p => p.ParentID == model.ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                    if (menuItems != null && menuItems.Any())
                    {
                        for (int i = 0; i < menuItems.Count(); i++)
                        {
                            var submenuItems = TotalMenuList.Where(p => p.ParentID == menuItems.ElementAt(i).ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                            if (submenuItems != null && submenuItems.Any())
                            {
                                foreach (var subitem in submenuItems)
                                {
                                    MenuList.Remove(subitem);
                                }
                            }
                            MenuList.Remove(menuItems.ElementAt(i));
                        }
                    }
                }
                else
                {
                    model.ImageName = "menuuparrow.png";
                    var menuItems = TotalMenuList.Where(p => p.ParentID == model.ContentCatgoryId).OrderBy(p => p.SequenceNumber);
                    if (menuItems != null && menuItems.Any())
                    {
                        var indexofItem = MenuList.IndexOf(model);
                        for (int i = 0; i < menuItems.Count(); i++)
                        {
                            var itemmenu = menuItems.ElementAt(i);
                            itemmenu.ImageName = "menudownarrow.png";
                            itemmenu.IsVisible = false;
                            MenuList.Insert(indexofItem + 1 + i, itemmenu);
                        }
                    }
                }
            }
            else
            {
                await PopupNavigation.Instance.PopAllAsync();
                var position = -1;
                var code = "";
                foreach (var item in TotalDomainCollection)
                {
                    position = -1;
                    var orderList = item.AdministrationContents.OrderBy(p => Convert.ToInt32(p.ItemNumber));
                    foreach (var inneritem in orderList)
                    {
                        position += 1;
                        if (inneritem.ContentItemID == model.ContentItemId)
                        {
                            code = item.DomainName;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(code))
                    {
                        break;
                    }
                }
                if (CurrentDomain.ToLower() == code.ToLower())
                {
                    if (position != -1)
                    {
                        CurrentQuestion = position;
                        InitialLoad = true;
                        await LoadParticularQuestion();
                        InitialLoad = false;
                    }
                }
                else
                {
                    UserDialogs.Instance.ShowLoading("Loading...");
                    await Task.Delay(600);
                    await Initialize(code, position == -1 ? 0 : position);
                    await Task.Delay(600);
                    UserDialogs.Instance.HideLoading();
                }
            }
            model.IsVisible = !model.IsVisible;
        }

        public void AddorRemoveItems(MenuContentModel model)
        {
            if (AssessmentConfigurationType == AssessmentConfigurationType.BattelleDevelopmentComplete)
            {
                AddorRemoveCompletedForm(model);
            }
            else if (AssessmentConfigurationType == AssessmentConfigurationType.BattelleDevelopmentScreener)
            {
                AddorRemoveScreenForm(model);
            }
        }

        private ObservableCollection<MenuContentModel> TotalMenuList;
        private ObservableCollection<MenuContentModel> menuList;
        public ObservableCollection<MenuContentModel> MenuList
        {
            get
            {
                return menuList;
            }
            set
            {
                menuList = value;
                OnPropertyChanged(nameof(MenuList));
            }
        }
        private void LoadDomain()
        {

        }
        private async Task LoadParticularQuestion()
        {
            UserDialogs.Instance.ShowLoading("Loading...");
            await Task.Delay(600);
            if (IsBattelleDevelopmentCompleteChecked)
            {
                var positionofSubdomain = -1;
                if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                {
                    positionofSubdomain = TotalSubdomainCollection.IndexOf(TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCode == CurrentSubDomain));
                    IsPreviousShown = positionofSubdomain > 0;
                    IsNextShown = positionofSubdomain < TotalSubdomainCollection.Count - 1;
                }
                if (CurrentQuestion < SubdomainCollection.Count)
                {
                    if (positionofSubdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    else
                    {
                        IsPreviousShown = CurrentQuestion > 0;
                    }
                    IsNextShown = (CurrentQuestion < SubdomainCollection.Count - 1) || positionofSubdomain < TotalSubdomainCollection.Count - 1;
                    ObservationStr = "Observation";
                    InterviewStr = "Interview";
                    StructuredStr = "Structured";
                    CheckStructure = CheckObservation = CheckInterview = false;
                    StructureFontAttribute = ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                    StructureBorderColor = ObservationBorderColor = InterviewBorderColor = Color.Gray;
                    StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");
                    ItemNumber = SubdomainCollection[CurrentQuestion].ItemNumber;
                    ItemAbbrevation = SubdomainCollection[CurrentQuestion].ItemAbbrevation;
                    BehaviorText = SubdomainCollection[CurrentQuestion].BehaviorContent;
                    BehaviorTextFilePath = SubdomainCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                    MaterialText = SubdomainCollection[CurrentQuestion].MaterialContent;
                    MaterialHtmlFilePath = SubdomainCollection[CurrentQuestion].MaterialContentFilePath;
                    MaxTime = SubdomainCollection[CurrentQuestion].MaxTime;
                    ScoringText = SubdomainCollection[CurrentQuestion].ScoringNotes;
                    ScoringNotes = SubdomainCollection[CurrentQuestion].ScoringCustomData;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");

                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc != null)
                    {
                        CaptureMode = SubdomainCollection[CurrentQuestion].CaptureModeDesc;
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            StructureEnabled = true;
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            ObservationEnabled = true;
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            InterviewEnabled = true;

                        if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                        {
                            ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                            InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                            ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                            InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                            SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                        }

                        var noSelection = !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (!noSelection)
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = false;
                                }
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = false;
                                }
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                            {
                                var hasScore = false;
                                if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == SubdomainCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = false;
                                }
                            }
                        }
                        noSelection = !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (noSelection)
                        {
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = true;
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = true;
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            {
                                SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = true;
                            }
                        }

                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null && CaptureMode.IsStructredSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                            CheckStructure = true;
                            StructureBorderColor = Colors.FrameBlueColor;
                            StructureFontAttribute = FontAttributes.Bold;
                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructureScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null && CaptureMode.IsObservationSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                            CheckObservation = true;
                            ObservationBorderColor = Colors.FrameBlueColor;
                            ObservationFontAttribute = FontAttributes.Bold;

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationeScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }
                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null && CaptureMode.IsInterViewSelected)
                        {
                            CaptureModeText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                            CaptureModeTextFilePath = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                            CheckInterview = true;
                            InterviewBorderColor = Colors.FrameBlueColor;
                            InterviewFontAttribute = FontAttributes.Bold;

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(SubdomainCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }

                            if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count > 0)
                            {
                                ImagePath1 = null;
                                ImagePath2 = null;
                                if (SubdomainCollection[CurrentQuestion].Images.Count > 1)
                                {
                                    string imageFileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                    ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                                    if (exist == ExistenceCheckResult.FileExists)
                                    {
                                        var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                                        ImagePath1 = imageFile1.Path;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc) || !string.IsNullOrEmpty(SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote))
                            {
                                ScoringText = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringDesc;
                                ScoringNotes = SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewScoringNote;
                                TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                            }
                        }

                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                        {
                            StructureEnabled = true;
                        }
                        else
                        {
                            StructureEnabled = false;
                            StructureDisabled = Color.FromHex("#979797");
                        }


                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                        {
                            ObservationEnabled = true;
                        }

                        else
                        {
                            ObservationEnabled = false;
                            ObservedDisabled = Color.FromHex("#979797");
                        }


                        if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                        {
                            InterviewEnabled = true;
                        }

                        else
                        {
                            InterviewEnabled = false;
                            InterviewDisabled = Color.FromHex("#979797");
                        }
                        CheckInterview = SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected;
                        CheckObservation = SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        CheckStructure = SubdomainCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected;
                    }

                    /*if (SubdomainCollection[ItemNumber - 2].ScoringValuesandDesc != null)
                    {
                        ContentRubricPointCollection = SubdomainCollection[ItemNumber - 2].ScoringValuesandDesc;
                    }*/

                    if (SubdomainCollection[CurrentQuestion].TallyContent != null)
                    {
                        if (SubdomainCollection[CurrentQuestion].TallyContent.Count == 0)
                        {
                            ContentItemTalliesCollection = null;
                            IsScoringOnly = true;
                            IsTallyAndScoring = false;
                        }

                        if (SubdomainCollection[CurrentQuestion].TallyContent.Count > 0)
                        {
                            ContentItemTalliesCollection = SubdomainCollection[CurrentQuestion].TallyContent;
                            IsScoringOnly = false;
                            IsTallyAndScoring = true;
                        }
                    }

                    if (SubdomainCollection[CurrentQuestion].TallyContent != null && SubdomainCollection[CurrentQuestion].TallyContent.Count > 0 && (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                    {
                        TallyTabTappedCommand.Execute(new object());
                    }
                    else
                    {
                        ScoringTabTappedCommand.Execute(new object());
                    }

                    if (SubdomainCollection[CurrentQuestion].CaptureModeDesc.StructuredCollectionData.Count == 0)
                    {
                        if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 3)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();
                            string image3FileName = (SubdomainCollection[CurrentQuestion].Images[2]).ToString().Split(':')[1].Trim();

                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                            if (existfolder == ExistenceCheckResult.FolderExists)
                            {
                                var imageFolder = await rootFolder.GetFolderAsync("Images");
                                ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                                ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                                ExistenceCheckResult exist3 = await imageFolder.CheckExistsAsync(image3FileName + ".png");

                                if (exist1 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                    ImagePath1 = imageFile1.Path;
                                }

                                if (exist2 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                    ImagePath2 = imageFile2.Path;
                                }

                                if (exist3 == ExistenceCheckResult.FileExists)
                                {
                                    var imageFile3 = await imageFolder.GetFileAsync(image3FileName + ".png");
                                    ImagePath3 = imageFile3.Path;
                                }
                            }
                        }
                        else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count == 2)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string image1FileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            string image2FileName = (SubdomainCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                            /*ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image1FileName + ".png";
                            ImagePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image2FileName + ".png";*/

                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            var imageFolder = await rootFolder.GetFolderAsync("Images");

                            ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                            ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                            if (exist1 == ExistenceCheckResult.FileExists)
                            {
                                var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                                ImagePath1 = imageFile1.Path;
                            }

                            if (exist2 == ExistenceCheckResult.FileExists)
                            {
                                var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                                ImagePath2 = imageFile2.Path;
                            }

                            //    var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                            //var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");

                            //ImagePath1 = imageFile1.Path;
                            //ImagePath2 = imageFile2.Path;
                        }
                        else if (SubdomainCollection[CurrentQuestion].Images != null && SubdomainCollection[CurrentQuestion].Images.Count > 0)
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                            string imageFileName = (SubdomainCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                            PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                            var imageFolder = await rootFolder.GetFolderAsync("Images");

                            ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                            if (exist == ExistenceCheckResult.FileExists)
                            {
                                var imageFile = await imageFolder.GetFileAsync(imageFileName + ".png");
                                ImagePath1 = imageFile.Path;
                            }

                            //var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                            //ImagePath1 = imageFile1.Path;

                            //ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + imageFileName + ".png";
                        }
                        else
                        {
                            ImagePath1 = null;
                            ImagePath2 = null;
                            ImagePath3 = null;
                        }
                    }
                }
            }
            else
            {
                var positionofdomain = -1;
                if (TotalDomainCollection != null && TotalDomainCollection.Any())
                {
                    positionofdomain = TotalDomainCollection.IndexOf(TotalDomainCollection.FirstOrDefault(p => p.DomainName == CurrentDomain));
                    if (positionofdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    IsNextShown = positionofdomain < TotalDomainCollection.Count - 1;
                }
                if (CurrentQuestion < domainContentCollection.Count)
                {
                    if (positionofdomain > 0)
                    {
                        IsPreviousShown = true;
                    }
                    else
                    {
                        IsPreviousShown = CurrentQuestion > 0;
                    }
                    IsNextShown = (CurrentQuestion < domainContentCollection.Count - 1) || positionofdomain < TotalDomainCollection.Count - 1;
                    ObservationStr = "Observation";
                    InterviewStr = "Interview";
                    StructuredStr = "Structured";
                    CheckStructure = CheckObservation = CheckInterview = false;
                    StructureFontAttribute = ObservationFontAttribute = InterviewFontAttribute = FontAttributes.None;
                    StructureBorderColor = ObservationBorderColor = InterviewBorderColor = Color.Gray;
                    StructureDisabled = ObservedDisabled = InterviewDisabled = Color.FromHex("#3d3f3f");
                    ItemNumber = domainContentCollection[CurrentQuestion].ItemNumber;
                    ItemAbbrevation = domainContentCollection[CurrentQuestion].ItemAbbrevation;// itemCode.Split(' ')[0];
                    BehaviorText = domainContentCollection[CurrentQuestion].BehaviorContent;// itemText;
                    BehaviorTextFilePath = domainContentCollection[CurrentQuestion].BehaviourHtmlFIlePath;
                    MaterialText = domainContentCollection[CurrentQuestion].MaterialContent;
                    MaterialHtmlFilePath = domainContentCollection[CurrentQuestion].MaterialContentFilePath;
                    MaxTime = domainContentCollection[CurrentQuestion].MaxTime;
                    ScoringText = domainContentCollection[CurrentQuestion].ScoringNotes;
                    ScoringNotes = domainContentCollection[CurrentQuestion].ScoringCustomData;
                    TotalScoringText = (!string.IsNullOrEmpty(ScoringNotes) ? ScoringNotes.Replace("Note:", "<span class='scoringClass'><b>Note:</b></span>") : "") + (!string.IsNullOrEmpty(ScoringText) ? "<br/>" + "<span class='scoringClass'><b>" + ScoringText + "</b></span>" : "");
                    //Commented for the previouspage is visible when we reverse back the items in scoring logics.
                    //IsPreviousShown = true;
                    IsNextShown = (CurrentQuestion < domainContentCollection.Count - 1) || positionofdomain < TotalSubdomainCollection.Count - 1;

                    if (domainContentCollection[CurrentQuestion].TallyContent != null)
                    {
                        if (domainContentCollection[CurrentQuestion].TallyContent.Count == 0)
                        {
                            ContentItemTalliesCollection = null;
                            IsScoringOnly = true;
                            IsTallyAndScoring = false;
                        }

                        if (domainContentCollection[CurrentQuestion].TallyContent.Count > 0)
                        {
                            ContentItemTalliesCollection = domainContentCollection[CurrentQuestion].TallyContent;
                            IsScoringOnly = false;
                            IsTallyAndScoring = true;
                        }
                    }

                    if (domainContentCollection[CurrentQuestion].TallyContent != null && domainContentCollection[CurrentQuestion].TallyContent.Count > 0 && (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null || SubdomainCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null))
                    {
                        TallyTabTappedCommand.Execute(new object());
                    }
                    else
                    {
                        ScoringTabTappedCommand.Execute(new object());
                    }

                    if (domainContentCollection[CurrentQuestion].CaptureModeDesc != null)
                    {
                        CaptureMode = domainContentCollection[CurrentQuestion].CaptureModeDesc;
                        StructureEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null;
                        ObservationEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null;
                        InterviewEnabled = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null;

                        if (CaptureMode.StructuredCollectionData != null && CaptureMode.StructuredCollectionData.Any())
                        {
                            ObservationEnabled = CaptureMode.StructuredCollectionData.Count >= 1;
                            InterviewEnabled = CaptureMode.StructuredCollectionData.Count == 2;
                            ObservationStr = ObservationEnabled ? "Structured" : "Observation";
                            InterviewStr = InterviewEnabled ? "Structured" : "Interview";
                            domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent = ObservationEnabled ? CaptureMode.StructuredCollectionData.FirstOrDefault() : null;
                            domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent = InterviewEnabled ? CaptureMode.StructuredCollectionData.LastOrDefault() : null;
                        }

                        var noSelection = !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (!noSelection)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = false;
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = false;
                                }
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                            {
                                var hasScore = false;
                                if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any() && domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                {
                                    var selectedScore = domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == domainContentCollection[CurrentQuestion].CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                    if (selectedScore != null)
                                    {
                                        hasScore = true;
                                        selectedScore.IsSelected = true;
                                    }
                                }
                                if (!hasScore)
                                {
                                    domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = false;
                                }
                            }
                        }
                        noSelection = !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected && !domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected;
                        if (noSelection)
                        {
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected = true;
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected = true;
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                            {
                                domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected = true;
                            }
                        }

                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.IsStructredSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent.HTMilFilePath;
                            CheckStructure = true;
                            StructureBorderColor = Colors.FrameBlueColor;
                            StructureFontAttribute = FontAttributes.Bold;
                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedStructuredContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.IsObservationSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent.HTMilFilePath;
                            CheckObservation = true;
                            ObservationBorderColor = Colors.FrameBlueColor;
                            ObservationFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedObservationContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.IsInterViewSelected)
                        {
                            CaptureModeText = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.ContentDescription;
                            CaptureModeTextFilePath = domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent.HTMilFilePath;
                            CheckInterview = true;
                            InterviewBorderColor = Colors.FrameBlueColor;
                            InterviewFontAttribute = FontAttributes.Bold;

                            if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContentScoring.OrderByDescending(p => p.points));
                            }
                            else if (domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring != null && domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.Any())
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>(domainContentCollection[CurrentQuestion].CaptureModeDesc.DefaultContentScoring.OrderByDescending(p => p.points));
                            }
                            else
                            {
                                ContentRubricPointCollection = new List<ContentRubricPoint>();
                            }
                            ContentRubricPointCollection.ForEach(p => p.IsSelected = false);
                            if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedDefaultRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedDefaultRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                            else if (ContentRubricPointCollection != null && ContentRubricPointCollection.Any() && CaptureMode.SelectedInterviewContentRubicPoint != null)
                            {
                                var selectedScore = ContentRubricPointCollection.FirstOrDefault(p => p.contentRubricPointsId == CaptureMode.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                if (selectedScore != null)
                                {
                                    selectedScore.IsSelected = true;
                                }
                            }
                        }

                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.StructuredContent != null)
                        {
                            StructureEnabled = true;
                        }
                        else
                        {
                            //Set it for whole Strucuture StackLayout.
                            StructureEnabled = false;
                            //Structure Text color changed.
                            StructureDisabled = Color.FromHex("#979797");
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.ObservationContent != null)
                        {
                            ObservationEnabled = true;
                        }
                        else
                        {
                            ObservationEnabled = false;
                            ObservedDisabled = Color.FromHex("#979797");
                        }
                        if (domainContentCollection[CurrentQuestion].CaptureModeDesc.InterviewContent != null)
                        {
                            InterviewEnabled = true;
                        }
                        else
                        {
                            InterviewEnabled = false;
                            InterviewDisabled = Color.FromHex("#979797");
                        }
                    }
                    if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count == 2)
                    {
                        string image1FileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                        string image2FileName = (domainContentCollection[CurrentQuestion].Images[1]).ToString().Split(':')[1].Trim();

                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                        ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(image1FileName + ".png");
                        ExistenceCheckResult exist2 = await imageFolder.CheckExistsAsync(image2FileName + ".png");
                        if (exist1 == ExistenceCheckResult.FileExists)
                        {
                            var imageFile1 = await imageFolder.GetFileAsync(image1FileName + ".png");
                            ImagePath1 = imageFile1.Path;
                        }

                        if (exist2 == ExistenceCheckResult.FileExists)
                        {
                            var imageFile2 = await imageFolder.GetFileAsync(image2FileName + ".png");
                            ImagePath2 = imageFile2.Path;
                        }

                        /*ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image1FileName + ".png";
                        ImagePath2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + image2FileName + ".png";*/
                    }
                    else if (domainContentCollection[CurrentQuestion].Images != null && domainContentCollection[CurrentQuestion].Images.Count > 0)
                    {

                        string imageFileName = (domainContentCollection[CurrentQuestion].Images[0]).ToString().Split(':')[1].Trim();
                        PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                        ExistenceCheckResult exist = await imageFolder.CheckExistsAsync(imageFileName + ".png");
                        if (exist == ExistenceCheckResult.FileExists)
                        {
                            var imageFile1 = await imageFolder.GetFileAsync(imageFileName + ".png");
                            ImagePath1 = imageFile1.Path;
                        }

                        //ImagePath1 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Images") + "\\" + imageFileName + ".png";
                    }
                    else
                    {
                        ImagePath1 = null;
                        ImagePath2 = null;
                    }
                }
            }

            await Task.Delay(600);
            UserDialogs.Instance.HideLoading();
        }

        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public bool isFirstRecord = false;
        public void Check30DayIssue()
        {
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
            {
                isFirstRecord = false;
                var testform = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == CaptureMode.DomainCategoryId);
                if (testform != null && commonDataService.StudentTestForms.IndexOf(testform) == 0)
                {
                    isFirstRecord = true;
                }
                var selectedTestRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == CaptureMode.DomainCategoryId);
                if (selectedTestRecord != null)
                {
                    var maxDateItem = commonDataService.StudentTestForms.OrderByDescending(p => p.DateOfTest).FirstOrDefault();
                    var mindateItem = commonDataService.StudentTestForms.OrderBy(p => p.DateOfTest).FirstOrDefault();
                    var dataservice = DependencyService.Get<ICommonDataService>();
                    var dobSpliteDate = dataservice.DOB.Split('/');
                    DateTime dobDateTime = new DateTime(Convert.ToInt32(dobSpliteDate[2]), Convert.ToInt32(dobSpliteDate[0]), Convert.ToInt32(dobSpliteDate[1]));
                    if (maxDateItem != null && mindateItem != null)
                    {
                        var dateDiff = maxDateItem.DateOfTest - mindateItem.DateOfTest;
                        if (dateDiff.TotalDays == 0)
                        {
                            var mindate = mindateItem.DateOfTest.AddDays(-30);
                            if (mindate < dobDateTime.Date)
                            {
                                MinDate = dobDateTime.Date;
                            }
                            else
                            {
                                MinDate = mindate;
                            }
                            var maxDate = maxDateItem.DateOfTest.AddDays(30);
                            if (maxDate > DateTime.Now.Date)
                            {
                                MaxDate = DateTime.Now;
                            }
                            else
                            {
                                MaxDate = maxDate;
                            }
                        }
                        else if (dateDiff.TotalDays == 30)
                        {
                            MinDate = mindateItem.DateOfTest;
                            MaxDate = maxDateItem.DateOfTest;
                        }
                        else
                        {
                            var diffDays = 30 - dateDiff.TotalDays;
                            var mindate = mindateItem.DateOfTest.AddDays(-diffDays);
                            if (mindate < dobDateTime.Date)
                            {
                                MinDate = dobDateTime.Date;
                            }
                            else
                            {
                                MinDate = mindate;
                            }
                            var maxDate = maxDateItem.DateOfTest.AddDays(diffDays);
                            if (maxDate > DateTime.Now.Date)
                            {
                                MaxDate = DateTime.Now;
                            }
                            else
                            {
                                MaxDate = maxDate.Date;
                            }
                        }
                    }
                }
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

        #endregion

        #region SaveRegion
        public ICommand UnSaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    ResetWebView?.Invoke();
                    if (ChildTapped)
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "Administrationpage");
                        App.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                    }
                    else
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "Administrationpage");
                        _navigationService.ClearModalStack();
                        App.Current.MainPage = new Views.DashboardHomeView();
                    }
                });
            }
        }
        public async void ChecakAndUpdateRawScore()
        {
            await Task.Delay(0);
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
            {
                var needtoUpdateRecords = new List<StudentTestForms>();
                var updatedTestRecords = commonDataService.StudentTestForms;
                if (updatedTestRecords != null && updatedTestRecords.Any() && OriginalStudentTestForms != null && OriginalStudentTestForms.Any())
                {
                    foreach (var item in updatedTestRecords)
                    {
                        var oldlocalTestRecord = OriginalStudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.contentCategoryId);
                        if (oldlocalTestRecord.rawScore != item.rawScore)
                        {
                            item.rawScore = oldlocalTestRecord.rawScore;
                            needtoUpdateRecords.Add(item);
                        }
                        if (oldlocalTestRecord.Notes != item.Notes)
                        {
                            item.Notes = oldlocalTestRecord.Notes;
                            if (!needtoUpdateRecords.Contains(item))
                            {
                                needtoUpdateRecords.Add(item);
                            }
                        }
                        if (oldlocalTestRecord.rawScoreEnabled != item.rawScoreEnabled)
                        {
                            item.rawScoreEnabled = oldlocalTestRecord.rawScoreEnabled;
                            if (!needtoUpdateRecords.Contains(item))
                            {
                                needtoUpdateRecords.Add(item);
                            }
                        }
                        if (oldlocalTestRecord.testDate != item.testDate)
                        {
                            item.testDate = oldlocalTestRecord.testDate;
                            if (!needtoUpdateRecords.Contains(item))
                            {
                                needtoUpdateRecords.Add(item);
                            }
                        }
                    }
                }
                if (needtoUpdateRecords != null && needtoUpdateRecords.Any())
                {
                    studentTestFormsService.UpdateAll(needtoUpdateRecords);
                }
            }
        }

        public ICommand SaveAndContinue
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    UserDialogs.Instance.ShowLoading("Saving Test Form...");
                    await Task.Delay(300);
                    TestFormSave();
                    UserDialogs.Instance.HideLoading();
                    ResetWebView?.Invoke();
                    if (ChildTapped)
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "Administrationpage");
                        App.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                    }
                    else
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "Administrationpage");
                        _navigationService.ClearModalStack();
                        App.Current.MainPage = new Views.DashboardHomeView();
                    }
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ChildTapped = false;
                    await PopupNavigation.Instance.PopAllAsync();
                });
            }
        }
        public bool HasDoneChanges { get; set; }
        private void TestFormSave()
        {
            var hasScored = false;
            int addedBy;
            int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
            var lstStudentTestFormResponses = new List<StudentTestFormResponses>();
            commonDataService.StudentTestFormOverview.IsFormSaved = true;
            commonDataService.StudentTestFormOverview.FormStatus = "Not started";
            commonDataService.StudentTestFormOverview.notes = FormNotes;

            if (IsBattelleDevelopmentCompleteChecked)
            {
                if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                {
                    foreach (var item in commonDataService.StudentTestForms)
                    {
                        item.rawScore = TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == item.contentCategoryId).subdomainscore;
                    }
                    if (commonDataService.StudentTestForms.Any(p => p.rawScore.HasValue))
                    {
                        commonDataService.StudentTestFormOverview.FormStatus = "Saved";
                    }
                }
                if (TotalCategories != null && TotalCategories.Any())
                {
                    var level2Categories = TotalCategories.Where(p => p.contentCategoryLevelId == 2).ToList();
                    if (level2Categories != null && level2Categories.Any())
                    {
                        foreach (var level2Item in level2Categories)
                        {
                            var lstFormJsonClass = new List<FormJsonClass>();
                            var startingPoint = StartingPointCatrgories.Where(p => p.parentContentCategoryId == level2Item.contentCategoryId);
                            if (startingPoint != null && startingPoint.Any())
                            {
                                foreach (var item in startingPoint)
                                {
                                    var section = new FormJsonClass();
                                    section.sectionId = item.contentCategoryId;
                                    section.items = new List<ItemInfo>();
                                    lstFormJsonClass.Add(section);

                                    var startingContentItems = commonDataService.ContentCategoryItems.Where(p => p.contentCategoryId == item.contentCategoryId);
                                    if (startingContentItems != null && startingContentItems.Any())
                                    {
                                        foreach (var startcontentitem in startingContentItems)
                                        {
                                            foreach (var itemcategory in TotalSubdomainCollection)
                                            {
                                                if (itemcategory.AdministrationContents != null && itemcategory.AdministrationContents.Any())
                                                {
                                                    foreach (var contentitem in itemcategory.AdministrationContents)
                                                    {
                                                        if (contentitem.ContentItemID == startcontentitem.contentItemId)
                                                        {
                                                            var hasAnyScoreSelected = false;
                                                            var itemInfo = new ItemInfo();
                                                            section.items.Add(itemInfo);
                                                            itemInfo.itemId = contentitem.ContentItemID;
                                                            itemInfo.itemNotes = contentitem.CaptureModeDesc.Notes;
                                                            if (contentitem.CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                                            {
                                                                var selectedScore = contentitem.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                                                if (selectedScore != null)
                                                                {
                                                                    hasAnyScoreSelected = true;
                                                                    hasScored = true;
                                                                    itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                    itemInfo.itemScore = selectedScore.points;
                                                                }
                                                            }
                                                            else if (contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                                            {
                                                                var selectedScore = contentitem.CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                                                if (selectedScore != null)
                                                                {
                                                                    hasAnyScoreSelected = true;
                                                                    hasScored = true;
                                                                    itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                    itemInfo.itemScore = selectedScore.points;
                                                                }
                                                            }
                                                            else if (contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                                            {
                                                                var selectedScore = contentitem.CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                                                if (selectedScore != null)
                                                                {
                                                                    hasAnyScoreSelected = true;
                                                                    hasScored = true;
                                                                    itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                    itemInfo.itemScore = selectedScore.points;
                                                                }
                                                            }
                                                            else if (contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                                            {
                                                                var selectedScore = contentitem.CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                                                if (selectedScore != null)
                                                                {
                                                                    hasAnyScoreSelected = true;
                                                                    hasScored = true;
                                                                    itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                    itemInfo.itemScore = selectedScore.points;
                                                                }
                                                            }
                                                            if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                                            {
                                                                var answeredTally = contentitem.TallyContent.Where(p => p.CheckCorrectVisible || p.CheckInCorrectVisible);
                                                                if (answeredTally != null && answeredTally.Any())
                                                                {
                                                                    itemInfo.tallyItems = new List<TallyItemInfo>();
                                                                    foreach (var tallyItem in answeredTally)
                                                                    {
                                                                        hasAnyScoreSelected = true;
                                                                        hasScored = true;
                                                                        var TallyItemInfo = new TallyItemInfo();
                                                                        TallyItemInfo.itemTallyId = tallyItem.contentItemTallyId;
                                                                        TallyItemInfo.tallyScore = tallyItem.CheckCorrectVisible ? 1 : 0;
                                                                        itemInfo.tallyItems.Add(TallyItemInfo);
                                                                    }
                                                                }
                                                            }
                                                            if (hasAnyScoreSelected)
                                                            {
                                                                itemInfo.captureMode = contentitem.CaptureModeDesc.IsStructredSelected ? "S" : contentitem.CaptureModeDesc.IsInterViewSelected ? "I" : contentitem.CaptureModeDesc.IsObservationSelected ? "O" : null;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            var StudentTestFormResponses = new StudentTestFormResponses();
                            StudentTestFormResponses.ContentCategoryId = level2Item.contentCategoryId;
                            StudentTestFormResponses.LocalFormInstanceId = LocaInstanceID;
                            StudentTestFormResponses.Response = JsonConvert.SerializeObject(lstFormJsonClass);
                            StudentTestFormResponses.CreatedBy = addedBy;
                            StudentTestFormResponses.CreatedOn = DateTime.Now;
                            lstStudentTestFormResponses.Add(StudentTestFormResponses);
                        }
                    }
                }
            }
            else
            {
                if (TotalDomainCollection != null && TotalDomainCollection.Any())
                {
                    foreach (var item in commonDataService.StudentTestForms)
                    {
                        item.rawScore = TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == item.contentCategoryId).rawscore;
                    }
                    if (commonDataService.StudentTestForms.Any(p => p.rawScore.HasValue))
                    {
                        commonDataService.StudentTestFormOverview.FormStatus = "Saved";
                    }
                }
                if (TotalCategories != null && TotalCategories.Any())
                {
                    var Categories = TotalCategories.Where(p => p.contentCategoryLevelId == 4).ToList();
                    if (Categories != null && Categories.Any())
                    {
                        foreach (var level2Item in Categories)
                        {
                            var lstFormJsonClass = new List<FormJsonClass>();
                            if (ScreenerStartingPointCatrgories != null)
                            {
                                var startingPoint = ScreenerStartingPointCatrgories.Where(p => p.parentContentCategoryId == level2Item.contentCategoryId);
                                if (startingPoint != null && startingPoint.Any())
                                {
                                    foreach (var item in startingPoint)
                                    {
                                        var section = new FormJsonClass();
                                        section.sectionId = item.contentCategoryId;
                                        section.items = new List<ItemInfo>();
                                        lstFormJsonClass.Add(section);

                                        var startingContentItems = commonDataService.ContentCategoryItems.Where(p => p.contentCategoryId == item.contentCategoryId);
                                        if (startingContentItems != null && startingContentItems.Any())
                                        {
                                            foreach (var startcontentitem in startingContentItems)
                                            {
                                                foreach (var itemcategory in TotalDomainCollection)
                                                {
                                                    if (itemcategory.AdministrationContents != null && itemcategory.AdministrationContents.Any())
                                                    {
                                                        foreach (var contentitem in itemcategory.AdministrationContents)
                                                        {
                                                            if (contentitem.ContentItemID == startcontentitem.contentItemId)
                                                            {
                                                                var anyScoreSelected = false;
                                                                var itemInfo = new ItemInfo();
                                                                section.items.Add(itemInfo);
                                                                itemInfo.itemId = contentitem.ContentItemID;
                                                                itemInfo.itemNotes = contentitem.CaptureModeDesc.Notes;
                                                                if (contentitem.CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                                                {
                                                                    var selectedScore = contentitem.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                                                    if (selectedScore != null)
                                                                    {
                                                                        anyScoreSelected = true;
                                                                        hasScored = true;
                                                                        itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                        itemInfo.itemScore = selectedScore.points;
                                                                    }
                                                                }
                                                                else if (contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                                                {
                                                                    var selectedScore = contentitem.CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                                                    if (selectedScore != null)
                                                                    {
                                                                        anyScoreSelected = true;
                                                                        hasScored = true;
                                                                        itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                        itemInfo.itemScore = selectedScore.points;
                                                                    }
                                                                }
                                                                else if (contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                                                {
                                                                    var selectedScore = contentitem.CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                                                    if (selectedScore != null)
                                                                    {
                                                                        anyScoreSelected = true;
                                                                        hasScored = true;
                                                                        itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                        itemInfo.itemScore = selectedScore.points;
                                                                    }
                                                                }
                                                                else if (contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                                                {
                                                                    var selectedScore = contentitem.CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                                                    if (selectedScore != null)
                                                                    {
                                                                        anyScoreSelected = true;
                                                                        hasScored = true;
                                                                        itemInfo.ContentRubricPointId = selectedScore.contentRubricPointsId;
                                                                        itemInfo.itemScore = selectedScore.points;
                                                                    }
                                                                }
                                                                if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                                                {
                                                                    var answeredTally = contentitem.TallyContent.Where(p => p.CheckCorrectVisible || p.CheckInCorrectVisible);
                                                                    if (answeredTally != null && answeredTally.Any())
                                                                    {
                                                                        anyScoreSelected = true;
                                                                        hasScored = true;
                                                                        itemInfo.tallyItems = new List<TallyItemInfo>();
                                                                        foreach (var tallyItem in answeredTally)
                                                                        {
                                                                            var TallyItemInfo = new TallyItemInfo();
                                                                            TallyItemInfo.itemTallyId = tallyItem.contentItemTallyId;
                                                                            TallyItemInfo.tallyScore = tallyItem.CheckCorrectVisible ? 1 : 0;
                                                                            itemInfo.tallyItems.Add(TallyItemInfo);
                                                                        }
                                                                    }
                                                                }
                                                                if (anyScoreSelected)
                                                                {
                                                                    itemInfo.captureMode = contentitem.CaptureModeDesc.IsStructredSelected ? "S" : contentitem.CaptureModeDesc.IsInterViewSelected ? "I" : contentitem.CaptureModeDesc.IsObservationSelected ? "O" : null;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            var StudentTestFormResponses = new StudentTestFormResponses();
                            StudentTestFormResponses.ContentCategoryId = level2Item.contentCategoryId;
                            StudentTestFormResponses.LocalFormInstanceId = LocaInstanceID;
                            StudentTestFormResponses.Response = JsonConvert.SerializeObject(lstFormJsonClass);
                            StudentTestFormResponses.CreatedBy = addedBy;
                            StudentTestFormResponses.CreatedOn = DateTime.Now;
                            lstStudentTestFormResponses.Add(StudentTestFormResponses);
                        }
                    }
                }
            }
            if (commonDataService.StudentTestForms.Any(p => p.BaselCeilingReached) || commonDataService.StudentTestForms.Any(p => p.rawScore.HasValue))
            {
                commonDataService.StudentTestFormOverview.FormStatus = "Saved";
            }
            else
            {
                commonDataService.StudentTestFormOverview.FormStatus = hasScored ? "In-Progress" : "Not started";
            }
            clinicalTestFormService.UpdateTestForm(commonDataService.StudentTestFormOverview);
            OriginalStudentTestFormOverView = JsonConvert.DeserializeObject<StudentTestFormOverview>(JsonConvert.SerializeObject(commonDataService.StudentTestFormOverview));
            studentTestFormsService.UpdateAll(commonDataService.StudentTestForms);
            OriginalStudentTestForms = new List<StudentTestForms>(commonDataService.StudentTestForms);
            if (lstStudentTestFormResponses != null && lstStudentTestFormResponses.Any())
            {
                studentTestFormResponsesService.DeleteAll(LocaInstanceID);
                studentTestFormResponsesService.InsertAll(lstStudentTestFormResponses);
            }
            else
            {
                studentTestFormResponsesService.DeleteAll(LocaInstanceID);
            }
        }
        public async void SaveTestForm()
        {
            if (userTappedSave)
                return;
            userTappedSave = true;

            UserDialogs.Instance.ShowLoading("Saving Test Form...");
            await Task.Delay(300);
            TestFormSave();
            HasDoneChanges = false;
            UserDialogs.Instance.HideLoading();
            //await UserDialogs.Instance.AlertAsync("Saved Successfully", "FormSave");
            await PopupNavigation.Instance.PushAsync(new FormSavePopUp() { BindingContext = this });
            await Task.Delay(500);
            userTappedSave = false;

        }
        #endregion

        #region Reset Region
        public void ResetScroes(List<int> selectedsubDomains)
        {
            if (selectedsubDomains != null && selectedsubDomains.Any())
            {
                if (IsBattelleDevelopmentCompleteChecked)
                {
                    if (selectedsubDomains.Any(p => p == SubdomainCollection[CurrentQuestion].CaptureModeDesc.DomainCategoryId))
                    {
                        LastItemScored = null;
                    }

                    foreach (var item in selectedsubDomains)
                    {
                        if (TotalSubdomainCollection != null && TotalSubdomainCollection.Any())
                        {
                            var contentCategories = TotalSubdomainCollection.Where(p => p.SubDomainCategoryId == item);
                            if (contentCategories != null && contentCategories.Any())
                            {
                                foreach (var inneritem in contentCategories)
                                {
                                    if (inneritem.AdministrationContents != null && inneritem.AdministrationContents.Any())
                                    {
                                        foreach (var contentitem in inneritem.AdministrationContents)
                                        {
                                            if (contentitem.CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedDefaultRubicPoint = null;
                                            }
                                            if (contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint = null;
                                            }
                                            if (contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint = null;
                                            }
                                            if (contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint = null;
                                            }
                                            if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                            {
                                                var answeredTally = contentitem.TallyContent.Where(p => p.CheckCorrectVisible || p.CheckInCorrectVisible);
                                                if (answeredTally != null && answeredTally.Any())
                                                {
                                                    HasDoneChanges = true;
                                                    foreach (var tallyItem in answeredTally)
                                                    {
                                                        tallyItem.IsCorrectChecked = false;
                                                        tallyItem.IsInCorrectChecked = false;
                                                        tallyItem.UncheckCorrectVisible = true;
                                                        tallyItem.UncheckInCorrectVisible = true;
                                                        tallyItem.CheckCorrectVisible = false;
                                                        tallyItem.CheckInCorrectVisible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in selectedsubDomains)
                    {
                        TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == item).subdomainscore = null;
                        TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == item).BasalCeilingReached = false;
                        var testRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item);
                        if (testRecord != null)
                        {
                            testRecord.BaselCeilingReached = false;
                            testRecord.rawScore = null;
                            testRecord.rawScoreEnabled = true;
                            testRecord.IsScoreSelected = false;
                            testRecord.TSOStatus = "Not Started";
                        }
                    }
                    CalculateBaselCeiling();
                }
                else
                {
                    if (selectedsubDomains.Any(p => p == domainContentCollection[CurrentQuestion].CaptureModeDesc.DomainCategoryId))
                    {
                        LastItemScored = null;
                    }
                    foreach (var item in selectedsubDomains)
                    {
                        if (TotalDomainCollection != null && TotalDomainCollection.Any())
                        {
                            var contentCategories = TotalDomainCollection.Where(p => p.DoaminCategoryId == item);
                            if (contentCategories != null && contentCategories.Any())
                            {
                                foreach (var inneritem in contentCategories)
                                {
                                    if (inneritem.AdministrationContents != null && inneritem.AdministrationContents.Any())
                                    {
                                        foreach (var contentitem in inneritem.AdministrationContents)
                                        {
                                            if (contentitem.CaptureModeDesc.SelectedDefaultRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.DefaultContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedDefaultRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedDefaultRubicPoint = null;
                                            }
                                            if (contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.StructuredContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedStructuredContentRubicPoint = null;
                                            }
                                            if (contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.ObservationContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedObservationContentRubicPoint = null;
                                            }
                                            if (contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null)
                                            {
                                                var selectedScore = contentitem.CaptureModeDesc.InterviewContentScoring.FirstOrDefault(p => p.contentRubricPointsId == contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint.contentRubricPointsId);
                                                if (selectedScore != null)
                                                {
                                                    selectedScore.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }
                                                contentitem.CaptureModeDesc.SelectedInterviewContentRubicPoint = null;
                                            }
                                            if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                            {
                                                var answeredTally = contentitem.TallyContent.Where(p => p.CheckCorrectVisible || p.CheckInCorrectVisible);
                                                if (answeredTally != null && answeredTally.Any())
                                                {
                                                    HasDoneChanges = true;
                                                    foreach (var tallyItem in answeredTally)
                                                    {
                                                        tallyItem.IsCorrectChecked = false;
                                                        tallyItem.IsInCorrectChecked = false;
                                                        tallyItem.UncheckCorrectVisible = true;
                                                        tallyItem.UncheckInCorrectVisible = true;
                                                        tallyItem.CheckCorrectVisible = false;
                                                        tallyItem.CheckInCorrectVisible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (var item in selectedsubDomains)
                    {
                        TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == item).rawscore = null;
                        TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == item).BasalCeilingObtained = false;
                        var testRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item);
                        if (testRecord != null)
                        {
                            testRecord.IsScoreSelected = false;
                            testRecord.TSOStatus = "Not Started";
                            testRecord.BaselCeilingReached = false;
                            testRecord.rawScore = null;
                            testRecord.rawScoreEnabled = true;
                        }
                    }
                    CalculateBaselCeiling();
                }
            }
        }
        #endregion

        #region Basel/Ceiling
        public ICommand CancelBCCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                });
            }
        }
        public ICommand SaveBCCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    UserDialogs.Instance.ShowLoading("Saving Test Form...");
                    await Task.Delay(300);
                    TestFormSave();
                    HasDoneChanges = false;
                    await LoadTSO();
                });
            }
        }
        public List<AdministrationContent> SequnetialScoredItems { get; set; }
        async void CeilingObtainedPopupView()
        {
            ResetTimer();
            await PopupNavigation.Instance.PushAsync(new CeilingObtainedPopupView(AdministrationHeader + ".") { BindingContext = this }, false);
        }
        private void CalculateBaselCeiling()
        {
            if (commonDataService.IsCompleteForm)
            {
                if (SubdomainCollection != null && SubdomainCollection.Any())
                {
                    if (commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == SubdomainCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).rawScoreEnabled)
                    {
                        BaselObtained = false;
                        BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                        CeilingObtained = false;
                        CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                        return;
                    }
                    var baselObtained = false;
                    var ceilingObtained = false;
                    var lstBasalItems = new List<AdministrationContent>();

                    var baselceilingReached = BaselObtained && CeilingObtained;
                    BaselObtained = false;
                    CeilingObtained = false;
                    var firstItemScore = -1;
                    var firstitem = SubdomainCollection.FirstOrDefault();
                    var baselItemsCount = 3;
                    var ceilingItemsCount = 3;
                    var baselScore = 2;
                    var ceilingScore = 0;
                    var baselObtainedItem = default(AdministrationContent);
                    var ceilingObtainedItem = default(AdministrationContent);
                    var contentBaseCeiling = ContentBasalCeilingsItems.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId);
                    if (contentBaseCeiling != null)
                    {
                        baselItemsCount = contentBaseCeiling.basalCount;
                        ceilingItemsCount = contentBaseCeiling.ceilingCount;
                        baselScore = contentBaseCeiling.basalScore;
                        ceilingScore = contentBaseCeiling.ceilingScore;
                    }
                    if (firstitem.CaptureModeDesc != null && (firstitem.CaptureModeDesc.SelectedDefaultRubicPoint != null || firstitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ||
                            firstitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || firstitem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                    {
                        var score = firstitem.CaptureModeDesc.SelectedDefaultRubicPoint != null ? firstitem.CaptureModeDesc.SelectedDefaultRubicPoint : (firstitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                firstitem.CaptureModeDesc.SelectedInterviewContentRubicPoint : (firstitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? firstitem.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                firstitem.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                        firstItemScore = score.points;
                    }
                    if (firstItemScore != -1)
                    {
                        baselObtainedItem = firstitem;
                        baselObtained = true;
                    }

                    var allScoredItems = new List<AdministrationContent>();
                    foreach (var item in SubdomainCollection)
                    {
                        var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                item.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                        if (score != null)
                        {
                            allScoredItems.Add(item);
                        }
                    }
                    var hasSequenceScored = true;
                    SequnetialScoredItems = new List<AdministrationContent>();
                    for (int i = 0; i < allScoredItems.Count - 1; i++)
                    {
                        if (i + 1 <= allScoredItems.Count - 1)
                        {
                            if (Convert.ToInt32(allScoredItems[i + 1].ItemNumber) - Convert.ToInt32(allScoredItems[i].ItemNumber) == 1)
                            {
                                if (!SequnetialScoredItems.Contains(allScoredItems[i]))
                                {
                                    SequnetialScoredItems.Add(allScoredItems[i]);
                                }
                                if (!SequnetialScoredItems.Contains(allScoredItems[i + 1]))
                                {
                                    SequnetialScoredItems.Add(allScoredItems[i + 1]);
                                }
                            }
                            else
                            {
                                hasSequenceScored = false;
                                break;
                            }
                        }
                    }

                    if (firstItemScore == -1)
                    {
                        if (SequnetialScoredItems != null && SequnetialScoredItems.Any())
                        {
                            var sequentilFirstItem = SequnetialScoredItems.FirstOrDefault();
                            var seqfirstscore = sequentilFirstItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ? sequentilFirstItem.CaptureModeDesc.SelectedDefaultRubicPoint : (sequentilFirstItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                            sequentilFirstItem.CaptureModeDesc.SelectedInterviewContentRubicPoint : (sequentilFirstItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? sequentilFirstItem.CaptureModeDesc.SelectedObservationContentRubicPoint : sequentilFirstItem.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                            if (seqfirstscore.points == 0 || seqfirstscore.points == 1)
                            {
                                baselObtained = false;
                            }
                            else
                            {
                                var basalCount = 0;
                                foreach (var item in SequnetialScoredItems)
                                {
                                    var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                                item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint : item.CaptureModeDesc.SelectedStructuredContentRubicPoint));

                                    if (score.points == baselScore)
                                    {
                                        basalCount += 1;
                                        if (basalCount >= baselItemsCount)
                                        {
                                            baselObtainedItem = SequnetialScoredItems[SequnetialScoredItems.IndexOf(item) - baselItemsCount + 1];
                                            baselObtained = true;
                                            break;
                                        }
                                    }
                                    else if (!baselObtained)
                                    {
                                        basalCount = 0;
                                    }
                                }
                            }
                        }
                    }
                    if (baselObtained)
                    {
                        if (SequnetialScoredItems != null && SequnetialScoredItems.Any())
                        {
                            var ceilingCount = 0;
                            foreach (var item in SequnetialScoredItems)
                            {
                                var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                            item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                            item.CaptureModeDesc.SelectedStructuredContentRubicPoint));

                                if (score.points == ceilingScore)
                                {
                                    ceilingCount += 1;
                                    if (ceilingCount >= ceilingItemsCount)
                                    {
                                        ceilingObtainedItem = item;
                                    }
                                }
                                else
                                {
                                    ceilingObtainedItem = null;
                                    ceilingCount = 0;
                                }
                            }
                        }
                    }

                    if (baselObtained && ceilingObtainedItem != null && hasSequenceScored)
                    {
                        ceilingObtained = true;
                    }
                    if (baselObtained && !ceilingObtained && hasSequenceScored)
                    {
                        var lastItemScored = SubdomainCollection.LastOrDefault();
                        var lastItemscore = lastItemScored.CaptureModeDesc.SelectedDefaultRubicPoint != null ? lastItemScored.CaptureModeDesc.SelectedDefaultRubicPoint : (lastItemScored.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                        lastItemScored.CaptureModeDesc.SelectedInterviewContentRubicPoint : (lastItemScored.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? lastItemScored.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                        lastItemScored.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                        if (lastItemscore != null)
                        {
                            ceilingObtainedItem = lastItemScored;
                            ceilingObtained = true;
                        }
                    }
                    BaselObtained = baselObtained;
                    BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                    CeilingObtained = baselObtained && ceilingObtained;
                    CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                    RawScore = null;
                    TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).subdomainscore = null;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawScore = RawScore;
                    TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BasalCeilingReached = false;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BaselCeilingReached = false;

                    if (BaselObtained && CeilingObtained)
                    {
                        var baselObtainedIndex = SubdomainCollection.IndexOf(baselObtainedItem);
                        var ceilingObtainedIndex = SubdomainCollection.IndexOf(ceilingObtainedItem);
                        for (int i = baselObtainedIndex; i <= ceilingObtainedIndex; i++)
                        {
                            var item = SubdomainCollection[i];
                            var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint : item.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                            if (!RawScore.HasValue)
                            {
                                RawScore = 0;
                            }
                            RawScore += score.points;
                        }
                        if (firstItemScore == -1)
                        {
                            var firstSequenceItem = SequnetialScoredItems.FirstOrDefault();
                            var index = SubdomainCollection.IndexOf(firstSequenceItem);
                            if (!RawScore.HasValue)
                            {
                                RawScore = 0;
                            }
                            RawScore += index * baselScore;
                        }
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawScore = RawScore;
                        TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).subdomainscore = RawScore;
                        TotalSubdomainCollection.FirstOrDefault(p => p.SubDomainCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BasalCeilingReached = true;
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BaselCeilingReached = true;

                        if (!InitialLoad && !baselceilingReached && BaselObtained && CeilingObtained)
                        {
                            CeilingObtainedPopupView();
                        }
                    }
                }
            }
            else if (commonDataService.IsScreenerForm)
            {
                if (domainContentCollection != null && domainContentCollection.Any())
                {
                    if (commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == domainContentCollection.FirstOrDefault().CaptureModeDesc.DomainCategoryId).rawScoreEnabled)
                    {
                        BaselObtained = false;
                        BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                        CeilingObtained = false;
                        CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                        return;
                    }
                    var baselObtained = false;
                    var ceilingObtained = false;
                    var lstBasalItems = new List<AdministrationContent>();
                    var baselceilingReached = BaselObtained && CeilingObtained;
                    BaselObtained = false;
                    CeilingObtained = false;
                    var firstItemScore = -1;
                    var firstitem = domainContentCollection.FirstOrDefault();

                    var baselItemsCount = 3;
                    var ceilingItemsCount = 3;
                    var baselScore = 2;
                    var ceilingScore = 0;
                    var baselObtainedItem = default(AdministrationContent);
                    var ceilingObtainedItem = default(AdministrationContent);
                    var contentBaseCeiling = ContentBasalCeilingsItems.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId);
                    if (contentBaseCeiling != null)
                    {
                        baselItemsCount = contentBaseCeiling.basalCount;
                        ceilingItemsCount = contentBaseCeiling.ceilingCount;
                        baselScore = contentBaseCeiling.basalScore;
                        ceilingScore = contentBaseCeiling.ceilingScore;
                    }
                    if (firstitem.CaptureModeDesc != null && (firstitem.CaptureModeDesc.SelectedDefaultRubicPoint != null || firstitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ||
                            firstitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null || firstitem.CaptureModeDesc.SelectedStructuredContentRubicPoint != null))
                    {
                        var score = firstitem.CaptureModeDesc.SelectedDefaultRubicPoint != null ? firstitem.CaptureModeDesc.SelectedDefaultRubicPoint : (firstitem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                firstitem.CaptureModeDesc.SelectedInterviewContentRubicPoint : (firstitem.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? firstitem.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                firstitem.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                        firstItemScore = score.points;
                    }
                    if (firstItemScore != -1)
                    {
                        baselObtainedItem = firstitem;
                        baselObtained = true;
                    }
                    var allScoredItems = new List<AdministrationContent>();
                    foreach (var item in domainContentCollection)
                    {
                        var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                item.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                        if (score != null)
                        {
                            allScoredItems.Add(item);
                        }
                    }
                    var hasSequenceScored = true;
                    SequnetialScoredItems = new List<AdministrationContent>();
                    for (int i = 0; i < allScoredItems.Count - 1; i++)
                    {
                        if (i + 1 <= allScoredItems.Count - 1)
                        {
                            if (Convert.ToInt32(allScoredItems[i + 1].ItemNumber) - Convert.ToInt32(allScoredItems[i].ItemNumber) == 1)
                            {
                                if (!SequnetialScoredItems.Contains(allScoredItems[i]))
                                {
                                    SequnetialScoredItems.Add(allScoredItems[i]);
                                }
                                if (!SequnetialScoredItems.Contains(allScoredItems[i + 1]))
                                {
                                    SequnetialScoredItems.Add(allScoredItems[i + 1]);
                                }
                            }
                            else
                            {
                                hasSequenceScored = false;
                                break;
                            }
                        }
                    }
                    if (firstItemScore == -1)
                    {
                        if (SequnetialScoredItems != null && SequnetialScoredItems.Any())
                        {
                            var sequentilFirstItem = SequnetialScoredItems.FirstOrDefault();
                            var seqfirstscore = sequentilFirstItem.CaptureModeDesc.SelectedDefaultRubicPoint != null ? sequentilFirstItem.CaptureModeDesc.SelectedDefaultRubicPoint : (sequentilFirstItem.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                            sequentilFirstItem.CaptureModeDesc.SelectedInterviewContentRubicPoint : (sequentilFirstItem.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? sequentilFirstItem.CaptureModeDesc.SelectedObservationContentRubicPoint : sequentilFirstItem.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                            if (seqfirstscore.points == 0 || seqfirstscore.points == 1)
                            {
                                baselObtained = false;
                            }
                            else
                            {
                                var basalCount = 0;
                                foreach (var item in SequnetialScoredItems)
                                {
                                    var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                                item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint : item.CaptureModeDesc.SelectedStructuredContentRubicPoint));

                                    if (score.points == baselScore)
                                    {
                                        basalCount += 1;
                                        if (basalCount >= baselItemsCount)
                                        {
                                            baselObtainedItem = SequnetialScoredItems[SequnetialScoredItems.IndexOf(item) - baselItemsCount + 1];
                                            baselObtained = true;
                                            break;
                                        }
                                    }
                                    else if (!baselObtained)
                                    {
                                        basalCount = 0;
                                    }
                                }
                            }
                        }
                    }
                    if (baselObtained)
                    {
                        var ceilingCount = 0;
                        foreach (var item in SequnetialScoredItems)
                        {
                            var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                        item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                        item.CaptureModeDesc.SelectedStructuredContentRubicPoint));

                            if (score.points == ceilingScore)
                            {
                                ceilingCount += 1;
                                if (ceilingCount >= ceilingItemsCount)
                                {
                                    ceilingObtainedItem = item;
                                    //break;
                                }
                            }
                            else
                            {
                                ceilingObtainedItem = null;
                                ceilingCount = 0;
                            }
                        }
                    }
                    if (ceilingObtainedItem != null && hasSequenceScored)
                    {
                        ceilingObtained = true;
                    }
                    if (baselObtained && !ceilingObtained && hasSequenceScored)
                    {
                        var lastItemScored = domainContentCollection.LastOrDefault();
                        var lastItemscore = lastItemScored.CaptureModeDesc.SelectedDefaultRubicPoint != null ? lastItemScored.CaptureModeDesc.SelectedDefaultRubicPoint : (lastItemScored.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                        lastItemScored.CaptureModeDesc.SelectedInterviewContentRubicPoint : (lastItemScored.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? lastItemScored.CaptureModeDesc.SelectedObservationContentRubicPoint :
                                        lastItemScored.CaptureModeDesc.SelectedStructuredContentRubicPoint));
                        if (lastItemscore != null)
                        {
                            ceilingObtainedItem = lastItemScored;
                            ceilingObtained = true;
                        }
                    }
                    BaselObtained = baselObtained;
                    BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                    CeilingObtained = baselObtained && ceilingObtained;
                    CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                    RawScore = null;
                    TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawscore = null;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawScoreEnabled = false;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawScore = RawScore;
                    TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BasalCeilingObtained = false;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BaselCeilingReached = false;
                    if (BaselObtained && CeilingObtained)
                    {
                        if (!RawScore.HasValue)
                        {
                            RawScore = 0;
                        }
                        var baselObtainedIndex = domainContentCollection.IndexOf(baselObtainedItem);
                        var ceilingObtainedIndex = domainContentCollection.IndexOf(ceilingObtainedItem);
                        for (int i = baselObtainedIndex; i <= ceilingObtainedIndex; i++)
                        {
                            var item = domainContentCollection[i];
                            var score = item.CaptureModeDesc.SelectedDefaultRubicPoint != null ? item.CaptureModeDesc.SelectedDefaultRubicPoint : (item.CaptureModeDesc.SelectedInterviewContentRubicPoint != null ?
                                item.CaptureModeDesc.SelectedInterviewContentRubicPoint : (item.CaptureModeDesc.SelectedObservationContentRubicPoint != null ? item.CaptureModeDesc.SelectedObservationContentRubicPoint : item.CaptureModeDesc.SelectedStructuredContentRubicPoint));

                            RawScore += score.points;
                        }
                        if (firstItemScore == -1)
                        {
                            var firstSequenceItem = SequnetialScoredItems.FirstOrDefault();
                            var index = domainContentCollection.IndexOf(firstSequenceItem);
                            if (!RawScore.HasValue)
                            {
                                RawScore = 0;
                            }
                            RawScore += index * baselScore;
                        }
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawScoreEnabled = false;
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawScore = RawScore;
                        TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).rawscore = RawScore;
                        TotalDomainCollection.FirstOrDefault(p => p.DoaminCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BasalCeilingObtained = true;
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == firstitem.CaptureModeDesc.DomainCategoryId).BaselCeilingReached = true;
                        if (!InitialLoad && !baselceilingReached && BaselObtained && CeilingObtained)
                        {
                            CeilingObtainedPopupView();
                        }
                    }
                }
            }
        }
        public int? RawScore { get; set; }
        private string basalImage = "notStarted.png";
        public string BasalImage
        {
            get
            {
                return basalImage;
            }
            set
            {
                basalImage = value;
                OnPropertyChanged(nameof(BasalImage));
            }
        }
        private string ceilingImage = "notStarted.png";
        public string CeilingImage
        {
            get
            {
                return ceilingImage;
            }
            set
            {
                ceilingImage = value;
                OnPropertyChanged(nameof(CeilingImage));
            }
        }

        public void Dispose()
        {
            GC.Collect();
        }
        #endregion
    }
}
