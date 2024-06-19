namespace Np.Admin.Service.Token
{
    using Np.ViewModel;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;
    public class TokenService : ITokenService
    {
        public string GenerateJwtToken(UserInfoWithRoleDto userInfo, JWT jwt)
        {
            var accountName = userInfo.UserEmail.Split('@')[0];
            var claims = new List<Claim>
                            {
                                new Claim("userid",userInfo.UserGuid.ToString()),
                                new Claim(ClaimTypes.Email, userInfo.UserEmail),
                                new Claim(ClaimTypes.Role, userInfo.RoleName),
                                new Claim(ClaimTypes.GroupSid, userInfo.UserRoleGuid.ToString()),
                                new Claim(ClaimTypes.Name, accountName),
                                new Claim("orgnisationGuid", userInfo.OrganisationGuid.ToString()),
                                new Claim("orgnisationName", userInfo.OrganisationName.ToString()),
                            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwt.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(jwt.TokenExpiryMins),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public RefreshTokenDto GenerateRefreshToken(string ipAddress, JWT jwt)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshTokenDto
                {
                    Token = Convert.ToBase64String(randomBytes),
                    Expires = DateTime.UtcNow.AddMinutes(jwt.RefreshTokenExpiryMins),
                    Created = DateTime.UtcNow,
                    CreatedByIp = ipAddress
                };
            }
        }
    }
}
