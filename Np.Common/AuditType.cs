namespace Np.Common
{
    public enum AuditType : byte
    {
        None = 0,
        Create = 1,
        Update = 2,
        Delete = 3
    }

    public enum ActivityLogType : byte
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Export = 4,
        Import = 5,
        Sign = 6,
        Print = 7,
    }
}