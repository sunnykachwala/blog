namespace Np.Admin.Service.ActivityLogs
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Np.Admin.Service.ActivityLogs.Model;
    using Np.Common;
    using Np.DAL.Domain;
    using Np.DAL.Repository;
    using System.Reflection;
    using System.Transactions;
    public class ActivityLogService : IActivityLogService

    {
        private readonly IBaseRepository<ActivityLog> activylogRepo;
        private readonly IBaseRepository<AuditLog> auditLogRepo;
        private readonly IMapper mapper;
        public ActivityLogService(IBaseRepository<ActivityLog> activylogRepo,
            IBaseRepository<AuditLog> auditLogRepo,
            IMapper mapper)
        {
            this.activylogRepo = activylogRepo;
            this.auditLogRepo = auditLogRepo;
            this.mapper = mapper;
        }

        public IQueryable<ActivityLog> GetActivityLog()
        {
            var result = this.activylogRepo.GetAllCustom();
            return result;
        }

        public int CreateActivityLog(CreateActivityLogDto activityLog, Guid modifiedBy)
        {
            using (var scope = new TransactionScope())
            {
                try
                {
                    var activity = this.mapper.Map<ActivityLog>(activityLog);
                    activity.ModifiedBy = modifiedBy;
                    activity.IpAddress = "127.0.0.1";
                    this.activylogRepo.Insert(activity);
                    this.activylogRepo.Save();
                    foreach (var auditLog in activityLog.AuditLog)
                    {
                        var audit = this.mapper.Map<AuditLog>(auditLog);
                        audit.ModifiedBy = modifiedBy;
                        audit.ActivityLogId = activity.ActivityLogId;
                        this.auditLogRepo.Insert(audit);
                    }
                    this.auditLogRepo.Save();
                    scope.Complete();
                    return activity.ActivityLogId;
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        public async Task<List<ActivityLogDto>> GetActivityLogData()
        {
            var activity = await this.GetActivityLog()
                .Select(x => new ActivityLogDto()
                {
                    ActivityLogId = x.ActivityLogId,
                    ActivityLogName = x.ActivityLogName,
                    LogType = EnumHelper.GetEnumValue<ActivityLogType>((int)x.LogType).ToString(),
                    EntityType = EnumHelper.GetEnumDescription(x.EntityType).ToString(),
                    PrimaryKeyValue = x.PrimaryKeyValue,
                    ModifiedAt = x.ModifiedAt.HasValue ? x.ModifiedAt.Value : x.CreatedAt,
                    ModifiedBy = x.ModifiedBy.HasValue ? x.ModifiedBy.Value : x.CreatedBy
                })
               .OrderByDescending(x => x.ModifiedAt)
               .ToListAsync();

            return activity;
        }

        public async Task<List<AuditLogDto>> GetActivityDetail(int activityId)
        {
            var activity = await this.auditLogRepo.GetAllCustom().Where(x => x.ActivityLogId == activityId)
                .Select(x => new AuditLogDto()
                {
                    ActivityLogId = x.ActivityLogId,
                    KeyName = x.KeyName,
                    NewValues = x.NewValues,
                    OldValue = x.OldValue,
                })
               .ToListAsync();

            return activity;
        }

        private List<AuditedDataDto> GetUpdatedPropertiesData<T>(T oldObject, T newObject)
        {
            List<string> propertyNames = new List<string>() { "SystemUser", "AppName", "ModifiedAt", "Richmond", "MedicalEvent", "MedicalEventAdditionalData", "EventDetail", "DairyCard" };

            var updatedProperties = new List<AuditedDataDto>();
            var properties = oldObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(IgnoreForAuditAttribute)))
                {
                    continue;
                }
                var oldValue = property.GetValue(oldObject);
                var newValue = property.GetValue(newObject);


                if (!Equals(oldValue, newValue) && !propertyNames.Contains(property.Name))
                {
                    updatedProperties.Add(new AuditedDataDto()
                    {
                        Key = property.Name,
                        OldValue = oldValue != null ? oldValue.ToString() : "",
                        NewValue = newValue != null ? newValue.ToString() : "",
                    });
                }
            }

            return updatedProperties;
        }
    }
}
