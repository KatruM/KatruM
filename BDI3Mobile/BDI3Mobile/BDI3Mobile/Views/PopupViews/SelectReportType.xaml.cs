using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectReportType : PopupPage
    {
        FullReportViewModel ReportViewModel { get; set; }
        public SelectReportType(FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            ReportViewModel = fullReportViewModel;
            BindingContext = fullReportViewModel;
            this.CloseWhenBackgroundIsClicked = true;
            this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
            ReportTypeListview.ItemTapped += FullReportReportTypeItemTapped;
            if(Device.RuntimePlatform == Device.iOS)
            {
                ReportTypePopUp.Margin = new Thickness(0, 10, 0, 0);
            }

            if (Device.RuntimePlatform != Device.Android)
            {
                Frame1.IsVisible = ReportTypeListview.IsVisible = true;
                AndroidFrame.IsVisible = ReportTypeCollectionView.IsVisible = false;
            }
            else
            {
                Frame1.IsVisible = ReportTypeListview.IsVisible = false;
                AndroidFrame.IsVisible = ReportTypeCollectionView.IsVisible = true;
            }
        }        
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e) {

            ReportViewModel.isReportTypePopupOpen = false;
        }
        private void FullReportReportTypeItemTapped(object sender, ItemTappedEventArgs e) 
        {
            var reportTypeItem = e.Item as ReportType;
            foreach (var item in ReportViewModel.ReportTypeList)
            {
                if (item.ReportTypeID == reportTypeItem.ReportTypeID)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.GetLocations();
                        ReportViewModel.SelectedReportTypeName = null;
                        ReportViewModel.SelectedReportTypeID = 0;
                        ReportViewModel.RunReport = false;
                        ReportViewModel.SelectedProgramLabelName = null;
                        ReportViewModel.SelectedProgramLabelID = 0;
                        foreach (var labels in ReportViewModel.ProgramLabelList)
                        {
                            labels.Selected = false;
                        }
                        ReportViewModel.SelectedLocations = null;
                        ReportViewModel.LocationsSelected = null;
                        ReportViewModel.selectedCount = 0;
                        ReportViewModel.SelectedChildID = null;
                        ReportViewModel.SelectedAssessmentID = 0;
                        ReportViewModel.SelectedRecordFormID = null;
                        ReportViewModel.IsScoringLayoutVisible = ReportViewModel.IsStandardDeviationLayoutVisible = ReportViewModel.IsBAESLayoutVisible = false;
                        ReportViewModel.IsScoresCheckboxCheckedChanged = ReportViewModel.IsSDScoresCheckboxCheckedChanged = ReportViewModel.IsActivitiesCheckboxCheckedChanged = ReportViewModel.IsDomainCheckboxCheckedChanged = false;
                        ReportViewModel.IsChildPopUpEnabled = ReportViewModel.IsBatteryTypePopupEnabled = ReportViewModel.IsRecordFormButtonEnabled = false;
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedReportTypeID = item.ReportTypeID;
                        ReportViewModel.SelectedReportTypeName = item.ReportTypeName;
                        if (ReportViewModel.SelectedReportTypeID != 0)
                        {
                            ReportViewModel.SelectedProgramLabelName = null;
                            ReportViewModel.SelectedProgramLabelID = 0;
                            foreach (var labels in ReportViewModel.ProgramLabelList)
                            {
                                labels.Selected = false;
                            }
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

                        if ((ReportViewModel.SelectedReportTypeID != 0 && ReportViewModel.SelectedReportTypeName != "Select a report type") && (ReportViewModel.SelectedLocations != null && ReportViewModel.SelectedLocations.Count > 0) && ReportViewModel.SelectedChildID != null
                            && ReportViewModel.SelectedAssessmentID != 0 && ReportViewModel.SelectedRecordFormID != null && ReportViewModel.OutputFormatTypeID != 0)
                        {
                            ReportViewModel.RunReport = true;
                        }
                    }
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }


        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var itemSelected = sender as StackLayout;
            var label = itemSelected.Children[0] as Label;
            var itemText = label.Text;

            foreach (var item in ReportViewModel.ReportTypeList)
            {
                if (item.ReportTypeName == itemText)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.GetLocations();
                        ReportViewModel.SelectedReportTypeName = null;
                        ReportViewModel.SelectedReportTypeID = 0;
                        ReportViewModel.RunReport = false;
                        ReportViewModel.SelectedProgramLabelName = null;
                        ReportViewModel.SelectedProgramLabelID = 0;
                        foreach (var labels in ReportViewModel.ProgramLabelList)
                        {
                            labels.Selected = false;
                        }
                        ReportViewModel.SelectedLocations = null;
                        ReportViewModel.LocationsSelected = null;
                        ReportViewModel.selectedCount = 0;
                        ReportViewModel.SelectedChildID = null;
                        ReportViewModel.SelectedAssessmentID = 0;
                        ReportViewModel.SelectedRecordFormID = null;
                        ReportViewModel.IsScoringLayoutVisible = ReportViewModel.IsStandardDeviationLayoutVisible = ReportViewModel.IsBAESLayoutVisible = false;
                        ReportViewModel.IsScoresCheckboxCheckedChanged = ReportViewModel.IsSDScoresCheckboxCheckedChanged = ReportViewModel.IsActivitiesCheckboxCheckedChanged = ReportViewModel.IsDomainCheckboxCheckedChanged = false;
                        ReportViewModel.IsChildPopUpEnabled = ReportViewModel.IsBatteryTypePopupEnabled = ReportViewModel.IsRecordFormButtonEnabled = false;
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedReportTypeID = item.ReportTypeID;
                        ReportViewModel.SelectedReportTypeName = item.ReportTypeName;
                        if (ReportViewModel.SelectedReportTypeID != 0)
                        {
                            ReportViewModel.SelectedProgramLabelName = null;
                            ReportViewModel.SelectedProgramLabelID = 0;
                            foreach (var labels in ReportViewModel.ProgramLabelList)
                            {
                                labels.Selected = false;
                            }
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

                        if ((ReportViewModel.SelectedReportTypeID != 0 && ReportViewModel.SelectedReportTypeName != "Select a report type") && (ReportViewModel.SelectedLocations != null && ReportViewModel.SelectedLocations.Count > 0) && ReportViewModel.SelectedChildID != null
                            && ReportViewModel.SelectedAssessmentID != 0 && ReportViewModel.SelectedRecordFormID != null && ReportViewModel.OutputFormatTypeID != 0)
                        {
                            ReportViewModel.RunReport = true;
                        }
                    }
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }
      }
}