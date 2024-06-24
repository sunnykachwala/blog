using Np.DAL.Domain;
using Np.DAL.Repository;

namespace Np.Admin.Service.LoginHistory
{
    using Np.ViewModel;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    public class LoginHistoryService : ILoginHistoryService
    {
        private readonly IBaseRepository<LoginResetHistory> loginHistoryRepo;
        private readonly IBaseRepository<AdminUser> userRepo;
        private readonly IMapper mapper;
        public LoginHistoryService(
            IBaseRepository<LoginResetHistory> loginHistoryRepo,
            IBaseRepository<AdminUser> userRepo,
            IMapper mapper)
        {
            this.loginHistoryRepo = loginHistoryRepo;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }

        public void ResetLoginHistory(Guid userId, Guid modifiedBy)
        {
            var loginHistory = this.loginHistoryRepo.GetAll().Where(x => x.LoginGuid == userId && x.IsActive).ToList();
            foreach (var login in loginHistory)
            {
                login.IsActive = false;
                login.ModifiedBy = modifiedBy;
                this.loginHistoryRepo.Edit(login);
            }
            this.loginHistoryRepo.Save();
        }

        public void CreateLoginHistory(LoginResetHistoryDto loginResetHistoryDto, Guid modifiedBy)
        {
            var history = mapper.Map<LoginResetHistory>(loginResetHistoryDto);
            history.LoginResetHistoryGuid = Guid.NewGuid();
            history.CreatedBy = modifiedBy;
            this.loginHistoryRepo.Insert(history);
            this.loginHistoryRepo.Save();
        }

        public async Task<LoginHistoryDto?> GetLoginHistory(Guid userId, bool isActive, string resetType)
        {
            var history = await (from h in this.loginHistoryRepo.GetAllCustom().Where(x => x.LoginGuid == userId && x.IsActive == isActive && x.ResetType == resetType)
                                 join u in this.userRepo.GetAllCustom() on h.LoginGuid equals u.UserGuid
                                 select new LoginHistoryDto()
                                 {
                                     LoginGuid = h.LoginGuid,
                                     IsActive = h.IsActive,
                                     ResetType = h.ResetType,
                                     ResetExpiryTime = h.ResetExpiryTime,
                                     Salt = h.Salt,
                                     HashedConfirmationCode = h.HashedConfirmationCode,
                                     UserEmail = u.UserEmail,
                                     OriginalSalt = u.Salt
                                 })
                                 .SingleOrDefaultAsync();

            return history;
        }
    }
}
