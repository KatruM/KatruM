using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentGroupService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentGroup> GetItems();
        void DeleteAll();
        Task<List<ContentGroup>> GetItemsAsync();
    }
}
