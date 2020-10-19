using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentCategoryLevelsService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentCategoryLevel> GetItems();
        void DeleteAll();
        Task<List<ContentCategoryLevel>> GetItemsAsync();
    }
}
