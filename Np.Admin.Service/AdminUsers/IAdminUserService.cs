namespace Np.Admin.Service.AdminUsers
{
    using Np.ViewModel;

    public interface IAdminUserService
    {
        Task<UserInfoWithRoleDto?> GetUserInfoByEmailWithRole(string email);
        Task<bool> IsAuthenticatedExternalUser(UserInfoWithRoleDto userInfo, string password);
        Task UpdateLoginAttempts(string email, Guid userName);
        Task ResetLoginAttemps(string email, Guid userName);
        void AddRefreshToken(RefreshTokenDto refreshToken);
        Guid UpdateRefreshToken(string token, RefreshTokenDto newToken, string ipAddress);
        Task<UserInfoWithRoleDto?> GetUserInfoByUserId(Guid userId);
        void RevokeToken(string refreshToken, string ipAddress);
        Task<UserInfoDto> GetUserInfo(Guid userId);

        Task ConfirmRegistration(string password, byte[] salt, Guid userGuid);
        Task<UserInfoDto> GetUserInfoByEmail(string email);

        Task AddTwoFactorAuthentication(string email, string encryptedSecret, bool isActive);

        Task UpdateUser(UserInfoDto user);

        Task<PaginatedResult<UserListingDto>> GetAllUser(UserFilterDto filter);

        Task<Guid> CreateUser(CreateUserDto user, string confirmationCode, Guid modifiedBy);

        Task<bool> UserEmailExists(string email);

        Task<PaginatedResult<UserListingDto>> GetUsersByRole(FilterDto filter, string role);


    }
}
