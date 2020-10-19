using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentRubricsService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentRubric> GetItems();
        void DeleteAll();
        void UpdateAll(List<ContentRubric> model);
        Task<List<ContentRubric>> GetItemsAsync();
    }
}
