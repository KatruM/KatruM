using Acr.UserDialogs;
using BDI3Mobile.Helpers;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.AcademicFolder;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using BDI3Mobile.Services.DigitalTestRecordService;
using BDI3Mobile.Views;
using BDI3Mobile.Views.ItemAdministrationView;
using BDI3Mobile.Views.ItemLevelNavigationPage;
using BDI3Mobile.Views.PopupViews;
using Newtonsoft.Json;
using PCLStorage;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


namespace BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel
{
    public class AcademicSurveyLiteracyViewModel : BaseclassViewModel, INotifyPropertyChanged
    {
        private TimerStopWatch _timer;
        private readonly IClinicalTestFormService clinicalTestFormService = DependencyService.Get<IClinicalTestFormService>();
        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
        private readonly IAssessmentsService _assessmentsService;
        private readonly IContentCategoryItemsService _contentCategoryItemsService;
        private readonly IContentCategoryService _contentcategoryservice;
        private readonly IContentCategoryLevelsService _contentCategoryLevelsService;
        private readonly IContentItemsService _contentItemsService;
        private readonly IContentItemAttributesService _contentItemAttributeService;
        private readonly IContentRubricsService _contentRubricsService;
        private readonly IContentRubricPointsService _contentRubicPointsService;
        private readonly IContentItemTallyService _contentItemTalliesService;
        private readonly IContentItemTalliesScoresService _contentItemTalliesScoresService;
        private readonly IContentGroupService _contentGroupService;
        private readonly IContentGroupItemsService _contentGroupItemsService;
        private readonly IContentBasalCeilingsService contentBasalCeilingsService;
        private readonly IStudentTestFormResponsesService _studentTestFormResponsesService;
        private readonly INavigationService _navigationService;
        private bool userTappedSave;

        #region TimerProperties
        private bool timerVisibilty;
        public bool TimerVisibilty
        {
            get { return timerVisibilty; }
            set { timerVisibilty = value; OnPropertyChanged(nameof(TimerVisibilty)); }
        }
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
        private int maxTime = 210;
        public int MaxTime
        {
            get { return maxTime; }
            set
            {
                maxTime = value;
                OnPropertyChanged(nameof(MaxTime));
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
        #endregion

        #region Template visibility properties
        private bool isItemInsScorMatGrid;
        public bool IsItemInsScorMatGrid
        {
            get { return isItemInsScorMatGrid; }
            set { isItemInsScorMatGrid = value; OnPropertyChanged(nameof(IsItemInsScorMatGrid)); }
        }
        private bool isImageSampleGrid;
        public bool IsImageSampleGrid
        {
            get { return isImageSampleGrid; }
            set { isImageSampleGrid = value; OnPropertyChanged(nameof(IsImageSampleGrid)); }
        }
        private bool isImageMaterialSampleGrid;
        public bool IsImageMaterialSampleGrid
        {
            get { return isImageMaterialSampleGrid; }
            set { isImageMaterialSampleGrid = value; OnPropertyChanged(nameof(IsImageMaterialSampleGrid)); }
        }
        private bool isInstructionItemsScoreGrid;
        public bool IsInstructionItemsScoreGrid
        {
            get { return isInstructionItemsScoreGrid; }
            set { isInstructionItemsScoreGrid = value; OnPropertyChanged(nameof(IsInstructionItemsScoreGrid)); }
        }
        private bool isScoreItemGrid;
        public bool IsScoreItemGrid
        {
            get { return isScoreItemGrid; }
            set { isScoreItemGrid = value; OnPropertyChanged(nameof(IsScoreItemGrid)); }
        }
        private bool isInstructionsImageItemsScoreGrid;
        public bool IsInstructionsImageItemsScoreGrid
        {
            get { return isInstructionsImageItemsScoreGrid; }
            set { isInstructionsImageItemsScoreGrid = value; OnPropertyChanged(nameof(IsInstructionsImageItemsScoreGrid)); }
        }
        private bool isImageItemScoreGrid;
        public bool IsImageItemScoreGrid
        {
            get { return isImageItemScoreGrid; }
            set { isImageItemScoreGrid = value; OnPropertyChanged(nameof(IsImageItemScoreGrid)); }
        }
        private bool isInstructionImageMaterialItemScoreGrid;
        public bool IsInstructionImageMaterialItemScoreGrid
        {
            get { return isInstructionImageMaterialItemScoreGrid; }
            set { isInstructionImageMaterialItemScoreGrid = value; OnPropertyChanged(nameof(IsInstructionImageMaterialItemScoreGrid)); }
        }
        #endregion

        private bool isInProgress;
        public bool IsInProgress
        {
            get { return isInProgress; }
            set { isInProgress = value; OnPropertyChanged(nameof(IsInProgress)); }
        }
        public bool ChildTapped { get; set; }
        private bool isnextVisible;
        public bool IsNextVisible
        {
            get
            {
                return isnextVisible;
            }
            set
            {
                isnextVisible = value;
                OnPropertyChanged(nameof(IsNextVisible));
            }
        }
        private bool ispreviousVisible;
        public bool IspreviousVisible
        {
            get
            {
                return ispreviousVisible;
            }
            set
            {
                ispreviousVisible = value;
                OnPropertyChanged(nameof(IspreviousVisible));
            }
        }
        private bool isScoringVisible = true;
        public bool IsScoringVisible
        {
            get { return isScoringVisible; }
            set { isScoringVisible = value; OnPropertyChanged(nameof(IsScoringVisible)); }
        }
        private bool isTallyVisible = true;
        public bool IsTallyVisible
        {
            get { return isTallyVisible; }
            set { isTallyVisible = value; OnPropertyChanged(nameof(IsTallyVisible)); }
        }
        private long fluencyItemDescriptionHeight;
        public long FluencyItemDescriptionHeight
        {
            get { return fluencyItemDescriptionHeight; }
            set
            {
                fluencyItemDescriptionHeight = value;
                OnPropertyChanged(nameof(FluencyItemDescriptionHeight));
            }

        }
        public int CurrentQuestion { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public bool isFirstRecord = false;

        public void SaveTestDate(string testdate)
        {
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any() && CurrentAcademicContentModel != null)
            {
                var categoryId = CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId;
                var testOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == categoryId);
                if (testOverview != null)
                {
                    HasDoneChanges = true;
                    testOverview.testDate = testdate;
                }
            }
        }
        public void Check30DayIssue()
        {
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
            {
                isFirstRecord = false;
                var testform = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId));
                if (testform != null && commonDataService.StudentTestForms.IndexOf(testform) == 0)
                {
                    isFirstRecord = true;
                }
                var selectedTestRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId));
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

        public bool popupOpenClicked { get; set; }
        public int NexttemCount { get; set; } = 0;
        public bool Row1IsSelected { get; set; }
        public bool Row2IsSelected { get; set; }
        public bool Row3IsSelected { get; set; }
        public int OfflineStudentID { get; set; }

        public int LocaInstanceID { get; set; }
        public int TotalAgeinMonths;
        public string DOB { get; set; }

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
        #region ItemswithMaterial&Instruction



        private List<AcademicItemModel> itemsCollection;

        public List<AcademicItemModel> ItemsCollection
        {
            get { return itemsCollection; }
            set { itemsCollection = value; OnPropertyChanged(nameof(ItemsCollection)); }
        }

        private List<AcademicScoringModel> scoringCollection;
        public List<AcademicScoringModel> ScoringCollection
        {
            get { return scoringCollection; }
            set { scoringCollection = value; OnPropertyChanged(nameof(ScoringCollection)); }
        }

        private List<ContentItemTally> tallyCollection;

        public List<ContentItemTally> TallyCollection
        {
            get { return tallyCollection; }
            set { tallyCollection = value; OnPropertyChanged(nameof(TallyCollection)); }
        }
        #endregion
        public Command OpenRecordToolCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ResetTimer(true);
                    await PopupNavigation.Instance.PushAsync(
                    new RecordToolsPOP(this));
                });
            }
        }
        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (userTappedSave)
                        return;
                    userTappedSave = true;
                    UserDialogs.Instance.ShowLoading("Saving Test Form...");
                    await Task.Delay(300);
                    TestFormSave();
                    HasDoneChanges = false;
                    UserDialogs.Instance.HideLoading();
                    await PopupNavigation.Instance.PushAsync(new FormSavePopUp() { BindingContext = this });
                    await Task.Delay(500);
                    userTappedSave = false;
                });
            }
        }
        public ICommand SearchErrorContinueCommand { get; set; }
        public ICommand ItemNavigationCommand { get; set; }
        public ICommand NextItemCommand { get; set; }
        public ICommand PreviousItemCommand { get; set; }
        public ICommand Row1TappedCommand { get; set; }
        public ICommand Row2TappedCommand { get; set; }
        public ICommand Row3TappedCommand { get; set; }

        public void TallyPointSelection(ContentItemTally tally, int pointsText)
        {
            var tallyPoint = TallyCollection.FirstOrDefault(p => p.contentItemTallyId == tally.contentItemTallyId);
            if (tallyPoint != null)
            {
                if (tallyPoint.IsCorrectChecked && pointsText == 1)
                {
                    tallyPoint.IsCorrectChecked = false;
                    tallyPoint.IsInCorrectChecked = false;
                }
                else if (tallyPoint.IsInCorrectChecked && pointsText == 0)
                {
                    tallyPoint.IsCorrectChecked = false;
                    tallyPoint.IsInCorrectChecked = false;
                }
                else if (tallyPoint.IsCorrectChecked && pointsText == 0)
                {
                    tallyPoint.IsCorrectChecked = false;
                    tallyPoint.IsInCorrectChecked = true;
                }
                else if (tallyPoint.IsInCorrectChecked && pointsText == 1)
                {
                    tallyPoint.IsCorrectChecked = true;
                    tallyPoint.IsInCorrectChecked = false;
                }
                else if (!tallyPoint.IsCorrectChecked && pointsText == 1)
                {
                    tallyPoint.IsCorrectChecked = true;
                    tallyPoint.IsInCorrectChecked = false;
                }
                else if (!tallyPoint.IsInCorrectChecked && pointsText == 0)
                {
                    tallyPoint.IsCorrectChecked = false;
                    tallyPoint.IsInCorrectChecked = true;
                }

                var selectedAcademicContentItem = default(List<AcademicContentModel>);
                if (CurrentAcademicContentModel.AreaId == 0)
                {
                    selectedAcademicContentItem = new List<AcademicContentModel>(AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == CurrentAcademicContentModel.SubDomainCategoryId));
                }
                else
                {
                    selectedAcademicContentItem = new List<AcademicContentModel>(AcademicContentModelCollection.Where(p => p.AreaId == CurrentAcademicContentModel.AreaId));
                }

                var tallySelected = false;
                if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                {
                    foreach (var item in selectedAcademicContentItem)
                    {
                        if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                        {
                            foreach (var academicitem in item.AcademicItemModel)
                            {
                                if (academicitem != null && academicitem.TallyContent != null && academicitem.TallyContent.Any())
                                {
                                    tallySelected = academicitem.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked);
                                    if (tallySelected)
                                    {
                                        break;
                                    }
                                }
                            }
                            if (tallySelected)
                            {
                                break;
                            }
                        }
                    }
                    foreach (var item in selectedAcademicContentItem)
                    {
                        item.rawScore = null;
                    }
                }
                var selectedSubDomain = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId));
                selectedSubDomain.rawScoreEnabled = !tallySelected;
                selectedSubDomain.IsScoreSelected = tallySelected;
                CalculateBaselCeiling();
            }
        }

        public void ContentRubicPointSelectionAction(AcademicScoringModel rubicPoint)
        {
            try
            {
                HasDoneChanges = true;
                ScoringCollection.Where(p => p.contentRubricPointsId != rubicPoint.contentRubricPointsId).ForEach(p => p.IsSelected = false);
                rubicPoint.IsSelected = !rubicPoint.IsSelected;
                var isBaselObtained = baselObtained;
                CheckScoreSelected();
                CalculateBaselCeiling();
                if (StartingPointContentCategory != null && ContentBasalCeilingsItems != null && ContentBasalCeilingsItems.Any())
                {
                    var category = ContentBasalCeilingsItems.FirstOrDefault(p => p.reverseContentCategoryId == StartingPointContentCategory.contentCategoryId);
                    if (category != null && !string.IsNullOrEmpty(category.reverseItem))
                    {
                        var reverseIds = category.reverseItem.Split(',');
                        if (reverseIds != null && reverseIds.Any())
                        {
                            var currentItem = ItemsCollection.FirstOrDefault(p => p.IsSelected);
                            if (currentItem != null)
                            {
                                if (currentItem.ScoringValues != null && currentItem.ScoringValues.Any())
                                {
                                    var selectedscore = currentItem.ScoringValues.Where(p => p.IsSelected);
                                    if (selectedscore != null && selectedscore.Any())
                                    {
                                        var splitted = currentItem.ItemTitle.Split(' ');
                                        if (splitted.Length > 1)
                                        {
                                            if (reverseIds.Contains(splitted[1]) && rubicPoint.points == 0 && !baselObtained && !ceilingObtained)
                                            {
                                                var selectedAcademicContentItem = default(IEnumerable<AcademicContentModel>);
                                                if (CurrentAcademicContentModel.AreaId == 0)
                                                {
                                                    selectedAcademicContentItem = AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == CurrentAcademicContentModel.SubDomainCategoryId);
                                                }
                                                else
                                                {
                                                    selectedAcademicContentItem = AcademicContentModelCollection.Where(p => p.AreaId == CurrentAcademicContentModel.AreaId);
                                                }
                                                if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                                                {
                                                    foreach (var item in selectedAcademicContentItem)
                                                    {
                                                        foreach (var contentitem in item.AcademicItemModel)
                                                        {
                                                            var itemtileSplit = contentitem.ItemTitle.Split(' ');
                                                            if (itemtileSplit.Length > 1 && itemtileSplit[1] == category.reverseStartItem)
                                                            {
                                                                InitializeDataSource(AcademicContentModelCollection.IndexOf(item), contentitem.ItemTitle);
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
                    }
                }
                if (CurrentAcademicContentModel != null && CurrentAcademicContentModel.DomainCategoryId == 198)
                {
                    if (LastModel == null)
                    {
                        foreach (var item in AcademicContentModelCollection)
                        {
                            if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                            {
                                foreach (var inneritem in item.AcademicItemModel)
                                {
                                    if (inneritem.ScoringValues != null && inneritem.ScoringValues.Any())
                                    {
                                        var score = inneritem.ScoringValues.FirstOrDefault(p => p.contentRubricId == rubicPoint.contentRubricId);
                                        if (score != null)
                                        {
                                            LastModel = inneritem;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (!BaselObtained)
                    {
                        if (rubicPoint.IsSelected && rubicPoint.points == 0)
                        {
                            if (CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any())
                            {
                                if (CurrentAcademicContentModel.AcademicItemModel.Count == 1)
                                {
                                    var moveQuestion = GetCurrentMovedQuestion(CurrentQuestion - 1);
                                    if (moveQuestion != -1)
                                    {
                                        LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                        InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                    }
                                }
                                else
                                {
                                    var indexScored = -1;
                                    for (int i = 0; i < CurrentAcademicContentModel.AcademicItemModel.Count; i++)
                                    {
                                        if (CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues != null && CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any())
                                        {
                                            var score = CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.FirstOrDefault(p => p.contentRubricId == rubicPoint.contentRubricId);
                                            if (score != null)
                                            {
                                                indexScored = i;
                                                break;
                                            }
                                        }
                                    }
                                    if (indexScored > 0)
                                    {
                                        var isFind = false;
                                        for (int i = indexScored - 1; i >= 0; i--)
                                        {
                                            if (CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues != null && CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any())
                                            {
                                                if (!CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any(p => p.IsSelected))
                                                {
                                                    LastModel = CurrentAcademicContentModel.AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                                    foreach (var item in CurrentAcademicContentModel.AcademicItemModel)
                                                    {
                                                        item.IsSelected = false;
                                                    }
                                                    CurrentAcademicContentModel.AcademicItemModel[i].IsSelected = true;
                                                    InitializeDataSource(AcademicContentModelCollection.IndexOf(CurrentAcademicContentModel), CurrentAcademicContentModel.AcademicItemModel[i].ItemTitle);
                                                    isFind = true;
                                                    break;
                                                }
                                            }
                                        }
                                        if (!isFind)
                                        {
                                            var moveQuestion = GetCurrentMovedQuestion(CurrentQuestion - 1);
                                            if (moveQuestion != -1)
                                            {
                                                LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                                InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var moveQuestion = GetCurrentMovedQuestion(CurrentQuestion - 1);
                                        if (moveQuestion != -1)
                                        {
                                            LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                            InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                        }
                                    }
                                }
                            }
                        }
                        else if (rubicPoint.IsSelected && rubicPoint.points == 1)
                        {
                            if (LastModel != null)
                            {
                                var lastmodelIndex = -1;
                                var found = false;
                                var lastmodelRubricPoint = LastModel.ScoringValues.FirstOrDefault(p => p.IsSelected);
                                foreach (var item in AcademicContentModelCollection)
                                {
                                    if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                                    {
                                        foreach (var inneritem in item.AcademicItemModel)
                                        {
                                            lastmodelIndex += 1;
                                            if (inneritem.ScoringValues != null && inneritem.ScoringValues.Any())
                                            {
                                                var score = inneritem.ScoringValues.FirstOrDefault(p => p.contentRubricId == lastmodelRubricPoint.contentRubricId);
                                                if (score != null)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (found)
                                    {
                                        break;
                                    }
                                }

                                var currentmodelIndex = -1;
                                found = false;
                                foreach (var item in AcademicContentModelCollection)
                                {
                                    if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                                    {
                                        foreach (var inneritem in item.AcademicItemModel)
                                        {
                                            currentmodelIndex += 1;
                                            if (inneritem.ScoringValues != null && inneritem.ScoringValues.Any())
                                            {
                                                var score = inneritem.ScoringValues.FirstOrDefault(p => p.contentRubricId == rubicPoint.contentRubricId);
                                                if (score != null)
                                                {
                                                    found = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (found)
                                    {
                                        break;
                                    }
                                }
                                if (currentmodelIndex < lastmodelIndex)
                                {
                                    if (CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any())
                                    {
                                        if (CurrentAcademicContentModel.AcademicItemModel.Count == 1)
                                        {
                                            var moveQuestion = GetCurrentMovedQuestion(CurrentQuestion - 1);
                                            if (moveQuestion != -1)
                                            {
                                                LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                                InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                            }
                                        }
                                        else
                                        {
                                            var indexScored = -1;
                                            for (int i = 0; i < CurrentAcademicContentModel.AcademicItemModel.Count; i++)
                                            {
                                                if (CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues != null && CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any())
                                                {
                                                    var score = CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.FirstOrDefault(p => p.contentRubricId == rubicPoint.contentRubricId);
                                                    if (score != null)
                                                    {
                                                        indexScored = i;
                                                        break;
                                                    }
                                                }
                                            }
                                            if (indexScored > 0)
                                            {
                                                for (int i = indexScored - 1; i >= 0; i--)
                                                {
                                                    if (CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues != null && CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any())
                                                    {
                                                        if (!CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any(p => p.IsSelected))
                                                        {
                                                            LastModel = CurrentAcademicContentModel.AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                                            foreach (var item in CurrentAcademicContentModel.AcademicItemModel)
                                                            {
                                                                item.IsSelected = false;
                                                            }
                                                            CurrentAcademicContentModel.AcademicItemModel[i].IsSelected = true;
                                                            InitializeDataSource(AcademicContentModelCollection.IndexOf(CurrentAcademicContentModel), CurrentAcademicContentModel.AcademicItemModel[i].ItemTitle);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                var moveQuestion = GetCurrentMovedQuestion(CurrentQuestion - 1);
                                                if (moveQuestion != -1)
                                                {
                                                    LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                                    InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (BaselObtained && !isBaselObtained && !ceilingObtained)
                    {
                        if (CurrentAcademicContentModel.AcademicItemModel.Count == 1)
                        {
                            var moveQuestion = GetForwardCurrentMovedQuestion(CurrentQuestion + 1);
                            if (moveQuestion != -1)
                            {
                                LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                            }
                        }
                        else
                        {
                            var indexScored = -1;
                            for (int i = 0; i < CurrentAcademicContentModel.AcademicItemModel.Count; i++)
                            {
                                if (CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues != null && CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any())
                                {
                                    var score = CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.FirstOrDefault(p => p.contentRubricId == rubicPoint.contentRubricId);
                                    if (score != null)
                                    {
                                        indexScored = i;
                                        break;
                                    }
                                }
                            }
                            if (indexScored >= 0)
                            {
                                var isFind = false;
                                for (int i = indexScored; i <= CurrentAcademicContentModel.AcademicItemModel.Count - 1; i++)
                                {
                                    if (CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues != null && CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any())
                                    {
                                        if (!CurrentAcademicContentModel.AcademicItemModel[i].ScoringValues.Any(p => p.IsSelected))
                                        {
                                            foreach (var item in CurrentAcademicContentModel.AcademicItemModel)
                                            {
                                                item.IsSelected = false;
                                            }
                                            CurrentAcademicContentModel.AcademicItemModel[i].IsSelected = true;
                                            LastModel = CurrentAcademicContentModel.AcademicItemModel[i];
                                            InitializeDataSource(AcademicContentModelCollection.IndexOf(CurrentAcademicContentModel), CurrentAcademicContentModel.AcademicItemModel[i].ItemTitle);
                                            isFind = true;
                                            break;
                                        }
                                    }
                                }
                                if (!isFind)
                                {
                                    var moveQuestion = GetForwardCurrentMovedQuestion(CurrentQuestion + 1);
                                    if (moveQuestion != -1)
                                    {
                                        LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                        InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                    }
                                }
                            }
                            else
                            {
                                var moveQuestion = GetForwardCurrentMovedQuestion(CurrentQuestion + 1);
                                if (moveQuestion != -1)
                                {
                                    LastModel = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected);
                                    InitializeDataSource(moveQuestion, AcademicContentModelCollection[moveQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ItemTitle);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public ICommand ContentRubicPointSelection
        {
            get
            {
                return new Command<AcademicScoringModel>((rubicPoint) =>
                {
                    ContentRubicPointSelectionAction(rubicPoint);
                });
            }
        }
        private int GetForwardCurrentMovedQuestion(int currentquestion)
        {
            if (currentquestion > AcademicContentModelCollection.Count)
            {
                return -1;
            }
            var model = AcademicContentModelCollection[currentquestion];
            if (model.AcademicItemModel != null && model.AcademicItemModel.Any())
            {
                if (model.DomainCategoryId != 198)
                {
                    return -1;
                }
                for (int i = 0; i < model.AcademicItemModel.Count; i++)
                {
                    if (model.AcademicItemModel[i].ScoringValues != null && model.AcademicItemModel[i].ScoringValues.Any())
                    {
                        var isScore = model.AcademicItemModel[i].ScoringValues.Any(p => p.IsSelected);
                        if (!isScore)
                        {
                            model.AcademicItemModel.ForEach((ITEM) =>
                            {
                                ITEM.IsSelected = false;
                            });
                            model.AcademicItemModel[i].IsSelected = true;
                            return currentquestion;
                        }
                    }
                }
                return GetForwardCurrentMovedQuestion(currentquestion + 1);
            }
            return -1;
        }
        private int GetCurrentMovedQuestion(int currentquestion)
        {
            if (currentquestion == -1)
            {
                return -1;
            }
            var model = AcademicContentModelCollection[currentquestion];
            if (model.AcademicItemModel != null && model.AcademicItemModel.Any())
            {
                if (model.DomainCategoryId != 198)
                {
                    return -1;
                }
                for (int i = model.AcademicItemModel.Count - 1; i >= 0; i--)
                {
                    if (model.AcademicItemModel[i].ScoringValues != null && model.AcademicItemModel[i].ScoringValues.Any())
                    {
                        var isScore = model.AcademicItemModel[i].ScoringValues.Any(p => p.IsSelected);
                        if (!isScore)
                        {
                            model.AcademicItemModel.ForEach((ITEM) =>
                            {
                                ITEM.IsSelected = false;
                            });
                            model.AcademicItemModel[i].IsSelected = true;
                            return currentquestion;
                        }
                    }
                }
                return GetCurrentMovedQuestion(currentquestion - 1);
            }
            return -1;
        }
        AcademicItemModel LastModel;
        public void ContentRubric(ContentRubricPoint rubicPoint)
        {
            //RowContentTallyPointCollection.ForEach(p => p.IsSelected = false);
            //rubicPoint.IsSelected = true;
        }

        #region BindingPropertiess
        private string assessmentType;
        public string AssessmentType
        {
            get { return assessmentType; }
            set { assessmentType = value; OnPropertyChanged(nameof(AssessmentType)); }
        }

        private string instructionHeader;
        public string InstructionHeader
        {
            get { return instructionHeader; }
            set { instructionHeader = value; OnPropertyChanged(nameof(InstructionHeader)); }
        }


        private string imagefilepath;
        public string ImageFilePath
        {
            get
            {
                return imagefilepath;
            }
            set
            {
                imagefilepath = value;
                OnPropertyChanged(nameof(ImageFilePath));
            }
        }
        private string instructionImageScoreDescFilePath;
        public string InstructionImageScoreDescFilePath
        {
            get
            {
                return instructionImageScoreDescFilePath;
            }
            set
            {
                instructionImageScoreDescFilePath = value;
                OnPropertyChanged(nameof(InstructionImageScoreDescFilePath));
            }
        }

        private string instructionImageScoreDescription;
        public string InstructionImageScoreDescription
        {
            get
            {
                return instructionImageScoreDescription;
            }
            set
            {
                instructionImageScoreDescription = value;
                OnPropertyChanged(nameof(InstructionImageScoreDescription));
            }
        }

        private string imageSampleGridDescFilePath;
        public string ImageSampleGridDescFilePath
        {
            get
            {
                return imageSampleGridDescFilePath;
            }
            set
            {
                imageSampleGridDescFilePath = value;
                OnPropertyChanged(nameof(ImageSampleGridDescFilePath));
            }
        }

        private string imageSampleGridDescription;
        public string ImageSampleGridDescription
        {
            get
            {
                return imageSampleGridDescription;
            }
            set
            {
                imageSampleGridDescription = value;
                OnPropertyChanged(nameof(ImageSampleGridDescription));
            }
        }


        private string instructionDescFilePath;
        public string InstructionDescFilePath
        {
            get
            {
                return instructionDescFilePath;
            }
            set
            {
                instructionDescFilePath = value;
                OnPropertyChanged(nameof(InstructionDescFilePath));
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string descriptionFilePath;
        public string DescriptionFilePath
        {
            get { return descriptionFilePath; }
            set
            {
                descriptionFilePath = value;
                OnPropertyChanged(nameof(DescriptionFilePath));
            }
        }

        private string itemdescription;
        public string ItemDescription
        {
            get { return itemdescription; }
            set
            {
                itemdescription = value;
                OnPropertyChanged(nameof(ItemDescription));
            }
        }


        private string notesDescriptionPath;
        public string NotesDescriptionPath
        {
            get { return notesDescriptionPath; }
            set
            {
                notesDescriptionPath = value;
                OnPropertyChanged(nameof(NotesDescriptionPath));
            }
        }
        private string notesDescription;
        public string NotesDescription
        {
            get { return notesDescription; }
            set
            {
                notesDescription = value;
                OnPropertyChanged(nameof(NotesDescription));
            }
        }

        private bool isScoringNotesVisible;
        public bool IsScoringNotesVisible
        {
            get { return isScoringNotesVisible; }
            set { isScoringNotesVisible = value; OnPropertyChanged(nameof(IsScoringNotesVisible)); }
        }
        private string scoringNotesDescription;
        public string ScoringNotesDescription
        {
            get { return scoringNotesDescription; }
            set
            {
                scoringNotesDescription = value;
                OnPropertyChanged(nameof(ScoringNotesDescription));
            }
        }
        private string itemdescriptionFilePath;
        public string ItemdescriptionFilePath
        {
            get { return itemdescriptionFilePath; }
            set
            {
                itemdescriptionFilePath = value;
                if (TemplateModel != null)
                {
                    TemplateModel.ItemdescriptionFilePath = value;
                }

                OnPropertyChanged(nameof(ItemdescriptionFilePath));
            }
        }
        private int itemNumber;
        public int ItemNumber
        {
            get
            {
                return itemNumber;
            }
            set
            {
                itemNumber = value;
                OnPropertyChanged(nameof(ItemNumber));
            }
        }

        private string materialFilePath;
        public string MaterialFilePath
        {
            get { return materialFilePath; }
            set { materialFilePath = value; OnPropertyChanged(nameof(MaterialFilePath)); }
        }

        private string materialText;
        public string MaterialText
        {
            get { return materialText; }
            set
            {
                //if (value != null && value.ToString().ToLower().Contains("timing device") && CurrentAcademicContentModel.GroupTitle != "FLU S")
                if (CurrentAcademicContentModel.SubDomainCode == "FLU" && CurrentAcademicContentModel.GroupTitle != "FLU S")
                {
                    TimerVisibilty = true;
                    TimerSepertorWidth = new GridLength(1, GridUnitType.Absolute);
                    TimerWidth = new GridLength(.8, GridUnitType.Star);
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
        private string rowHeader1;
        public string RowHeader1
        {
            get { return rowHeader1; }
            set { rowHeader1 = value; OnPropertyChanged(nameof(RowHeader1)); }
        }

        private string rowHeader2;
        public string RowHeader2
        {
            get { return rowHeader2; }
            set { rowHeader2 = value; OnPropertyChanged(nameof(RowHeader2)); }
        }

        private string rowHeader3;
        public string RowHeader3
        {
            get { return rowHeader3; }
            set { rowHeader3 = value; OnPropertyChanged(nameof(RowHeader3)); }
        }

        private Color row1Bckgrd;
        public Color Row1Bckgrd
        {
            get { return row1Bckgrd; }
            set { row1Bckgrd = value; OnPropertyChanged(nameof(Row1Bckgrd)); }
        }

        private Color row1BoxColor;
        public Color Row1BoxColor
        {
            get { return row1BoxColor; }
            set { row1BoxColor = value; OnPropertyChanged(nameof(Row1BoxColor)); }
        }

        private Color row1TextColor;
        public Color Row1TextColor
        {
            get { return row1TextColor; }
            set { row1TextColor = value; OnPropertyChanged(nameof(Row1TextColor)); }
        }

        private Color row2Bckgrd;
        public Color Row2Bckgrd
        {
            get { return row2Bckgrd; }
            set { row2Bckgrd = value; OnPropertyChanged(nameof(Row2Bckgrd)); }
        }

        private Color row2BoxColor;
        public Color Row2BoxColor
        {
            get { return row2BoxColor; }
            set { row2BoxColor = value; OnPropertyChanged(nameof(Row2BoxColor)); }
        }

        private Color row2TextColor;
        public Color Row2TextColor
        {
            get { return row2TextColor; }
            set { row2TextColor = value; OnPropertyChanged(nameof(Row2TextColor)); }
        }

        private Color row3Bckgrd;
        public Color Row3Bckgrd
        {
            get { return row3Bckgrd; }
            set { row3Bckgrd = value; OnPropertyChanged(nameof(Row3Bckgrd)); }
        }

        private Color row3BoxColor;
        public Color Row3BoxColor
        {
            get { return row3BoxColor; }
            set { row3BoxColor = value; OnPropertyChanged(nameof(Row3BoxColor)); }
        }

        private Color row3TextColor;
        public Color Row3TextColor
        {
            get { return row3TextColor; }
            set { row3TextColor = value; OnPropertyChanged(nameof(Row3TextColor)); }
        }

        private FontAttributes row1FontAttribute;
        public FontAttributes Row1FontAttribute
        {
            get { return row1FontAttribute; }
            set { row1FontAttribute = value; OnPropertyChanged(nameof(Row1FontAttribute)); }
        }

        private FontAttributes row2FontAttribute;
        public FontAttributes Row2FontAttribute
        {
            get { return row2FontAttribute; }
            set { row2FontAttribute = value; OnPropertyChanged(nameof(Row2FontAttribute)); }
        }

        private FontAttributes row3FontAttribute;
        public FontAttributes Row3FontAttribute
        {
            get { return row3FontAttribute; }
            set { row3FontAttribute = value; OnPropertyChanged(nameof(Row3FontAttribute)); }
        }
        #endregion
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
        #region Commands
        public StudentTestFormOverview OriginalStudentTestFormOverView { get; set; }
        public List<StudentTestForms> OriginalStudentTestForms { get; set; }
        public Action ResetWebViews { get; set; }
        public Command BackCommand
        {
            get
            {
                return new Command(async () =>
                {
                    ResetTimer(true);
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

                                    if (itemdateTime.Date != itemtestdateTime || newRecord.examinerId != item.examinerId || newRecord.rawScore != item.rawScore)
                                    {
                                        HasDoneChanges = true;
                                    }

                                    if (newRecord.contentCategoryId == 162 && (newRecord.TimeTaken != item.TimeTaken))
                                    {
                                        HasDoneChanges = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!HasDoneChanges)
                    {
                        var originalFormParameters = JsonConvert.DeserializeObject<FormParamterClass>(OriginalStudentTestFormOverView.formParameters);
                        var updatedFromParameters = JsonConvert.DeserializeObject<FormParamterClass>(commonDataService.StudentTestFormOverview.formParameters);
                        if (originalFormParameters.AllStandard != updatedFromParameters.AllStandard)
                        {
                            HasDoneChanges = true;
                        }
                        if (!HasDoneChanges && originalFormParameters.GlassesUsed != updatedFromParameters.GlassesUsed)
                        {
                            HasDoneChanges = true;
                        }
                        if (!HasDoneChanges && originalFormParameters.HasGlasses != updatedFromParameters.HasGlasses)
                        {
                            HasDoneChanges = true;
                        }
                        if (!HasDoneChanges && originalFormParameters.HasHearingAid != updatedFromParameters.HasHearingAid)
                        {
                            HasDoneChanges = true;
                        }
                        if (!HasDoneChanges && originalFormParameters.HearingAidUsed != updatedFromParameters.HearingAidUsed)
                        {
                            HasDoneChanges = true;
                        }
                        if (!HasDoneChanges && originalFormParameters.ValidRepresentation != updatedFromParameters.ValidRepresentation)
                        {
                            HasDoneChanges = true;
                        }
                    }
                    if (HasDoneChanges)
                    {
                        await PopupNavigation.Instance.PushAsync(new SavePopUp() { BindingContext = this });
                    }
                    else
                    {
                        _timer = null;
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "AcademicPage");
                        if (ChildTapped)
                        {
                            App.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                        }
                        else
                        {
                            MenuList = null; TotalMenuList = null;
                            AcademicContentModelCollection = null; CurrentAcademicContentModel = null; ScoringCollection = null;
                            ContentCategoryLevels = null; ContentItem = null;
                            ContentCategory = null; ContentCategoryItems = null; ContentItemAttributes = null;
                            ContentItemAttributes = null; ContentRubrics = null; ContentRubricPoints = null; ContentItemTallyScores = null;
                            ContentItemTally = null; ContentBasalCeilingsItems = null;

                            _navigationService.ClearModalStack();
                            App.Current.MainPage = new Views.DashboardHomeView();
                        }
                    }
                });
            }
        }
        public ICommand UnSaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    if (ChildTapped)
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "AcademicPage");
                        App.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                    }
                    else
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "AcademicPage");
                        _navigationService.ClearModalStack();
                        App.Current.MainPage = new Views.DashboardHomeView();
                    }
                });
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

                    if (ChildTapped)
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "AcademicPage");
                        App.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                    }
                    else
                    {
                        MessagingCenter.Unsubscribe<ItemNavigationHeaderData>(this, "AcademicPage");
                        _navigationService.ClearModalStack();
                        App.Current.MainPage = new Views.DashboardHomeView();
                    }
                });
            }
        }

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
                    UserDialogs.Instance.HideLoading();
                    await PopupNavigation.Instance.PushAsync(new AcademicTestSessionOverview(), false);
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
        public ICommand ContinueToNextTestCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAllAsync();
                    var findModel = AcademicContentModelCollection.FirstOrDefault(p => p.GroupTitle == "NCS 1-3");
                    CurrentQuestion = AcademicContentModelCollection.IndexOf(findModel);
                    SetData();
                });
            }
        }
        #endregion
        private readonly IStudentTestFormsService studentTestFormsService;
        public AcademicSurveyLiteracyViewModel(int loaclInstanceID = 0, string dob = null, string testdate = null)
        {
            DOB = dob;
            TestDate = testdate;
            LocaInstanceID = loaclInstanceID;
            studentTestFormsService = DependencyService.Get<IStudentTestFormsService>();
            contentBasalCeilingsService = DependencyService.Get<IContentBasalCeilingsService>();
            _assessmentsService = DependencyService.Get<IAssessmentsService>();
            _contentCategoryItemsService = DependencyService.Get<IContentCategoryItemsService>();
            _contentcategoryservice = DependencyService.Get<IContentCategoryService>();
            _contentCategoryLevelsService = DependencyService.Get<IContentCategoryLevelsService>();
            _contentItemsService = DependencyService.Get<IContentItemsService>();
            _contentItemAttributeService = DependencyService.Get<IContentItemAttributesService>();
            _contentRubricsService = DependencyService.Get<IContentRubricsService>();
            _contentRubicPointsService = DependencyService.Get<IContentRubricPointsService>();
            _contentGroupService = DependencyService.Get<IContentGroupService>();
            _contentGroupItemsService = DependencyService.Get<IContentGroupItemsService>();
            _contentItemTalliesService = DependencyService.Get<IContentItemTallyService>();
            _contentItemTalliesScoresService = DependencyService.Get<IContentItemTalliesScoresService>();
            _navigationService = DependencyService.Get<INavigationService>();
            _studentTestFormResponsesService = DependencyService.Get<IStudentTestFormResponsesService>();

            ScoringCollection = new List<AcademicScoringModel>();
            //ItemContent = ItemsCollection[0].ItemContent;


            StartCommand = new Command(StartTimerCommand);
            StopCommand = new Command(StopTimerCommand);
            //make sure you put this in the constructor
            _timer = new TimerStopWatch(TimeSpan.FromSeconds(1), CountDown);
            TotalSeconds = _totalSeconds;
            if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any())
            {
                var fluency = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == 162);
                if (fluency != null && fluency.TimeTaken.HasValue)
                {
                    TotalSeconds = new TimeSpan(0, 0, fluency.TimeTaken.Value);
                }
            }
            if (TotalSeconds.TotalSeconds == MaxTime)
            {
                IsTimerInProgress = false;
                TimerStatusText = "Start";
                StartEnabled = false;
                TimerButtonBckgrd = Color.LightGray;
                TimerReset = "iconrefreshblue.png";
                ResetEnabled = true;
            }

            InfoIconNavigationCommand = new Command(NavigateToInformationPage);
            ItemLevelCloseCommand = new Command(CloseTreeView);
            SearchErrorContinueCommand = new Xamarin.Forms.Command(SearchErrorContinueClicked);
            ItemNavigationCommand = new Command(OpenItemLevelNavigation);
            NextItemCommand = new Command(execute: () => { NextItemClicked(); },
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

            CalculateAgeDiff();
            MessagingCenter.Subscribe<ItemNavigationHeaderData>(this, "AcademicPage", async (value) =>
            {
                UserDialogs.Instance.ShowLoading("Loading");
                string academicHeader = value.Title;
                if (string.IsNullOrEmpty(academicHeader))
                    return;
                await PopupNavigation.Instance.PopAsync();
                int startindexof = academicHeader.IndexOf('(');
                int Endindexof = academicHeader.IndexOf(')');
                string domains = string.Empty;
                domains = academicHeader.Substring(startindexof + 1, Endindexof - startindexof - 1);
                var categories = commonDataService.TotalCategories.Where(p => (p.contentCategoryLevelId == 7 || p.contentCategoryLevelId == 8) && p.code == domains).ToList();
                if (categories != null && categories.Any())
                {
                    var startingPointCategory = ContentCategory.Where(p => p.contentCategoryLevelId == 9 && p.parentContentCategoryId == categories.FirstOrDefault().contentCategoryId).FirstOrDefault(p => (TotalAgeinMonths <= p.highAge && TotalAgeinMonths >= p.lowAge));
                    if (startingPointCategory == null)
                    {
                        StartingPointContentCategory = null;
                        startingPointCategory = commonDataService.TotalCategories.Where(p => p.parentContentCategoryId == categories.FirstOrDefault().contentCategoryId).OrderBy(p => p.contentCategoryId).FirstOrDefault();
                    }
                    else
                    {
                        StartingPointContentCategory = startingPointCategory;
                    }
                    if (startingPointCategory != null)
                    {
                        var parentCategory = commonDataService.TotalCategories.FirstOrDefault(p => p.contentCategoryId == startingPointCategory.parentContentCategoryId);
                        var contentCategoryItems = _contentCategoryItemsService.GetItems();
                        var splittedcode = parentCategory != null ? parentCategory.code.Split(' ').FirstOrDefault() : domains;
                        var contentItem = contentCategoryItems.Where(p => p.contentCategoryId == startingPointCategory.contentCategoryId);
                        if (contentItem != null && contentItem.Any())
                        {
                            var startingContentitem = commonDataService.ContentItems.Where(p => contentItem.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).FirstOrDefault();
                            if (startingContentitem != null)
                            {
                                var findModel = AcademicContentModelCollection.FirstOrDefault(p => p.AcademicItemModel != null && p.AcademicItemModel.Any(q => q.ContentItemId == startingContentitem.contentItemId));
                                if (findModel != null)
                                {
                                    findModel.AcademicItemModel?.ForEach(p => p.IsSelected = false);
                                    CurrentQuestion = AcademicContentModelCollection.IndexOf(findModel);
                                    SetData();
                                    ItemDescription = findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId).Description;
                                    ScoringCollection = findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId).ScoringValues != null
                                        && findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId).ScoringValues.Any() ? findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId).ScoringValues : new List<AcademicScoringModel>();

                                    ImageFilePath = findModel.ImageCotentCollection != null && findModel.ImageCotentCollection.Any() ?
                                        findModel.ImageCotentCollection.FirstOrDefault(p => p.ContentItemId == startingContentitem.contentItemId)?.ImageStr : null;

                                }
                                else
                                {
                                    var academcModel = AcademicContentModelCollection.FirstOrDefault(p => p.SubDomainCode == domains || p.AreaCode == domains);
                                    if (academcModel != null)
                                    {
                                        academcModel.AcademicItemModel?.ForEach(p => p.IsSelected = false);
                                        CurrentQuestion = AcademicContentModelCollection.IndexOf(academcModel);
                                    }
                                    SetData();
                                }
                            }
                            else
                            {
                                var academcModel = AcademicContentModelCollection.FirstOrDefault(p => p.SubDomainCode == domains || p.AreaCode == domains);
                                if (academcModel != null)
                                {
                                    academcModel.AcademicItemModel?.ForEach(p => p.IsSelected = false);
                                    CurrentQuestion = AcademicContentModelCollection.IndexOf(academcModel);
                                }
                                SetData();
                            }
                            if (AcademicContentModelCollection.Count == 0 || AcademicContentModelCollection.Count == 1)
                            {
                                IsNextVisible = false;
                                IspreviousVisible = false;
                            }
                            else
                            {
                                IspreviousVisible = CurrentQuestion > 0;
                                IsNextVisible = CurrentQuestion < AcademicContentModelCollection.Count - 1;
                            }
                        }
                    }
                }
                CheckScoreSelected();
                InitialLoad = true;
                CalculateBaselCeiling();
                InitialLoad = false;
                if (LastModel != null)
                {
                    var breaknow = false;
                    foreach (var item in AcademicContentModelCollection)
                    {
                        if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                        {
                            foreach (var inneritem in item.AcademicItemModel)
                            {
                                if (LastModel != null && inneritem.ContentItemId == LastModel.ContentItemId)
                                {
                                    if (item.DomainCategoryId != CurrentAcademicContentModel.DomainCategoryId)
                                    {
                                        LastModel = null;
                                        breaknow = true;
                                        break;
                                    }
                                }
                            }
                        }
                        if (breaknow)
                        {
                            break;
                        }
                    }
                }
                UserDialogs.Instance.HideLoading();
            });
            commonDataService.ResetTestDate = new Action(() =>
            {
                if (commonDataService.StudentTestForms != null && commonDataService.StudentTestForms.Any() && CurrentAcademicContentModel != null)
                {
                    var categoryId = CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId;
                    var testOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == categoryId);
                    if (testOverview != null)
                    {
                        TestDate = testOverview.testDate;
                    }
                    if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                    {
                        foreach (var item in AcademicContentModelCollection)
                        {
                            var form = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item.SubDomainCategoryId || p.contentCategoryId == item.AreaId);
                            if (form != null)
                            {
                                item.rawScore = form.rawScore;
                            }
                        }
                    }

                    var fluencytestOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == 162);
                    if (fluencytestOverview != null)
                    {
                        if (fluencytestOverview.TimeTaken.HasValue)
                        {
                            TotalSeconds = TimeSpan.FromSeconds(fluencytestOverview.TimeTaken.Value);
                        }
                        else
                        {
                            TotalSeconds = TimeSpan.FromSeconds(0);
                        }
                        TimerReset = "iconrefreshblue.png";
                        ResetEnabled = true;
                        if (TotalSeconds.TotalSeconds == MaxTime)
                        {
                            IsTimerInProgress = false;
                            TimerStatusText = "Start";
                            StartEnabled = false;
                            TimerButtonBckgrd = Color.LightGray;

                            TimerReset = "iconrefreshblue.png";
                            ResetEnabled = true;
                        }
                    }
                }
            });

            commonDataService.ObservationNotes = new Action<string>((param) =>
            {
                FormNotes = param;
            });
        }
        public async void OpenItemLevelNavigation()
        {
            ResetTimer(true);
            if (popupOpenClicked)
                return;
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack)
                {
                    if (popup is ItemLevelNavigationPage && (popup as ItemLevelNavigationPage).Title == "ItemLevelNaviagationTitle")
                    {
                        return;
                    }
                }
            }

            popupOpenClicked = true;
            await GenerateMenuItems();
            await PopupNavigation.Instance.PushAsync(new AcademicMenuNavigation() { BindingContext = this, Title = "ItemLevelNaviagationTitle" });
            popupOpenClicked = false;
        }
        public async void SearchErrorContinueClicked()
        {
            await PopupNavigation.Instance.PopAsync();
        }

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

            if (TemplateModel != null)
            {
                TemplateModel.TimerStatusText = TimerStatusText;
                TemplateModel.TimerButtonBckgrd = Color.FromHex("#478128");
                TemplateModel.TimerReset = TimerReset;
                TemplateModel.ResetEnabled = ResetEnabled;
            }
        }

        internal void StopTimerWithFluencyItem9()
        {
            if (IsTimerInProgress)
            {
                TimerReset = "iconrefreshblue.png";
                ResetEnabled = true;
                TimerStatusText = "Start";
                TimerButtonBckgrd = Color.FromHex("#478128");
                IsTimerInProgress = false;
                _timer.Stop();

                if (TemplateModel != null)
                {
                    TemplateModel.TimerStatusText = TimerStatusText;
                    TemplateModel.IsTimerInProgress = IsTimerInProgress;
                    TemplateModel.TimerButtonBckgrd = TimerButtonBckgrd;
                    TemplateModel.TimerReset = TimerReset;
                    TemplateModel.ResetEnabled = ResetEnabled;
                }
            }
        }

        /// <summary>
        /// Stops and resets the timer.
        /// </summary>
        private void StopTimerCommand()
        {
            StartEnabled = true;
            IsTimerInProgress = false;
            TimerButtonBckgrd = Color.FromHex("#478128");
            TotalSeconds = new TimeSpan(0, 0, 0, 0);
            commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == 162).TimeTaken = 0;
            TimerReset = "iconrefreshgray.png";
            ResetEnabled = false;
            _timer.Stop();

            if (TemplateModel != null)
            {
                TemplateModel.StartEnabled = StartEnabled;
                TemplateModel.IsTimerInProgress = IsTimerInProgress;
                TemplateModel.TimerButtonBckgrd = TimerButtonBckgrd;
                TemplateModel.TimerReset = TimerReset;
                TemplateModel.ResetEnabled = ResetEnabled;
                TemplateModel.TotalSeconds = TotalSeconds;
            }
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
                    if (popup is FluencyTimerLimitPopUp && (popup as FluencyTimerLimitPopUp).Title == "FluencyTimerLimitPopUp")
                    {
                        return;
                    }
                }
            }
            if (TotalSeconds.TotalSeconds >= this.MaxTime && IsTimerInProgress)
            {
                //StopTimerCommand();
                _timer.Stop();
                IsTimerInProgress = false;
                TimerStatusText = "Start";
                StartEnabled = false;
                TimerButtonBckgrd = Color.LightGray;
                //TimerButtonBckgrd = Color.FromHex("#478128");
                await PopupNavigation.Instance.PushAsync(new FluencyTimerLimitPopUp() { BindingContext = this });
            }
            else
            {
                IsTimerInProgress = true;
                TotalSeconds = _totalSeconds.Add(new TimeSpan(0, 0, 0, 1));
                commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == 162).TimeTaken = Convert.ToInt32(TotalSeconds.TotalSeconds);
            }

            if (TemplateModel != null)
            {
                TemplateModel.StartEnabled = StartEnabled;
                TemplateModel.IsTimerInProgress = IsTimerInProgress;
                TemplateModel.TimerButtonBckgrd = TimerButtonBckgrd;
                TemplateModel.TimerReset = TimerReset;
                TemplateModel.ResetEnabled = ResetEnabled;
                TemplateModel.TotalSeconds = TotalSeconds;
            }
        }
        #endregion
        #region TSONavigation
        public bool isTestRecordNavigationOpen { get; set; }
        public ICommand InfoIconNavigationCommand { get; set; }
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
        public async void CloseTreeView()
        {
            var popNavigationInstance = PopupNavigation.Instance;
            await popNavigationInstance.PopAsync();
        }

        public ICommand ItemLevelCloseCommand { get; set; }
        public CurrentAcademiTemplate CurrentAcademiTemplate { get; set; }
        public AcademicContentModel CurrentAcademicContentModel { get; set; }
        #endregion

        # region Menu
        private List<MenuContentModel> TotalMenuList;
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
        public async void AddorRemoveItems(MenuContentModel model)
        {
            model.ImageName = "menudownarrow.png";
            if (model.ContentLevelID == 6)
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
            else if (model.ContentLevelID == 7 || model.ContentLevelID == 8)
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
                                for (int j = 0; j < submenuItems.Count(); j++)
                                {
                                    MenuList.Remove(submenuItems.ElementAt(j));
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
                UserDialogs.Instance.ShowLoading("Loading");
                await Task.Delay(1000);
                if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                {
                    var findModel = AcademicContentModelCollection.FirstOrDefault(p => p.AcademicItemModel != null && p.AcademicItemModel.Any(q => q.ContentItemId == model.ContentItemId));
                    if (findModel != null)
                    {
                        StartingPointContentCategory = null;
                        var parentCategoryId = findModel.AreaId == 0 ? findModel.SubDomainCategoryId : findModel.AreaId;
                        var startingPointCatgory = ContentCategory.Where(p => p.contentCategoryLevelId == 9 && p.parentContentCategoryId == parentCategoryId).FirstOrDefault(p => (TotalAgeinMonths <= p.highAge && TotalAgeinMonths >= p.lowAge));
                        if (startingPointCatgory != null)
                        {
                            StartingPointContentCategory = startingPointCatgory;
                        }
                        CurrentQuestion = AcademicContentModelCollection.IndexOf(findModel);
                        await PopupNavigation.Instance.PopAsync();
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(600);
                        SetData(model.Code);
                        await Task.Delay(600);
                        UserDialogs.Instance.HideLoading();
                        ItemDescription = findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == model.ContentItemId).Description;
                        ScoringCollection = findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == model.ContentItemId).ScoringValues != null
                            && findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == model.ContentItemId).ScoringValues.Any() ? findModel.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == model.ContentItemId).ScoringValues : new List<AcademicScoringModel>();


                        ImageFilePath = findModel.ImageCotentCollection != null && findModel.ImageCotentCollection.Any() ?
                            findModel.ImageCotentCollection.FirstOrDefault(p => p.ContentItemId == model.ContentItemId)?.ImageStr : null;

                        if (ItemsCollection != null && ItemsCollection.Any())
                        {
                            ItemsCollection.ForEach(p => p.IsSelected = false);
                            var itemModel = ItemsCollection.FirstOrDefault(p => p.ContentItemId == model.ContentItemId);
                            if (itemModel != null)
                            {
                                itemModel.IsSelected = true;
                            }
                            //await Task.Delay(1000);
                        }
                        if (CurrentAcademicContentModel != null && CurrentAcademicContentModel.ImageCotentCollection != null && CurrentAcademicContentModel.ImageCotentCollection.Any())
                        {
                            ImageFilePath = CurrentAcademicContentModel.ImageCotentCollection.FirstOrDefault(p => p.ContentItemId == model.ContentItemId)?.ImageStr;
                        }
                        if (AcademicContentModelCollection.Count == 0 || AcademicContentModelCollection.Count == 1)
                        {
                            IsNextVisible = false;
                            IspreviousVisible = false;
                        }
                        else
                        {
                            IspreviousVisible = CurrentQuestion > 0;
                            IsNextVisible = CurrentQuestion < AcademicContentModelCollection.Count - 1;
                        }
                        CheckScoreSelected();
                        InitialLoad = true;
                        CalculateBaselCeiling();
                        InitialLoad = false;
                        if (LastModel != null)
                        {
                            foreach (var item in AcademicContentModelCollection)
                            {
                                if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                                {
                                    foreach (var inneritem in item.AcademicItemModel)
                                    {
                                        if (LastModel != null && inneritem.ContentItemId == LastModel.ContentItemId)
                                        {
                                            if (item.DomainCategoryId != CurrentAcademicContentModel.DomainCategoryId)
                                            {
                                                LastModel = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (PopupNavigation.Instance.PopupStack.Count > 0)
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                Device.BeginInvokeOnMainThread(() => { UserDialogs.Instance.HideLoading(); });
            }
            model.IsVisible = !model.IsVisible;
        }

        private async Task GenerateMenuItems()
        {
            await Task.Delay(0);
            if (TotalMenuList != null && TotalMenuList.Any())
            {
                foreach (var item in TotalMenuList)
                {
                    if (item.ContentLevelID == 7)
                    {
                        var startingPointCategory = commonDataService.TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
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
                    else if (item.ContentLevelID == 8)
                    {
                        var startingPointCategory = commonDataService.TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
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
                    if (item.ContentLevelID == 7)
                    {
                        var startingPointCategory = commonDataService.TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
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
                                                    if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                    {
                                                        foreach (var domaincollection in AcademicContentModelCollection)
                                                        {
                                                            if (domaincollection.AcademicItemModel != null && domaincollection.AcademicItemModel.Any())
                                                            {
                                                                var contentItemTest = domaincollection.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == TotalMenuList[i].ContentItemId);
                                                                if (contentItemTest != null)
                                                                {
                                                                    if (contentItemTest.ScoringValues != null && (contentItemTest.ScoringValues.Any(p => p.IsSelected)))
                                                                    {
                                                                        item.ProgressImage = "notStarted.png";
                                                                        for (int j = i; j <= startPointContentItemPosition - 1; j++)
                                                                        {
                                                                            TotalMenuList[j].ProgressImage = "notStarted.png";
                                                                        }
                                                                        find = true;
                                                                        break;
                                                                    }
                                                                    else if (contentItemTest.TallyContent != null && (contentItemTest.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked)))
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
                    else if (item.ContentLevelID == 8)
                    {
                        var startingPointCategory = commonDataService.TotalCategories.Where(p => p.parentContentCategoryId == item.ContentCatgoryId && TotalAgeinMonths >= p.lowAge && TotalAgeinMonths <= p.highAge).OrderBy(p => p.contentCategoryId).FirstOrDefault();
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
                                                    if (TotalMenuList[i].ParentID == item.ContentCatgoryId)
                                                    {
                                                        foreach (var domaincollection in AcademicContentModelCollection)
                                                        {
                                                            if (domaincollection.AcademicItemModel != null && domaincollection.AcademicItemModel.Any())
                                                            {
                                                                var contentItemTest = domaincollection.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == TotalMenuList[i].ContentItemId);
                                                                if (contentItemTest != null)
                                                                {
                                                                    if (contentItemTest.ScoringValues != null && (contentItemTest.ScoringValues.Any(p => p.IsSelected)))
                                                                    {
                                                                        item.ProgressImage = "notStarted.png";
                                                                        for (int j = i; j <= startPointContentItemPosition - 1; j++)
                                                                        {
                                                                            TotalMenuList[j].ProgressImage = "notStarted.png";
                                                                        }
                                                                        find = true;
                                                                        break;
                                                                    }
                                                                    else if (contentItemTest.TallyContent != null && (contentItemTest.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked)))
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
                    if (item.ContentLevelID == 9 && !item.IsStartingPoint)
                    {
                        if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                        {
                            foreach (var domaincollection in AcademicContentModelCollection)
                            {
                                if (domaincollection.AcademicItemModel != null && domaincollection.AcademicItemModel.Any())
                                {
                                    var contentItem = domaincollection.AcademicItemModel.FirstOrDefault(p => p.ContentItemId == item.ContentItemId);
                                    if (contentItem != null)
                                    {
                                        if (contentItem.ScoringValues != null && contentItem.ScoringValues.Any(p => p.IsSelected))
                                        {
                                            item.ProgressImage = "completed_TickMark.png";
                                        }
                                        else if (contentItem.TallyContent != null && contentItem.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked))
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
                    if (item.ContentLevelID == 7)
                    {
                        var childItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId && !p.IsStartingPoint);
                        if (childItems != null && childItems.Any())
                        {
                            foreach (var childitem in childItems)
                            {
                                if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                                {
                                    var modelItems = new List<AcademicItemModel>();
                                    var collection = AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == item.ContentCatgoryId || p.AreaId == item.ContentCatgoryId);
                                    if (collection != null && collection.Any())
                                    {
                                        foreach (var collectionitem in collection)
                                        {
                                            modelItems.AddRange(collectionitem.AcademicItemModel);
                                        }
                                    }
                                    if (modelItems != null && modelItems.Any())
                                    {
                                        var contentItem = modelItems.FirstOrDefault(p => p.ContentItemId == childitem.ContentItemId);
                                        if (contentItem != null)
                                        {
                                            if (!(contentItem.ScoringValues != null && contentItem.ScoringValues.Any(p => p.IsSelected)))
                                            {
                                                var indexofContentItem = modelItems.Where(p => p.ItemNumber.ToLower() != "s").OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(contentItem);
                                                var belowScored = false;
                                                var aboveScored = false;
                                                for (int i = 0; i < indexofContentItem; i++)
                                                {
                                                    var indexItem = modelItems.Where(p => p.ItemNumber.ToLower() != "s").OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if (indexItem != null)
                                                    {
                                                        if ((indexItem.ScoringValues != null && (indexItem.ScoringValues.Any(p => p.IsSelected))))
                                                        {
                                                            belowScored = true;
                                                            break;
                                                        }
                                                        if (indexItem.TallyContent != null && indexItem.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked))
                                                        {
                                                            belowScored = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                for (int i = indexofContentItem + 1; i < modelItems.Where(p => p.ItemNumber.ToLower() != "s").Count(); i++)
                                                {
                                                    var indexItem = modelItems.Where(p => p.ItemNumber.ToLower() != "s").OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if (indexItem != null)
                                                    {
                                                        if ((indexItem.ScoringValues != null && (indexItem.ScoringValues.Any(p => p.IsSelected))))
                                                        {
                                                            aboveScored = true;
                                                            break;
                                                        }
                                                        if (indexItem.TallyContent != null && indexItem.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked))
                                                        {
                                                            aboveScored = true;
                                                            break;
                                                        }
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

                    if (item.ContentLevelID == 8)
                    {
                        var childItems = TotalMenuList.Where(p => p.ParentID == item.ContentCatgoryId && !p.IsStartingPoint);
                        if (childItems != null && childItems.Any())
                        {
                            foreach (var childitem in childItems)
                            {
                                if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                                {
                                    var modelItems = new List<AcademicItemModel>();
                                    var collection = AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == item.ContentCatgoryId || p.AreaId == item.ContentCatgoryId);
                                    if (collection != null && collection.Any())
                                    {
                                        foreach (var collectionitem in collection)
                                        {
                                            modelItems.AddRange(collectionitem.AcademicItemModel);
                                        }
                                    }
                                    if (modelItems != null && modelItems.Any())
                                    {
                                        var contentItem = modelItems.FirstOrDefault(p => p.ContentItemId == childitem.ContentItemId);
                                        if (contentItem != null)
                                        {
                                            if (!(contentItem.ScoringValues != null && contentItem.ScoringValues.Any(p => p.IsSelected)))
                                            {
                                                var indexofContentItem = modelItems.Where(p => p.ItemNumber.ToLower() != "s").OrderBy(p => Convert.ToInt32(p.ItemNumber)).IndexOf(contentItem);
                                                var belowScored = false;
                                                var aboveScored = false;
                                                for (int i = 0; i < indexofContentItem; i++)
                                                {
                                                    var indexItem = modelItems.Where(p => p.ItemNumber.ToLower() != "s").OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if (indexItem != null)
                                                    {
                                                        if ((indexItem.ScoringValues != null && (indexItem.ScoringValues.Any(p => p.IsSelected))))
                                                        {
                                                            belowScored = true;
                                                            break;
                                                        }
                                                        if (indexItem.TallyContent != null && indexItem.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked))
                                                        {
                                                            belowScored = true;
                                                            break;
                                                        }
                                                    }
                                                }
                                                for (int i = indexofContentItem + 1; i < modelItems.Where(p => p.ItemNumber.ToLower() != "s").Count(); i++)
                                                {
                                                    var indexItem = modelItems.Where(p => p.ItemNumber.ToLower() != "s").OrderBy(p => Convert.ToInt32(p.ItemNumber)).ElementAt(i);
                                                    if (indexItem != null)
                                                    {
                                                        if ((indexItem.ScoringValues != null && (indexItem.ScoringValues.Any(p => p.IsSelected))))
                                                        {
                                                            aboveScored = true;
                                                            break;
                                                        }
                                                        if (indexItem.TallyContent != null && indexItem.TallyContent.Any(p => p.IsCorrectChecked || p.IsInCorrectChecked))
                                                        {
                                                            aboveScored = true;
                                                            break;
                                                        }
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
            TotalMenuList = new List<MenuContentModel>();
            if (ContentCategory != null && ContentCategory.Any())
            {
                var sequence = 0;
                foreach (var item in ContentCategory.OrderBy(p => p.sequenceNo))
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
                        MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        TotalMenuList.Add(MenuContentModel);
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
                        MenuContentModel.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                        TotalMenuList.Add(MenuContentModel);
                    }
                    else if (item.contentCategoryLevelId == 9)
                    {
                        if (ContentCategoryItems != null && ContentCategoryItems.Any())
                        {
                            var filteredContentCategoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == item.contentCategoryId);
                            if (filteredContentCategoryItems != null && filteredContentCategoryItems.Any())
                            {
                                if (ContentItem != null && ContentItem.Any())
                                {
                                    var filteredContentItems = ContentItem.Where(p => filteredContentCategoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => !p.sampleItem).ThenBy(p => p.sequenceNo);
                                    if (filteredContentItems != null && filteredContentItems.Any())
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
                                        startingpoint.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                        TotalMenuList.Add(startingpoint);
                                        foreach (var contentItem in filteredContentItems)
                                        {
                                            if (contentItem.sampleItem)
                                            {
                                                continue;
                                            }
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
                                            menuItem.SelectedCommandAction = new Action<MenuContentModel>(AddorRemoveItems);
                                            TotalMenuList.Add(menuItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            MenuList = new ObservableCollection<MenuContentModel>(TotalMenuList.Where(p => p.ContentLevelID == 6).OrderBy(p => p.SequenceNumber));
        }
        #endregion

        #region LoadContentData
        private async Task LoadContentData()
        {
            var categorylevelTask = Task.Run(() =>
            {
                ContentCategoryLevels = _contentCategoryLevelsService.GetItems().Where(p => p.assessmentId == 43).ToList();
            });
            var contentCategoryTask = Task.Run(() =>
            {
                ContentCategory = _contentcategoryservice.GetContentCategory();
            });
            var contentItemTask = Task.Run(() =>
            {
                ContentItem = _contentItemsService.GetItems();
            });
            var contentCategoryItemsTask = Task.Run(() =>
            {
                ContentCategoryItems = _contentCategoryItemsService.GetItems();
            });

            var ContentItemAttributeTask = Task.Run(() =>
            {
                ContentItemAttributes = _contentItemAttributeService.GetItems();
            });
            var contentRubricsTask = Task.Run(() =>
            {
                ContentRubrics = _contentRubricsService.GetItems();
            });
            var contentRubicPointsTask = Task.Run(() =>
            {
                ContentRubricPoints = _contentRubicPointsService.GetItems();
            });
            var contentItemTallyScoresTask = Task.Run(() =>
            {
                ContentItemTallyScores = _contentItemTalliesScoresService.GetItems();
            });
            var contentItemTallyTask = Task.Run(() =>
            {
                ContentItemTally = _contentItemTalliesService.GetItems();
            });
            var baselItemsTask = Task.Run(() =>
            {
                ContentBasalCeilingsItems = contentBasalCeilingsService.GetItems();
            });
            await Task.WhenAll(new List<Task>
            {
                categorylevelTask , contentCategoryTask , contentItemTask,contentRubicPointsTask,contentItemTallyScoresTask,
                contentCategoryItemsTask,ContentItemAttributeTask,contentRubricsTask,contentItemTallyTask,baselItemsTask
            });
            contentCategoryTask.Dispose(); contentCategoryTask = null; contentItemTask.Dispose(); contentItemTask = null;
            contentRubicPointsTask.Dispose(); contentRubicPointsTask = null;
            contentItemTallyScoresTask.Dispose(); contentItemTallyScoresTask = null; contentCategoryItemsTask.Dispose(); contentCategoryItemsTask = null;
            ContentItemAttributeTask.Dispose(); ContentItemAttributeTask = null; contentRubricsTask.Dispose(); contentRubricsTask = null;
            contentItemTallyTask.Dispose(); contentItemTallyTask = null; baselItemsTask.Dispose(); baselItemsTask = null;
            categorylevelTask.Dispose();
            categorylevelTask = null;
        }
        public async Task LoadPage()
        {
            await LoadContentData();
            var menuTask = GenerateMenuItems();
            var loadAllDomainTask = LoadAllDomains();
            await Task.WhenAll(new[] { menuTask, loadAllDomainTask });
            menuTask.Dispose();
            menuTask = null; loadAllDomainTask.Dispose(); loadAllDomainTask = null;
            var startingContentItem = CalculateStartingPoint();
            var currentQuestionIndex = 0;
            if (startingContentItem != null)
            {
                if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                {
                    currentQuestionIndex = AcademicContentModelCollection.IndexOf(AcademicContentModelCollection.FirstOrDefault(p => p.StartingPointCnntentItemID == startingContentItem.contentItemId));
                }
            }
            InitializeDataSource(currentQuestionIndex == -1 ? 0 : currentQuestionIndex);
            InitialLoad = true;
            CalculateBaselCeiling();
            InitialLoad = false;
            await PopupNavigation.Instance.PushAsync(new AcademicTestSessionOverview(), false);
            UserDialogs.Instance.HideLoading();
        }
        #endregion

        #region StratingPointCalc
        private List<ContentCategoryLevel> ContentCategoryLevels { get; set; }
        private List<ContentCategory> ContentCategory { get; set; }
        private List<ContentItem> ContentItem { get; set; }
        private List<ContentCategoryItem> ContentCategoryItems { get; set; }
        private List<ContentItemAttribute> ContentItemAttributes { get; set; }
        private List<ContentRubric> ContentRubrics { get; set; }
        private List<ContentRubricPoint> ContentRubricPoints { get; set; }
        private List<ContentItemTallyScore> ContentItemTallyScores { get; set; }
        private List<ContentItemTally> ContentItemTally { get; set; }
        #endregion

        #region AgeDiff
        private void CalculateAgeDiff()
        {
            DateTime Now = DateTime.Now;
            var splittedDate = DOB.Split('/');
            DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
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
        private ContentCategory StartingPointContentCategory;
        private ContentItem CalculateStartingPoint()
        {
            StartingPointContentCategory = null;
            if (ContentCategory != null && ContentCategory.Any())
            {
                var startingPointCatgory = ContentCategory.Where(p => p.contentCategoryLevelId == 9).FirstOrDefault(p => (TotalAgeinMonths <= p.highAge && TotalAgeinMonths >= p.lowAge));
                if (startingPointCatgory != null)
                {
                    StartingPointContentCategory = startingPointCatgory;
                    var categoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == startingPointCatgory.contentCategoryId).OrderBy(p => p.contentItemId);
                    if (categoryItems != null && categoryItems.Any())
                    {
                        if (ContentItem != null && ContentItem.Any())
                        {
                            var filteredContentItems = ContentItem.Where(p => categoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderByDescending(p => p.sampleItem).ThenBy(p => p.sequenceNo);
                            if (filteredContentItems != null && filteredContentItems.Any())
                            {
                                return filteredContentItems.FirstOrDefault();
                            }
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region AcademicformsCollectionProperties
        public List<ContentBasalCeilings> ContentBasalCeilingsItems { get; set; }
        public List<ContentCategory> StartingPointCategoryCollection { get; set; } = new List<ContentCategory>();
        #endregion

        #region AcademicFormCommands
        private void ItemClickAction(AcademicItemModel obj)
        {
            if (obj == null)
                return;

            if (ItemsCollection != null && ItemsCollection.Any())
            {
                var model = ItemsCollection.FirstOrDefault(p => p.ContentItemId == obj.ContentItemId);
                if (model != null)
                {
                    if (model.IsSelected)
                    {
                        return;
                    }
                }
            }
            Description = obj.Description;
            ItemDescription = obj.Description;

            if (ItemDescription != null)
            {
                ItemdescriptionFilePath = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.ContentItemId == obj.ContentItemId).HtmlFilePath;
            }

            var itemTitle = obj.ItemTitle;
            NotesDescriptionPath = obj.ScoringHtmlFilePath;
            IsScoringNotesVisible = NotesDescriptionPath != null ? true : false;
            if (IsScoringNotesVisible)
                ScoringNotesDescription = obj.ScoringNotes;
            else
                ScoringNotesDescription = null;
            ScoringCollection = obj.ScoringValues;
            TallyCollection = obj.TallyContent;
            if (TallyCollection.Count > 0)
                TallyCollection.ForEach(p => p.RowNum = obj.ItemSequenceNo);
            MaterialText = obj.MaterialContent;
            if (CurrentAcademicContentModel != null && CurrentAcademicContentModel.ImageCotentCollection.Any())
            {
                var image = CurrentAcademicContentModel.ImageCotentCollection.FirstOrDefault(p => p.ContentItemId == obj.ContentItemId);
                if (image != null)
                {
                    if (ImageFilePath != image.ImageStr)
                    {
                        ImageFilePath = image.ImageStr;
                        TemplateModel.ImageFilePath = ImageFilePath;
                    }

                }
                else
                {
                    ImageFilePath = null;
                }
            }
            if (ItemsCollection != null && ItemsCollection.Any())
            {
                foreach (var item in ItemsCollection)
                {
                    if (item.ContentItemId == obj.ContentItemId)
                    {
                        item.IsSelected = true;
                    }
                    else if (item.IsSelected)
                    {
                        item.IsSelected = false;
                    }
                }
            }

            TemplateModel.NotesDescriptionPath = NotesDescriptionPath;
            TemplateModel.ScoringCollection = ScoringCollection;
            TemplateModel.IsScoringNotesVisible = IsScoringNotesVisible;
            TemplateModel.TallyCollection = TallyCollection;
            TemplateModel.MaterialText = MaterialText;
            TemplateModel.ItemDescription = ItemDescription;
            TemplateModel.ScoringNotesDescription = ScoringNotesDescription;
        }
        public async void NextItemClicked()
        {
            UserDialogs.Instance.ShowLoading("Loading");
            await Task.Delay(300);

            var prevdoamin = CurrentAcademicContentModel.DomainCode;
            var prevsubdomainCode = CurrentAcademicContentModel.SubDomainCode;
            var prevareaCode = CurrentAcademicContentModel.AreaCode;

            if (AcademicContentModelCollection != null && AcademicContentModelCollection.Count > 0)
            {
                if (CurrentQuestion < AcademicContentModelCollection.Count - 1)
                {
                    CurrentQuestion += 1;
                    if (CurrentQuestion <= AcademicContentModelCollection.Count - 1)
                    {
                        SetData();
                    }
                }
                if (ItemsCollection != null && ItemsCollection.Any())
                {

                    if (ItemsCollection.Any(p => p.IsSelected))
                    {
                        var contentItemID = ItemsCollection.FirstOrDefault(p => p.IsSelected).ContentItemId;
                        if (CurrentAcademicContentModel.ImageCotentCollection != null && CurrentAcademicContentModel.ImageCotentCollection.Any())
                        {
                            ImageFilePath = CurrentAcademicContentModel.ImageCotentCollection.FirstOrDefault(p => p.ContentItemId == contentItemID)?.ImageStr;
                        }
                        //await Task.Delay(1000);
                    }

                }

                if (AcademicContentModelCollection.Count == 0 || AcademicContentModelCollection.Count == 1)
                {
                    IsNextVisible = false;
                    IspreviousVisible = false;
                }
                else
                {
                    IspreviousVisible = CurrentQuestion > 0;
                    IsNextVisible = CurrentQuestion < AcademicContentModelCollection.Count - 1;
                }
            }
            if (prevdoamin != CurrentAcademicContentModel.DomainCode || prevsubdomainCode != CurrentAcademicContentModel.SubDomainCode ||
                prevareaCode != CurrentAcademicContentModel.AreaCode)
            {
                StartingPointContentCategory = null;
                var parentCategoryId = CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId;
                var startingPointCatgory = ContentCategory.Where(p => p.contentCategoryLevelId == 9 && p.parentContentCategoryId == parentCategoryId).FirstOrDefault(p => (TotalAgeinMonths <= p.highAge && TotalAgeinMonths >= p.lowAge));
                if (startingPointCatgory != null)
                {
                    StartingPointContentCategory = startingPointCatgory;
                }
                InitialLoad = true;
                CheckScoreSelected();
                CalculateBaselCeiling();
                InitialLoad = false;
                if (LastModel != null)
                {
                    foreach (var item in AcademicContentModelCollection)
                    {
                        if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                        {
                            foreach (var inneritem in item.AcademicItemModel)
                            {
                                if (LastModel != null && inneritem.ContentItemId == LastModel.ContentItemId)
                                {
                                    if (item.DomainCategoryId != CurrentAcademicContentModel.DomainCategoryId)
                                    {
                                        LastModel = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            UserDialogs.Instance.HideLoading();
            //await Task.Delay(1000);
            //CanInitiateNextItemTap(true);
            //Dispose();

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

        void ResetTimer(bool dontResetValue = false)
        {
            TimerStatusText = "Start";

            if (TotalSeconds.TotalSeconds == this.MaxTime)
            {
                StartEnabled = false;
                TimerButtonBckgrd = Color.LightGray;
            }
            else
            {
                StartEnabled = true;
                TimerButtonBckgrd = Color.FromHex("#478128");
            }

            if (TotalSeconds.TotalSeconds != 0)
            {
                TimerReset = "iconrefreshblue.png";
                ResetEnabled = true;
            }


            _timer.Stop();
            if (!dontResetValue)
            {
                TotalSeconds = new TimeSpan(0, 0, 0, 0);
            }
        }
        private void SetData(string itemtoselect = null)
        {
            CurrentAcademicContentModel = AcademicContentModelCollection[CurrentQuestion];
            if (CurrentAcademicContentModel != null && (CurrentAcademicContentModel.SubDomainCode != "FLU" || CurrentAcademicContentModel.GroupTitle == "FLU S"))
            {
                ResetTimer(true);
            }

            FluencyItemDescriptionHeight = (CurrentAcademicContentModel != null && CurrentAcademicContentModel.GroupTitle == "FLU S") ? 250 : 150;

            if (CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any())
            {
                if (!string.IsNullOrEmpty(itemtoselect))
                {
                    foreach (var item in CurrentAcademicContentModel.AcademicItemModel)
                    {
                        item.IsSelected = item.ItemTitle == itemtoselect;
                    }
                }
                else
                {
                    var selectedItems = CurrentAcademicContentModel.AcademicItemModel.FirstOrDefault(p => p.IsSelected == true);
                    if (selectedItems == null)
                        CurrentAcademicContentModel.AcademicItemModel.FirstOrDefault().IsSelected = true;
                }

            }
            SetHeader();
            CheckWhichTemplate();
            NotesDescriptionPath = CurrentAcademicContentModel.AcademicItemModel.FirstOrDefault(p => p.IsSelected)?.ScoringHtmlFilePath;
            IsScoringNotesVisible = NotesDescriptionPath != null ? true : false;
            InstructionHeader = ((AcademicContentModelCollection[CurrentQuestion].Title == null || AcademicContentModelCollection[CurrentQuestion].Title == "Introduction") && AcademicContentModelCollection[CurrentQuestion].GroupTitle != "PC 1-7") ? AcademicContentModelCollection[CurrentQuestion].GroupTitle : AcademicContentModelCollection[CurrentQuestion].Title;

            InstructionHeader = (!string.IsNullOrEmpty(CurrentAcademicContentModel.AreaName) ? (CurrentAcademicContentModel.AreaName + ": ") : (CurrentAcademicContentModel.SubDomainName + ": ")) + InstructionHeader;

            if (CurrentAcademiTemplate == CurrentAcademiTemplate.ItemsInstructionScoreMaterialGrid)
            {
                var previousDesc = Description ?? "";
                Description = AcademicContentModelCollection[CurrentQuestion].Description;
                if (!string.IsNullOrEmpty(AcademicContentModelCollection[CurrentQuestion].DescFilePath) && previousDesc.ToLower() != Description)
                {
                    InstructionDescFilePath = AcademicContentModelCollection[CurrentQuestion].DescFilePath;
                }
            }
            else if (CurrentAcademiTemplate == CurrentAcademiTemplate.InstructionsImageItemsScoreGrid)
            {
                var previousDesc = Description ?? "";
                Description = AcademicContentModelCollection[CurrentQuestion].Description;
                if (!string.IsNullOrEmpty(AcademicContentModelCollection[CurrentQuestion].DescFilePath) && previousDesc.ToLower() != Description)
                {
                    InstructionImageScoreDescFilePath = AcademicContentModelCollection[CurrentQuestion].DescFilePath;
                    InstructionImageScoreDescription = AcademicContentModelCollection[CurrentQuestion].Description;
                }
            }
            else if (CurrentAcademiTemplate == CurrentAcademiTemplate.ImageSampleGrid)
            {
                if (AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any()
                    && !string.IsNullOrEmpty(AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault().HtmlFilePath))
                {
                    var previousFilePath = ImageSampleGridDescFilePath ?? "";
                    if (previousFilePath != AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault().HtmlFilePath)
                    {
                        ImageSampleGridDescFilePath = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault().HtmlFilePath;
                        ImageSampleGridDescription = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault().Description;
                    }
                }
            }
            else
            {
                Description = !AcademicContentModelCollection[CurrentQuestion].IsSampleItem ? AcademicContentModelCollection[CurrentQuestion].Description :
                               (AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ? AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(a => a.IsSelected == true).Description : "");
                if (!string.IsNullOrEmpty(Description))
                {
                    InstructionImageScoreDescFilePath = DescriptionFilePath = AcademicContentModelCollection[CurrentQuestion].IsSampleItem ? AcademicContentModelCollection[CurrentQuestion].HtmlFilePath :
                        AcademicContentModelCollection[CurrentQuestion].DescFilePath != null ? AcademicContentModelCollection[CurrentQuestion].DescFilePath :
                        (AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ?
                        AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(a => a.IsSelected == true).HtmlFilePath : "");
                }
            }
            ImageFilePath = null;
            MaterialText = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ?
                AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(a => a.IsSelected == true).MaterialContent : "Timing device";

            MaterialFilePath = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ?
               AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(a => a.IsSelected == true).MaterialHtmFilePath : "";

            ItemsCollection = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ?
                AcademicContentModelCollection[CurrentQuestion].AcademicItemModel : new List<AcademicItemModel>();

            ItemDescription = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ?
                AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(a => a.IsSelected == true).Description : "";

            if (IsScoringNotesVisible)
            {
                ScoringNotesDescription = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.Any() ?
                    AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(a => a.IsSelected == true).ScoringNotes : "";
            }
            else
            {
                ScoringNotesDescription = "";
            }

            var selectedItem = ItemsCollection.FirstOrDefault(p => p.IsSelected);

            if (selectedItem != null)
            {
                ItemdescriptionFilePath = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.ContentItemId == selectedItem.ContentItemId).HtmlFilePath;
            }
            else if (ItemDescription != null)
            {
                ItemdescriptionFilePath = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel[0].HtmlFilePath;
            }

            TallyCollection = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected) != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).TallyContent != null
                && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).TallyContent.Any() ?
                AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).TallyContent : new List<ContentItemTally>();

            if (selectedItem != null)
                TallyCollection.ForEach(p => p.RowNum = selectedItem.ItemSequenceNo);

            IsTallyVisible = TallyCollection.Count > 0 ? true : false;
            ScoringCollection = AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected) != null && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ScoringValues != null
                && AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ScoringValues.Any() ?
                AcademicContentModelCollection[CurrentQuestion].AcademicItemModel.FirstOrDefault(p => p.IsSelected).ScoringValues : new List<AcademicScoringModel>();

            if (IsTallyVisible)
                IsScoringVisible = false;
            else
                IsScoringVisible = ScoringCollection.Count > 0 ? true : false;


            if (AcademicContentModelCollection[CurrentQuestion].ImageCotentCollection.Count > 0)
            {
                ImageFilePath = AcademicContentModelCollection[CurrentQuestion].ImageCotentCollection[0].ImageStr;
            }
            GroupTitle = "";
            if (CurrentAcademicContentModel != null && CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any())
            {
                GroupTitle = CurrentAcademicContentModel.GroupTitle;
            }

            if (CurrentAcademicContentModel != null)
            {
                var categoryId = CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId;
                var testOverview = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == categoryId);
                if (testOverview != null)
                {
                    TestDate = testOverview.testDate;
                }
            }

            TemplateModel = new AcademicTemplateSelectorModel()
            {
                DescriptionFilePath = DescriptionFilePath,
                ContentRubicPointSelectionAction = ContentRubicPointSelectionAction,
                CurrentAcademiTemplate = CurrentAcademiTemplate,
                GroupTitle = GroupTitle,
                ImageFilePath = ImageFilePath,
                ImageSampleGridDescription = ImageSampleGridDescription,
                ImageSampleGridDescFilePath = ImageSampleGridDescFilePath,
                Description = Description,
                InstructionDescFilePath = InstructionDescFilePath,
                InstructionHeader = InstructionHeader,
                InstructionImageScoreDescription = InstructionImageScoreDescription,
                InstructionImageScoreDescFilePath = InstructionImageScoreDescFilePath,
                IsScoringNotesVisible = IsScoringNotesVisible,
                IsScoringVisible = IsScoringVisible,
                IsTallyVisible = IsTallyVisible,
                FluencyItemDescriptionHeight = FluencyItemDescriptionHeight,
                ItemDescription = ItemDescription,
                ItemdescriptionFilePath = ItemdescriptionFilePath,
                ItemsCollection = ItemsCollection,
                MaterialFilePath = MaterialFilePath,
                MaterialText = MaterialText,
                NotesDescription = NotesDescription,
                NotesDescriptionPath = NotesDescriptionPath,
                ScoringCollection = ScoringCollection,
                TallyCollection = TallyCollection,
                TimerVisibilty = TimerVisibilty,
                TimerSepertorWidth = TimerSepertorWidth,
                TimerWidth = TimerWidth,
                IsTimerInProgress = IsTimerInProgress,
                MaxTime = MaxTime,
                ResetEnabled = ResetEnabled,
                StartEnabled = StartEnabled,
                StartCommand = StartCommand,
                StopCommand = StopCommand,
                TimerButtonBckgrd = TimerButtonBckgrd,
                TimerReset = TimerReset,
                TimerStatusText = TimerStatusText,
                TotalSeconds = TotalSeconds,
                ScoringNotesDescription = ScoringNotesDescription
            };

            SetTemplateVisibility();
        }

        /// <summary>
        /// set visibily of the appropriate 'template' stacklayout
        /// </summary>
        private void SetTemplateVisibility()
        {
            ResetTemplateVisibility();
            if (TemplateModel != null)
            {
                switch (TemplateModel.CurrentAcademiTemplate)
                {
                    case CurrentAcademiTemplate.ImageMaterialSampleGrid:
                        IsImageMaterialSampleGrid = true;
                        break;
                    case CurrentAcademiTemplate.ItemsInstructionScoreMaterialGrid:
                        IsItemInsScorMatGrid = true;
                        break;
                    case CurrentAcademiTemplate.InstructionItemsScoreGrid:
                        IsInstructionItemsScoreGrid = true;
                        break;
                    case CurrentAcademiTemplate.ScoreItemGrid:
                        IsScoreItemGrid = true;
                        break;
                    case CurrentAcademiTemplate.InstructionsImageItemsScoreGrid:
                        IsInstructionsImageItemsScoreGrid = true;
                        break;
                    case CurrentAcademiTemplate.ImageItemScoreGrid:
                        IsImageItemScoreGrid = true;
                        break;
                    case CurrentAcademiTemplate.ImageSampleGrid:
                        IsImageSampleGrid = true;
                        break;
                    case CurrentAcademiTemplate.InstructionImageMaterialItemScoreGrid:
                        IsInstructionImageMaterialItemScoreGrid = true;
                        break;
                    default:
                        break;
                }
            }

        }

        private void ResetTemplateVisibility()
        {
            IsImageMaterialSampleGrid = false;
            IsItemInsScorMatGrid = false;
            IsInstructionItemsScoreGrid = false;
            IsScoreItemGrid = false;
            IsInstructionsImageItemsScoreGrid = false;
            IsImageItemScoreGrid = false;
            IsImageSampleGrid = false;
            IsInstructionImageMaterialItemScoreGrid = false;
        }

        public async void PreviousItemClicked()
        {
            UserDialogs.Instance.ShowLoading("Loading");
            await Task.Delay(300);
            var prevdoamin = CurrentAcademicContentModel.DomainCode;
            var prevsubdomainCode = CurrentAcademicContentModel.SubDomainCode;
            var prevareaCode = CurrentAcademicContentModel.AreaCode;

            if (AcademicContentModelCollection != null && AcademicContentModelCollection.Count > 0 && CurrentQuestion > 0)
            {
                if (CurrentQuestion > 0)
                {
                    CurrentQuestion -= 1;
                    SetData();
                }
            }

            if (ItemsCollection != null && ItemsCollection.Any())
            {
                if (ItemsCollection.Any(p => p.IsSelected))
                {
                    var contentItemID = ItemsCollection.FirstOrDefault(p => p.IsSelected).ContentItemId;
                    if (CurrentAcademicContentModel.ImageCotentCollection != null && CurrentAcademicContentModel.ImageCotentCollection.Any())
                    {
                        ImageFilePath = CurrentAcademicContentModel.ImageCotentCollection.FirstOrDefault(p => p.ContentItemId == contentItemID)?.ImageStr;
                    }
                }

            }

            if (AcademicContentModelCollection.Count == 0 || AcademicContentModelCollection.Count == 1)
            {
                IsNextVisible = false;
                IspreviousVisible = false;
            }
            else
            {
                IspreviousVisible = CurrentQuestion > 0;
                IsNextVisible = CurrentQuestion < AcademicContentModelCollection.Count - 1;
            }
            if (prevdoamin != CurrentAcademicContentModel.DomainCode || prevsubdomainCode != CurrentAcademicContentModel.SubDomainCode ||
                prevareaCode != CurrentAcademicContentModel.AreaCode)
            {
                StartingPointContentCategory = null;
                var parentCategoryId = CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId;
                var startingPointCatgory = ContentCategory.Where(p => p.contentCategoryLevelId == 9 && p.parentContentCategoryId == parentCategoryId).FirstOrDefault(p => (TotalAgeinMonths <= p.highAge && TotalAgeinMonths >= p.lowAge));
                if (startingPointCatgory != null)
                {
                    StartingPointContentCategory = startingPointCatgory;
                }
                InitialLoad = true;
                CheckScoreSelected();
                CalculateBaselCeiling();
                InitialLoad = false;
                if (LastModel != null)
                {
                    foreach (var item in AcademicContentModelCollection)
                    {
                        if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                        {
                            foreach (var inneritem in item.AcademicItemModel)
                            {
                                if (LastModel != null && inneritem.ContentItemId == LastModel.ContentItemId)
                                {
                                    if (item.DomainCategoryId != CurrentAcademicContentModel.DomainCategoryId)
                                    {
                                        LastModel = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            UserDialogs.Instance.HideLoading();
            //await Task.Delay(1000);
            //CanInitiatePrevItemTap(true);
            //Dispose();

        }

        public void InitializeDataSource(int currentQuestion, string itemtitle = null)
        {
            if (AcademicContentModelCollection != null && AcademicContentModelCollection.Count > 0)
            {
                CurrentQuestion = currentQuestion;
                SetData(itemtitle);
                if (AcademicContentModelCollection.Count == 0 || AcademicContentModelCollection.Count == 1)
                {
                    IsNextVisible = false;
                    IspreviousVisible = false;
                }
                else
                {
                    IspreviousVisible = CurrentQuestion > 0;
                    IsNextVisible = CurrentQuestion < AcademicContentModelCollection.Count - 1;
                }
            }
        }
        private string grouptitle;
        public string GroupTitle
        {
            get
            {
                return grouptitle;
            }
            set
            {
                grouptitle = value;
                OnPropertyChanged(nameof(GroupTitle));
            }
        }

        public Action SetTemplate { get; set; }

        private void CheckWhichTemplate()
        {

            if (CurrentAcademicContentModel != null)
            {
                if (CurrentAcademicContentModel.SubDomainCode == "FLU" && CurrentAcademicContentModel.GroupTitle != "FLU S")
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.ImageMaterialSampleGrid;
                    SetTemplate?.Invoke();
                    return;
                }

                if (!string.IsNullOrEmpty(CurrentAcademicContentModel.Title) && CurrentAcademicContentModel.AcademicItemModel != null &&
                    CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.ImageCotentCollection.Any() &&
                    CurrentAcademicContentModel.AcademicItemModel.Any(p => !string.IsNullOrEmpty((p.MaterialContent))) &&
                    CurrentAcademicContentModel.AcademicItemModel.Any(p => p.ScoringValues != null && p.ScoringValues.Any()))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.InstructionImageMaterialItemScoreGrid;
                    SetTemplate?.Invoke();
                    return;
                }

                if (!string.IsNullOrEmpty(CurrentAcademicContentModel.Title) && CurrentAcademicContentModel.AcademicItemModel != null &&
                CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.ImageCotentCollection.Any() && CurrentAcademicContentModel.AcademicItemModel.Any(p => p.ScoringValues != null && p.ScoringValues.Any()))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.InstructionsImageItemsScoreGrid;
                    SetTemplate?.Invoke();
                    return;
                }

                if (!string.IsNullOrEmpty(CurrentAcademicContentModel.Title) && CurrentAcademicContentModel.AcademicItemModel != null &&
                    CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.AcademicItemModel.Any(p => !string.IsNullOrEmpty((p.MaterialContent))) &&
                    CurrentAcademicContentModel.AcademicItemModel.Any(p => p.ScoringValues != null && p.ScoringValues.Any()))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.ItemsInstructionScoreMaterialGrid;
                    SetTemplate?.Invoke();
                    return;
                }

                if (!string.IsNullOrEmpty(CurrentAcademicContentModel.Title) && CurrentAcademicContentModel.AcademicItemModel != null &&
                   CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.AcademicItemModel.Any(p => p.ScoringValues != null && p.ScoringValues.Any()))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.InstructionItemsScoreGrid;
                    SetTemplate?.Invoke();
                    return;
                }

                if (CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any()
                    && CurrentAcademicContentModel.ImageCotentCollection != null && CurrentAcademicContentModel.ImageCotentCollection.Any() && CurrentAcademicContentModel.AcademicItemModel.Any(p => p.ScoringValues != null && p.ScoringValues.Any()))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.ImageItemScoreGrid;
                    SetTemplate?.Invoke();
                    return;
                }
                if (CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.AcademicItemModel.Any(p => p.ScoringValues != null && p.ScoringValues.Any()))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.ScoreItemGrid;
                    SetTemplate?.Invoke();
                    return;
                }

                if (!string.IsNullOrEmpty(CurrentAcademicContentModel.Title) && CurrentAcademicContentModel.AcademicItemModel != null &&
                    CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.ImageCotentCollection != null && CurrentAcademicContentModel.ImageCotentCollection.Any() && CurrentAcademicContentModel.AcademicItemModel.Any(p => !string.IsNullOrEmpty(p.MaterialContent)))
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.ImageMaterialSampleGrid;
                    SetTemplate?.Invoke();
                    return;
                }
                if (CurrentAcademicContentModel.AcademicItemModel != null && CurrentAcademicContentModel.AcademicItemModel.Any() && CurrentAcademicContentModel.ImageCotentCollection != null && CurrentAcademicContentModel.ImageCotentCollection.Any())
                {
                    CurrentAcademiTemplate = CurrentAcademiTemplate.ImageSampleGrid;
                    SetTemplate?.Invoke();
                    return;
                }
            }
        }

        private void SetHeader()
        {
            AssessmentType = "BATTELLE EARLY ACADEMIC SURVEY";
            string header = CurrentAcademicContentModel.DomainName + ": " + CurrentAcademicContentModel.SubDomainName
                + (!string.IsNullOrEmpty(CurrentAcademicContentModel.AreaName) ? (" - " + CurrentAcademicContentModel.AreaName) : "");
            AdministrationHeader = header.ToUpper();
        }
        #endregion

        #region Correct/Incorrect Selection
        public void CorrectAction(ContentItemTally tally)
        {
            HasDoneChanges = true;
            tally.CheckCorrectVisible = !tally.CheckCorrectVisible;
            tally.UncheckCorrectVisible = !tally.UncheckCorrectVisible;
            tally.CheckInCorrectVisible = false;
            tally.UncheckInCorrectVisible = true;
        }
        public void InCorrectAction(ContentItemTally tally)
        {
            HasDoneChanges = true;
            tally.CheckCorrectVisible = false;
            tally.UncheckCorrectVisible = true;
            tally.CheckInCorrectVisible = !tally.CheckInCorrectVisible;
            tally.UncheckInCorrectVisible = !tally.UncheckInCorrectVisible;
        }
        #endregion

        #region LoadAllDomainRegion
        private List<StudentTestFormResponses> studentTestFormResponses;
        List<AcademicContentModel> AcademicContentModelCollection = new List<AcademicContentModel>();
        private async Task LoadAllDomains()
        {
            await Task.Delay(0);
            List<ItemInfo> formItemInfo = new List<ItemInfo>();
            List<ContentCategory> DomainCategoryCollection = new List<ContentCategory>();
            List<ContentCategory> SubDomainCategoryCollection = new List<ContentCategory>();
            List<ContentCategory> AreaCategoryCollection = new List<ContentCategory>();
            List<ContentGroupWithItems> ContentGroupItemscollection = new List<ContentGroupWithItems>();

            OriginalStudentTestFormOverView = clinicalTestFormService.GetStudentTestFormsByID(LocaInstanceID);
            FormNotes = OriginalStudentTestFormOverView?.notes;
            foreach (var Category in ContentCategory)
            {
                if (Category.contentCategoryLevelId == 6)
                {
                    DomainCategoryCollection.Add(Category);
                }
                else if (Category.contentCategoryLevelId == 7)
                {
                    SubDomainCategoryCollection.Add(Category);
                }
                else if (Category.contentCategoryLevelId == 8)
                {
                    AreaCategoryCollection.Add(Category);
                }
                else if (Category.contentCategoryLevelId == 9)
                {
                    StartingPointCategoryCollection.Add(Category);
                }
            }

            studentTestFormResponses = _studentTestFormResponsesService.GetStudentTestFormResponses(LocaInstanceID);
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
            var contentGroup = _contentGroupService.GetItems();
            var contentGroupItems = _contentGroupItemsService.GetItems();
            ContentGroupItemscollection = new List<ContentGroupWithItems>();
            if (contentGroupItems != null && contentGroupItems.Any())
            {
                ContentGroupItemscollection = contentGroupItems.GroupBy(p => p.contentGroupId).Select(p => new ContentGroupWithItems { GroupId = p.Key, ContentItemIDs = p.Select(q => q.contentItemId).ToList() }).ToList();
            }

            AcademicContentModelCollection = new List<AcademicContentModel>();
            if (DomainCategoryCollection != null && DomainCategoryCollection.Any())
            {
                foreach (var item in DomainCategoryCollection.OrderBy(p => p.sequenceNo))
                {
                    var subDomainCategory = ContentCategory.Where(p => p.parentContentCategoryId == item.contentCategoryId).OrderBy(p => p.sequenceNo);
                    if (subDomainCategory != null && subDomainCategory.Any())
                    {
                        foreach (var subdomainCategory in subDomainCategory)
                        {
                            var areaCategory = ContentCategory.Where(p => p.parentContentCategoryId == subdomainCategory.contentCategoryId).OrderBy(p => p.sequenceNo);
                            if (areaCategory != null && areaCategory.Any())
                            {
                                foreach (var areacategory in areaCategory)
                                {
                                    var itemLevelCategories = ContentCategory.Where(p => p.parentContentCategoryId == areacategory.contentCategoryId).OrderBy(p => p.sequenceNo);
                                    if (itemLevelCategories != null && itemLevelCategories.Any())
                                    {
                                        if (ContentCategoryItems != null && ContentCategoryItems.Any())
                                        {
                                            foreach (var itemlevelcategory in itemLevelCategories)
                                            {
                                                var itemLevelCategoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == itemlevelcategory.contentCategoryId).OrderBy(p => p.contentItemId);
                                                if (itemLevelCategoryItems != null && itemLevelCategoryItems.Any())
                                                {
                                                    var startPointCategoryItem = itemLevelCategoryItems.FirstOrDefault().contentItemId;
                                                    var filteredContentItems = ContentItem.Where(p => itemLevelCategoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).ToList();
                                                    var sampleItems = filteredContentItems.Where(p => p.sampleItem);
                                                    filteredContentItems = filteredContentItems.Where(p => !p.sampleItem).OrderBy(p => p.sequenceNo).ToList();
                                                    if (contentGroupItems != null && contentGroupItems.Any() && filteredContentItems != null && filteredContentItems.Any())
                                                    {
                                                        var lstContentGroupWithItems = new List<ContentGroupWithItems>();
                                                        foreach (var filteredContentItem in filteredContentItems)
                                                        {
                                                            var group = contentGroupItems.FirstOrDefault(p => p.contentItemId == filteredContentItem.contentItemId);
                                                            if (group != null)
                                                            {
                                                                if (!lstContentGroupWithItems.Any())
                                                                {
                                                                    lstContentGroupWithItems.Add(new ContentGroupWithItems() { GroupId = group.contentGroupId, ContentItemIDs = new List<int>() { group.contentItemId } });
                                                                }
                                                                else
                                                                {
                                                                    var alreadyGroup = lstContentGroupWithItems.FirstOrDefault(p => p.GroupId == group.contentGroupId);
                                                                    if (alreadyGroup != null)
                                                                    {
                                                                        alreadyGroup.ContentItemIDs.Add(group.contentItemId);
                                                                    }
                                                                    else
                                                                    {
                                                                        lstContentGroupWithItems.Add(new ContentGroupWithItems() { GroupId = group.contentGroupId, ContentItemIDs = new List<int>() { group.contentItemId } });
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                lstContentGroupWithItems.Add(new ContentGroupWithItems() { GroupId = 0, ContentItemIDs = new List<int>() { filteredContentItem.contentItemId } });
                                                            }
                                                        }
                                                        if (sampleItems != null && sampleItems.Any())
                                                        {
                                                            filteredContentItems.AddRange(sampleItems);
                                                            lstContentGroupWithItems.Insert(0, new ContentGroupWithItems() { ContentItemIDs = sampleItems.Select(p => p.contentItemId).ToList() });
                                                        }
                                                        foreach (var groupItem in lstContentGroupWithItems)
                                                        {
                                                            var finalfilteredContentItems = filteredContentItems.Where(p => groupItem.ContentItemIDs.Contains(p.contentItemId));
                                                            if (finalfilteredContentItems != null && finalfilteredContentItems.Any())
                                                            {
                                                                var AcademicContentModel = new AcademicContentModel();
                                                                AcademicContentModel.BasalCeilingObtained = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == areacategory.contentCategoryId).BaselCeilingReached;
                                                                AcademicContentModel.rawScore = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == areacategory.contentCategoryId).rawScore; ;
                                                                AcademicContentModel.DomainName = item.name;
                                                                AcademicContentModel.SubDomainName = subdomainCategory.name;
                                                                AcademicContentModel.AreaName = areacategory.name;
                                                                AcademicContentModel.DomainCode = item.code;
                                                                AcademicContentModel.SubDomainCode = subdomainCategory.code;
                                                                AcademicContentModel.AreaCode = areacategory.code;
                                                                AcademicContentModel.AreaId = areacategory.contentCategoryId;
                                                                AcademicContentModel.DomainCategoryId = item.contentCategoryId;
                                                                AcademicContentModel.SubDomainCategoryId = subdomainCategory.contentCategoryId;
                                                                AcademicContentModel.AcademicItemModel = new List<AcademicItemModel>();
                                                                AcademicContentModel.ImageCotentCollection = new List<ImageContentItem>();
                                                                AcademicContentModelCollection.Add(AcademicContentModel);
                                                                AcademicContentModel.GroupTitle = contentGroup.FirstOrDefault(p => p.contentGroupId == groupItem.GroupId)?.groupText;
                                                                foreach (var contentItem in finalfilteredContentItems.OrderBy(p => p.sequenceNo))
                                                                {
                                                                    if (startPointCategoryItem == contentItem.contentItemId)
                                                                    {
                                                                        AcademicContentModel.StartingPointCnntentItemID = startPointCategoryItem;
                                                                    }
                                                                    if (groupItem.GroupId == 0)
                                                                    {
                                                                        AcademicContentModel.GroupTitle = contentItem.itemCode;
                                                                    }
                                                                    var itemModel = new Models.AcademicFolder.AcademicItemModel();
                                                                    itemModel.ItemClick = ItemClickAction;
                                                                    itemModel.ItemNumber = contentItem.itemNumber;
                                                                    itemModel.Description = contentItem.itemText;
                                                                    itemModel.ContentItemId = contentItem.contentItemId;
                                                                    itemModel.ItemTitle = contentItem.itemCode;
                                                                    itemModel.MaxTime = contentItem.maxTime1;
                                                                    itemModel.ItemSequenceNo = contentItem.sequenceNo;
                                                                    itemModel.HtmlFilePath = contentItem.HtmlFilePath;
                                                                    AcademicContentModel.AcademicItemModel.Add(itemModel);
                                                                    itemModel.ScoringValues = new List<AcademicScoringModel>();
                                                                    itemModel.TallyContent = new List<ContentItemTally>();
                                                                    AcademicContentModel.IsSampleItem = contentItem.sampleItem;
                                                                    if (contentItem.sampleItem)
                                                                    {
                                                                        AcademicContentModel.Title = "Sample";
                                                                    }
                                                                    if (ContentItemAttributes != null && ContentItemAttributes.Any())
                                                                    {
                                                                        ContentItemAttributes.ForEach(async (x) =>
                                                                        {
                                                                            if (contentItem.contentItemId == x.contentItemId)
                                                                            {
                                                                                if (x.name == "Materials")
                                                                                {
                                                                                    itemModel.MaterialContent = x.value;
                                                                                    itemModel.MaterialHtmFilePath = x.HtmlFilePath;
                                                                                }
                                                                                if (x.name == "Sample" || x.name == "Introduction")
                                                                                {
                                                                                    AcademicContentModel.Title = x.name;
                                                                                    AcademicContentModel.Description = x.value;
                                                                                    AcademicContentModel.DescFilePath = x.HtmlFilePath;
                                                                                }
                                                                                if (x.name == "Image")
                                                                                {
                                                                                    string path = null;
                                                                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                                                                    ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                                                                    if (existfolder == ExistenceCheckResult.FolderExists)
                                                                                    {
                                                                                        var imageFolder = await rootFolder.GetFolderAsync("Images");
                                                                                        ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(x.value + ".png");

                                                                                        if (exist1 == ExistenceCheckResult.FileExists)
                                                                                        {
                                                                                            var imageFile1 = await imageFolder.GetFileAsync(x.value + ".png");
                                                                                            path = imageFile1.Path;
                                                                                        }
                                                                                    }
                                                                                    AcademicContentModel.ImageCotentCollection.Add(new ImageContentItem() { ImageStr = path, ContentItemId = x.contentItemId });
                                                                                }
                                                                            }
                                                                        });
                                                                    }
                                                                    if (ContentRubrics != null && ContentRubrics.Any())
                                                                    {
                                                                        ContentRubrics.ForEach(x =>
                                                                        {
                                                                            if (x.contentItemId == contentItem.contentItemId)
                                                                            {
                                                                                itemModel.ScoringNotes = x.notes;
                                                                                itemModel.ScoringDesc = x.pointsDesc;
                                                                                itemModel.ScoringHtmlFilePath = x.HtmlFilePath;
                                                                                ContentRubricPoints.ForEach(y =>
                                                                                {
                                                                                    if (x.contentRubricId == y.contentRubricId)
                                                                                    {
                                                                                        var score = new AcademicScoringModel();
                                                                                        score.points = y.points;
                                                                                        score.description = y.description;
                                                                                        score.contentRubricId = y.contentRubricId;
                                                                                        score.contentRubricPointsId = y.contentRubricPointsId;
                                                                                        score.HtmlFilePath = y.HtmlFilePath;
                                                                                        itemModel.ScoringValues.Add(score);
                                                                                    }
                                                                                });
                                                                            }
                                                                            if (itemModel.ScoringValues != null && itemModel.ScoringValues.Any())
                                                                            {
                                                                                itemModel.ScoringValues = new List<AcademicScoringModel>(itemModel.ScoringValues.OrderByDescending(p => p.points));
                                                                            }
                                                                        });
                                                                    }
                                                                    if (ContentItemTally != null && ContentItemTally.Any())
                                                                    {
                                                                        ContentItemTally.ForEach(x =>
                                                                        {
                                                                            if (x.contentItemId == contentItem.contentItemId)
                                                                            {
                                                                                var tally = new ContentItemTally();
                                                                                tally.contentItemId = contentItem.contentItemId; ;
                                                                                tally.contentItemTallyId = x.contentItemTallyId;
                                                                                tally.text = x.text;
                                                                                tally.sequenceNo = x.sequenceNo;
                                                                                tally.RowNum = x.RowNum;
                                                                                tally.CorrectLayoutAction = CorrectAction;
                                                                                tally.InCorrectLayoutAction = InCorrectAction;
                                                                                itemModel.TallyContent.Add(tally);
                                                                            }
                                                                        });
                                                                    }
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
                                        if (ContentCategoryItems != null && ContentCategoryItems.Any())
                                        {
                                            var areaCategoryItems = ContentCategoryItems.Where(p => p.contentCategoryId == areacategory.contentCategoryId).OrderBy(p => p.contentItemId);
                                            if (areaCategoryItems != null && areaCategoryItems.Any())
                                            {
                                                var startingPointContentItemId = areaCategoryItems.FirstOrDefault().contentItemId;
                                                if (ContentItem != null && ContentItem.Any())
                                                {
                                                    var filteredContentItems = ContentItem.Where(p => areaCategoryItems.Select(q => q.contentItemId).Contains(p.contentItemId)).OrderBy(p => p.sequenceNo).ToList();
                                                    var sampleItems = filteredContentItems.Where(p => p.sampleItem);
                                                    filteredContentItems = filteredContentItems.Where(p => !p.sampleItem).OrderBy(p => p.sequenceNo).ToList();
                                                    var lstContentGroupWithItems = new List<ContentGroupWithItems>();
                                                    foreach (var filteredContentItem in filteredContentItems)
                                                    {
                                                        var group = contentGroupItems.FirstOrDefault(p => p.contentItemId == filteredContentItem.contentItemId);
                                                        if (group != null)
                                                        {
                                                            if (!lstContentGroupWithItems.Any())
                                                            {
                                                                lstContentGroupWithItems.Add(new ContentGroupWithItems() { GroupId = group.contentGroupId, ContentItemIDs = new List<int>() { group.contentItemId } });
                                                            }
                                                            else
                                                            {
                                                                var alreadyGroup = lstContentGroupWithItems.FirstOrDefault(p => p.GroupId == group.contentGroupId);
                                                                if (alreadyGroup != null)
                                                                {
                                                                    alreadyGroup.ContentItemIDs.Add(group.contentItemId);
                                                                }
                                                                else
                                                                {
                                                                    lstContentGroupWithItems.Add(new ContentGroupWithItems() { GroupId = group.contentGroupId, ContentItemIDs = new List<int>() { group.contentItemId } });
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            lstContentGroupWithItems.Add(new ContentGroupWithItems() { GroupId = 0, ContentItemIDs = new List<int>() { filteredContentItem.contentItemId } });
                                                        }
                                                    }
                                                    if (sampleItems != null && sampleItems.Any())
                                                    {
                                                        filteredContentItems.AddRange(sampleItems);
                                                        lstContentGroupWithItems.Insert(0, new ContentGroupWithItems() { ContentItemIDs = sampleItems.Select(p => p.contentItemId).ToList() });
                                                    }
                                                    foreach (var groupItem in lstContentGroupWithItems)
                                                    {
                                                        var finalfilteredContentItems = filteredContentItems.Where(p => groupItem.ContentItemIDs.Contains(p.contentItemId));
                                                        if (finalfilteredContentItems != null && finalfilteredContentItems.Any())
                                                        {
                                                            var AcademicContentModel = new AcademicContentModel();
                                                            AcademicContentModel.BasalCeilingObtained = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == subdomainCategory.contentCategoryId).BaselCeilingReached;
                                                            AcademicContentModel.rawScore = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == subdomainCategory.contentCategoryId).rawScore;
                                                            AcademicContentModel.DomainName = item.name;
                                                            AcademicContentModel.SubDomainName = subdomainCategory.name;
                                                            AcademicContentModel.AreaName = null;
                                                            AcademicContentModel.DomainCode = item.code;
                                                            AcademicContentModel.SubDomainCode = subdomainCategory.code;
                                                            AcademicContentModel.AreaCode = null;
                                                            AcademicContentModel.AreaId = 0;
                                                            AcademicContentModel.DomainCategoryId = item.contentCategoryId;
                                                            AcademicContentModel.SubDomainCategoryId = subdomainCategory.contentCategoryId;
                                                            AcademicContentModel.AcademicItemModel = new List<AcademicItemModel>();
                                                            AcademicContentModel.ImageCotentCollection = new List<ImageContentItem>();
                                                            AcademicContentModelCollection.Add(AcademicContentModel);
                                                            AcademicContentModel.GroupTitle = contentGroup.FirstOrDefault(p => p.contentGroupId == groupItem.GroupId)?.groupText;
                                                            foreach (var contentItem in finalfilteredContentItems.OrderBy(p => p.sequenceNo))
                                                            {
                                                                if (startingPointContentItemId == contentItem.contentItemId)
                                                                {
                                                                    AcademicContentModel.StartingPointCnntentItemID = startingPointContentItemId;
                                                                }
                                                                if (groupItem.GroupId == 0)
                                                                {
                                                                    AcademicContentModel.GroupTitle = contentItem.itemCode;
                                                                }
                                                                var itemModel = new Models.AcademicFolder.AcademicItemModel();
                                                                itemModel.ItemClick = ItemClickAction;
                                                                itemModel.Description = !string.IsNullOrEmpty(contentItem.itemText) ? contentItem.itemText : contentItem.alternateItemText;
                                                                itemModel.ContentItemId = contentItem.contentItemId;
                                                                itemModel.HtmlFilePath = contentItem.HtmlFilePath;
                                                                itemModel.ItemTitle = contentItem.itemCode;
                                                                itemModel.ItemNumber = contentItem.itemNumber;
                                                                itemModel.MaxTime = contentItem.maxTime1;
                                                                itemModel.ItemSequenceNo = contentItem.sequenceNo;
                                                                AcademicContentModel.AcademicItemModel.Add(itemModel);
                                                                itemModel.ScoringValues = new List<AcademicScoringModel>();
                                                                itemModel.TallyContent = new List<ContentItemTally>();
                                                                AcademicContentModel.IsSampleItem = contentItem.sampleItem;
                                                                if (contentItem.sampleItem)
                                                                {
                                                                    AcademicContentModel.Title = "Sample";
                                                                }
                                                                if (ContentItemAttributes != null && ContentItemAttributes.Any())
                                                                {
                                                                    ContentItemAttributes.ForEach(async (x) =>
                                                                    {
                                                                        if (contentItem.contentItemId == x.contentItemId)
                                                                        {
                                                                            if (x.name == "Materials")
                                                                            {
                                                                                itemModel.MaterialContent = x.value;
                                                                                itemModel.MaterialHtmFilePath = x.HtmlFilePath;
                                                                            }
                                                                            if (x.name == "Sample" || x.name == "Introduction")
                                                                            {
                                                                                AcademicContentModel.Title = x.name;
                                                                                AcademicContentModel.Description += x.value;
                                                                                AcademicContentModel.DescFilePath = x.HtmlFilePath;
                                                                            }
                                                                            if (x.name == "Sample 1" || x.name == "Sample 2")
                                                                            {
                                                                                if (!string.IsNullOrEmpty(itemModel.Description))
                                                                                {
                                                                                    itemModel.Description += "<b>" + x.name + "</b>" + x.value + "<br/><br/>";
                                                                                    PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                                                                    ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("contenthtml");
                                                                                    if (existfolder == ExistenceCheckResult.FolderExists)
                                                                                    {
                                                                                        var imageFolder = await rootFolder.GetFolderAsync("contenthtml");
                                                                                        var file = await imageFolder.CreateFileAsync("multi_sample.html", CreationCollisionOption.OpenIfExists);
                                                                                        var htmlstring = "<!DOCTYPE html>" + "<html lang='en' xmlns = 'http://www.w3.org/1999/xhtml'>" + "<head>" + "<meta http-equiv='Content-Type' charset='UTF-8' content='text/html;charset = UTF-8' name='viewport' content='width=device-width,initial-scale=1,maximum-scale=1'/></head>";
                                                                                        var finalString = htmlstring + itemModel.Description + "</body></html>";
                                                                                        System.IO.File.WriteAllText(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, imageFolder.Name, file.Name), finalString);
                                                                                        if (Xamarin.Essentials.DeviceInfo.Platform == DevicePlatform.Android)
                                                                                        {
                                                                                            itemModel.HtmlFilePath = "file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, imageFolder.Name, file.Name);
                                                                                        }
                                                                                        else if (Xamarin.Essentials.DeviceInfo.Platform == DevicePlatform.iOS)
                                                                                        {
                                                                                            itemModel.HtmlFilePath = imageFolder.Name + "/" + file.Name;
                                                                                        }

                                                                                        else
                                                                                        {
                                                                                            itemModel.HtmlFilePath = "ms-appdata:///local/contenthtml" + "/multi_sample.html";
                                                                                        }
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    itemModel.Description = "<b>" + x.name + "</b>" + x.value;
                                                                                }
                                                                            }
                                                                            if (x.name == "Image")
                                                                            {
                                                                                string path = null;
                                                                                PCLStorage.IFolder rootFolder = PCLStorage.FileSystem.Current.LocalStorage;
                                                                                ExistenceCheckResult existfolder = await rootFolder.CheckExistsAsync("Images");
                                                                                if (existfolder == ExistenceCheckResult.FolderExists)
                                                                                {
                                                                                    var imageFolder = await rootFolder.GetFolderAsync("Images");
                                                                                    ExistenceCheckResult exist1 = await imageFolder.CheckExistsAsync(x.value + ".png");

                                                                                    if (exist1 == ExistenceCheckResult.FileExists)
                                                                                    {
                                                                                        var imageFile1 = await imageFolder.GetFileAsync(x.value + ".png");
                                                                                        path = imageFile1.Path;
                                                                                    }
                                                                                }
                                                                                AcademicContentModel.ImageCotentCollection.Add(new ImageContentItem() { ImageStr = path, ContentItemId = x.contentItemId });
                                                                            }
                                                                        }
                                                                    });
                                                                }
                                                                if (ContentRubrics != null && ContentRubrics.Any())
                                                                {
                                                                    ContentRubrics.ForEach(x =>
                                                                    {
                                                                        if (x.contentItemId == contentItem.contentItemId)
                                                                        {
                                                                            itemModel.ScoringNotes = x.notes;
                                                                            itemModel.ScoringDesc = x.pointsDesc;
                                                                            itemModel.ScoringHtmlFilePath = x.HtmlFilePath;

                                                                            ContentRubricPoints.ForEach(y =>
                                                                            {
                                                                                if (x.contentRubricId == y.contentRubricId)
                                                                                {
                                                                                    var score = new AcademicScoringModel();
                                                                                    score.points = y.points;
                                                                                    score.description = y.description;
                                                                                    score.contentRubricId = y.contentRubricId;
                                                                                    score.contentRubricPointsId = y.contentRubricPointsId;
                                                                                    score.HtmlFilePath = y.HtmlFilePath;
                                                                                    itemModel.ScoringValues.Add(score);
                                                                                }
                                                                            });
                                                                        }
                                                                    });
                                                                    if (itemModel.ScoringValues != null && itemModel.ScoringValues.Any())
                                                                    {
                                                                        itemModel.ScoringValues = new List<AcademicScoringModel>(itemModel.ScoringValues.OrderByDescending(p => p.points));
                                                                    }
                                                                }
                                                                if (ContentItemTally != null && ContentItemTally.Any())
                                                                {
                                                                    ContentItemTally.ForEach(x =>
                                                                    {
                                                                        if (x.contentItemId == contentItem.contentItemId)
                                                                        {
                                                                            var tally = new ContentItemTally();
                                                                            tally.contentItemId = contentItem.contentItemId; ;
                                                                            tally.contentItemTallyId = x.contentItemTallyId;
                                                                            tally.text = x.text;
                                                                            tally.sequenceNo = x.sequenceNo;
                                                                            tally.RowNum = x.RowNum;
                                                                            tally.CorrectLayoutAction = CorrectAction;
                                                                            tally.InCorrectLayoutAction = InCorrectAction;
                                                                            itemModel.TallyContent.Add(tally);
                                                                        }
                                                                    });
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
                        }
                    }
                }
            }

            if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any() && formItemInfo != null && formItemInfo.Any())
            {
                foreach (var academicContentModel in AcademicContentModelCollection)
                {
                    if (academicContentModel.AcademicItemModel != null && academicContentModel.AcademicItemModel.Any())
                    {
                        foreach (var item in academicContentModel.AcademicItemModel)
                        {
                            var contentItemResponse = formItemInfo.FirstOrDefault(p => p.itemId == item.ContentItemId);
                            if (contentItemResponse != null)
                            {
                                item.Notes = contentItemResponse.itemNotes;
                                if (item.ScoringValues != null && item.ScoringValues.Any())
                                {
                                    var score = item.ScoringValues.FirstOrDefault(p => p.contentRubricPointsId == contentItemResponse.ContentRubricPointId);
                                    if (score != null)
                                    {
                                        score.IsSelected = true;
                                    }
                                }
                                if (contentItemResponse.tallyItems != null && contentItemResponse.tallyItems.Any())
                                {
                                    if (item.TallyContent != null && item.TallyContent.Any())
                                    {
                                        foreach (var tallyitem in contentItemResponse.tallyItems)
                                        {
                                            var tallyContent = item.TallyContent.FirstOrDefault(p => p.contentItemTallyId == tallyitem.itemTallyId);
                                            if (tallyContent != null)
                                            {
                                                if (tallyitem.tallyScore == 0)
                                                {
                                                    tallyContent.IsCorrectChecked = false;
                                                    tallyContent.IsInCorrectChecked = true;

                                                    tallyContent.CheckInCorrectVisible = true;
                                                    tallyContent.UncheckInCorrectVisible = false;

                                                    tallyContent.CheckCorrectVisible = false;
                                                    tallyContent.UncheckCorrectVisible = true;
                                                }
                                                else
                                                {
                                                    tallyContent.IsCorrectChecked = true;
                                                    tallyContent.IsInCorrectChecked = false;

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

                    }
                }
            }

            formItemInfo = null;
            DomainCategoryCollection = null;
            SubDomainCategoryCollection = null;
            AreaCategoryCollection = null;
            ContentGroupItemscollection = null;
        }
        #endregion

        #region Basel/Ceiling Properties
        private bool baselCeilingApplicable;
        public bool BaselCeilingApplicable
        {
            get
            {
                return baselCeilingApplicable;
            }
            set
            {
                baselCeilingApplicable = value;
                OnPropertyChanged(nameof(BaselCeilingApplicable));
            }
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
        #endregion

        #region Basel/Ceiling Calculation
        private void CheckBasalCeilingApplicable()
        {
            var contentBaseCeiling = ContentBasalCeilingsItems.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId));
            BaselCeilingApplicable = contentBaseCeiling != null;
        }
        async void CeilingObtainedPopupView()
        {
            ResetTimer(true);
            await PopupNavigation.Instance.PushAsync(new CeilingObtainedPopupView(AdministrationHeader + ".") { BindingContext = this }, false);
        }
        public void CalculateBaselCeiling()
        {
            var baselceilingReached = BaselObtained && CeilingObtained;
            CheckBasalCeilingApplicable();
            var selectedAcademicContentItem = default(List<AcademicContentModel>);
            if (CurrentAcademicContentModel.AreaId == 0)
            {
                selectedAcademicContentItem = new List<AcademicContentModel>(AcademicContentModelCollection.Where(p => !p.IsSampleItem && p.SubDomainCategoryId == CurrentAcademicContentModel.SubDomainCategoryId));
            }
            else
            {
                selectedAcademicContentItem = new List<AcademicContentModel>(AcademicContentModelCollection.Where(p => !p.IsSampleItem && p.AreaId == CurrentAcademicContentModel.AreaId));
            }
            int? rawscore = default(int?);
            if (!BaselCeilingApplicable)
            {
                if (commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScoreEnabled)
                {
                    BaselObtained = false;
                    BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                    CeilingObtained = false;
                    CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                    if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                    {
                        foreach (var academicItem in selectedAcademicContentItem)
                        {
                            academicItem.BasalCeilingObtained = true;
                        }
                    }
                    return;
                }
                if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                {
                    foreach (var academicItem in selectedAcademicContentItem)
                    {
                        if (academicItem.AcademicItemModel != null && academicItem.AcademicItemModel.Any())
                        {
                            var selectedScoring = academicItem.AcademicItemModel.Where(p => p.ScoringValues != null && p.ScoringValues.Any());
                            if (selectedScoring != null && selectedScoring.Any())
                            {
                                foreach (var item in selectedScoring)
                                {
                                    var scoreitem = item.ScoringValues.FirstOrDefault(p => p.IsSelected);
                                    if (scoreitem != null)
                                    {
                                        if (!rawscore.HasValue)
                                        {
                                            rawscore = 0;
                                        }
                                        rawscore += scoreitem.points;
                                    }
                                }
                            }
                            if (!rawscore.HasValue)
                            {
                                rawscore = 0;
                            }
                            var selectedTally = academicItem.AcademicItemModel.Where(p => p.TallyContent != null && p.TallyContent.Any());
                            if (selectedTally != null && selectedTally.Any())
                            {
                                foreach (var item in selectedTally)
                                {
                                    var scoreitem = item.TallyContent.Where(p => p.IsCorrectChecked);
                                    if (scoreitem != null && scoreitem.Any())
                                    {
                                        rawscore += commonDataService.ContentItemTallyScores.Where(p => p.contentItemId == scoreitem.FirstOrDefault().contentItemId).FirstOrDefault(p => p.totalPoints == scoreitem.Count())?.score ?? scoreitem.Count();
                                    }
                                }
                            }
                        }
                    }
                }

                if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                {
                    foreach (var academicItem in selectedAcademicContentItem)
                    {
                        academicItem.rawScore = rawscore;
                        academicItem.BasalCeilingObtained = true;
                    }
                }
                if (CurrentAcademicContentModel.SubDomainCategoryId == 162) // For Fluency We need to have Rawscore Calculation Different
                {
                    if (rawscore.HasValue)
                    {
                        if (TotalSeconds.TotalSeconds <= 30)
                        {
                            float score = Convert.ToSingle(rawscore) / 30;
                            float finalscore = score * 60;
                            rawscore = Convert.ToInt32(finalscore);
                        }
                        else
                        {
                            float score = Convert.ToSingle(rawscore) / Convert.ToInt32(TotalSeconds.TotalSeconds);
                            float finalscore = score * 60;
                            rawscore = Convert.ToInt32(finalscore);
                        }
                        CurrentAcademicContentModel.rawScore = rawscore;
                    }
                    else
                    {
                        CurrentAcademicContentModel.rawScore = rawscore;
                    }
                }
                else
                {
                    CurrentAcademicContentModel.rawScore = rawscore;
                }
                CurrentAcademicContentModel.BasalCeilingObtained = true;
                commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScore = rawscore;
                commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).BaselCeilingReached = true;
                return;
            }
            if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
            {
                if (commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScoreEnabled)
                {
                    BaselObtained = false;
                    BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                    CeilingObtained = false;
                    CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                    return;
                }
                var firstItemScore = -1;
                BaselObtained = false;
                CeilingObtained = false;
                var baselItemsCount = 3;
                var ceilingItemsCount = 3;
                var baselScore = 2;
                var ceilingScore = 0;
                BasalImage = "notStarted.png";
                CeilingImage = "notStarted.png";
                var baselObtainedItem = default(AcademicItemModel);
                var ceilingObtainedItem = default(AcademicItemModel);
                var contentBaseCeiling = ContentBasalCeilingsItems.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId));
                if (contentBaseCeiling != null)
                {
                    BaselCeilingApplicable = true;
                    baselItemsCount = contentBaseCeiling.basalCount;
                    ceilingItemsCount = contentBaseCeiling.ceilingCount;
                    baselScore = contentBaseCeiling.basalScore;
                    ceilingScore = contentBaseCeiling.ceilingScore;

                    var firstAcademicContentItem = selectedAcademicContentItem.FirstOrDefault(p => p.DomainCategoryId == CurrentAcademicContentModel.DomainCategoryId);
                    if (firstAcademicContentItem != null && firstAcademicContentItem.AcademicItemModel != null && firstAcademicContentItem.AcademicItemModel.Any())
                    {
                        var scoreingPoints = firstAcademicContentItem.AcademicItemModel.FirstOrDefault().ScoringValues;
                        if (scoreingPoints != null && scoreingPoints.Any())
                        {
                            var score = scoreingPoints.FirstOrDefault(p => p.IsSelected);
                            if (score != null)
                            {
                                firstItemScore = score.points;
                            }
                        }
                    }
                    if (firstItemScore != -1)
                    {
                        baselObtainedItem = firstAcademicContentItem.AcademicItemModel.FirstOrDefault(p => p.ScoringValues.Any(q => q.IsSelected));
                        baselObtained = true;
                    }
                    var allScoredItems = new List<AcademicItemModel>();
                    foreach (var item in selectedAcademicContentItem)
                    {
                        if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                        {
                            foreach (var contentItem in item.AcademicItemModel)
                            {
                                if (contentItem.ScoringValues != null && contentItem.ScoringValues.Any())
                                {
                                    var scoreItem = contentItem.ScoringValues.FirstOrDefault(p => p.IsSelected);
                                    if (scoreItem != null)
                                    {
                                        allScoredItems.Add(contentItem);
                                    }
                                }
                            }
                        }
                    }

                    var hasSequenceScored = true;
                    var SequnetialScoredItems = new List<AcademicItemModel>();
                    for (int i = 0; i < allScoredItems.Count - 1; i++)
                    {
                        if (i + 1 <= allScoredItems.Count - 1)
                        {
                            if (allScoredItems[i + 1].ItemSequenceNo - allScoredItems[i].ItemSequenceNo == 1)
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
                            var seqfirstscore = sequentilFirstItem.ScoringValues.FirstOrDefault(p => p.IsSelected);
                            if (seqfirstscore.points == 0)
                            {
                                baselObtained = false;
                            }
                            else
                            {
                                var basalCount = 0;
                                foreach (var item in SequnetialScoredItems)
                                {
                                    var score = item.ScoringValues.FirstOrDefault(p => p.IsSelected);
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
                            var score = item.ScoringValues.FirstOrDefault(p => p.IsSelected);
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
                    if (baselObtained && ceilingObtainedItem != null && hasSequenceScored)
                    {
                        ceilingObtained = true;
                    }
                    if (baselObtained && !ceilingObtained && hasSequenceScored)
                    {
                        var allItems = selectedAcademicContentItem.LastOrDefault(p => p.AcademicItemModel != null);
                        var lastItem = allItems.AcademicItemModel.LastOrDefault();
                        if (lastItem != null && lastItem.ScoringValues != null)
                        {
                            var score = lastItem.ScoringValues.FirstOrDefault(p => p.IsSelected);
                            if (score != null)
                            {
                                ceilingObtainedItem = lastItem;
                                ceilingObtained = true;
                            }
                        }
                    }
                    BaselObtained = baselObtained;
                    BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                    CeilingObtained = baselObtained && ceilingObtained;
                    CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";
                    CurrentAcademicContentModel.BasalCeilingObtained = false;
                    if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                    {
                        foreach (var item in selectedAcademicContentItem)
                        {
                            item.rawScore = null;
                            item.BasalCeilingObtained = BaselObtained && CeilingObtained;
                        }
                    }
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScoreEnabled = false;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScore = null;
                    commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).BaselCeilingReached = BaselObtained && CeilingObtained;
                    CurrentAcademicContentModel.rawScore = rawscore;
                    if (BaselObtained && CeilingObtained)
                    {
                        if (!rawscore.HasValue)
                        {
                            rawscore = 0;
                        }
                        var baselObtainedIndex = 0;
                        var ceilingObtainedIndex = 0;
                        var academicItems = selectedAcademicContentItem.Where(p => p.AcademicItemModel != null);
                        var domainContentCollection = new List<AcademicItemModel>();
                        foreach (var item in academicItems)
                        {
                            domainContentCollection.AddRange(item.AcademicItemModel);
                        }
                        baselObtainedIndex = domainContentCollection.IndexOf(baselObtainedItem);
                        ceilingObtainedIndex = domainContentCollection.IndexOf(ceilingObtainedItem);
                        for (int i = baselObtainedIndex; i <= ceilingObtainedIndex; i++)
                        {
                            var item = domainContentCollection[i];
                            var score = item.ScoringValues.FirstOrDefault(p => p.IsSelected);
                            rawscore += score.points;
                        }
                        if (firstItemScore == -1)
                        {
                            if (!rawscore.HasValue)
                            {
                                rawscore = 0;
                            }
                            var firstSequenceItem = SequnetialScoredItems.FirstOrDefault();
                            var index = domainContentCollection.IndexOf(firstSequenceItem);
                            rawscore += index * baselScore;
                        }
                        if (!rawscore.HasValue)
                        {
                            rawscore = 0;
                        }
                        if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                        {
                            foreach (var item in selectedAcademicContentItem)
                            {
                                if (item.AcademicItemModel != null && item.AcademicItemModel.Any())
                                {
                                    var selectedTally = item.AcademicItemModel.Where(p => p.TallyContent != null);
                                    if (selectedTally != null && selectedTally.Any())
                                    {
                                        foreach (var tallyitem in selectedTally)
                                        {
                                            var scoreitem = tallyitem.TallyContent.Where(p => p.IsCorrectChecked);
                                            if (scoreitem != null && scoreitem.Any())
                                            {
                                                rawscore += commonDataService.ContentItemTallyScores.Where(p => p.contentItemId == scoreitem.FirstOrDefault().contentItemId).FirstOrDefault(p => p.totalPoints == scoreitem.Count())?.score ?? scoreitem.Count();
                                            }
                                        }
                                    }
                                }
                                item.BasalCeilingObtained = true;
                            }
                        }
                        CurrentAcademicContentModel.BasalCeilingObtained = true;
                        if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                        {
                            foreach (var item in selectedAcademicContentItem)
                            {
                                item.rawScore = rawscore;
                            }
                        }
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScoreEnabled = false;
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScore = rawscore;
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).BaselCeilingReached = true;
                        if (!InitialLoad && !baselceilingReached && BaselObtained && CeilingObtained)
                        {
                            CeilingObtainedPopupView();
                        }
                    }
                }
                else
                {
                    BaselCeilingApplicable = false;
                    if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                    {
                        if (commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).rawScoreEnabled)
                        {
                            BaselObtained = false;
                            BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                            CeilingObtained = false;
                            CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";

                            foreach (var item in selectedAcademicContentItem)
                            {
                                item.BasalCeilingObtained = true;
                            }
                            return;
                        }
                        if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                        {
                            foreach (var academicItem in selectedAcademicContentItem)
                            {
                                if (academicItem.AcademicItemModel != null && academicItem.AcademicItemModel.Any())
                                {
                                    var selectedScoring = academicItem.AcademicItemModel.Where(p => p.ScoringValues != null && p.ScoringValues.Any());
                                    if (selectedScoring != null && selectedScoring.Any())
                                    {
                                        foreach (var item in selectedScoring)
                                        {
                                            var scoreitem = item.TallyContent.Where(p => p.IsCorrectChecked);
                                            if (scoreitem != null && scoreitem.Any())
                                            {
                                                foreach (var tally in scoreitem)
                                                {
                                                    if (!rawscore.HasValue)
                                                    {
                                                        rawscore = 0;
                                                    }
                                                    rawscore += 1;
                                                }
                                            }
                                        }
                                    }
                                    if (!rawscore.HasValue)
                                    {
                                        rawscore = 0;
                                    }
                                    var selectedTally = academicItem.AcademicItemModel.Where(p => p.TallyContent != null && p.TallyContent.Any());
                                    if (selectedTally != null && selectedTally.Any())
                                    {
                                        foreach (var item in selectedTally)
                                        {
                                            var scoreitem = item.TallyContent.Where(p => p.IsCorrectChecked);
                                            if (scoreitem != null && scoreitem.Any())
                                            {
                                                rawscore += commonDataService.ContentItemTallyScores.Where(p => p.contentItemId == scoreitem.FirstOrDefault().contentItemId).FirstOrDefault(p => p.totalPoints == scoreitem.Count())?.score ?? scoreitem.Count();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (selectedAcademicContentItem != null && selectedAcademicContentItem.Any())
                        {
                            foreach (var academicItem in selectedAcademicContentItem)
                            {
                                academicItem.rawScore = rawscore;
                                academicItem.BasalCeilingObtained = true;
                            }
                        }
                        CurrentAcademicContentModel.rawScore = rawscore;
                        CurrentAcademicContentModel.BasalCeilingObtained = true;
                        commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId != 0 ? CurrentAcademicContentModel.AreaId : CurrentAcademicContentModel.SubDomainCategoryId)).BaselCeilingReached = true;
                    }
                }
            }
        }
        private void CheckScoreSelected()
        {
            var selectedAcademicContentItem = default(List<AcademicContentModel>);
            if (CurrentAcademicContentModel.AreaId == 0)
            {
                selectedAcademicContentItem = new List<AcademicContentModel>(AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == CurrentAcademicContentModel.SubDomainCategoryId));
            }
            else
            {
                selectedAcademicContentItem = new List<AcademicContentModel>(AcademicContentModelCollection.Where(p => p.AreaId == CurrentAcademicContentModel.AreaId));
            }
            if (selectedAcademicContentItem != null)
            {
                var selectedSubDomain = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == (CurrentAcademicContentModel.AreaId == 0 ? CurrentAcademicContentModel.SubDomainCategoryId : CurrentAcademicContentModel.AreaId));
                selectedSubDomain.rawScoreEnabled = true;
                var scores = selectedAcademicContentItem.Where(p => p.AcademicItemModel != null && p.AcademicItemModel.Any(q => q.ScoringValues != null));
                if (scores != null && scores.Any(p => p.AcademicItemModel.Any(q => q.ScoringValues.Any(r => r.IsSelected))))
                {
                    if (selectedSubDomain != null)
                    {
                        selectedSubDomain.rawScore = null;
                        selectedSubDomain.BaselCeilingReached = false;
                        selectedSubDomain.rawScoreEnabled = false;
                        selectedSubDomain.TSOStatus = "In-Progress";
                        selectedSubDomain.IsScoreSelected = true;
                        foreach (var item in selectedAcademicContentItem)
                        {
                            item.rawScore = null;
                            item.BasalCeilingObtained = false;
                        }
                        CurrentAcademicContentModel.rawScore = null;
                        CurrentAcademicContentModel.BasalCeilingObtained = false;
                    }
                }
                else
                {
                    if (!selectedSubDomain.rawScoreEnabled)
                    {
                        BaselObtained = false;
                        CeilingObtained = false;
                        BasalImage = BaselObtained ? "completed_TickMark.png" : "notStarted.png";
                        CeilingImage = CeilingObtained ? "ceiling_obtained.png" : "notStarted.png";

                        CurrentAcademicContentModel.rawScore = null;
                        CurrentAcademicContentModel.BasalCeilingObtained = false;

                        selectedSubDomain.rawScore = null;
                        selectedSubDomain.BaselCeilingReached = false;
                        selectedSubDomain.rawScoreEnabled = true;
                        selectedSubDomain.TSOStatus = "Not Started";
                        selectedSubDomain.IsScoreSelected = false;
                        foreach (var item in selectedAcademicContentItem)
                        {
                            item.rawScore = null;
                            item.BasalCeilingObtained = false;
                        }
                    }
                    else if (selectedSubDomain.rawScore.HasValue)
                    {
                        selectedSubDomain.IsScoreSelected = true;
                        selectedSubDomain.TSOStatus = "In-Progress";
                    }
                }
            }
        }
        #endregion

        #region DomainCodeProperties
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
                    else if (CurrentAcademicContentModel != null)
                    {
                        if (model.IsItemLevelNote)
                        {
                            CurrentAcademicContentModel.AcademicItemModel.FirstOrDefault(p => p.IsSelected).Notes = model.Notes;
                        }
                        else
                        {
                            HasDoneChanges = true;
                            CurrentAcademicContentModel.SubDomainNote = model.Notes;
                        }
                    }
                });
            }
        }
        public string DomainCode { get; set; }
        public string SubDoaminCode { get; set; }
        public string AreaCode { get; set; }
        #endregion

        #region TestFormSave
        private void TestFormSave()
        {
            bool hasScored = false;
            int addedBy;
            int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
            var lstStudentTestFormResponses = new List<StudentTestFormResponses>();
            commonDataService.StudentTestFormOverview.IsFormSaved = true;
            commonDataService.StudentTestFormOverview.notes = FormNotes;
            commonDataService.StudentTestFormOverview.FormStatus = "Not started";
            if (commonDataService.TotalCategories != null && commonDataService.TotalCategories.Any())
            {
                foreach (var item in commonDataService.StudentTestForms)
                {
                    item.rawScore = AcademicContentModelCollection.Where(p => !p.IsSampleItem).FirstOrDefault(p => p.SubDomainCategoryId == item.contentCategoryId || p.AreaId == item.contentCategoryId).rawScore;
                }
                if (commonDataService.StudentTestForms.Any(p => p.rawScore.HasValue))
                {
                    commonDataService.StudentTestFormOverview.FormStatus = "Saved";
                }
                var levelCategories = commonDataService.TotalCategories.Where(p => p.contentCategoryLevelId == 7 || p.contentCategoryLevelId == 8).ToList();
                if (levelCategories != null && levelCategories.Any())
                {
                    foreach (var levelCategoroy in levelCategories)
                    {
                        if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                        {
                            var lstFormJsonClass = new List<FormJsonClass>();
                            var startingPoint = StartingPointCategoryCollection.Where(p => p.parentContentCategoryId == levelCategoroy.contentCategoryId);
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
                                            foreach (var itemcategory in AcademicContentModelCollection)
                                            {
                                                if (itemcategory.AcademicItemModel != null && itemcategory.AcademicItemModel.Any())
                                                {
                                                    foreach (var contentitem in itemcategory.AcademicItemModel)
                                                    {
                                                        if (contentitem.ContentItemId == startcontentitem.contentItemId)
                                                        {
                                                            var itemInfo = new ItemInfo();
                                                            section.items.Add(itemInfo);
                                                            itemInfo.itemId = contentitem.ContentItemId;
                                                            itemInfo.itemNotes = contentitem.Notes;
                                                            if (contentitem.ScoringValues != null && contentitem.ScoringValues.Any())
                                                            {
                                                                var selectedScoring = contentitem.ScoringValues.FirstOrDefault(p => p.IsSelected);
                                                                if (selectedScoring != null)
                                                                {
                                                                    hasScored = true;
                                                                    itemInfo.ContentRubricPointId = selectedScoring.contentRubricPointsId;
                                                                    itemInfo.itemScore = selectedScoring.points;
                                                                }
                                                            }
                                                            if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                                            {
                                                                var answeredTally = contentitem.TallyContent.Where(p => p.IsCorrectChecked || p.IsInCorrectChecked);
                                                                if (answeredTally != null && answeredTally.Any())
                                                                {
                                                                    hasScored = true;
                                                                    itemInfo.tallyItems = new List<TallyItemInfo>();
                                                                    foreach (var tallyItem in answeredTally)
                                                                    {
                                                                        var TallyItemInfo = new TallyItemInfo();
                                                                        TallyItemInfo.itemTallyId = tallyItem.contentItemTallyId;
                                                                        TallyItemInfo.tallyScore = tallyItem.IsCorrectChecked ? 1 : 0;
                                                                        itemInfo.tallyItems.Add(TallyItemInfo);
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
                            }
                            var StudentTestFormResponses = new StudentTestFormResponses();
                            StudentTestFormResponses.ContentCategoryId = levelCategoroy.contentCategoryId;
                            StudentTestFormResponses.LocalFormInstanceId = LocaInstanceID;
                            StudentTestFormResponses.Response = JsonConvert.SerializeObject(lstFormJsonClass);
                            StudentTestFormResponses.CreatedBy = addedBy;
                            StudentTestFormResponses.CreatedOn = DateTime.Now;
                            lstStudentTestFormResponses.Add(StudentTestFormResponses);
                        }
                    }
                }
            }

            if (commonDataService.StudentTestForms.Any(p => p.BaselCeilingReached && p.IsBaselCeilingApplied) || commonDataService.StudentTestForms.Any(p => p.rawScore.HasValue))
            {
                commonDataService.StudentTestFormOverview.FormStatus = "Saved";
            }
            else
            {
                commonDataService.StudentTestFormOverview.FormStatus = hasScored ? "In-Progress" : "Not started";
            }
            studentTestFormsService.UpdateAll(commonDataService.StudentTestForms);
            OriginalStudentTestForms = new List<StudentTestForms>(commonDataService.StudentTestForms);
            clinicalTestFormService.UpdateTestForm(commonDataService.StudentTestFormOverview);
            OriginalStudentTestFormOverView = JsonConvert.DeserializeObject<StudentTestFormOverview>(JsonConvert.SerializeObject(commonDataService.StudentTestFormOverview));
            if (lstStudentTestFormResponses != null && lstStudentTestFormResponses.Any())
            {
                _studentTestFormResponsesService.DeleteAll(LocaInstanceID);
                _studentTestFormResponsesService.InsertAll(lstStudentTestFormResponses);
            }
            else
            {
                _studentTestFormResponsesService.DeleteAll(LocaInstanceID);
            }
        }
        #endregion

        #region Reset Region
        public bool InitialLoad { get; set; }
        public bool HasDoneChanges { get; set; }
        public void ResetScroes(List<int> selectedsubDomains)
        {
            if (selectedsubDomains != null && selectedsubDomains.Any())
            {
                if (selectedsubDomains.Any(p => p == CurrentAcademicContentModel.SubDomainCategoryId))
                {
                    LastModel = null;
                }
                foreach (var item in selectedsubDomains)
                {
                    if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                    {
                        var contentCategories = AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == item);
                        if (contentCategories != null && contentCategories.Any())
                        {
                            foreach (var inneritem in contentCategories)
                            {
                                if (inneritem.AcademicItemModel != null && inneritem.AcademicItemModel.Any())
                                {
                                    foreach (var contentitem in inneritem.AcademicItemModel)
                                    {
                                        if (contentitem.ScoringValues != null && contentitem.ScoringValues.Any())
                                        {
                                            foreach (var scoring in contentitem.ScoringValues)
                                            {
                                                if (scoring.IsSelected)
                                                {
                                                    scoring.IsSelected = false;
                                                    HasDoneChanges = true;
                                                }

                                            }
                                        }
                                        if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                        {
                                            var answeredTally = contentitem.TallyContent.Where(p => p.IsCorrectChecked || p.IsInCorrectChecked);
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
                                                    tallyItem.IsSelected = false;
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
                    if (AcademicContentModelCollection != null && AcademicContentModelCollection.Any())
                    {
                        var contentCategories = AcademicContentModelCollection.Where(p => p.AreaId == item);
                        if (contentCategories != null && contentCategories.Any())
                        {
                            foreach (var inneritem in contentCategories)
                            {
                                if (inneritem.AcademicItemModel != null && inneritem.AcademicItemModel.Any())
                                {
                                    foreach (var contentitem in inneritem.AcademicItemModel)
                                    {
                                        if (contentitem.ScoringValues != null && contentitem.ScoringValues.Any())
                                        {
                                            foreach (var scoring in contentitem.ScoringValues)
                                            {
                                                scoring.IsSelected = false;
                                            }
                                        }
                                        if (contentitem.TallyContent != null && contentitem.TallyContent.Any())
                                        {
                                            var answeredTally = contentitem.TallyContent.Where(p => p.IsCorrectChecked || p.IsInCorrectChecked);
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
                    var testRecord = commonDataService.StudentTestForms.FirstOrDefault(p => p.contentCategoryId == item);
                    if (testRecord != null)
                    {
                        testRecord.BaselCeilingReached = false;
                        testRecord.rawScore = null;
                        testRecord.rawScoreEnabled = true;
                        testRecord.TSOStatus = "Not Started";
                        testRecord.IsScoreSelected = false;
                        if (testRecord.contentCategoryId == 162)
                        {
                            testRecord.TimeTaken = 0;
                            TotalSeconds = new TimeSpan(0, 0, 0, 210);
                            ResetEnabled = true;
                            StartEnabled = false;
                            TimerButtonBckgrd = Color.LightGray;
                            TimerReset = "iconrefreshblue.png";
                        }
                    }

                    var items = AcademicContentModelCollection.Where(p => p.SubDomainCategoryId == item || p.AreaId == item);
                    if (items != null && items.Any())
                    {
                        foreach (var resetitem in items)
                        {
                            resetitem.rawScore = null;
                            resetitem.BasalCeilingObtained = false;
                        }
                    }

                    if (CurrentAcademicContentModel.SubDomainCategoryId == item || CurrentAcademicContentModel.AreaId == item)
                    {
                        CurrentAcademicContentModel.rawScore = null;
                        CurrentAcademicContentModel.BasalCeilingObtained = false;
                    }
                }
                CalculateBaselCeiling();
            }
        }
        #endregion

        public Action ClearContent { get; set; }
        public void Dispose()
        {
            //GC.Collect();
        }
        private AcademicTemplateSelectorModel templateSelectorModel;
        public AcademicTemplateSelectorModel TemplateModel
        {
            get
            {
                return templateSelectorModel;
            }
            set
            {
                templateSelectorModel = value;
                OnPropertyChanged(nameof(TemplateModel));
            }
        }
    }

    public class AcademicTemplateSelectorModel : BindableObject, INotifyPropertyChanged
    {
        private string imageFilePath;
        public string ImageFilePath
        {
            get
            {
                return imageFilePath;
            }
            set
            {
                imageFilePath = value;
                OnPropertyChanged(nameof(ImageFilePath));
            }
        }

        private bool destroyWebView;
        public bool DestroyWebView
        {
            get
            {
                return destroyWebView;
            }
            set
            {
                destroyWebView = value;
                OnPropertyChanged(nameof(DestroyWebView));
            }
        }
        public Action<AcademicScoringModel> ContentRubicPointSelectionAction { get; set; }
        public ICommand ItemClickCommand { get; set; }
        public ICommand ContentRubicPointSelection
        {
            get
            {
                return new Command(() =>
                {
                    ContentRubicPointSelectionAction?.Invoke(null);
                });
            }
        }
        AcademicContentModel CurrentAcademicContentModel { get; set; }
        public CurrentAcademiTemplate CurrentAcademiTemplate { get; set; }
        private string notesDescriptionPath;
        public string NotesDescriptionPath
        {
            get
            {
                return notesDescriptionPath;
            }
            set
            {
                notesDescriptionPath = value;
                OnPropertyChanged(nameof(NotesDescriptionPath));
            }
        }
        private string notesDescription;
        public string NotesDescription
        {
            get { return notesDescription; }
            set
            {
                notesDescription = value;
                OnPropertyChanged(nameof(NotesDescription));
            }
        }
        private bool isScoringNoteVisible;
        public bool IsScoringNotesVisible
        {
            get
            {
                return isScoringNoteVisible;
            }
            set
            {
                isScoringNoteVisible = value;
                OnPropertyChanged(nameof(IsScoringNotesVisible));
            }
        }
        private string instructionHeader;
        public string InstructionHeader
        {
            get
            {
                return instructionHeader;
            }
            set
            {
                instructionHeader = value;
                OnPropertyChanged(nameof(InstructionHeader));
            }
        }
        private string instructionDescFilePath;
        public string InstructionDescFilePath
        {
            get
            {
                return instructionDescFilePath;
            }
            set
            {
                instructionDescFilePath = value;
                OnPropertyChanged(nameof(InstructionDescFilePath));
            }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private string instructionImageScoreDescFilePath;
        public string InstructionImageScoreDescFilePath
        {
            get
            {
                return instructionImageScoreDescFilePath;
            }
            set
            {
                instructionImageScoreDescFilePath = value;
                OnPropertyChanged(nameof(InstructionImageScoreDescFilePath));
            }
        }
        private string instructionImageScoreDescription;
        public string InstructionImageScoreDescription
        {
            get
            {
                return instructionImageScoreDescription;
            }
            set
            {
                instructionImageScoreDescription = value;
                OnPropertyChanged(nameof(InstructionImageScoreDescription));
            }
        }
        private string imageSampleGridDescFilePath;
        public string ImageSampleGridDescFilePath
        {
            get
            {
                return imageSampleGridDescFilePath;
            }
            set
            {
                imageSampleGridDescFilePath = value;
                OnPropertyChanged(nameof(ImageSampleGridDescFilePath));
            }
        }
        private string imageSampleGridDescription;
        public string ImageSampleGridDescription
        {
            get
            {
                return imageSampleGridDescription;
            }
            set
            {
                imageSampleGridDescription = value;
                OnPropertyChanged(nameof(ImageSampleGridDescription));
            }
        }
        private bool timerVisibilty;
        public bool TimerVisibilty
        {
            get { return timerVisibilty; }
            set { timerVisibilty = value; OnPropertyChanged(nameof(TimerVisibilty)); }
        }
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
        private string materialText;
        public string MaterialText
        {
            get { return materialText; }
            set
            {
                materialText = value;
                OnPropertyChanged(nameof(MaterialText));
            }
        }
        private string materialFilePath;
        public string MaterialFilePath
        {
            get { return materialFilePath; }
            set { materialFilePath = value; OnPropertyChanged(nameof(MaterialFilePath)); }
        }
        private List<AcademicItemModel> itemsCollection;

        public List<AcademicItemModel> ItemsCollection
        {
            get { return itemsCollection; }
            set { itemsCollection = value; OnPropertyChanged(nameof(ItemsCollection)); }
        }
        private string itemdescription;
        public string ItemDescription
        {
            get { return itemdescription; }
            set
            {
                itemdescription = value;
                OnPropertyChanged(nameof(ItemDescription));
            }
        }
        private string itemdescriptionFilePath;
        public string ItemdescriptionFilePath
        {
            get { return itemdescriptionFilePath; }
            set
            {
                itemdescriptionFilePath = value;
                OnPropertyChanged(nameof(ItemdescriptionFilePath));
            }
        }
        private string scoringNotesDescription;
        public string ScoringNotesDescription
        {
            get { return scoringNotesDescription; }
            set
            {
                scoringNotesDescription = value;
                OnPropertyChanged(nameof(ScoringNotesDescription));
            }
        }
        private List<ContentItemTally> tallyCollection;

        public List<ContentItemTally> TallyCollection
        {
            get { return tallyCollection; }
            set { tallyCollection = value; OnPropertyChanged(nameof(TallyCollection)); }
        }
        private bool isTallyVisible = true;
        public bool IsTallyVisible
        {
            get { return isTallyVisible; }
            set { isTallyVisible = value; OnPropertyChanged(nameof(IsTallyVisible)); }
        }
        private long fluencyItemDescriptionHeight;
        public long FluencyItemDescriptionHeight
        {
            get { return fluencyItemDescriptionHeight; }
            set
            {
                fluencyItemDescriptionHeight = value;
                OnPropertyChanged(nameof(FluencyItemDescriptionHeight));
            }

        }
        private List<AcademicScoringModel> scoringCollection;
        public List<AcademicScoringModel> ScoringCollection
        {
            get { return scoringCollection; }
            set { scoringCollection = value; OnPropertyChanged(nameof(ScoringCollection)); }
        }
        private bool isScoringVisible = true;
        public bool IsScoringVisible
        {
            get { return isScoringVisible; }
            set { isScoringVisible = value; OnPropertyChanged(nameof(IsScoringVisible)); }
        }
        private string grouptitle;
        public string GroupTitle
        {
            get
            {
                return grouptitle;
            }
            set
            {
                grouptitle = value;
                OnPropertyChanged(nameof(GroupTitle));
            }
        }
        private string descriptionFilePath;
        public string DescriptionFilePath
        {
            get { return descriptionFilePath; }
            set
            {
                descriptionFilePath = value;
                OnPropertyChanged(nameof(DescriptionFilePath));
            }
        }
        private int maxTime = 210;
        public int MaxTime
        {
            get { return maxTime; }
            set
            {
                maxTime = value;
                OnPropertyChanged(nameof(MaxTime));
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

        public bool IsItemdescriptionVisible
        {
            get
            {
                return string.IsNullOrEmpty(ItemdescriptionFilePath);
            }
        }
    }
}
