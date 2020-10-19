using System;
using System.Collections.Generic;
using System.Linq;
using BDI3Mobile.IServices;
using BDI3Mobile.Models.DBModels;
using SQLite;
using Xamarin.Forms;

namespace BDI3Mobile.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IDBConnection _dbconnection;
        private readonly SQLiteConnection _sqlConnection;
        public MembershipService()
        {
            _dbconnection = DependencyService.Get<IDBConnection>();
            _sqlConnection = _dbconnection.GetConnection();
            _sqlConnection.CreateTable<Membership>();
        }
        public Membership GetMembershipById(int id)
        {
            try
            {
                return _sqlConnection.Table<Membership>().ToList().FirstOrDefault(p => p.Id == id);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public List<Membership> GetMemberships()
        {
            try
            {
                return _sqlConnection.Table<Membership>().ToList();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }

        public void InsertAll(List<Membership> membershipList)
        {
            try
            {
                _sqlConnection.InsertAll(membershipList, typeof(Membership));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void InsertorUpdate(Membership membership)
        {
            try
            {
                var localMembership = _sqlConnection.Table<Membership>().ToList().FirstOrDefault(p => p.UserId == membership.UserId);
                if (localMembership == null)
                {
                    _sqlConnection.Insert(membership);
                }
                else
                {
                    _sqlConnection.Update(membership);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateAll(List<Membership> membershipList)
        {
            try
            {
                _sqlConnection.UpdateAll(membershipList);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }
    }
}
