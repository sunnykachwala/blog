namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class LoginResetHistory : IDefault, IAudited
    {
        [Key]
        [Required]
        public Guid LoginResetHistoryGuid { get; set; }

        [Required]
        public Guid LoginGuid { get; set; }

        [Required]
        public string ResetType { get; set; }

        [Required]
        public DateTime ResetExpiryTime { get; set; }

        [Required]
        public Byte[] Salt { get; set; }

        [Required]
        public String HashedConfirmationCode { get; set; }

        [Required]
        public bool IsActive { get; set; }

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
