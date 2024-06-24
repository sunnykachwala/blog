
namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    public class AuditRecord : IDefault, IAudited
    {
        [Key]
        public Guid AuditRecordGuid { get; set; }
        public string Type { get; set; } = string.Empty;
        public string TableName { get; set; } = string.Empty;
        public string? OldValues { get; set; } = string.Empty;
        public string? NewValues { get; set; } = string.Empty;
        public string? AffectedColumns { get; set; } = string.Empty;
        public string PrimaryKey { get; set; } = string.Empty;
        [MaxLength(100)]
        public string KeyValue { get; set; } = string.Empty;

        public int ActivityLogId { get; set; }

        public virtual ActivityLog ActivityLog { get; set; } = null!;

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
