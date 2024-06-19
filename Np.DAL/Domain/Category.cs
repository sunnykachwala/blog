namespace Np.DAL.Domain
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;

    public class Category : IDefault, IAudited, ISeo
    {
        [DisplayName("S.No")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(120)]
        [DisplayName("Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        [DisplayName("Details")]
        public string Details { get; set; } = string.Empty;

        [DisplayName("Category Image")]
        public string CategoryImage { get; set; } = string.Empty;

        [DisplayName("Parent")]
        public Guid? ParentCategoryId { get; set; } = null;
        public Category ParentCategory { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        public int DispalyOrder { get; set; }

        public virtual ICollection<Category> Subcategories { get; set; } = new List<Category>();

        public ICollection<ArticleCategory> ArticleCategories { get; set; }


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
        public string CreatedBy { get; set; } = string.Empty;

        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; } = string.Empty;
        #endregion
    }
}
