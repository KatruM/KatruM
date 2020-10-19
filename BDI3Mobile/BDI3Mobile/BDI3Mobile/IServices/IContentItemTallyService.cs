using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentItemTallyService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentItemTally> GetItems();
        void DeleteAll();
        Task<List<ContentItemTally>> GetItemsAsync();
    }
}
