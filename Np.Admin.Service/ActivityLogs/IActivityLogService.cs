
namespace Np.Admin.Service.ActivityLogs
{
    using Np.Admin.Service.ActivityLogs.Model;

    public interface IActivityLogService
    {
        Task<List<ActivityLogDto>> GetActivityLogData();
        Task<List<AuditLogDto>> GetActivityDetail(int activityId);
        int CreateActivityLog(CreateActivityLogDto activityLog, Guid modifiedBy);
        string GetUserIpAddress();
    }
}
