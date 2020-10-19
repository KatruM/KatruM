using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentCategoryItemsService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentCategoryItem> GetItems();
        void DeleteAll();
        Task<List<ContentCategoryItem>> GetItemsAsync();
    }
}
