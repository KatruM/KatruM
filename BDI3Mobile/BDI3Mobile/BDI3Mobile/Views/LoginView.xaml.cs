using IntelliAbb.Xamarin.Controls;
using System;
using BDI3Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace BDI3Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        LoginViewModel LoginViewModel;
        public LoginView(bool isSessionExpired = false)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoginViewModel = new LoginViewModel();
            BindingContext = LoginViewModel;
            ((LoginViewModel)BindingContext).InActivityErrorMessageVisible = isSessionExpired;
            var checkbox = new Checkbox
            {
                OutlineColor = Color.Purple,
                FillColor = Color.Purple,
                CheckColor = Color.White,
                Shape = Shape.Rectangle
            };
            AwesomeCheckbox.FillColor = Common.Colors.BlueColor;
            AwesomeCheckbox.OutlineColor = Common.Colors.chekboxColor;
            LoginViewModel.ClearContent = ClearContent;
        }

        public void ClearContent()
        {
            try
            {
                this.BindingContext = null;
                this.MainGridView.Children.Clear();
                this.LoginViewModel.BindingContext = null;
                this.LoginViewModel = null;
                this.Content = null;
            }
            catch(Exception ex)
            {
            }
            GC.Collect();
            GC.SuppressFinalize(this);
        }

        private async void TermsOfUse_Tapped(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await Launcher.OpenAsync(new Uri("https://cms.riversideinsights.com/uploads/fae1a1e2deb44b19b54600af53d80866.pdf"));
            }
        }
        private async void PrivacyPolicy_Tapped(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await Launcher.OpenAsync(new Uri("https://cms.riversideinsights.com/uploads/e2f5cad99f33448d8c72cc57e71710fa.pdf"));
            }
        }

        private void EnterEvent(object sender, EventArgs e)
        {
            btSignIn.Command.Execute(null);

        }

        private async void Helplink_Tapped(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await Launcher.OpenAsync(new Uri("http://onlinehelp.riversideinsights.com/Help/OSR/index.htm#t=Getting_Started%2FLog_In.htm"));
            }
        }
    }
}