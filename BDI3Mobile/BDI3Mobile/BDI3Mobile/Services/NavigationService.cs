using BDI3Mobile.IServices;

namespace BDI3Mobile.Services
{
    public class NavigationService : INavigationService
    {
        public async void ClearModalStack()
        {
            while (App.Current.MainPage.Navigation.ModalStack.Count > 0)
            {
                await App.Current.MainPage.Navigation.PopModalAsync(false);
            }
        }
    }
}
