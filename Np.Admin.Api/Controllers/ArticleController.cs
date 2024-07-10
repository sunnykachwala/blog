namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Np.Admin.Service.Articles;
    using Np.Admin.WebApi.Model;
    using Np.Admin.WebApi.Utilities;
    using Np.ViewModel;
    using Microsoft.Extensions.Options;

    [Route("api/article")]
    [ApiController]
    public class ArticleController : NpBaseController
    {
        private readonly IArticleService articleService;
        private readonly AppConfig appConfig;
        public ArticleController(IArticleService articleService, IOptions<AppConfig> appConfig)
        {
            this.articleService = articleService;
            this.appConfig = appConfig.Value;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateArticel([FromForm] CreateArticleWithFileDto model)
        {
            string ipAddress = CommonHelper.GetIPAddress(HttpContext);

            var file = model.SourceFile;

            if (file.Length <= 0)
                return BadRequest("Empty file");

            var originalFileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            //Create a unique file path
            var uniqueFileName = Path.GetRandomFileName() + extension;
            var uniqueFilePath = Path.Combine(Directory.GetCurrentDirectory(), @$"{this.appConfig.FilePath.PostImage}", uniqueFileName);

            //Save the file to disk
            using (var stream = System.IO.File.Create(uniqueFilePath))
            {
                await file.CopyToAsync(stream);
            }
            var modelData = new CreateArticleDto()
            {
                Content = model.Content,
                DefaultImage = uniqueFileName,
                DispalyOrder = model.DispalyOrder,
                IsPublished = model.IsPublished,
                Keywords = model.Keywords,
                MetaDescription = model.MetaDescription,
                MetaTitle = model.MetaTitle,
                SelectedCategories = model.SelectedCategories,
                SelectedTags = model.SelectedTags,
                Slug = model.Slug,
                Title = model.Title,
            };
#if DEBUG
            var articleId = await this.articleService.Add(modelData, ipAddress, Guid.NewGuid());

#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
            var articleId = await this.articleService.Add(modelData, ipAddress, Guid.NewGuid());
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
