namespace Np.DAL.Domain
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    public class ArticleCategory
    {
        [Key]
        [Required]
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        
        [DisplayName("S.No")]
        public Guid CategoryId { get; set; }

        public Category Category { get;set; }
    }
}
