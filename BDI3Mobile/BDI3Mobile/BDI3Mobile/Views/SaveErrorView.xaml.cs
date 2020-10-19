using BDI3Mobile.ViewModels;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaveErrorView 
	{
		public SaveErrorView (AddChildViewModel addChildViewModel)
		{
			InitializeComponent ();
            BindingContext = new SaveErrorViewModel(addChildViewModel);
             
		}

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    PopupNavigation.Instance.PopAsync();
        //    App.Current.MainPage.Navigation.PushModalAsync(new DashboardHomeView());
        //}

        //private void Button_Clicked_1(object sender, EventArgs e)
        //{
        //    PopupNavigation.Instance.PopAsync();
        //}
    }
}