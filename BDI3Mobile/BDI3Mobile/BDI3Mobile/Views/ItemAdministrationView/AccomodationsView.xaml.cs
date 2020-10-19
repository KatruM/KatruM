using BDI3Mobile.ViewModels.AdministrationViewModels;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.ItemAdministrationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccomodationsView 
    {
        AccomodationsViewModel viewModel;

        public AccomodationsView(string notes)
        {
            InitializeComponent();
            Xamarin.Forms.Application.Current.On<Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize);
            viewModel = new AccomodationsViewModel();
            viewModel.Notes = notes;
            BindingContext = viewModel;
        }
    }
}
