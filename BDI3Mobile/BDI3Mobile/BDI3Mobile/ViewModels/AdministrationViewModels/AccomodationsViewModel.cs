using System.Threading.Tasks;
using System.Windows.Input;
using BDI3Mobile.Common;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels.DigitalTestRecord;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AdministrationViewModels
{
    public class AccomodationsViewModel : BindableObject
    {
        public ICommand CancelCommand => new AsyncCommand(OnCancelButtonClicked);
        public ICommand SaveCommand => new AsyncCommand(OnSaveButtonClicked);
        private string question1 = "No";
        private string question2 = "No";
        private string primaryQuestion1 = "No";
        private string primaryQuestion2 = "No";
        private string subQuestion1 = "No";
        private string subQuestion2 = "No";

        private readonly ICommonDataService commonDataService = DependencyService.Get<ICommonDataService>();
        private FormParamterClass formParamterClass;
        public FormParamterClass FormParamterClass
        {
            get
            {
                return formParamterClass;
            }
            set
            {
                formParamterClass = value;
                OnPropertyChanged(nameof(FormParamterClass));
            }
        }
        private string notes;
        public string Notes
        {
            get
            {
                return notes;
            }
            set
            {
                notes = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        public AccomodationsViewModel()
        {
            FormParamterClass = JsonConvert.DeserializeObject<FormParamterClass>(commonDataService.StudentTestFormOverview.formParameters);
            if (FormParamterClass != null && FormParamterClass.AllStandard.HasValue)
            {
                Question1Toggled = FormParamterClass.AllStandard.Value;
            }
            if (FormParamterClass != null && FormParamterClass.ValidRepresentation.HasValue)
            {
                Question2Toggled = FormParamterClass.ValidRepresentation.Value;
            }
            if (FormParamterClass != null && FormParamterClass.HasGlasses.HasValue)
            {
                PrimaryQuestion1Toggled = FormParamterClass.HasGlasses.Value;
            }
            if (FormParamterClass != null && FormParamterClass.HasHearingAid.HasValue)
            {
                PrimaryQuestion2Toggled = FormParamterClass.HasHearingAid.Value;
            }
            if (FormParamterClass != null && FormParamterClass.GlassesUsed.HasValue)
            {
                SecondaryQuestion1Toggled = FormParamterClass.GlassesUsed.Value;
            }
            if (FormParamterClass != null && FormParamterClass.HearingAidUsed.HasValue)
            {
                SecondaryQuestion2Toggled = FormParamterClass.HearingAidUsed.Value;
            }
        }

        #region Handling Visibility of Secondary Questions based on Primary Question Toggle/Switch Values
        private bool _secondaryQuestion1Visibility;
        public bool SecondaryQuestion1Visibility
        {
            get { return _secondaryQuestion1Visibility; }
            set
            {
                _secondaryQuestion1Visibility = value;
                OnPropertyChanged(nameof(SecondaryQuestion1Visibility));
            }

        }
        private bool _secondaryQuestion2Visibility;
        public bool SecondaryQuestion2Visibility
        {
            get { return _secondaryQuestion2Visibility; }
            set
            {
                _secondaryQuestion2Visibility = value;
                OnPropertyChanged(nameof(SecondaryQuestion2Visibility));
            }

        }
        #endregion

        #region Switch Controls for Questions
        private bool? _primaryQuestion1Toggled;
        public bool? PrimaryQuestion1Toggled
        {
            get { return _primaryQuestion1Toggled; }
            set
            {
                _primaryQuestion1Toggled = value;
                SecondaryQuestion1Visibility = _primaryQuestion1Toggled.HasValue ? _primaryQuestion1Toggled.Value : false;
                if (_primaryQuestion1Toggled.HasValue && _primaryQuestion1Toggled.Value)
                    PrimaryQuestion1 = "Yes";
                else
                {
                    PrimaryQuestion1 = "No";
                    SecondaryQuestion1Toggled = value;
                }
                OnPropertyChanged(nameof(PrimaryQuestion1Toggled));
            }
        }
        private bool? _question1Toggled;
        public bool? Question1Toggled
        {
            get { return _question1Toggled; }
            set
            {
                _question1Toggled = value;
                if (_question1Toggled.HasValue && _question1Toggled.Value)
                    Question1 = "Yes";
                else
                    Question1 = "No";
                OnPropertyChanged(nameof(Question1Toggled));
            }
        }

        private bool? _question2Toggled;
        public bool? Question2Toggled
        {
            get { return _question2Toggled; }
            set
            {
                _question2Toggled = value;
                if (_question2Toggled.HasValue && _question2Toggled.Value)
                    Question2 = "Yes";
                else
                    Question2 = "No";
                OnPropertyChanged(nameof(Question2Toggled));
            }
        }


        private bool? _primaryQuestion2Toggled;
        public bool? PrimaryQuestion2Toggled
        {
            get { return _primaryQuestion2Toggled; }
            set
            {
                _primaryQuestion2Toggled = value;
                SecondaryQuestion2Visibility = _primaryQuestion2Toggled.HasValue ? _primaryQuestion2Toggled.Value : false;
                if (_primaryQuestion2Toggled.HasValue && _primaryQuestion2Toggled.Value)
                    PrimaryQuestion2 = "Yes";
                else
                {
                    PrimaryQuestion2 = "No";
                    SecondaryQuestion2Toggled = value;
                }
                OnPropertyChanged(nameof(PrimaryQuestion2Toggled));
            }
        }

        private bool? _secondaryQuestion1Toggled;
        public bool? SecondaryQuestion1Toggled
        {
            get { return _secondaryQuestion1Toggled; }
            set
            {
                _secondaryQuestion1Toggled = value;
                if (_secondaryQuestion1Toggled.HasValue && _secondaryQuestion1Toggled.Value)
                    SubQuestion1 = "Yes";
                else
                    SubQuestion1 = "No";
                OnPropertyChanged(nameof(SecondaryQuestion1Toggled));
            }
        }

        private bool? _secondaryQuestion2Toggled;
        public bool? SecondaryQuestion2Toggled
        {
            get { return _secondaryQuestion2Toggled; }
            set
            {
                _secondaryQuestion2Toggled = value;
                if (_secondaryQuestion2Toggled.HasValue && _secondaryQuestion2Toggled.Value)
                    SubQuestion2 = "Yes";
                else
                    SubQuestion2 = "No";
                OnPropertyChanged(nameof(SecondaryQuestion2Toggled));
            }
        }
        #endregion

        private string _additionalInformationText;
        public string AdditionalInformationText
        {
            get { return _additionalInformationText; }
            set
            {
                _additionalInformationText = value;
                OnPropertyChanged(nameof(SecondaryQuestion2Toggled));
            }
        }
        public string Question1
        {
            get { return question1; }
            set
            {
                question1 = value;
                OnPropertyChanged(nameof(Question1));
            }
        }
        public string Question2
        {
            get { return question2; }
            set
            {
                question2 = value;
                OnPropertyChanged(nameof(Question2));
            }
        }
        public string SubQuestion1
        {
            get { return subQuestion1; }
            set
            {
                subQuestion1 = value;
                OnPropertyChanged(nameof(SubQuestion1));
            }
        }
        public string SubQuestion2
        {
            get { return subQuestion2; }
            set
            {
                subQuestion2 = value;

                OnPropertyChanged(nameof(SubQuestion2));
            }
        }
        public string PrimaryQuestion1
        {
            get { return primaryQuestion1; }
            set
            {
                primaryQuestion1 = value;
                OnPropertyChanged(nameof(PrimaryQuestion1));
            }
        }
        public string PrimaryQuestion2
        {
            get { return primaryQuestion2; }
            set
            {
                primaryQuestion2 = value;
                OnPropertyChanged(nameof(PrimaryQuestion2));
            }
        }

        private async Task OnCancelButtonClicked()
        {
            await PopupNavigation.Instance.PopAsync();
        }
        private async Task OnSaveButtonClicked()
        {
            if (Question1Toggled.HasValue)
            {
                FormParamterClass.AllStandard = Question1Toggled;
            }
            if (Question2Toggled.HasValue)
            {
                FormParamterClass.ValidRepresentation = Question2Toggled;
            }
            if (PrimaryQuestion1Toggled.HasValue)
            {
                FormParamterClass.HasGlasses = PrimaryQuestion1Toggled;
            }
            if (PrimaryQuestion2Toggled.HasValue)
            {
                FormParamterClass.HasHearingAid = PrimaryQuestion2Toggled;
            }
            if (SecondaryQuestion1Toggled.HasValue)
            {
                FormParamterClass.GlassesUsed = SecondaryQuestion1Toggled;
            }
            if (SecondaryQuestion2Toggled.HasValue)
            {
                FormParamterClass.HearingAidUsed = SecondaryQuestion2Toggled;
            }
            commonDataService.ObservationNotes?.Invoke(Notes);
            commonDataService.StudentTestFormOverview.formParameters = JsonConvert.SerializeObject(FormParamterClass);
            await PopupNavigation.Instance.PopAsync();
        }
    }
}
