namespace Np.ViewModel
{
    public class TagDto
    {
        public Guid Id { get; set; }
        public string TagName { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int DispalyOrder { get; set; }

        #region Seo
        public string? Keywords { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaTitle { get; set; }
        #endregion
    }
}
