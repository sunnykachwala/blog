namespace Np.Admin.Service.Tags
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Np.Admin.Service.ActivityLogs;
    using Np.Admin.Service.UrlRecords;
    using Np.Common;
    using Np.DAL.Domain;
    using Np.DAL.Repository;
    using Np.ViewModel;
    using System.Transactions;

    public class TagService : ITagService
    {
        private readonly IBaseRepository<Tag> tagRepo;
        private readonly IUrlRecordService urlRecordService;
        private readonly IMapper mapper;
        private readonly IActivityLogService activityLogService;
        private readonly IMemoryCache cache;

        public TagService(IBaseRepository<Tag> tagRepo,
            IUrlRecordService urlRecordService,
            IMapper mapper,
            IActivityLogService activityLogService,
            IMemoryCache cache)
        {
            this.tagRepo = tagRepo;
            this.urlRecordService = urlRecordService;
            this.activityLogService = activityLogService;
            this.cache = cache;
            this.mapper = mapper;
        }

        public async Task<Guid> Add(CreateTagDto model, Guid modifiedBy)
        {
            var tagInfo = await this.tagRepo.GetFindByColumnAsync(x => x.TagName == model.TagName);

            if (tagInfo != null)
                throw new Exception($"Tag with Name {model.TagName} already exists!");

            var activityId = this.activityLogService.CreateActivityLog("Tag Created", ActivityLogType.Create, modifiedBy);

            string slug = model.Slug.ToUrlSlug().ToLower();
            while (!await this.urlRecordService.IsSlugUnique(slug))
            {
                string randomCode = UtilityHelper.GenerateRandomCode();
                slug = $"{randomCode}-{slug.ToLower()}";
            }

            var tag = mapper.Map<Tag>(model);
            tag.Id = Guid.NewGuid();
            tag.CreatedBy = modifiedBy;
            tag.Slug = slug;

            var urlRecord = new CreateUrlReordDto()
            {
                EntityId = tag.Id,
                EntityType = (byte)UrlEntityType.Tag,
                IsActive = true,
                Slug = slug,
            };
            using (var scope = new TransactionScope())
            {
                try
                {
                    this.tagRepo.Insert(tag);
                    this.urlRecordService.AddUrlRecord(urlRecord, modifiedBy);
                    this.tagRepo.SaveAudited(modifiedBy, activityId);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }

            return tag.Id;
        }

        public async Task Update(TagDto model, Guid modifiedBy)
        {
            var tag = await this.tagRepo.GetFindByColumnAsync(x => x.Id == model.Id) ?? throw new Exception($"Tag does not exist with the provided id {model.Id}");

            var tagMapped = this.mapper.Map<Tag>(model);
            tagMapped.ModifiedBy = modifiedBy;
            tagMapped.ModifiedAt = DateTime.UtcNow;
            tagMapped.CreatedBy = tag.CreatedBy;
            tagMapped.CreatedAt = tag.CreatedAt;
            tagMapped.Slug = tag.Slug;
            var activityId = this.activityLogService.CreateActivityLog("Tag Updated ", ActivityLogType.Update, modifiedBy);

            this.tagRepo.Edit(tagMapped);
            this.tagRepo.SaveAudited(modifiedBy, activityId);
        }

        public async Task<List<TagDto>?> AllCachedTag(FilterDto filter)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);
            string key = nameof(AllCachedTag);
            if (!cache.TryGetValue(key, out List<TagDto>? list))
            {
                list = await GetTag()
                    .Where(i => i.IsActive
                      && (string.IsNullOrWhiteSpace(filter.Search) || i.TagName.Contains(filter.Search)))
                     .OrderBy(x => x.DispalyOrder)
                     .Skip(skip)
                     .Take(filter.PageSize)
                     .ToListAsync();

                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
                };

                cache.Set(key, list, cacheEntryOptions);
            }
            return list;
        }

        public IQueryable<TagDto> GetTag()
        {
            var result = this.tagRepo.GetAllCustom();
            var mappedResult = mapper.ProjectTo<TagDto>(result);
            return mappedResult;
        }
        public void RemoveTagCache()
        {
            string key = nameof(AllCachedTag);
            cache.Remove(key);
        }
    }
}
