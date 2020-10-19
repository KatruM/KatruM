using System;
using BDI3Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AcademicSurveyView 
	{
        public AcademicSurveyViewModel MyviewModel { get; set; }
        public AcademicSurveyView (ViewModels.AssessmentViewModels.NewAssessmentViewModel newAssessmentViewModel)
		{
			InitializeComponent ();
            MyviewModel = new AcademicSurveyViewModel();
            this.BindingContext = MyviewModel;
        }


        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        private void ListView_ItemTapped_1(object sender, ItemTappedEventArgs e)
        {

        }
    }
}