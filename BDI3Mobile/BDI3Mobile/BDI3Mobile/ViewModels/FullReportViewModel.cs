using Acr.UserDialogs;
using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace BDI3Mobile.ViewModels
{
    public class FullReportViewModel : BindableObject
    {
        #region Properties
        private List<ReportType> reportTypeCollection = new List<ReportType>();
        public List<ReportType> ReportTypeList
        {
            get
            {
                return reportTypeCollection;
            }

            set
            {
                reportTypeCollection = value;
                OnPropertyChanged(nameof(ReportTypeList));
            }
        }
        private List<ProgramLabels> programLabelTypeCollection = new List<ProgramLabels>();
        public List<ProgramLabels> ProgramLabelList
        {
            get
            {
                return programLabelTypeCollection;
            }

            set
            {
                programLabelTypeCollection = value;
                OnPropertyChanged(nameof(ProgramLabelList));
            }
        }
        private List<ScoreType> scoreTypeCollection = new List<ScoreType>();

        public List<ScoreType> ScoreTypeList
        {
            get
            {
                return scoreTypeCollection;
            }

            set
            {
                scoreTypeCollection = value;
                OnPropertyChanged(nameof(ScoreTypeList));
            }
        }
        private List<NotesType> noteTypeCollection = new List<NotesType>();
        public List<NotesType> NoteTypeList
        {
            get
            {
                return noteTypeCollection;
            }

            set
            {
                noteTypeCollection = value;
                OnPropertyChanged(nameof(NoteTypeList));
            }
        }
        private List<StandardDeviation> standardDeviationCollection = new List<StandardDeviation>();
        public List<StandardDeviation> StandardDeviationList
        {
            get
            {
                return standardDeviationCollection;
            }

            set
            {
                standardDeviationCollection = value;
                OnPropertyChanged(nameof(StandardDeviationList));
            }
        }
        private List<OutputFormatType> outputFormatTypeCollection = new List<OutputFormatType>();
        public List<OutputFormatType> OutputFormatTypeList
        {
            get
            {
                return outputFormatTypeCollection;
            }

            set
            {
                outputFormatTypeCollection = value;
                OnPropertyChanged(nameof(OutputFormatTypeList));
            }
        }
        private List<string> _selectedLocations;
        public List<string> SelectedLocations
        {
            get { return _selectedLocations; }
            set
            {
                _selectedLocations = value;
                OnPropertyChanged(nameof(SelectedLocations));
            }
        }
        public bool isLocationPopupOpen { get; set; }
        public bool isChildRecordPopupOpen { get; set; }
        public bool isBatteryTypePopupOpen { get; set; }
        public bool isRecordFormPopupOpen { get; set; }
        public bool isReportTypePopupOpen { get; set; }
        public bool isNotesTypePopupOpen { get; set; }
        public bool isScoreTypePopupOpen { get; set; }
        public bool isProgramLabelPopupOpen { get; set; }
        public bool isOutputFormatPopupOpen { get; set; }
        public bool isStandardDeviationPopupOpen { get; set; }
        public bool isItemNotesSelected { get; set; }
        private int _selectedReportTypeID;
        public int SelectedReportTypeID
        {
            get { return _selectedReportTypeID; }
            set
            {
                _selectedReportTypeID = value;
                OnPropertyChanged(nameof(SelectedReportTypeID));
            }
        }
        private string _selectedReportTypeName;
        public string SelectedReportTypeName
        {
            get { return _selectedReportTypeName; }
            set
            {
                _selectedReportTypeName = value;
                OnPropertyChanged(nameof(SelectedReportTypeName));
            }
        }
        private string _selectedOutputFormatType;
        public string SelectedOutputFormatType
        {
            get { return _selectedOutputFormatType; }
            set
            {
                _selectedOutputFormatType = value;
                OnPropertyChanged(nameof(SelectedOutputFormatType));
            }
        }

        private int _outputFormatTypeID;
        public int OutputFormatTypeID
        {
            get { return _outputFormatTypeID; }
            set
            {
                _outputFormatTypeID = value;
                OnPropertyChanged(nameof(OutputFormatTypeID));
            }
        }
        private List<string> _selectedScoreTypes;
        public List<string> SelectedScoreTypes
        {
            get { return _selectedScoreTypes; }
            set
            {
                _selectedScoreTypes = value;
                OnPropertyChanged(nameof(SelectedScoreTypes));
            }
        }
        private double _selectedStandardDeviationValue;
        public double SelectedStandardDeviationValue
        {
            get { return _selectedStandardDeviationValue; }
            set
            {
                _selectedStandardDeviationValue = value;
                OnPropertyChanged(nameof(SelectedStandardDeviationValue));
            }
        }
        private string _standardDeviationValue;
        public string StandardDeviationValue
        {
            get { return _standardDeviationValue; }
            set
            {
                _standardDeviationValue = value;
                OnPropertyChanged(nameof(StandardDeviationValue));
            }
        }
        private List<string> _selectedNoteTypes;
        public List<string> SelectedNoteTypes
        {
            get { return _selectedNoteTypes; }
            set
            {
                _selectedNoteTypes = value;
                OnPropertyChanged(nameof(SelectedNoteTypes));
            }
        }
        private string _selectedProgramLabelName;
        public string SelectedProgramLabelName
        {
            get { return _selectedProgramLabelName; }
            set
            {
                _selectedProgramLabelName = value;
                OnPropertyChanged(nameof(SelectedProgramLabelName));
            }
        }
        private List<string> _selectedLocationsReport;
        public List<string> SelectedLocationsReport
        {
            get { return _selectedLocationsReport; }
            set
            {
                _selectedLocationsReport = value;
                OnPropertyChanged(nameof(SelectedLocationsReport));
            }
        }
        private int _selectedProgramLabelID;
        public int SelectedProgramLabelID
        {
            get { return _selectedProgramLabelID; }
            set
            {
                _selectedProgramLabelID = value;
                OnPropertyChanged(nameof(SelectedProgramLabelID));
            }
        }
        private List<ReportLocations> AllLocations;
        private List<ReportLocations> locations;
        public List<ReportLocations> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                locations = value;
                OnPropertyChanged(nameof(Locations));

            }
        }
        List<ChildRecord> _childRecords = new List<ChildRecord>();
        public List<ChildRecord> ChildRecords
        {
            get
            {
                return _childRecords;
            }

            set
            {
                _childRecords = value;
                OnPropertyChanged(nameof(ChildRecords));
            }
        }
        List<BatteryTypes> _batteryTypes = new List<BatteryTypes>();
        public List<BatteryTypes> BatteryTypeList
        {
            get
            {
                return _batteryTypes;
            }

            set
            {
                _batteryTypes = value;
                OnPropertyChanged(nameof(BatteryTypeList));
            }
        }
        private ObservableCollection<LocationNew> locationsObservableCollection;
        public ObservableCollection<LocationNew> LocationsObservableCollection
        {
            get
            {
                return locationsObservableCollection;
            }
            set
            {
                locationsObservableCollection = value;
                OnPropertyChanged(nameof(LocationsObservableCollection));

            }
        }
        private string _locationSeleted;
        public string LocationsSelected
        {
            get { return _locationSeleted; }
            set
            {
                _locationSeleted = value;
                OnPropertyChanged(nameof(LocationsSelected));
                if (value == null)
                    LocationsSelected = "Select location(s)";
            }
        }
        private string _selectedChild;
        public string SelectedChildID
        {
            get { return _selectedChild; }
            set
            {
                _selectedChild = value;
                OnPropertyChanged(nameof(SelectedChildID));
                if (value == null)
                    SelectedChildName = "Select a child";
            }
        }

        private string _selectedChildName;
        public string SelectedChildName
        {
            get { return _selectedChildName; }
            set
            {
                _selectedChildName = value;
                OnPropertyChanged(nameof(SelectedChildName));
            }
        }
        private string _selectedRecordForm;
        public string SelectedRecordForm
        {
            get { return _selectedRecordForm; }
            set
            {
                _selectedRecordForm = value;
                OnPropertyChanged(nameof(SelectedRecordForm));
            }
        }
        private string _selectedRecordFormID;
        public string SelectedRecordFormID
        {
            get { return _selectedRecordFormID; }
            set
            {
                _selectedRecordFormID = value;
                OnPropertyChanged(nameof(SelectedRecordFormID));
                if (value == null)
                    SelectedRecordForm = "Select record form";
            }
        }
        private int _selectedAssessmentID;
        public int SelectedAssessmentID
        {
            get { return _selectedAssessmentID; }
            set
            {
                _selectedAssessmentID = value;
                OnPropertyChanged(nameof(SelectedAssessmentID));
                if (value == 0)
                    SelectedAssessmentType = "Select battery type";
            }
        }
        private string _selectedAssessmentType;
        public string SelectedAssessmentType
        {
            get { return _selectedAssessmentType; }
            set
            {
                _selectedAssessmentType = value;
                OnPropertyChanged(nameof(SelectedAssessmentType));
            }
        }
        List<RecordForms> _childTestRecordsRecords = new List<RecordForms>();
        public List<RecordForms> ChildTestRecords
        {
            get
            {
                return _childTestRecordsRecords;
            }

            set
            {
                _childTestRecordsRecords = value;
                OnPropertyChanged(nameof(ChildTestRecords));
            }
        }
        private bool _isScoringLayoutVisible;
        public bool IsScoringLayoutVisible
        {
            get { return _isScoringLayoutVisible; }
            set
            {
                _isScoringLayoutVisible = value;
                OnPropertyChanged(nameof(IsScoringLayoutVisible));
            }
        }

        private bool _isBAESLayoutVisible;
        public bool IsBAESLayoutVisible
        {
            get { return _isBAESLayoutVisible; }
            set
            {
                _isBAESLayoutVisible = value;
                OnPropertyChanged(nameof(IsBAESLayoutVisible));
            }
        }
        private bool _isStandardDeviationLayoutVisible;
        public bool IsStandardDeviationLayoutVisible
        {
            get { return _isStandardDeviationLayoutVisible; }
            set
            {
                _isStandardDeviationLayoutVisible = value;
                OnPropertyChanged(nameof(IsStandardDeviationLayoutVisible));
            }
        }
        private bool _isChildPopUpEnabled;
        public bool IsChildPopUpEnabled
        {
            get { return _isChildPopUpEnabled; }
            set
            {
                _isChildPopUpEnabled = value;
                OnPropertyChanged(nameof(IsChildPopUpEnabled));
            }
        }
        private bool _isBatteryTypePopupEnabled;
        public bool IsBatteryTypePopupEnabled
        {
            get { return _isBatteryTypePopupEnabled; }
            set
            {
                _isBatteryTypePopupEnabled = value;
                OnPropertyChanged(nameof(IsBatteryTypePopupEnabled));
            }
        }
        private bool _isRecordTypePopupEnabled;
        public bool IsRecordFormButtonEnabled
        {
            get { return _isRecordTypePopupEnabled; }
            set
            {
                _isRecordTypePopupEnabled = value;
                OnPropertyChanged(nameof(IsRecordFormButtonEnabled));
            }
        }
        public int selectedCount { get; set; }
        public int selectedNoteCount { get; set; }
        public int selectedScoreCount { get; set; }
        private bool _isStandardDeviationPopUpEnabled;
        public bool IsStandardDeviationPopUpEnabled
        {
            get { return _isStandardDeviationPopUpEnabled; }
            set
            {
                _isStandardDeviationPopUpEnabled = value;
                OnPropertyChanged(nameof(IsStandardDeviationPopUpEnabled));
            }
        }
        private bool _isScoresPopUpEnabled;
        public bool IsScoresPopUpEnabled
        {
            get { return _isScoresPopUpEnabled; }
            set
            {
                _isScoresPopUpEnabled = value;
                OnPropertyChanged(nameof(IsScoresPopUpEnabled));
            }
        }
        private bool _isNotesPopUpEnabled;
        public bool IsNotesPopUpEnabled
        {
            get { return _isNotesPopUpEnabled; }
            set
            {
                _isNotesPopUpEnabled = value;
                OnPropertyChanged(nameof(IsNotesPopUpEnabled));
            }
        }
        private bool _isScoresCheckboxCheckedChanged;
        public bool IsScoresCheckboxCheckedChanged
        {
            get { return _isScoresCheckboxCheckedChanged; }
            set
            {
                _isScoresCheckboxCheckedChanged = value;
                OnPropertyChanged(nameof(IsScoresCheckboxCheckedChanged));
                SetNotesData(SelectedAssessmentID);
            }
        }
        private bool _isSDScoresCheckboxCheckedChanged;
        public bool IsSDScoresCheckboxCheckedChanged
        {
            get { return _isSDScoresCheckboxCheckedChanged; }
            set
            {
                _isSDScoresCheckboxCheckedChanged = value;
                OnPropertyChanged(nameof(IsSDScoresCheckboxCheckedChanged));
                if (value == true && SelectedAssessmentID != 0)
                    SetNotesData(SelectedAssessmentID);
                else
                    SetNotesData();
            }
        }

        private bool _issActivitiesCheckboxCheckedChanged;
        public bool IsActivitiesCheckboxCheckedChanged
        {
            get { return _issActivitiesCheckboxCheckedChanged; }
            set
            {
                _issActivitiesCheckboxCheckedChanged = value;
                OnPropertyChanged(nameof(IsActivitiesCheckboxCheckedChanged));
            }
        }
        private bool _isDomainCheckboxCheckedChanged;
        public bool IsDomainCheckboxCheckedChanged
        {
            get { return _isDomainCheckboxCheckedChanged; }
            set
            {
                _isDomainCheckboxCheckedChanged = value;
                OnPropertyChanged(nameof(IsDomainCheckboxCheckedChanged));
            }
        }
        private string _noteSelected;
        public string NoteSelected
        {
            get { return _noteSelected; }
            set
            {
                _noteSelected = value;
                OnPropertyChanged(nameof(NoteSelected));

            }
        }
        private string _scoreSelected;
        public string ScoreSelected
        {
            get { return _scoreSelected; }
            set
            {
                _scoreSelected = value;
                OnPropertyChanged(nameof(ScoreSelected));
                if (value == null)
                    ScoreSelected = "4 selected";
            }
        }
        private bool runReport;
        public bool RunReport
        {
            get
            {
                return runReport;
            }
            set
            {
                runReport = value;
                OnPropertyChanged(nameof(RunReport));
            }
        }
        string selectedLocations { get; set; }
        private bool isIncludeDomain { get; set; }
        private bool isItemScores { get; set; }
        private string scores { get; set; }
        private string notes { get; set; }
        #endregion

        #region Commands
        public ICommand OpenRecordTypePopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isReportTypePopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectReportType && (popup as SelectReportType).Title == "SelectReportType")
                            {
                                return;
                            }
                        }
                    }
                    isReportTypePopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectReportType(this));
                });
            }
        }
        public ICommand OpenLocationPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isLocationPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectLocationPopupView && (popup as SelectLocationPopupView).Title == "SelectLocationPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isLocationPopupOpen = true;

                    var item = (e as BasicReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectLocationPopupView(null, this, SelectedLocationsReport));
                });
            }
        }
        public ICommand OpenChildRecordPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isChildRecordPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectChildPopupView && (popup as SelectChildPopupView).Title == "SelectChildPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isChildRecordPopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectChildPopupView(null, this));
                });
            }
        }
        public ICommand OpenBatteryTypeRecordPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isBatteryTypePopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectBatteryTypePopupView && (popup as SelectBatteryTypePopupView).Title == "SelectBatteryTypePopupView")
                            {
                                return;
                            }
                        }
                    }
                    isBatteryTypePopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectBatteryTypePopupView(null, this));
                });
            }
        }
        public ICommand OpenRecordFormsPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isRecordFormPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectRecordFormPopupView && (popup as SelectRecordFormPopupView).Title == "SelectRecordFormPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isRecordFormPopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectRecordFormPopupView(null, this));
                });
            }
        }
        public ICommand OpenScorePopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isScoreTypePopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectScoresPopupView && (popup as SelectScoresPopupView).Title == "SelectScoresPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isScoreTypePopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectScoresPopupView(this));
                });
            }
        }
        public ICommand OpenNotesPopupCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isNotesTypePopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectNotesPopupView && (popup as SelectNotesPopupView).Title == "SelectNotesPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isNotesTypePopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectNotesPopupView(this));
                });
            }
        }
        public ICommand OpenOutputFormatTypeCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isOutputFormatPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectOutputFormatPopupView && (popup as SelectOutputFormatPopupView).Title == "SelectOutputFormatPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isOutputFormatPopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectOutputFormatPopupView(this));
                });
            }
        }
        public ICommand OpenProgramLabelCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isProgramLabelPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectProgramLabel && (popup as SelectProgramLabel).Title == "SelectNotesPopupView")
                            {
                                return;
                            }
                        }
                    }
                    isProgramLabelPopupOpen = true;
                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new SelectProgramLabel(this));
                });
            }
        }
        public ICommand OpenStandardDeviationCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isStandardDeviationPopupOpen)
                        return;
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is StandardDeviationPopUpView && (popup as StandardDeviationPopUpView).Title == "StandardDeviationPopUpView")
                            {
                                return;
                            }
                        }
                    }
                    isStandardDeviationPopupOpen = true;

                    var item = (e as FullReportViewModel);
                    await PopupNavigation.Instance.PushAsync(new StandardDeviationPopUpView(this));
                });
            }
        }
        public ICommand RunReportCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if ((SelectedReportTypeID != 0 && SelectedReportTypeName != "Select a report type") && SelectedLocations != null && SelectedChildID != null &&
                           SelectedAssessmentID != 0 && SelectedRecordFormID != null && OutputFormatTypeID != 0)
                    {
                        scores = null;
                        notes = null;
                        if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID)
                        {
                            isIncludeDomain = IsDomainCheckboxCheckedChanged;
                            isItemScores = IsScoresCheckboxCheckedChanged;
                            SelectedStandardDeviationValue = 0;
                            if (selectedScoreCount == 0)
                                RunReport = false;
                            else
                                RunReport = true;
                        }
                        else if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalScreenerID)
                        {
                            isIncludeDomain = IsDomainCheckboxCheckedChanged;
                            isItemScores = IsSDScoresCheckboxCheckedChanged;
                            IsActivitiesCheckboxCheckedChanged = false;
                            if (SelectedStandardDeviationValue == 0)
                                RunReport = false;
                            else
                                RunReport = true;
                        }
                        else
                        {
                            isIncludeDomain = IsDomainCheckboxCheckedChanged;
                            isItemScores = false;
                            IsActivitiesCheckboxCheckedChanged = false;
                            SelectedStandardDeviationValue = 0;
                        }
                        if (SelectedScoreTypes != null && SelectedScoreTypes.Count > 0)
                        {
                            for (int i = 0; i < SelectedScoreTypes.Count; i++)
                            {
                                scores += SelectedScoreTypes[i];
                                if (i < SelectedScoreTypes.Count - 1)
                                    scores += ",";
                            }
                        }
                        if (SelectedNoteTypes != null && SelectedNoteTypes.Count > 0)
                        {
                            for (int i = 0; i < SelectedNoteTypes.Count; i++)
                            {
                                notes += SelectedNoteTypes[i];
                                if (i < SelectedNoteTypes.Count - 1)
                                    notes += ",";
                            }
                        }
                        UserDialogs.Instance.ShowLoading("Loading...");
                        await Task.Delay(300);
                        var username = Convert.ToString(Application.Current.Properties["UserName"]);
                        var password = Convert.ToString(Application.Current.Properties["PassID"]);
                        try
                        {
                            var authresponse = await new BDIWebServices().LoginUser(new { username = username.Trim(), password = password.Trim() });
                            if (authresponse != null)
                            {
                                if (!string.IsNullOrEmpty(authresponse.StatusCode))
                                {
                                    UserDialogs.Instance.HideLoading();
                                    await UserDialogs.Instance.AlertAsync("Report download failed!!");
                                    return;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            UserDialogs.Instance.HideLoading();
                            if (!string.IsNullOrEmpty(ex.Message) && ex.Message == "User Don't have BDI product")
                            {
                                await UserDialogs.Instance.AlertAsync("You Don't have BDI product access. Please contact Admin");
                            }
                            else
                            {
                                await UserDialogs.Instance.AlertAsync("Report download failed!!");
                            }
                            return;
                        }
                        var response = await services.SaveReportCriteria(new CriteriaModel()
                        {
                            reportType = SelectedReportTypeID,
                            childUserId = SelectedChildID,
                            locationIds = selectedLocations,
                            batteryType = SelectedAssessmentID,
                            forminstanceIds = SelectedRecordFormID,
                            outputFormat = OutputFormatTypeID,
                            isIncludeDomain = isIncludeDomain,
                            isItemScores = isItemScores,
                            isSuggestActivities = IsActivitiesCheckboxCheckedChanged,
                            notes = notes,
                            programLabelId = SelectedProgramLabelID.ToString(),
                            selectedDeviations = SelectedStandardDeviationValue,
                            selectedScores = scores
                        });
                        var reportName = "BDI3_ScoreReport";
                        if (SelectedReportTypeID == 2)
                        {
                            reportName = "BDI3_FamilyReport";
                        }
                        var url = await services.ExecuteReport(response.criteriaId, reportName, OutputFormatTypeID == 1 ? ".pdf" : ".docx");
                        if (!string.IsNullOrEmpty(url))
                        {
                            ICreateHtmlFiles createhtmlFile = DependencyService.Get<ICreateHtmlFiles>();
                            await createhtmlFile.SaveFile(url);
                            UserDialogs.Instance.HideLoading();
                        }
                        else
                        {
                            UserDialogs.Instance.HideLoading();
                            await UserDialogs.Instance.AlertAsync("Report download failed!!");
                        }
                        SelectedStandardDeviationValue = 1.5;
                    }
                    else
                    {
                        RunReport = false;
                    }
                });

            }
        }

        #endregion
        private BDIWebServices services = new BDIWebServices();
        public FullReportViewModel()
        {
            IsChildPopUpEnabled = false;
            SelectedStandardDeviationValue = -1.5;
            StandardDeviationValue = "-1.5";
            SelectedReportTypeName = "Score Report";
            SelectedReportTypeID = 1;
            SetReportTypeListData();
            GetLocations();
            SetNotesData();
            SetOutputFormat();
        }
        public async Task SetReportTypeListData()
        {
            ReportTypeList = new List<ReportType>();
            var response = await services.GetReportParamters(new ReportParameters() { FromDate = null, ToDate = null });
            if (response != null && response.reportTypes.Count > 0)
            {
                foreach (var item in response.reportTypes)
                {
                    if (item.value != 3)
                        ReportTypeList.Add(new ReportType() { ReportTypeID = item.value, ReportTypeName = item.text, IsSelected = item.selected });
                }
            }
            if (response != null && response.programLabels.Count > 0)
            {
                foreach (var item in response.programLabels)
                {
                    ProgramLabelList.Add(new ProgramLabels() { LabelName = item.labelname, Selected = item.selected, LabelID = item.labelId });
                }
            }
        }
        public async void GetLocations()
        {
            var locations = await services.GetLocationsForReport();
            List<ReportLocations> reportLocations = new List<ReportLocations>();
            if (locations != null)
            {
                foreach (var item in locations)
                {
                    reportLocations.Add(item);
                    if (item.canHaveChildren)
                    {
                        foreach (var levelOneChildLocations in item.children)
                        {
                            reportLocations.Add(levelOneChildLocations);
                            if (levelOneChildLocations.canHaveChildren)
                            {
                                foreach (var levelTwoChildLocations in levelOneChildLocations.children)
                                {
                                    reportLocations.Add(levelTwoChildLocations);
                                    if (levelTwoChildLocations.canHaveChildren)
                                    {
                                        foreach (var levelThreeChildLocations in levelTwoChildLocations.children)
                                        {
                                            reportLocations.Add(levelThreeChildLocations);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (locations != null && locations.Any())
                {
                    AllLocations = locations.ToList();
                    Locations = new List<ReportLocations>();
                    LocationsObservableCollection = new ObservableCollection<LocationNew>();
                    var parentLocation = reportLocations.Where(p => p.ParentLocationID == null).ToList();
                    if (parentLocation != null && parentLocation.Any())
                    {
                        foreach (var item in parentLocation)
                        {
                            var location = new ReportLocations();
                            var locationnew = new LocationNew();
                            int parentLocationID = 0;

                            location.LocationID = item.LocationID;
                            location.Name = item.Name;
                            location.enabled = item.enabled;
                            location.ParentLocationID = item.ParentLocationID;
                            Locations.Add(location);

                            if (item.ParentLocationID != null)
                                parentLocationID = int.Parse(item.ParentLocationID);
                            locationnew.LocationId = item.LocationID;
                            locationnew.LocationName = item.Name;
                            locationnew.IsEnabled = item.enabled;
                            locationnew.ParentLocationId = parentLocationID;
                            LocationsObservableCollection.Add(locationnew);
                            reportLocations.Remove(item);
                            GenerateSubLocation(locationnew, reportLocations);
                        }
                    }
                }
            }
            else
            {

            }
        }
        private void GenerateSubLocation(LocationNew location, List<ReportLocations> lstLocation)
        {

            var subLocation = lstLocation.Where(p => p.ParentLocationID == location.LocationId.ToString());
            int parentLocationID = 0;
            if (subLocation != null && subLocation.Any())
            {
                location.SubLocations = new List<LocationNew>();
                foreach (var item in subLocation)
                {
                    if (item.ParentLocationID != null)
                        parentLocationID = int.Parse(item.ParentLocationID);

                    item.depth = location.Level + 1;
                    var locationnew = new LocationNew();
                    locationnew.LocationId = item.LocationID;
                    locationnew.LocationName = item.Name;
                    //locationnew.IsSelected = item.IsSelected;
                    locationnew.IsEnabled = item.enabled;
                    locationnew.ParentLocationId = parentLocationID;
                    locationnew.Level = item.depth;
                    location.SubLocations.Add(locationnew);
                    location.HasSubLocations = true;
                    GenerateSubLocation(locationnew, lstLocation);
                }
            }
        }
        public void SetScoreTypeData()
        {
            if (SelectedReportTypeID == 1)
            {
                ScoreTypeList = new List<ScoreType>();
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "RS", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "SS", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "PR", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "Z-Score", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "AE", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "RDI", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "CSS", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "CSS 90%", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "T-Score", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "NCE", IsSelected = false });
                SelectedScoreTypes = new List<string>();
                SelectedScoreTypes.Add("RS");
                SelectedScoreTypes.Add("SS");
                SelectedScoreTypes.Add("PR");
                SelectedScoreTypes.Add("Z-Score");
                selectedScoreCount = SelectedScoreTypes.Count;
            }
            //Family Report
            else if (SelectedReportTypeID == 2)
            {
                ScoreTypeList = new List<ScoreType>();
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "RS", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "SS", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "PR", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "Z-Score", IsSelected = true });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "AE", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "RDI", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "CSS", IsSelected = false });
                ScoreTypeList.Add(new ScoreType { ScoreTypeName = "CSS 90%", IsSelected = false });
                SelectedScoreTypes = new List<string>();
                SelectedScoreTypes.Add("RS");
                SelectedScoreTypes.Add("SS");
                SelectedScoreTypes.Add("PR");
                SelectedScoreTypes.Add("Z-Score");
                selectedScoreCount = SelectedScoreTypes.Count;
            }
        }
        public void SetStandarDeviationTypeData()
        {
            if (StandardDeviationList.Any())
                StandardDeviationList.Clear();
            StandardDeviationList = new List<StandardDeviation>();
            StandardDeviationList.Add(new StandardDeviation { StandardDeviationValue = -2.0, IsSelected = false });
            StandardDeviationList.Add(new StandardDeviation { StandardDeviationValue = -1.5, IsSelected = true });
            StandardDeviationList.Add(new StandardDeviation { StandardDeviationValue = -1.0, IsSelected = false });

        }
        public void SetNotesData(int batteryTypeID = 0)
        {
            if (batteryTypeID == 0)
            {
                NoteTypeList = new List<NotesType>();
                NoteTypeList.Add(new NotesType { NoteTypeName = "Record Form Notes", IsSelected = false, NotesTypeID = 1 });
                NoteTypeList.Add(new NotesType { NoteTypeName = "Subdomain/Area Notes", IsSelected = false, NotesTypeID = 2 });
            }
            else
            {
                if (batteryTypeID == AssignmentTypes.BattelleDevelopmentalCompleteID || batteryTypeID == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID)
                {
                    if (!NoteTypeList.Any())
                    {
                        var data = NoteTypeList.Where(p => p.IsSelected);

                        NoteTypeList = new List<NotesType>();
                        NoteTypeList.Add(new NotesType { NoteTypeName = "Record Form Notes", IsSelected = false, NotesTypeID = 1 });
                        NoteTypeList.Add(new NotesType { NoteTypeName = "Subdomain/Area Notes", IsSelected = false, NotesTypeID = 2 });
                    }
                    if (IsScoresCheckboxCheckedChanged)
                    {
                        NotesType item = new NotesType() { NotesTypeID = 3, NoteTypeName = "Item Notes" };

                        if (NoteTypeList.Count < 3 && !NoteTypeList.Contains(item))
                            NoteTypeList.Add(new NotesType { NoteTypeName = "Item Notes", IsSelected = false, NotesTypeID = 3 });
                    }
                    else
                    {
                        NotesType item = new NotesType() { NotesTypeID = 3, NoteTypeName = "Item Notes" };
                        if (NoteTypeList.Count == 3)
                        {
                            NoteTypeList.RemoveAt(2);

                            if (SelectedNoteTypes != null)
                            {
                                if (SelectedNoteTypes.Contains("Item Notes"))
                                {
                                    SelectedNoteTypes.Remove("Item Notes");
                                    selectedNoteCount = selectedNoteCount - 1;
                                }
                            }
                            if (selectedNoteCount > 2)
                                selectedNoteCount = selectedNoteCount - 1;
                            if (selectedNoteCount > 0)
                                NoteSelected = selectedNoteCount + " selected";
                            else
                                NoteSelected = "Select notes";
                        }
                    }
                }
                else if (batteryTypeID == AssignmentTypes.BattelleDevelopmentalScreenerID)
                {
                    NoteTypeList = new List<NotesType>();
                    
                    NoteTypeList = new List<NotesType>();
                    NoteTypeList.Add(new NotesType { NoteTypeName = "Record Form Notes", IsSelected = false, NotesTypeID = 1 });
                    NoteTypeList.Add(new NotesType { NoteTypeName = "Domain Notes", IsSelected = false, NotesTypeID = 2 });
                    
                    if (IsSDScoresCheckboxCheckedChanged)
                    {
                        NotesType item = new NotesType() { NotesTypeID = 3, NoteTypeName = "Item Notes" };

                        if (NoteTypeList.Count < 3 && !NoteTypeList.Contains(item))
                            NoteTypeList.Add(new NotesType { NoteTypeName = "Item Notes", IsSelected = false, NotesTypeID = 3 });
                    }
                    else
                    {
                        NotesType item = new NotesType() { NotesTypeID = 3, NoteTypeName = "Item Notes" };
                        if (NoteTypeList.Count == 3)
                        {
                            NoteTypeList.RemoveAt(2);

                            if (SelectedNoteTypes != null)
                            {
                                if (SelectedNoteTypes.Contains("Item Notes"))
                                {
                                    SelectedNoteTypes.Remove("Item Notes");
                                    selectedNoteCount = selectedNoteCount - 1;
                                }
                            }
                            if (selectedNoteCount > 2)
                                selectedNoteCount = selectedNoteCount - 1;
                            if (selectedNoteCount > 0)
                                NoteSelected = selectedNoteCount + " selected";
                            else
                                NoteSelected = "Select notes";
                        }
                        else
                        {
                            selectedNoteCount = 0;
                            NoteSelected = "Select notes";
                        }
                    }
                }
            }
        }
        public async void SetChildData()
        {
            if (ChildRecords.Any())
                ChildRecords.Clear();
            ChildRecords = new List<ChildRecord>();
            selectedLocations = null;
            if (SelectedLocations.Count > 0)
            {
                for (int i = 0; i < SelectedLocations.Count; i++)
                {
                    selectedLocations += SelectedLocations[i];
                    if (i < SelectedLocations.Count - 1)
                        selectedLocations += ",";
                }
                var response = await services.GetChildData(new ReportLocationParameters { FromDate = null, ToDate = null, LocationIds = selectedLocations, IsDataExport = false });
                if (response != null && response.Count > 0)
                {
                    foreach (var item in response)
                    {
                        ChildRecords.Add(new ChildRecord() { Name = item.lastName + " " + item.firstName, ChildUserID = item.userId, selected = false });
                    }
                    IsChildPopUpEnabled = true;
                }
                else
                {
                    IsChildPopUpEnabled = false;
                    SelectedChildName = "No results found";
                }
            }
        }
        public async void SetBatteryTypesData()
        {
            BatteryTypeList = new List<BatteryTypes>();
            var recordForms = await services.GetBatteryTypes(new ReportBatteryTypes() { FromDate = null, ToDate = null, userId = SelectedChildID });
            if (recordForms != null && recordForms.Count > 0)
            {
                if (SelectedProgramLabelID == 0)
                {
                    var testRecords = recordForms.OrderBy(x => x.assessmentId).Select(x => x.assessmentId).Distinct();
                    foreach (var item in testRecords)
                    {
                        if (item == AssignmentTypes.BattelleDevelopmentalCompleteID)
                            BatteryTypeList.Add(new BatteryTypes() { assessmentId = item, Description = AssignmentTypes.BattelleDevelopmentalCompleteString, selected = false });
                        else if (item == AssignmentTypes.BattelleDevelopmentalScreenerID)
                            BatteryTypeList.Add(new BatteryTypes() { assessmentId = item, Description = AssignmentTypes.BattelleDevelopmentScreenerString, selected = false });
                        else if (item == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID)
                            BatteryTypeList.Add(new BatteryTypes() { assessmentId = item, Description = AssignmentTypes.BattelleEarlyAcademicSurveyString, selected = false });
                    }
                }
                else
                {
                    var sortedForms = recordForms.Where(x => x.programLabelId == SelectedProgramLabelID.ToString()).OrderBy(x => x.assessmentId).Select(x => x.assessmentId).Distinct();
                    foreach (var item in sortedForms)
                    {
                        if (item == AssignmentTypes.BattelleDevelopmentalCompleteID)
                            BatteryTypeList.Add(new BatteryTypes() { assessmentId = item, Description = AssignmentTypes.BattelleDevelopmentalCompleteString, selected = false });
                        else if (item == AssignmentTypes.BattelleDevelopmentalScreenerID)
                            BatteryTypeList.Add(new BatteryTypes() { assessmentId = item, Description = AssignmentTypes.BattelleDevelopmentScreenerString, selected = false });
                        else if (item == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID)
                            BatteryTypeList.Add(new BatteryTypes() { assessmentId = item, Description = AssignmentTypes.BattelleEarlyAcademicSurveyString, selected = false });
                    }
                }
                if (BatteryTypeList.Count > 0)
                {
                    IsBatteryTypePopupEnabled = true;
                }
                else
                {
                    IsBatteryTypePopupEnabled = false;
                    SelectedAssessmentType = "No results found";
                }
            }
            else
            {
                IsBatteryTypePopupEnabled = false;
            }
        }
        public async void SetRecordFormsData()
        {
            ChildTestRecords = new List<RecordForms>();
            var recordForms = await services.GetBatteryTypes(new ReportBatteryTypes() { FromDate = null, ToDate = null, userId = SelectedChildID });
            if (recordForms != null || recordForms.Count > 0)
            {
                if (SelectedProgramLabelID == 0)
                {
                    var sortedRecordForms = recordForms.Where(x => x.assessmentId == SelectedAssessmentID);
                    foreach (var item in sortedRecordForms)
                    {
                        ChildTestRecords.Add(new RecordForms()
                        {
                            AssessmentID = SelectedAssessmentID,
                            RecordFormName = item.description,
                            FormInstanceID = item.formInstanceId,
                            ProgramLabelID = item.programLabelId,
                            IsSelect = false
                        });
                    }
                    IsRecordFormButtonEnabled = true;
                    SetLayoutVisibilty();
                    SetNotesData(SelectedAssessmentID);
                }
                else
                {
                    var sortedRecordForms = recordForms.Where(x => x.assessmentId == SelectedAssessmentID && x.programLabelId == SelectedProgramLabelID.ToString());
                    foreach (var item in sortedRecordForms)
                    {
                        ChildTestRecords.Add(new RecordForms()
                        {
                            AssessmentID = SelectedAssessmentID,
                            RecordFormName = item.description,
                            FormInstanceID = item.formInstanceId,
                            ProgramLabelID = item.programLabelId,
                            IsSelect = false
                        });

                    }
                    IsRecordFormButtonEnabled = true;
                    SetLayoutVisibilty();
                    SetNotesData(SelectedAssessmentID);
                }
            }
            else
            {
                IsRecordFormButtonEnabled = false;
            }
        }
        public void SetLayoutVisibilty()
        {
            if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID)
            {
                IsScoringLayoutVisible = IsScoresPopUpEnabled = true;
                IsStandardDeviationLayoutVisible = IsStandardDeviationPopUpEnabled = IsBAESLayoutVisible = false;
                SetScoreTypeData();
                if (SelectedReportTypeID == 2)
                {
                    IsDomainCheckboxCheckedChanged = IsScoresCheckboxCheckedChanged = IsActivitiesCheckboxCheckedChanged = true;
                }
            }
            else if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalScreenerID)
            {
                IsScoringLayoutVisible = IsScoresPopUpEnabled = IsBAESLayoutVisible = false;
                IsStandardDeviationLayoutVisible = IsStandardDeviationPopUpEnabled = true;
                SetStandarDeviationTypeData();
                if (SelectedReportTypeID == 2)
                {
                    IsDomainCheckboxCheckedChanged = IsSDScoresCheckboxCheckedChanged = true;
                }
            }
            else if (SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalAcademicSurveyID)
            {
                IsBAESLayoutVisible = true;
                if (SelectedReportTypeID == 2)
                {
                    IsDomainCheckboxCheckedChanged = true;
                }
            }
        }
        public void SetOutputFormat()
        {
            OutputFormatTypeList = new List<OutputFormatType>();
            OutputFormatTypeList.Add(new OutputFormatType() { OutputFormatTypeID = 1, OutputFormatTypeName = "PDF", IsSelected = false });
            OutputFormatTypeList.Add(new OutputFormatType() { OutputFormatTypeID = 2, OutputFormatTypeName = "Word", IsSelected = false });

        }
    }
}

