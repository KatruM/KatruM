using Xamarin.Forms;
using System.Windows.Input;
using BDI3Mobile.Views;
using Rg.Plugins.Popup.Services;

namespace BDI3Mobile.ViewModels
{
    public class SaveErrorViewModel : BaseclassViewModel
    {

        //private string yesbutton;
        //public string Yesbutton
        //{
        //    get
        //    {
        //        return yesbutton;
        //    }
        //    set
        //    {
        //        yesbutton = value;
        //        OnPropertyChanged(nameof(Yesbutton));

        //    }
                
        //}

        public ICommand Yesbuttoncommand { get; set; }
        public ICommand Nobuttoncommand { get; set; }
        public AddChildViewModel AddChildViewModelInstance { get; set; }

        public  SaveErrorViewModel(AddChildViewModel addChildViewModel)
        {
            AddChildViewModelInstance = addChildViewModel;
            Yesbuttoncommand = new Command(yesbutton);
            Nobuttoncommand = new Command(nobutton);
        }

        public void yesbutton()
        {
            if(AddChildViewModelInstance != null)
                AddChildViewModelInstance.ReloadCommand.Execute(new object());
            App.Current.MainPage.Navigation.PushModalAsync(new DashboardHomeView());
            PopupNavigation.Instance.PopAsync();
        }

        public void nobutton()
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
