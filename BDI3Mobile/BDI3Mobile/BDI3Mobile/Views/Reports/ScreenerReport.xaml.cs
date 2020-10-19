using System;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;
using BDI3Mobile.ViewModels.AdministrationViewModels;

namespace BDI3Mobile.Views.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScreenerReport : PopupPage
    {
        public TestSessionOverviewScreenerViewModel vM { get; set; }

        public ScreenerReport(string age)
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            if (age != null)
            {
                vM = new TestSessionOverviewScreenerViewModel(age, true);
            }
            BindingContext = vM;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await vM.SaveTestSessionOverView();
            await PopupNavigation.Instance.PopAsync();
        }
    }
}