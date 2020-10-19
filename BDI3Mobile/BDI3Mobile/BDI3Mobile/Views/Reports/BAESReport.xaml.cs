using BDI3Mobile.CustomRenderer;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BAESReport : PopupPage
    {
        public AcademicTestSessionOverviewModel vM { get; set; }
        public string DomainCode { get; set; }
        public BAESReport()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            vM = new AcademicTestSessionOverviewModel(true);
            BindingContext = vM;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           
            await PopupNavigation.Instance.PopAsync();
        }

    }
}