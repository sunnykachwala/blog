namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Np.Admin.Service.Categories;
    using Np.ViewModel;

    [Route("api/category")]
    [ApiController]
    public class CategoryController : NpBaseController
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto model)
        {
#if DEBUG
            var categoryId = await this.categoryService.Add(model, Guid.NewGuid());
#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
                var categoryId =  await this.categoryService.Add(model,  base.LoggedInUserInfo.UserId);         
#endif
            return Ok(new { message = $"Category Added successfully {categoryId}." });
        }

        [AllowAnonymous]
        [HttpPost("update")]
        public async Task<IActionResult> Update(CategoryDto model)
        {
#if DEBUG
            await this.categoryService.Update(model, Guid.NewGuid());
#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
              await this.categoryService.Update(model, base.LoggedInUserInfo.UserId);         
#endif
            return Ok(new { message = $"Category Updated successfully." });
        }

        [AllowAnonymous]
        [HttpGet("get-parent")]
        public async Task<IActionResult> GetParent(FilterDto filter)
        {
            var result = await this.categoryService.GetAllCachedParentCategory(filter);

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(FilterDto filter)
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
    }
}
