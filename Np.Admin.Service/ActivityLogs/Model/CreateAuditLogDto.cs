namespace Np.Admin.Service.ActivityLogs.Model
{
    using System.ComponentModel.DataAnnotations;
    public class CreateAuditLogDto
    {
        public int ActivityLogId { get; set; }
        [Required]
        [MaxLength(100)]
        public string KeyName { get; set; }
        public string? OldValue { get; set; } = string.Empty;
        public string? NewValues { get; set; } = string.Empty;
    }
}
