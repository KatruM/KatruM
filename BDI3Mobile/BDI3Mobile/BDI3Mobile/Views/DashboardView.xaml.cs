using System;
using Xamarin.Forms;
using BDI3Mobile.ViewModels;
using BDI3Mobile.Common;


//This will be initial view of the Dashboard. 
//The central frame of DashboardHomeView is empty and 
//any content view will replace the Main View

namespace BDI3Mobile.Views
{
    public partial class DashboardView : ContentView
    {
        public DashboardView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private async void BrowseUpdateChildRecord_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SearchEditChildView());
        }


        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if(height>width)
            {
                if(Device.RuntimePlatform == Device.Android)
                {
                    AssesmentsLabel.FontSize = 18;
                    ChildInfoLabel.FontSize = 18;
                    ReportsLabel.FontSize = 18;
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    AssesmentsLabel.FontSize = 24;
                    ChildInfoLabel.FontSize = 24;
                    ReportsLabel.FontSize = 24;
                }
            }

        }

        private void FullReport_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device.RuntimePlatform != Device.iOS)
            {
                if ((sender as Button).IsEnabled)
                {
                    FullReport.BackgroundColor = Colors.PrimaryColor;
                }
                else
                {
                    FullReport.BackgroundColor = Color.Gray;

                }
            }
        }
    }
}
