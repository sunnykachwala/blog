namespace Np.Admin.Service
{
    using AutoMapper;
    using Np.Admin.Service.ActivityLogs.Model;
    using Np.Admin.Service.Articles.Model;
    using Np.Admin.Service.Categories.Model;
    using Np.DAL.Domain;
    using Np.ViewModel;

    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ActivityLog, ActivityLogDto>()
            .ReverseMap();

            CreateMap<ActivityLog, CreateActivityLogDto>()
            .ReverseMap();
            
            CreateMap<AuditLog, CreateAuditLogDto>()
            .ReverseMap();

            CreateMap<AdminUser, AdminUserDto>()
            .ReverseMap();

            CreateMap<AdminUser, CreateUserDto>()
            .ReverseMap();

            CreateMap<AdminUserRole, AdminUserRoleDto>()
            .ReverseMap();

            CreateMap<Article, CreateArticleDto>()
            .ReverseMap();

            CreateMap<Article, ArticleDto>()
            .ReverseMap();

            CreateMap<AppSetting, AppSettingDto>()
            .ReverseMap();

            CreateMap<Category, CreateCategoryDto>()
            .ReverseMap();

            CreateMap<Category, CategoryDto>()
            .ReverseMap();

            CreateMap<LoginResetHistory, LoginResetHistoryDto>()
            .ReverseMap();

            CreateMap<Organisation, OrganisationDto>()
            .ReverseMap();

            CreateMap<RefreshToken, RefreshTokenDto>()
            .ReverseMap();

            CreateMap<Tag, CreateTagDto>()
            .ReverseMap();

            CreateMap<Tag, TagDto>()
            .ReverseMap();

            CreateMap<UrlRecord, CreateUrlReordDto>()
           .ReverseMap();

        }

    }
}