namespace Np.Admin.Service.AdminUsers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.EntityFrameworkCore;
    using Np.Admin.Service.ActivityLogs;
    using Np.Admin.Service.ActivityLogs.Model;
    using Np.Admin.Service.LoginHistory;
    using Np.Common;
    using Np.DAL;
    using Np.DAL.Domain;
    using Np.DAL.Repository;
    using Np.ViewModel;
    public class AdminUserService : IAdminUserService
    {
        private readonly IBaseRepository<AdminUser> userRepo;
        private readonly IBaseRepository<AdminUserRole> useRoleRepo;
        private readonly IBaseRepository<RefreshToken> refreshTokenRepo;
        private readonly IBaseRepository<Organisation> orgRepo;
        private readonly IBaseRepository<RolePermissionMapping> rolePermissionMappingRepo;
        private readonly IBaseRepository<UserPermission> userPermissionRepo;
        private readonly IActivityLogService activityLogService;
        private readonly ILoginHistoryService loginHistory;
        private readonly IMapper mapper;
        private readonly ISqlHelper sqlHelper;

        public AdminUserService(IBaseRepository<AdminUser> userRepo,
            IBaseRepository<AdminUserRole> useRoleRepo,
            IBaseRepository<RefreshToken> refreshTokenRepo,
            IBaseRepository<Organisation> orgRepo,
            IActivityLogService activityLogService,
            ILoginHistoryService loginHistory,
            IMapper mapper,
             ISqlHelper sqlHelper,
            IBaseRepository<RolePermissionMapping> rolePermissionMappingRepo,
            IBaseRepository<UserPermission> userPermissionRepo)
        {
            this.userRepo = userRepo;
            this.orgRepo = orgRepo;
            this.mapper = mapper;
            this.refreshTokenRepo = refreshTokenRepo;
            this.useRoleRepo = useRoleRepo;
            this.sqlHelper = sqlHelper;
            this.activityLogService = activityLogService;
            this.loginHistory = loginHistory;
            this.userPermissionRepo = userPermissionRepo;
            this.rolePermissionMappingRepo = rolePermissionMappingRepo;
        }
        public async Task<UserInfoWithRoleDto?> GetUserInfoByEmailWithRole(string email)
        {
            var userList = await (from u in this.userRepo.GetAllCustom()
                                  join ug in this.useRoleRepo.GetAllCustom() on u.UserRoleGuid equals ug.UserRoleGuid
                                  join o in this.orgRepo.GetAllCustom() on u.OrganisationGuid equals o.OrganisationGuid
                                  select new UserInfoWithRoleDto()
                                  {
                                      UserGuid = u.UserGuid,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      IsActive = u.IsActive,
                                      UserEmail = u.UserEmail,
                                      RoleName = ug.Name,
                                      UserRoleGuid = u.UserRoleGuid,
                                      TwofactorEnabled = u.TwofactorEnabled,
                                      UserPasswordHash = u.UserPasswordHash,
                                      Salt = u.Salt,
                                      EncryptedSecret = u.EncryptedSecret,
                                      OrganisationName = o.OrganisationName,
                                      OrganisationGuid = o.OrganisationGuid,
                                      DefaultHome = ug.DefaultHome,
                                  })
                                  .FirstOrDefaultAsync(x => x.IsActive && x.UserEmail == email);
            if (userList != null)
            {
                var permission = (from p in this.rolePermissionMappingRepo.GetAllCustom()
                                  join rp in this.userPermissionRepo.GetAllCustom() on p.UserPermissionId equals rp.UserPermissionId
                                  select rp.Permission).ToList();
                userList.Permissions = string.Join(", ", permission.Select(p => p));
            }

            return userList;
        }

        public async Task<bool> IsAuthenticatedExternalUser(UserInfoWithRoleDto userInfo, string password)
        {
            bool isAuthenticated = true;
            //Not AD user mean Exteranal user compare password with system entered password
            byte[] salt = userInfo.Salt;

            var hashedPassword = userInfo.UserPasswordHash;

            // Hash the user input password 
            string hashedInputPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            if (hashedInputPassword != hashedPassword)
            {
                await this.UpdateLoginAttempts(userInfo.UserEmail, Guid.NewGuid());
                isAuthenticated = false;
            }

            return isAuthenticated;
        }

        public async Task UpdateLoginAttempts(string email, Guid userName)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserEmail == email);
            if (userInfo != null)
            {
                var activityId = this.activityLogService.CreateActivityLog(new CreateActivityLogDto()
                {
                    ActivityLogName = "Update Login Attempts",
                    EntityType = EntityTypes.AdminUsers,
                    LogType = ActivityLogType.Update,
                    PrimaryKeyValue = userInfo.UserGuid.ToString(),
                    AuditLog = new List<CreateAuditLogDto>() { new CreateAuditLogDto() { KeyName = "LoginAttempts", NewValues = (userInfo.LoginAttempts + 1).ToString(), OldValue = userInfo.LoginAttempts.ToString() } }
                }, userName);
                userInfo.LoginAttempts += 1;

                userRepo.Edit(userInfo);
                userRepo.SaveAudited(userName, activityId);
            }
        }

        public async Task ResetLoginAttemps(string email, Guid userName)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserEmail == email);
            if (userInfo != null)
            {
                userInfo.LoginAttempts = 0;
               // var activityId = this.activityLogService.CreateActivityLog("Reset Login Attemps", ActivityLogType.Update, userName);
                var activityId = 1;
                userRepo.Edit(userInfo);
                userRepo.SaveAudited(userName, activityId);
                this.loginHistory.ResetLoginHistory(userInfo.UserGuid, userName);
            }
        }

        public void AddRefreshToken(RefreshTokenDto refreshToken)
        {
            var refreshTokenModel = mapper.Map<RefreshToken>(refreshToken);
            this.refreshTokenRepo.Insert(refreshTokenModel);
            this.refreshTokenRepo.Save();
        }

        public Guid UpdateRefreshToken(string token, RefreshTokenDto newToken, string ipAddress)
        {

            var refreshToken = this.refreshTokenRepo.GetFindBy(x => x.Token == token).First();

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newToken.Token;
            this.refreshTokenRepo.Edit(refreshToken);
            ;
            var newTokenModel = mapper.Map<RefreshToken>(newToken);

            newTokenModel.UserGuid = refreshToken.UserGuid;
            newTokenModel.ReplacedByToken = string.Empty;
            newTokenModel.RevokedByIp = string.Empty;
            this.refreshTokenRepo.Insert(newTokenModel);
            this.refreshTokenRepo.Save();
            return refreshToken.UserGuid;
        }

        public async Task<UserInfoWithRoleDto?> GetUserInfoByUserId(Guid userId)
        {
            var userList = await (from u in this.userRepo.GetAllCustom()
                                  join ug in this.useRoleRepo.GetAllCustom() on u.UserRoleGuid equals ug.UserRoleGuid
                                  join o in this.orgRepo.GetAllCustom() on u.OrganisationGuid equals o.OrganisationGuid
                                  select new UserInfoWithRoleDto()
                                  {
                                      UserGuid = u.UserGuid,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      IsActive = u.IsActive,
                                      UserEmail = u.UserEmail,
                                      RoleName = ug.Name,
                                      UserRoleGuid = u.UserRoleGuid,
                                      TwofactorEnabled = u.TwofactorEnabled,
                                      OrganisationName = o.OrganisationName,
                                      OrganisationGuid = o.OrganisationGuid,
                                      DefaultHome = ug.DefaultHome,
                                  }).FirstOrDefaultAsync(x => x.UserGuid == userId);
            if (userList != null)
            {
                var permission = (from p in this.rolePermissionMappingRepo.GetAllCustom()
                                  join rp in this.userPermissionRepo.GetAllCustom() on p.UserPermissionId equals rp.UserPermissionId
                                  select rp.Permission).ToList();
                userList.Permissions = string.Join(", ", permission.Select(p => p));
            }
            return userList;
        }

        public void RevokeToken(string refreshToken, string ipAddress)
        {
            var refreshTokenUpdate = this.refreshTokenRepo.GetFindBy(x => x.Token == refreshToken).First();

            var refreshTokenModel = mapper.Map<RefreshToken>(refreshTokenUpdate);
            refreshTokenModel.Revoked = DateTime.UtcNow;
            refreshTokenModel.RevokedByIp = ipAddress;
            this.refreshTokenRepo.Edit(refreshTokenModel);
            this.refreshTokenRepo.Save();
        }

        public async Task<UserInfoDto> GetUserInfo(Guid userId)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserGuid == userId);
            var user = mapper.Map<UserInfoDto>(userInfo);
            return user;
        }

        public async Task ConfirmRegistration(string hashedInputPassword, byte[] salt, Guid userGuid)
        {
            var user = await userRepo.GetByIdAsync(userGuid);
            user.IsConfirmedRegistration = true;
            user.UserPasswordHash = hashedInputPassword;
            this.userRepo.Edit(user);
            this.userRepo.Save();
        }

        public async Task<UserInfoDto> GetUserInfoByEmail(string email)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserEmail == email);
            var user = mapper.Map<UserInfoDto>(userInfo);
            return user;
        }

        public async Task AddTwoFactorAuthentication(string email, string encryptedSecret, bool isActive)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserEmail == email && x.IsActive == isActive);
            if (userInfo != null)
            {
                userInfo.EncryptedSecret = encryptedSecret;
                userRepo.Edit(userInfo);
                userRepo.Save();
            }
        }

        public async Task UpdateUser(UserInfoDto user)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserGuid != user.UserGuid && x.UserEmail == user.UserEmail);

            if (userInfo != null)
                throw new Exception($"User with email {user.UserEmail} already exists!");

            userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserGuid == user.UserGuid);

            if (userInfo == null)
                throw new Exception($"User does not exists!");

            var userData = mapper.Map<AdminUser>(user);
            userData.ModifiedBy = user.UserGuid;
            //var activityId = this.activityLogService.CreateActivityLog("Update User", ActivityLogType.Update, user.UserGuid);
            var activityId = 1;
            this.userRepo.Edit(userData);
            this.userRepo.SaveAudited(user.UserGuid, activityId);
        }

        public async Task<Guid> CreateUser(CreateUserDto userData, string confirmationCode, Guid modifiedBy)
        {
            byte[] salt = UtilityHelper.GenerateSalt();
            string randomPassword = UtilityHelper.GenerateSecurePassword(30);

            string hashedPassword = UtilityHelper.GenerateHashedPassword(salt, randomPassword);
            string hashedConfirmationCode = UtilityHelper.GenerateHashedConfirmationCode(confirmationCode, salt);

            try
            {
                var userExist = await this.UserEmailExists(userData.UserEmail);
                if (userExist)
                    throw new Exception($"User with email {userData.UserEmail} already exists.");

                //   var activityId = this.activityLogService.CreateActivityLog("Admin User Created", ActivityLogType.Create, modifiedBy);
                var activityId = 1;
                var user = mapper.Map<AdminUser>(userData);
                user.UserGuid = Guid.NewGuid();
                user.Salt = salt;
                user.HashedConformationCode = hashedConfirmationCode;
                user.UserPasswordHash = hashedPassword;
                user.ModifiedBy = modifiedBy;
                this.userRepo.Insert(user);
                this.userRepo.SaveAudited(modifiedBy, activityId);
                return user.UserGuid;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UserEmailExists(string email)
        {
            var userInfo = await this.userRepo.GetFindByColumnAsync(x => x.UserEmail == email);
            return userInfo != null;
        }

        public async Task<PaginatedResult<UserListingDto>> GetUsersByRole(FilterDto filter, string role)
        {
            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);

            var totalUser = (from u in this.userRepo.GetAllCustom().Where(x => x.IsActive == filter.IsActive)
                             join ug in this.useRoleRepo.GetAllCustom().Where(x => x.Name.Equals(role)) on u.UserRoleGuid equals ug.UserRoleGuid
                             join o in this.orgRepo.GetAllCustom() on u.OrganisationGuid equals o.OrganisationGuid
                             select new { u.IsActive }
                             ).Count(x => x.IsActive);

            var userList = await (from u in this.userRepo.GetAllCustom().Where(x => x.IsActive == filter.IsActive
                               && (string.IsNullOrWhiteSpace(filter.Search) ||
                                        x.FirstName.Contains(filter.Search) ||
                                        x.LastName.Contains(filter.Search) ||
                                               x.UserEmail.Contains(filter.Search)))
                                  join ug in this.useRoleRepo.GetAllCustom().Where(x => x.Name.Equals(role)) on u.UserRoleGuid equals ug.UserRoleGuid
                                  join o in this.orgRepo.GetAllCustom() on u.OrganisationGuid equals o.OrganisationGuid
                                  select new UserListingDto()
                                  {
                                      UserGuid = u.UserGuid,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      IsActive = u.IsActive,
                                      UserEmail = u.UserEmail,
                                      RoleName = ug.Name,
                                      OrganisationName = o.OrganisationName,
                                  }).Skip(skip).Take(filter.PageSize).ToListAsync();

            var result = new PaginatedResult<UserListingDto>()
            {
                TotalRecord = totalUser,
                List = userList,
            };

            return result;
        }
        public async Task<PaginatedResult<UserListingDto>> GetAllUser(UserFilterDto filter)
        {
            var skip = PageHelper.GetSkipCount(filter.Page, filter.PageSize);

            var totalUser = (from u in this.userRepo.GetAllCustom()
                             join ug in this.useRoleRepo.GetAllCustom() on u.UserRoleGuid equals ug.UserRoleGuid
                             join o in this.orgRepo.GetAllCustom() on u.OrganisationGuid equals o.OrganisationGuid
                             where ((filter.IsActive.HasValue ? u.IsActive == filter.IsActive : true)
                                  && (filter.OrganisationGuid.HasValue ? u.OrganisationGuid == filter.OrganisationGuid : true)
                                  && (filter.UserRoleGuid.HasValue ? u.UserRoleGuid == filter.UserRoleGuid : true))
                             select new { u.IsActive }
                             ).Count();

            var userList = await (from u in this.userRepo.GetAllCustom().Where(x => (string.IsNullOrWhiteSpace(filter.Search) || x.FirstName.Contains(filter.Search) || x.UserEmail.Contains(filter.Search)))
                                  join ug in this.useRoleRepo.GetAllCustom() on u.UserRoleGuid equals ug.UserRoleGuid
                                  join o in this.orgRepo.GetAllCustom() on u.OrganisationGuid equals o.OrganisationGuid
                                  where (
                                     (filter.IsActive.HasValue ? u.IsActive == filter.IsActive : true)
                                  && (filter.OrganisationGuid.HasValue ? u.OrganisationGuid == filter.OrganisationGuid : true)
                                  && (filter.UserRoleGuid.HasValue ? u.UserRoleGuid == filter.UserRoleGuid : true))
                                  select new UserListingDto()
                                  {
                                      UserGuid = u.UserGuid,
                                      FirstName = u.FirstName,
                                      LastName = u.LastName,
                                      IsActive = u.IsActive,
                                      UserEmail = u.UserEmail,
                                      RoleName = ug.Name,
                                      OrganisationName = o.OrganisationName,
                                  }).Skip(skip).Take(filter.PageSize).ToListAsync();

            var result = new PaginatedResult<UserListingDto>()
            {
                TotalRecord = totalUser,
                List = userList,
            };

            return result;
        }
    }
}
