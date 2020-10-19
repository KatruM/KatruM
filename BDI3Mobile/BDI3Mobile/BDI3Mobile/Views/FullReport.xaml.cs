using BDI3Mobile.Common;
using BDI3Mobile.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FullReport : PopupPage
    {
        FullReportViewModel ReportViewModel;

        public FullReport()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = true;
            ReportViewModel = new FullReportViewModel();
            BindingContext = ReportViewModel;
            ReportViewModel.RunReport = false;

            LocationText.TextColor = Colors.LightGrayColor;
            childText.TextColor = Colors.LightGrayColor;
            batteryTypeText.TextColor = Colors.LightGrayColor;
            recordFormText.TextColor = Colors.LightGrayColor;
            ReportTypeText.TextColor = Colors.LightGrayColor;
        }
        public async void OnCancelClick(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private void FullReport_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                if ((sender as Button).IsEnabled)
                {
                    RunReportButton.BackgroundColor = Colors.PrimaryColor;
                }
                else
                {
                    RunReportButton.BackgroundColor = Color.Gray;
                }
            }
        }
    }
}