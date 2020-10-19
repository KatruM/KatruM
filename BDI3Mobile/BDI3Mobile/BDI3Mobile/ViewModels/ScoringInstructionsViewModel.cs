using Xamarin.Forms;
using System.Windows.Input;
using Rg.Plugins.Popup.Services;

namespace BDI3Mobile.ViewModels
{
    public class ScoringInstructionsViewModel:BaseclassViewModel
    {

        public ICommand closebuttonCommand { get; set; }
        public ScoringInstructionsViewModel()
        {
            closebuttonCommand = new Command(closebuttomcommandpopup);
        }

        public void closebuttomcommandpopup()
        {
            PopupNavigation.Instance.PopAsync();
        }
    }
}
