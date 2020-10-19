using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StandardDeviationPopUpView : PopupPage
    {
        FullReportViewModel ReportViewModel { get; set; }
        public StandardDeviationPopUpView(FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            ReportViewModel = fullReportViewModel;
            BindingContext = fullReportViewModel;
            this.CloseWhenBackgroundIsClicked = true;
            this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
            StandardDeviationListview.ItemTapped += StandardDeviationItemTapped;
            if (Device.RuntimePlatform != Device.Android)
            {
                Frame1.IsVisible = StandardDeviationListview.IsVisible = true;
                AndroidFrame.IsVisible = StandardDeviationCollectionview.IsVisible = false;
            }
            else
            {
                Frame1.IsVisible = StandardDeviationListview.IsVisible = false;
                AndroidFrame.IsVisible = StandardDeviationCollectionview.IsVisible = true;
            }
        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isStandardDeviationPopupOpen = false;
            EnableRunReport();
        }
        private void StandardDeviationItemTapped(object sender, ItemTappedEventArgs e)
        {
            var standardDeviationItem = e.Item as StandardDeviation;
            foreach (var item in ReportViewModel.StandardDeviationList)
            {
                if (item.StandardDeviationValue == standardDeviationItem.StandardDeviationValue)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedStandardDeviationValue = 0;
                        ReportViewModel.StandardDeviationValue = "Select standard deviation";
                        EnableRunReport();
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedStandardDeviationValue = item.StandardDeviationValue;
                        ReportViewModel.StandardDeviationValue = item.StandardDeviationValue.ToString();
                        EnableRunReport();
                    }
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

        private void EnableRunReport()
        {
            if ((ReportViewModel.SelectedReportTypeID != 0 && ReportViewModel.SelectedReportTypeName != "Select a report type") && ReportViewModel.SelectedLocations.Count > 0 && ReportViewModel.SelectedChildID != null
                    && ReportViewModel.SelectedStandardDeviationValue != 0 && ReportViewModel.SelectedAssessmentID != 0 && ReportViewModel.SelectedRecordFormID != null && ReportViewModel.OutputFormatTypeID != 0)
            {
                ReportViewModel.RunReport = true;
            }
            else
            {
                ReportViewModel.RunReport = false;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var itemSelected = sender as StackLayout;
            var label = itemSelected.Children[0] as Label;
            var itemText = label.Text;

            foreach (var item in ReportViewModel.StandardDeviationList)
            {
                if (item.StandardDeviationValue.ToString() == itemText)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedStandardDeviationValue = 0;
                        ReportViewModel.StandardDeviationValue = "Select standard deviation";
                        EnableRunReport();
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedStandardDeviationValue = item.StandardDeviationValue;
                        ReportViewModel.StandardDeviationValue = item.StandardDeviationValue.ToString();
                        EnableRunReport();
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