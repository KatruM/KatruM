using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentItemTalliesScoresService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentItemTallyScore> GetItems();
        void DeleteAll();
        Task<List<ContentItemTallyScore>> GetItemsAsync();
    }
}
