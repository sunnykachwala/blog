namespace Np.Admin.Service.UrlRecords
{
    using AutoMapper;
    using Np.Admin.Service.ActivityLogs;
    using Np.Admin.Service.ActivityLogs.Model;
    using Np.Common;
    using Np.DAL.Domain;
    using Np.DAL.Repository;
    using Np.ViewModel;

    public class UrlRecordService : IUrlRecordService
    {
        private readonly IBaseRepository<UrlRecord> urlRecordRepo;
        private readonly IMapper mapper;
        private readonly IActivityLogService activityLogService;
        public UrlRecordService(IBaseRepository<UrlRecord> urlRecordRepo,
            IMapper mapper,
            IActivityLogService activityLogService)
        {
            this.urlRecordRepo = urlRecordRepo;
            this.mapper = mapper;
            this.activityLogService = activityLogService;
        }

        public async Task<bool> IsSlugUnique(string slug)
        {
            var urlRecordExist = await urlRecordRepo.GetFindByColumnAsync(x => x.Slug.ToLower() == slug.ToLower());
            return urlRecordExist == null;
        }

        public Guid AddUrlRecord(CreateUrlReordDto model, Guid modifiedBy)
        {
            var urlRecord = new UrlRecord()
            {
                Slug = model.Slug,
                EntityType = (UrlEntityType)model.EntityType,
                EntityId = model.EntityId,
                IsActive = model.IsActive,
                Id = Guid.NewGuid()
            };
            var activityId = this.activityLogService.CreateActivityLog(new CreateActivityLogDto()
            {
                ActivityLogName = "Create Url Record",
                EntityType = EntityTypes.UrlRecord,
                LogType = ActivityLogType.Create,
                PrimaryKeyValue = urlRecord.Id.ToString(),
                AuditLog = new List<CreateAuditLogDto>()
            }, modifiedBy);
           
            urlRecordRepo.Insert(urlRecord);
            urlRecordRepo.SaveAudited(modifiedBy, activityId);
            return urlRecord.Id;
        }



    }
}
