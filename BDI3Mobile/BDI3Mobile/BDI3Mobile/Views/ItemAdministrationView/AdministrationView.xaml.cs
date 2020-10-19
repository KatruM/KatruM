using BDI3Mobile.Common;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels.AdministrationViewModels;
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

namespace BDI3Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdministrationView : ContentPage
    {
        private bool expandImageClicked;
        public AdministrationViewModel MyViewModel;
        public AdministrationView(AdminstrationNavigationParams adminstrationNavigationParams)
        {
            var instaceId = 0;
            var txtdob = "";
            var testdate = "";
            instaceId = adminstrationNavigationParams.LocalInstanceID;
            txtdob = adminstrationNavigationParams.DOB;
            testdate = adminstrationNavigationParams.TestDate;
            MyViewModel = new AdministrationViewModel(instaceId, adminstrationNavigationParams.IsDevelopmentCompleteForm, txtdob, testdate);            
            this.BindingContext = MyViewModel;
            InitializeComponent();            

            MyViewModel.OfflineStudentID = adminstrationNavigationParams.OfflineStudentID;
            MyViewModel.LocaInstanceID = adminstrationNavigationParams.LocalInstanceID;
            DatePickerGrid.IsVisible = false;
            DateTime Now = DateTime.Now;
            var splittedDate = txtdob.Split('/');

            try
            {
                DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
                int Years = new DateTime(DateTime.Now.Subtract(itemdateTime).Ticks).Year - 1;
                DateTime PastYearDate = itemdateTime.AddYears(Years);
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
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            loadtestform();
        }
        public async void loadtestform()
        { 
            await Task.Delay(500);
            await MyViewModel.LoadTestForm();
        }

        private void MyViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TestDate")
            {
                testDate.Text = MyViewModel.TestDate;
            }
            //Scrolls the Capture mode area to top - Bug Fix - CLINICAL-4409
            CaptureModeScrollView.ScrollToAsync(0, 0, false);
            ScoringScrollView.ScrollToAsync(0, 0, false);
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
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Device.RuntimePlatform == Device.iOS)
            {
                childName.FontSize = childNameHeader.FontSize = dob.FontSize = dobHeader.FontSize = age.FontSize = ageHeader.FontSize = testDate.FontSize = testDateHeader.FontSize = basal.FontSize = ceiling.FontSize = 15;
            }
            if (height > width)
            {
                if (Device.RuntimePlatform == Device.UWP || Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                {
                    ChildColumn.Orientation = DOBColumn.Orientation = AgeColumn.Orientation = TestDateColumn.Orientation = StackOrientation.Vertical;
                }
            }
            else
            {
                ChildColumn.Orientation = DOBColumn.Orientation = AgeColumn.Orientation = TestDateColumn.Orientation = StackOrientation.Horizontal;
            }
        }

        private void TallyTabTapped(object sender, EventArgs e)
        {
            tallyLayout.IsVisible = true;
            tallyStack.BackgroundColor = Color.White;
            tallyBoxview.IsVisible = true;
            tallyLbl.TextColor = Color.Black;

            scoringLayout.IsVisible = false;
            scoringStack.BackgroundColor = Color.LightGray;
            scoringBoxview.IsVisible = false;
            scoringLbl.TextColor = Color.White;
        }

        private void ScoringTabTapped(object sender, EventArgs e)
        {
            tallyLayout.IsVisible = tallyBoxview.IsVisible = false;
            tallyStack.BackgroundColor = Color.LightGray;
            scoringLayout.IsVisible = scoringBoxview.IsVisible = true;
            scoringStack.BackgroundColor = tallyLbl.TextColor = Color.White;
            scoringLbl.TextColor = Color.Black;
        }

        private void OpenTestDatePicker_Tapped(object sender, EventArgs e)
        {
            MyViewModel.Check30DayIssue();
            DatePickerGrid.IsVisible = true;
            var splittedDate = MyViewModel.TestDate.Split('/');
            DatePicker_DOB.Date = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
            DatePicker_DOB.ManualMinDate = true;
            DatePicker_DOB.ManualMaxDate = true;
            DatePicker_DOB.MinimumDate = MyViewModel.MinDate;
            DatePicker_DOB.MaximumDate = MyViewModel.MaxDate;
            if (Device.RuntimePlatform == Device.UWP)
            {
                DatePicker_DOB.ShowDatePicker();
            }
            else
            {
                DatePicker_DOB.Focus();
            }
        }

        private void RubbicPointsTapped(object sender, EventArgs e)
        {
            var items = ((((sender as Grid).Parent.Parent.Parent as ViewCell)).View as StackLayout).Children[0];
            var outerGrid = (sender as Grid);
            //outerGrid.BackgroundColor = Color.Equals(outerGrid.BackgroundColor, Color.FromHex("#0779ca")) ? Color.FromHex("#f4f4f4") : Color.FromHex("#0779ca");
            var innerGrid = outerGrid.Children[0];
            int pointsText = Convert.ToInt32(((innerGrid as Grid).Children[0] as Label).Text);

            MyViewModel.ContentRubicPointSelection.Execute(MyViewModel.ContentRubricPointCollection.FirstOrDefault(p => p.points == pointsText));
            MyViewModel.IsInProgress = true;
            return;            
        }

        private void InnerRubbicPointsTapped(object sender, EventArgs e)
        {
            int pointsText = Convert.ToInt32(((sender as Grid).Children[0] as Label).Text);
            for (int i = 0; i < MyViewModel.ContentRubricPointCollection.Count; i++)
            {
                IEnumerable<PropertyInfo> pInfos = (listView as ItemsView<Cell>).GetType().GetRuntimeProperties();
                var templatedItems = pInfos.FirstOrDefault(info => info.Name == "TemplatedItems");
                if (templatedItems != null)
                {
                    var cells = templatedItems.GetValue(listView);
                    var viewcells = cells as ITemplatedItemsList<Cell>;
                    if (viewcells != null)
                    {
                        if ((viewcells[i] as ViewCell) != null)
                        {
                            var grid = (((viewcells[i] as ViewCell).View as StackLayout).Children[0] as Grid).Children[0] as Grid;

                            var innerGridReflection = grid.Children[0];
                            if (innerGridReflection != null)
                            {
                                if (pointsText == MyViewModel.ContentRubricPointCollection[i].points)
                                {
                                    innerGridReflection.Margin = 3;
                                    innerGridReflection.BackgroundColor = Colors.RubicPointSelectionBckgrd;
                                }
                                else
                                {
                                    innerGridReflection.Margin = 0;
                                    innerGridReflection.BackgroundColor = Colors.RubicPointDefaultBckgrd;
                                }
                            }
                        }
                    }
                }
            }
        }

        private async void OpenExpandedImageVIew(object sender, EventArgs e)
        {
            if (expandImageClicked)
                return;

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack)
                {
                    if (popup is ItemLevelNavigationPage && (popup as ItemLevelNavigationPage).Title == "ItemLevelNaviagationTitle")
                    {
                        await PopupNavigation.Instance.PopAsync(false);
                    }

                    if (popup is ImagePopupView && (popup as ImagePopupView).Title == "ExpandImageTitle")
                    {
                        return;
                    }
                }
            }

            var image = (sender as Image).Source;
            string[] seperator = { "File:", " " };
            int count = 2;
            if (image != null)
            {
                string[] mainImage = image.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries);


                var firstimagelocation = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[0] as StackLayout).Children[0] as Image).Source;
                var secondimagelocation = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[1] as StackLayout).Children[0] as Image).Source;
                var thirdimagelocation = (((((sender as Image).Parent as StackLayout).Parent as Grid).Children[2] as StackLayout).Children[0] as Image).Source;
                string[] allImagesLoc = new string[] { firstimagelocation != null ? firstimagelocation.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries)[0] : null, secondimagelocation != null ? secondimagelocation.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries)[0] : null, thirdimagelocation != null ? thirdimagelocation.ToString().Split(seperator, count, StringSplitOptions.RemoveEmptyEntries)[0] : null };

                expandImageClicked = true;
                await PopupNavigation.Instance.PushAsync(new Views.PopupViews.ImagePopupView(mainImage[0], allImagesLoc) { Title = "ExpandImageTitle" });
                expandImageClicked = false;
            }
        }


        private async void Imageview_Clicked(object sender, EventArgs e)
        {
            if (expandImageClicked)
                return;

            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                foreach (var popup in PopupNavigation.Instance.PopupStack)
                {
                    if (popup is ItemLevelNavigationPage && (popup as ItemLevelNavigationPage).Title == "ItemLevelNaviagationTitle")
                    {
                        await PopupNavigation.Instance.PopAsync(false);
                    }

                    if (popup is ImagePopupView && (popup as ImagePopupView).Title == "ExpandImageTitle")
                    {
                        return;
                    }
                }
            }

            var image = (sender as ImageButton).Source;

            string[] mainImage = image.ToString().Split(':');
            expandImageClicked = true;
            await PopupNavigation.Instance.PushAsync(new Views.PopupViews.ImagePopupView(mainImage[1].Trim(), new string[] { "fsd", "saf" }) { Title = "ExpandImageTitle" });
            expandImageClicked = false;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MyViewModel.ContentRubicPointSelection.Execute(e.Item as ContentRubricPoint);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            MyViewModel.ChildTapped = true;
            MyViewModel.BackCommand.Execute(null);
            // await App.Current.MainPage.Navigation.PushModalAsync(new ChildInformationpageView(MyViewModel.OfflineStudentID));
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.SelectedItem == null) return;
            // do something with e.SelectedItem
            ((ListView)sender).SelectedItem = null; // de-select the row
        }

        private void ListView_ItemTapped_1(object sender, ItemTappedEventArgs e)
        {
            // don't do anything if we just de-selected the row
            if (e.Item == null) return;
            // do something with e.SelectedItem
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MyViewModel.PropertyChanged -= MyViewModel_PropertyChanged;
            MyViewModel.PropertyChanged += MyViewModel_PropertyChanged;

            DatePicker_DOB.PropertyChanged -= DatePicker_DOB_PropertyChanged;
            DatePicker_DOB.PropertyChanged += DatePicker_DOB_PropertyChanged;
        }
        protected override void OnDisappearing()
        {            
            MyViewModel.PropertyChanged -= MyViewModel_PropertyChanged;
            DatePicker_DOB.PropertyChanged -= DatePicker_DOB_PropertyChanged;

            base.OnDisappearing();
        }

    }
}