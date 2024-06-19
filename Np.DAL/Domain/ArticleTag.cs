namespace Np.DAL.Domain
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class ArticleTag
    {
        [Key]
        [Required]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }

        [DisplayName("S.No")]
        public Guid TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
