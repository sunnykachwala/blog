namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Np.Admin.Service.Articles;
    using Np.Admin.Service.Articles.Model;
    using Np.Admin.WebApi.Helper;
    using Np.Admin.WebApi.Model;
    using Np.ViewModel;

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
            if (model.SourceFile == null || model.SourceFile.Length <= 0)
                return BadRequest("Empty file");

            // Get the path for saving the uploaded image
            string path = Path.Combine(Directory.GetCurrentDirectory(), @$"{this.appConfig.FilePath.PostImage}");

            // Upload the image and handle the response
            var res = await FileManager.UploadImageAsync(model.SourceFile, path);

            // Handle the error case if image upload fails
            if (res["status"] == "error")
            {
                return BadRequest(new { message = res.GetValueOrDefault("message") });
            }

            var modelData = new CreateArticleDto()
            {
                Content = model.Content,
                DefaultImage = res.GetValueOrDefault("message"),
                DispalyOrder = model.DisplayOrder,
                IsPublished = model.IsPublished,
                Keywords = model.Keywords,
                MetaDescription = model.MetaDescription,
                MetaTitle = model.MetaTitle,
                SelectedCategories = model.SelectedCategories,
                SelectedTags = model.SelectedTags,
                Slug = model.Slug,
                Title = model.Title,
            };

            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();

            var articleId = await this.articleService.Add(modelData, base.LoggedInUserInfo.UserId);

            return Ok(new { message = $"Article Added successfully {articleId}." });
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllArticel([FromQuery] ArticleFilterDto model)
        {
            //base.InitializContext();
            //if (base.LoggedInUserInfo == null)
            //    return Unauthorized();
            var result = await this.articleService.AllArticle(model);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetArticel(Guid id)
        {
            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
            var result = await this.articleService.GetById(id);
            return Ok(result);
        }
    }
}
