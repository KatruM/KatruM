using BDI3Mobile.CustomRenderer;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.ItemAdministrationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AcademicTestSessionOverview : PopupPage
    {
        public AcademicTestSessionOverviewModel vM { get; set; }

        public string DomainCode { get; set; }

        public AcademicTestSessionOverview()
        {
                InitializeComponent();
                this.CloseWhenBackgroundIsClicked = false;
                vM = new AcademicTestSessionOverviewModel(false);
                BindingContext = vM;

                //iOS - CollectionView [Entry issue inside ListView]
                if (Device.RuntimePlatform != Device.iOS)
                {
                    TSOList1.IsVisible = true;
                    TSOGrid.IsVisible = true;
                    TSOList1iOS.IsVisible = false;
                }
                else
                {
                    TSOList1iOS.IsVisible = true;
                    TSOList1.IsVisible = false;
                    TSOGrid.IsVisible = false;
                }
            
        }
        /// <summary>
        /// Grid size changes for TSO.
        /// </summary>
        public void ResizeGrid()
        {
            if (Device.RuntimePlatform == Device.UWP)
            {
                
                TSOGrid.ColumnDefinitions[12].Width = 0;
                TSOGrid.ColumnDefinitions[11].Width = 0;
                TSOGrid.ColumnDefinitions[10].Width = 0;
            }
        }
        private async void DatePicker_DOB_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedDate")
            {
                var date = sender as MyDatePicker;
                if (date != null && string.IsNullOrWhiteSpace(date.SelectedDate))
                    return;
                var hasError = false;
                var errorMessage = "";
                if (date != null && Convert.ToDateTime(date.SelectedDate).Date > DateTime.Now)
                {
                    hasError = true;
                    errorMessage = "Date entered cannot exceed current date.";
                }
                else
                {
                    if (date != null)
                    {
                        var selectedDate = Convert.ToDateTime(date.SelectedDate);
                        if (!(selectedDate.Date <= vM.MaximumDate && selectedDate.Date >= vM.MinimumDate))
                        {
                            hasError = true;
                        }
                    }
                }
                if (hasError)
                {
                    date.SelectedDate = string.Empty; //This will trigger the SelectedDate again.
                    var firstTestRecord = vM.AcademicTestSessionRecords.FirstOrDefault(p => p.IsDateVisible);
                    if (errorMessage.Length == 0)
                    {
                        if (firstTestRecord.DomainCode == DomainCode)
                        {
                            errorMessage = "Testing ranges must be within 30 days of testing each subdomain due to developmental milestones progressing over time. Please enter a valid test date or enter scores in a new record form.";
                        }
                        else
                        {
                            errorMessage = "One or more testing dates are not within a 30 day period of the first test date. Testing ranges must be within 30 days of testing each subdomain due to developmental milestones progressing over time. Please enter a valid date or enter scores in a new record form.";
                        }
                    }
                    var popup = new CustomPopupView(new CustomPopUpDetails() { Header = "Invalid Test Date", Message = errorMessage, Height = 228, Width = 450 });
                    popup.BindingContext = vM;
                    popup.CloseWhenBackgroundIsClicked = false;
                    await PopupNavigation.Instance.PushAsync(popup);
                    return;
                }
                vM.IsOverviewChanged = true;
                vM.AcademicTestSessionRecords.Where(p => !string.IsNullOrEmpty(p.DomainCode)).FirstOrDefault(p => p.DomainCode == DomainCode).TestDate = date.SelectedDate;
                date.SelectedDate = string.Empty; //This will trigger the SelectedDate again.
            }
        }
        private void VM_DatePicketSelectedEvent(object sender, TestSessionModel e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                /*this.DomainCode = e.DomainCode;
                var success = DateTime.TryParse(e.TestDate, out DateTime dateTime);
                if (success) //on inital load value will be mm/dd/yyyy
                {                   
                    DOBpicker.Date = dateTime;
                }
                else
                {
                    DOBpicker.Date = DateTime.Now;
                }
                DOBpicker.ShowDatePicker();*/

                this.DomainCode = e.DomainCode;
                //DOBpicker.IsShowPicker = true;
                var success = DateTime.TryParse(e.TestDate, out DateTime dateTime);
                if (success) //on inital load value will be mm/dd/yyyy
                {
                    if (vM.AcademicTestSessionRecords != null)
                    {
                        var selectedTestRecord = vM.AcademicTestSessionRecords.FirstOrDefault(p => p.DomainCode == e.DomainCode);
                        if (selectedTestRecord != null)
                        {
                            var maxDateItem = vM.AcademicTestSessionRecords.Where(p => p.IsDateVisible).OrderByDescending(p => p.DateOfTest).FirstOrDefault();
                            var mindateItem = vM.AcademicTestSessionRecords.Where(p => p.IsDateVisible).OrderBy(p => p.DateOfTest).FirstOrDefault();
                            var dataservice = DependencyService.Get<ICommonDataService>();
                            var dobSpliteDate = dataservice.DOB.Split('/');
                            DateTime dobDateTime = new DateTime(Convert.ToInt32(dobSpliteDate[2]), Convert.ToInt32(dobSpliteDate[0]), Convert.ToInt32(dobSpliteDate[1]));
                            if (maxDateItem != null && mindateItem != null)
                            {
                                var dateDiff = maxDateItem.DateOfTest - mindateItem.DateOfTest;
                                if (dateDiff.TotalDays == 0)
                                {
                                    var mindate = mindateItem.DateOfTest.AddDays(-30);
                                    if (mindate < dobDateTime.Date)
                                    {
                                        vM.MinimumDate = dobDateTime.Date;
                                    }
                                    else
                                    {
                                        vM.MinimumDate = mindate;
                                    }
                                    var maxDate = maxDateItem.DateOfTest.AddDays(30);
                                    if (maxDate > DateTime.Now.Date)
                                    {
                                        vM.MaximumDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        vM.MaximumDate = maxDate;
                                    }
                                }
                                else if (dateDiff.TotalDays == 30)
                                {
                                    vM.MinimumDate = mindateItem.DateOfTest;
                                    vM.MaximumDate = maxDateItem.DateOfTest;
                                }
                                else
                                {
                                    var diffDays = 30 - dateDiff.TotalDays;
                                    var mindate = mindateItem.DateOfTest.AddDays(-diffDays);
                                    if (mindate < dobDateTime.Date)
                                    {
                                        vM.MinimumDate = dobDateTime.Date;
                                    }
                                    else
                                    {
                                        vM.MinimumDate = mindate;
                                    }
                                    var maxDate = maxDateItem.DateOfTest.AddDays(diffDays);
                                    if (maxDate > DateTime.Now.Date)
                                    {
                                        vM.MaximumDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        vM.MaximumDate = maxDate.Date;
                                    }
                                }
                            }
                        }
                    }
                    DOBpicker.Date = dateTime;
                    DOBpicker.ManualMaxDate = true;
                    DOBpicker.ManualMinDate = true;
                    DOBpicker.MaximumDate = vM.MaximumDate;
                    DOBpicker.MinimumDate = vM.MinimumDate;
                }
                else
                {
                    DOBpicker.Date = DateTime.Now;
                }

                if (Device.RuntimePlatform == Device.UWP)
                {
                    DOBpicker.ShowDatePicker();
                }
                else
                {
                    DOBpicker.Focus();
                }
            });
        }
        private async void ProgramLabel_Tapped(object sender, EventArgs e)
        {
            if (vM.IsProgramLabelClicked || vM.IsAcademicBasicReport)
                return;

            vM.IsProgramLabelClicked = true;
            await PopupNavigation.Instance.PushAsync(new AddProgramLabelPopupView(vM));

        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DOBpicker.IsShowPicker = false;
            DOBpicker.IsVisible = false;
            DOBpicker.Unfocus();
            await vM.SaveTestSessionOverView();
            await PopupNavigation.Instance.PopAsync();
        }
        private void DOBpicker_DateSelected(object sender, DateChangedEventArgs e)
        {
        }
        private void RawScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            vM.IsOverviewChanged = true;
        }

        private void Timer_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var entxt = (sender as Entry).Text;
                var bindingContext = ((sender as Entry).BindingContext as TestSessionModel).ContentCategoryId == 162;
                if (!bindingContext)
                {
                    return;
                }
                if (!string.IsNullOrEmpty(entxt))
                {
                    vM.TimerErrorMessage = entxt.Length > 0 ? false : true;

                    if (entxt.Length == 4)
                    {
                        DateTime dt;
                        double totalSeconds = 0;
                        if (DateTime.TryParseExact(entxt, "m:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        {
                            var splitTime = entxt.Split(':');
                            if (splitTime.Length == 2)
                            {
                                vM.TimerErrorMessage = false;
                                vM.AcademicTestSessionRecords.Where(p => p.Domain.ToLower() == "fluency" || p.ContentCategoryId == 162).FirstOrDefault().ShowTimerErrorMessage = false;
                                var timesapan = new TimeSpan(0, Convert.ToInt32(splitTime[0]), Convert.ToInt32(splitTime[1]));
                                if (timesapan.TotalSeconds > 210)
                                {
                                    vM.TimerErrorMessage = true;
                                    vM.AcademicTestSessionRecords.Where(p => p.Domain.ToLower() == "fluency" || p.ContentCategoryId == 162).FirstOrDefault().ShowTimerErrorMessage = true;
                                }
                                else
                                {
                                    vM.TimerErrorMessage = false;
                                    vM.AcademicTestSessionRecords.Where(p => p.Domain.ToLower() == "fluency" || p.ContentCategoryId == 162).FirstOrDefault().ShowTimerErrorMessage = false;
                                    vM.AcademicTestSessionRecords.Where(p => p.Domain.ToLower() == "fluency" || p.ContentCategoryId == 162).FirstOrDefault().Timer = timesapan;
                                }
                            }
                            else
                            {
                                vM.TimerErrorMessage = true;
                                vM.AcademicTestSessionRecords.Where(p => p.Domain.ToLower() == "fluency" || p.ContentCategoryId == 162).FirstOrDefault().ShowTimerErrorMessage = true;
                            }
                        }
                        else
                        {
                            vM.TimerErrorMessage = true;
                            vM.AcademicTestSessionRecords.Where(p => p.Domain.ToLower() == "fluency" || p.ContentCategoryId == 162).FirstOrDefault().ShowTimerErrorMessage = true;
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void Timer_Unfocused(object sender, FocusEventArgs e)
        {
            var text = (sender as Entry).Text;
            if (string.IsNullOrEmpty(text))
            {
                vM.TimerErrorMessage = true;
                return;
            }

            if (!text.Contains(":"))
            {
                if (Convert.ToInt32(text) <= 30)
                {
                    (sender as Entry).Text = "0:30";
                }
                else if(Convert.ToInt32(text) <= 210)
                {

                    TimeSpan t = TimeSpan.FromSeconds(Convert.ToInt32(text));
                    string convertedtime = t.ToString(@"m\:ss");
                    (sender as Entry).Text = convertedtime;
                }
                else
                {
                    vM.TimerErrorMessage = true;
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DOBpicker.PropertyChanged -= DatePicker_DOB_PropertyChanged;
            vM.DatePickerSelectedEvent -= VM_DatePicketSelectedEvent;
            DOBpicker.PropertyChanged += DatePicker_DOB_PropertyChanged;
            vM.DatePickerSelectedEvent += VM_DatePicketSelectedEvent;
        }
        protected override void OnDisappearing()
        {

            DOBpicker.PropertyChanged -= DatePicker_DOB_PropertyChanged;
            vM.DatePickerSelectedEvent -= VM_DatePicketSelectedEvent;
            base.OnDisappearing();

        }
    }
}