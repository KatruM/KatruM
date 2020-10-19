using System;
using BDI3Mobile.Common;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChildInformationpageView : ContentPage
    {
        public ChildInformationpageViewModel viewModel { get; set; }
        public ChildInformationpageView(int offlineStudentID)
        {
            viewModel = new ChildInformationpageViewModel(offlineStudentID);
            BindingContext = viewModel;
            InitializeComponent();
            viewModel.ResetContent = new Action(ResetContent);
            AwesomeCheckbox.FillColor = Common.Colors.LightBlueColor;
            AwesomeCheckbox.OutlineColor = Common.Colors.LightBlueColor;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (((sender as Label).BindingContext as ChildInformationRecord).StatusCode != -1 && ((sender as Label).BindingContext as ChildInformationRecord).StatusCode != 0)
            {
                var model = ((sender as Label).BindingContext as ChildInformationRecord);
                var getErrorMessage = GetErrorMessage(model.StatusCode);
                await PopupNavigation.Instance.PushAsync(new GenericFailuretoCommitView() { BindingContext = new GenericFailuretoCommitViewModel() { ErrorMessage = getErrorMessage } });
            }
        }

        private void Checkbox_IsCheckedChanged(object sender, TappedEventArgs e)
        {
            
        }

        private void Checkbox1_IsCheckedChanged(object sender, TappedEventArgs e)
        {

        }

        private void Checkbox2_IsCheckedChanged(object sender, TappedEventArgs e)
        {

        }

        private void Checkbox3_IsCheckedChanged(object sender, TappedEventArgs e)
        {

        }

        private void Checkbox4_IsCheckedChanged(object sender, TappedEventArgs e)
        {

        }

        private void Checkbox5_IsCheckedChanged(object sender, TappedEventArgs e)
        {

        }

        private void Checkbox6_IsCheckedChanged(object sender, TappedEventArgs e)
        {

        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            try
            {
                this.ListView.DisposeListView?.Invoke();
                viewModel.BindingContext = null;
                viewModel = null;
                this.BindingContext = null;
                this.MainGrid.Children.Clear();
                this.Content = null;
                App.Current.MainPage = new DashboardHomeView();
            }
            catch (Exception)
            {
            }
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        private void CheckBox_CheckedChanged(object sender, TappedEventArgs e)
        {
            try
            {
                var checkBox = ((IntelliAbb.Xamarin.Controls.Checkbox)sender);
                var model = (ChildInformationpageViewModel)BindingContext;
                model.CheckBoxChanged(checkBox.IsChecked);
            }
            catch(Exception ex)
            {

            }
        }

        private void btnSync_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((sender as Button).IsEnabled)
            {
                SyncButton.BackgroundColor = SyncButton.BorderColor = Colors.PrimaryColor;
                 
            }
            else
            {
                SyncButton.BackgroundColor = SyncButton.BorderColor = Color.Gray;

            }
        }

        private void btnDelete_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((sender as Button).IsEnabled)
            {
                DeleteButton.BackgroundColor = DeleteButton.BorderColor = Colors.PrimaryColor;
            }
            else
            {
                DeleteButton.BackgroundColor = DeleteButton.BorderColor = Color.Gray;

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
        private void ResetContent()
        {
            try
            {
                this.ListView.DisposeListView?.Invoke();
                this.BindingContext = null;
                this.viewModel.BindingContext = null;
                this.viewModel = null;
                this.MainGrid.Children.Clear();
                this.Content = null;
            }
            catch(Exception ex)
            {

            }
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        protected override void OnDisappearing()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
            base.OnDisappearing();
        }

    }
}