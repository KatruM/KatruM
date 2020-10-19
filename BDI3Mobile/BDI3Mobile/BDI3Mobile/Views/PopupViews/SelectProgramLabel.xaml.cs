using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectProgramLabel : PopupPage
    {
        FullReportViewModel ReportViewModel { get; set; }
        public SelectProgramLabel(FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            ReportViewModel = fullReportViewModel;
            BindingContext = fullReportViewModel;
            this.CloseWhenBackgroundIsClicked = true;
            this.BackgroundClicked += ProgramLabelPopupView_BackgroundClicked;
            if (Device.RuntimePlatform == Device.iOS)
            {
                SelectProgramLabelPopupView.Margin = new Thickness(0, 10, 0, 0);
            }
            if (Device.RuntimePlatform != Device.Android)
            {
                Frame1.IsVisible = ProgramLabelListview.IsVisible = true;
                AndroidFrame.IsVisible = ProgramLabelCollectionview.IsVisible = false;
            }
            else
            {
                Frame1.IsVisible = ProgramLabelListview.IsVisible = false;
                AndroidFrame.IsVisible = ProgramLabelCollectionview.IsVisible = true;
            }
        }
        private void ProgramLabelPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isProgramLabelPopupOpen = false;
        }
        private void ReportProgramNote_Tapped(object sender, ItemTappedEventArgs e)
        {
            var labelItem = e.Item as ProgramLabels;
            foreach (var item in ReportViewModel.ProgramLabelList)
            {
                if (item.LabelName == labelItem.LabelName)
                {
                    if (item.Selected)
                    {
                        item.Selected = !item.Selected;
                        ReportViewModel.GetLocations();
                        ReportViewModel.SelectedProgramLabelName = null;
                        ReportViewModel.SelectedProgramLabelID = 0;
                        ReportViewModel.SelectedLocations = null;
                        ReportViewModel.LocationsSelected = null;
                        ReportViewModel.selectedCount = 0;
                        ReportViewModel.SelectedChildID = null;
                        ReportViewModel.SelectedAssessmentID = 0;
                        ReportViewModel.SelectedRecordFormID = null;
                        ReportViewModel.IsScoringLayoutVisible = ReportViewModel.IsStandardDeviationLayoutVisible = ReportViewModel.IsBAESLayoutVisible = false;
                        ReportViewModel.IsScoresCheckboxCheckedChanged = ReportViewModel.IsSDScoresCheckboxCheckedChanged = ReportViewModel.IsActivitiesCheckboxCheckedChanged = ReportViewModel.IsDomainCheckboxCheckedChanged = false;
                        ReportViewModel.IsChildPopUpEnabled = ReportViewModel.IsBatteryTypePopupEnabled = ReportViewModel.IsRecordFormButtonEnabled = false;
                        ReportViewModel.RunReport = false;
                    }
                    else
                    {
                        item.Selected = !item.Selected;
                        ReportViewModel.SelectedProgramLabelName = item.LabelName;
                        ReportViewModel.SelectedProgramLabelID = item.LabelID;

                        if (ReportViewModel.SelectedProgramLabelID != 0)
                        {
                            ReportViewModel.GetLocations();
                            ReportViewModel.LocationsSelected = null;
                            ReportViewModel.selectedCount = 0;
                            ReportViewModel.SelectedChildID = null;
                            ReportViewModel.SelectedAssessmentID = 0;
                            ReportViewModel.SelectedRecordFormID = null;
                            ReportViewModel.RunReport = false;
                            ReportViewModel.IsScoresCheckboxCheckedChanged = ReportViewModel.IsSDScoresCheckboxCheckedChanged = ReportViewModel.IsActivitiesCheckboxCheckedChanged = ReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            ReportViewModel.IsScoringLayoutVisible = ReportViewModel.IsStandardDeviationLayoutVisible = ReportViewModel.IsBAESLayoutVisible = false;
                            ReportViewModel.IsChildPopUpEnabled = ReportViewModel.IsBatteryTypePopupEnabled = ReportViewModel.IsRecordFormButtonEnabled = false;
                        }
                    }
                }
                else
                {
                    item.Selected = false;
                }
            }
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var itemSelected = sender as StackLayout;
            var label = itemSelected.Children[0] as Label;
            var itemText = label.Text;
            foreach (var item in ReportViewModel.ProgramLabelList)
            {
                if (item.LabelName == itemText)
                {
                    if (item.Selected)
                    {
                        item.Selected = !item.Selected;
                        ReportViewModel.GetLocations();
                        ReportViewModel.SelectedProgramLabelName = null;
                        ReportViewModel.SelectedProgramLabelID = 0;
                        ReportViewModel.SelectedLocations = null;
                        ReportViewModel.LocationsSelected = null;
                        ReportViewModel.selectedCount = 0;
                        ReportViewModel.SelectedChildID = null;
                        ReportViewModel.SelectedAssessmentID = 0;
                        ReportViewModel.SelectedRecordFormID = null;
                        ReportViewModel.IsScoringLayoutVisible = ReportViewModel.IsStandardDeviationLayoutVisible = ReportViewModel.IsBAESLayoutVisible = false;
                        ReportViewModel.IsScoresCheckboxCheckedChanged = ReportViewModel.IsSDScoresCheckboxCheckedChanged = ReportViewModel.IsActivitiesCheckboxCheckedChanged = ReportViewModel.IsDomainCheckboxCheckedChanged = false;
                        ReportViewModel.IsChildPopUpEnabled = ReportViewModel.IsBatteryTypePopupEnabled = ReportViewModel.IsRecordFormButtonEnabled = false;
                        ReportViewModel.RunReport = false;
                    }
                    else
                    {
                        item.Selected = !item.Selected;
                        ReportViewModel.SelectedProgramLabelName = item.LabelName;
                        ReportViewModel.SelectedProgramLabelID = item.LabelID;

                        if (ReportViewModel.SelectedProgramLabelID != 0)
                        {
                            ReportViewModel.GetLocations();
                            ReportViewModel.LocationsSelected = null;
                            ReportViewModel.selectedCount = 0;
                            ReportViewModel.SelectedChildID = null;
                            ReportViewModel.SelectedAssessmentID = 0;
                            ReportViewModel.SelectedRecordFormID = null;
                            ReportViewModel.RunReport = false;
                            ReportViewModel.IsScoresCheckboxCheckedChanged = ReportViewModel.IsSDScoresCheckboxCheckedChanged = ReportViewModel.IsActivitiesCheckboxCheckedChanged = ReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            ReportViewModel.IsScoringLayoutVisible = ReportViewModel.IsStandardDeviationLayoutVisible = ReportViewModel.IsBAESLayoutVisible = false;
                            ReportViewModel.IsChildPopUpEnabled = ReportViewModel.IsBatteryTypePopupEnabled = ReportViewModel.IsRecordFormButtonEnabled = false;
                        }
                    }
                }
                else
                {
                    item.Selected = false;
                }
            }
        }
    }
    }