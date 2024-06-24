namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Np.Admin.Service.Articles;
    using Np.Admin.WebApi.Utilities;
    using Np.ViewModel;

    [Route("api/article")]
    [ApiController]
    public class ArticleController : NpBaseController
    {
        private readonly IArticleService articleService;
        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticel(CreateArticleDto model)
        {
            string ipAddress = CommonHelper.GetIPAddress(HttpContext);
#if DEBUG
            var articleId = await this.articleService.Add(model, ipAddress, Guid.NewGuid());

#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
                var articleId =  await this.articleService.Add(model,ipAddress,  base.LoggedInUserInfo.UserId);         
#endif
            return Ok(new { message = $"Article Added successfully {articleId}." });
        }


        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllArticel(ArticleFilterDto model)
        {
#if DEBUG
            var result = await this.articleService.AllArticle(model);
#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
                var result = await this.articleService.AllArticle(model);
#endif
            return Ok(result);
        }
    }
}
