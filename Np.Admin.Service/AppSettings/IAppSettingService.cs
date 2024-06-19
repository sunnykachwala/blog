namespace Np.Admin.Service.AppSettings
{
    using Np.ViewModel;
    public interface IAppSettingService
    {
        Task<AppSettingDto> GetAppSetting();
    }
}
