namespace Np.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class IgnoreForAuditAttribute : Attribute
    {
    }
}
