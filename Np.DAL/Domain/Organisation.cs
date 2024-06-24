namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Organisation : IDefault, IAudited
    {
        [Key]
        public Guid OrganisationGuid { get; set; }

        [Required]
        [MaxLength(256)]
        public string OrganisationName { get; set; }

        public bool IsActive { get; set; }

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
