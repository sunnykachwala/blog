namespace Np.ViewModel
{
    public class FilterDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
    }
}
