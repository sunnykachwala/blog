namespace Np.ViewModel
{
    using System.Text.Json.Serialization;
    public class AuthenticateResponse
    {
        public Guid UserGuid { get; set; }
        public string FullName { get; set; }
        public string LastName { get; set; }
        public bool TwofactorEnabled { get; set; }
        public string JwtToken { get; set; }
        public string UserEmail { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
        public string OrganisationName { get; set; }
        public Guid OrganisationGuid { get; set; }
        public string Role { get; set; }
        public string Permissions { get; set; }
        public string DefaultHome { get; set; }

        public AuthenticateResponse(UserInfoWithRoleDto user, string jwtToken, string refreshToken)
        {
            UserGuid = user.UserGuid;
            FullName = user.FirstName;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
            TwofactorEnabled = user.TwofactorEnabled;
            UserEmail = user.UserEmail;
            Role = user.RoleName;
            OrganisationGuid = user.OrganisationGuid;
            OrganisationName = user.OrganisationName;
            Permissions = user.Permissions;
            DefaultHome = user.DefaultHome;
        }
    }
}
