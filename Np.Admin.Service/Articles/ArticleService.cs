namespace Np.Admin.Service.Articles
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

    public class ArticleService : IArticleService
    {
        private readonly IBaseRepository<Article> articleRepo;
        private readonly IBaseRepository<ArticleCategory> articleCategoryRepo;
        private readonly IBaseRepository<ArticleTag> articleTagRepo;
        private readonly IUrlRecordService urlRecordService;
        private readonly IMapper mapper;
        private readonly IActivityLogService activityLogService;

        public ArticleService(IBaseRepository<Article> articleRepo,
              IBaseRepository<ArticleCategory> articleCategoryRepo,
              IBaseRepository<ArticleTag> articleTagRepo,
              IUrlRecordService urlRecordService,
              IMapper mapper,
              IActivityLogService activityLogService
             )
        {
            this.articleRepo = articleRepo;
            this.articleTagRepo = articleTagRepo;
            this.articleCategoryRepo = articleCategoryRepo;
            this.urlRecordService = urlRecordService;
            this.mapper = mapper;
            this.activityLogService = activityLogService;
        }

        public async Task<Guid> Add(CreateArticleDto model, string ipAddress, Guid modifiedBy)
        {
            var activityId = this.activityLogService.CreateActivityLog("Create Article", ActivityLogType.Create, modifiedBy);

            string slug = model.Slug.ToUrlSlug().ToLower();
            while (!await this.urlRecordService.IsSlugUnique(slug))
            {
                string randomCode = UtilityHelper.GenerateRandomCode();
                slug = $"{randomCode}-{slug.ToLower()}";
            }

            var article = mapper.Map<Article>(model);
            article.ArticleId = Guid.NewGuid();
            article.CreatedBy = modifiedBy;
            article.Slug = slug;
            article.IpAddress = ipAddress;
            var urlRecord = new CreateUrlReordDto()
            {
                EntityId = article.ArticleId,
                EntityType = (byte)UrlEntityType.NewsArticle,
                IsActive = true,
                Slug = slug,
            };

            using (var scope = new TransactionScope())
            {
                try
                {
                    this.articleRepo.Insert(article);
                    this.urlRecordService.AddUrlRecord(urlRecord, modifiedBy);

                    #region Article Category
                    foreach (var category in model.SelectedCategories)
                    {
                        this.articleCategoryRepo.Insert(new ArticleCategory()
                        {
                            ArticleId = article.ArticleId,
                            CategoryId = category
                        });
                    }
                    #endregion

                    #region Article Tag
                    foreach (var tag in model.SelectedTags)
                    {
                        this.articleTagRepo.Insert(new ArticleTag()
                        {
                            ArticleId = article.ArticleId,
                            TagId = tag
                        });
                    }
                    #endregion

                    this.articleRepo.SaveAudited(modifiedBy, activityId);
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    scope.Dispose();
                    throw ex;
                }
            }

            return article.ArticleId;
        }


        public async Task<List<ArticleDto>?> AllArticle(FilterDto filter)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);

            var list = await GetArticle()
                   .Where(i => (string.IsNullOrWhiteSpace(filter.Search) || i.Title.Contains(filter.Search)))
                  .OrderBy(x => x.DispalyOrder)
                  .Skip(skip)
                  .Take(filter.PageSize)
                  .ToListAsync();
            return list;
        }

        public async Task<List<ArticleDto>?> AllArticle(ArticleFilterDto filter)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);

            var articleIdList = this.articleCategoryRepo.GetAllCustom().Where(x => filter.CategoryId.Count() > 0 && filter.CategoryId.Contains(x.CategoryId)).Select(x => x.ArticleId)
                .Union(this.articleTagRepo.GetAllCustom().Where(x => filter.TagId.Count() > 0 && filter.TagId.Contains(x.TagId)).Select(x => x.ArticleId)
                ).Distinct().ToList();

            var list = await GetArticle()
                   .Where(i => (string.IsNullOrWhiteSpace(filter.Search) || i.Title.Contains(filter.Search)
                   && (!articleIdList.Any() || articleIdList.Contains(i.ArticleId))
                   && filter.AuthorId.Contains(i.AuthorId)))

                  .OrderBy(x => x.DispalyOrder)
                  .Skip(skip)
                  .Take(filter.PageSize)
                  .ToListAsync();
            return list;
        }

        public async Task<List<ArticleDto>?> AllArticleByCateogryId(FilterDto filter, Guid categoryId)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);

            var list = await (from a in GetArticle().Where(i => (string.IsNullOrWhiteSpace(filter.Search) || i.Title.Contains(filter.Search)))
                              join ac in this.articleCategoryRepo.GetAllCustom() on a.ArticleId equals ac.ArticleId
                              orderby a.DispalyOrder
                              where ac.CategoryId == categoryId && a.IsPublished
                              select new ArticleDto()
                              {
                                  AuthorId = a.AuthorId,
                                  ArticleId = a.ArticleId,
                                  Content = a.Content,
                                  DefaultImage = a.DefaultImage,
                                  DispalyOrder = a.DispalyOrder,
                                  IsPublished = a.IsPublished,
                                  Keywords = a.Keywords,
                                  MetaDescription = a.MetaDescription,
                                  MetaTitle = a.MetaTitle,
                                  PublishedDate = a.PublishedDate,
                                  Slug = a.Slug,
                                  Title = a.Title
                              })
                  .Skip(skip)
                  .Take(filter.PageSize)
                  .ToListAsync();
            return list;
        }


        public async Task<List<ArticleDto>?> AllArticleByTagId(FilterDto filter, Guid tagId)
        {
            if (filter.PageSize > 100) filter.PageSize = 100;

            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);

            var list = await (from a in GetArticle().Where(i => (string.IsNullOrWhiteSpace(filter.Search) || i.Title.Contains(filter.Search)))
                              join ac in this.articleTagRepo.GetAllCustom() on a.ArticleId equals ac.ArticleId
                              orderby a.DispalyOrder
                              where ac.TagId == tagId && a.IsPublished
                              select new ArticleDto()
                              {
                                  AuthorId = a.AuthorId,
                                  ArticleId = a.ArticleId,
                                  Content = a.Content,
                                  DefaultImage = a.DefaultImage,
                                  DispalyOrder = a.DispalyOrder,
                                  IsPublished = a.IsPublished,
                                  Keywords = a.Keywords,
                                  MetaDescription = a.MetaDescription,
                                  MetaTitle = a.MetaTitle,
                                  PublishedDate = a.PublishedDate,
                                  Slug = a.Slug,
                                  Title = a.Title
                              })
                  .Skip(skip)
                  .Take(filter.PageSize)
                  .ToListAsync();
            return list;
        }

        public IQueryable<ArticleDto> GetArticle()
        {
            var result = this.articleRepo.GetAllCustom();
            var mappedResult = mapper.ProjectTo<ArticleDto>(result);
            return mappedResult;
        }
    }
}
