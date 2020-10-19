using System;
using System.Collections.Generic;
using System.Linq;
using BDI3Mobile.Models.DBModels;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.IServices
{
    public class ProductResearchCodeValuesService : IProductResearchCodeValuesService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public ProductResearchCodeValuesService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<ProductResearchCodeValues>();
        }

        public List<ProductResearchCodeValues> GetProductResearchCodes(int offlinesstudentId)
        {
            try
            {
                return _sqlConnection.Table<ProductResearchCodeValues>().Where(p => p.OfflineStudentID == offlinesstudentId).ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<ProductResearchCodeValues> productResearchCodeValues)
        {
            try
            {
                _sqlConnection.InsertAll(productResearchCodeValues);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(ProductResearchCodeValues productResearchCodeValues)
        {
            try
            {
                var localreseachCode = _sqlConnection.Table<ProductResearchCodeValues>().ToList().FirstOrDefault(p => p.ResearchCodeValueId == productResearchCodeValues.ResearchCodeValueId);
                if (localreseachCode == null)
                {
                    _sqlConnection.Insert(localreseachCode);
                }
                else
                {
                    _sqlConnection.Update(productResearchCodeValues);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<ProductResearchCodeValues> productResearchCodeValues)
        {
            try
            {
                _sqlConnection.UpdateAll(productResearchCodeValues);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteByStudentId(int offlinestudenId)
        {
            try
            {

                var deletequery = "delete from ProductResearchCodeValues where OfflineStudentID = " + offlinestudenId;
                _sqlConnection.Execute(deletequery);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
