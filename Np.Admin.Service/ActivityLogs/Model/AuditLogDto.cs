namespace Np.Admin.Service.ActivityLogs.Model
{
    public class AuditLogDto
    {
        public Guid AuditLogGuid { get; set; }
        public int ActivityLogId { get; set; }
        public string KeyName { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValues { get; set; } = string.Empty;
    }
}
