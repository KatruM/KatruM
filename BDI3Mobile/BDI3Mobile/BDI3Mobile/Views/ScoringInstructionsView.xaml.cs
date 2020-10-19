using BDI3Mobile.ViewModels;
using Xamarin.Forms.Xaml;
namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScoringInstructionsView  
	{
        
		public ScoringInstructionsView ()
		{
			InitializeComponent ();
            BindingContext = new ScoringInstructionsViewModel();
		}
	}
}