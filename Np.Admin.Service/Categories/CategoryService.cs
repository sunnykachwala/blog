namespace Np.Admin.Service.Categories
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Np.Admin.Service.ActivityLogs;
    using Np.Admin.Service.ActivityLogs.Model;
    using Np.Admin.Service.Categories.Model;
    using Np.Admin.Service.UrlRecords;
    using Np.Common;
    using Np.DAL.Domain;
    using Np.DAL.Repository;
    using Np.ViewModel;
    using System.Transactions;

    public class CategoryService : ICategoryService
    {
        private readonly IBaseRepository<Category> categoryRepo;
        private readonly IUrlRecordService urlRecordService;
        private readonly IMapper mapper;
        private readonly IActivityLogService activityLogService;
        private readonly IMemoryCache cache;
        public CategoryService(IBaseRepository<Category> categoryRepo,
            IUrlRecordService urlRecordService,
            IMapper mapper,
            IActivityLogService activityLogService,
             IMemoryCache cache)
        {
            this.categoryRepo = categoryRepo;
            this.urlRecordService = urlRecordService;
            this.mapper = mapper;
            this.activityLogService = activityLogService;
            this.cache = cache;
        }

        public async Task<Guid> Add(CreateCategoryDto model, Guid modifiedBy)
        {
            var categoryInfo = await this.categoryRepo.GetFindByColumnAsync(x => x.Title == model.Title);

            if (categoryInfo != null)
                throw new Exception($"Category with Title {model.Title} already exists!");

            string slug = model.Slug.ToUrlSlug().ToLower();
            while (!await this.urlRecordService.IsSlugUnique(slug))
            {
                string randomCode = UtilityHelper.GenerateRandomCode();
                slug = $"{randomCode}-{slug.ToLower()}";
            }

            var category = mapper.Map<Category>(model);
            category.Id = Guid.NewGuid();
            category.CreatedBy = modifiedBy;
            category.Slug = slug;

            var activityId = this.activityLogService.CreateActivityLog(new CreateActivityLogDto()
            {
                ActivityLogName = "Create Category",
                EntityType = EntityTypes.Category,
                LogType = ActivityLogType.Create,
                PrimaryKeyValue = category.Id.ToString(),
                AuditLog = new List<CreateAuditLogDto>()
            }, modifiedBy);

            var urlRecord = new CreateUrlReordDto()
            {
                EntityId = category.Id,
                EntityType = (byte)UrlEntityType.Category,
                IsActive = true,
                Slug = slug,
            };

            using (var scope = new TransactionScope())
            {
                try
                {
                    this.categoryRepo.Insert(category);
                    this.urlRecordService.AddUrlRecord(urlRecord, modifiedBy);
                    this.categoryRepo.SaveAudited(modifiedBy, activityId);
                    this.RemoveCategoryCache();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }

            return category.Id;
        }

        public async Task Update(CategoryDto model, Guid modifiedBy)
        {
            var category = await this.categoryRepo.GetFindByColumnAsync(x => x.Id == model.Id) ?? throw new Exception($"Category does not exist with the provided id {model.Id}");

            var categoryMapped = this.mapper.Map<Category>(model);
            categoryMapped.ModifiedBy = modifiedBy;
            categoryMapped.ModifiedAt = DateTime.UtcNow;
            categoryMapped.CreatedBy = category.CreatedBy;
            categoryMapped.CreatedAt = category.CreatedAt;
            categoryMapped.Slug = category.Slug;
          
            var updatedProperties = UtilityHelper.GetUpdatedPropertiesData(category, categoryMapped);

            var activityId = this.activityLogService.CreateActivityLog(new CreateActivityLogDto()
            {
                ActivityLogName = "Update Category",
                EntityType = EntityTypes.Category,
                LogType = ActivityLogType.Update,
                PrimaryKeyValue = category.Id.ToString(),
                AuditLog = updatedProperties.Select(x => new CreateAuditLogDto() { KeyName = x.Key, NewValues = x.NewValue, OldValue = x.OldValue }).ToList()
            }, modifiedBy);

            this.categoryRepo.Edit(categoryMapped);
            this.categoryRepo.SaveAudited(modifiedBy, activityId);
            this.RemoveCategoryCache();
        }
        public async Task<List<CategoryDto>?> GetAllCachedParentCategory(FilterDto filter)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);
            string key = nameof(GetAllCachedParentCategory);
            if (!cache.TryGetValue(key, out List<CategoryDto>? list))
            {
                list = await GetCategory()
                    .Where(i => i.IsActive && !i.ParentCategoryId.HasValue
                      && (string.IsNullOrWhiteSpace(filter.Search) || i.Title.Contains(filter.Search)))
                     .OrderBy(x => x.DisplayOrder)
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

        public async Task<List<CategoryDto>?> AllCachedCategory(FilterDto filter)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);
            string key = nameof(AllCachedCategory);
            if (!cache.TryGetValue(key, out List<CategoryDto>? list))
            {
                list = await GetCategory()
                    .Where(i => i.IsActive
                      && (string.IsNullOrWhiteSpace(filter.Search) || i.Title.Contains(filter.Search)))
                     .OrderBy(x => x.DisplayOrder)
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

        public IQueryable<CategoryDto> GetCategory()
        {
            var result = this.categoryRepo.GetAllCustom();
            var mappedResult = mapper.ProjectTo<CategoryDto>(result);
            return mappedResult;
        }

        public void RemoveParentCategoryCache()
        {
            string key = nameof(GetAllCachedParentCategory);
            cache.Remove(key);
        }

        public void RemoveCategoryCache()
        {
            string key = nameof(AllCachedCategory);
            cache.Remove(key);
        }
    }
}
