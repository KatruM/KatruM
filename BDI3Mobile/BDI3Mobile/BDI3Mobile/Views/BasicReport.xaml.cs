using BDI3Mobile.ViewModels;
using BDI3Mobile.Common;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasicReport : PopupPage
    {
        BasicReportViewModel MyBasicReportViewModel;
        private WeakReference _currentField;

        public BasicReport()
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = true;
            MyBasicReportViewModel = new BasicReportViewModel();
            BindingContext = MyBasicReportViewModel;
            MyBasicReportViewModel.RunReport = false;
        }

        public async void OnCancelClick(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentField = new WeakReference(sender);
        }
        private void BasicReport_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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