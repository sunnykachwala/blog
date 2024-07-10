namespace Np.Admin.Service.ActivityLogs.Model
{
    using Np.Common;
    using System.ComponentModel.DataAnnotations;
    public class CreateActivityLogDto
    {
        [Required]
        [MaxLength(256)]
        public string ActivityLogName { get; set; }

        [Required]
        public ActivityLogType LogType { get; set; }

        [Required]
        public EntityTypes EntityType { get; set; }

        [Required]
        public string PrimaryKeyValue { get; set; }


        public List<CreateAuditLogDto> AuditLog { get; set; }
    }
}
