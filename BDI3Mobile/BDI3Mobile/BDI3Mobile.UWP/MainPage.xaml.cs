using Rg.Plugins.Popup.Services;
using System;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml.Navigation;

namespace BDI3Mobile.UWP
{
    public sealed partial class MainPage
    {
        private IActivatedEventArgs e;

        public MainPage()
        {
            Rg.Plugins.Popup.Popup.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            Xamarin.Forms.Forms.Init(e, Rg.Plugins.Popup.Popup.GetExtraAssemblies());
            this.InitializeComponent();
            BDI3Mobile.App.SqlFilePath = ApplicationData.Current.LocalFolder.Path;
            var app = new BDI3Mobile.App();
            
            LoadApplication(app);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged += onIsIdleChanged;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            App.Current.IsIdleChanged -= onIsIdleChanged;
        }

        private async void onIsIdleChanged(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"IsIdle: {App.Current.IsIdle}");
            if(App.Current.IsIdle)
            {
                var popUpNavigationInstance = PopupNavigation.Instance;
                var popup = new Views.PopupViews.CustomPopupView(new Views.PopupViews.CustomPopUpDetails() { Header = "Session Expiring", Message = "Session will expire in 5 minutes, press Continue to extend time.",  Height = 211, Width= 450 });
                await popUpNavigationInstance.PushAsync(popup);
                return;
            }
        }
    }
}
