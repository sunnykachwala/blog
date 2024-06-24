namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class AdminUserRole : IDefault, IAudited
    {
        [Key]
        [Required]
        public Guid UserRoleGuid { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public bool IsDefaultRole { get; set; } = false;

        [Required]
        [StringLength(100)]
        public string DefaultHome { get; set; }
        
        // Navigation Property
        public AdminUser AdminUser { get; set; }

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
