using Acr.UserDialogs;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup 
    {
        public LoadingPopup()
        {
            InitializeComponent();
            UserDialogs.Instance.ShowLoading("Loading...");
        }
    }
}