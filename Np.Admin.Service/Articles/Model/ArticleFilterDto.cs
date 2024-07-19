namespace Np.Admin.Service.Articles.Model
{
    using Np.ViewModel;
    public class ArticleFilterDto : FilterDto
    {
        public Guid? CategoryId { get; set; }
        public Guid? TagId { get; set;}
        public Guid? AuthorId { get; set; }
    }
}
