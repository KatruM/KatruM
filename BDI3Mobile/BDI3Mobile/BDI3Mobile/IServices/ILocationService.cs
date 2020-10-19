using BDI3Mobile.Models.DBModels;
using System.Collections.Generic;

namespace BDI3Mobile.IServices
{
    public interface ILocationService
    {
        void InsertAll(List<Location> locationList);
        List<Location> GetLocations();
        Location GetLocationById(int guid);
        void UpdateAll(List<Location> locationList);
        void InsertorUpdate(Location _location);
        List<Location> GetLocationTree();
        void DeleteAllLocations();
        void DeleteByDownloadedBy(int downloadedby);
        List<Location> GetAllByDownloadedByLocations(int downloadedBy);
    }
}
