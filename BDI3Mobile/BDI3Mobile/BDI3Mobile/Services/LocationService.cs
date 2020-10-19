using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class LocationService : ILocationService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public LocationService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Location>();
        }

        public Location GetLocationById(int guid)
        {
            try
            {
                return _sqlConnection.Table<Location>().ToList().FirstOrDefault(p => p.LocationId == guid);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public List<Location> GetLocations()
        {
            try
            {
                return _sqlConnection.Table<Location>().Where(p => !p.isDeleted).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public List<Location> GetAllByDownloadedByLocations(int downloadedBy)
        {
            try
            {
                return _sqlConnection.Table<Location>().Where(p => p.DownloadedBy == downloadedBy).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<Location> locationList)
        {
            try
            {
                _sqlConnection.InsertAll(locationList, typeof(Location));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(Location _location)
        {
            try
            {
                var localLocation = _sqlConnection.Table<Location>().ToList().FirstOrDefault(p => p.LocationId == _location.LocationId);
                if (localLocation == null)
                {
                    _sqlConnection.Insert(_location);
                }
                else
                {
                    _sqlConnection.Update(_location);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<Location> locationList)
        {
            try
            {
                _sqlConnection.UpdateAll(locationList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<Location> GetLocationTree()
        {
            var userId = Convert.ToInt32(Application.Current.Properties["UserID"].ToString());
            var locationTree = default(List<Location>);
            try
            {
                locationTree = _sqlConnection.Table<Location>().Where(l => l.UserId == userId && !l.isDeleted).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return locationTree;
        }
        public void DeleteAllLocations()
        {
            var userId = Convert.ToInt32(Application.Current.Properties["UserID"].ToString());
            try
            {
                var query = "Delete from Location where UserId=@userId";
                _sqlConnection.Execute(query, userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void DeleteByDownloadedBy(int downloadedby)
        {
            var query = "Delete from Location where DownloadedBy =" + downloadedby;
            _sqlConnection.Execute(query);
        }
    }
}
