using System;
using System.ComponentModel;
using System.Linq;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using BDI3Mobile.Views.PopupViews;
using ClinicalScoring.BundleParsers;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DevelopmentalReport : PopupPage
    {
        public TestSessionOverViewModel vM { get; set; }

        public DevelopmentalReport()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            vM = new TestSessionOverViewModel(true);
            BindingContext = vM;
            vM.IsAcademicBasicReport = true;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await vM.SaveTestSessionOverView();
            await PopupNavigation.Instance.PopAsync();
        }
    }
}