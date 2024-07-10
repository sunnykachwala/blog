namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class AuditLog : IDefault, IAudited
    {
        [Key]
        public Guid AuditLogGuid { get; set; }

        public int ActivityLogId { get; set; }

        public virtual ActivityLog ActivityLog { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string KeyName { get; set; }
        public string? OldValue { get; set; } = string.Empty;
        public string? NewValues { get; set; } = string.Empty;

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
