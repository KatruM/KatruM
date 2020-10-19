using BDI3Mobile.IServices;
using System;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class SaveImages : ISaveImageService
    {
        public string GetStorageFolderPath()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                return Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);// .LocalApplicationData);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                return Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            }
            else
            {
                return Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);// .LocalApplicationData);
            }
        }
    }
}
