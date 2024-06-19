namespace Np.DAL.Domain
{
    using Np.Common;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ActivityLog : IDefault, IAudited
    {
        [Key]
        public int ActivityLogId { get; set; }

        [Required]
        [MaxLength(256)]
        public string ActivityLogName { get; set; }

        [Required]
        public ActivityLogType LogType { get; set; }

        public virtual ICollection<AuditRecord> AuditRecord { get; } = new List<AuditRecord>();
        #region Inherited
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string SystemUser { get; set; } = string.Empty;
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AppName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; } = string.Empty;
        #endregion
    }
}
