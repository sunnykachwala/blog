namespace Np.Admin.Service.ActivityLogs.Model
{
    public class ActivityLogDto
    {
        public int ActivityLogId { get; set; }

        public string ActivityLogName { get; set; }

        public string LogType { get; set; }

        public string EntityType { get; set; }

        public string PrimaryKeyValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
