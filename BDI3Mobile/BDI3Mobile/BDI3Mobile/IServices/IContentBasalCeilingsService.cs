using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface IContentBasalCeilingsService
    {
        void InserAll(List<ContentBasalCeilings> model);
        List<ContentBasalCeilings> GetItems();
        void DeleteAll();
        Task<List<ContentBasalCeilings>> GetItemsAsync();
    }
}
