namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Np.Admin.Service.Categories;
    using Np.Admin.Service.Categories.Model;
    using Np.Admin.WebApi.Helper;
    using Np.Admin.WebApi.Model;
    using Np.ViewModel;

    [Route("api/category")]
    [ApiController]
    public class CategoryController : NpBaseController
    {
        private readonly ICategoryService categoryService;
        private readonly AppConfig appConfig;
        public CategoryController(ICategoryService categoryService,
            IOptions<AppConfig> appConfig)
        {
            this.categoryService = categoryService;
            this.appConfig = appConfig.Value;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryWithFileDto model)
        {
            if (model.SourceFile == null || model.SourceFile.Length <= 0)
                return BadRequest("Empty file");

            // Get the path for saving the uploaded image
            string path = Path.Combine(Directory.GetCurrentDirectory(), @$"{this.appConfig.FilePath.CategoryImage}");

            // Upload the image and handle the response
            var res = await FileManager.UploadImageAsync(model.SourceFile, path);

            // Handle the error case if image upload fails
            if (res["status"] == "error")
            {
                return BadRequest(new { message = res.GetValueOrDefault("message") });
            }

            var modelData = new CreateCategoryDto()
            {
                Title = model.Title,
                Details = model.Details,
                CategoryImage = res.GetValueOrDefault("message"),
                ParentCategoryId = model.ParentCategoryId,
                IsActive = model.IsActive,
                DisplayOrder = model.DisplayOrder,
                Keywords = model.Keywords,
                MetaDescription = model.MetaDescription,
                MetaTitle = model.MetaTitle,
                Slug = model.Slug,
            };

            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();

            var categoryId = await this.categoryService.Add(modelData, base.LoggedInUserInfo.UserId);

            return Ok(new { message = $"Category Added successfully {categoryId}." });
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(CategoryDto model)
        {
            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
            await this.categoryService.Update(model, base.LoggedInUserInfo.UserId);

            return Ok(new { message = $"Category Updated successfully." });
        }

        [HttpGet("get-parent")]
        public async Task<IActionResult> GetParent([FromQuery] FilterDto filter)
        {
            var result = await this.categoryService.GetAllCachedParentCategory(filter);

            return Ok(result);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll([FromQuery] FilterDto filter)
        {
            var result = await this.categoryService.AllCachedCategory(filter);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("remove-parent-cache")]
        public IActionResult RemoveParentCache()
        {
            this.categoryService.RemoveParentCategoryCache();
            return Ok(new { message = $"Category Cache removed successfully." });
        }

        [AllowAnonymous]
        [HttpGet("remvoe-cache")]
        public IActionResult RemoveCache()
        {
            this.categoryService.RemoveCategoryCache();
            return Ok(new { message = $"Category Cache removed successfully." });
        }

        [HttpGet]
        [Route("view")]
        public async Task<IActionResult> ViewImage(string imageName)
        {
            var file = imageName;
            var uniqueFilePath = Path.Combine(Directory.GetCurrentDirectory(), @$"{this.appConfig.FilePath.CategoryImage}", file);
            var stream = new FileStream(uniqueFilePath, FileMode.Open);

            string fileType = "";

            if (file.ToLower().Contains(".pdf"))
                fileType = "application/pdf";

            if (file.ToLower().Contains(".png"))
                fileType = "image/png";

            if (file.ToLower().Contains(".jpeg") || file.ToLower().Contains(".jpg"))
                fileType = "image/jpeg";

            return File(stream, fileType, file);
        }

    }
}
