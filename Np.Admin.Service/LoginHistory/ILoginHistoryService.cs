namespace Np.Admin.Service.LoginHistory
{
    using Np.ViewModel;
    public interface ILoginHistoryService
    {
        void ResetLoginHistory(Guid userId, string modifiedBy);
        void CreateLoginHistory(LoginResetHistoryDto loginResetHistoryDto, string modifiedBy);
        Task<LoginHistoryDto?> GetLoginHistory(Guid userId, bool isActive, string resetType);
    }
}