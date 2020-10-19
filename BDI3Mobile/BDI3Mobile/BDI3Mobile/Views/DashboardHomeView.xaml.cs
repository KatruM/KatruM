using Acr.UserDialogs;
using BDI3Mobile.IServices;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDI3Mobile.Views
{
    public partial class DashboardHomeView : ContentPage
    {
        DashboardpageViewModel ViewModel;
        private bool isSupportPopupOpen;
        public DashboardHomeView()
        {
            InitializeComponent();
            ViewModel = new DashboardpageViewModel();
            this.BindingContext = ViewModel;

            if (Xamarin.Essentials.Connectivity.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {
                this.imgSync.IsVisible = true;
                // this.imgSyncDisable.IsVisible = false;
            }
            else
            {
                this.imgSync.IsVisible = false;
                //this.imgSyncDisable.IsVisible = true;
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            this.imgSync.IsVisible = !(e.NetworkAccess == NetworkAccess.None);
        }

        private async void SupportIconTapped(object sender, EventArgs e)
        {
            if (isSupportPopupOpen)
                return;
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                {
                    await PopupNavigation.Instance.PopAsync();
                }
            }

            isSupportPopupOpen = true;
            await PopupNavigation.Instance.PushAsync(new DashboardpopupView(ViewModel.DeviceID));
            isSupportPopupOpen = false;
        }

        private async void Syncing(object sender, EventArgs e)
        {
            try
            {
                await ViewModel.CheckFirstTimeAndDownload();

            }
            catch (Exception)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //TO DO: Add the pop up navigation stack count check
                    if (PopupNavigation.Instance.PopupStack.Count > 0)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAllAsync();
                            await UserDialogs.Instance.AlertAsync("Download Failed!");
                        }
                    }
                });
            }

        }

        private async void DashboardHelp_Tapped(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await Launcher.OpenAsync(new Uri("http://onlinehelp.riversideinsights.com/Help/BDI3_Mobile/index.htm"));
            }
            else
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    var createhtmlFile = DependencyService.Get<ICreateHtmlFiles>();
                    await createhtmlFile.SaveFile("BDI3_Mobile\\index.htm");

                }
                else if (Device.RuntimePlatform == Device.iOS)
                {

                    var iOSPath = Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, "BDI3_Mobile/index.htm");
                    await Navigation.PushModalAsync(new InAppBrowserXaml(iOSPath));

                }
                else
                {
                    var iOSPath = "file://" + Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, "BDI3_Mobile/index.htm");
                    await Navigation.PushModalAsync(new InAppBrowserXaml(iOSPath));
                }
            }

        }

        protected override void OnAppearing()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;

        }

    }

}
