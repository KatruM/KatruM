using Xamarin.Forms;

namespace BDI3Mobile.Models.ReportModel
{
    public class ReportType : BindableObject
    {
        private string name;
        public string ReportTypeName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(ReportTypeName));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        private int reportTypeID;
        public int ReportTypeID
        {
            get
            {
                return reportTypeID;
            }
            set
            {
                reportTypeID = value;
                OnPropertyChanged(nameof(ReportTypeID));
            }
        }
    }
    public class ScoreType : BindableObject
    {
        private string name;
        public string ScoreTypeName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(ScoreTypeName));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
       
    }
    public class NotesType : BindableObject
    {
        private string name;
        public string NoteTypeName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(NoteTypeName));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        private int id;
        public int NotesTypeID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(NotesTypeID));
            }
        }
    }
    public class OutputFormatType : BindableObject
    {
        private string name;
        public string OutputFormatTypeName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(OutputFormatTypeName));
            }
        }
        private int id;
        public int OutputFormatTypeID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged(nameof(OutputFormatTypeID));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

    }
    public class StandardDeviation : BindableObject
    {
        private double standardDeviationValue;
        public double StandardDeviationValue
        {
            get
            {
                return standardDeviationValue;
            }
            set
            {
                standardDeviationValue = value;
                OnPropertyChanged(nameof(StandardDeviationValue));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }
    public class ReportParameters
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
    public class ReportLocationParameters
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string LocationIds { get; set; }
        public bool IsDataExport { get; set; }
    }
    public class ReportBatteryTypes
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string userId { get; set; }
    }
    public class ProgramLabels : BindableObject
    {
        private string labelname;
        public string LabelName
        {
            get
            {
                return labelname;
            }
            set
            {
                labelname = value;
                OnPropertyChanged(nameof(LabelName));
            }
        }
        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        private bool disabled;
        public bool Disabled
        {
            get
            {
                return disabled;
            }
            set
            {
                disabled = value;
                OnPropertyChanged(nameof(Disabled));
            }
        }

        private int labelId;
        public int LabelID
        {
            get
            {
                return labelId;
            }
            set
            {
                labelId = value;
                OnPropertyChanged(nameof(LabelID));
            }
        }


    }
    public class RecordForms : BindableObject
    {
        private string formInstanceId;
        public string FormInstanceID
        {
            get
            {
                return formInstanceId;
            }
            set
            {
                formInstanceId = value;
                OnPropertyChanged(nameof(FormInstanceID));
            }
        }
        private int assessmentId;
        public int AssessmentID
        {
            get
            {
                return assessmentId;
            }
            set
            {
                assessmentId = value;
                OnPropertyChanged(nameof(AssessmentID));
            }
        }
        private string description { get; set; }
        public string RecordFormName
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                OnPropertyChanged(nameof(RecordFormName));
            }
        }
        private string programLabelId;
        public string ProgramLabelID
        {
            get
            {
                return programLabelId;
            }
            set
            {
                programLabelId = value;
                OnPropertyChanged(nameof(ProgramLabelID));
            }

        }
        private bool selected;
        public bool IsSelect
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged(nameof(IsSelect));
            }
        }
    }
    public class DeviationType : BindableObject
    {
        private string name;
        public string DeviationName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(DeviationName));
            }
        }
        private string deviationValue;
        public string DeviationValue
        {
            get
            {
                return deviationValue;
            }
            set
            {
                deviationValue = value;
                OnPropertyChanged(nameof(DeviationValue));
            }
        }
        private bool isSelected;
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

    }

}