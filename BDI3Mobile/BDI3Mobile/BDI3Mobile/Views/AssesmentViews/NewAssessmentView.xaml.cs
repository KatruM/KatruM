using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels.AssessmentViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BDI3Mobile.Views.AssesmentViews
{
    public partial class NewAssessmentView
    {
        private int _selectedCount;
        public NewAssessmentViewModel MyNewAssessmentViewModel { get; set; }
        private bool _dobErrorMsgShown;
        private bool _enrollmentErrorMsgShown;

        public NewAssessmentView()
        {
            MyNewAssessmentViewModel = new NewAssessmentViewModel();
            BindingContext = MyNewAssessmentViewModel;
            InitializeComponent();
            DOB.Unfocused += DOB_Unfocused;
            DOB.Focused += DOB_Focused;
            DOB.TextChanged += DOB_TextChanged;
            EnrollmentDate.Unfocused += EnrollmentDate_Unfocused;
            EnrollmentDate.Focused += EnrollmentDate_Focused;
            EnrollmentDate.TextChanged += EnrollmentDate_TextChanged;
            locationListview.ItemSelected += LocationListview_ItemSelected;

            DatePicker_EnrollmentDate.Date = DateTime.Now;
            MessagingCenter.Subscribe<String, bool>(this, "Tab", (text, value) =>
            {
                LocationImageBtn.IsTabStop = value;
                ChildFirstName.IsTabStop = value;
                ChildMiddleName.IsTabStop = value;
                ChildLastName.IsTabStop = value;
                CountryName.IsTabStop = value;
                DOB.IsTabStop = value;
                EnrollmentDate.IsTabStop = value;
            });
            MyNewAssessmentViewModel.SelectedLocations = new List<string>();
            LoadTree();
            MyNewAssessmentViewModel.ClearData = new Action(() =>
            {
                this.locationListview.DisposeListView?.Invoke();
                this.TestRecordListView.DisposeListView?.Invoke();
                this.parentGrid.Children.Clear();
                this.Content = null;
                GC.Collect();
                GC.SuppressFinalize(this);
            });
        }

        private void LoadTree()
        {
            locationListview.BindingContext = this.BindingContext as NewAssessmentViewModel;
        }
        public void RefreshDobErrorStack()
        {
            ((NewAssessmentViewModel)this.BindingContext).DOBIsValid = false;
            DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
            DOB.TextColor = Colors.LightGrayColor;
            DOB.FontAttributes = FontAttributes.None;
            DOBFrame.BorderColor = Color.FromHex("#898D8D");
            DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
            DOBImageFrame.BorderColor = Color.FromHex("#898D8D");
            _dobErrorMsgShown = false;
        }

        public void RefreshEnrollmentErrorStack()
        {
            ((NewAssessmentViewModel)this.BindingContext).EnrollmentDateIsValid = false;
            EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
            EnrollmentDate.TextColor = Colors.LightGrayColor;
            EnrollmentDate.FontAttributes = FontAttributes.None;
            EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
            EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
            EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");
            _enrollmentErrorMsgShown = false;
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            (this.BindingContext as NewAssessmentViewModel)?.SelectLocationCommand.Execute(new object());
        }
        private void EnrollmentDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_enrollmentErrorMsgShown)
                RefreshEnrollmentErrorStack();
            _enrollmentErrorMsgShown = false;

            string entryText = (sender as Entry)?.Text;
            if (entryText != null && entryText.Length == 10)
            {
                if (this.BindingContext != null && entryText != "mm/dd/yyyy")
                {
                    ((NewAssessmentViewModel)this.BindingContext).EnrollmentDateIsValid = false;
                    EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                    EnrollmentDate.TextColor = Colors.LightGrayColor;
                    EnrollmentDate.FontAttributes = FontAttributes.None;
                    EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
                    EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
                    EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");
                }
            }
        }

        private void EnrollmentDate_Focused(object sender, FocusEventArgs e)
        {
            var entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                EnrollmentDate.Text = entryText;
            }
            else
            {
                ((NewAssessmentViewModel)this.BindingContext).EnrollmentDateIsValid = false;
                EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                EnrollmentDate.Text = "";
                EnrollmentDate.TextColor = Colors.LightGrayColor;
                EnrollmentDate.FontAttributes = FontAttributes.None;
                EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
                EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
                EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");

                if (((NewAssessmentViewModel)this.BindingContext).DOBIsValid && _dobErrorMsgShown)
                {
                    DOB.Focus();
                }

                else if (((NewAssessmentViewModel)this.BindingContext).DOBIsValid)
                {
                    RefreshDobErrorStack();
                }
            }

        }

        private void EnrollmentDate_Unfocused(object sender, FocusEventArgs e)
        {
            string entryText = (sender as Entry)?.Text;
            DateFormatStructure dateTime = HelperMethods.DateValidationForEnrollment(entryText);
            if (!string.IsNullOrEmpty(entryText) && !dateTime.result)
            {
                ((NewAssessmentViewModel)this.BindingContext).EnrollmentDateIsValid = true;
                EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                EnrollmentDate.Text = "mm/dd/yyyy";
                EnrollmentDate.TextColor = Color.FromHex("CC1417");
                EnrollmentDate.FontAttributes = FontAttributes.Bold;
                EnrollmentFrame.BorderColor = Color.FromHex("#CC1417");
                EnrollmentImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                EnrollmentImageFrame.BorderColor = Color.FromHex("#CC1417");
                Enrollment.Focus();
                _enrollmentErrorMsgShown = true;

                if (entryText == "mm/dd/yyyy" & _enrollmentErrorMsgShown)
                {
                    _enrollmentErrorMsgShown = false;
                }
            }
            else if (EnrollmentDate.Text.Length > 1)
            {
                EnrollmentDate.Text = dateTime.time.Month + "/" + dateTime.time.Day + "/" + dateTime.time.Year;
            }
        }

        private void DOB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_dobErrorMsgShown)
                RefreshDobErrorStack();
            _dobErrorMsgShown = false;

            var entryText = (sender as Entry)?.Text;
            if (entryText != null && entryText.Length == 10)
            {
                if (this.BindingContext != null && entryText != "mm/dd/yyyy")
                {
                    ((NewAssessmentViewModel)this.BindingContext).DOBIsValid = false;
                    DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
                    DOB.TextColor = Colors.LightGrayColor;
                    DOB.FontAttributes = FontAttributes.None;
                    DOBFrame.BorderColor = Color.FromHex("#898D8D");
                    DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
                    DOBImageFrame.BorderColor = Color.FromHex("#898D8D");
                }
            }
        }

        private void DOB_Focused(object sender, FocusEventArgs e)
        {
            string entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                DOB.Text = entryText;
            }
            else
            {
                ((NewAssessmentViewModel)this.BindingContext).DOBIsValid = false;
                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
                DOB.Text = "";
                DOB.TextColor = Colors.LightGrayColor;
                DOB.FontAttributes = FontAttributes.None;
                DOBFrame.BorderColor = Color.FromHex("#898D8D");
                DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
                DOBImageFrame.BorderColor = Color.FromHex("#898D8D");

                switch (((NewAssessmentViewModel)this.BindingContext).EnrollmentDateIsValid)
                {
                    case true when _enrollmentErrorMsgShown:
                        EnrollmentDate.Focus();
                        break;
                    case true:
                        RefreshEnrollmentErrorStack();
                        break;
                }
            }
        }

        private void DOB_Unfocused(object sender, FocusEventArgs e)
        {
            string entryText = (sender as Entry)?.Text;
            DateFormatStructure dateTime = HelperMethods.DateValidationForDOB(entryText);
            if (!string.IsNullOrEmpty(entryText) && !dateTime.result)
            {
                ((NewAssessmentViewModel)this.BindingContext).DOBIsValid = true;
                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                DOB.Text = "mm/dd/yyyy";
                DOB.TextColor = Color.FromHex("CC1417");
                DOB.FontAttributes = FontAttributes.Bold;
                DOBFrame.BorderColor = Color.FromHex("#CC1417");
                DOBImageFrame.BackgroundColor = Color.FromHex("#CC1417");
                DOBImageFrame.BorderColor = Color.FromHex("#CC1417");
                _dobErrorMsgShown = true;

                if (entryText == "mm/dd/yyyy" & _dobErrorMsgShown)
                {
                    _dobErrorMsgShown = false;
                }
            }
            else if (DOB.Text.Length > 1)
            {
                DOB.Text = dateTime.time.Month + "/" + dateTime.time.Day + "/" + dateTime.time.Year;
            }
        }

        private async void Dashboard_TappedAsync(object sender, EventArgs e)
        {
            this.locationListview.DisposeListView?.Invoke();
            this.TestRecordListView.DisposeListView?.Invoke();
            this.parentGrid.Children.Clear();
            this.Content = null;
            GC.Collect();
            GC.SuppressFinalize(this);
            await Navigation.PopModalAsync();
        }

        private void ReloadButtonClicked(object sender, EventArgs args)
        {
            DOB.Text = "";
            EnrollmentDate.Text = "";
            EnrollmentDate.BackgroundColor = DOB.BackgroundColor = EnrollmentDateStackLayout.BackgroundColor = DOBStackLayout.BackgroundColor = Color.White;
            DOB.TextColor = Colors.LightGrayColor;
            DOB.FontAttributes = FontAttributes.None;
            DOBFrame.BorderColor = Color.FromHex("#898D8D");
            DOBImageFrame.BackgroundColor = Color.FromHex("147cbd");
            DOBImageFrame.BorderColor = Color.FromHex("#898D8D");
            EnrollmentDate.TextColor = Colors.LightGrayColor;
            EnrollmentDate.FontAttributes = FontAttributes.None;
            EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
            EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
            EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D"); var viewModel = (NewAssessmentViewModel)BindingContext;
            viewModel.ReloadClicked();
        }

        private void SearchButtonClicked(object sender, EventArgs args)
        {
            string dob = null, enrollmentDate = null;
            if (!string.IsNullOrEmpty(DOB.Text))
            {
                if (DateTime.TryParse(DOB.Text, out var temp))
                {
                    dob = temp.ToString("MM/dd/yyyy");
                }
            }
            if (!string.IsNullOrEmpty(EnrollmentDate.Text))
            {
                if (DateTime.TryParse(EnrollmentDate.Text, out var temp))
                {
                    enrollmentDate = temp.ToString("MM/dd/yyyy");
                }
            }
            MyNewAssessmentViewModel.SearchClicked(dob, enrollmentDate);
        }

        void DOB_Tapped(object sender, EventArgs e)
        {
            RefreshDobErrorStack();
            if (Device.RuntimePlatform == Device.UWP)
            {
                DatePicker_DOB.ShowDatePicker();
            }
            else
            {
                DatePicker_DOB.Focus();
            }
        }

        void Enrollment_Tapped(object sender, EventArgs e)
        {
            RefreshEnrollmentErrorStack();
            if (Device.RuntimePlatform == Device.UWP)
            {
                DatePicker_EnrollmentDate.ShowDatePicker();
            }
            else
            {
                DatePicker_EnrollmentDate.Focus();
            }
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);

            if (Device.RuntimePlatform == Device.UWP)
            {
                SelectCountryFrame.CornerRadius = 0;
                SearchDeleteChild.VerticalOptions = LayoutOptions.End;
            }

            if (Device.RuntimePlatform == Device.Android)
            {
                SelectCountryFrame.CornerRadius = 0;
                SearchDeleteChild.VerticalOptions = LayoutOptions.Center;
            }

            if (height > width)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    NameRow.Height = new GridLength(1, GridUnitType.Star);
                    EnrollmentRow.Height = new GridLength(1, GridUnitType.Star);
                    SeparatorRow.Height = new GridLength(0.3, GridUnitType.Star);
                    TableRow.Height = new GridLength(8, GridUnitType.Star);
                    ShareRow.Height = new GridLength(1, GridUnitType.Star);
                    DOBErrorText.Padding = new Thickness(10, -15, 100, 0);
                    EnrollmentDateErrorText.Padding = new Thickness(10, -15, 100, 0);
                    Thickness dobThickness = DOBErrorText.Margin;
                    DOBErrorText.Margin = new Thickness(dobThickness.Left, 0, dobThickness.Right, dobThickness.Bottom);
                    Thickness enrollmentThickness = EnrollmentDateErrorText.Margin;
                    EnrollmentDateErrorText.Margin = new Thickness(enrollmentThickness.Left, 0, enrollmentThickness.Right, enrollmentThickness.Bottom);
                }
                if (Device.RuntimePlatform == Device.Android)
                {
                    NameField.Text = "Last Name,\n" + "First Name";
                    NameRow.Height = new GridLength(1.5, GridUnitType.Star);
                    EnrollmentRow.Height = new GridLength(1.5, GridUnitType.Star);
                    SeparatorRow.Height = new GridLength(0.3, GridUnitType.Star);
                    TableRow.Height = new GridLength(8, GridUnitType.Star);
                    ShareRow.Height = new GridLength(1, GridUnitType.Star);
                    DOBErrorText.Padding = new Thickness(10, -25, 100, 0);
                    EnrollmentDateErrorText.Padding = new Thickness(10, -25, 100, 0);
                }
            }
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                {
                    DOBErrorText.Padding = new Thickness(10, -13, 100, 0);
                    EnrollmentDateErrorText.Padding = new Thickness(10, -13, 100, 0);
                    Thickness dobThickness = DOBErrorText.Margin;
                    DOBErrorText.Margin = new Thickness(dobThickness.Left, -15, dobThickness.Right, dobThickness.Bottom);
                    Thickness enrollmentThickness = EnrollmentDateErrorText.Margin;
                    EnrollmentDateErrorText.Margin = new Thickness(enrollmentThickness.Left, -15, enrollmentThickness.Right, enrollmentThickness.Bottom);
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    NameField.Text = "Last Name, " + "First Name";
                    DOBErrorText.Padding = new Thickness(10, -10, 100, 0);
                    EnrollmentDateErrorText.Padding = new Thickness(10, -10, 100, 0);
                }
                if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
                {
                    NameRow.Height = new GridLength(2, GridUnitType.Star);
                    EnrollmentRow.Height = new GridLength(2, GridUnitType.Star);
                    SeparatorRow.Height = new GridLength(1, GridUnitType.Star);
                    TableRow.Height = new GridLength(8, GridUnitType.Star);
                    ShareRow.Height = new GridLength(1, GridUnitType.Star);
                }
            }
        }

        private void ChildRecords_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        private void ChildRecords_SizeChanged(object sender, EventArgs e)
        {
            var listview = (ListView)sender;
            MyNewAssessmentViewModel.UpdateListviewBoundCommand.Execute(listview.Height);
        }
        void CountryList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender != null)
            {
                if (e.SelectedItem != null)
                {
                    CountryName.Text = ((ListView)sender).SelectedItem.ToString();
                    ((ListView)sender).SelectedItem = null;
                    SelectCountryName.IsVisible = false;
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (specialLabel.TextColor == Color.Gray)
            {
                specialLabel.TextColor = Color.Black;
                SpecialTest.TextColor = Color.LightGray;
            }

            else if (specialLabel.TextColor == Color.Black)
            {
                specialLabel.TextColor = Color.LightGray;
                SpecialTest.TextColor = Color.Black;
            }

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;

            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;


            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            specialLabe.TextColor = Color.Gray;
            SpecialTes.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (SpecialTest.TextColor == Color.Black)
            {
                specialLabel.TextColor = Color.Black;
                SpecialTest.TextColor = Color.LightGray;
            }

            else if (SpecialTest.TextColor == Color.LightGray)
            {
                specialLabel.TextColor = Color.Black;
                SpecialTest.TextColor = Color.LightGray;
            }

            else if (SpecialTest.TextColor == Color.Gray)
            {
                SpecialTest.TextColor = Color.Black;
                specialLabel.TextColor = Color.LightGray;
            }

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;


            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;

            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            specialLabe.TextColor = Color.Gray;
            SpecialTes.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {

            if (special.TextColor == Color.Gray)
            {
                special.TextColor = Color.Black;
                specialLa.TextColor = Color.LightGray;
            }

            else if (special.TextColor == Color.Black)
            {
                special.TextColor = Color.LightGray;
                specialLa.TextColor = Color.Black;
            }

            specialLabel.TextColor = Color.Gray;
            SpecialTest.TextColor = Color.Gray;

            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;


            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            specialLabe.TextColor = Color.Gray;
            SpecialTes.TextColor = Color.Gray;



        }

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            if (specialLa.TextColor == Color.Black)
            {
                special.TextColor = Color.Black;
                specialLa.TextColor = Color.LightGray;
            }

            else if (specialLa.TextColor == Color.LightGray)
            {
                special.TextColor = Color.Black;
                specialLa.TextColor = Color.LightGray;
            }

            else if (specialLa.TextColor == Color.Gray)
            {
                specialLa.TextColor = Color.Black;
                special.TextColor = Color.LightGray;
            }

            specialLabel.TextColor = Color.Gray;
            SpecialTest.TextColor = Color.Gray;


            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;

            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            specialLabe.TextColor = Color.Gray;
            SpecialTes.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_4(object sender, EventArgs e)
        {
            if (specia1.TextColor == Color.Gray)
            {
                specia1.TextColor = Color.Black;
                specialLa2.TextColor = Color.LightGray;
            }

            else if (specia1.TextColor == Color.Black)
            {
                specia1.TextColor = Color.LightGray;
                specialLa2.TextColor = Color.Black;
            }

            specialLabel.TextColor = Color.Gray;
            SpecialTest.TextColor = Color.Gray;

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;

            specialLabel.TextColor = Color.Gray;
            SpecialTest.TextColor = Color.Gray;

            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_5(object sender, EventArgs e)
        {
            if (specialLa2.TextColor == Color.Black)
            {
                specia1.TextColor = Color.Black;
                specialLa2.TextColor = Color.LightGray;
            }

            else if (specialLa2.TextColor == Color.LightGray)
            {
                specia1.TextColor = Color.Black;
                specialLa2.TextColor = Color.LightGray;
            }

            else if (specialLa2.TextColor == Color.Gray)
            {
                specialLa2.TextColor = Color.Black;
                specia1.TextColor = Color.LightGray;
            }

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;

            SpecialTest.TextColor = Color.Gray;
            specialLabel.TextColor = Color.Gray;


            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            specialLabe.TextColor = Color.Gray;
            SpecialTes.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_6(object sender, EventArgs e)
        {
            if (EnrollmentA.TextColor == Color.Gray)
            {
                EnrollmentA.TextColor = Color.Black;
                Enrollment.TextColor = Color.LightGray;
            }

            else if (EnrollmentA.TextColor == Color.Black)
            {
                EnrollmentA.TextColor = Color.LightGray;
                Enrollment.TextColor = Color.Black;
            }

            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;

            specialLabel.TextColor = Color.Gray;
            SpecialTest.TextColor = Color.Gray;


            specialLa.TextColor = Color.Gray;
            specia1.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_7(object sender, EventArgs e)
        {

            if (Enrollment.TextColor == Color.Black)
            {
                EnrollmentA.TextColor = Color.Black;
                Enrollment.TextColor = Color.LightGray;
            }

            else if (Enrollment.TextColor == Color.LightGray)
            {
                EnrollmentA.TextColor = Color.Black;
                Enrollment.TextColor = Color.LightGray;
            }

            else if (Enrollment.TextColor == Color.Gray)
            {
                Enrollment.TextColor = Color.Black;
                EnrollmentA.TextColor = Color.LightGray;
            }

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;

            SpecialTest.TextColor = Color.Gray;
            specialLabel.TextColor = Color.Gray;

            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;


            specialLabe.TextColor = Color.Gray;
            SpecialTes.TextColor = Color.Gray;

        }

        private void TapGestureRecognizer_Tapped_8(object sender, EventArgs e)
        {
            if (specialLabe.TextColor == Color.Gray)
            {
                specialLabe.TextColor = Color.Black;
                SpecialTes.TextColor = Color.LightGray;
            }

            else if (specialLabe.TextColor == Color.Black)
            {
                specialLabe.TextColor = Color.LightGray;
                SpecialTes.TextColor = Color.Black;
            }
            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            SpecialTest.TextColor = Color.Gray;
            specialLabel.TextColor = Color.Gray;

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;


            specialLa.TextColor = Color.Gray;
            specia1.TextColor = Color.Gray;
        }

        private void TapGestureRecognizer_Tapped_9(object sender, EventArgs e)
        {

            if (SpecialTes.TextColor == Color.Black)
            {
                specialLabe.TextColor = Color.Black;
                SpecialTes.TextColor = Color.LightGray;
            }

            else if (SpecialTes.TextColor == Color.LightGray)
            {
                specialLabe.TextColor = Color.Black;
                SpecialTes.TextColor = Color.LightGray;
            }

            else if (SpecialTes.TextColor == Color.Gray)
            {
                SpecialTes.TextColor = Color.Black;
                specialLabe.TextColor = Color.LightGray;
            }

            EnrollmentA.TextColor = Color.Gray;
            Enrollment.TextColor = Color.Gray;

            special.TextColor = Color.Gray;
            specialLa.TextColor = Color.Gray;

            SpecialTest.TextColor = Color.Gray;
            specialLabel.TextColor = Color.Gray;

            specia1.TextColor = Color.Gray;
            specialLa2.TextColor = Color.Gray;
        }
        private void Level1ImageButton_Clicked(object sender, EventArgs e)
        {

            var clickedCell = (sender as ImageButton)?.BindingContext as LocationNew;
            var locations = (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection;
            var subLocations = ((sender as ImageButton)?.BindingContext as LocationNew)?.SubLocations.OrderByDescending(l => l.LocationName);

            if (subLocations != null)
            {
                foreach (var item in subLocations)
                {
                    if (locations != null && !locations.Contains(item))
                    {
                        var index = locations.IndexOf(clickedCell) + 1;
                        if (clickedCell != null) clickedCell.IsExpanded = true;
                        (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection.Insert(index, item);
                    }
                    else
                    {
                        if (item.SubLocations != null)
                        {
                            foreach (var subItem in item.SubLocations)
                            {
                                if (subItem.SubLocations != null)
                                {
                                    foreach (var subItem2 in subItem.SubLocations)
                                    {
                                        if (subItem2.SubLocations != null)
                                        {
                                            foreach (var subItem3 in subItem2.SubLocations)
                                            {
                                                (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection.Remove(subItem3);
                                            }
                                        }
                                        subItem2.IsExpanded = false;
                                        (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection.Remove(subItem2);
                                    }
                                }
                                subItem.IsExpanded = false;
                                (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection.Remove(subItem);
                            }
                        }
                        item.IsExpanded = false;
                        if (clickedCell != null) clickedCell.IsExpanded = false;
                        (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection.Remove(item);

                    }
                }
            }
        }

        private void LocationListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (LocationNew)e.SelectedItem;
            var locations = (this.BindingContext as NewAssessmentViewModel)?.LocationsObservableCollection;
            if (locations != null && locations.Contains(item))
            {
                if (!item.IsEnabled)
                    return;
                item.IsSelected = !item.IsSelected;
                if (item.SubLocations != null)
                {
                    foreach (var subitem in item.SubLocations)
                    {
                        subitem.IsSelected = item.IsSelected;
                        if (subitem.SubLocations != null)
                        {
                            foreach (var subitem2 in subitem.SubLocations)
                            {
                                subitem2.IsSelected = item.IsSelected;
                                if (subitem2.SubLocations != null)
                                {
                                    foreach (var subitem3 in subitem2.SubLocations)
                                    {
                                        subitem3.IsSelected = item.IsSelected;
                                        if (subitem3.SubLocations != null)
                                        {
                                            foreach (var subitem4 in subitem3.SubLocations)
                                            {
                                                subitem4.IsSelected = item.IsSelected;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (item.IsSelected)
                {
                    if (!item.IsAddedToCount)
                    {
                        _selectedCount = _selectedCount + 1;
                        item.IsAddedToCount = true;
                    }
                    if (!(MyNewAssessmentViewModel.SelectedLocations.Contains(item.LocationName)))
                        MyNewAssessmentViewModel.SelectedLocations.Add(item.LocationName);

                    if (item.SubLocations != null)
                    {
                        foreach (var locs in item.SubLocations)
                        {
                            if (!locs.IsAddedToCount)
                            {
                                _selectedCount = _selectedCount + 1;
                                locs.IsAddedToCount = true;
                            }
                            if (!(MyNewAssessmentViewModel.SelectedLocations.Contains(locs.LocationName)))
                                MyNewAssessmentViewModel.SelectedLocations.Add(locs.LocationName);

                            CountryName.Text = _selectedCount + " Selected";
                            if (locs.SubLocations != null)
                            {
                                foreach (var locs2 in locs.SubLocations)
                                {
                                    if (!locs2.IsAddedToCount)
                                    {
                                        _selectedCount = _selectedCount + 1;
                                        locs2.IsAddedToCount = true;
                                    }
                                    if (!(MyNewAssessmentViewModel.SelectedLocations.Contains(locs2.LocationName)))
                                        MyNewAssessmentViewModel.SelectedLocations.Add(locs2.LocationName);

                                    CountryName.Text = _selectedCount + " Selected";
                                    if (locs2.SubLocations != null)
                                    {
                                        foreach (var locs3 in locs2.SubLocations)
                                        {
                                            if (!locs3.IsAddedToCount)
                                            {
                                                _selectedCount = _selectedCount + 1;
                                                locs3.IsAddedToCount = true;
                                            }
                                            if (!(MyNewAssessmentViewModel.SelectedLocations.Contains(locs3.LocationName)))
                                                MyNewAssessmentViewModel.SelectedLocations.Add(locs3.LocationName);

                                            CountryName.Text = _selectedCount + " Selected";
                                            if (locs3.SubLocations != null)
                                            {
                                                foreach (var locs4 in locs3.SubLocations)
                                                {
                                                    if (!locs4.IsAddedToCount)
                                                    {
                                                        _selectedCount = _selectedCount + 1;
                                                        locs4.IsAddedToCount = true;
                                                    }
                                                    CountryName.Text = _selectedCount + " Selected";

                                                    if (!(MyNewAssessmentViewModel.SelectedLocations.Contains(locs4.LocationName)))
                                                        MyNewAssessmentViewModel.SelectedLocations.Add(locs4.LocationName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (_selectedCount == 0)
                        {
                            CountryName.Text = item.LocationName;
                        }
                        else
                        {
                            CountryName.Text = _selectedCount + " Selected";
                        }
                    }
                }
                else
                {
                    if (item.SubLocations != null)
                    {
                        if (_selectedCount > 0 && item.IsAddedToCount)
                        {
                            _selectedCount = _selectedCount - 1;
                        }
                        if (MyNewAssessmentViewModel.SelectedLocations.Contains(item.LocationName))
                            MyNewAssessmentViewModel.SelectedLocations.Remove(item.LocationName);
                        item.IsAddedToCount = false;
                        foreach (var locs in item.SubLocations)
                        {
                            if (_selectedCount > 0 && locs.IsAddedToCount)
                            {
                                _selectedCount = _selectedCount - 1;
                            }
                            locs.IsAddedToCount = false;
                            if (MyNewAssessmentViewModel.SelectedLocations.Contains(locs.LocationName))
                                MyNewAssessmentViewModel.SelectedLocations.Remove(locs.LocationName);
                            CountryName.Text = _selectedCount + " Selected";
                            if (locs.SubLocations != null)
                            {
                                foreach (var locs2 in locs.SubLocations)
                                {
                                    if (_selectedCount > 0 && locs2.IsAddedToCount)
                                    {
                                        _selectedCount = _selectedCount - 1;
                                    }
                                    locs2.IsAddedToCount = false;
                                    if (MyNewAssessmentViewModel.SelectedLocations.Contains(locs2.LocationName))
                                        MyNewAssessmentViewModel.SelectedLocations.Remove(locs2.LocationName);
                                    CountryName.Text = _selectedCount + " Selected";
                                    if (locs2.SubLocations != null)
                                    {
                                        foreach (var locs3 in locs2.SubLocations)
                                        {
                                            if (_selectedCount > 0 && locs3.IsAddedToCount)
                                            {
                                                _selectedCount = _selectedCount - 1;
                                            }
                                            locs3.IsAddedToCount = false;
                                            if (MyNewAssessmentViewModel.SelectedLocations.Contains(locs3.LocationName))
                                                MyNewAssessmentViewModel.SelectedLocations.Remove(locs3.LocationName);
                                            CountryName.Text = _selectedCount + " Selected";
                                            if (locs3.SubLocations != null)
                                            {
                                                foreach (var locs4 in locs3.SubLocations)
                                                {
                                                    if (_selectedCount > 0)
                                                    {
                                                        _selectedCount = _selectedCount - 1;
                                                    }
                                                    locs4.IsAddedToCount = false;
                                                    CountryName.Text = _selectedCount + " Selected";
                                                    if (MyNewAssessmentViewModel.SelectedLocations.Contains(locs4.LocationName))
                                                        MyNewAssessmentViewModel.SelectedLocations.Remove(locs4.LocationName);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _selectedCount = _selectedCount - 1;
                        item.IsAddedToCount = false;
                        if (MyNewAssessmentViewModel.SelectedLocations.Contains(item.LocationName))
                            MyNewAssessmentViewModel.SelectedLocations.Remove(item.LocationName);
                        CountryName.Text = _selectedCount + " Selected";
                    }
                    if (_selectedCount == 0)
                    {
                        CountryName.Text = "Select location(s)";
                        MyNewAssessmentViewModel.SelectedLocations.Clear();
                        MyNewAssessmentViewModel.SelectedLocations = new List<string>();
                    }
                }
                if (_selectedCount == 1)
                {
                    var itemSource = locationListview.ItemsSource.Cast<LocationNew>().ToList();
                    foreach (var items in itemSource)
                    {
                        if (items.IsSelected)
                        {
                            CountryName.Text = items.LocationName;
                        }
                    }
                }
                locationListview.SelectedItem = null;
            }
        }
        protected override void OnDisappearing()
        {
            try
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                    DependencyService.Get<IKeyboardHelper>().HideKeyboard();
            }
            catch (Exception)
            {

            }
            base.OnDisappearing();
        }
    }
}
