using System;
using System.Collections.Generic;
using System.Linq;
using BDI3Mobile.Models.DBModels;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.IServices
{
    public class ProductResearchCodesService : IProductResearchCodesService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public ProductResearchCodesService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ProductResearchCodes>();
        }

        public List<ProductResearchCodeValues> GetProductResearchCodes()
        {
            try
            {
                return _sqlConnection.Table<ProductResearchCodeValues>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<ProductResearchCodes> productResearchCodes)
        {
            try
            {
                _sqlConnection.InsertAll(productResearchCodes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(ProductResearchCodes productResearchCodes)
        {
            try
            {
                var localreseachCode = _sqlConnection.Table<ProductResearchCodes>().ToList().FirstOrDefault(p => p.ResearchCodeId == productResearchCodes.ResearchCodeId);
                if (localreseachCode == null)
                {
                    _sqlConnection.Insert(localreseachCode);
                }
                else
                {
                    _sqlConnection.Update(productResearchCodes);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<ProductResearchCodes> productResearchCodes)
        {
            try
            {
                _sqlConnection.UpdateAll(productResearchCodes);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public List<ProductResearchCodes> GetResearchCodesByOrg(int OrganizationId)
        {
            try
            {
                return _sqlConnection.Table<ProductResearchCodes>().Where(p => p.OrganizationId == OrganizationId).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }
        public void DeleteAll(int orgID)
        {
            try
            {
                var sqlQuery = "delete from ProductResearchCodes where OrganizationId = " + orgID;
                _sqlConnection.Execute(sqlQuery);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
