namespace Np.Admin.Service.LoginHistory
{
    using Np.ViewModel;
    public interface ILoginHistoryService
    {
        void ResetLoginHistory(Guid userId, Guid modifiedBy);
        void CreateLoginHistory(LoginResetHistoryDto loginResetHistoryDto, Guid modifiedBy);
        Task<LoginHistoryDto?> GetLoginHistory(Guid userId, bool isActive, string resetType);
    }
}