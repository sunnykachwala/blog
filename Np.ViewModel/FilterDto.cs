namespace Np.ViewModel
{
    public class FilterDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public bool? IsActive { get; set; }
        public string? SortField { get; set; }
        public string? SortOrder { get; set; }
       // public List<SortData>? Sort { get; set; }
    }

    public class SortData
    {
        public string? Key { get; set; }

        public string? Value { get; set; }
    }
}
