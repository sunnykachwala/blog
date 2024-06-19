namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Np.Admin.Service.AppSettings;
    using Np.DAL.Domain;
    using Np.ViewModel;
    using System.Security.Claims;

    [Route("api/[controller]")]
    [ApiController]
    public class NpBaseController : ControllerBase
    {
        private readonly IAppSettingService appSettingService;
        protected AppSettingDto _appSetting;
        protected LoggedInUserInfo LoggedInUserInfo { get; set; }

        public NpBaseController()
        {

        }
        public NpBaseController(IAppSettingService appSettingService)
        {
            this.appSettingService = appSettingService;
        }
        protected async Task<AppSettingDto> GetAppSetting()
        {
            _appSetting ??= await this.appSettingService.GetAppSetting();
            return _appSetting;
        }
        protected void InitializContext()
        {
            var user = HttpContext.User.Claims;
            if (user != null)
            {
                var userId = user.FirstOrDefault(x => x.Type.Equals("userid"))?.Value;
                if (userId != null)
                {
                    var userClaims = new LoggedInUserInfo()
                    {
                        UserName = user.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name, StringComparison.Ordinal))?.Value,
                        UserId = Guid.Parse(userId),
                        IPAddress = HttpContext.Connection.RemoteIpAddress.ToString(),
                        Role = user.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Role))?.Value,
                        UserEmail = user.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email))?.Value,
                    };
                    HttpContext.Items["UserClaims"] = userClaims;
                    LoggedInUserInfo = userClaims;
                }
            }
        }
    }
}
