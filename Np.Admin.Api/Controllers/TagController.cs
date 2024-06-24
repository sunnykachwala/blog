namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Np.Admin.Service.Tags;
    using Np.ViewModel;

    [Route("api/tag")]
    [ApiController]
    public class TagController : NpBaseController
    {
        private readonly ITagService tagService;

        public TagController(ITagService tagService)
        {
            this.tagService = tagService;
        }

        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<IActionResult> CreateTag(CreateTagDto model)
        {
#if DEBUG
            var tagId = await this.tagService.Add(model, Guid.NewGuid());
#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
                var tagId =  await this.tagService.Add(model, base.LoggedInUserInfo.UserId);         
#endif
            return Ok(new { message = $"Tag Added successfully {tagId}." });
        }

        [AllowAnonymous]
        [HttpPost("update")]
        public async Task<IActionResult> Update(TagDto model)
        {
#if DEBUG
            await this.tagService.Update(model, Guid.NewGuid());
#else
    base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
              await this.tagService.Update(model, base.LoggedInUserInfo.UserId);         
#endif
            return Ok(new { message = $"Tag Updated successfully." });
        }

        [AllowAnonymous]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(FilterDto filter)
        {
            var result = await this.tagService.AllCachedTag(filter);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("remvoe-cache")]
        public IActionResult RemoveCache()
        {
            this.tagService.RemoveTagCache();
            return Ok(new { message = $"Tag  Cache removed successfully." });
        }

    }
}
