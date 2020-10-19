using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using BDI3Mobile.Models.DBModels;
using Xamarin.Forms;

namespace BDI3Mobile.Models.AcademicFolder
{
    public class AcademicContentModel
    {
        public bool BasalCeilingObtained { get; set; }
        public string SubDomainNote { get; set; }
        public int? rawScore { get; set; }
        public string GroupTitle { get; set; }
        public bool IsSampleItem { get; set; }
        public string AreaName { get; set; }
        public string DomainName { get; set; }
        public string SubDomainName { get; set; }
        public string AreaCode { get; set; }
        public string SubDomainCode { get; set; }
        public string DomainCode { get; set; }
        public int DomainCategoryId { get; set; }
        public int SubDomainCategoryId { get; set; }
        public int AreaId { get; set; }
        public int StartingPointCnntentItemID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DescFilePath { get; set; }
        public string ContentType { get; set; }
        public string HtmlFilePath { get; set; }
        public List<AcademicItemModel> AcademicItemModel { get; set; }
        public List<ImageContentItem> ImageCotentCollection { get; set; }
    }
    public class ImageContentItem
    {
        public string ImageStr { get; set; }
        public int ContentItemId { get; set; }
    }
    public class AcademicItemModel : INotifyPropertyChanged
    {
        public string ItemNumber { get; set; }
        public string Notes { get; set; }
        public string ItemNote { get; set; }
        public Action<AcademicItemModel> ItemClick { get; set; }
        public ICommand ItemClickCommand
        {
            get
            {
                return new Command(() =>
                {
                    ItemClick?.Invoke(this);
                });
            }
        }
        public int MaxTime { get; set; }
        public string MaterialContent { get; set; }
        public string ScoringNotes { get; set; }
        public string ScoringHtmlFilePath { get; set; }
        public string MaterialHtmFilePath { get; set; }
        public string ScoringDesc { get; set; }
        public int ItemSequenceNo { get; set; }
        public string ItemTitle { get; set; }
        public string Description { get; set; }
        public int ContentItemId { get; set; }
        public List<AcademicScoringModel> ScoringValues { get; set; }
        public string HtmlFilePath { get; set; }
        public List<ContentItemTally> TallyContent { get; set; }
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
                OnPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class AcademicScoringModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int points { get; set; }
        public string description { get; set; }
        public int contentRubricPointsId { get; set; }
        public int contentRubricId { get; set; }
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
                OnPropertyChanged("IsSelected");
            }
        }
        public string HtmlFilePath { get; set; }
    }


    public class ContentGroupWithItems
    {
        public int GroupId { get; set; }
        public List<int> ContentItemIDs { get; set; }
    }
    public enum CurrentAcademiTemplate
    {
        ImageMaterialSampleGrid = 0,
        ItemsInstructionScoreMaterialGrid = 1,
        InstructionItemsScoreGrid = 2,
        ScoreItemGrid = 3,
        InstructionsImageItemsScoreGrid = 4,
        ImageItemScoreGrid = 5,
        ImageSampleGrid = 6,
        InstructionImageMaterialItemScoreGrid = 7,
    }
}
