using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentCategoryService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentCategory> GetContentCategory();
        void DeleteAll();
        Task<List<ContentCategory>> GetItemsAsync();

    }
}
