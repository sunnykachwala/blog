namespace Np.Admin.Service.Token
{
    using Np.ViewModel;
    public interface ITokenService
    {
        string GenerateJwtToken(UserInfoWithRoleDto userInfo, JWT jwt);
        RefreshTokenDto GenerateRefreshToken(string ipAddress, JWT jwt);
    }
}