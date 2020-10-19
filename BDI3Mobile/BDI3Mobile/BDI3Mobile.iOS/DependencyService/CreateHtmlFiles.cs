using System;
using System.IO;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BDI3Mobile.CustomRenderer;
using BDI3Mobile.IServices;
using Xamarin.Essentials;
using Xamarin.Forms;
[assembly: Dependency(typeof(BDI3Mobile.iOS.DependencyService.CreateHtmlFiles))]

namespace BDI3Mobile.iOS.DependencyService
{
    public class CreateHtmlFiles: ICreateHtmlFiles
    {
        public async Task CreateHtmlFolders()
        {
           
        }

        public async Task SaveFile(string filename)
        {
            try
            {
                UserDialogs.Instance.HideLoading();
                if (filename == "BDI3_Mobile/index.htm")
                {
                    UserDialogs.Instance.HideLoading();
                    string baseUrl = PCLStorage.FileSystem.Current.LocalStorage.Path;

                    string uri = Path.Combine(baseUrl, "index.html");
                    await Browser.OpenAsync(new Uri("file://" + uri));
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, filename))
                    });

                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Error while opening the Report.");
            }
        }

        
    }


}
