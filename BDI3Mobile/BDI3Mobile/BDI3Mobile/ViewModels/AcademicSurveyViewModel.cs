using Xamarin.Forms;
using System.Collections.Generic;

namespace BDI3Mobile.ViewModels
{
    public class AcademicSurveyViewModel:BaseclassViewModel
    {
        private List<AcademicSurverModel> _acadmecSurveyRecords = new List<AcademicSurverModel>();
        public List<AcademicSurverModel> AcadmecSurveyRecords
        {
            get
            {
                return _acadmecSurveyRecords;
            }

            set
            {
                _acadmecSurveyRecords = value;
                OnPropertyChanged(nameof(AcadmecSurveyRecords));
            }
        }

        //public readonly IContentCategoryService _contentcategoryservice;

        public AcademicSurveyViewModel()
        {
            AcadmecSurveyRecords.Add(new AcademicSurverModel() { ROW = "1", Item = "1", CorrectResponse = "crow", Point = "1  0",});
            AcadmecSurveyRecords.Add(new AcademicSurverModel() { ROW = "1", Item = "1", CorrectResponse = "crow", Point = "1  0", });
            AcadmecSurveyRecords.Add(new AcademicSurverModel() { ROW = "1", Item = "1", CorrectResponse = "crow", Point = "1  0", });
            AcadmecSurveyRecords.Add(new AcademicSurverModel() { ROW = "1", Item = "1", CorrectResponse = "crow", Point = "1  0", });
            AcadmecSurveyRecords.Add(new AcademicSurverModel() { ROW = "1", Item = "1", CorrectResponse = "crow", Point = "1  0", });
        }

        public class AcademicSurverModel : BindableObject
        {
            private string row;
            public string ROW
            {
                get
                {
                    return row;
                }
                set
                {
                    row = value;
                    OnPropertyChanged(nameof(ROW));
                }
            }

            private string item;
            public string Item
            {
                get
                {
                    return item;
                }
                set
                {
                    item = value;
                    OnPropertyChanged(nameof(Item));
                }
            }

            private string correctresponse;
            public string CorrectResponse
            {
                get
                {
                    return correctresponse;
                }
                set
                {
                    correctresponse = value;
                    OnPropertyChanged(nameof(CorrectResponse));
                }
            }

            private string point;
            public string Point
            {
                get
                {
                    return point;
                }
                set
                {
                    point = value;
                    OnPropertyChanged(nameof(Point));
                }
            }

        }
    }
     
}
