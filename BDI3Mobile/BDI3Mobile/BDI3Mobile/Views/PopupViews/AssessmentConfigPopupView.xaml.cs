using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.Models.Responses;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentConfigPopupView: PopupPage
    {
        public AssessmentConfigPopupViewModel AssessmentConfigPopupViewModel { get; set; }

        #region Construtor
        public AssessmentConfigPopupView(int offlinestudentid)
        {
            AssessmentConfigPopupViewModel = new AssessmentConfigPopupViewModel(offlinestudentid);
            BindingContext = AssessmentConfigPopupViewModel;
            InitializeComponent();
            AssessmentConfigPopupViewModel.Examiner = AssessmentConfigPopupViewModel.DefaultExaminer;
            AssessmentConfigPopupViewModel.ProgramNote = "Select a program label";


            AssessmentConfigPopupViewModel.SetExaminer = () =>
            {
                AssessmentConfigPopupViewModel.Examiner = AssessmentConfigPopupViewModel.DefaultExaminer;
            };

            if (Device.RuntimePlatform == Device.iOS)
            {
                //Removing frame tap gesture in iOS, as the tap gestures override the collection view gesture.
                MainGrid.GestureRecognizers.Remove(FrameGesture);
            }
            /*if (string.IsNullOrEmpty(Examiner.Text))
            {
                if (Application.Current.Properties.ContainsKey("Name"))
                {
                    Examiner.Text = Application.Current.Properties["Name"].ToString();
                }
                else
                {
                    Examiner.Text = "";
                }
            }*/

            DatePicker_AssessmentDate.DateSelected -= DatePicker_AssessmentDate_DateSelected;
            ProgramNote.Focused -= ProgramNote_Focused;
            Examiner.Focused -= Examiner_Focused;

            DatePicker_AssessmentDate.DateSelected += DatePicker_AssessmentDate_DateSelected;
            ProgramNote.Focused += ProgramNote_Focused;
            Examiner.Focused += Examiner_Focused;

            AssessmentConfigPopupViewModel.LoadData();
            AssessmentDate.Text = AssessmentConfigPopupViewModel.TestDate = DateTime.Now.ToString("MM/dd/yyyy");

        }
        #endregion

        #region EventHandlers
        private void DatePicker_AssessmentDate_DateSelected(object sender, DateChangedEventArgs e)
        {
            AssessmentConfigPopupViewModel.TestDate = e.NewDate.ToString("MM/dd/yyyy");
        }
        private void Examiner_Focused(object sender, FocusEventArgs e)
        {
            Examiner.Unfocus();
        }

        private void ProgramNote_Focused(object sender, FocusEventArgs e)
        {
            ProgramNote.Unfocus();
        }

        #endregion

        #region DatePickerOpen
        void Assessment_Tapped(object sender, EventArgs e)
        {
            var dobSplit = AssessmentConfigPopupViewModel.DOB.Split('/');
            var mindate = new DateTime(Convert.ToInt32(dobSplit[2]), Convert.ToInt32(dobSplit[0]), Convert.ToInt32(dobSplit[1]));
            DatePicker_AssessmentDate.ManualMinDate = true;
            DatePicker_AssessmentDate.MinimumDate = mindate;
            if (Device.RuntimePlatform == Device.UWP)
            {
                DatePicker_AssessmentDate.ShowDatePicker();
            }
            else
            {
                DatePicker_AssessmentDate.Focus();
            }
        }
        #endregion

        #region Overrides
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.RuntimePlatform == Device.UWP)
            {
                if (height > width)
                {
                    MainFrame.Margin = new Thickness(50, 130, 50, 130);
                }
                else
                {
                    MainFrame.Margin = new Thickness(180, 110, 180, 110);
                }
            }
           
        }
        #endregion
       
        private void EnabledStartAssessment()
        {
            string placeholderText = "select an examiner";
            AssessmentConfigPopupViewModel.IsStartEnabled = false;

            if (!string.IsNullOrEmpty(AssessmentConfigPopupViewModel.Examiner) && (AssessmentConfigPopupViewModel.Examiner.ToLower() != placeholderText) && !string.IsNullOrEmpty(this.AssessmentDate.Text) && !AssessmentConfigPopupViewModel.IsAgeRestricted)
            {
                if (AssessmentConfigPopupViewModel.SelectedItem != null)
                {
                    AssessmentConfigPopupViewModel.IsStartEnabled = true;
                    if (Device.RuntimePlatform != Device.iOS)
                    {
                        this.btnstart.BackgroundColor = Colors.FrameBlueColor;
                        this.btnstart.BorderColor = Colors.FrameBlueColor;
                    }
                    else
                    {
                        this.btnstartiOS.BorderColor = Colors.PrimaryColor;
                    }
                }
            }
        }       

        #region Examiner & ProgrameNote Lookups 
        private void ExaminerPopupButton_Clicked(object sender, EventArgs e)
        {
            //First time if the selected examiner already there; hightlight the same in listview
            
            //if (Device.RuntimePlatform != Device.iOS)
            //{
            //    if (selectExaminerListView.SelectedItem == null && !string.IsNullOrWhiteSpace(AssessmentConfigPopupViewModel.Examiner))
            //        selectExaminerListView.SelectedItem = AssessmentConfigPopupViewModel.Examiner;
            //    foreach (var item in AssessmentConfigPopupViewModel.ExaminerList)
            //    {
            //        if (item.text == AssessmentConfigPopupViewModel.Examiner)
            //        {
            //            item.selected = true;
            //        }
            //        else
            //        {
            //            item.selected = false;
            //        }
            //    }
            //    SelectExaminerFrame.IsVisible = true;
            //}
            //else
            //{
                foreach (var item in AssessmentConfigPopupViewModel.ExaminerList)
                {
                    if (item.text == AssessmentConfigPopupViewModel.Examiner)
                    {
                        item.selected = true;
                    }
                    else
                    {
                        item.selected = false;
                    }
                }
                PopupNavigation.Instance.PushAsync(new SelectExaminerPopupView(null, new Thickness(0, 0, 0, 0), AssessmentConfigPopupViewModel));

            //}

        }

        private void CloseExaminerPopup(object sender, EventArgs e)
        {            
            SelectExaminerFrame.IsVisible = false;
            SelectProgrameNoteFrame.IsVisible = false;
        }

        private void ExaminerListTapped(object sender, EventArgs e)
        {
            SelectExaminerFrame.IsVisible = true;
        }

        private void ProgramListTapped(object sender, EventArgs e)
        {
            SelectProgrameNoteFrame.IsVisible = true;
        }
        
        private void ProgramNotePopupButton_Clicked(object sender, EventArgs e)
        {
            //First time if the selected program note already there; hightlight the same in listview
            
            //if (Device.RuntimePlatform != Device.iOS)
            //{
            //    if (selectProgramNoteListView.SelectedItem == null && !string.IsNullOrWhiteSpace(ProgramNote.Text))
            //        selectProgramNoteListView.SelectedItem = ProgramNote.Text;
            //    SelectProgrameNoteFrame.IsVisible = true;
            //}
            //else
            //{
            PopupNavigation.Instance.PushAsync(new AddProgramLabelPopupView(null, AssessmentConfigPopupViewModel));
            //}
        }
        #endregion

        #region ItemSelected in Examiner & ProgrameNote Lookups
        private void ProgramNote_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AssessmentConfigPopupViewModel.ProgramNote = e.SelectedItem != null ? e.SelectedItem.ToString() : "";
            SelectProgrameNoteFrame.IsVisible = false;
            this.EnabledStartAssessment();
        }
        private void ProgramNote_Tapped(object sender, EventArgs e)
        {
            AssessmentConfigPopupViewModel.ProgramNote = ((Label)sender).Text;
            SelectProgrameNoteFrame.IsVisible = false;
        }
        //Bug Fix: CLINICAL - 4405
        /// <summary>
        /// On selection of examiner item from ExmainerListView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ExaminerTapped(object sender, ItemTappedEventArgs e)
        {
            var examiner = e.Item as Examiner;            
            foreach (var item in AssessmentConfigPopupViewModel.ExaminerList)
            {
                if (item.text == examiner.text)
                {
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        AssessmentConfigPopupViewModel.Examiner = "Select an examiner";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        AssessmentConfigPopupViewModel.Examiner = examiner.text.ToString();
                        selectExaminerListView.SelectedItem  = examiner.text;
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
            this.EnabledStartAssessment();
        }
        /// <summary>
        /// On selection of item from ProgramNoteListView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProgramLabelTapped(object sender, ItemTappedEventArgs e)
        {
            var programLabel = e.Item as ProgramNote;
            foreach (var item in AssessmentConfigPopupViewModel.ProgramNoteList)
            {
                if (item.text == programLabel.text)
                {
                    // Bug Fix: CLINICAL 4055
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        AssessmentConfigPopupViewModel.ProgramNote = "Select a program label";
                        selectProgramNoteListView.SelectedItem = "Select a program label";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        AssessmentConfigPopupViewModel.ProgramNote = programLabel.text.ToString();
                        selectProgramNoteListView.SelectedItem = programLabel.text;
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
            this.EnabledStartAssessment();
        }
        #endregion

        #region Close Lookups
        private void OuterGrid_Tapped(object sender, EventArgs e)
        {
            //DatePicker_AssessmentDate.IsShowPicker = false;
            //DatePicker_AssessmentDate.IsVisible = false;
            //Bug Fix: 4567
            //DatePicker_AssessmentDate.Unfocus();
            cancelButton.Focus();
            cancelButton.Unfocus();
        }
        private void ClosePopup_Tapped(object sender, EventArgs e)
        {
            SelectExaminerFrame.IsVisible = false;
            SelectProgrameNoteFrame.IsVisible = false;
        }

        #endregion
        void OnRecordFormCollectionViewItemSelected(object sender, SelectionChangedEventArgs e)
        {
            var selectedForm = AssessmentConfigPopupViewModel.SelectedItem;

            if (selectedForm.Description == AssignmentTypes.BattelleDevelopmentalCompleteString)
            {
                AssessmentConfigPopupViewModel.IsBattelleDevelopmentalCompleteChecked = selectedForm.IsChecked;
                AssessmentConfigPopupViewModel.IsBattelleDevelopmentalScreenerChecked = false;
                AssessmentConfigPopupViewModel.IsBattelleEarlyAcademicSurveyChecked = false;
                this.EnabledStartAssessment();
            }
            else if(selectedForm.Description == AssignmentTypes.BattelleDevelopmentScreenerString)
            {
                AssessmentConfigPopupViewModel.IsBattelleDevelopmentalScreenerChecked = selectedForm.IsChecked;
                AssessmentConfigPopupViewModel.IsBattelleEarlyAcademicSurveyChecked = false;
                AssessmentConfigPopupViewModel.IsBattelleDevelopmentalCompleteChecked = false;
                this.EnabledStartAssessment();
            }
            else
            {
                AssessmentConfigPopupViewModel.IsBattelleEarlyAcademicSurveyChecked = selectedForm.IsChecked;
                AssessmentConfigPopupViewModel.IsBattelleDevelopmentalCompleteChecked = false;
                AssessmentConfigPopupViewModel.IsBattelleDevelopmentalScreenerChecked = false;
                this.EnabledStartAssessment();
            }
        }

        private void AssessmentDate_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                var entryText = (sender as Label)?.Text;

                var dateTime = HelperMethods.DateValidation(entryText);
                var ageDiff = HelperMethods.CalculateAge(AssessmentConfigPopupViewModel.DOB, entryText);
                if (!string.IsNullOrEmpty(entryText) && dateTime.result)
                {
                    if(ageDiff != null && ((ageDiff[0] > 7) || ((ageDiff[0] == 7) && (ageDiff[1] >11)) || ((ageDiff[0] >= 7) && (ageDiff[1] >= 11) && (ageDiff[2] > 0))))
                    {
                        AssessmentConfigPopupViewModel.IsAgeRestricted = true;
                    }
                    else
                    {
                        AssessmentConfigPopupViewModel.IsAgeRestricted = false;
                    }
                }

                this.EnabledStartAssessment();
            }
        }
        private void btnstart_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                if ((sender as Button).IsEnabled)
                {
                    btnstart.BackgroundColor = Colors.PrimaryColor;
                }
                else
                {
                    btnstart.BackgroundColor = Color.Gray;

                }
            }
        }
        protected override void OnDisappearing()
        {
            try
            {
                this.AssessmentConfigPopupViewModel.OrgRecordFormList = null;
                this.AssessmentConfigPopupViewModel.ExaminerList = null;
                this.AssessmentConfigPopupViewModel.ProgramNoteList = null;
                this.MainLayout.Children.Clear();
                this.AssessmentConfigPopupViewModel.BindingContext = null;
                this.BindingContext = null;
                this.Content = null;
            }
            catch (Exception)
            {

            }
            GC.Collect();
            GC.SuppressFinalize(this);
            base.OnDisappearing();
        }
    }
}
