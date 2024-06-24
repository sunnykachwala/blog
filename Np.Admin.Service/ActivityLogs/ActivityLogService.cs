using Np.Common;
using Np.DAL.Domain;
using Np.DAL.Repository;

namespace Np.Admin.Service.ActivityLogs
{
    public class ActivityLogService : IActivityLogService

    {
        private readonly IBaseRepository<ActivityLog> activylogRepo;

        public ActivityLogService(IBaseRepository<ActivityLog> activylogRepo)
        {
            this.activylogRepo = activylogRepo;
        }

        public int CreateActivityLog(string activityLogName, ActivityLogType logType, Guid modifiedBy)
        {
            var activity = new ActivityLog()
            {
                ActivityLogName = activityLogName,
                LogType = logType,
                CreatedBy = modifiedBy
            };
            this.activylogRepo.Insert(activity);
            this.activylogRepo.Save();
            return activity.ActivityLogId;
        }
    }
}
