using BDI3Mobile.IServices;
using BDI3Mobile.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDI3Mobile.Views.AddChildViews
{
    public partial class AddUserFieldView : ContentPage
    {
        public Grid Grid;
        public Action SaveChildAction { get; set; }
        public AddUserFieldView()
        {
            InitializeComponent();
            Grid = MainGrid;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }
        public void SaveChildTapped(object sender, EventArgs e)
        {
            SaveChildAction.Invoke();
        }

        protected override void OnAppearing()
        {
            try
            {
                if (this.BindingContext != null)
                {
                    var viewModel = (AddChildViewModel)this.BindingContext;
                    this.lblTitle.Text = viewModel.OfflineStudentID > 0 ? "EDIT CHILD INFORMATION" : "ADD CHILD INFORMATION";
                }
            }
            catch (Exception)
            {

            }
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
