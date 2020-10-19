using BDI3Mobile.Common;
using BDI3Mobile.Models.ReportModel;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectNotesPopupView : PopupPage
    {
        FullReportViewModel ReportViewModel { get; set; }
        public SelectNotesPopupView(FullReportViewModel fullReportViewModel)
        {
                InitializeComponent();
                ReportViewModel = fullReportViewModel;
                BindingContext = fullReportViewModel;
                this.CloseWhenBackgroundIsClicked = true;
                this.BackgroundClicked += FullReportPopupView_BackgroundClicked;
                NotesListView.ItemTapped += FullReportNoteItemTapped;
                {
                    NotesPopupView.Margin = new Thickness(0, 10, 0, 0);
                }

                if (Device.RuntimePlatform != Device.Android)
                {
                    Frame1.IsVisible = NotesListView.IsVisible = true;
                    AndroidFrame.IsVisible = NotesCollectionView.IsVisible = false;
                }
                else
                {
                    Frame1.IsVisible = NotesListView.IsVisible = false;
                    AndroidFrame.IsVisible = NotesCollectionView.IsVisible = true;
                }

                if (ReportViewModel.selectedNoteCount == 0)
                {
                    foreach (var item in ReportViewModel.NoteTypeList)
                    {
                        if (item.IsSelected)
                        {
                            item.IsSelected = false;
                        }
                  }
                
                }
            }
        
            private void FullReportPopupView_BackgroundClicked(object sender, EventArgs e)
            {
            ReportViewModel.isNotesTypePopupOpen = false;

            if ((ReportViewModel.SelectedReportTypeID != 0 && ReportViewModel.SelectedReportTypeName != "Select a report type") && (ReportViewModel.SelectedLocations != null && ReportViewModel.SelectedLocations.Count > 0) && ReportViewModel.SelectedChildID != null
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
            }
            else
            {
                ReportViewModel.RunReport = false;
            }
        }
        private void FullReportNoteItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (ReportViewModel.SelectedNoteTypes == null)
                ReportViewModel.SelectedNoteTypes = new List<string>();
            var noteTypeItem = e.Item as NotesType;
            foreach (var item in ReportViewModel.NoteTypeList)
            {
                if (item.NoteTypeName == noteTypeItem.NoteTypeName)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        if (ReportViewModel.SelectedNoteTypes.Contains(item.NoteTypeName))
                            ReportViewModel.SelectedNoteTypes.Remove(item.NoteTypeName);
                        ReportViewModel.selectedNoteCount = ReportViewModel.SelectedNoteTypes.Count;
                        if (ReportViewModel.selectedNoteCount > 0 && ReportViewModel.selectedNoteCount <4)
                            ReportViewModel.NoteSelected = ReportViewModel.selectedNoteCount + " selected";
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedNoteTypes.Add(item.NoteTypeName);
                        ReportViewModel.selectedNoteCount = ReportViewModel.SelectedNoteTypes.Count;
                        if (ReportViewModel.selectedNoteCount > 0 && ReportViewModel.selectedNoteCount < 4)
                            ReportViewModel.NoteSelected = ReportViewModel.selectedNoteCount + " selected";
                    }

                    ReportViewModel.selectedNoteCount = ReportViewModel.SelectedNoteTypes.Count;
                    if(ReportViewModel.selectedNoteCount > 0 && ReportViewModel.selectedNoteCount < 4)
                        ReportViewModel.NoteSelected = ReportViewModel.selectedNoteCount + " selected";
                    else
                        ReportViewModel.NoteSelected = "Select notes";

                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (ReportViewModel.SelectedNoteTypes == null)
                ReportViewModel.SelectedNoteTypes = new List<string>();
            var itemSelected = sender as StackLayout;
            var label = itemSelected.Children[1] as Label;
            var itemText = label.Text;

            foreach (var item in ReportViewModel.NoteTypeList)
            {
                if (item.NoteTypeName == itemText)
                {
                    if (item.IsSelected)
                    {
                        item.IsSelected = !item.IsSelected;
                        if (ReportViewModel.SelectedNoteTypes.Contains(item.NoteTypeName))
                            ReportViewModel.SelectedNoteTypes.Remove(item.NoteTypeName);
                        ReportViewModel.selectedNoteCount = ReportViewModel.SelectedNoteTypes.Count;
                        if (ReportViewModel.selectedNoteCount > 0 && ReportViewModel.selectedNoteCount < 4)
                            ReportViewModel.NoteSelected = ReportViewModel.selectedNoteCount + " selected";
                    }
                    else
                    {
                        item.IsSelected = !item.IsSelected;
                        ReportViewModel.SelectedNoteTypes.Add(item.NoteTypeName);
                        ReportViewModel.selectedNoteCount = ReportViewModel.SelectedNoteTypes.Count;
                        if (ReportViewModel.selectedNoteCount > 0 && ReportViewModel.selectedNoteCount < 4)
                            ReportViewModel.NoteSelected = ReportViewModel.selectedNoteCount + " selected";
                    }

                    ReportViewModel.selectedNoteCount = ReportViewModel.SelectedNoteTypes.Count;
                    if (ReportViewModel.selectedNoteCount > 0 && ReportViewModel.selectedNoteCount < 4)
                        ReportViewModel.NoteSelected = ReportViewModel.selectedNoteCount + " selected";
                    else
                        ReportViewModel.NoteSelected = "Select notes";

                }
            }
        }
        }
}