using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IProductService
    {
        void InserAll(ContentCategoriesModel model);
        List<Product> GetItems();
        void DeleteAll();
    }
}
