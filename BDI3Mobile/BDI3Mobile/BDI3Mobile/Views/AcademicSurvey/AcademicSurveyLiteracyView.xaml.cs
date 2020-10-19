using Acr.UserDialogs;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.AcademicSurvey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcademicSurveyLiteracyView : ContentPage
    {
        public AcademicSurveyLiteracyViewModel MyViewModel;
        public NewAssessmentViewModel newAssessmentViewModel;
        public SearchEditViewModel SearchEditViewModel;
        public ChildInformationpageViewModel ChildInformationpageViewModel;
        public AssessmentConfigPopupViewModel AssessmentConfigPopupViewModel;
        public AcademicSurveyLiteracyView(AdminstrationNavigationParams adminstrationNavigationParams)
        {
            UserDialogs.Instance.HideLoading();
            var instaceId = 0;
            var txtdob = "";
            var testdate = "";
            instaceId = adminstrationNavigationParams.LocalInstanceID;
            txtdob = adminstrationNavigationParams.DOB;
            testdate = adminstrationNavigationParams.TestDate;

            MyViewModel = new AcademicSurveyLiteracyViewModel(instaceId, txtdob, testdate);
            this.BindingContext = MyViewModel;
            InitializeComponent();

            MyViewModel.OfflineStudentID = adminstrationNavigationParams.OfflineStudentID;
            MyViewModel.LocaInstanceID = adminstrationNavigationParams.LocalInstanceID;         
                       
            DatePicker_DOB.PropertyChanged += DatePicker_DOB_PropertyChanged;

            //structure.PropertyChanged += Structure_PropertyChanged;
            //observation.PropertyChanged += Observation_PropertyChanged;
            //interview.PropertyChanged += Interview_PropertyChanged;
            DatePickerGrid.IsVisible = false;

            DateTime Now = DateTime.Now;
            var splittedDate = txtdob.Split('/');
            DateTime dateOfBirth = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
            int Years = dateOfBirth < Now ? new DateTime(DateTime.Now.Subtract(dateOfBirth).Ticks).Year - 1 : 0;
            DateTime PastYearDate = dateOfBirth.AddYears(Years);
            int Months = 0;
            for (int i = 1; i <= 12; i++)
            {
                if (PastYearDate.AddMonths(i) == Now)
                {
                    Months = i;
                    break;
                }
                else if (PastYearDate.AddMonths(i) >= Now)
                {
                    Months = i - 1;
                    break;
                }
            }
            var totalMonths = Months + (Years * 12);
            if (totalMonths <= 23)
            {
                age.Text = totalMonths + (totalMonths == 1 ? " month" : " months");
            }
            else
            {
                age.Text = Years + " years, " + Months + (Months == 1 ? " month" : " months");
            }

            childName.Text = adminstrationNavigationParams.FullName;
            dob.Text = adminstrationNavigationParams.DOB;
            if (!string.IsNullOrEmpty(MyViewModel.TestDate))
            {
                testDate.Text = MyViewModel.TestDate;
            }


            //if (childInfo is NewAssessmentViewModel)
            //{
            //    newAssessmentViewModel = childInfo as NewAssessmentViewModel;
            //    MyViewModel.OfflineStudentID = newAssessmentViewModel.OfflineStudentId;
            //}
            //else if (childInfo is SearchEditViewModel)
            //{
            //    SearchEditViewModel = childInfo as SearchEditViewModel;
            //    MyViewModel.OfflineStudentID = SearchEditViewModel.OfflineStudentId;
            //}
            //else if (childInfo is ChildInformationpageViewModel)
            //{
            //    ChildInformationpageViewModel = childInfo as ChildInformationpageViewModel;
            //    MyViewModel.OfflineStudentID = ChildInformationpageViewModel.OfflineStudentId;
            //}
            //else
            //{
            //    AssessmentConfigPopupViewModel = childInfo as AssessmentConfigPopupViewModel;
            //    MyViewModel.OfflineStudentID = AssessmentConfigPopupViewModel.OfflineStudentId;
            //}


            //age.Text = "<years, months>";

            //if (childInfo != null)
            //{
            //    childName.Text = (newAssessmentViewModel != null ? newAssessmentViewModel.FullName : SearchEditViewModel != null ? SearchEditViewModel.FullName : ChildInformationpageViewModel != null ? ChildInformationpageViewModel.FullName : AssessmentConfigPopupViewModel.FullName);
            //    dob.Text = (newAssessmentViewModel != null ? newAssessmentViewModel.DOB : SearchEditViewModel != null ? SearchEditViewModel.DOB : ChildInformationpageViewModel != null ? ChildInformationpageViewModel.DOB : AssessmentConfigPopupViewModel.DOB);
            //    testDate.Text = (newAssessmentViewModel != null ? newAssessmentViewModel.TestDate : SearchEditViewModel != null ? SearchEditViewModel.TestDate : ChildInformationpageViewModel != null ? ChildInformationpageViewModel.TestDate : AssessmentConfigPopupViewModel.TestDate);
            //}
            //else
            //{
            //    childName.Text = "child name";
            //    dob.Text = "00/00/0000";
            //    testDate.Text = "00/00/0000";
            //}
        }

        private void DatePicker_DOB_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                testDate.Text = (sender as MyDatePicker).SelectedDate;
            }
            if (Device.RuntimePlatform == Device.iOS)
                DatePickerGrid.IsVisible = false;
        }

        private void OpenTestDatePicker_Tapped(object sender, EventArgs e)
        {
            DatePickerGrid.IsVisible = true;
            DatePicker_DOB.IsShowPicker = true;
            DatePicker_DOB.IsVisible = true;
            DatePicker_DOB.Focus();
        }

        private void Column_1TallyPointTapped(object sender, EventArgs e)
        {
            //var items = ((((sender as Grid).Parent.Parent.Parent as ViewCell)).View as Grid).Children[0];

            //outerGrid.BackgroundColor = Color.Equals(outerGrid.BackgroundColor, Color.FromHex("#0779ca")) ? Color.FromHex("#f4f4f4") : Color.FromHex("#0779ca");

            var outerGrid = (sender as Grid);
            var innerGrid = outerGrid.Children[0];
            int pointsText = Convert.ToInt32(((innerGrid as Grid).Children[0] as Label).Text);

            //MyViewModel.TallyPointSelection.Execute(MyViewModel.RowContentTallyPointCollection.FirstOrDefault(p => p.lowScore == pointsText));
            return;
        }

        private void Column_2TallyPointTapped(object sender, EventArgs e)
        {
            //var items = ((((sender as Grid).Parent.Parent.Parent as ViewCell)).View as StackLayout).Children[0];
            //outerGrid.BackgroundColor = Color.Equals(outerGrid.BackgroundColor, Color.FromHex("#0779ca")) ? Color.FromHex("#f4f4f4") : Color.FromHex("#0779ca");
            var outerGrid = (sender as Grid);
            var innerGrid = outerGrid.Children[0];
            int pointsText = Convert.ToInt32(((innerGrid as Grid).Children[0] as Label).Text);

            //MyViewModel.TallyPointSelection.Execute(MyViewModel.RowContentTallyPointCollection.FirstOrDefault(p => p.highScore == pointsText));
            return;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new ChildInformationpageView(MyViewModel.OfflineStudentID));
        }
    }
}