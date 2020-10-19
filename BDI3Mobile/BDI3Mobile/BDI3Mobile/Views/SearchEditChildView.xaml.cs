using BDI3Mobile.Common;
using BDI3Mobile.Helpers;
using BDI3Mobile.Models.DBModels;
using BDI3Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Linq;
using BDI3Mobile.IServices;
using Xamarin.Essentials;

namespace BDI3Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchEditChildView
    {
        private int _selectedCount;
        public SearchEditViewModel MySearchEditViewModel { get; set; }
        private bool _dobErrorMsgShown;
        private bool _enrollmentErrorMsgShown;

        public SearchEditChildView()
        {
            MySearchEditViewModel = new SearchEditViewModel();
            BindingContext = MySearchEditViewModel;
            InitializeComponent();

            SelectCountryName.IsVisible = false;

            MessagingCenter.Subscribe<String, bool>(this, "Tab", (text, value) =>
            {
                LocationImageButton.IsTabStop = value;
                ChildFirstName.IsTabStop = value;
                ChildLastName.IsTabStop = value;
                CountryName.IsTabStop = value;
                ChildID.IsTabStop = value;
                DOB.IsTabStop = value;
                EnrollmentDate.IsTabStop = value;
            });

            MySearchEditViewModel.SelectedLocations = new List<string>();
            LoadTree();                       
        }
        private void LoadTree()
        {
            locationListview.BindingContext = this.BindingContext as SearchEditViewModel;
        }
        public void RefreshDobErrorStack()
        {
            ((SearchEditViewModel) this.BindingContext).DOBIsValid = false;
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
            ((SearchEditViewModel) this.BindingContext).EnrollmentDateIsValid = false;
            EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
            EnrollmentDate.TextColor = Colors.LightGrayColor;
            EnrollmentDate.FontAttributes = FontAttributes.None;
            EnrollmentFrame.BorderColor = Color.FromHex("#898D8D");
            EnrollmentImageFrame.BackgroundColor = Color.FromHex("147cbd");
            EnrollmentImageFrame.BorderColor = Color.FromHex("#898D8D");
            _enrollmentErrorMsgShown = false;
        }
        #region Event handler
        private void EnrollmentDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_enrollmentErrorMsgShown)
                RefreshEnrollmentErrorStack();
            _enrollmentErrorMsgShown = false;

            string entryText = (sender as Entry)?.Text;
            if (entryText != null && entryText.Length != 10) return;
            if (this.BindingContext == null || entryText == "mm/dd/yyyy") return;
            ((SearchEditViewModel) this.BindingContext).EnrollmentDateIsValid = false;
            EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
            EnrollmentDate.TextColor = Colors.LightGrayColor;
            EnrollmentDate.FontAttributes = FontAttributes.None;
            EnrollmentFrame.BorderColor = Colors.BorderColor;
            EnrollmentImageFrame.BackgroundColor = Colors.FrameBlueColor;
            EnrollmentImageFrame.BorderColor = Colors.BorderColor;
        }

        private void EnrollmentDate_Focused(object sender, FocusEventArgs e)
        {
            string entryText = (sender as Entry)?.Text;
            if (DateTime.TryParseExact(entryText, "MM/dd/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            {
                EnrollmentDate.Text = entryText;
            }
            else
            {
                ((SearchEditViewModel) this.BindingContext).EnrollmentDateIsValid = false;
                EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.White;
                EnrollmentDate.Text = "";
                EnrollmentDate.TextColor = Colors.LightGrayColor;
                EnrollmentDate.FontAttributes = FontAttributes.None;
                EnrollmentFrame.BorderColor = Colors.BorderColor;
                EnrollmentImageFrame.BackgroundColor = Colors.FrameBlueColor;
                EnrollmentImageFrame.BorderColor = Colors.BorderColor;

                if (((SearchEditViewModel) this.BindingContext).DOBIsValid && _dobErrorMsgShown)
                {
                    DOB.Focus();
                }

                else if (((SearchEditViewModel) this.BindingContext).DOBIsValid)
                {
                    RefreshDobErrorStack();
                }
            }
        }

        private void EnrollmentDate_Unfocused(object sender, FocusEventArgs e)
        {
            var entryText = (sender as Entry)?.Text;
            var dateTime = HelperMethods.DateValidationForEnrollment(entryText);
            if (!string.IsNullOrEmpty(entryText) && !dateTime.result)
            {
                ((SearchEditViewModel) this.BindingContext).EnrollmentDateIsValid = true;
                EnrollmentDateStackLayout.BackgroundColor = EnrollmentDate.BackgroundColor = Color.FromHex("#FFF1F1");
                EnrollmentDate.Text = "mm/dd/yyyy";
                EnrollmentDate.TextColor = Color.FromHex("CC1417");
                EnrollmentDate.FontAttributes = FontAttributes.Bold;
                EnrollmentFrame.BorderColor = Colors.ErrorRedColor;
                EnrollmentImageFrame.BackgroundColor = Colors.ErrorRedColor;
                EnrollmentImageFrame.BorderColor = Colors.ErrorRedColor;
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
            if (entryText == null || entryText.Length != 10) return;
            if (this.BindingContext == null || entryText == "mm/dd/yyyy") return;
            ((SearchEditViewModel) this.BindingContext).DOBIsValid = false;
            DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
            DOB.TextColor = Colors.LightGrayColor;
            DOB.FontAttributes = FontAttributes.None;
            DOBFrame.BorderColor = Colors.BorderColor;
            DOBImageFrame.BackgroundColor = Colors.FrameBlueColor;
            DOBImageFrame.BorderColor = Colors.BorderColor;
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
                ((SearchEditViewModel) this.BindingContext).DOBIsValid = false;
                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.White;
                DOB.Text = "";
                DOB.TextColor = Colors.LightGrayColor;
                DOB.FontAttributes = FontAttributes.None;
                DOBFrame.BorderColor = Colors.BorderColor;
                DOBImageFrame.BackgroundColor = Colors.FrameBlueColor;
                DOBImageFrame.BorderColor = Colors.BorderColor;

                switch (((SearchEditViewModel) this.BindingContext).EnrollmentDateIsValid)
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
            var entryText = (sender as Entry)?.Text;
            DateFormatStructure dateTime = HelperMethods.DateValidationForDOB(entryText);
            if (!string.IsNullOrEmpty(entryText) && !dateTime.result)
            {
                ((SearchEditViewModel) this.BindingContext).DOBIsValid = true;
                DOBStackLayout.BackgroundColor = DOB.BackgroundColor = Color.FromHex("#FFF1F1");
                DOB.Text = "mm/dd/yyyy";
                DOB.TextColor = Color.FromHex("CC1417");
                DOB.FontAttributes = FontAttributes.Bold;
                DOBFrame.BorderColor = Colors.ErrorRedColor;
                DOBImageFrame.BackgroundColor = Colors.ErrorRedColor;
                DOBImageFrame.BorderColor = Colors.ErrorRedColor;
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
            if(Device.RuntimePlatform == Device.UWP)
            {
                DatePicker_EnrollmentDate.ShowDatePicker();
            }
            else
            {
                DatePicker_EnrollmentDate.Focus();
            }
           
        }

        #endregion

        #region Clicked Evnt hanlr
        private void ChildRecords_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
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
            MySearchEditViewModel.SearchClicked(dob, enrollmentDate);
        }

        private void ReloadButtonClicked(object sender, EventArgs args)
        {
            DOB.Text = "";
            EnrollmentDate.Text = "";
            EnrollmentDate.BackgroundColor = DOB.BackgroundColor = EnrollmentDateStackLayout.BackgroundColor = DOBStackLayout.BackgroundColor = Color.White;
            DOBFrame.BorderColor = Colors.BorderColor;
            DOBImageFrame.BackgroundColor = Colors.FrameBlueColor;
            DOBImageFrame.BorderColor = Colors.FrameBlueColor;
            EnrollmentFrame.BorderColor = Colors.BorderColor;
            EnrollmentImageFrame.BackgroundColor = Colors.FrameBlueColor;
            EnrollmentImageFrame.BorderColor = Colors.FrameBlueColor;
            EnrollmentDate.FontAttributes = FontAttributes.None;
            DOB.FontAttributes = FontAttributes.None;
            MySearchEditViewModel.ReloadClicked();
        }

        private void ChildRecords_SizeChanged(object sender, EventArgs e)
        {
            var listview = (ListView)sender;
            if (listview == null) throw new ArgumentNullException(nameof(listview));
            MySearchEditViewModel.UpdateListviewBoundCommand.Execute(listview.Height);
        }
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            (this.BindingContext as SearchEditViewModel)?.SelectLocationCommand.Execute(new object());
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

        private async void Dashboard_TappedAsync(object sender, EventArgs e)
        {
            try
            {
                this.BindingContext = null;
                this.parentGrid.Children.Clear();
                this.Content = null;
                if (this.MySearchEditViewModel != null)
                {
                    this.MySearchEditViewModel.BindingContext = null;
                    this.MySearchEditViewModel = null;
                }
            }
            catch(Exception ex)
            {

            }
            GC.Collect();
            GC.SuppressFinalize(this);
            await Navigation.PopModalAsync();
        }

        #endregion

        #region Overide
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Device.RuntimePlatform == Device.iOS)
            {
                ChildRecords.SeparatorVisibility = SeparatorVisibility.None;
            }
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
                    NameField.Text = "Last Name,\n" + "First Name";
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
                    NameField.Text = "Last Name, " + "First Name";
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
        #endregion

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
            var locations = (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection;
            var subLocations = ((sender as ImageButton)?.BindingContext as LocationNew)?.SubLocations.OrderByDescending(l => l.LocationName);

            if (subLocations != null)
            {
                foreach (var item in subLocations)
                {
                    if (locations != null && !locations.Contains(item))
                    {
                        var index = locations.IndexOf(clickedCell) + 1;
                        if (clickedCell != null) clickedCell.IsExpanded = true;
                        (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection.Insert(index, item);
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
                                                (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection.Remove(subItem3);
                                            }
                                        }
                                        subItem2.IsExpanded = false;
                                        (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection.Remove(subItem2);
                                    }
                                }
                                subItem.IsExpanded = false;
                                (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection.Remove(subItem);
                            }
                        }
                        item.IsExpanded = false;
                        if (clickedCell != null) clickedCell.IsExpanded = false;
                        (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection.Remove(item);

                    }
                }
            }
        }

        private void LocationListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = (LocationNew)e.SelectedItem;
            var locations = (this.BindingContext as SearchEditViewModel)?.LocationsObservableCollection;
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
                    if(!(MySearchEditViewModel.SelectedLocations.Contains(item.LocationName)))
                       MySearchEditViewModel.SelectedLocations.Add(item.LocationName);

                    if (item.SubLocations != null)
                    {
                        foreach (var locs in item.SubLocations)
                        {
                            if (!locs.IsAddedToCount)
                            {
                                _selectedCount = _selectedCount + 1;
                                locs.IsAddedToCount = true;
                            }
                            CountryName.Text = _selectedCount + " Selected";
                            if (!(MySearchEditViewModel.SelectedLocations.Contains(locs.LocationName)))
                                MySearchEditViewModel.SelectedLocations.Add(locs.LocationName);

                            if (locs.SubLocations != null)
                            {
                                foreach (var locs2 in locs.SubLocations)
                                {
                                    if (!locs2.IsAddedToCount)
                                    {
                                        _selectedCount = _selectedCount + 1;
                                        locs2.IsAddedToCount = true;
                                    }
                                    CountryName.Text = _selectedCount + " Selected";
                                    if (!(MySearchEditViewModel.SelectedLocations.Contains(locs2.LocationName)))
                                       MySearchEditViewModel.SelectedLocations.Add(locs2.LocationName);

                                    if (locs2.SubLocations != null)
                                    {
                                        foreach (var locs3 in locs2.SubLocations)
                                        {
                                            if (!locs3.IsAddedToCount)
                                            {
                                                _selectedCount = _selectedCount + 1;
                                                locs3.IsAddedToCount = true;
                                            }
                                            if (!(MySearchEditViewModel.SelectedLocations.Contains(locs3.LocationName)))
                                                MySearchEditViewModel.SelectedLocations.Add(locs3.LocationName);

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
                                                    if (!(MySearchEditViewModel.SelectedLocations.Contains(locs4.LocationName)))
                                                        MySearchEditViewModel.SelectedLocations.Add(locs4.LocationName);
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
                        if (_selectedCount == 1)
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
                        if (MySearchEditViewModel.SelectedLocations.Contains(item.LocationName))
                            MySearchEditViewModel.SelectedLocations.Remove(item.LocationName);
                        item.IsAddedToCount = false;
                        foreach (var locs in item.SubLocations)
                        {
                            if (_selectedCount > 0 && locs.IsAddedToCount)
                            {
                                _selectedCount = _selectedCount - 1;
                            }
                            locs.IsAddedToCount = false;

                            if(MySearchEditViewModel.SelectedLocations.Contains(locs.LocationName))
                                MySearchEditViewModel.SelectedLocations.Remove(locs.LocationName);

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
                                    if (MySearchEditViewModel.SelectedLocations.Contains(locs2.LocationName))
                                        MySearchEditViewModel.SelectedLocations.Remove(locs2.LocationName);

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
                                            if (MySearchEditViewModel.SelectedLocations.Contains(locs3.LocationName))
                                                MySearchEditViewModel.SelectedLocations.Remove(locs3.LocationName);
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
                                                    if (MySearchEditViewModel.SelectedLocations.Contains(locs4.LocationName))
                                                        MySearchEditViewModel.SelectedLocations.Remove(locs4.LocationName);
                                                    CountryName.Text = _selectedCount + " Selected";
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
                        if (MySearchEditViewModel.SelectedLocations.Contains(item.LocationName))
                            MySearchEditViewModel.SelectedLocations.Remove(item.LocationName);
                        CountryName.Text = _selectedCount + " Selected";
                    }
                    if (_selectedCount == 0)
                    {
                        CountryName.Text = "Select location(s)";
                        MySearchEditViewModel.SelectedLocations.Clear();
                        MySearchEditViewModel.SelectedLocations = new List<string>();
                    }
                    
                }               
            }

            if (_selectedCount == 1)
            {
                var itemSource = locationListview.ItemsSource.Cast<LocationNew>().ToList();
                foreach (var items in itemSource.Where(items => items.IsSelected))
                {
                    CountryName.Text = items.LocationName;
                }
            }

            locationListview.SelectedItem = null;

        }

        protected override void OnAppearing()
        {
            DOB.Unfocused += DOB_Unfocused;
            DOB.Focused += DOB_Focused;
            DOB.TextChanged += DOB_TextChanged;
            EnrollmentDate.Unfocused += EnrollmentDate_Unfocused;
            EnrollmentDate.Focused += EnrollmentDate_Focused;
            EnrollmentDate.TextChanged += EnrollmentDate_TextChanged;
            locationListview.ItemSelected += LocationListview_ItemSelected;

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            try
            {
                DOB.Unfocused -= DOB_Unfocused;
                DOB.Focused -= DOB_Focused;
                DOB.TextChanged -= DOB_TextChanged;
                EnrollmentDate.Unfocused -= EnrollmentDate_Unfocused;
                EnrollmentDate.Focused -= EnrollmentDate_Focused;
                EnrollmentDate.TextChanged -= EnrollmentDate_TextChanged;
                locationListview.ItemSelected -= LocationListview_ItemSelected;

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
