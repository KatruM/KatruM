using System.Collections.Generic;

namespace BDI3Mobile.Models.Requests
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
    }

    public class ContentCategoriesModel
    {
        public List<Product> products { get; set; }
        public List<Assessment> assessments { get; set; }
        public List<ContentCategoryLevel> contentCategoryLevels { get; set; }
        public List<ContentCategory> contentCategories { get; set; }
    }

}
