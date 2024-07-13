namespace Np.Common
{
    using System.ComponentModel;
    public enum EntityTypes : byte
    {
        [Description("Admin Users")]
        AdminUsers = 1,

        [Description("User Role")]
        UserRole = 2,

        [Description("Article")]
        Article = 3,

        [Description("Category")]
        Category = 4,

        [Description("Url Record")]
        UrlRecord = 5,
    }
}
