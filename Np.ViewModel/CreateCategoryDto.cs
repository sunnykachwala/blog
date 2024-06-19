namespace Np.ViewModel
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class CreateCategoryDto
    {
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
        public Guid? ParentCategoryId { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

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
    }
}
