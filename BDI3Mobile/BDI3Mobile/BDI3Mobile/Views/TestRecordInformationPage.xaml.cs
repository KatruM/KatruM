using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestRecordInformationPage : PopupPage
    {
        public TestRecordInformationPage()
        {
            InitializeComponent();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (height > width)
            {
                if (Device.RuntimePlatform == Device.Android)
                    MainFrame.Margin = new Thickness(0, 93, 400, 0);
                else if (Device.RuntimePlatform == Device.iOS)
                    MainFrame.Margin = new Thickness(0, 80, 400, 0);
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                    MainFrame.Margin = new Thickness(0, 93, 600, 0);
                else if (Device.RuntimePlatform == Device.iOS)
                    MainFrame.Margin = new Thickness(0, 80, 0, 0);
            }
        }

        private void NavigateToItemLevelPage(object sender, EventArgs e)
        {            
            PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackgroundClicked()
        {
            PopupNavigation.Instance.PopAllAsync();
            return base.OnBackgroundClicked();
        }

    }

        
    }
    