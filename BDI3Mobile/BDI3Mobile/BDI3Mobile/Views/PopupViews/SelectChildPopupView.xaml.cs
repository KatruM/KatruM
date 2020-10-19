using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectChildPopupView : PopupPage
    {
        BasicReportViewModel ReportViewModel { get; set; }
        FullReportViewModel FullReportViewModel { get; set; }
        public SelectChildPopupView(BasicReportViewModel basicReportViewModel, FullReportViewModel fullReportViewModel)
        {
            InitializeComponent();
            if (basicReportViewModel != null)
            {
                ReportViewModel = basicReportViewModel;
                BindingContext = basicReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += BasicReportPopupView_BackgroundClicked;
            }
            else if (fullReportViewModel != null)
            {
                FullReportViewModel = fullReportViewModel;
                BindingContext = fullReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    ChildPopupView.Margin = new Thickness(0, 10, 0, 0);
                }
            }
            if (Device.RuntimePlatform != Device.Android)
            {
                Frame1.IsVisible = childListview.IsVisible = true;
                childListview.ItemTapped += ChildItemTapped;
                AndroidFrame.IsVisible = childCollectionView.IsVisible = false;
            }
            else
            {
                Frame1.IsVisible = childListview.IsVisible = false;
                AndroidFrame.IsVisible = childCollectionView.IsVisible = true;
            }
        }
        private void BasicReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            ReportViewModel.isChildRecordPopupOpen = false;
            if (ReportViewModel.OfflineStudentID == 0)
            {
                ReportViewModel.IsBatteryTypeButtonEnabled = false;
                ReportViewModel.IsRecordFormButtonEnabled = false;
                ReportViewModel.RunReport = false;
            }
            else
            {
                if (ReportViewModel.SelectedAssessmentID == 0)
                {
                    ReportViewModel.IsBatteryTypeButtonEnabled = true;
                    ReportViewModel.IsRecordFormButtonEnabled = false;
                    ReportViewModel.SelectedRecordForm = null;
                    ReportViewModel.SelectedRecordFormID = 0;
                }
            }

        }
        private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            FullReportViewModel.isChildRecordPopupOpen = false;
            if (FullReportViewModel.SelectedChildID != null)
            {
                FullReportViewModel.SetBatteryTypesData();
            }
            else
            {
                FullReportViewModel.SelectedAssessmentID = 0;
                FullReportViewModel.SelectedRecordFormID = null;
                FullReportViewModel.SelectedScoreTypes = null;
                FullReportViewModel.ScoreSelected = null;
                FullReportViewModel.IsBatteryTypePopupEnabled = false;
                FullReportViewModel.IsRecordFormButtonEnabled = false;
                FullReportViewModel.RunReport = false;
            }
        }
        private void ChildItemTapped(object sender, ItemTappedEventArgs e)
        {
            var childItem = e.Item as ChildRecord;
            if (ReportViewModel != null)
            {
                foreach (var item in ReportViewModel.ChildRecords)
                {
                    if (item.OfflineStudentId == childItem.OfflineStudentId)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            ReportViewModel.SelectedChild = "Select child";
                            ReportViewModel.OfflineStudentID = ReportViewModel.SelectedAssessmentID = ReportViewModel.SelectedRecordFormID = 0;
                            ReportViewModel.SelectedChild = ReportViewModel.SelectedAssessmentType = ReportViewModel.SelectedRecordForm  = null;
                        }
                        else
                        {
                            item.selected = !item.selected;
                            ReportViewModel.FullName = ReportViewModel.SelectedChild = item.Name;
                            ReportViewModel.OfflineStudentID = item.OfflineStudentId;
                            ReportViewModel.DOB = item.DOB;
                            ReportViewModel.SelectedAssessmentID = ReportViewModel.SelectedRecordFormID = 0;
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
                foreach (var item in FullReportViewModel.ChildRecords)
                {
                    if (item.ChildUserID == childItem.ChildUserID)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedChildID = null;

                            FullReportViewModel.SelectedAssessmentType = null;
                            FullReportViewModel.SelectedRecordForm = null;

                            FullReportViewModel.SelectedAssessmentID = 0;
                            FullReportViewModel.SelectedRecordFormID = null;

                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            FullReportViewModel.RunReport = false;
                        }
                        else
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedChildID = item.ChildUserID;
                            FullReportViewModel.SelectedChildName = item.Name;

                            FullReportViewModel.SelectedAssessmentID = 0;
                            FullReportViewModel.SelectedRecordFormID = null;

                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            FullReportViewModel.RunReport = false;

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
                foreach (var item in ReportViewModel.ChildRecords)
                {
                    if (item.Name == itemText)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            ReportViewModel.SelectedChild = "Select child";
                            ReportViewModel.OfflineStudentID = 0;

                            ReportViewModel.SelectedChild = null;
                            ReportViewModel.SelectedAssessmentType = null;
                            ReportViewModel.SelectedRecordForm = null;

                            ReportViewModel.SelectedAssessmentID = 0;
                            ReportViewModel.SelectedRecordFormID = 0;

                        }
                        else
                        {
                            item.selected = !item.selected;
                            ReportViewModel.FullName = ReportViewModel.SelectedChild = item.Name;
                            ReportViewModel.OfflineStudentID = item.OfflineStudentId;
                            ReportViewModel.DOB = item.DOB;
                            ReportViewModel.SelectedAssessmentID = 0;
                            ReportViewModel.SelectedRecordFormID = 0;
                            ReportViewModel.SelectedAssessmentType = null;
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
                foreach (var item in FullReportViewModel.ChildRecords)
                {
                    if (item.Name == itemText)
                    {
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedChildID = null;

                            FullReportViewModel.SelectedAssessmentType = null;
                            FullReportViewModel.SelectedRecordForm = null;

                            FullReportViewModel.SelectedAssessmentID = 0;
                            FullReportViewModel.SelectedRecordFormID = null;

                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            if (FullReportViewModel.SelectedNoteTypes != null && FullReportViewModel.SelectedNoteTypes.Count > 0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }
                            FullReportViewModel.RunReport = false;
                        }
                        else
                        {
                            item.selected = !item.selected;
                            FullReportViewModel.SelectedChildID = item.ChildUserID;
                            FullReportViewModel.SelectedChildName = item.Name;

                            FullReportViewModel.SelectedAssessmentID = 0;
                            FullReportViewModel.SelectedRecordFormID = null;

                            FullReportViewModel.IsScoringLayoutVisible = FullReportViewModel.IsStandardDeviationLayoutVisible = FullReportViewModel.IsBAESLayoutVisible = false;
                            FullReportViewModel.IsScoresCheckboxCheckedChanged = FullReportViewModel.IsSDScoresCheckboxCheckedChanged = FullReportViewModel.IsActivitiesCheckboxCheckedChanged = FullReportViewModel.IsDomainCheckboxCheckedChanged = false;
                            FullReportViewModel.ScoreSelected = null;
                            FullReportViewModel.RunReport = false;
                            if (FullReportViewModel.SelectedNoteTypes!= null && FullReportViewModel.SelectedNoteTypes.Count>0)
                            {
                                FullReportViewModel.SelectedNoteTypes.Clear();
                                FullReportViewModel.selectedNoteCount = 0;
                                FullReportViewModel.NoteSelected = "Select notes";
                            }

                        }
                    }
                    else
                    {
                        item.selected = false;
                    }
                }

            }
        }
    }

}