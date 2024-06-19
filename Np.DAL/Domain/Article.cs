namespace Np.DAL.Domain
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Article : IDefault, IAudited, ISeo
    {
        [Key]
        [Required]
        public Guid ArticleId { get; set; }

        [Required]
        [MaxLength(512)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public Guid AuthorId { get; set; }
        public AdminUser Author { get; set; }

        [MaxLength(256)]

        public string? DefaultImage { get; set; }

        [Required]
        [DisplayName("Is Published")]
        public bool IsPublished { get; set; }
        public DateTime PublishedDate { get; set; }

        public int DispalyOrder { get; set; }

        #region Seo
        [StringLength(300)]
        [DisplayName("Keywords")]
        public string? Keywords { get; set; }

        [StringLength(300)]
        [DisplayName("Meta Description")]
        public string? MetaDescription { get; set; }

        [StringLength(160)]
        [DisplayName("Meta Title")]
        public string? MetaTitle { get; set; }

        [Required]
        [StringLength(160)]
        [DisplayName("Slug")]
        public string Slug { get; set; }
        #endregion
        
        [Required]
        [StringLength(160)]
        public string IpAddress { get; set; }

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

        public ICollection<ArticleComment> Comments { get; set; }
        public ICollection<ArticleCategory> ArticleCategories { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }
    }
}
