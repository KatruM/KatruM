using System.Threading.Tasks;

namespace BDI3Mobile.IServices
{
    public interface ICreateHtmlFiles
    {
        Task CreateHtmlFolders();
        Task SaveFile(string filename);
    }
}
