using BDI3Mobile.IServices;
using BDI3Mobile.Models.Responses;
using SQLite;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class OrgRecordFormService : IOrgRecordFormService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;

        public OrgRecordFormService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<OrganizationRecordForms>();
        }
        public List<OrganizationRecordForms> GetRecordForms()
        {
            var orgId = Convert.ToInt32(Application.Current.Properties["OrgnazationID"].ToString());
            try
            {
                return _sqlConnection.Table<OrganizationRecordForms>().Where(forms=>forms.OrganizationId == orgId).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void Insert(List<OrganizationRecordForms> organizationRecordForms)
        {
            try
            {
                _sqlConnection.InsertAll(organizationRecordForms, typeof(OrganizationRecordForms));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteCurrentOrgRecordForm(int orgId)
        {
            try
            {
                _sqlConnection.Table<OrganizationRecordForms>().Where(form => form.OrganizationId == orgId).Delete();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
        public void DeleteAll()
        {
            try
            {
                _sqlConnection.DeleteAll<OrganizationRecordForms>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
