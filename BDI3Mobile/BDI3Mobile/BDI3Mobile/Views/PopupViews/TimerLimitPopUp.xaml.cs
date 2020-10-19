using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerLimitPopUp
    {
        public TimerLimitPopUp()
        {
            InitializeComponent();
            this.PropertyChanged += TimerLimitPopUp_PropertyChanged;
        }

        private void TimerLimitPopUp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName == "IsVisible")
            {
                if (!(sender as TimerLimitPopUp).IsVisible)
                {
                    if ((BindingContext != null && BindingContext is AdministrationViewModel) && 
                        (this.BindingContext as AdministrationViewModel).TotalSeconds.TotalSeconds == (this.BindingContext as AdministrationViewModel).MaxTime)
                    {
                        (this.BindingContext as AdministrationViewModel).ResetEnabled = true;
                        (this.BindingContext as AdministrationViewModel).TimerReset = "iconrefreshblue.png";
                    }
                }
                
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
            if(this.BindingContext is AdministrationViewModel)
            {
                (this.BindingContext as AdministrationViewModel).ResetEnabled = true;
                (this.BindingContext as AdministrationViewModel).TimerReset = "iconrefreshblue.png";
                (this.BindingContext as AdministrationViewModel).ScoringTabTappedCommand.Execute(new object());
            }
            else if(this.BindingContext is AcademicSurveyLiteracyViewModel)
            {
                (this.BindingContext as AcademicSurveyLiteracyViewModel).ResetEnabled = true;
                (this.BindingContext as AcademicSurveyLiteracyViewModel).TimerReset = "iconrefreshblue.png";
                //(this.BindingContext as AcademicSurveyLiteracyViewModel).ScoringTabTappedCommand.Execute(new object());
            }
            
        }
    }
}