using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncingPopupView : PopupPage
    {
        public SyncingPopupView()
        {
            InitializeComponent();
            CloseWhenBackgroundIsClicked = false;
            
        }
    }
}