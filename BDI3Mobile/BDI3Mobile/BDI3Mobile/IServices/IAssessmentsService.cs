using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface IAssessmentsService
    {
        void InserAll(ContentCategoriesModel model);
        List<Assessment> GetItems();
        void DeleteAll();
    }
}
