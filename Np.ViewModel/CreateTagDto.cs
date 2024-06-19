using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Np.ViewModel
{
    public class CreateTagDto
    {
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
