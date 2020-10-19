using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectScoresPopupView : PopupPage
    {
        FullReportViewModel ReportViewModel { get; set; }
        public SelectScoresPopupView(FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            ReportViewModel = fullReportViewModel;
            BindingContext = fullReportViewModel;
            this.CloseWhenBackgroundIsClicked = true;
            this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
            ScoreListView.ItemTapped += FullReportReportTypeItemTapped;
        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isScoreTypePopupOpen = false;
            if ((ReportViewModel.SelectedReportTypeID != 0 && ReportViewModel.SelectedReportTypeName != "Select a report type") && ReportViewModel.SelectedLocations.Count > 0 && ReportViewModel.SelectedChildID != null
                     && ReportViewModel.selectedScoreCount != 0 && ReportViewModel.SelectedAssessmentID != 0 && ReportViewModel.SelectedRecordFormID != null && ReportViewModel.OutputFormatTypeID != 0)
            {
                ReportViewModel.RunReport = true;
            }
        }
        private async void FullReportReportTypeItemTapped(object sender, ItemTappedEventArgs e)
        {
            var scoreTypeItem = e.Item as ScoreType;
            foreach (var item in ReportViewModel.ScoreTypeList)
            {
                if (item.ScoreTypeName == scoreTypeItem.ScoreTypeName)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        if (ReportViewModel.SelectedScoreTypes.Contains(item.ScoreTypeName))
                            ReportViewModel.SelectedScoreTypes.Remove(item.ScoreTypeName);
                        ReportViewModel.selectedScoreCount = ReportViewModel.SelectedScoreTypes.Count;
                        if (ReportViewModel.selectedScoreCount > 0 && ReportViewModel.selectedScoreCount < 11)
                            ReportViewModel.ScoreSelected = ReportViewModel.selectedScoreCount + " selected";
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                            ReportViewModel.SelectedScoreTypes.Add(item.ScoreTypeName);
                        ReportViewModel.selectedScoreCount = ReportViewModel.SelectedScoreTypes.Count;
                        if (ReportViewModel.selectedScoreCount > 0 && ReportViewModel.selectedScoreCount < 11)
                            ReportViewModel.ScoreSelected = ReportViewModel.selectedScoreCount + " selected";
                    }
                }                
                ReportViewModel.selectedScoreCount = ReportViewModel.SelectedScoreTypes.Count;
                if (ReportViewModel.selectedScoreCount > 0 && ReportViewModel.selectedScoreCount < 11)
                    ReportViewModel.ScoreSelected = ReportViewModel.selectedScoreCount + " selected";
                else
                {
                    ReportViewModel.ScoreSelected = "Select score";
                    ReportViewModel.RunReport = false;
                }
                await Task.Delay(50);
            }
        }

    }
}