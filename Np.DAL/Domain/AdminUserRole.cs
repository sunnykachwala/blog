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


        #region Inherited
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string SystemUser { get; set; } = string.Empty;
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AppName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; } = string.Empty;
        #endregion
    }

}
