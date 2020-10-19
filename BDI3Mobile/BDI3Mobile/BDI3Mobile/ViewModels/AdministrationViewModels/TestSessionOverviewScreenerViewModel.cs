using System;
using System.Linq;
using System.Windows.Input;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AdministrationViewModels
{
    public class TestSessionOverviewScreenerViewModel : TestSessionOverViewModel
    {
        private bool isExaminerPopupOpen { get; set; }
        private bool isBasicReport;
        public bool IsBasicReport
        {
            get
            {
                return isBasicReport;
            }
            set
            {
                isBasicReport = value;
                OnPropertyChanged(nameof(IsBasicReport));
            }
        }
        
        public string DOB { get; set; }

        public TestSessionOverviewScreenerViewModel(string age = null, bool isReport = false) : base()
        {
            if (age != null)
                DOB = age;
            if (isReport)
            {
                IsBasicReport = IsScreenerBasicReport = isReport;
                Title = "BDI-3 Developmental Screener Basic Report ";
                IsDateVisible = false;
            }
            else
            {
                Title = "Test Session Overview";
                IsDateVisible = true;
            }
            if (IsScreenerBasicReport)
            {
                if (ProgramLabel == "Select a program label")
                    ProgramLabel = string.Empty;
            }
        }
        public new ICommand SelectExaminerCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    
                    if (PopupNavigation.Instance.PopupStack.Count > 1)
                    {
                        foreach (var popup in PopupNavigation.Instance.PopupStack.ToList())
                        {
                            await PopupNavigation.Instance.PopAsync();
                            if (popup is SelectExaminerPopupView && (popup as SelectExaminerPopupView).Title == "ExaminerPopUp")
                            {
                                return;
                            }
                        }
                    }
                    isExaminerPopupOpen = true;

                    var item = (e as TestSessionModel);
                    SelectedItemDomainCode = item.DomainCode;
                    await PopupNavigation.Instance.PushAsync(new SelectExaminerPopupView(this, new Thickness(0, 0, 0, 0)) { BindingContext = this, Title = "ExaminerPopUp" });
                    isExaminerPopupOpen = false;

                    //var selectExaminerPopupView = new SelectExaminerPopupView(this);
                    //if (!selectExaminerPopupView.isPopupViewOpen)
                    //{
                   
                    //selectExaminerPopupView.isPopupViewOpen = true;
                    //}
                    //else
                    //    PopupNavigation.Instance.PopAsync();
                });
            }
        }
        public ICommand LoadDomainBasedQuestionsCommand
        {
            get
            {
                return new Command(async(e) =>
                {
                    var item = (e as TestSessionModel);
                    var navigationHeaader = new ItemNavigationHeaderData();
                    var selectedDomain = Records.Where(p => !string.IsNullOrEmpty(p.DomainCode)).FirstOrDefault(p => p.DomainCode == item.DomainCode);
                    navigationHeaader.HeaderColor = Color.FromHex("#5c2d91");
                    if (selectedDomain != null)
                    {
                        if (selectedDomain.ParentDomainCode.ToUpper() == "ADP")
                        {
                            navigationHeaader.HeaderColor = Color.FromHex("#D73648");
                        }
                        else if (selectedDomain.ParentDomainCode.ToUpper() == "MOT")
                        {
                            navigationHeaader.HeaderColor = Color.FromHex("#0066AD");
                        }
                        else if (selectedDomain.ParentDomainCode.ToUpper() == "COM")
                        {
                            navigationHeaader.HeaderColor = Color.FromHex("#5C2D91");
                        }
                        else if (selectedDomain.ParentDomainCode.ToUpper() == "SE")
                        {
                            navigationHeaader.HeaderColor = Color.FromHex("#008550");
                        }
                        else if (selectedDomain.ParentDomainCode.ToUpper() == "COG")
                        {
                            navigationHeaader.HeaderColor = Color.FromHex("#CC4B00");
                        }
                        else if (selectedDomain.ParentDomainCode == "Academic")
                        {
                            // TODO : Need to check for the Academic Domain Because at this time it is not coming from the API 06/01/2020
                            navigationHeaader.HeaderColor = Color.FromHex("#BFD730");
                        }
                    }
                    navigationHeaader.Title = selectedDomain.Domain + " (" + item.DomainCode + ")";
                    if (selectedDomain != null)
                    {
                        navigationHeaader.ContentCategoryId = selectedDomain.ContentCategoryId;
                        navigationHeaader.TestDate = selectedDomain.TestDate;
                        navigationHeaader.Title = selectedDomain.Domain + " (" + item.DomainCode + ")";
                        navigationHeaader.TestDate = selectedDomain.TestDate;
                    }
                    await SaveTestSessionOverView();
                    MessagingCenter.Send<ItemNavigationHeaderData>(navigationHeaader, "Administrationpage");
                });
            }
        }
        public override void UpdateSelectedExaminer(string selectedText)
        {
            IsOverviewChanged = true;
            Records.Where(p => p.DomainCode == SelectedItemDomainCode).FirstOrDefault().Examiner = selectedText;
            Records.Where(p => p.DomainCode == SelectedItemDomainCode).FirstOrDefault().ExaminerID = Convert.ToInt32(ExaminerList.FirstOrDefault(p => p.text == selectedText)?.id);
        }
    }
}

