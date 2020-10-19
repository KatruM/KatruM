using System;

using Xamarin.Forms;

namespace BDI3Mobile.Views
{
    public partial class InAppBrowserXaml : ContentPage
    {
        public InAppBrowserXaml(string URL)
        {
            InitializeComponent();
          
            var urlSource = new UrlWebViewSource();
            urlSource.Url = URL;
            webView.Source = urlSource;
        }

        async void OnBackButtonClicked(object sender, EventArgs e)
        {

            await Application.Current.MainPage.Navigation.PushModalAsync(new DashboardHomeView());
        }
        }
}
