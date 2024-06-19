namespace Np.Admin.Service.AppSettings
{
    using AutoMapper;
    using Np.DAL.Domain;
    using Np.DAL.Repository;
    using Np.ViewModel;
    public class AppSettingService : IAppSettingService
    {
        private readonly IBaseRepository<AppSetting> appSettingRepo;
        private readonly IMapper mapper;
        public AppSettingService(IBaseRepository<AppSetting> appSettingRepo,
            IMapper mapper)
        {
            this.appSettingRepo = appSettingRepo;
            this.mapper = mapper;
        }

        public async Task<AppSettingDto> GetAppSetting()
        {
            var applicationId = Guid.Parse("687EB657-26F7-4876-ADF0-EF81E03BD3EF");
            var appSetting = await this.appSettingRepo.GetByIdAsync(applicationId);
            var result = this.mapper.Map<AppSettingDto>(appSetting);
            return result;
        }

    }
}
