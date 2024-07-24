namespace Np.Admin.Service.Articles.Model
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateArticleDto
    {
        [Required]
        [MaxLength(512)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

    
        [MaxLength(256)]
        public string? DefaultImage { get; set; }

        [Required]
        [DisplayName("Is Published")]
        public bool IsPublished { get; set; }

        public int DisplayOrder { get; set; }

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

        public ICollection<Guid> SelectedCategories { get; set; }
        public ICollection<Guid> SelectedTags { get; set; }

    }
}
