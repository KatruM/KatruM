using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentItemAttributesService
    {
        void InserAll(ContentCategoriesModel model);
        List<ContentItemAttribute> GetItems();
        void DeleteAll();
        void UpdateAll(List<ContentItemAttribute> contentItems);
        Task<List<ContentItemAttribute>> GetItemsAsyns();
    }
}
