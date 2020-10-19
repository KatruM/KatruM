using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels
{
    public class GenericFailuretoCommitViewModel : BindableObject
    {
        public GenericFailuretoCommitViewModel()
        {

        }
        private string errorMessage;
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public Command ClosePopupCommand
        {
            get
            {
                return new Command(async()=> 
                {
                    await PopupNavigation.Instance.PopAllAsync();
                });
            }
        }
    }
}
