using System;
using BDI3Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace BDI3Mobile.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordView : ContentPage
    {
        private string _uName;
        public string UName
        {
            get
            {
                return _uName;
            }
            set
            {
                _uName = value;
            }
        }
        public ForgotPasswordView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new ForgotPasswordViewModel();
            UserName.Text = UName;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UserName.Text = UName;
        }

        public void canceleventhandler (object sender, EventArgs eventArgs)
        {
            Navigation.PushModalAsync(new LoginView());
        }

        private void TermsOfUse_Tapped(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://cms.riversideinsights.com/uploads/fae1a1e2deb44b19b54600af53d80866.pdf"));
        }
        private void PrivacyPolicy_Tapped(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri("https://cms.riversideinsights.com/uploads/e2f5cad99f33448d8c72cc57e71710fa.pdf"));
        }

    }
}