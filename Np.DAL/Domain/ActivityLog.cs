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

        [Required]
        public EntityTypes EntityType { get; set; }

        [Required]
        public string PrimaryKeyValue { get; set; }

        [Required]
        [MaxLength(128)]
        public string IpAddress { get; set; }


        public virtual ICollection<AuditRecord> AuditRecord { get; } = new List<AuditRecord>();
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
