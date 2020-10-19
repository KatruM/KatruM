using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentItemsService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentItem> GetItems();
        void DeleteAll();
        void UpdateAll(List<ContentItem> contentItems);
        Task<List<ContentItem>> GetItemsAsync();
    }
}
