namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations;
    public class ArticleComment
    {
        [Key]
        [Required]
        public Guid CommentId { get; set; }
        [Required]
        [MaxLength(2048)]
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public Guid ArticleId { get; set; }
        public Article Article { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(160)]
        public string IpAddress { get; set; }

        public bool IsApproved { get; set; }

    }
}
