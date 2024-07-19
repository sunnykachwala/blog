namespace Np.Admin.Service.Articles
{
    using Np.Admin.Service.Articles.Model;
    public interface IArticleService
    {
        Task<Guid> Add(CreateArticleDto model,  Guid modifiedBy);

        Task<List<ArticleDto>?> AllArticle(ArticleFilterDto filter);
        Task<ArticleDto?> GetById(Guid articleId);
    }
}