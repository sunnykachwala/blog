namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class ArticleView
    {
        [Key]
        public int ArticleViewId { get; set; }

        public Guid ArticleId { get; set; }
        public virtual Article Article { get; set; }

        [Required]
        [StringLength(160)]
        public string IpAddress { get; set; }

        public DateTime ViewDate { get; set; } = DateTime.UtcNow;

        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string SystemUser { get; set; } = string.Empty;
        [MaxLength(256)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string AppName { get; set; } = string.Empty;
    }
}
