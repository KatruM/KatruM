using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Models.AcademicFolder;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.AcademicSurvey
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcademicformWithMatAndItems : ContentPage
    {
        public AcademicSurveyLiteracyViewModel MyViewModel;
        MyListView myList;
        CollectionView myColllectionView;
        public AcademicformWithMatAndItems(AdminstrationNavigationParams adminstrationNavigationParams)
        {
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

            MyViewModel.PropertyChanged -= MyViewModel_PropertyChanged;
            MyViewModel.PropertyChanged += MyViewModel_PropertyChanged;

            DatePicker_DOB.PropertyChanged -= DatePicker_DOB_PropertyChanged;
            DatePicker_DOB.PropertyChanged += DatePicker_DOB_PropertyChanged;

            //structure.PropertyChanged += Structure_PropertyChanged;
            //observation.PropertyChanged += Observation_PropertyChanged;
            //interview.PropertyChanged += Interview_PropertyChanged;
            DatePickerGrid.IsVisible = false;

            DateTime Now = DateTime.Now;
            var splittedDate = txtdob.Split('/');
            DateTime dateOfBirth = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
            int Years = dateOfBirth.Date < Now.Date ? new DateTime(DateTime.Now.Subtract(dateOfBirth).Ticks).Year - 1 : 0;
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
            loadpage();
        }
        private async Task loadpage()
        {
            await Task.Delay(500);
            await MyViewModel.LoadPage();
        }

        private void MyViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TestDate")
            {
                testDate.Text = MyViewModel.TestDate;
            }

            var item = sender as AcademicSurveyLiteracyViewModel;

        }

        private async void DatePicker_DOB_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                var date = sender as MyDatePicker;
                if (date != null && string.IsNullOrWhiteSpace(date.SelectedDate))
                    return;
                var hasError = false;
                var errorMessage = "";
                var splittedDate = date.SelectedDate.Split('/');
                DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
                if (date != null && itemdateTime.Date > DateTime.Now)
                {
                    hasError = true;
                    errorMessage = "Date entered cannot exceed current date.";
                }
                else
                {
                    if (date != null)
                    {
                        var selectedDate = itemdateTime;
                        if (!(selectedDate.Date <= MyViewModel.MaxDate && selectedDate.Date >= MyViewModel.MinDate))
                        {
                            hasError = true;
                        }
                    }
                }
                if (hasError)
                {
                    date.SelectedDate = string.Empty; //This will trigger the SelectedDate again.
                    if (errorMessage.Length == 0)
                    {
                        if (MyViewModel.isFirstRecord)
                        {
                            errorMessage = "Testing ranges must be within 30 days of testing each subdomain due to developmental milestones progressing over time. Please enter a valid test date or enter scores in a new record form.";
                        }
                        else
                        {
                            errorMessage = "One or more testing dates are not within a 30 day period of the first test date. Testing ranges must be within 30 days of testing each subdomain due to developmental milestones progressing over time. Please enter a valid date or enter scores in a new record form.";
                        }
                    }
                    var popup = new CustomPopupView(new CustomPopUpDetails() { Header = "Invalid Test Date", Message = errorMessage, Height = 228, Width = 450 });
                    popup.BindingContext = MyViewModel;
                    popup.CloseWhenBackgroundIsClicked = false;
                    await PopupNavigation.Instance.PushAsync(popup);
                    return;
                }
                date.SelectedDate = string.Empty; //This will trigger the SelectedDate again.
                testDate.Text = (sender as MyDatePicker).SelectedDate;
                MyViewModel.TestDate = testDate.Text;
                MyViewModel.SaveTestDate((sender as MyDatePicker).SelectedDate);
            }



            if (Device.RuntimePlatform == Device.iOS)
                DatePickerGrid.IsVisible = false;
        }

        private void OpenTestDatePicker_Tapped(object sender, EventArgs e)
        {
            MyViewModel.Check30DayIssue();
            DatePickerGrid.IsVisible = true;
            var splittedDate = MyViewModel.TestDate.Split('/');
            DatePicker_DOB.Date = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[0]));
            if (Device.RuntimePlatform == Device.UWP)
            {
                DatePicker_DOB.ShowDatePicker();
            }
            else
            {
                DatePicker_DOB.Focus();
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MyViewModel.ChildTapped = true;
            MyViewModel.BackCommand.Execute(null);
        }
        private void RubbicPointsTapped(object sender, EventArgs e)
        {
            var items = ((((sender as Grid).Parent.Parent.Parent as ViewCell)).View as StackLayout).Children[0];
            var outerGrid = (sender as Grid);
            //outerGrid.BackgroundColor = Color.Equals(outerGrid.BackgroundColor, Color.FromHex("#0779ca")) ? Color.FromHex("#f4f4f4") : Color.FromHex("#0779ca");
            var innerGrid = outerGrid.Children[0];
            int pointsText = Convert.ToInt32(((innerGrid as Grid).Children[0] as Label).Text);

            MyViewModel.ContentRubicPointSelection.Execute(MyViewModel.ScoringCollection.FirstOrDefault(p => p.points == pointsText));
            MyViewModel.IsInProgress = true;
            return;
        }

        private void TallyPointsTapped(object sender, EventArgs e)
        {
            var outerGrid = (sender as Grid);
            var innerGrid = outerGrid.Children[0];
            int pointsText = Convert.ToInt32(((innerGrid as Grid).Children[0] as Label).Text);
            if (outerGrid.BindingContext != null)
            {
                var dataContext = outerGrid.BindingContext as ContentItemTally;
                if (dataContext != null)
                {
                    MyViewModel.TallyPointSelection(dataContext, pointsText);
                    if (dataContext.IsCorrectChecked || dataContext.IsInCorrectChecked)
                    {
                        dataContext.IsSelected = true;
                    }
                    else
                    {
                        dataContext.IsSelected = false;
                    }
                }

                if (dataContext.RowNum == 9)
                {
                    var allTallyselected = MyViewModel.TallyCollection.All(i => i.IsSelected == true);
                    if (allTallyselected)
                    {
                        MyViewModel.StopTimerWithFluencyItem9();
                    }
                }

            }
        }

        void listview6_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Device.RuntimePlatform == Device.iOS)
            {
                if (MyViewModel != null)
                {
                    if (MyViewModel.CurrentAcademiTemplate == Models.AcademicFolder.CurrentAcademiTemplate.ImageMaterialSampleGrid)
                    {
                        //iOS listview isue -> Listview needs to scroll to first item.

                        /*if (listview6 != null && MyViewModel.CurrentAcademicContentModel != null)
                        {
                            if (MyViewModel.CurrentAcademicContentModel.GroupTitle != null && MyViewModel.CurrentAcademicContentModel.GroupTitle != "FLU S")
                            {
                                ObservableCollection<ContentItemTally> tallyItems = new ObservableCollection<ContentItemTally>();
                                if (listview6.ItemsSource != null)
                                {
                                    tallyItems = (ObservableCollection<ContentItemTally>)listview6.ItemsSource;
                                    if (tallyItems != null)
                                    {
                                        listview6.ScrollTo(tallyItems.FirstOrDefault(), ScrollToPosition.Start, false);
                                    }
                                }
                            }
                        }*/
                    }
                }
            }

        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            MyViewModel.ContentRubicPointSelection.Execute(((sender as Grid).BindingContext as AcademicScoringModel));
            MyViewModel.IsInProgress = true;
            return;
        }
        private void listView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            myList = sender as MyListView;
        }

        private void CollectionView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            myColllectionView = sender as CollectionView;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            MyViewModel.PropertyChanged -= MyViewModel_PropertyChanged;
            DatePicker_DOB.PropertyChanged -= DatePicker_DOB_PropertyChanged;

            MyViewModel.PropertyChanged += MyViewModel_PropertyChanged;
            DatePicker_DOB.PropertyChanged += DatePicker_DOB_PropertyChanged;
        }
        protected override void OnDisappearing()
        {
            MyViewModel.PropertyChanged -= MyViewModel_PropertyChanged;
            DatePicker_DOB.PropertyChanged -= DatePicker_DOB_PropertyChanged;

            MyViewModel.Dispose();
            //GC.Collect();
            //GC.SuppressFinalize(this);
            base.OnDisappearing();
        }        
    }
}