using System;
using System.Linq;
using System.Windows.Input;
using BDI3Mobile.Models.Common;
using BDI3Mobile.Views.PopupViews;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;


namespace BDI3Mobile.ViewModels.AcademicSurveyLiteracyViewModel
{
    public class AcademicTestSessionOverviewModel : TestSessionOverViewModel
    {
        private bool isAcademicReport;
        public bool IsAcademicReport
        {
            get
            {
                return isAcademicReport;
            }
            set
            {
                isAcademicReport = value;
                OnPropertyChanged(nameof(IsAcademicReport));
            }
        }

        public string TimerErrorText { get; set; } = "Please enter a value within the valid time range 0:00 – 3:30.";

        private bool timerErrorMessage;
        public bool TimerErrorMessage
        {
            get
            {
                return timerErrorMessage;
            }
            set
            {
                timerErrorMessage = value;
                AcademicTestSessionRecords.Where(p => p.Domain == "Fluency").FirstOrDefault().ShowTimerErrorMessage = value;
                OnPropertyChanged(nameof(TimerErrorMessage));
            }
        }
        private bool isExaminerPopupOpen { get; set; }
        public AcademicTestSessionOverviewModel(bool isReport = false) : base()
        {
            if (isReport)
            {
                IsAcademicReport = IsAcademicBasicReport = true;
                Title = "Battelle Early Academic Survey Basic Report";
                IsDateVisible = false;
            }
            else
            {
                IsAcademicReport = false;
                Title = "Test Session Overview";
                IsDateVisible = true;
            }
            if (IsAcademicBasicReport)
            {
                if (ProgramLabel == "Select a program label")
                    ProgramLabel = string.Empty;
            }
        }

        public override void UpdateSelectedExaminer(string selectedText)
        {
            AcademicTestSessionRecords.Where(p => p.DomainCode == SelectedItemDomainCode).FirstOrDefault().Examiner = selectedText;
            AcademicTestSessionRecords.Where(p => p.DomainCode == SelectedItemDomainCode).FirstOrDefault().ExaminerID = Convert.ToInt32(ExaminerList.FirstOrDefault(p => p.text == selectedText)?.id);
        }

        public new ICommand SelectExaminerCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (isExaminerPopupOpen)
                        return;
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
                    await PopupNavigation.Instance.PushAsync(new SelectExaminerPopupView(this, new Thickness(350, 0, 0, 0)) { BindingContext = this, Title = "ExaminerPopUp" });
                    isExaminerPopupOpen = false;
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
                    var selectedDomain = AcademicTestSessionRecords.Where(p => !string.IsNullOrEmpty(p.DomainCode)).FirstOrDefault(p => p.DomainCode == item.DomainCode);
                    navigationHeaader.HeaderColor = Color.FromHex("#5c2d91");
                    if (selectedDomain != null)
                    {
                        if (selectedDomain.DomainCode == "PA" || selectedDomain.DomainCode == "PWR")
                        {
                            return;
                        }
                        else
                        {
                            if (selectedDomain.ParentDomainCode.ToUpper() == "LIT")
                            {
                                navigationHeaader.HeaderColor = Color.FromHex("#BFD730");
                            }
                            else if (selectedDomain.ParentDomainCode.ToUpper() == "MAT")
                            {
                                navigationHeaader.HeaderColor = Color.FromHex("#BFD730");
                            }
                        }
                    }
                    if (selectedDomain != null && item != null)
                    {
                        if (selectedDomain.Domain != null && item.DomainCode != null)
                        {
                            navigationHeaader.Title = selectedDomain.Domain + " (" + item.DomainCode + ")";
                        }
                    }
                    await SaveTestSessionOverView();
                    MessagingCenter.Send<ItemNavigationHeaderData>(navigationHeaader, "AcademicPage");
                });
            }
        }
    }
    
}
