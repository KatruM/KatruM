using Acr.UserDialogs;
using BDI3Mobile.Models;
using BDI3Mobile.Views.AddChildViews;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using BDI3Mobile.IServices;
using BDI3Mobile.Common;
using BDI3Mobile.Models.DBModels;
using Newtonsoft.Json;
using BDI3Mobile.LookUps.ChildPageLookUps;
using BDI3Mobile.Models.Requests;
using BDI3Mobile.ViewModels.AdministrationViewModels;
using BDI3Mobile.Views;

//The class has been distributed as Partial class across multiple files to make it more readable.
//Implementations of the three pages have been split into separate files. 
//Commands and Initializations happen in this file. 
namespace BDI3Mobile.ViewModels
{
    public partial class AddChildViewModel : BindableObject
    {
        private bool cover = false;
        private readonly IStudentsService _studentService;
        private readonly IProductResearchCodesService _productResearchCodesService;
        private readonly IProductResearchCodeValuesService _productResearchCodesValuesService;
        private readonly ILocationService _locationService;
        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();

        private double parentEmailFontSize;
        public double ParentEmailFontSize
        {
            get
            {
                return parentEmailFontSize;
            }
            set
            {
                parentEmailFontSize = value;
                OnPropertyChanged(nameof(ParentEmailFontSize));
            }
        }

        public bool Cover
        {
            get => cover;
            set
            {
                cover = value;
                OnPropertyChanged(nameof(Cover));
            }
        }

        private bool dOBIsInvalid, enrollmentDateIsInvalid, iFSPInitialDateIsValid, iFSPExitDateIsValid, iEPInitialDateIsValid, iEPExitDateIsValid,
        isFirstNameEmpty, isLastNameEmpty, isGenderEmpty, isDOBEmpty, isEnrollmentEmpty, IgnoreDOB;
        public bool IsFirstNameEmpty
        {
            get => isFirstNameEmpty;
            set
            {
                isFirstNameEmpty = value;
                OnPropertyChanged(nameof(IsFirstNameEmpty));
            }
        }
        public bool IsLastNameEmpty
        {
            get => isLastNameEmpty;
            set
            {
                isLastNameEmpty = value;
                OnPropertyChanged(nameof(IsLastNameEmpty));
            }
        }

        public bool IsGenderEmpty
        {
            get => isGenderEmpty;
            set
            {
                isGenderEmpty = value;
                OnPropertyChanged(nameof(IsGenderEmpty));
            }
        }
        public bool IsDOBEmpty
        {
            get => isDOBEmpty;
            set
            {
                isDOBEmpty = value;
                OnPropertyChanged(nameof(IsDOBEmpty));
            }
        }


        public bool IsEnrollmentEmpty
        {
            get => isEnrollmentEmpty;
            set
            {
                isEnrollmentEmpty = value;
                OnPropertyChanged(nameof(IsEnrollmentEmpty));
            }
        }
        public bool DOBIsInvalid
        {
            get => dOBIsInvalid;
            set
            {
                dOBIsInvalid = value;
                OnPropertyChanged(nameof(DOBIsInvalid));
            }
        }
        public bool EnrollmentDateIsInvalid
        {
            get => enrollmentDateIsInvalid;
            set
            {
                enrollmentDateIsInvalid = value;
                OnPropertyChanged(nameof(EnrollmentDateIsInvalid));
            }
        }
        public bool IFSPInitialDateIsValid
        {
            get => iFSPInitialDateIsValid;
            set
            {
                iFSPInitialDateIsValid = value;
                OnPropertyChanged("IFSPInitialDateIsValid");
            }
        }
        public bool IFSPExitDateIsValid
        {
            get => iFSPExitDateIsValid;
            set
            {
                iFSPExitDateIsValid = value;
                OnPropertyChanged("IFSPExitDateIsValid");
            }
        }
        public bool IEPInitialDateIsValid
        {
            get => iEPInitialDateIsValid;
            set
            {
                iEPInitialDateIsValid = value;
                OnPropertyChanged("IEPInitialDateIsValid");
            }
        }
        public bool IEPExitDateIsValid
        {
            get => iEPExitDateIsValid;
            set
            {
                iEPExitDateIsValid = value;
                OnPropertyChanged("IEPExitDateIsValid");
            }
        }
        private DateTime minimumDate;
        public DateTime MinimumDate
        {
            get
            {
                return minimumDate;
            }
            set
            {
                minimumDate = value;
                OnPropertyChanged(nameof(MinimumDate));
            }
        }
        #region Commands
        public ICommand AddForceCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAsync();
                    await SaveChild(true);
                });
            }
        }
        public ICommand SelectLocationCommand => new Command(() =>
        {
            ShowLocation = !ShowLocation;
            Cover = ShowLocation;
        });
        public ICommand CancelPopup
        {
            get
            {
                return new Command(async () =>
                {
                    await PopupNavigation.Instance.PopAsync();
                });
            }
        }
        public ICommand SelectGenderCommand => new Command(SelectGender);
        public ICommand GenderSelectionCommand => new Command(GenderSelected);
        public ICommand RaceItemTappedCommand { get; set; }
        public ICommand ShowRaceCommand => new Command(SelectRace);

        public ICommand ShowSourcesCommand => new Command(SelectFundingSources);

        public ICommand ShowLanguagesCommand => new Command(SelectLanguage);

        public ICommand ShowPrimaryDiagnosesCommand => new Command(SelectPrimaryDiagnoses);

        public ICommand ShowSecondaryDiagnosesCommand => new Command(SelectSecondaryDiagnoses);

        public ICommand UnCoverCommand => new Command(UnCover);

        public ICommand SaveChildCommand { get; set; }
        public ICommand ShowEthencityCommand => new Command(SelectEthencity);

        public ICommand BackCommand => new Command(async () =>
        {
            if (!IsPageSaved)
            {
                bool primary = false;
                if ((OldChildValue.Diagnoses == null || OldChildValue.Diagnoses.Count == 0) && (PrimaryDiagnostics != null && PrimaryDiagnostics.Any(p => p.selected)))
                {
                    primary = true;
                }
                else if ((OldChildValue.Diagnoses != null && OldChildValue.Diagnoses.Count > 0) && (PrimaryDiagnostics != null && !PrimaryDiagnostics.Any(p => p.selected)))
                {
                    primary = true;
                }
                else if (PrimaryDiagnostics != null && PrimaryDiagnostics.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.Diagnoses)
                    {
                        if (PrimaryDiagnostics.Where(p => p.selected).ToList().Contains(item))
                            primary = false;
                        else
                        {
                            primary = true;
                            break;
                        }
                    }
                }

                bool secondary = false;
                if ((OldChildValue.SecondaryDiagnoses == null || OldChildValue.SecondaryDiagnoses.Count == 0) && (SecondaryDiagnostics != null && SecondaryDiagnostics.Any(p => p.selected)))
                {
                    secondary = true;
                }
                else if ((OldChildValue.SecondaryDiagnoses != null && OldChildValue.SecondaryDiagnoses.Count > 0) && (SecondaryDiagnostics != null && !SecondaryDiagnostics.Any(p => p.selected)))
                {
                    secondary = true;
                }
                else if (SecondaryDiagnostics != null && SecondaryDiagnostics.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.SecondaryDiagnoses)
                    {
                        if (SecondaryDiagnostics.Where(p => p.selected).ToList().Contains(item))
                            secondary = false;
                        else
                        {
                            secondary = true;
                            break;
                        }
                    }
                }

                bool ethencities = false;
                if ((OldChildValue.Ethnicity == null || OldChildValue.Ethnicity.Count == 0) && (Ethencities != null && Ethencities.Any(p => p.selected)))
                {
                    ethencities = true;
                }
                else if ((OldChildValue.Ethnicity != null && OldChildValue.Ethnicity.Count > 0) && (Ethencities != null && !Ethencities.Any(p => p.selected)))
                {
                    ethencities = true;
                }
                else if (Ethencities != null && Ethencities.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.Ethnicity)
                    {
                        if (Ethencities.Where(p => p.selected).ToList().Contains(item))
                            ethencities = false;
                        else
                        {
                            ethencities = true;
                            break;
                        }
                    }
                }

                bool fundingSources = false;
                if ((OldChildValue.FundingSources == null || OldChildValue.FundingSources.Count == 0) && (FundingSources != null && FundingSources.Any(p => p.selected)))
                {
                    fundingSources = true;
                }
                else if ((OldChildValue.FundingSources != null && OldChildValue.FundingSources.Count > 0) && (FundingSources != null && !FundingSources.Any(p => p.selected)))
                {
                    fundingSources = true;
                }
                else if (FundingSources != null && FundingSources.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.FundingSources)
                    {
                        if (FundingSources.Where(p => p.selected).ToList().Contains(item))
                            fundingSources = false;
                        else
                        {
                            fundingSources = true;
                            break;
                        }
                    }
                }

                bool genders = false;
                if ((OldChildValue.Gender == null || OldChildValue.Gender.Count == 0) && (Genders != null && Genders.Any(p => p.selected)))
                {
                    genders = true;
                }
                else if ((OldChildValue.Gender != null && OldChildValue.Gender.Count > 0) && (Genders != null && !Genders.Any(p => p.selected)))
                {
                    genders = true;
                }
                else if (Genders != null && Genders.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.Gender)
                    {
                        if (Genders.Where(p => p.selected).ToList().Contains(item))
                            genders = false;
                        else
                        {
                            genders = true;
                            break;
                        }
                    }
                }

                bool languages = false;
                if ((OldChildValue.Language == null || OldChildValue.Language.Count == 0) && (Languages != null && Languages.Any(p => p.selected)))
                {
                    languages = true;
                }
                else if ((OldChildValue.Language != null && OldChildValue.Language.Count > 0) && (Languages != null && !Languages.Any(p => p.selected)))
                {
                    languages = true;
                }
                else if (Languages != null && Languages.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.Language)
                    {
                        if (Languages.Where(p => p.selected).ToList().Contains(item))
                            languages = false;
                        else
                        {
                            languages = true;
                            break;
                        }
                    }
                }

                bool races = false;
                if ((OldChildValue.Race == null || OldChildValue.Race.Count == 0) && (Races != null && Races.Any(p => p.selected)))
                {
                    races = true;
                }
                else if ((OldChildValue.Race != null && OldChildValue.Race.Count > 0) && (Races != null && !Races.Any(p => p.selected)))
                {
                    races = true;
                }
                else if (Races != null && Races.Where(p => p.selected).ToList().Count > 0)
                {
                    foreach (var item in OldChildValue.Race)
                    {
                        if (Races.Where(p => p.selected).ToList().Contains(item))
                            races = false;
                        else
                        {
                            races = true;
                            break;
                        }
                    }
                }
                bool researchCodes = false;
                if (ResearchCodes != null && ResearchCodes.Count >= 0)
                {
                    if (OldChildValue.ResearchCodes != null && OldChildValue.ResearchCodes.Count > 0)
                    {
                        foreach (var item in OldChildValue.ResearchCodes)
                        {
                            if ((ResearchCodes.Where(x => x.ProductID == item.ProductID && x.ResearchCodeValueId == item.ResearchCodeValueId && x.ResearchCodeId == item.ResearchCodeId && x.value == item.value && x.ResearchCode.ValueName == item.ResearchCode.ValueName).ToList().Count) > 0)
                            {
                                researchCodes = false;
                            }
                            else
                            {
                                researchCodes = true;
                                break;
                            }
                        }
                    }
                }

                var isLocationSelected = OldChildValue.Location.Where(l => l.value != (SelectedLocation?.LocationId ?? 0)).ToList().Any();
                var year1900 = new DateTime(1900, 1, 1);
                var year1900Str = year1900.Month + "/" + year1900.Day + "/" + year1900.Year;
                var year0001 = new DateTime(0001, 1, 1);
                var year0001Str = year0001.Month + "/" + year0001.Day + "/" + year0001.Year;
                var defautlDate = default(DateTime);
                var defaultDatestr = defautlDate.Month + "/" + defautlDate.Day + "/" + defautlDate.Year;


                var cond = (OldChildValue.IEPInitialDate != (IEPinitialdateofEligibility.HasValue ? (((IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year).Equals(defaultDatestr) || (IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year).Equals(year1900Str) || (IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year).Equals(year0001Str) || (IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year).Equals(year0001Str)) ? null : (IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year)) : null)) ||
                    (OldChildValue.IEPExitDate != (IEPExitdate.HasValue ? (((IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year).Equals(defaultDatestr) || (IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year).Equals(year1900Str) || (IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year).Equals(year0001Str) || (IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year).Equals(year0001Str)) ? null : (IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year)) : null)) ||
                    (OldChildValue.IFSPInitialDate != (IFSPinitialdateofEligibility.HasValue ? (((IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year).Equals(defaultDatestr) || (IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year).Equals(year1900Str) || (IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year).Equals(year0001Str) || (IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year).Equals(year0001Str)) ? null : (IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year)) : null)) ||
                    (OldChildValue.IFSPExitDate != (IFSPExitdate.HasValue ? (((IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year).Equals(defaultDatestr) || (IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year).Equals(year1900Str) || (IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year).Equals(year0001Str) || (IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year).Equals(year0001Str)) ? null : (IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year)) : null));

                var enrollCond = (OldChildValue.EnrollmentDate != (EnrollDate.HasValue ?
                    ((EnrollDate.Value.Month + "/" + EnrollDate.Value.Day + "/" + EnrollDate.Value.Year).Equals(year1900Str)) ? null :
                     ((EnrollDate.Value.Month + "/" + EnrollDate.Value.Day + "/" + EnrollDate.Value.Year).Equals(year0001Str) ? null :
                         (EnrollDate.Value.Month + "/" + EnrollDate.Value.Day + "/" + EnrollDate.Value.Year)) : null));

                var dobCond = (OldChildValue.DOB != (((DateofBirth.Month + "/" + DateofBirth.Day + "/" + DateofBirth.Year).Equals(year1900)) ? null : ((DateofBirth.Month + "/" + DateofBirth.Day + "/" + DateofBirth.Year).Contains(year0001Str) ? null : (DateofBirth.Month + "/" + DateofBirth.Day + "/" + DateofBirth.Year))));

                var childIdchanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.ChildId) && !string.IsNullOrEmpty(ChildID)) || (!string.IsNullOrEmpty(OldChildValue.ChildId) && string.IsNullOrEmpty(ChildID)))
                {
                    childIdchanged = true;
                }
                var firstNamechanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.FirstName) && !string.IsNullOrEmpty(FirstName)) || (!string.IsNullOrEmpty(OldChildValue.FirstName) && string.IsNullOrEmpty(FirstName)))
                {
                    firstNamechanged = true;
                }
                var lastNamechanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.LastName) && !string.IsNullOrEmpty(LastName)) || (!string.IsNullOrEmpty(OldChildValue.LastName) && string.IsNullOrEmpty(LastName)))
                {
                    lastNamechanged = true;
                }
                var middleNamechanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.MiddleName) && !string.IsNullOrEmpty(MiddleName)) || (!string.IsNullOrEmpty(OldChildValue.MiddleName) && string.IsNullOrEmpty(MiddleName)))
                {
                    middleNamechanged = true;
                }
                var parent1Namechanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.Parent1Name) && !string.IsNullOrEmpty(Parent1Name)) || (!string.IsNullOrEmpty(OldChildValue.Parent1Name) && string.IsNullOrEmpty(Parent1Name)))
                {
                    parent1Namechanged = true;
                }
                var parent2Namechanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.Parent2Name) && !string.IsNullOrEmpty(Parent2Name)) || (!string.IsNullOrEmpty(OldChildValue.Parent2Name) && string.IsNullOrEmpty(Parent2Name)))
                {
                    parent2Namechanged = true;
                }
                var parent1emailchanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.Parent1Email) && !string.IsNullOrEmpty(Parent1Email)) || (!string.IsNullOrEmpty(OldChildValue.Parent1Email) && string.IsNullOrEmpty(Parent1Email)))
                {
                    parent1emailchanged = true;
                }
                var parent2emailchanged = false;
                if ((string.IsNullOrEmpty(OldChildValue.Parent2Email) && !string.IsNullOrEmpty(Parent2Email)) || (!string.IsNullOrEmpty(OldChildValue.Parent2Email) && string.IsNullOrEmpty(Parent2Email)))
                {
                    parent2emailchanged = true;
                }


                if (childIdchanged || OldChildValue.ForceSave != false || firstNamechanged || lastNamechanged || middleNamechanged || parent1Namechanged || parent2Namechanged ||
                    parent1emailchanged || parent2emailchanged || (OldChildValue.IFSP != (IFSPSelected ? 1 : 0)) || (OldChildValue.IEP != (IEPSelected ? 1 : 0)) ||
                    (secondary) || (Convert.ToInt32(OldChildValue.FreeLunch) != (IsFreeReducedLunch ? 1 : 0)) ||
                    (primary) || (ethencities) || (fundingSources) || (genders) || (languages) || (races) || (researchCodes) || (isLocationSelected) || dobCond
                    || enrollCond || cond)
                {
                    var popUpNavigationInstance = PopupNavigation.Instance;
                    await popUpNavigationInstance.PushAsync(new SaveErrorView(this));
                    if (val)
                    {
                        ReloadAction.Invoke();
                        await Application.Current.MainPage.Navigation.PopModalAsync();
                        return;
                    }
                    else
                    {
                        return;
                    }   
                }
            }
            ReloadAction.Invoke();
            Races = null; FundingSources = null; PrimaryDiagnostics = null; SecondaryDiagnostics = null; Languages = null; Locations = null; ResearchCodes = null;
            commonDataService.ClearAddChildContent?.Invoke();
            Application.Current.MainPage = new DashboardHomeView();
        });
        public ICommand ReloadCommand { get; set; }
        public Action ReloadAction { get; set; }
        public Action ReloadDemographicView { get; set; }
        public Child OldChildValue { get; set; }
        public bool IsPageSaved { get; set; }
        #endregion


        public AddChildViewModel(int offlineStudentid)
        {
            OfflineStudentID = offlineStudentid;
            ParentEmailFontSize = 12;
            _locationService = DependencyService.Get<ILocationService>();
            _studentService = DependencyService.Get<IStudentsService>();
            _productResearchCodesService = DependencyService.Get<IProductResearchCodesService>();
            _productResearchCodesValuesService = DependencyService.Get<IProductResearchCodeValuesService>();
            initialize();
            MessagingCenter.Subscribe<ItemGroup>(this, "LocationSelected", (arg1) =>
            {
                Cover = false;
                if (SelectedLocation != null && SelectedLocation.LocationId == arg1.TreeViewId)
                {
                    LocationName = "";
                    SelectedLocation = null;
                }
                else
                {
                    LocationName = arg1.Name;
                    SelectedLocation = new Location() { LocationId = arg1.TreeViewId, LocationName = arg1.Name };
                }
                ShowLocation = false;
            });
            SaveChildCommand = new Command(async () =>
            {
                IsFirstNameEmpty = IsLastNameEmpty = IsGenderEmpty = IsDOBEmpty = IsLocationEmpty = false;
                if (string.IsNullOrEmpty(DateofBirth.ToString()) || string.IsNullOrEmpty(Gender) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName) || SelectedLocation == null || string.IsNullOrEmpty(SelectedLocation.LocationName))
                {
                    if (string.IsNullOrEmpty(FirstName))
                        IsFirstNameEmpty = true;
                    if (string.IsNullOrEmpty(LastName))
                        IsLastNameEmpty = true;
                    if (string.IsNullOrEmpty(Gender))
                        IsGenderEmpty = true;
                    if (string.IsNullOrEmpty(DateofBirth.ToString()) || IgnoreDOB == false)
                        IsDOBEmpty = true;
                    if (SelectedLocation == null || string.IsNullOrEmpty(SelectedLocation.LocationName))
                        IsLocationEmpty = true;
                    MessagingCenter.Send<string>("Error", "Error");

                    return;
                }
                await SaveChild();
            });
            ReloadCommand = new Command(async () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.ShowLoading("loading...", MaskType.Black);
                });
                GetLocations();
                if (OfflineStudentID == 0)
                {
                    Gender = "";
                    FirstName = null;
                    LocationName = "";
                    LastName = "";
                    MiddleName = "";
                    Parent1Name = "";
                    Parent2Name = "";
                    Parent1Email = "";
                    Parent2Email = "";
                    ChildID = "";
                    RaceCount = 0;
                    DisplayRace = "";
                    EthencityStr = "";
                    Language = "";
                    DisplaySource = "";
                    SourceCount = 0;
                    PrimaryDiagnoses = "";
                    SecondaryDiagnoses = "";
                    EnrollDate = DateTime.Now;
                    DateofBirth = DateTime.Now;

                    IFSPSelected = false;
                    IEPSelected = false;
                    Races.All(p =>
                    {
                        p.selected = false;
                        return true;
                    });
                    PrimaryDiagnostics.All(p => p.selected = false);
                    SecondaryDiagnostics.All(p => p.selected = false);
                    Languages.All(p => p.selected = false);

                    foreach (var item in PrimaryDiagnostics)
                    {
                        item.selected = false;
                    }
                    foreach (var item in SecondaryDiagnostics)
                    {
                        item.selected = false;
                    }
                    
                    Ethencities.All(p => p.selected = false);
                    FundingSources.All(p =>
                    {
                        p.selected = false;
                        return true;
                    });
                    foreach(var item in Languages)
                    {
                        item.selected = false;
                    }
                    Genders.ForEach(p => p.selected = false);
                    IsFreeReducedLunch = false;
                    IsFirstNameEmpty = false;
                    IsLastNameEmpty = false;
                    IsGenderEmpty = false;
                    IsDOBEmpty = false;
                    IsLocationEmpty = false;
                    IFSPInitialDateIsValid = false;
                    IFSPExitDateIsValid = false;
                    IEPInitialDateIsValid = false;
                    IEPExitDateIsValid = false;
                    MessagingCenter.Send<string>("ClearDateErrorText", "ClearErrorText1");
                    MessagingCenter.Send<string>("ClearDateErrorText", "ClearErrorText2");
                    ReloadAction?.Invoke();
                    ReloadDemographicView?.Invoke();
                    LoadUserIdentifyFields(OfflineStudentID);
                }
                else if (OfflineStudentID > 0)
                {
                    LoadPageForEdit();

                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    UserDialogs.Instance.HideLoading();
                });

            });
            if (OfflineStudentID > 0)
            {
                LoadPageForEdit();
            }
            if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            {
                MinimumDate = DateTime.SpecifyKind(new DateTime(1919, 01, 01), DateTimeKind.Unspecified);
            }

            OldChild();
        }

        private void ClearIFSPandIEP()
        {
            PrimaryDiagnoses = "";
            SecondaryDiagnoses = "";

            foreach (var item in PrimaryDiagnostics)
            {
                item.selected = false;
            }
            foreach (var item in SecondaryDiagnostics)
            {
                item.selected = false;
            }
        }
        public void OldChild()
        {
            try
            {
                var year1900 = new DateTime(1900, 1, 1);
                var year1900Str = year1900.Month + "/" + year1900.Day + "/" + year1900.Year;
                var year0001 = new DateTime(0001, 1, 1);
                var year0001Str = year0001.Month + "/" + year0001.Day + "/" + year0001.Year;
                var defautlDate = default(DateTime);
                var defaultDatestr = defautlDate.Month + "/" + defautlDate.Day + "/" + defautlDate.Year;

                OldChildValue = new Child
                {
                    ChildId = ChildID,
                    ForceSave = false,
                    FirstName = FirstName,
                    LastName = LastName,
                    MiddleName = MiddleName
                };
                var locationResponse = new LocationResponseModel
                {
                    value = SelectedLocation?.LocationId ?? 0
                };
                OldChildValue.Location = new List<LocationResponseModel> { locationResponse };
                OldChildValue.Parent1Name = Parent1Name;
                OldChildValue.Parent2Name = Parent2Name;
                OldChildValue.Parent1Email = Parent1Email;
                OldChildValue.Parent2Email = Parent2Email;
                OldChildValue.EnrollmentDate = EnrollDate.HasValue ? (EnrollDate.Value.Month + "/" + EnrollDate.Value.Day + "/" + EnrollDate.Value.Year).Equals(defaultDatestr) ? null : (EnrollDate.Value.Month + "/" + EnrollDate.Value.Day + "/" + EnrollDate.Value.Year) : null;
                OldChildValue.DOB = (DateofBirth.Month + "/" + DateofBirth.Day + "/" + DateofBirth.Year);
                OldChildValue.IFSP = IFSPSelected ? 1 : 0;
                OldChildValue.IEP = IEPSelected ? 1 : 0;
                OldChildValue.IEPInitialDate = IEPinitialdateofEligibility.HasValue ? (IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year).Equals(defaultDatestr) ? null : (IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year) : null;
                OldChildValue.IEPExitDate = IEPExitdate.HasValue ? (IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year).Equals(defaultDatestr) ? null : (IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year) : null;
                OldChildValue.IFSPInitialDate = IFSPinitialdateofEligibility.HasValue ? (IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year).Equals(defaultDatestr) ? null : (IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year) : null;
                OldChildValue.IFSPExitDate = IFSPExitdate.HasValue ?(IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year).Equals(defaultDatestr) ? null : (IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year) : null;
                if (SecondaryDiagnostics != null)
                    OldChildValue.SecondaryDiagnoses = SecondaryDiagnostics.Where(p => p.selected).ToList();

                if (PrimaryDiagnostics != null)
                    OldChildValue.Diagnoses = PrimaryDiagnostics.Where(p => p.selected).ToList();

                if (Ethencities != null)
                    OldChildValue.Ethnicity = Ethencities.Where(p => p.selected).ToList();

                if (FundingSources != null)
                    OldChildValue.FundingSources = FundingSources.Where(p => p.selected).ToList();

                if (Genders != null)
                    OldChildValue.Gender = Genders.Where(p => p.selected).ToList();

                if (Languages != null)
                    OldChildValue.Language = Languages.Where(p => p.selected).ToList();

                OldChildValue.FreeLunch = IsFreeReducedLunch ? 1 : 0;

                if (Races != null)
                    OldChildValue.Race = Races.Where(p => p.selected).ToList();

                if (ResearchCodes != null && OldChildValue.ResearchCodes == null)
                    OldChildValue.ResearchCodes = JsonConvert.DeserializeObject<List<ProductResearchCodeValues>>( JsonConvert.SerializeObject(ResearchCodes));
            }
            catch (Exception ex)
            {
                Microsoft.AppCenter.Crashes.Crashes.TrackError(ex);
            }
        }

        public async Task SaveChild(bool isforceSave = false)
        {
            UserDialogs.Instance.ShowLoading("Saving Child Info...");
            var child = new Child
            {
                OfflineStudentId = OfflineStudentID,
                ChildId = ChildID,
                ForceSave = isforceSave,
                FirstName = FirstName,
                LastName = LastName,
                MiddleName = MiddleName
            };
            var locationResponse = new LocationResponseModel { value = SelectedLocation.LocationId };
            child.Location = new List<LocationResponseModel> { locationResponse };
            child.Parent1Name = Parent1Name;
            child.Parent2Name = Parent2Name;
            child.Parent1Email = Parent1Email;
            child.Parent2Email = Parent2Email;
            child.EnrollmentDate = EnrollDate.HasValue ? EnrollDate.Value.Date.Equals(default(DateTime)) ? null : EnrollDate.Value.Month + "/" + EnrollDate.Value.Day + "/" + EnrollDate.Value.Year: null;
            child.DOB = DateofBirth.Month + "/" + DateofBirth.Day + "/" + DateofBirth.Year;
            child.IFSP = IFSPSelected ? 1 : 0;
            child.IEP = IEPSelected ? 1 : 0;
            if (IEPSelected && IEPinitialdateofEligibility.HasValue)
            {
                child.IEPInitialDate = IEPinitialdateofEligibility.Value.Month + "/" + IEPinitialdateofEligibility.Value.Day + "/" + IEPinitialdateofEligibility.Value.Year;
            }
            if (IEPSelected && IEPExitdate.HasValue)
            {
                child.IEPExitDate = IEPExitdate.Value.Month + "/" + IEPExitdate.Value.Day + "/" + IEPExitdate.Value.Year;
            }
            if (IFSPSelected && IFSPinitialdateofEligibility.HasValue)
            {
                child.IFSPInitialDate = IFSPinitialdateofEligibility.Value.Month + "/" + IFSPinitialdateofEligibility.Value.Day + "/" + IFSPinitialdateofEligibility.Value.Year;
            }
            if (IFSPSelected && IFSPExitdate.HasValue)
            {
                child.IFSPExitDate = IFSPExitdate.Value.Month + "/" + IFSPExitdate.Value.Day + "/" + IFSPExitdate.Value.Year;
            }
            child.SecondaryDiagnoses = SecondaryDiagnostics.Where(p => p.selected).ToList();
            child.Diagnoses = PrimaryDiagnostics.Where(p => p.selected).ToList();
            child.Ethnicity = Ethencities.Where(p => p.selected).ToList();
            child.FundingSources = FundingSources.Where(p => p.selected).ToList();
            child.Gender = Genders.Where(p => p.selected).ToList();
            child.Language = Languages.Where(p => p.selected).ToList();
            child.FreeLunch = IsFreeReducedLunch ? 1 : 0;
            child.Race = Races.Where(p => p.selected).ToList();
            child.ResearchCodes = ResearchCodes.ToList();

            if (OfflineStudentID != 0)
            {
                var offlinechild = _studentService.GetStudentById(OfflineStudentID);
                if (offlinechild != null)
                {
                    child.ChildUserID = offlinechild.UserId;
                }
            }
            if (isforceSave)
            {
                child.OfflineStudentId = 0;
                child.ChildUserID = null;
                await SaveChildOffline(child);
            }
            else
            {
                await SaveChildToDB(child);
            }
        }

        public async void SaveChildClicked(string dob, string enrollment)
        {
            IsFirstNameEmpty = IsLastNameEmpty = IsGenderEmpty = IsDOBEmpty = IsEnrollmentEmpty = false;
            IsFirstNameEmpty = string.IsNullOrEmpty(FirstName) || FirstName.Trim().Length == 0;
            IsLastNameEmpty = string.IsNullOrEmpty(LastName) || LastName.Trim().Length == 0;
            IsGenderEmpty = string.IsNullOrEmpty(Gender) || Gender.Trim().Length == 0;
            IsLocationEmpty = SelectedLocation == null || string.IsNullOrEmpty(SelectedLocation.LocationName);
            if (!DOBIsInvalid)
                IsDOBEmpty = (string.IsNullOrEmpty(dob));
            if (!IsDOBEmpty)
            {
                var dobSplit = dob.Split('/');
                var dobDate = new DateTime(Convert.ToInt32(dobSplit[2]), Convert.ToInt32(dobSplit[0]), Convert.ToInt32(dobSplit[1]));
                var dateToCompare = new DateTime(1919, 1, 1);
                var currentDate = DateTime.Today;
                DOBIsInvalid = dobDate < dateToCompare || dobDate > currentDate;
            }
            if (IsFirstNameEmpty || IsLastNameEmpty || IsGenderEmpty || IsDOBEmpty || DOBIsInvalid || EnrollmentDateIsInvalid || IsLocationEmpty)
            {
                MessagingCenter.Send<string>("Error", "Error");
                return;
            }
            var dobSplit1 = dob.Split('/');
            var dobDate1 = new DateTime(Convert.ToInt32(dobSplit1[2]), Convert.ToInt32(dobSplit1[0]), Convert.ToInt32(dobSplit1[1]));
            DateofBirth = dobDate1;
            EnrollDate = null;
            if (!string.IsNullOrEmpty(enrollment))
            {
                var enrollmentSplit1 = enrollment.Split('/');
                var enrollmentDate1 = new DateTime(Convert.ToInt32(enrollmentSplit1[2]), Convert.ToInt32(enrollmentSplit1[0]), Convert.ToInt32(enrollmentSplit1[1]));
                EnrollDate = enrollmentDate1;
            }
            IFSPinitialdateofEligibility = !IFSPinitialdateofEligibility.HasValue ? null : IFSPinitialdateofEligibility;
            IFSPExitdate = !IFSPExitdate.HasValue ? null : IFSPExitdate;
            IEPinitialdateofEligibility = !IEPinitialdateofEligibility.HasValue ? null : IEPinitialdateofEligibility;
            IEPExitdate = !IEPExitdate.HasValue ? null : IEPExitdate;
            await SaveChild();
        }
        public string StudentUserId { get; set; }
        public int OfflineStudentID { get; set; }
        private void LoadPageForEdit()
        {
            Students childRecord = null;
            var students = _studentService.GetStudentById(OfflineStudentID);
            if (students != null)
            {
                childRecord = students;
                StudentUserId = students.UserId;
            }

            if (childRecord == null) return;
            ChildID = childRecord.ChildID;
            FirstName = childRecord.FirstName;
            LastName = childRecord.LastName;
            MiddleName = childRecord.MiddleName;
            Parent1Name = childRecord.ParentGuardian1;
            Parent2Name = childRecord.ParentGuardian2;
            Parent1Email = (childRecord.ParentEmailAddress1 == null) ? "" : childRecord.ParentEmailAddress1;
            Parent2Email = (childRecord.ParentEmailAddress2 == null) ? "" : childRecord.ParentEmailAddress2;
            if (childRecord.EnrollmentDate.Year != 0001)
                EnrollDate = childRecord.EnrollmentDate;
            DateofBirth = childRecord.Birthdate;
            IFSPSelected = Convert.ToBoolean(childRecord.IsIFSP);
            IEPSelected = Convert.ToBoolean(childRecord.IsIEP);
            if (childRecord.IEPEligibilityDate.Year != 0001)
                IEPinitialdateofEligibility = childRecord.IEPEligibilityDate;
            if (childRecord.IEPExitDate.Year != 0001)
                IEPExitdate = childRecord.IEPExitDate;
            if (childRecord.IFSPEligibilityDate.Year != 0001)
                IFSPinitialdateofEligibility = childRecord.IFSPEligibilityDate;
            if (childRecord.IFSPExitDate.Year != 0001)
                IFSPExitdate = childRecord.IFSPExitDate;
            IsFreeReducedLunch = Convert.ToBoolean(childRecord.IsFreeLunch);
            foreach (var item in Genders)
            {
                if (item.value == childRecord.Gender)
                {
                    item.selected = true;
                    IsGenderEmpty = false;
                    Gender = item.text;
                }
            }
            var values = new[] 
            { 
                (childRecord.Birthdate.Month < 10 ? "0" + childRecord.Birthdate.Month : childRecord.Birthdate.Month + "") + "/" + (childRecord.Birthdate.Day < 10 ? "0" + childRecord.Birthdate.Day : childRecord.Birthdate.Day + "") + "/" + (childRecord.Birthdate.Year == 1 ? "000" + childRecord.Birthdate.Year : childRecord.Birthdate.Year + ""),
                (childRecord.EnrollmentDate.Month < 10 ? "0" + childRecord.EnrollmentDate.Month : childRecord.EnrollmentDate.Month + "") + "/" + (childRecord.EnrollmentDate.Day < 10 ? "0" + childRecord.EnrollmentDate.Day : childRecord.EnrollmentDate.Day + "") + "/" + (childRecord.EnrollmentDate.Year == 1 ? "000" + childRecord.EnrollmentDate.Year: childRecord.EnrollmentDate.Year + ""),
                (childRecord.IEPEligibilityDate.Month < 10 ? "0" + childRecord.IEPEligibilityDate.Month : childRecord.IEPEligibilityDate.Month + "") + "/" + (childRecord.IEPEligibilityDate.Day < 10 ? "0" + childRecord.IEPEligibilityDate.Day : childRecord.IEPEligibilityDate.Day + "") + "/" +(childRecord.IEPEligibilityDate.Year == 1 ? "000" +   childRecord.IEPEligibilityDate.Year :childRecord.IEPEligibilityDate.Year + ""),
                (childRecord.IEPExitDate.Month < 10 ? "0" + childRecord.IEPExitDate.Month : childRecord.IEPExitDate.Month + "") + "/" + (childRecord.IEPExitDate.Day < 10 ? "0" + childRecord.IEPExitDate.Day : childRecord.IEPExitDate.Day + "") + "/" +(childRecord.IEPExitDate.Year == 1 ? "000" + childRecord.IEPExitDate.Year:childRecord.IEPExitDate.Year + ""),
                (childRecord.IFSPEligibilityDate.Month < 10 ? "0" + childRecord.IFSPEligibilityDate.Month : childRecord.IFSPEligibilityDate.Month + "") + "/" + (childRecord.IFSPEligibilityDate.Day < 10 ? "0" + childRecord.IFSPEligibilityDate.Day : childRecord.IFSPEligibilityDate.Day + "") + "/" +(childRecord.IFSPEligibilityDate.Year == 1 ? "000" + childRecord.IFSPEligibilityDate.Year : childRecord.IFSPEligibilityDate.Year + ""),
                (childRecord.IFSPExitDate.Month < 10 ? "0" + childRecord.IFSPExitDate.Month : childRecord.IFSPExitDate.Month + "") + "/" + (childRecord.IFSPExitDate.Day < 10 ? "0" + childRecord.IFSPExitDate.Day : childRecord.IFSPExitDate.Day + "") + "/" +(childRecord.IFSPExitDate.Year == 1 ? "000" + childRecord.IFSPExitDate.Year : childRecord.IFSPExitDate.Year + ""),
            };

            if (!string.IsNullOrEmpty(childRecord.SelectedEthnictyIds))
            {
                var ids = childRecord.SelectedEthnictyIds.Split(',');
                foreach (var item in ids)
                {
                    var ehtnicity = Ethencities.FirstOrDefault(p => p.value.ToString() == item);
                    if (ehtnicity != null)
                    {
                        ehtnicity.selected = true;
                        EthencityStr = ehtnicity.text;
                    }
                }
            }

            if (!string.IsNullOrEmpty(childRecord.SelectedRaceIds))
            {
                var ids = childRecord.SelectedRaceIds.Split(',');
                foreach (var item in ids)
                {
                    var race = Races.FirstOrDefault(p => p.value.ToString() == item);
                    if (race != null)
                    {
                        race.selected = true;
                    }
                }
            }
            if (Races != null)
            {
                RaceCount = Races.Where(p => p.selected).ToList().Count();
                if (RaceCount > 0) DisplayRace = RaceCount + " Selected";
            }

            if (!string.IsNullOrEmpty(childRecord.SelectedLanguageIds))
            {
                var ids = childRecord.SelectedLanguageIds.Split(',');
                foreach (var item in ids)
                {
                    var language = Languages.FirstOrDefault(p => p.value.ToString() == item);
                    if (language != null)
                    {
                        language.selected = true;
                        Language = language.text;
                    }
                }
            }

            if (!string.IsNullOrEmpty(childRecord.SelectedFundingSourceIds))
            {
                var ids = childRecord.SelectedFundingSourceIds.Split(',');
                foreach (var item in ids)
                {
                    var fundingSource = FundingSources.FirstOrDefault(p => p.value.ToString() == item);
                    if (fundingSource != null)
                    {
                        fundingSource.selected = true;
                    }
                }
            }
            if (FundingSources != null)
            {
                SourceCount = FundingSources.Where(p => p.selected).ToList().Count();
                if (SourceCount > 0) DisplaySource = SourceCount + " Selected";
            }

            if (!string.IsNullOrEmpty(childRecord.SelectedPrimaryDiagnosesIds))
            {
                var ids = childRecord.SelectedPrimaryDiagnosesIds.Split(',');
                foreach (var item in ids)
                {
                    var diagnoses = PrimaryDiagnostics.FirstOrDefault(p => p.value.ToString() == item);
                    if (diagnoses != null)
                    {
                        diagnoses.selected = true;
                        PrimaryDiagnoses = diagnoses.text;
                    }
                }
            }
            if (!string.IsNullOrEmpty(childRecord.SelectedSecondaryDiagnosesIds))
            {
                var ids = childRecord.SelectedSecondaryDiagnosesIds.Split(',');
                foreach (var item in ids)
                {
                    var diagnoses = SecondaryDiagnostics.FirstOrDefault(p => p.value.ToString() == item);
                    if (diagnoses != null)
                    {
                        diagnoses.selected = true;
                        SecondaryDiagnoses = diagnoses.text;
                    }
                }
            }
            if (childRecord.SelectedLocationId != null && AllLocations != null && AllLocations.Any())
            {
                SelectedLocation = AllLocations.FirstOrDefault(p => p.LocationId == childRecord.SelectedLocationId);
                LocationName = SelectedLocation?.LocationName;
                foreach (var item in LocationsObservableCollection)
                {
                    if (item.LocationName == LocationName)
                        item.IsSelected = true;

                    if (item.SubLocations != null)
                    {
                        foreach (var subitem in item.SubLocations)
                        {
                            if (subitem.LocationName == LocationName)
                                subitem.IsSelected = true;
                            if (subitem.SubLocations != null)
                            {
                                foreach (var subitem2 in subitem.SubLocations)
                                {
                                    if (subitem2.LocationName == LocationName)
                                        subitem2.IsSelected = true;
                                    if (subitem2.SubLocations != null)
                                    {
                                        foreach (var subitem3 in subitem2.SubLocations)
                                        {
                                            if (subitem3.LocationName == LocationName)
                                                subitem3.IsSelected = true;
                                            if (subitem3.SubLocations != null)
                                            {
                                                foreach (var subitem4 in subitem3.SubLocations)
                                                {
                                                    if (subitem4.LocationName == LocationName)
                                                        subitem4.IsSelected = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            MessagingCenter.Send<string[]>(values, "EditData");
        }

        private async Task SaveChildToDB(Child child)
        {
            int addedBy;
            bool isSucess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
            int genderValue = 0;
            bool recordFound = false;
            if (!string.IsNullOrEmpty(Gender))
            {
                var first = Genders.ToList()
                    .Where(p =>
                    {
                        if (p == null) throw new ArgumentNullException(nameof(p));
                        return p.text == Gender.ToString();
                    })
                    .FirstOrDefault();

                if (first != null) genderValue = first.value;
                if (genderValue > 0)
                {
                    var records = _studentService.GetStudents().FirstOrDefault(p => p.FirstName == child.FirstName &&
                          p.LastName == child.LastName &&
                          Convert.ToDateTime(p.Birthdate.ToShortDateString()) == Convert.ToDateTime(child.DOB)
                          && p.OfflineStudentID != child.OfflineStudentId && p.DownloadedBy == addedBy);
                    if (records != null)
                    {
                        recordFound = true;
                    }
                }
            }
            UserDialogs.Instance.HideLoading();
            if (recordFound)
            {
                await ShowDuplicateChildOffline(child);
            }
            else
            {
                await SaveChildOffline(child);
            }
        }
        private async Task ShowDuplicateChildOffline(Child child)
        {
            string userFirstName = "", userLastName = "";
            if (Application.Current.Properties.ContainsKey("FirstName") && Application.Current.Properties["FirstName"] != null)
            {
                userFirstName = Application.Current.Properties["FirstName"].ToString();
            }
            if (Application.Current.Properties.ContainsKey("LastName") && Application.Current.Properties["LastName"] != null)
            {
                userLastName = Application.Current.Properties["LastName"].ToString();
            }

            var errorMessage1 = string.Format(ErrorMessages.DuplicateChildMessage1, child.FirstName, child.LastName, child.DOB, userFirstName, userLastName);
            var errorMessage2 = string.Format(ErrorMessages.DuplicateChildMessage2, userFirstName, userLastName);
            var errorMessages = new List<string>()
                {
                    errorMessage1,
                    errorMessage2
                };
            ValidationMessages = errorMessages;
            var popUpNavigationInstance = PopupNavigation.Instance;
            var popup = new DuplicateChildView();
            popup.BindingContext = this;
            await popUpNavigationInstance.PushAsync(popup);
        }
        private async Task SaveChildOffline(Child child)
        {
            int addedBy;
            if (Application.Current.Properties.ContainsKey("UserID"))
            {
                bool isSucess = int.TryParse(Application.Current.Properties["UserID"].ToString(), out addedBy);
                if (isSucess)
                {
                    int gender = 0;
                    var selectedLanguageIds = "";
                    if (Languages != null && Languages.Any())
                    {
                        var selectedLanguage = Languages.Where(p => p.selected);
                        var enumerable = selectedLanguage as Language[] ?? selectedLanguage.ToArray();
                        if (enumerable.Any())
                        {
                            selectedLanguageIds = string.Join(",", enumerable.Select(p => p.value));
                        }
                    }
                    if (Genders != null && Genders.Any())
                    {
                        var selectedGender = Genders.FirstOrDefault(p => p.selected);
                        if (selectedGender != null)
                        {
                            gender = selectedGender.value;
                        }
                    }

                    var selectedFundingSourceIds = "";
                    if (FundingSources != null && FundingSources.Any())
                    {
                        var selected = FundingSources.Where(p => p.selected);
                        if (selected.Any())
                        {
                            selectedFundingSourceIds = string.Join(",", selected.Select(p => p.value));
                        }
                    }

                    var selectedEthnicitiIds = "";
                    if (Ethencities != null && Ethencities.Any())
                    {
                        var selected = Ethencities.Where(p => p.selected);
                        if (selected.Any())
                        {
                            selectedEthnicitiIds = string.Join(",", selected.Select(p => p.value));
                        }
                    }

                    var selectedRaceIds = "";
                    if (Races != null && Races.Any())
                    {
                        var selected = Races.Where(p => p.selected);
                        if (selected.Any())
                        {
                            selectedRaceIds = string.Join(",", selected.Select(p => p.value));
                        }
                    }

                    var selectePrimaryDiagnosesIds = "";
                    if (PrimaryDiagnostics != null && PrimaryDiagnostics.Any())
                    {
                        var selected = PrimaryDiagnostics.Where(p => p.selected);
                        if (selected.Any())
                        {
                            selectePrimaryDiagnosesIds = string.Join(",", selected.Select(p => p.value));
                        }
                    }

                    var selectedSecondaryDiagnosesIds = "";
                    if (SecondaryDiagnostics != null && SecondaryDiagnostics.Any())
                    {
                        var selected = SecondaryDiagnostics.Where(p => p.selected);
                        if (selected.Any())
                        {
                            selectedSecondaryDiagnosesIds = string.Join(",", selected.Select(p => p.value));
                        }
                    }
                    var newChildRecord = new Students()
                    {
                        DownloadedBy = addedBy,
                        UserId = child.ChildUserID != null ? child.ChildUserID.ToString() : null,
                        OfflineStudentID = child.OfflineStudentId,
                        SelectedEthnictyIds = selectedEthnicitiIds,
                        SelectedFundingSourceIds = selectedFundingSourceIds,
                        SelectedLanguageIds = selectedLanguageIds,
                        SelectedPrimaryDiagnosesIds = selectePrimaryDiagnosesIds,
                        SelectedRaceIds = selectedRaceIds,
                        SelectedSecondaryDiagnosesIds = selectedSecondaryDiagnosesIds,
                        ChildID = child.ChildId,
                        FirstName = child.FirstName,
                        LastName = child.LastName,
                        MiddleName = child.MiddleName,
                        Gender = gender,
                        Birthdate = Convert.ToDateTime(child.DOB),
                        EnrollmentDate = Convert.ToDateTime(child.EnrollmentDate),
                        ParentGuardian1 = child.Parent1Name,
                        ParentGuardian2 = child.Parent2Name,
                        ParentEmailAddress1 = child.Parent1Email,
                        ParentEmailAddress2 = child.Parent2Email,
                        IsIFSP = Convert.ToByte(child.IFSP),
                        IsIEP = Convert.ToByte(child.IEP),
                        AddedBy = addedBy,
                        IEPEligibilityDate = Convert.ToDateTime(child.IEPInitialDate),
                        IEPExitDate = Convert.ToDateTime(child.IEPExitDate),
                        IFSPEligibilityDate = Convert.ToDateTime(child.IFSPInitialDate),
                        IFSPExitDate = Convert.ToDateTime(child.IFSPExitDate),
                        IsFreeLunch = Convert.ToByte(child.FreeLunch),
                        IsSynced = false,
                        SelectedLocationId = SelectedLocation.LocationId
                    };
                    try
                    {
                        newChildRecord.updatedOn = DateTime.Now.ToUniversalTime().ToString();
                        newChildRecord.updatedOnUTC = DateTime.Now.ToUniversalTime().ToString();
                        _studentService.InsertorUpdate(newChildRecord);
                        OfflineStudentID = newChildRecord.OfflineStudentID;
                        if (newChildRecord.OfflineStudentID > 0)
                        {
                            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());

                            var researchCodes = ResearchCodes.Where(p => p.ResearchCode != null).Select(p => p.ResearchCode);
                            if (researchCodes != null && researchCodes.Any())
                            {
                                _productResearchCodesService.DeleteAll(orgId);
                                var lstCodes = new List<ProductResearchCodes>();
                                foreach (var item in researchCodes)
                                {
                                    var code = new ProductResearchCodes();
                                    code.ResearchCodeId = item.ResearchCodeId;
                                    code.ValueName = item.ValueName;
                                    code.OrganizationId = orgId;
                                    code.ProductID = item.ProductID;
                                    lstCodes.Add(code);
                                }
                                _productResearchCodesService.InsertAll(lstCodes);
                            }
                            if (ResearchCodes != null && ResearchCodes.Any())
                            {
                                _productResearchCodesValuesService.DeleteByStudentId(newChildRecord.OfflineStudentID);
                                var lstCodesvalues = new List<ProductResearchCodeValues>();
                                foreach (var item in ResearchCodes)
                                {
                                    var code = new ProductResearchCodeValues();
                                    code.ResearchCodeId = item.ResearchCodeId;
                                    code.OrganizationId = orgId;
                                    code.OfflineStudentID = newChildRecord.OfflineStudentID;
                                    code.value = item.value;
                                    code.ProductID = item.ProductID;
                                    lstCodesvalues.Add(code);
                                }
                                _productResearchCodesValuesService.InsertAll(lstCodesvalues);
                            }
                        }
                        child.OfflineStudentId = OfflineStudentID;
                        UserDialogs.Instance.HideLoading();
                        Races = null; FundingSources = null; PrimaryDiagnostics = null; SecondaryDiagnostics = null; Languages = null; Locations = null; ResearchCodes = null;
                        commonDataService.ClearAddChildContent?.Invoke();
                        Application.Current.MainPage = new ChildInformationpageView(OfflineStudentID);
                    }
                    catch (Exception ex)
                    {
                        await UserDialogs.Instance.AlertAsync(new AlertConfig() { Message = "Save Failed", OkText = "Ok", Title = "Failure" });
                    }

                    IsPageSaved = true;
                }
                else
                {
                    IsPageSaved = false;
                    await UserDialogs.Instance.AlertAsync(new AlertConfig() { Message = "Save Failed", OkText = "Ok", Title = "Failure" });
                }
            }
            else
            {
                IsPageSaved = false;
                await UserDialogs.Instance.AlertAsync(new AlertConfig() { Message = "Save Failed", OkText = "Ok", Title = "Failure" });
            }
        }
        private void LoadUserIdentifyFields(int offlinestudentId)
        {
            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
            var codes = _productResearchCodesService.GetResearchCodesByOrg(orgId);
            var codevalues = _productResearchCodesValuesService.GetProductResearchCodes(offlinestudentId);
            ResearchCodes = new List<ProductResearchCodeValues>();
            if (codes != null && codes.Any())
            {
                foreach (var item in codes)
                {
                    var researchcode = new ProductResearchCodeValues();
                    researchcode.OrganizationId = orgId;
                    researchcode.ResearchCodeId = item.ResearchCodeId;
                    researchcode.ProductID = item.ProductID;
                    researchcode.ResearchCode = new ProductResearchCodes();
                    researchcode.ResearchCode.ResearchCodeId = item.ResearchCodeId;
                    researchcode.ResearchCode.ValueName = item.ValueName;
                    researchcode.ResearchCode.OrganizationId = orgId;
                    researchcode.ResearchCode.ProductID = item.ProductID;
                    if (codevalues != null && codevalues.Any())
                    {
                        var codevalue = codevalues.FirstOrDefault(p => p.ResearchCodeId == item.ResearchCodeId);
                        if (codevalue != null)
                        {
                            researchcode.ResearchCodeId = codevalue.ResearchCodeId;
                            researchcode.value = codevalue.value;
                            researchcode.ResearchCodeValueId = codevalue.ResearchCodeValueId;
                            researchcode.ProductID = codevalue.ProductID;

                        }
                    }
                    ResearchCodes.Add(researchcode);
                }
            }
            OnPropertyChanged(nameof(ResearchCodes));
        }
        public List<string> ValidationMessages { get; set; }
        private async void initialize()
        {
            Genders = GenderLookUp.GetGenders();
            Races = new List<Race>(commonDataService.Races);
            PrimaryDiagnostics = new List<Diagnostics>(commonDataService.PrimaryDiagnostics);
            SecondaryDiagnostics = new List<Diagnostics>(commonDataService.SecondaryDiagnostics);
            Languages = new List<Language>(commonDataService.Languages);
            FundingSources = new List<FundingSource>(commonDataService.fundingSources);
            Ethencities = EthencityLookUp.GetEthencities();
            LoadUserIdentifyFields(OfflineStudentID);
            GetLocations();
        }
        void UnCover(object obj)
        {
            ShowGender = false;
            ShowLocations = false;
            ShowRaces = false;
            ShowFundingSources = false;
            ShowLanguages = false;
            ShowPrimaryDiagnoses = false;
            ShowSecondaryDiagnoses = false;
            ShowEthencity = false;
            ShowLocation = false;
            Cover = false;
        }


        #region LocationTree
        private bool isLocationEmpty;
        public bool IsLocationEmpty
        {
            get
            {
                return isLocationEmpty;
            }
            set
            {
                isLocationEmpty = value;
                OnPropertyChanged(nameof(IsLocationEmpty));
            }
        }
        private bool _showLocation;
        public bool ShowLocation
        {
            get
            {
                return _showLocation;
            }
            set
            {
                _showLocation = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowLocation));
            }
        }

        private bool _isTabStop = true;

        public bool IsTabStop
        {
            get
            {
                return _isTabStop;
            }
            set
            {
                _isTabStop = value;
                OnPropertyChanged(nameof(IsTabStop));

                MessagingCenter.Send<String, bool>("Tab", "Tab", IsTabStop);

            }
        }
        private string _locationName;
        public string LocationName
        {
            get { return _locationName; }
            set { _locationName = value; OnPropertyChanged(nameof(LocationName)); }
        }
        private Location selectedLocation;
        public Location SelectedLocation
        {
            get
            {
                return selectedLocation;
            }
            set
            {
                if (value != null && !string.IsNullOrEmpty(value.LocationName)) { IsLocationEmpty = false; }
                selectedLocation = value;
                OnPropertyChanged(nameof(SelectedLocation));

            }
        }
        private List<Location> AllLocations;
        private List<Location> locations;
        private bool val;

        public List<Location> Locations
        {
            get
            {
                return locations;
            }
            set
            {
                locations = value;
                OnPropertyChanged(nameof(Locations));

            }
        }

        private ObservableCollection<LocationNew> locationsObservableCollection;
        public ObservableCollection<LocationNew> LocationsObservableCollection
        {
            get
            {
                return locationsObservableCollection;
            }
            set
            {
                locationsObservableCollection = value;
                OnPropertyChanged(nameof(LocationsObservableCollection));

            }
        }
        public void GetLocations()
        {
            var locations = _locationService.GetLocationTree();
            if (locations != null && locations.Any())
            {
                AllLocations = locations.ToList();
                Locations = new List<Location>();
                LocationsObservableCollection = new ObservableCollection<LocationNew>();
                var parentLocation = locations.Where(p => p.ParentLocationId == 0).ToList();
                if (parentLocation != null && parentLocation.Any())
                {
                    foreach (var item in parentLocation)
                    {
                        var location = new Location();
                        var locationnew = new LocationNew();

                        location.LocationId = item.LocationId;
                        location.LocationName = item.LocationName;
                        location.IsSelected = item.IsSelected;
                        location.IsEnabled = item.IsEnabled;
                        location.ParentLocationId = item.ParentLocationId;
                        Locations.Add(location);



                        locationnew.LocationId = item.LocationId;
                        locationnew.LocationName = item.LocationName;
                        locationnew.IsSelected = item.IsSelected;
                        locationnew.IsEnabled = item.IsEnabled;
                        locationnew.ParentLocationId = item.ParentLocationId;
                        LocationsObservableCollection.Add(locationnew);
                        locations.Remove(item);
                        GenerateSubLocation(locationnew, locations);
                    }
                }
            }
        }
        private void GenerateSubLocation(LocationNew location, List<Location> lstLocation)
        {
            var subLocation = lstLocation.Where(p => p.ParentLocationId == location.LocationId);
            if (subLocation != null && subLocation.Any())
            {
                location.SubLocations = new List<LocationNew>();
                foreach (var item in subLocation)
                {
                    item.Level = location.Level + 1;
                    var locationnew = new LocationNew();
                    locationnew.LocationId = item.LocationId;
                    locationnew.LocationName = item.LocationName;
                    locationnew.IsSelected = item.IsSelected;
                    locationnew.IsEnabled = item.IsEnabled;
                    locationnew.ParentLocationId = item.ParentLocationId;
                    locationnew.Level = item.Level;
                    location.SubLocations.Add(locationnew);
                    location.HasSubLocations = true;
                    GenerateSubLocation(locationnew, lstLocation);
                }
            }
        }

        #endregion
    }
}
