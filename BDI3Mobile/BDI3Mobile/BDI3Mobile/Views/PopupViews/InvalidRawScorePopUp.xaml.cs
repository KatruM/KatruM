using System;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InvalidRawScorePopUp : PopupPage
    {
        public InvalidRawScorePopUp(PopUpDetails details)
        {
            InitializeComponent();
            Message.Text = details.Message;
            CloseWhenBackgroundIsClicked = false;
        }

        
        public void OnContinueClicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
    public class PopUpDetails
    {
        public string Message;
    }
}