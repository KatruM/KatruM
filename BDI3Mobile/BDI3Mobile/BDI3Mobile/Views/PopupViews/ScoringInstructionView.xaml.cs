using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScoringInstructionView
    {
        public ScoringInstructionView()
        {
            InitializeComponent();
        }

        private async void CloseScoringIns(object sender, EventArgs e)
        {
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAsync(false);
            }
        }
    }
}