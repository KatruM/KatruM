using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectBatteryTypePopupView : PopupPage
    {
        BasicReportViewModel ReportViewModel { get; set; }
        FullReportViewModel FullReportViewModel { get; set; }
        public SelectBatteryTypePopupView(BasicReportViewModel basicReportViewModel, FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            if (basicReportViewModel != null)
            {
                ReportViewModel = basicReportViewModel;
                BindingContext = basicReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += BasicReportPopupView_BackgroundClicked;
                assesmentTypeListview.ItemTapped += AssessmentItemTapped;
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    assesmentTypeListview.RowHeight = 52;
                }
                
            }
            else
            {
                FullReportViewModel = fullReportViewModel;
                BindingContext = fullReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
                assesmentTypeListview.ItemTapped += AssessmentItemTapped;
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    assesmentTypeListview.RowHeight = 50;
                }
                if (Device.RuntimePlatform == Device.iOS)
                {
                    BatteryTypePopupView.Margin = new Thickness(0, 10, 0, 0);
                }
                
            }
            if (Device.RuntimePlatform != Device.Android)
            {
                Frame1.IsVisible = assesmentTypeListview.IsVisible = true;
                AndroidFrame.IsVisible = assesmentTypeCollectionView.IsVisible = false;
            }
            else
            {
                Frame1.IsVisible = assesmentTypeListview.IsVisible = false;
                AndroidFrame.IsVisible = assesmentTypeCollectionView.IsVisible = true;
            }

        }
        
        private void BasicReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isBatteryTypePopupOpen = false;

                if (ReportViewModel.SelectedAssessmentID == 0)
                {
                    ReportViewModel.IsRecordFormButtonEnabled = false;
                    ReportViewModel.RunReport = false;
                }
                else
                {
                    if (ReportViewModel.SelectedRecordFormID == 0)
                    {
                        ReportViewModel.IsRecordFormButtonEnabled = true;
                    }
                }
            
        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            FullReportViewModel.isBatteryTypePopupOpen = false;
            if (FullReportViewModel.SelectedAssessmentID != 0 && FullReportViewModel.SelectedRecordFormID == null)
            {
                FullReportViewModel.SetRecordFormsData();
            }
            else
            {               
                FullReportViewModel.IsRecordFormButtonEnabled = false;
            }
            EnableRunReport();
        }
        private void AssessmentItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (ReportViewModel != null)
            {

                var batteryTypeItem = e.Item as BatteryTypes;
                foreach (var item in ReportViewModel.BatteryTypeList)
                {
                    if (item.Description == batteryTypeItem.Description)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            ReportViewModel.SelectedAssessmentType = ReportViewModel.SelectedRecordForm = null;
                            ReportViewModel.SelectedAssessmentID = ReportViewModel.SelectedRecordFormID = 0;
                          
                        }
                        else
                        {
                            item.selected = !item.selected;
                            ReportViewModel.SelectedAssessmentType = item.Description;
                            ReportViewModel.SelectedAssessmentID = item.assessmentId;
                            ReportViewModel.SelectedRecordFormID = 0;
                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
            else
            {
                var batteryTypeItem = assesmentTypeListview.SelectedItem as BatteryTypes;
                foreach (var item in FullReportViewModel.BatteryTypeList)
                {
                    if (item.Description == batteryTypeItem.Description)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedAssessmentType = FullReportViewModel.SelectedRecordFormID = FullReportViewModel.SelectedRecordForm =  null;
                            FullReportViewModel.SelectedAssessmentID = 0;
                           
                            FullReportViewModel.SelectedScoreTypes = null;
                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            EnableRunReport();
                        }
                        else
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedAssessmentType = item.Description;
                            FullReportViewModel.SelectedAssessmentID = item.assessmentId;
                            FullReportViewModel.SelectedRecordFormID = null;
                            FullReportViewModel.SelectedRecordForm = null;

                            FullReportViewModel.SelectedScoreTypes = null;
                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            EnableRunReport();
                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var itemSelected = sender as StackLayout;
            var label = itemSelected.Children[0] as Label;
            var itemText = label.Text;
            if (ReportViewModel != null)
            {
                foreach (var item in ReportViewModel.BatteryTypeList)
                {
                    if (item.Description == itemText)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            ReportViewModel.SelectedAssessmentType = null;
                            ReportViewModel.SelectedAssessmentID = 0;
                            ReportViewModel.SelectedRecordFormID = 0;
                            ReportViewModel.SelectedRecordForm = null;
                        }
                        else
                        {
                            item.selected = !item.selected;
                            ReportViewModel.SelectedAssessmentType = item.Description;
                            ReportViewModel.SelectedAssessmentID = item.assessmentId;
                            ReportViewModel.SelectedRecordFormID = 0;
                            ReportViewModel.SelectedRecordForm = null;
                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
            else
            {
                foreach (var item in FullReportViewModel.BatteryTypeList)
                {
                    if (item.Description == itemText)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedAssessmentType = null;
                            FullReportViewModel.SelectedAssessmentID = 0;
                            FullReportViewModel.SelectedRecordFormID = null;
                            FullReportViewModel.SelectedRecordForm = null;

                            FullReportViewModel.SelectedScoreTypes = null;
                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            EnableRunReport();
                        }
                        else
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedAssessmentType = item.Description;
                            FullReportViewModel.SelectedAssessmentID = item.assessmentId;
                            FullReportViewModel.SelectedRecordFormID = null;
                            FullReportViewModel.SelectedRecordForm = null;

                            FullReportViewModel.SelectedScoreTypes = null;
                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            EnableRunReport();
                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
            }
        }

            private void EnableRunReport()
        {
            if ((FullReportViewModel.SelectedReportTypeID != 0 && FullReportViewModel.SelectedReportTypeName != "Select a report type") && FullReportViewModel.SelectedLocations.Count > 0 && FullReportViewModel.SelectedChildID != null &&
                                    FullReportViewModel.SelectedAssessmentID != 0 && FullReportViewModel.SelectedRecordFormID != null && FullReportViewModel.OutputFormatTypeID != 0)
            {
                FullReportViewModel.RunReport = true;
            }
            else
            {
                FullReportViewModel.RunReport = false;
            }
        }
    }
}