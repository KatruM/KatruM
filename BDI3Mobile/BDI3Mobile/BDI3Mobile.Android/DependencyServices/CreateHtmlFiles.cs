using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using BDI3Mobile.IServices;
using Xamarin.Essentials;
using Xamarin.Forms;
[assembly: Dependency(typeof(BDI3Mobile.Droid.DependencyServices.CreateHtmlFiles))]

namespace BDI3Mobile.Droid.DependencyServices
{
    class CreateHtmlFiles : ICreateHtmlFiles
    {

        public async Task CreateHtmlFolders()
        {

        }

        public async Task SaveFile(string filename)
        {
            try
            {
                UserDialogs.Instance.HideLoading();
                var filePath = System.IO.Path.Combine(PCLStorage.FileSystem.Current.LocalStorage.Path, filename);
                UserDialogs.Instance.HideLoading();
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(filePath)
                });

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Error while opening the Report.");
            }
        }
    }
}
