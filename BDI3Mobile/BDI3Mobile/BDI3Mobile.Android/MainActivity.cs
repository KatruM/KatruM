using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Services;
using System.Windows.Input;
using Xamarin.Forms;
using BDI3Mobile.Droid.DependencyServices;
using BDI3Mobile.IServices;

namespace BDI3Mobile.Droid
{
    [Activity(Label = "BDI3Mobile", Icon = "@drawable/myicon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Acr.UserDialogs.UserDialogs.Init(this);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            Xamarin.Forms.DependencyService.Register<IKeyboardHelper, DroidKeyboardHelper>();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            ConfigureSession();
            App.SqlFilePath = Android.App.Application.Context.FilesDir.AbsolutePath;
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(true);
            LoadApplication(new App());

        }
        public override void OnUserInteraction()
        {
            base.OnUserInteraction();
            SessionManager.SessionManager.Instance.ExtendSession();
        }
        private void ConfigureSession()
        {
            SessionManager.SessionManager.Instance.StartSessionTimer();
        }
    }
}