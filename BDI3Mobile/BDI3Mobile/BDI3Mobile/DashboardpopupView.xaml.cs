using System;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboardpopupView
    {
        public DashboardpopupView(string deviceid)
        {
            InitializeComponent();
            this.deviceid.Text = deviceid;
            this.CloseWhenBackgroundIsClicked = true;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var popNavigationInstance = PopupNavigation.Instance;
            
            popNavigationInstance.PopAsync();
        }

        private void Navigate_CustomerCare(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new System.Uri("https://customercare.hmhco.com/product/techsupport/CCTechSupportLandingPage.html"));
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Launcher.OpenAsync(new Uri($"mailto:{(sender as Label).Text}"));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if(height>width)
            {
                if(Device.RuntimePlatform == Device.Android)
                {
                    //CustomerCareURI.FontSize = 13;
                    CustomCareSupportURI.FontSize = 13;
                    //TechSupportURI.FontSize = 12.5;
                    RiversideTechSupportURI.FontSize = 13;
                    //AssesmentSupportURI.FontSize = 13;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    //CustomerCareURI.FontSize = 14;
                    CustomCareSupportURI.FontSize = 14;
                    //TechSupportURI.FontSize = 14;
                    RiversideTechSupportURI.FontSize = 14;
                    //AssesmentSupportURI.FontSize = 14;
                }
            }
        }

        void FindRepClicked(System.Object sender, System.EventArgs e)
        {
            Launcher.OpenAsync(new System.Uri("https://www.riversideinsights.com/support/representatives"));

        }

        private void TechSupport_Tapped(object sender, EventArgs e)
        {
            
            Launcher.OpenAsync(new Uri($"mailto:{(sender as Label).Text}"));
        }
    }
}
