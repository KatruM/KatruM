using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Xamarin.Forms;

namespace BDI3Mobile.Models.DevelopmentalFormModel
{

    public class SubDomainContent
    {
        public bool BasalCeilingReached { get; set; }
        public int? subdomainscore { get; set; }
        public int SubDomainCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string ParentCategoryName { get; set; }
        public string SubDomainCode { get; set; }
        public string SubDomainName { get; set; }
        public List<AdministrationContent> AdministrationContents { get; set; }

        public List<ContentItem> ContentItemCollection { get; set; }
        public List<ContentCategory> ContentCategoryCollection { get; set; }
    }

    #region ScreenerModel
    public class DomainContent
    {
        public bool BasalCeilingObtained { get; set; }
        public int DoaminCategoryId { get; set; }
        public int? rawscore { get; set; }
        public int CategoryId { get; set; }
        public string DomainCode { get; set; }
        public string DomainName { get; set; }
        public List<AdministrationContent> AdministrationContents { get; set; }
    }
    #endregion

    public class AdministrationContent
    {
        public int ContentItemID { get; set; }
        public string ItemAbbrevation { get; set; }
        public string ItemNumber { get; set; }
        public int MaxTime { get; set; }
        public string BehaviourHtmlFIlePath { get; set; }
        public string BehaviorContent { get; set; }
        public CaptureMode CaptureModeDesc { get; set; }

        public string MaterialContentFilePath { get; set; }
        public string MaterialContent { get; set; }
        public string ScoringNotes { get; set; }
        public string ScoringCustomData { get; set; }
        public bool StructureEnabled { get; set; }
        public bool ObservationEnabled { get; set; }
        public bool InterviewEnabled { get; set; }
        public bool ScoringListviewCheckboxEnabled { get; set; }
        public List<ImageSource> Images { get; set; } = new List<ImageSource>();
        public List<ContentRubricPoint> ScoringValuesandDesc { get; set; } = new List<ContentRubricPoint>();
        public List<ContentItemTally> TallyContent { get; set; } = new List<ContentItemTally>();
    }
    public class CaptureMode
    {
        public string Notes { get; set; }
        public int DomainCategoryId { get; set; }

        public List<CaptureModeContentModel> StructuredCollectionData { get; set; }
        public List<CaptureModeContentModel> ObservationCollectionData { get; set; }
        public List<CaptureModeContentModel> InterviewCollectionData { get; set; }
        public CaptureModeContentModel StructuredContent { get; set; }
        public CaptureModeContentModel ObservationContent { get; set; }
        public CaptureModeContentModel InterviewContent { get; set; }

        public string StructureScoringDesc { get; set; }
        public string ObservationeScoringDesc { get; set; }
        public string InterviewScoringDesc { get; set; }
        public string StructureScoringNote { get; set; }
        public string ObservationeScoringNote { get; set; }
        public string InterviewScoringNote { get; set; }

        public string StructureScoringContent { get; set; }
        public string ObservationeScoringContent { get; set; }
        public string InterviewScoringContent { get; set; }

        public List<ContentRubricPoint> StructuredContentScoring { get; set; } = new List<ContentRubricPoint>();
        public List<ContentRubricPoint> ObservationContentScoring { get; set; } = new List<ContentRubricPoint>();
        public List<ContentRubricPoint> InterviewContentScoring { get; set; } = new List<ContentRubricPoint>();
        public List<ContentRubricPoint> DefaultContentScoring { get; set; } = new List<ContentRubricPoint>();
        public ContentRubricPoint SelectedStructuredContentRubicPoint { get; set; }
        public ContentRubricPoint SelectedObservationContentRubicPoint { get; set; }
        public ContentRubricPoint SelectedInterviewContentRubicPoint { get; set; }
        public ContentRubricPoint SelectedDefaultRubicPoint { get; set; }
        public bool IsInterViewSelected { get; set; }
        public bool IsObservationSelected { get; set; }
        public bool IsStructredSelected { get; set; }
        public CaptureMode ThisObject
        {
            get { return this; }
        }
    }
    public class ImageLocation : INotifyPropertyChanged
    {
        private string image;

        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyChange)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyChange));
            }
        }
    }
    public class Model : INotifyPropertyChanged
    {
        private string textValue;
        private bool isChecked;
        public Model()
        {

        }

        public string TextValue
        {
            get
            {
                return textValue;
            }
            set
            {
                textValue = value;
                OnPropertyChanged("TextValue");
            }
        }

        public bool IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                isChecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyChange)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyChange));
            }
        }
    }

    public class CaptureModeContentModel
    {
        public int ContentID { get; set; }
        public string HTMilFilePath { get; set; }
        public string ContentDescription { get; set; }
    }

    public enum AssessmentConfigurationType
    {
        BattelleDevelopmentComplete,
        BattelleDevelopmentScreener,
        BattelleEarlyAcademicSurvey
    }
}
