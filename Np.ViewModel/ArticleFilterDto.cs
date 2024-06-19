namespace Np.ViewModel
{
    public class ArticleFilterDto : FilterDto
    {
        public List<Guid?> CategoryId { get; set; }
        public List<Guid?> TagId { get; set;}
        public List<Guid?> AuthorId { get; set; }
    }
}
