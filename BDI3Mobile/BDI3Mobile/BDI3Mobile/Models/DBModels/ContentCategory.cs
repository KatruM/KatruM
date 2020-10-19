using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.Models.DBModels
{
    public class Product
    {
        public int productId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string abbreviation { get; set; }
        public int sequence { get; set; }
    }

    public class Assessment
    {
        public int assessmentId { get; set; }
        public int productId { get; set; }
        public string description { get; set; }
    }

    public class ContentCategoryLevel
    {
        public int contentCategoryLevelId { get; set; }
        public int assessmentId { get; set; }
        public int level { get; set; }
        public string name { get; set; }
    }

    public class ContentCategory 
    {
        public int contentCategoryId { get; set; }
        public int contentCategoryLevelId { get; set; }
        public int parentContentCategoryId { get; set; }
        public int sequenceNo { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public string code { get; set; }
        [Ignore]
        public string ParentCategoryTitle { get; set; }
        public string BehaviourContent { get; set; }
        public int lowAge { get; set; }
        public int highAge { get; set; }
    }

    //public class ContentCategoriesModel
    //{
    //    //public List<Product> products { get; set; }
    //    //public List<Assessment> assessments { get; set; }
    //    //public List<ContentCategoryLevel> contentCategoryLevels { get; set; }
    //    public List<ContentCategory> contentCategories { get; set; }
    //}

    public class ContentItem
    {
        [PrimaryKey]
        public int contentItemId { get; set; }
        public string itemText { get; set; }
        public string itemNumber { get; set; }
        public string itemCode { get; set; }
        public int sequenceNo { get; set; }
        public int lowScore { get; set; }
        public int highScore { get; set; }
        public int maxTime1 { get; set; }
        public int maxTime2 { get; set; }
        public bool sampleItem { get; set; }
        public string HtmlFilePath { get; set; }
        public string alternateItemText { get; set; }
    }

    public class ContentCategoryItem
    {
        public int contentCategoryId { get; set; }
        public int contentItemId { get; set; }
    }

    public class ContentItemAttribute
    {
        [PrimaryKey]
        public int contentItemAttributeId { get; set; }
        public int contentItemId { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public string HtmlFilePath { get; set; }
    }

    public class ContentRubric
    {
        [PrimaryKey]
        public int contentRubricId { get; set; }
        public int contentItemId { get; set; }
        public int contentItemAttributeId { get; set; }
        public string title { get; set; }
        public string notes { get; set; }
        public string pointsDesc { get; set; }
        public string HtmlFilePath { get; set; }
    }

    public class ContentRubricPoint : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        [PrimaryKey]
        public int contentRubricPointsId { get; set; }
        public int contentRubricId { get; set; }
        public int points { get; set; }
        public string description { get; set; }

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

    public class ContentItemTally : INotifyPropertyChanged
    {
        private long tallyDescriptionWidth;
        [Ignore]
        public long TallyDescriptionWidth
        {
            get
            {
                return tallyDescriptionWidth;
            }
            set
            {
                tallyDescriptionWidth = value;
                OnPropertyChanged("TallyDescriptionWidth");
            }
        }
        private bool checkCorrectVisible;
        [Ignore]
        public bool CheckCorrectVisible
        {
            get
            {
                return checkCorrectVisible;
            }
            set
            {
                checkCorrectVisible = value;
                OnPropertyChanged("CheckCorrectVisible");
            }
        }
        private bool uncheckCorrectVisible = true;
        [Ignore]
        public bool UncheckCorrectVisible
        {
            get
            {
                return uncheckCorrectVisible;
            }
            set
            {
                uncheckCorrectVisible = value;
                OnPropertyChanged("UncheckCorrectVisible");
            }
        }


        private bool checkInCorrectVisible;
        [Ignore]
        public bool CheckInCorrectVisible
        {
            get
            {
                return checkInCorrectVisible;
            }
            set
            {
                checkInCorrectVisible = value;
                OnPropertyChanged("CheckInCorrectVisible");
            }
        }
        private bool uncheckInCorrectVisible = true;
        [Ignore]
        public bool UncheckInCorrectVisible
        {
            get
            {
                return uncheckInCorrectVisible;
            }
            set
            {
                uncheckInCorrectVisible = value;
                OnPropertyChanged("UncheckInCorrectVisible");
            }
        }

        private bool isCorrectChecked;
        [Ignore]
        public bool IsCorrectChecked
        {
            get
            {
                return isCorrectChecked;
            }
            set
            {
                isCorrectChecked = value;
                OnPropertyChanged("IsCorrectChecked");
            }
        }
        private bool isInCorrectChecked;
        [Ignore]
        public bool IsInCorrectChecked
        {
            get
            {
                return isInCorrectChecked;
            }
            set
            {
                isInCorrectChecked = value;
                OnPropertyChanged("IsInCorrectChecked");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        public int contentItemTallyId { get; set; }
        public int contentItemId { get; set; }
        public string groupText { get; set; }
        public string text { get; set; }
        public int sequenceNo { get; set; }
        public int lowScore { get; set; }
        public int highScore { get; set; }
        public int correctIncorrect { get; set; }

        public int RowNum { get; set; }

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
        [Ignore]
        public Action<ContentItemTally> CorrectLayoutAction { get; set; }
        [Ignore]
        public Action<ContentItemTally> InCorrectLayoutAction { get; set; }
        [Ignore]
        public ICommand CorrectLayoutCommand
        {
            get
            {
                return new Command(()=> 
                {
                    CorrectLayoutAction?.Invoke(this);
                });
            }
        }
        [Ignore]
        public ICommand InCorrectLayoutCommand
        {
            get
            {
                return new Command(() =>
                {
                    InCorrectLayoutAction?.Invoke(this);
                });
            }
        }
    }
    public class ContentItemTallyScore
    {
        public int contentItemId { get; set; }
        public int totalPoints { get; set; }
        public int score { get; set; }
    }

    public class ContentGroup
    {
        public int contentGroupId { get; set; }
        public int assessmentId { get; set; }
        public string groupText { get; set; }
    }

    public class ContentGroupItem
    {
        public int contentGroupId { get; set; }
        public int contentItemId { get; set; }
    }

    public class ContentBasalCeilings
    {
        [PrimaryKey]
        public int contentCategoryId { get; set; }
        public int basalCount { get; set; }
        public int ceilingCount { get; set; }
        public int basalScore { get; set; }
        public int ceilingScore { get; set; }
        public string reverseItem { get; set; }
        public string reverseStartItem {get;set;}
        public int reverseContentCategoryId { get; set; }
    }

    public class ContentCategoriesModel
    {
        public List<Product> products { get; set; }
        public List<Assessment> assessments { get; set; }
        public List<ContentCategoryLevel> contentCategoryLevels { get; set; }
        public List<ContentCategory> contentCategories { get; set; }
        public List<ContentItem> contentItems { get; set; }
        public List<ContentCategoryItem> contentCategoryItems { get; set; }
        public List<ContentItemAttribute> contentItemAttributes { get; set; }
        public List<ContentRubric> contentRubrics { get; set; }
        public List<ContentRubricPoint> contentRubricPoints { get; set; }
        public List<ContentItemTally> contentItemTallies { get; set; }
        public List<ContentItemTallyScore> contentItemTallyScores { get; set; }
        public List<ContentGroup> contentGroups { get; set; }
        public List<ContentGroupItem> contentGroupItems { get; set; }
        public List<ContentBasalCeilings> contentBasalCeilings { get; set; }
        public int StatusCode { get; set; }
    }

}
