using BDI3Mobile.Common;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectRecordFormPopupView : PopupPage
    {
        BasicReportViewModel ReportViewModel { get; set; }
        FullReportViewModel FullReportViewModel { get; set; }
        public SelectRecordFormPopupView(BasicReportViewModel basicReportViewModel, FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            if (basicReportViewModel != null)
            {
                ReportViewModel = basicReportViewModel;
                BindingContext = basicReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += BasicReportPopupView_BackgroundClicked;
                recordFormListview.ItemTapped += RecordFormItemTapped;
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    recordFormListview.RowHeight = 52;
                }
            }
            else
            {
                FullReportViewModel = fullReportViewModel;
                BindingContext = fullReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
                recordFormListview.ItemTapped += RecordFormItemTapped;
                if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
                {
                    recordFormListview.RowHeight = 50;
                }
            }
        }
        private void BasicReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isRecordFormPopupOpen = false;
            if (ReportViewModel.SelectedRecordFormID != 0 )
                ReportViewModel.RunReport = true;
            else
                ReportViewModel.RunReport = false;
        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e) {
            FullReportViewModel.isRecordFormPopupOpen = false;            
        }
        private void RecordFormItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (ReportViewModel != null)
            {
                var recordFormItem = e.Item as ChildInformationRecord;

                foreach (var item in ReportViewModel.ChildTestRecords)
                {
                    if (item.LocalTestInstance == recordFormItem.LocalTestInstance)
                    {
                        if (item.IsSelect)
                        {
                            item.IsSelect = !item.IsSelect;
                            ReportViewModel.SelectedRecordForm = null;
                            ReportViewModel.SelectedRecordFormID = 0;
                        }
                        else
                        {
                            item.IsSelect = !item.IsSelect;
                            ReportViewModel.SelectedRecordForm = item.RecordForm + "  (" + item.InitialTestDate + ")";
                            ReportViewModel.SelectedRecordFormID = item.LocalTestInstance;
                            ReportViewModel.TestDate = item.InitialTestDate;
                        }
                    }
                    else
                    {
                        item.IsSelect = false;
                    }
                }
            }
            else
            {
                var recordFormItem = e.Item as RecordForms;

                foreach (var item in FullReportViewModel.ChildTestRecords)
                {
                    if (item.FormInstanceID == recordFormItem.FormInstanceID)
                    {
                        if (item.IsSelect)
                        {
                            item.IsSelect = !item.IsSelect;
                            
                            FullReportViewModel.SelectedRecordForm = null;
                            FullReportViewModel.SelectedRecordFormID = null;
                            EnableRunReport();

                        }
                        else
                        {
                            item.IsSelect = !item.IsSelect;
                            FullReportViewModel.SelectedRecordForm = item.RecordFormName;
                            FullReportViewModel.SelectedRecordFormID = item.FormInstanceID;
                            EnableRunReport();
                        }
                    }
                    else
                    {
                        item.IsSelect = false;
                    }
                }

            }
        }
        
        private void EnableRunReport()
        {
            if ((FullReportViewModel.SelectedReportTypeID != 0 && FullReportViewModel.SelectedReportTypeName != "Select a report type") && FullReportViewModel.SelectedLocations.Count > 0 && FullReportViewModel.SelectedChildID != null
                      && FullReportViewModel.SelectedAssessmentID != 0 && FullReportViewModel.SelectedRecordFormID != null && FullReportViewModel.OutputFormatTypeID != 0)
            {
                if (FullReportViewModel.SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID)
                {
                    if (FullReportViewModel.selectedScoreCount != 0)
                    {
                        FullReportViewModel.RunReport = true;
                    }
                    else
                    {
                        FullReportViewModel.RunReport = false;
                    }
                }
                else if (FullReportViewModel.SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalScreenerID)
                {
                    if (FullReportViewModel.SelectedStandardDeviationValue != 0)
                    {
                        FullReportViewModel.RunReport = true;
                    }
                    else
                    {
                        FullReportViewModel.RunReport = false;
                    }
                }
                else
                {
                    FullReportViewModel.RunReport = true;
                }
            }
            else
            {
                FullReportViewModel.RunReport = false;
            }
        }
    }
}