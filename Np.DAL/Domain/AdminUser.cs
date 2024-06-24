namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class AdminUser
    {
        [Key]
        [Required]
        public Guid UserGuid { get; set; }
        [Required]
        [MaxLength(256)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(256)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(450)]
        public string UserEmail { get; set; } = string.Empty;
        public byte[]? Salt { get; set; }
        public string? UserPasswordHash { get; set; }
        public string? HashedConformationCode { get; set; }
        public bool? IsConfirmedRegistration { get; set; }
        public bool IsActive { get; set; }
        public int LoginAttempts { get; set; }
        public string? EncryptedSecret { get; set; }
        public bool EmailOTP { get; set; } = false;
        public Guid UserRoleGuid { get; set; }
        public bool TwofactorEnabled { get; set; }
        public Guid OrganisationGuid { get; set; }
        [MaxLength(128)]
        public string AvatarUrl { get; set; }

        [MaxLength(4000)]
        public string AboutUser { get; set; }

        #region Inherited
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string SystemUser { get; set; } = string.Empty;
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AppName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
public Guid CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public Guid? ModifiedBy { get; set; }
        #endregion
    }
}
