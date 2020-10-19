using System;
using BDI3Mobile.Models.Responses;
using BDI3Mobile.ViewModels;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace BDI3Mobile.Views.PopupViews
{
    public partial class AddProgramLabelPopupView : PopupPage
    {
        TestSessionOverViewModel _viewModel;
        AssessmentConfigPopupViewModel _assessmentConfigPopupViewModel;
        public AddProgramLabelPopupView(TestSessionOverViewModel vM, AssessmentConfigPopupViewModel assessmentConfigPopupViewModel=null)
        {
            InitializeComponent();
            
            if (vM != null)
            {
                _viewModel = vM;
                BindingContext = vM;
                this.BackgroundClicked += AddProgramLabelPopupView_BackgroundClicked;

                if (_viewModel.ProgramLabel != null)
                {
                    ProgramNoteListView.SelectedItem = _viewModel.ProgramLabel;
                    foreach (var item in _viewModel.ProgramNoteList)
                    {
                        if (item.text == _viewModel.ProgramLabel)
                        {
                            item.selected = true;
                        }
                        else
                        {
                            item.selected = false;
                        }
                    }
                }
            }
            else if(assessmentConfigPopupViewModel!=null)
            {
                MainFrame.Margin = new Thickness(0, 0, 200, 0);
                _assessmentConfigPopupViewModel = assessmentConfigPopupViewModel;
                BindingContext = assessmentConfigPopupViewModel;
                CloseWhenBackgroundIsClicked = true;
            }
            //Bug Fix: CLINICAL - 4405
            
        }

        private void AddProgramLabelPopupView_BackgroundClicked(object sender, EventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.IsProgramLabelClicked = false;
            }
           
        }

        //Bug Fix: CLINICAL - 4405
        /// <summary>
        /// On selection of item from ProgramListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgramNote_Tapped(object sender, ItemTappedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.IsProgramLabelClicked = false;
                var programNote = e.Item as ProgramNote;
                foreach (var item in _viewModel.ProgramNoteList)
                {
                    if (item.text == programNote.text)
                    {
                        // Bug Fix: CLINICAL 4055
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            _viewModel.ProgramLabel = "Select a program label";
                        }
                        else
                        {
                            item.selected = !item.selected;
                            _viewModel.ProgramLabel = programNote.text.ToString();
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
                var programLabel = e.Item as ProgramNote;
                foreach (var item in _assessmentConfigPopupViewModel.ProgramNoteList)
                {
                    if (item.text == programLabel.text)
                    {
                        // Bug Fix: CLINICAL 4055
                        if (item.selected)
                        {
                            item.selected = !item.selected;
                            _assessmentConfigPopupViewModel.ProgramNote = "";
                            ProgramNoteListView.SelectedItem = _assessmentConfigPopupViewModel.ProgramNote = "Select a program label";
                        }
                        else
                        {
                            item.selected = !item.selected;
                            _assessmentConfigPopupViewModel.ProgramNote = programLabel.text.ToString();
                            ProgramNoteListView.SelectedItem = programLabel.text.ToString();
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
