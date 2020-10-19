using BDI3Mobile.IServices;
using BDI3Mobile.Models.Responses;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class ProgramNoteService : IProgramNoteService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public ProgramNoteService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ProgramNoteModel>();
        }
        public void DeleteAll()
        {
            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
            try
            {
                var query = "Delete from ProgramNoteModel where OrganizationId=@orgId";
                _sqlConnection.Execute(query, orgId);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<ProgramNoteModel> GetProgramNote()
        {
            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
            var userid = Convert.ToInt32(Application.Current.Properties["UserID"].ToString());
            try
            {
                return _sqlConnection.Table<ProgramNoteModel>().Where(p=>p.DownLoadedBy == userid).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<ProgramNoteModel> programNotelist)
        {
            try
            {
                _sqlConnection.InsertAll(programNotelist, typeof(ProgramNoteModel));
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void DeleteByDownloadedBy(int downloadedby)
        {
            var query = "Delete from ProgramNoteModel where DownLoadedBy =" + downloadedby;
            _sqlConnection.Execute(query);
        }
    }
}
