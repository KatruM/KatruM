using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.PopupViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FluencyTimerLimitPopUp
    {
        public FluencyTimerLimitPopUp()
        {
            InitializeComponent();
            Message.Text = "You have reached the time limit of 3 minutes and 30 seconds would you like to stay and "+ 
                                              "\ncontinue recording scores for Fluency or continue to the next test?";
            this.PropertyChanged += TimerLimitPopUp_PropertyChanged;
        }   

        private void TimerLimitPopUp_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName == "IsVisible")
            {
               
                if (!(sender as FluencyTimerLimitPopUp).IsVisible)
                {
                    if ((BindingContext != null && BindingContext is AcademicSurveyLiteracyViewModel) && 
                        (this.BindingContext as AcademicSurveyLiteracyViewModel).TotalSeconds.TotalSeconds == (this.BindingContext as AcademicSurveyLiteracyViewModel).MaxTime)
                    {
                        (this.BindingContext as AcademicSurveyLiteracyViewModel).ResetEnabled = true;
                        (this.BindingContext as AcademicSurveyLiteracyViewModel).TimerReset = "iconrefreshblue.png";
                    }
                }
                
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
            if(this.BindingContext is AcademicSurveyLiteracyViewModel)
            {
                (this.BindingContext as AcademicSurveyLiteracyViewModel).ResetEnabled = true;
                (this.BindingContext as AcademicSurveyLiteracyViewModel).TimerReset = "iconrefreshblue.png";
                //(this.BindingContext as AcademicSurveyLiteracyViewModel).ScoringTabTappedCommand.Execute(new object());
            }
            
        }
    }
}