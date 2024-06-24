namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class UserPermission : IDefault, IAudited
    {
        [Key]
        [Required]
        public Guid UserPermissionId { get; set; }

        [Required, MaxLength(126)]
        public string Permission { get; set; }

        public bool IsActive { get; set; } = true;

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
