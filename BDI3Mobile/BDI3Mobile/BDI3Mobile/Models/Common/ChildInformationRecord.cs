using BDI3Mobile.Common;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BDI3Mobile.Models.Common
{
    public class ChildInformationRecord : BindableObject
    {
        public Action<ChildInformationRecord> ItemDelete { get; set; }
        public Action<ChildInformationRecord> ItemSync { get; set; }

        public ICommand ItemDeleteCommand
        {
            get
            {
                return new Command(() => 
                {
                    ItemDelete?.Invoke(this);
                });
            }
        }
        public ICommand ItemSyncCommand
        {
            get
            {
                return new Command(() => 
                {
                    ItemSync?.Invoke(this);
                });
            }
        }

        public int AssesmentId { get; set; }
        public int LocalTestInstance { get; set; }
        private bool _isSelect;
        public bool IsSelect
        {
            get
            {
                return _isSelect;
            }
            set
            {
                _isSelect = value;
                OnPropertyChanged(nameof(IsSelect));
            }
        }
        private Color _fColor;

        public Color FillColor
        {
            get
            {
                return _fColor;
            }
            set
            {
                _fColor = value;
                OnPropertyChanged(nameof(FillColor));
            }
        }

        private Color _outlineColor;
        public Color OutlineColor
        {
            get
            {
                return _outlineColor;
            }
            set
            {
                _outlineColor = value;
                OnPropertyChanged(nameof(OutlineColor));
            }
        }
        public string InitialTestDate { get; set; }
        public string Status { get; set; }
        public string RecordForm { get; set; }
        public string RecordFormName { get; set; }
        private bool enableRow=true;
        private string syncStatus;
        public string SyncStatus
        {
            get
            {
                return syncStatus;
            }
            set
            {
                syncStatus = value;
                OnPropertyChanged(nameof(SyncStatus));
            }
        }

        public int StatusCode { get; set; }
        public bool EnableRow
        {
            get
            {
                return enableRow;
            }
            set
            {
                enableRow = value;
                OnPropertyChanged(nameof(EnableRow));
            }
        }
        public ChildInformationRecord()
        {
            OutlineColor = Colors.LightBlueColor;
            FillColor = Colors.LightBlueColor;
        }
        public ChildInformationRecord ThisObject
        {
            get { return this; }
        }
    }
    public class AdminstrationNavigationParams
    {
        public string DOB { get; set; }
        public string TestDate { get; set; }
        public int LocalInstanceID { get; set; }
        public string AssesmentType { get; set; }
        public int OfflineStudentID { get; set; }
        public string FullName { get; set; }
        public bool IsDevelopmentCompleteForm { get; set; }
    }

    public class ChildRecordsNewAssessment : BindableObject
    {
        public int OfflineStudentId { get; set; }
        public ChildRecordsNewAssessment() { }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ChildID { get; set; }

        public string DOB { get; set; }

        public string Location { get; set; }

        public string Enrollment { get; set; }

        public string ChildUserID { get; set; }
        public int LocationID { get; set; }

        private bool _isImageVisible = true;
        public bool IsImageVisible
        {
            get { return _isImageVisible; }
            set
            {
                _isImageVisible = false;

                OnPropertyChanged(nameof(IsImageVisible));
            }
        }
        private bool isTableSepartorVisble = false;
        public bool IsTableSepartorVisble
        {
            get
            {
                return isTableSepartorVisble;
            }
            set
            {
                isTableSepartorVisble = value;
                OnPropertyChanged(nameof(IsTableSepartorVisble));
            }
        }
        private double rowHeight;
        public double RowHeight
        {
            get
            {
                return rowHeight;
            }
            set
            {
                rowHeight = value;
                OnPropertyChanged(nameof(RowHeight));
            }
        }

        public DateTime? DOBDateField
        {
            get
            {
                return !string.IsNullOrEmpty(DOB) ? Convert.ToDateTime(DOB) : default(DateTime?);
            }
        }
        public DateTime? EnrollmentDateField
        {
            get
            {
                return !string.IsNullOrEmpty(Enrollment) ? Convert.ToDateTime(Enrollment) : default(DateTime?);
            }
        }

        public ChildRecordsNewAssessment ThisObject
        {
            get { return this; }
        }
        private bool isChildEnable;
        public bool IsChildEditEnable
        {
            get
            {
                return isChildEnable;
            }
            set
            {
                isChildEnable = value;
                OnPropertyChanged(nameof(IsChildEditEnable));
            }
        }
        private string addRfIconImage = "icon_add_record.png";
        public string AddRfIconImage
        {
            get
            {
                return addRfIconImage;
            }
            set
            {
                addRfIconImage = value;
                OnPropertyChanged(nameof(AddRfIconImage));
            }
        }
    }
    public class ChildRecord : BindableObject
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private bool isSelected;
        public bool selected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
        private bool _isChecked;
        public bool isChecked
        {
            get
            {
                return _isChecked;
            }
            set
            {
                _isChecked = value;
                OnPropertyChanged(nameof(isChecked));
            }
        }

        private Color _fColor;

        public Color FillColor
        {
            get
            {
                return _fColor;
            }
            set
            {
                _fColor = value;
                OnPropertyChanged(nameof(FillColor));
            }
        }

        private Color _outlineColor;
        public Color OutlineColor
        {
            get
            {
                return _outlineColor;
            }
            set
            {
                _outlineColor = value;
                OnPropertyChanged(nameof(OutlineColor));
            }
        }

        private bool _isCheckboxVisible = true;
        public bool IsCheckboxVisible
        {
            get { return _isCheckboxVisible; }
            set
            {
                _isCheckboxVisible = value;
                OnPropertyChanged(nameof(IsCheckboxVisible));
            }
        }

        private bool _isImageVisible = true;
        public bool IsImageVisible
        {
            get { return _isImageVisible; }
            set
            {
                _isImageVisible = false;

                OnPropertyChanged(nameof(IsImageVisible));
            }
        }
        private bool isTableSepartorVisble = false;
        public bool IsTableSepartorVisble
        {
            get
            {
                return isTableSepartorVisble;
            }
            set
            {
                isTableSepartorVisble = value;
                OnPropertyChanged(nameof(IsTableSepartorVisble));
            }
        }
        private double rowHeight;
        public double RowHeight
        {
            get
            {
                return rowHeight;
            }
            set
            {
                rowHeight = value;
                OnPropertyChanged(nameof(RowHeight));
            }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ChildID { get; set; }
        public string DOB { get; set; }
        public string Location { get; set; }
        public string Enrollment { get; set; }
        public string ChildUserID { get; set; }
        public int LocationID { get; set; }
        private string editIconImage = "icon_edit.png";
        public string EditIconImage
        {
            get
            {
                return editIconImage;
            }
            set
            {
                editIconImage = value;
                OnPropertyChanged(nameof(EditIconImage));
            }
        }
        private string addRfIconImage = "icon_add_record.png";
        public string AddRfIconImage
        {
            get
            {
                return addRfIconImage;
            }
            set
            {
                addRfIconImage = value;
                OnPropertyChanged(nameof(AddRfIconImage));
            }
        }
        private bool isChildEnable;
        public bool IsChildEditEnable
        {
            get
            {
                return isChildEnable;
            }
            set
            {
                isChildEnable = value;
                OnPropertyChanged(nameof(IsChildEditEnable));
            }
        }
        public int OfflineStudentId { get; set; }
        public ChildRecord ThisObject
        {
            get { return this; }
        }
        public ChildRecord()
        {
            OutlineColor = Colors.LightBlueColor;
            FillColor = Colors.LightBlueColor;
        }

        public DateTime? DOBDateField
        {
            get
            {
                return !string.IsNullOrEmpty(DOB) ? Convert.ToDateTime(DOB) : default(DateTime?);
            }
        }
        public DateTime? EnrollmentDateField
        {
            get
            {
                return !string.IsNullOrEmpty(Enrollment) ? Convert.ToDateTime(Enrollment) : default(DateTime?);
            }
        }
    }
    public class TestSessionModel : BindableObject
    {
        public bool IsScoreSelected { get; set; }
        public string Notes { get; set; }
        public int ContentCategoryId { get; set; }
        public string ParentDomainCode { get; set; }
        private string domain;
        public string Domain
        {
            get
            {

                return domain;
            }
            set
            {
                if (value == "Adaptive" || value == "Social-Emotional" || value == "Communication" || value == "Motor" || value == "Cognitive" || value == "Literacy" || value == "Mathematics")
                {
                    DomainColor = Color.FromHex("#038277");
                    DomainFontAttribute = FontAttributes.Bold;
                }                         
                else
                {
                    DomainColor = Color.FromHex("#2673b9");
                    DomainFontAttribute = FontAttributes.None;
                }
                domain = value;
                OnPropertyChanged(nameof(Domain));
            }
        }

        private Color domainColor;
        public Color DomainColor
        {
            get
            {
                return domainColor;
            }
            set
            {
                domainColor = value;
                OnPropertyChanged(nameof(DomainColor));
            }
        }

        private Color _domainBackgroundColor;
        public Color DomainBackgroundColor
        {
            get
            {
                return _domainBackgroundColor;
            }
            set
            {
                _domainBackgroundColor = value;
                OnPropertyChanged(nameof(DomainBackgroundColor));
            }
        }

        private Thickness domainMargin;
        public Thickness DomainMargin
        {
            get
            {
                return domainMargin;
            }
            set
            {
                domainMargin = value;
                OnPropertyChanged(nameof(domainMargin));
            }
        }

        private FontAttributes domainFontAttribute;
        public FontAttributes DomainFontAttribute
        {
            get
            {
                return domainFontAttribute;
            }
            set
            {
                domainFontAttribute = value;
                OnPropertyChanged(nameof(DomainFontAttribute));
            }
        }

        public DateTime DateOfTest
        {
            get
            {
                if (!string.IsNullOrEmpty(TestDate))
                {
                    var splittedDate = TestDate.Split('/');
                    DateTime itemdateTime = new DateTime(Convert.ToInt32(splittedDate[2]), Convert.ToInt32(splittedDate[0]), Convert.ToInt32(splittedDate[1]));
                    return itemdateTime;
                }
                return DateTime.Now;
            }
        }

        private string testDate;
        public string TestDate
        {
            get
            {
                return testDate;
            }
            set
            {
                testDate = value;
                OnPropertyChanged(nameof(TestDate));
            }
        }

        private string examiner;
        public string Examiner
        {
            get
            {
                return examiner;
            }
            set
            {
                examiner = value;
                OnPropertyChanged(nameof(Examiner));
            }
        }

        private string status;
        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private string rawScore;
        public string RawScore
        {
            get
            {
                return rawScore;
            }
            set
            {
                rawScore = value;
                OnPropertyChanged(nameof(RawScore));
            }
        }

        private TimeSpan timer;
        public TimeSpan Timer
        {
            get
            {
                return timer;
            }
            set
            {
                timer = value;
                OnPropertyChanged(nameof(Timer));
            }
        }

        private bool showTimerErrorMessage;

        public bool ShowTimerErrorMessage
        {
            get
            {
                return showTimerErrorMessage;
            }
            set
            {
                showTimerErrorMessage = value;
                OnPropertyChanged(nameof(ShowTimerErrorMessage));
            }
        }

        private string ae;
        public string AE
        {
            get
            {
                return ae;
            }
            set
            {
                ae = value;
                OnPropertyChanged(nameof(AE));
            }
        }

        private string pr;
        public string PR
        {
            get
            {
                return pr;
            }
            set
            {
                pr = value;
                OnPropertyChanged(nameof(PR));

            }
        }

        private string ss;
        public string SS
        {
            get
            {
                return ss;
            }
            set
            {
                ss = value;
                OnPropertyChanged(nameof(SS));
            }
        }

        private bool _IsDateVisible;
        public bool IsDateVisible
        {
            get
            {
                return _IsDateVisible;
            }
            set
            {
                _IsDateVisible = value;
                OnPropertyChanged(nameof(IsDateVisible));
            }
        }

        private bool _IsDomainVisible;
        public bool DomainVisibility
        {
            get
            {
                return _IsDomainVisible;
            }
            set
            {
                _IsDomainVisible = value;
                OnPropertyChanged(nameof(DomainVisibility));
            }
        }

        private bool _IsFluency;
        public bool IsFluency
        {
            get
            {
                return _IsFluency;
            }
            set
            {
                _IsFluency = value;
                OnPropertyChanged(nameof(IsFluency));
            }
        }

        private bool _IsNotFluency;
        public bool IsNotFluency
        {
            get
            {
                return _IsNotFluency;
            }
            set
            {
                _IsNotFluency = value;
                OnPropertyChanged(nameof(IsNotFluency));
            }
        }

        public Action<string> LoadDomainBasedQuestionsAction { get; set; }
        public ICommand LoadDomainBasedQuestions
        {
            get
            {
                return new Command<string>((domainCode) =>
                {
                    LoadDomainBasedQuestionsAction?.Invoke(domainCode);
                });
            }
        }
        public string DomainCode { get; set; }
        private bool enablerawScore;
        public bool EnablerawScore
        {
            get
            {
                return enablerawScore;
            }
            set
            {
                enablerawScore = value;
                OnPropertyChanged(nameof(EnablerawScore));
            }
        }
        public int ExaminerID { get; set; }
    }

    public class BatteryTypes : BindableObject
    {
        public int assessmentId { get; set; }
        public int productId { get; set; }
        private string description;
        public string Description
        {
            get
            {
                return description;
            }
            
            set
            {
                description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private bool isSelected;
        public bool selected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(selected));
            }
        }
    }
}
