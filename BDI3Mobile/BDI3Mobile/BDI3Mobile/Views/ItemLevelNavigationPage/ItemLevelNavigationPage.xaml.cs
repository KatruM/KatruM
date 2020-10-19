using Rg.Plugins.Popup.Pages;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BDI3Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemLevelNavigationPage : PopupPage
    {
        private bool _IsLoaded;

        public  ItemLevelNavigationPage()
        {
            InitializeComponent();            
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            
            if(height > width)
            {
                if(Device.RuntimePlatform == Device.Android)
                    MainFrame.Margin = new Thickness(0, 93, 400, 0);
                else if(Device.RuntimePlatform == Device.iOS)
                    MainFrame.Margin = new Thickness(0, 80, 400, 0);
            }
            else
            {
                if(Device.RuntimePlatform == Device.Android)
                    MainFrame.Margin = new Thickness(0, 93, 0, 0);
                else if (Device.RuntimePlatform == Device.iOS)
                    MainFrame.Margin = new Thickness(0, 80, 0, 0);
            }
        }
    }

    public class MenuContentModel : INotifyPropertyChanged
    {
        public bool ShowStatus { get; set; }
        public bool ShowSubDomainStatus
        {
            get
            {
                return !string.IsNullOrEmpty(SubDomainStatus);
            }
        }
        public string SubDomainStatus { get; set; }
        public bool BelowStartingPoint { get; set; }
        private string progressImage = "completed_TickMark.png";
        public string ProgressImage
        {
            get
            {
                return progressImage;
            }
            set
            {
                progressImage = value;
                OnPropertyChanged(nameof(ProgressImage));
            }
        }

        public string HtmlFilePath { get; set; }
        private string imageName = "menudownarrow.png";
        public string ImageName
        {
            get
            {
                return imageName;
            }
            set
            {
                imageName = value;
                OnPropertyChanged(nameof(ImageName));
            }
        }
        public int SequenceNumber { get; set; }
        public Action<MenuContentModel> SelectedCommandAction;
        public ICommand SelectedCommand
        {
            get
            {
                return new Command(()=> 
                {
                    SelectedCommandAction?.Invoke(this);
                });
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyChanged)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
        }

        private int rowHeight;
        public int RowHeight
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

        public int ContentCatgoryId { get; set; }
        public int ContentLevelID { get; set; }
        public string Code { get; set; }
        private string text;
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        public int ParentID { get; set; }
        public bool ShowImage {get;set;}
        public bool ShowUpArrow { get; set; }
        public int ContentItemId { get; set; }
        public bool IsStartingPoint { get; set; }
        private bool isVisible;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        
    }
    public class NavigationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Level1Template { get; set; }
        public DataTemplate Level2Template { get; set; }
        public DataTemplate Level3Template { get; set; }
        public DataTemplate StartingPointTemplate { get; set; }
        public DataTemplate AreaTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var menuItem = (MenuContentModel)item;
            if (menuItem.IsStartingPoint)
            {
                return StartingPointTemplate;
            }
            else if(menuItem.ContentLevelID == 1 || menuItem.ContentLevelID == 4 || menuItem.ContentLevelID == 6)
            {
                return Level1Template;
            }
            else if (menuItem.ContentLevelID == 2 || menuItem.ContentLevelID == 7)
            {
                return Level2Template;
            }
            else if (menuItem.ContentLevelID == 3 || menuItem.ContentLevelID == 5 || menuItem.ContentLevelID == 9)
            {
                return Level3Template;
            }
            else if (menuItem.ContentLevelID == 8)
            {
                return AreaTemplate;
            }
            return null;
        }
    }
}