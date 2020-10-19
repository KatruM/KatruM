using BDI3Mobile.Common;
using Newtonsoft.Json;
using SQLite;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Models.Common
{
    public class CommonData
    {
        [JsonProperty("fundingSources")]
        public List<AllComanDataModel> FundingSources { get; set; }
        [JsonProperty("languages")]
        public List<AllComanDataModel> Languages { get; set; }
        [JsonProperty("diagnoses")]
        public List<AllComanDataModel> Diagnoses { get; set; }
        [JsonProperty("races")]
        public List<AllComanDataModel> Races { get; set; }
        [JsonProperty("userTypes")]
        public List<AllComanDataModel> UserTypes { get; set; }
        [JsonProperty("permissions")]
        public List<AllComanDataModel> Permissions { get; set; }

    }

    public class AllComanDataModel
    {
        public int Value { get; set; }
        public string TextValue { get; set; }
        public string Text { get; set; }
        public bool Selected { get; set; }
        public bool Disabled { get; set; }
        public int Type { get; set; }
    }

    public class UserSyncTable
    {
        [PrimaryKey, AutoIncrement]
        public int LocalSyncID { get; set; }
        public int DownLoadedBy { get; set; }
        public string LastSyncDatetime { get; set; }
    }

    public class ContentSyncData
    {
        public bool ContentSynced { get; set; }
        public string ContentType { get; set; }
        public string LastSyncDate { get; set; }
    }

    public class ChildAssessmentRecord : BindableObject
    {
        public int SyncStatusCode { get; set; }
        private bool _isSelected;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
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
        public string AssessmentName { get; set; }
        public string TestDate { get; set; }
        public string ChildLastName { get; set; }
        public string ChildFirstName { get; set; }
        public string Status { get; set; }
        public string SyncStatus { get; set; }
        public int StatusCode { get; set; }

        public string DOB { get; set; }
        public string FullName { get; set; }

        public ChildAssessmentRecord()
        {
            OutlineColor = Colors.LightBlueColor;
            FillColor = Colors.LightBlueColor;
        }
        public int AssesmentId { get; set; }
        public int OfflineStudentID { get; set; }
        public int LocalTestInstance { get; set; }
        public string InitialTestDate { get; set; }
        public string RecordForm { get; set; }
        public string RecordFormName { get; set; }
        public ChildAssessmentRecord ThisObject
        {
            get { return this; }
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
    }

    public class TestRecordStatus : BindableObject
    {
        private string statusText;
        public string StatusText
        {
            get
            {
                return statusText;
            }
            set
            {
                statusText = value;
                OnPropertyChanged(nameof(StatusText));
            }
        }
        private bool isselected;
        public bool Selected
        {
            get
            {
                return isselected;
            }
            set
            {
                isselected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }
    }

}
