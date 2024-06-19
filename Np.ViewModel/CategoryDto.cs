namespace Np.ViewModel
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Details { get; set; } = string.Empty;

        public string CategoryImage { get; set; } = string.Empty;

        public Guid? ParentCategoryId { get; set; } = null;

        public bool IsActive { get; set; }

        public int DispalyOrder { get; set; }

        #region Seo
        public string? Keywords { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }
        public string Slug { get; set; }
        #endregion
    }
}
