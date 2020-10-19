using BDI3Mobile.Common;
using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectOutputFormatPopupView : PopupPage
    {
        FullReportViewModel ReportViewModel { get; set; }
        public SelectOutputFormatPopupView(FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            ReportViewModel = fullReportViewModel;
            BindingContext = fullReportViewModel;
            this.CloseWhenBackgroundIsClicked = true;
            this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
            if (Device.RuntimePlatform == Device.Android)
            {
                ReportViewModel.SetOutputFormat();
                OutputFormatList.ItemsSource = ReportViewModel.OutputFormatTypeList;
                SetData();
            }
            OutputFormatList.ItemTapped += OutputFormatTypeItemTapped;

            if (Device.RuntimePlatform != Device.Android)
            {
                Frame1.IsVisible = OutputFormatList.IsVisible = true;
                AndroidFrame.IsVisible = OutputFormatCollectionView.IsVisible = false;
            }
            else
            {
                Frame1.IsVisible = OutputFormatList.IsVisible = false;
                AndroidFrame.IsVisible = OutputFormatCollectionView.IsVisible = true;
            }
        }

        void SetData()
        {
            foreach (var item in ReportViewModel.OutputFormatTypeList)
            {
                if (ReportViewModel.OutputFormatTypeID == item.OutputFormatTypeID)
                {
                    item.IsSelected = true;
                }
            }
            
        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isOutputFormatPopupOpen = false;
            EnableRunReport();
        }
        private void OutputFormatTypeItemTapped(object sender, ItemTappedEventArgs e)
        {
            var outputFormTypeItem = e.Item as OutputFormatType;
            foreach (var item in ReportViewModel.OutputFormatTypeList)
            {
                if (item.OutputFormatTypeID == outputFormTypeItem.OutputFormatTypeID)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedOutputFormatType = null;
                        ReportViewModel.OutputFormatTypeID = 0;
                        EnableRunReport();

                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedOutputFormatType = item.OutputFormatTypeName;
                        ReportViewModel.OutputFormatTypeID = item.OutputFormatTypeID;
                        EnableRunReport();
                    }
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }

        public void EnableRunReport()
        {
            if ((ReportViewModel.SelectedReportTypeID != 0 && ReportViewModel.SelectedReportTypeName != "Select a report type") && (ReportViewModel.SelectedLocations!=null && ReportViewModel.SelectedLocations.Count > 0) && ReportViewModel.SelectedChildID != null
                    && ReportViewModel.SelectedAssessmentID != 0 && ReportViewModel.SelectedRecordFormID != null && ReportViewModel.OutputFormatTypeID != 0)
            {
                if (ReportViewModel.SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalCompleteID)
                {
                    if (ReportViewModel.selectedScoreCount != 0)
                    {
                        ReportViewModel.RunReport = true;
                    }
                    else
                    {
                        ReportViewModel.RunReport = false;
                    }
                }
                else if (ReportViewModel.SelectedAssessmentID == AssignmentTypes.BattelleDevelopmentalScreenerID)
                {
                    if (ReportViewModel.SelectedStandardDeviationValue != 0)
                    {
                        ReportViewModel.RunReport = true;
                    }
                    else
                    {
                        ReportViewModel.RunReport = false;
                    }
                }
                else
                {
                    ReportViewModel.RunReport = true;

                }

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

            foreach (var item in ReportViewModel.OutputFormatTypeList)
            {
                if (item.OutputFormatTypeName == itemText)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedOutputFormatType = null;
                        ReportViewModel.OutputFormatTypeID = 0;
                        EnableRunReport();

                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedOutputFormatType = item.OutputFormatTypeName;
                        ReportViewModel.OutputFormatTypeID = item.OutputFormatTypeID;
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