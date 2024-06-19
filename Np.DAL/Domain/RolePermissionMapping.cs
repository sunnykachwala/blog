namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class RolePermissionMapping 
    {
        [Key]
        [Required]
        public Guid RolePermissionMappingId { get; set; }

        [Required]
        [ForeignKey("AdminUserRole")]
        public Guid UserRoleId { get; set; }
        public virtual AdminUserRole UserRole { get; set; }

        [Required]
        [ForeignKey("UserPermission")]
        public Guid UserPermissionId { get; set; }

    }
}
