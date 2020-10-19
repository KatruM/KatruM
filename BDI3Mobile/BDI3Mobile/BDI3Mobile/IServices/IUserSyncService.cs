using BDI3Mobile.Models.Common;

namespace BDI3Mobile.IServices
{
    public interface IUserSyncService
    {
        void InsertUserSync(UserSyncTable userSyncTable);
        void UpdateUserSync(UserSyncTable userSyncTable);
        void DeleteUserSync(int downloadedBy);
        UserSyncTable GetUserSyncTable(int downloadedBy);
        void Insert(ContentSyncData contentSyncData);
        void Update(ContentSyncData contentSyncData);
        ContentSyncData GetContentSyncData(string contentType);
        void DeleteSyncData();
    }
}
