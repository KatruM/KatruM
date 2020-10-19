using BDI3Mobile.Models.DBModels;
using Xamarin.Forms;

namespace BDI3Mobile.DataTemplates
{
    class LocationDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ZeroLevel { get; set; }
        public DataTemplate FirstLevel { get; set; }
        public DataTemplate SecondLevel { get; set; }
        public DataTemplate ThirdLevel { get; set; }
        public DataTemplate FourthLevel { get; set; }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            DataTemplate dataTemplate = ZeroLevel;
            if(((LocationNew)item).Level == 1)
            {
                dataTemplate = FirstLevel;
            }
            else if (((LocationNew)item).Level == 2)
            {
                dataTemplate = SecondLevel;
            }
            else if (((LocationNew)item).Level == 3)
            {
                dataTemplate = ThirdLevel;
            }
            else if (((LocationNew)item).Level == 4)
            {
                dataTemplate = FourthLevel;
            }
            return dataTemplate;
        }
    }
}
