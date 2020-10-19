using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.BL;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AdministrationViewModels
{
    public class ResetScoresViewModel : BaseViewModel
    {
        private string resetText = "Select the subdomains below to clear the item level scores and set the test back to default state. You make select as many subdomains as you like.";
        public string ResetText
        {
            get
            {
                return resetText;
            }
            set
            {
                resetText = value;
                OnPropertyChanged(nameof(ResetText));
            }
        }
        private bool isScreenerForm;
        public bool IsScreenerForm
        {
            get
            {
                return isScreenerForm;
            }
            set
            {
                isScreenerForm = value;
                OnPropertyChanged(nameof(IsScreenerForm));
            }
        }
        private bool isCompleteForm;
        public bool IsCompleteForm
        {
            get
            {
                return isCompleteForm;
            }
            set
            {
                isCompleteForm = value;
                OnPropertyChanged(nameof(IsCompleteForm));
            }
        }

        private bool isAcademicForm;
        public bool IsAcademicForm
        {
            get
            {
                return isAcademicForm;
            }
            set
            {
                isAcademicForm = value;
                OnPropertyChanged(nameof(IsAcademicForm));
            }
        }

        public Action<List<int>> ResetAction { get; set; }
        public ICommand ResetScoresClickCommand { get; set; }
        public ICommand ResetScoresInAlertClickCommand { get; set; }
        public ICommand CancelButtonInAlertClickCommand { get; set; }

        public readonly IContentCategoryService _contentcategoryservice;

        private bool _showResetAlertMessagePopup;
        public bool ShowResetAlertMessagePopup
        {
            get => _showResetAlertMessagePopup;
            set => SetAndRaisePropertyChanged(ref _showResetAlertMessagePopup, value);
        }

        private List<ResetScoreGroup> _resetScoreListGroup = new List<ResetScoreGroup>();
        public List<ResetScoreGroup> ResetScoreListGroup
        {
            get
            {
                return _resetScoreListGroup;
            }
            set
            {
                _resetScoreListGroup = value;
                OnPropertyChanged(nameof(ResetScoreListGroup));
            }
        }

        private List<ResetScoreGroup> _acdemicResetScoreListGroup = new List<ResetScoreGroup>();
        public List<ResetScoreGroup> AcademicResetScoreList
        {
            get
            {
                return _acdemicResetScoreListGroup;
            }
            set
            {
                _acdemicResetScoreListGroup = value;
                OnPropertyChanged(nameof(AcademicResetScoreList));
            }
        }

        public ResetScoresViewModel(bool isbatteldevelopment, bool isAcademicForm=false)
        {
            IsCompleteForm = isbatteldevelopment;
            IsScreenerForm = !isbatteldevelopment && !isAcademicForm;
            IsAcademicForm = isAcademicForm;
            
            if (IsScreenerForm)
            {
                ResetText = "Select the domains below to clear the item level scores and set the test back to default state. You make select as many domains as you like.";
            }
            ResetScoresClickCommand = new Command(ExecuteResetScoresClick);
            ResetScoresInAlertClickCommand = new Command(ExecuteResetScoresInAlertClick);
            CancelButtonInAlertClickCommand = new Command(ExecuteCancelButtonInAlertClick);

            _contentcategoryservice = DependencyService.Get<IContentCategoryService>();
            var domains = _contentcategoryservice.GetContentCategory();

            if (IsCompleteForm)
            {
                var parentDomains = from domain in domains
                                    where domain.contentCategoryLevelId == 1
                                    select domain;
                var subDomains = from domain in domains
                                 where domain.contentCategoryLevelId == 2
                                 select domain;
                foreach (var item in parentDomains)
                {
                    ResetScoreListGroup.Add(new
                        ResetScoreGroup(item.name.ToUpper(), subDomains.
                        Where(t => t.parentContentCategoryId == item.contentCategoryId).
                        Select(t => new ResetScore { DomainName = item.name, DomainCode = item.code, SubDomainContentCatgoryId = t.contentCategoryId, SubDomainCode = t.code, SubDomainName = t.name, IsSubDomain = true }).
                        ToList()));
                }
            }
            else if(IsScreenerForm)
            {
                ResetScores = new List<ResetScore>();
                var screenercontentCategories = domains.Where(p => p.contentCategoryLevelId == 4);
                if (screenercontentCategories != null && screenercontentCategories.Any())
                {
                    foreach (var item in screenercontentCategories)
                    {
                        ResetScores.Add(new ResetScore()
                        {
                            DomainCode = item.code,
                            DomainName = item.name,
                            SubDomainContentCatgoryId = item.contentCategoryId,
                        });
                    }
                }
            }
            // Reset Scores - Academic form
            else
            {
                var parentDomains = from domain in domains
                                    where domain.contentCategoryLevelId == 6
                                    select domain;
                var subDomains = from domain in domains
                                 where domain.contentCategoryLevelId == 7 orderby domain.sequenceNo
                                 select domain;
                var areas = from domain in domains
                                 where domain.contentCategoryLevelId == 8
                                 select domain;

                List<ResetScore> areaList1 = new List<ResetScore>();
                
                foreach (var item in areas)
                {
                    if (item.parentContentCategoryId == 164)
                    {
                        areaList1.Add(new ResetScore()
                        {
                            DomainCode = item.code,
                            DomainName = item.name,
                            SubDomainContentCatgoryId = item.contentCategoryId,
                        });
                    }
                }

                List<ResetScore> areaList2 = new List<ResetScore>();

                foreach (var item in areas)
                {
                    if (item.parentContentCategoryId == 182)
                    {
                        areaList2.Add(new ResetScore()
                        {
                            DomainCode = item.code,
                            DomainName = item.name,
                            SubDomainContentCatgoryId = item.contentCategoryId,
                        });
                    }
                }

                foreach (var item in parentDomains)
                {
                    AcademicResetScoreList.Add(new
                        ResetScoreGroup(item.name.ToUpper(), subDomains.
                        Where(t => t.parentContentCategoryId == item.contentCategoryId).
                        Select(t => new ResetScore { DomainName = item.name, DomainCode = item.code, SubDomainContentCatgoryId = t.contentCategoryId, SubDomainCode = t.code, SubDomainName = t.name, IsSubDomain = true, IsAreaListVisible = (t.contentCategoryId == 164 || t.contentCategoryId == 182) ? true : false, ResetScoresAreaList = (t.contentCategoryId == 156 || t.contentCategoryId == 159 || t.contentCategoryId == 162 || t.contentCategoryId == 208 || t.contentCategoryId == 199 || t.contentCategoryId == 204 || t.contentCategoryId == 210) ?  null : (t.contentCategoryId == 164) ? areaList1 : areaList2 }).
                        ToList())); ;
                }

            }
        }
        private List<ResetScore> resetScores;
        public List<ResetScore> ResetScores
        {
            get
            {
                return resetScores;
            }
            set
            {
                resetScores = value;
                OnPropertyChanged(nameof(ResetScores));
            }
        }
        async void ExecuteResetScoresClick()
        {
            ShowResetAlertMessagePopup = true;
        }
        async void ExecuteResetScoresInAlertClick()
        {
            //TODO: Do the logic here on tap of Reset Score Label.
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                await PopupNavigation.Instance.PopAllAsync();
                var lstSelected = new List<int>();
                if (IsCompleteForm)
                {
                    if (ResetScoreListGroup != null && ResetScoreListGroup.Any())
                    {
                        foreach (var item in ResetScoreListGroup)
                        {
                            foreach (var inneritem in item)
                            {
                                if (inneritem.IsSelected)
                                {
                                    lstSelected.Add(inneritem.SubDomainContentCatgoryId);
                                }
                            }
                        }
                    }
                }
                else if(IsScreenerForm)
                {
                    lstSelected.AddRange(ResetScores.Where(p => p.IsSelected).Select(p => p.SubDomainContentCatgoryId).ToList());
                }
                else
                {
                    if (AcademicResetScoreList != null && AcademicResetScoreList.Any())
                    {
                        foreach (var item in AcademicResetScoreList)
                        {
                            foreach (var inneritem in item)
                            {
                                if (inneritem.IsSelected)
                                {
                                    lstSelected.Add(inneritem.SubDomainContentCatgoryId);
                                }
                                if (inneritem.ResetScoresAreaList != null && inneritem.ResetScoresAreaList.Any())
                                {
                                    foreach (var areaList in inneritem.ResetScoresAreaList)
                                    {
                                        if (areaList.IsSelected)
                                        {
                                            lstSelected.Add(areaList.SubDomainContentCatgoryId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (lstSelected != null && lstSelected.Any())
                {
                    ResetAction?.Invoke(lstSelected);
                }
            }
        }
        async void ExecuteCancelButtonInAlertClick()
        {
            ShowResetAlertMessagePopup = false;
        }
    }
}
