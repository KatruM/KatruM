using System;
using System.ComponentModel;
using System.Linq;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.Common;
using BDI3Mobile.ViewModels;
using BDI3Mobile.Views.PopupViews;
using ClinicalScoring.BundleParsers;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile.Views.ItemAdministrationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestsessionOverviewView : PopupPage
    {
        public TestSessionOverViewModel vM { get; set; }

        public string DomainCode { get; set; }
        private bool isFocused;

        public TestsessionOverviewView(bool isBasicReport = false)
        {
            InitializeComponent();
            this.CloseWhenBackgroundIsClicked = false;
            vM = new TestSessionOverViewModel(isBasicReport);           
            BindingContext = vM;
           
                //iOS - CollectionView [Entry issue inside ListView]
                if (Device.RuntimePlatform != Device.iOS)
                {
                    TSOList1.IsVisible = true;
                    TSOList1iOS.IsVisible = false;
                }
                else
                {
                    TSOList1iOS.IsVisible = true;
                    TSOList1.IsVisible = false;
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
                var splittedDate = date.SelectedDate.Split('/');
                try
                {
                    DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
                    if (date != null && itemdateTime.Date > DateTime.Now.Date)
                    {
                        hasError = true;
                        errorMessage = "Date entered cannot exceed current date.";
                    }
                    else
                    {
                        if (date != null)
                        {
                            var selectedDate = itemdateTime;
                            if (!(selectedDate.Date <= vM.MaximumDate && selectedDate.Date >= vM.MinimumDate))
                            {
                                hasError = true;
                            }
                        }
                    }
                    if (hasError)
                    {
                        date.SelectedDate = string.Empty; //This will trigger the SelectedDate again.
                        var firstTestRecord = vM.TestSessionRecords.FirstOrDefault(p => p.IsDateVisible);
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
                    vM.TestSessionRecords.Where(p => !string.IsNullOrEmpty(p.DomainCode)).FirstOrDefault(p => p.DomainCode == DomainCode).TestDate = date.SelectedDate;
                    date.SelectedDate = string.Empty; //This will trigger the SelectedDate again.
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                }
                
            }
        }


        private void VM_DatePicketSelectedEvent(object sender, TestSessionModel e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.DomainCode = e.DomainCode;
                //DOBpicker.IsShowPicker = true;
                var splittedDate1 = e.TestDate.Split('/');
                DateTime dateTime = new DateTime(Convert.ToInt32(splittedDate1[2]), Convert.ToInt32(splittedDate1[0]), Convert.ToInt32(splittedDate1[1]));
                if (vM.TestSessionRecords != null)
                {
                    var selectedTestRecord = vM.TestSessionRecords.FirstOrDefault(p => p.DomainCode == e.DomainCode);
                    if (selectedTestRecord != null)
                    {
                        var maxDateItem = vM.TestSessionRecords.Where(p => p.IsDateVisible).OrderByDescending(p => p.DateOfTest).FirstOrDefault();
                        var mindateItem = vM.TestSessionRecords.Where(p => p.IsDateVisible).OrderBy(p => p.DateOfTest).FirstOrDefault();
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
                // DOBpicker.IsVisible = true;
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //DOBpicker.IsShowPicker = false;
            //DOBpicker.IsVisible = false;
            //DOBpicker.Unfocus();
            await vM.SaveTestSessionOverView();
            await PopupNavigation.Instance.PopAsync();
        }

        private async void ProgramLabel_Tapped(object sender, EventArgs e)
        {
            if (vM.IsProgramLabelClicked || vM.IsDevelopmentalBasicReport)
                return;

            vM.IsProgramLabelClicked = true;
            await PopupNavigation.Instance.PushAsync(new AddProgramLabelPopupView(vM));

        }

        private void DOBpicker_DateSelected(object sender, DateChangedEventArgs e)
        {
        }

        private void DOBpicker_Unfocused(object sender, FocusEventArgs e)
        {
            var date = sender as CustomRenderer.MyDatePicker;
        }

        private void RawScore_Unfocused(object sender, FocusEventArgs e)
        {

            if ((sender as Entry).BindingContext != null)
            {
                var testform = (sender as Entry).BindingContext as TestSessionModel;
                if (!string.IsNullOrEmpty(testform.RawScore))
                {
                    var bdi3ScoringParser = Bdi3ScoringParser.GetInstance();
                    var aescore = bdi3ScoringParser.GetAeFromRs(Convert.ToDouble(testform.RawScore), null, testform.Domain);
                    testform.AE = aescore > 0 ? (aescore / 12 > 0 ? aescore / 12 + " yrs, " : "") + aescore % 12 + " mos" : "--";
                    var ss = bdi3ScoringParser.GetSsFromRs(Convert.ToDouble(testform.RawScore), testform.Domain, vM.commonDataService.TotalAgeinMonths);
                    if (ss > ScoringParser.NO_SCORE)
                    {
                        testform.SS = ss.ToString();
                        testform.PR = bdi3ScoringParser.GetPrFromSs((double)ss)?.FirstOrDefault()?.StringValue;
                    }
                    else
                    {
                        testform.SS = "--";
                        testform.PR = "--";
                    }
                }
                else
                {
                    testform.SS = "";
                    testform.PR = "";
                    testform.AE = "";
                    Focus();
                }
            }
        }

        private void RawScore_TextChanged(object sender, TextChangedEventArgs e)
        {
            vM.IsOverviewChanged = true;
        }

        void RawScore_Focused(object sender, FocusEventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DOBpicker.PropertyChanged -= DatePicker_DOB_PropertyChanged;
            DOBpicker.PropertyChanged += DatePicker_DOB_PropertyChanged;
            vM.DatePickerSelectedEvent -= VM_DatePicketSelectedEvent;
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