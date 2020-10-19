using System;
using BDI3Mobile.View;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LogoutpopupView 
    {
		public LogoutpopupView ()
		{
			InitializeComponent ();
            BindingContext = new LogoutpopupViewModel();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
        }

         
        //private async Task Button_Clicked_1Async(object sender, EventArgs e)
        //{
        //    await PopupNavigation.Instance.PopAsync();
        //    await App.Current.MainPage.Navigation.PushModalAsync(new LoginView());
        //}

        //private async Task Button_Clicked_1Async(object sender, EventArgs e)
        //{
        //    //await PopupNavigation.Instance.PopAsync();
        //    await App.Current.MainPage.Navigation.PushModalAsync(new LoginView());

        //}

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            PopupNavigation.Instance.PopAsync();
            App.Current.MainPage.Navigation.PushModalAsync(new LoginView());
        }
    }
}