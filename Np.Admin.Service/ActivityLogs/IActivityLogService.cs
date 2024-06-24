using Np.Common;

namespace Np.Admin.Service.ActivityLogs
{
    public interface IActivityLogService
    {
        int CreateActivityLog(string activityLogName, ActivityLogType logType, Guid modifiedBy);
    }
}
