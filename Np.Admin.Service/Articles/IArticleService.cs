namespace Np.Admin.Service.Articles
{
    using Np.ViewModel;
    public interface IArticleService
    {
        Task<Guid> Add(CreateArticleDto model, string ipAddress, Guid modifiedBy);

        Task<List<ArticleDto>?> AllArticle(ArticleFilterDto filter);
    }
}