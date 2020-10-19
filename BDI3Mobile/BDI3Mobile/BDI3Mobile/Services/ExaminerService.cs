using BDI3Mobile.IServices;
using BDI3Mobile.Models.Responses;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public  class ExaminerService : IExaminerService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public ExaminerService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<SearchStaffResponse>();
        }
        public void DeleteAll()
        {
            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
            try
            {
                var query = "Delete from SearchStaffResponse where OrganizationId=@orgId";
                _sqlConnection.Execute(query, orgId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<SearchStaffResponse> GetExamainer()
        {
            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
            var userid = Convert.ToInt32(Application.Current.Properties["UserID"].ToString());
            try
            {
                return _sqlConnection.Table<SearchStaffResponse>().Where(s=>s.DownloadedBy == userid).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<SearchStaffResponse> examinerlist)
        {
            try
            {
                _sqlConnection.InsertAll(examinerlist, typeof(SearchStaffResponse));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void DeleteByDownloadedBy(int downloadedby)
        {
            var query = "Delete from SearchStaffResponse where DownloadedBy =" + downloadedby;
            _sqlConnection.Execute(query);
        }
    }
}
