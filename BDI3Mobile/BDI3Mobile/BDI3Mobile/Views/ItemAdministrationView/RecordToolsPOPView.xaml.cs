using System;
using BDI3Mobile.Models.DevelopmentalFormModel;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using BDI3Mobile.Views.RecordToolsForms;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.ItemAdministrationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RecordToolsPOP 
	{
        public bool IsTestSessionOverviewClicked;
        private bool IsResetScoreButtonClicked;
        private AcademicSurveyLiteracyViewModel academicSurveyLiteracyViewModel { get; set; }

        public AdministrationViewModel administrationViewModel { get; set; }

		public RecordToolsPOP (AdministrationViewModel adminViewModel)
		{
			InitializeComponent ();
            administrationViewModel = adminViewModel;
        }

        public RecordToolsPOP(AcademicSurveyLiteracyViewModel academicSurveyLiteracyViewModel)
        {
            InitializeComponent();
            this.academicSurveyLiteracyViewModel = academicSurveyLiteracyViewModel;
            AccommodationSection.IsVisible = true;
        }

        private async void OpenTestRecordNavigation(object sender, EventArgs e)
        {
            if (IsTestSessionOverviewClicked)
                return;

            IsTestSessionOverviewClicked = true;
            await PopupNavigation.Instance.PopAsync();

            if (administrationViewModel != null)
            {
                if (administrationViewModel.AssessmentConfigurationType == AssessmentConfigurationType.BattelleDevelopmentComplete)
                {
                    await PopupNavigation.Instance.PushAsync(new TestsessionOverviewView(), CloseWhenBackgroundIsClicked);
                }
                else if (administrationViewModel.AssessmentConfigurationType == AssessmentConfigurationType.BattelleDevelopmentScreener)
                {
                    await PopupNavigation.Instance.PushAsync(new TestSessionOverviewScreenerView(), CloseWhenBackgroundIsClicked);
                }
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new AcademicTestSessionOverview(), CloseWhenBackgroundIsClicked);
            }

            IsTestSessionOverviewClicked = false;


        }
        private async void OpenAccomodationView(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            await PopupNavigation.Instance.PushAsync(new AccomodationsView(academicSurveyLiteracyViewModel.FormNotes), CloseWhenBackgroundIsClicked);
        }
        private async void OpenResetScoresView(object sender, EventArgs e)
        {
            //Fix: CLINICAL - 4220
            if (IsResetScoreButtonClicked)
                return;
            await PopupNavigation.Instance.PopAllAsync();

            IsResetScoreButtonClicked = true;
            if (administrationViewModel != null)
            {
                await PopupNavigation.Instance.PushAsync(new ResetScoresView(administrationViewModel.ResetScroes, administrationViewModel.IsBattelleDevelopmentCompleteChecked, ShowToolsPopuponBack));
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new ResetScoresView(academicSurveyLiteracyViewModel.ResetScroes, false,  ShowToolsPopuponBack, true));
            }
            IsResetScoreButtonClicked = false;
        }

        public void ShowToolsPopuponBack()
        {
            if (administrationViewModel != null)
                administrationViewModel.OpenRecordToolCommand.Execute(null);
            else
                academicSurveyLiteracyViewModel.OpenRecordToolCommand.Execute(null);
        }

        private async void Done_TappedAsync(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
            await PopupNavigation.Instance.PushAsync(new TestsessionOverviewView(), CloseWhenBackgroundIsClicked);
        }

        private async void OpenNotesMenu(object sender, EventArgs e)
        {
            if( PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync(false);
            }
            if (administrationViewModel != null)
            {
                await PopupNavigation.Instance.PushAsync(new NotesMenu(administrationViewModel), CloseWhenBackgroundIsClicked);
            }
            else
            {
                await PopupNavigation.Instance.PushAsync(new NotesMenu(academicSurveyLiteracyViewModel), CloseWhenBackgroundIsClicked);
            }
        }
    }
}