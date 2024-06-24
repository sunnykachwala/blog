namespace Np.DAL.Domain
{

    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Newtonsoft.Json;
    using Np.Common;

    internal class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public Guid UserGuid { get; set; } 
        public string TableName { get; set; } = string.Empty;
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public string KeyValue { get; set; } = string.Empty;

        public int ActivityLogId { get; set; }
        public AuditRecord ToAuditRecord()

        {
            var audit = new AuditRecord
            {
                CreatedBy = UserGuid,
                CreatedAt = DateTime.UtcNow,
                Type = AuditType.ToString(),
                TableName = TableName,
                PrimaryKey = JsonConvert.SerializeObject(KeyValues),
                OldValues = !OldValues.Any() ? null : JsonConvert.SerializeObject(OldValues),
                NewValues = !NewValues.Any() ? null : JsonConvert.SerializeObject(NewValues),
                AffectedColumns = ChangedColumns.Any() ? JsonConvert.SerializeObject(ChangedColumns) : null,
                KeyValue = KeyValue,
                ActivityLogId = ActivityLogId,
            };
            return audit;
        }
    }
}
