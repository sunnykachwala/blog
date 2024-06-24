namespace Np.DAL.Domain
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Tag : IDefault, IAudited, ISeo
    {
        [DisplayName("S.No")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("TagName")]
        public string TagName { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        [DisplayName("Details")]
        public string Details { get; set; } = string.Empty;

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public int DispalyOrder { get; set; }
        public ICollection<ArticleTag> ArticleTags { get; set; }


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
