using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentGroupItemsService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentGroupItem> GetItems();
        void DeleteAll();
        Task<List<ContentGroupItem>> GetItemsAsync();
    }
}
