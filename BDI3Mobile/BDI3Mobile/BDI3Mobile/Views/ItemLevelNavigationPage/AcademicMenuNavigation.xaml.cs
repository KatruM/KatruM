using Rg.Plugins.Popup.Pages;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.ItemLevelNavigationPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AcademicMenuNavigation : PopupPage
    {
        public AcademicMenuNavigation()
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
                    MainFrame.Margin = new Thickness(0, 93, 0, 0);
                else if (Device.RuntimePlatform == Device.iOS)
                    MainFrame.Margin = new Thickness(0, 80, 0, 0);
            }
        }
    }
}