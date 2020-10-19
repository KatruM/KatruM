using BDI3Mobile.Models;
using BDI3Mobile.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels
{
    public partial class AddChildViewModel : BindableObject
    {
        private bool showFundingSources, showLangauges, showPrimaryDiagnoses, showSecondaryDiagnoses, showEthencity;
        private int raceCount, sourceCount;
        private string displayRace, displaySource, primaryDiagnoses, secondayDiagnoses;
        private List<Race> races;
        private string language;
        private bool ifspSelected = false, iepSelected = false;
        private string ifspswitchText = "No";
        private string iepswitchText = "No";
        private string freeReducedLunchText = "No";

        private bool isFreeReducedLunch;
        public bool IsFreeReducedLunch
        {
            get
            {
                return isFreeReducedLunch;
            }
            set
            {
                isFreeReducedLunch = value;
                if (isFreeReducedLunch)
                    FreeReducedLunchText = "Yes";
                else
                    FreeReducedLunchText = "No";
                OnPropertyChanged(nameof(IsFreeReducedLunch));
            }
        }

        private bool showDiagnoses = false;
        public bool ShowFundingSources
        {
            get { return showFundingSources; }
            set
            {
                showFundingSources = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowFundingSources));
            }
        }
        public bool ShowPrimaryDiagnoses
        {
            get { return showPrimaryDiagnoses; }
            set
            {
                showPrimaryDiagnoses = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowPrimaryDiagnoses));
            }
        }



        public bool ShowEthencity
        {
            get { return showEthencity; }
            set
            {
                showEthencity = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowEthencity));
            }
        }

        public bool ShowSecondaryDiagnoses
        {
            get { return showSecondaryDiagnoses; }
            set
            {
                showSecondaryDiagnoses = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowSecondaryDiagnoses));
            }
        }
        public bool ShowDiagnoses
        {
            get { return showDiagnoses; }
            set
            {
                showDiagnoses = value;
                OnPropertyChanged(nameof(ShowDiagnoses));
            }
        }
        public bool IFSPSelected
        {
            get { return ifspSelected; }
            set
            {
                ifspSelected = value;

                if (!ifspSelected && !iepSelected)
                {
                    ClearIFSPandIEP();
                }

                if (ifspSelected || iepSelected)
                {
                    ShowDiagnoses = true;
                }
                else
                {
                    ShowDiagnoses = false;
                }

                if (ifspSelected)
                    IFSPSwitchText = "Yes";
                else
                {
                    IFSPSwitchText = "No";
                    IFSPExitDateIsValid = IFSPInitialDateIsValid = false;
                    MessagingCenter.Send<string>("ClearDateErrorText", "ClearErrorText1");
                }


                OnPropertyChanged(nameof(IFSPSelected));
            }
        }
        public bool IEPSelected
        {
            get { return iepSelected; }
            set
            {
                iepSelected = value;

                if (!ifspSelected && !iepSelected)
                {
                    ClearIFSPandIEP();
                }

                if (ifspSelected || iepSelected)
                {
                    ShowDiagnoses = true;
                }
                else
                {
                    ShowDiagnoses = false;
                }

                if (iepSelected)
                    IEPSwitchText = "Yes";
                else
                {
                    IEPSwitchText = "No";
                    IEPExitDateIsValid = IEPInitialDateIsValid = false;
                    MessagingCenter.Send<string>("ClearDateErrorText", "ClearErrorText2");
                }
                OnPropertyChanged(nameof(IEPSelected));
            }
        }

        public string IFSPSwitchText
        {
            get { return ifspswitchText; }
            set
            {
                ifspswitchText = value;
                OnPropertyChanged(nameof(IFSPSwitchText));
            }
        }
        public string IEPSwitchText
        {
            get { return iepswitchText; }
            set
            {
                iepswitchText = value;
                OnPropertyChanged(nameof(IEPSwitchText));
            }
        }

        public string FreeReducedLunchText
        {
            get { return freeReducedLunchText; }
            set
            {
                freeReducedLunchText = value;
                OnPropertyChanged(nameof(FreeReducedLunchText));
            }
        }


        public bool ShowLanguages
        {
            get { return showLangauges; }
            set
            {
                showLangauges = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowLanguages));
            }
        }
        public string Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged(nameof(Language));
            }
        }
        public List<Language> Languages { get; set; }

        private List<Diagnostics> primaryDiagnostics;
        public List<Diagnostics> PrimaryDiagnostics
        {
            get { return primaryDiagnostics; }

            set
            {
                primaryDiagnostics = value;
                OnPropertyChanged(nameof(PrimaryDiagnostics));
            }
        }
        private List<Diagnostics> secondaryDiagnostics;
        public List<Diagnostics> SecondaryDiagnostics
        {
            get { return secondaryDiagnostics; }

            set
            {
                secondaryDiagnostics = value;
                OnPropertyChanged(nameof(SecondaryDiagnostics));
            }
        }

        private List<FundingSource> fundingSources;
        public List<FundingSource> FundingSources
        {
            get { return fundingSources; }
            set
            {
                fundingSources = value;
                OnPropertyChanged(nameof(FundingSources));
            }
        }

        private string ethencityStr;
        public string EthencityStr
        {
            get
            {
                return ethencityStr;
            }
            set
            {
                ethencityStr = value;
                OnPropertyChanged(nameof(EthencityStr));
            }
        }
        private List<Ethencity> ethencities;
        public List<Ethencity> Ethencities
        {
            get { return ethencities; }
            set
            {
                ethencities = value;
                OnPropertyChanged(nameof(Ethencities));
            }
        }

        private List<ProductResearchCodeValues> researchCodes;
        public List<ProductResearchCodeValues> ResearchCodes
        {
            get
            {
                return researchCodes;
            }
            set
            {
                researchCodes = value;
                OnPropertyChanged(nameof(ResearchCodes));
            }
        }

        public List<Race> Races
        {
            get { return races; }
            set
            {
                races = value;
                OnPropertyChanged(nameof(Races));
            }
        }

        public bool ShowRaces
        {
            get { return showRaces; }
            set
            {
                showRaces = value;
                IsTabStop = !value;
                OnPropertyChanged(nameof(ShowRaces));
            }
        }
        public int RaceCount
        {
            get { return raceCount; }
            set
            {
                raceCount = value;
                OnPropertyChanged(nameof(RaceCount));
            }
        }
        public int SourceCount
        {
            get { return sourceCount; }
            set
            {
                sourceCount = value;
                OnPropertyChanged(nameof(SourceCount));
            }
        }
        public string DisplayRace
        {
            get { return displayRace; }
            set
            {
                displayRace = value;
                OnPropertyChanged(nameof(DisplayRace));
            }
        }
        public string DisplaySource
        {
            get { return displaySource; }
            set
            {
                displaySource = value;
                OnPropertyChanged(nameof(DisplaySource));
            }
        }
        public Diagnostics SecondaryDiagnostic { get; set; }

        public string PrimaryDiagnoses
        {
            get { return primaryDiagnoses; }
            set
            {
                primaryDiagnoses = value;
                OnPropertyChanged(nameof(PrimaryDiagnoses));
            }
        }

        public string SecondaryDiagnoses
        {
            get => secondayDiagnoses;
            set
            {
                secondayDiagnoses = value;
                OnPropertyChanged(nameof(SecondaryDiagnoses));
            }
        }

        public DateTime? iFSPinitialdateofEligibility;
        public DateTime? IFSPinitialdateofEligibility
        {
            get
            {
                return iFSPinitialdateofEligibility;
            }
            set
            {
                iFSPinitialdateofEligibility = value;
                OnPropertyChanged(nameof(IFSPinitialdateofEligibility));
            }
        }

        public DateTime? iFSPexitdate;
        public DateTime? IFSPExitdate
        {
            get
            {
                return iFSPexitdate;
            }
            set
            {
                iFSPexitdate = value;
                OnPropertyChanged(nameof(IFSPExitdate));
            }
        }
        public DateTime? iEPinitialdateofEligibility;
        public DateTime? IEPinitialdateofEligibility
        {
            get
            {
                return iEPinitialdateofEligibility;
            }
            set
            {
                iEPinitialdateofEligibility = value;
                OnPropertyChanged(nameof(IEPinitialdateofEligibility));
            }
        }

        public DateTime? iEPexitdate;
        public DateTime? IEPExitdate
        {
            get
            {
                return iEPexitdate;
            }
            set
            {
                iEPexitdate = value;
                OnPropertyChanged(nameof(IEPExitdate));
            }
        }






        void SelectRace(object obj)
        {
            Cover = true;
            ShowRaces = true;

        }

        public void RaceItemTapped(object obj)
        {
            (obj as Race).selected = !(obj as Race).selected;
            int count = 0;
            foreach (var race in Races)
            {
                if (race.selected)
                {
                    count++;
                    DisplayRace = count > 0 ? count + " Selected" : "";
                }
            }
            if (count == 0)
            {
                DisplayRace = "";
            }
            RaceCount = count;
        }

        void SelectFundingSources(object obj)
        {
            Cover = true;
            ShowFundingSources = true;

        }

        void SelectEthencity(object obj)
        {
            Cover = true;
            ShowEthencity = true;
        }

        public void FundingSourceTapped(object obj)
        {
            (obj as FundingSource).selected = !(obj as FundingSource).selected;
            int count = 0;
            foreach (var source in FundingSources)
            {
                if (source.selected)
                {
                    count++;
                    DisplaySource = count > 0 ? count + " Selected" : "";
                }
            }
            if (count == 0)
                DisplaySource = "";

            SourceCount = count;
        }
        void SelectLanguage(object obj)
        {

            Cover = true;
            ShowLanguages = true;

        }

        public void LanguageSelected(object obj)
        {
            var language = obj as Language;
            foreach (var item in Languages)
            {
                if (item.value == language.value)
                {
                    // Bug Fix: CLINICAL 4055
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        Language = "";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        Language = language.text;
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
        }
        void SelectPrimaryDiagnoses(object obj)
        {
            Cover = true;
            ShowPrimaryDiagnoses = true;

        }
        void SelectSecondaryDiagnoses(object obj)
        {
            Cover = true;
            ShowSecondaryDiagnoses = true;

        }

        public void PrimarDiagnosesTapped(object obj)
        {
            var diagnostics = obj as Diagnostics;
            PrimaryDiagnoses = diagnostics.text;
            foreach (var item in PrimaryDiagnostics)
            {
                if (item.value == diagnostics.value)
                {
                    // Bug Fix: CLINICAL 4055
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        PrimaryDiagnoses = "";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        PrimaryDiagnoses = diagnostics.text.ToString();
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
        }

        public void SecondaryDiagnosesTapped(object obj)
        {
            var diagnostics = obj as Diagnostics;
            SecondaryDiagnoses = diagnostics.text;
            foreach (var item in SecondaryDiagnostics)
            {
                if (item.value == diagnostics.value)
                {
                    // Bug Fix: CLINICAL 4055
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        SecondaryDiagnoses = "";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        SecondaryDiagnoses = diagnostics.text.ToString();
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
        }
        public void EthencitySelected(object obj)
        {
            var ethencity = obj as Ethencity;
            foreach (var item in Ethencities)
            {
                if (item.value == ethencity.value)
                {
                    // Bug Fix: CLINICAL 4055
                    if (item.selected)
                    {
                        item.selected = !item.selected;
                        EthencityStr = "";
                    }
                    else
                    {
                        item.selected = !item.selected;
                        EthencityStr = ethencity.text.ToString();
                    }
                }
                else
                {
                    item.selected = false;
                }
            }
        }
    }
}
