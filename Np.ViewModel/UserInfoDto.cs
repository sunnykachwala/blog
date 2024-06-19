namespace Np.ViewModel
{
    using System.Text.Json.Serialization;

    public class UserInfoDto
    {
        public Guid UserGuid { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public bool? IsConfirmedRegistration { get; set; }
        public bool IsActive { get; set; }
        public int LoginAttempts { get; set; }
        public Guid UserRoleGuid { get; set; }
        public string? UserType { get; set; }
        public bool TwofactorEnabled { get; set; }
        public byte[]? Salt { get; set; }
        public string? UserPasswordHash { get; set; }
        public bool EmailOTP { get; set; } = false;
        public string? HashedConformationCode { get; set; }
        [JsonIgnore]
        public string? EncryptedSecret { get; set; }
        [JsonIgnore]
        public List<RefreshTokenDto> RefreshTokens { get; set; }
        public Guid OrganisationGuid { get; set; }
    }
}
