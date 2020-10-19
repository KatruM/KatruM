using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using BDI3Mobile.Views.PopupViews;
using System.Globalization;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BDI3Mobile.IServices;
using Xamarin.Essentials;

namespace BDI3Mobile.Views.AssesmentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResumeSyncAssesmentView : ContentPage
    {
        public ResumeSyncAssessmentViewModel MyResumeSyncViewModel { get; set; }
        public ResumeSyncAssesmentView()
        {
            MyResumeSyncViewModel = new ResumeSyncAssessmentViewModel();
            MyResumeSyncViewModel.ResetContent = ClearContent;
            BindingContext = MyResumeSyncViewModel;
            InitializeComponent();
            if (Device.RuntimePlatform != Device.iOS)
            {
                btnCommit.IsEnabled = btnDelete.IsEnabled = false;
            }
            else
            {
                btnCommitiOS.IsEnabled = btnDeleteiOS.IsEnabled = false;

            }
            AwesomeCheckbox.FillColor = Colors.LightBlueColor;
            AwesomeCheckbox.OutlineColor = Colors.LightBlueColor;

            TestDate.Unfocused -= TestDate_Unfocused;
            TestDate.Focused -= TestDate_Focused;
            TestDate.TextChanged -= TestDate_TextChanged;

            TestDate.Unfocused += TestDate_Unfocused;
            TestDate.Focused += TestDate_Focused;
            TestDate.TextChanged += TestDate_TextChanged;

            TestName.Focused += TestName_Focused;
            ChildFirstName.Focused += ChildFirstName_Focused;
            ChildLastName.Focused += ChildLastName_Focused;


            MyResumeSyncViewModel.PropertyChanged -= MyResumeSyncViewModel_PropertyChanged;
            MyResumeSyncViewModel.PropertyChanged += MyResumeSyncViewModel_PropertyChanged;
        }
        private void ChildLastName_Focused(object sender, FocusEventArgs e)
        {
            if (MyResumeSyncViewModel.ShowTestSelection)
            {
                MyResumeSyncViewModel.ShowTestSelection = false;
                ChildLastName.Unfocus();
            }
        }
        private void ChildFirstName_Focused(object sender, FocusEventArgs e)
        {
            if (MyResumeSyncViewModel.ShowTestSelection)
            {
                MyResumeSyncViewModel.ShowTestSelection = false;
                ChildFirstName.Unfocus();
            }
        }
        private void TestName_Focused(object sender, FocusEventArgs e)
        {
            if (MyResumeSyncViewModel.ShowTestSelection)
            {
                MyResumeSyncViewModel.ShowTestSelection = false;
                TestName.Unfocus();
            }
        }
        private void MyResumeSyncViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectAll")
            {
                if (MyResumeSyncViewModel.SelectAll)
                {
                }
                else
                {
                }
            }
        }
        private void TestDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            string entryText = (sender as Entry)?.Text;
            if (entryText != null && entryText.Length == 10)
            {
                if (this.BindingContext != null && entryText != "mm/dd/yyyy")
                {
                    ((ResumeSyncAssessmentViewModel)this.BindingContext).TestDateIsValid = false;
                    TestDateStackLayout.BackgroundColor = TestDate.BackgroundColor = Color.White;
                    TestDate.TextColor = Colors.LightGrayColor;
                    TestDate.FontAttributes = FontAttributes.None;
                    TestDateFrame.BorderColor = Color.FromHex("#898D8D");
                    TestDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                    TestDateImageFrame.BorderColor = Color.FromHex("#898D8D");
                }
            }
        }
        private void TestDate_Focused(object sender, FocusEventArgs e)
        {
            if (MyResumeSyncViewModel.ShowTestSelection)
            {
                MyResumeSyncViewModel.ShowTestSelection = false;
                TestDate.Unfocus();
            }


            var entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                TestDate.Text = entryText;
            }
            else
            {
                ((ResumeSyncAssessmentViewModel)this.BindingContext).TestDateIsValid = false;
                TestDateStackLayout.BackgroundColor = TestDate.BackgroundColor = Color.White;
                TestDate.Text = "";
                TestDate.TextColor = Colors.LightGrayColor;
                TestDate.FontAttributes = FontAttributes.None;
                TestDateFrame.BorderColor = Color.FromHex("#898D8D");
                TestDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
                TestDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            }

            
        }
        private void TestDate_Unfocused(object sender, FocusEventArgs e)
        {
            string entryText = (sender as Entry)?.Text;
            DateFormatStructure dateTime = HelperMethods.DateValidation(entryText);
            if (!string.IsNullOrEmpty(entryText) && !dateTime.result)
            {
                ((ResumeSyncAssessmentViewModel)this.BindingContext).TestDateIsValid = true;
                TestDateStackLayout.BackgroundColor = TestDate.BackgroundColor = Color.FromHex("#FFF1F1");
                TestDate.Text = "mm/dd/yyyy";
                TestDate.TextColor = Color.FromHex("CC1417");
                TestDate.FontAttributes = FontAttributes.Bold;
                TestDateFrame.BorderColor = Color.FromHex("#CC1417");
                TestDateImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                TestDateImageFrame.BorderColor = Color.FromHex("#CC1417");
            }
        }
        void TestDate_Tapped(object sender, System.EventArgs e)
        {
            if (MyResumeSyncViewModel.ShowTestSelection)
            {
                MyResumeSyncViewModel.ShowTestSelection = false;
                TestDate.Unfocus();
                return;
            }
            if (Device.RuntimePlatform == Device.UWP)
            {
                DOB_Picker.ShowDatePicker();
            }
            else
            {
                DOB_Picker.Focus();
            }
        }
        void SelectStatusTapped(object sender, System.EventArgs e)
        {
            MyResumeSyncViewModel.ShowTestSelection = !MyResumeSyncViewModel.ShowTestSelection;
        }
        private void Checkbox_IsCheckedChanged(object sender, TappedEventArgs e)
        {
            try { 
            MyResumeSyncViewModel.ShowTestSelection = false;
            var checkBox = ((IntelliAbb.Xamarin.Controls.Checkbox)sender);
            var model = (ResumeSyncAssessmentViewModel)BindingContext;
            var checkBoxContext = (ChildAssessmentRecord)checkBox.BindingContext;
                if (checkBoxContext != null) {
                    checkBoxContext.IsSelected = checkBox.IsChecked;
                    model.ChildAssessmentRecords.FirstOrDefault(p => p.LocalTestInstance == checkBoxContext.LocalTestInstance).IsSelected = checkBox.IsChecked;
                    model.CheckBoxChanged();
                }
            }
            catch(Exception ex)
            {

            }
        }
        private async void Dashboard_TappedAsync(object sender, EventArgs e)
        {
            try
            {
                this.BindingContext = null;
                this.teststatus.DisposeListView?.Invoke();
                this.TestRecordListView.DisposeListView?.Invoke();
                this.MainGrid.Children.Clear();
                this.Content = null;
                MyResumeSyncViewModel.BindingContext = null;
                MyResumeSyncViewModel = null;
            }
            catch(Exception ex)
            {

            }
            GC.Collect();
            GC.SuppressFinalize(this);
            await Navigation.PopModalAsync();
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.RuntimePlatform == Device.iOS)
            {
                // AscendingLabel.FontSize = 25;
                TestRecordListView.SeparatorVisibility = SeparatorVisibility.None;
            }
            if (Device.RuntimePlatform == Device.UWP)
            {
                SearchDeleteChild.VerticalOptions = LayoutOptions.End;
                //AscendingLabel.FontSize = 25;
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                SearchDeleteChild.VerticalOptions = LayoutOptions.Center;
                //AscendingLabel.FontSize = 25;
            }

            if (height > width)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    NameRow.Height = new GridLength(1, GridUnitType.Star);
                    EnrollmentRow.Height = new GridLength(1, GridUnitType.Star);
                    SeparatorRow.Height = new GridLength(0.3, GridUnitType.Star);
                    TableRow.Height = new GridLength(8, GridUnitType.Star);
                    commitanddeleteButtonRow.Height = new GridLength(0.7, GridUnitType.Star);
                    Thickness dobThickness = TestDateErrorText.Margin;
                    TestDateErrorText.Margin = new Thickness(dobThickness.Left, -5, dobThickness.Right, dobThickness.Bottom);
                }
                if (Device.RuntimePlatform == Device.Android)
                {
                    NameRow.Height = new GridLength(1.5, GridUnitType.Star);
                    EnrollmentRow.Height = new GridLength(1.5, GridUnitType.Star);
                    SeparatorRow.Height = new GridLength(0.3, GridUnitType.Star);
                    TableRow.Height = new GridLength(8, GridUnitType.Star);
                    commitanddeleteButtonRow.Height = new GridLength(0.7, GridUnitType.Star);
                    TestDateErrorText.Padding = new Thickness(10, -13, 100, 0);
                    Thickness dobThickness = TestDateErrorText.Margin;
                    TestDateErrorText.Margin = new Thickness(dobThickness.Left, -20, dobThickness.Right, dobThickness.Bottom);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    Thickness dobThickness = TestDateErrorText.Margin;
                    TestDateErrorText.Margin = new Thickness(dobThickness.Left, -15, dobThickness.Right, dobThickness.Bottom);
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    TestDateErrorText.Padding = new Thickness(10, -10, 100, 0);
                    Thickness dobThickness = TestDateErrorText.Margin;
                    TestDateErrorText.Margin = new Thickness(dobThickness.Left, 0, dobThickness.Right, dobThickness.Bottom);
                }

                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                {
                    NameRow.Height = new GridLength(2, GridUnitType.Star);
                    EnrollmentRow.Height = new GridLength(2, GridUnitType.Star);
                    SeparatorRow.Height = new GridLength(1, GridUnitType.Star);
                    TableRow.Height = new GridLength(8, GridUnitType.Star);
                    commitanddeleteButtonRow.Height = new GridLength(1, GridUnitType.Star);
                }
            }
        }
        private void ChildRecords_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void OuterGrid_Tapped(object sender, EventArgs e)
        {
            DOB_Picker.IsShowPicker = false;
            DOB_Picker.IsVisible = false;
            DOB_Picker.Unfocus();
            MyResumeSyncViewModel.ShowTestSelection = false;
        }
        private void SearchButtonClicked(object sender, EventArgs args)
        {
            string testDate = null;
            if (!string.IsNullOrEmpty(TestDate.Text))
            {
                DateTime temp;
                if (DateTime.TryParse(TestDate.Text, out temp))
                {
                    testDate = temp.ToString("MM/dd/yyyy");
                }
            }
            if (!string.IsNullOrEmpty(TestDate.Text))
            {
                MyResumeSyncViewModel.Search(TestDate.Text.ToString());
            }
            else
            {
                MyResumeSyncViewModel.Search("");
            }
            MyResumeSyncViewModel.ShowTestSelection = false;

        }
        private void ReloadButtonClicked(object sender, EventArgs args)
        {
            TestDate.Text = "";
            TestDate.Text = "";
            TestDateStackLayout.BackgroundColor = TestDate.BackgroundColor = Color.White;
            TestDate.TextColor = Colors.LightGrayColor;
            TestDate.FontAttributes = FontAttributes.None;
            MyResumeSyncViewModel.Status = "";
            MyResumeSyncViewModel.TestName = "";
            MyResumeSyncViewModel.FirstName = "";
            MyResumeSyncViewModel.LastName = "";
            MyResumeSyncViewModel.TestDateIsValid = false;
            MyResumeSyncViewModel.SearchErrorVisible = false;
            TestDateFrame.BorderColor = Color.FromHex("#898D8D");
            TestDateImageFrame.BorderColor = Color.FromHex("#898D8D");
            TestDateImageFrame.BackgroundColor = Color.FromHex("147cbd");
            MyResumeSyncViewModel.Search("");
            MyResumeSyncViewModel.ShowTestSelection = false;

            foreach (var item in MyResumeSyncViewModel.TestRecordStatusList)
            {
                item.Selected = false;
            }
        }
        private void TestStatus_Selected(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as ResumeSyncAssessmentViewModel)?.StatusSelectionCommand.Execute(e.Item);
           
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (((sender as Label).BindingContext as ChildAssessmentRecord).StatusCode != -1 && ((sender as Label).BindingContext as ChildAssessmentRecord).StatusCode != 0)
            {
                var model = ((sender as Label).BindingContext as ChildAssessmentRecord);
                var getErrorMessage = GetErrorMessage(model.StatusCode);
                await PopupNavigation.Instance.PushAsync(new GenericFailuretoCommitView() { BindingContext = new GenericFailuretoCommitViewModel() { ErrorMessage = getErrorMessage } });
            }
        }
        private string GetErrorMessage(int statuscode)
        {
            if (statuscode == 0)
            {
                return "An unexpected error occurred with status code 0. Please contact your administrator for more information.";
            }
            if (statuscode == 2)
            {
                return "The Child Record for the data you are attempting to sync has been deleted on the web.  Please restore the Child Record and try again.";
            }
            if (statuscode == 3)
            {
                return "The Child Record for the data you are attempting to sync has been deleted on the web.  Please restore the Child Record and try again.";
            }
            if (statuscode == 4)
            {
                return "An unexpected error occurred with status code 4. Please contact your administrator for more information.";
            }
            if (statuscode == 5)
            {
                return "You no longer have access to the Child Location. Please contact your administrator to return Location access.";
            }
            if (statuscode == 6)
            {
                return "You no longer have access to the Child Location. Please contact your administrator to return Location access.";
            }
            if (statuscode == 7)
            {
                return "An unexpected error occurred with status code 7. Please contact your administrator for more information.";
            }
            if (statuscode == 8)
            {
                return "An unexpected error occurred with status code 8. Please contact your administrator for more information.";
            }
            if (statuscode == 9)
            {
                return "An unexpected error occurred with status code 9. Please contact your administrator for more information.";
            }
            if (statuscode == 10)
            {
                return "An unexpected error occurred with status code 10. Please contact your administrator for more information.";
            }
            if (statuscode == 11)
            {
                return "An unexpected error occurred with status code 11. Please contact your administrator for more information.";
            }
            if (statuscode == 12)
            {
                return "An unexpected error occurred with status code 12. Please contact your administrator for more information.";
            }
            if (statuscode == 13)
            {
                return "An unexpected error occurred with status code 13. Please contact your administrator for more information.";
            }
            if (statuscode == 14)
            {
                return "An unexpected error occurred with status code 14. Please contact your administrator for more information.";
            }
            if (statuscode == 15)
            {
                return "An unexpected error occurred with status code 15. Please contact your administrator for more information.";
            }
            if (statuscode == 16)
            {
                return "An unexpected error occurred with status code 16. Please contact your administrator for more information.";
            }
            if (statuscode == 17)
            {
                return "An unexpected error occurred with status code 17. Please contact your administrator for more information.";
            }
            if (statuscode == 18)
            {
                return "An unexpected error occurred with status code 18. Please contact your administrator for more information.";
            }
            if (statuscode == 19)
            {
                return "An unexpected error occurred with status code 19. Please contact your administrator for more information.";
            }
            if (statuscode == 20)
            {
                return "An unexpected error occurred with status code 20. Please contact your administrator for more information.";
            }
            if (statuscode == 21)
            {
                return "The BDI-3 assessments are designed to assess children 7y11mos and under. If you are receiving this in error, please check your child’s date of birth and test date and try again.";
            }
            if (statuscode == 22)
            {
                return "An unexpected error occurred with status code 22. Please contact your administrator for more information.";
            }
            if (statuscode == 23)
            {
                return "You have insufficient Electronic Record Forms (ERF’s) to sync the selected data to the web.  Please contact your administrator to add more ERF’s and try again.";
            }
            if (statuscode == 24)
            {
                return "An unexpected error occurred with status code 24. Please contact your administrator for more information.";
            }
            if (statuscode == 25)
            {
                return "An unexpected error occurred with status code 25. Please contact your administrator for more information.";
            }
            if (statuscode == 26)
            {
                return "An unexpected error occurred with status code 26. Please contact your administrator for more information.";
            }
            if (statuscode == 27)
            {
                return "You have insufficient Electronic Record Forms (ERF’s) to sync the selected data to the web.  Please contact your administrator to add more ERF’s and try again.";
            }
            if (statuscode == 28)
            {
                return "An unexpected error occurred with status code 28. Please contact your administrator for more information.";
            }
            return null;
        }
        private void Records_SizeChanged(object sender, EventArgs e)
        {
            var listview = (ListView)sender;
            MyResumeSyncViewModel.UpdateListviewBoundCommand.Execute(listview.Height);
        }
        private void btnCommit_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                if ((sender as Button).IsEnabled)
                {
                    btnCommit.BackgroundColor = Colors.PrimaryColor;
                }
                else
                {
                    btnCommit.BackgroundColor = Color.Gray;

                }
            }
        }
        private void btnDelete_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                if ((sender as Button).IsEnabled)
                {
                    btnDelete.BackgroundColor = Colors.PrimaryColor;
                }
                else
                {
                    btnDelete.BackgroundColor = Color.Gray;

                }
            }
        }
        private void ClearContent()
        {
            try
            {
                this.BindingContext = null;
                this.teststatus.DisposeListView?.Invoke();
                this.TestRecordListView.DisposeListView?.Invoke();
                this.MainGrid.Children.Clear();
                this.Content = null;
                MyResumeSyncViewModel.BindingContext = null;
                MyResumeSyncViewModel = null;
            }
            catch(Exception ex)
            {

            }
            GC.Collect();
        }
        protected override void OnDisappearing()
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            }
            catch (Exception)
            {

            }
            base.OnDisappearing();
        }
    }
}
