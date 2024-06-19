namespace Np.ViewModel
{
    public class PaginatedResult<T> where T : class
    {
        public int TotalRecord { get; set; }
        public List<T> List { get; set; }
    }
}
