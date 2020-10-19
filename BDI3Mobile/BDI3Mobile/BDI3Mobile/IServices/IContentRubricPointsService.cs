using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentRubricPointsService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentRubricPoint> GetItems();
        void DeleteAll();
        void UpdateAll(List<ContentRubricPoint> contentRubricPoints);
        Task<List<ContentRubricPoint>> GetItemsAsync();
    }
}
