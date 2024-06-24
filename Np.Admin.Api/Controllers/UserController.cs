using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Np.Admin.Service.AdminUsers;
using Np.Admin.Service.Emails;
using Np.Common;
using Np.ViewModel;
using Razor.Templating.Core;
using static QRCoder.PayloadGenerator.WiFi;

namespace Np.Admin.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : NpBaseController
    {
        private readonly IAdminUserService userService;
        private readonly IEmailService emailService;
        private readonly AppConfig appConfig;
        public UserController(IAdminUserService userService,
            IOptions<AppConfig> appConfig,
            IEmailService emailService)
        {
            this.userService = userService;
            this.appConfig = appConfig.Value;
            this.emailService = emailService;
        }

        [HttpGet]
        [Route("user-info")]
        public async Task<IActionResult> GetUserInfo()
        {
            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();

            var user = await this.userService.GetUserInfo(base.LoggedInUserInfo.UserId);

            return Ok(user);
        }

        [HttpGet]
        [Route("get-all-user")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUserInfo([FromQuery] UserFilterDto filter)
        {
            //var loggedInUser = HttpContext.Items["UserClaims"] as LoggedInUserInfo;
            var userList = await this.userService.GetAllUser(filter);
            return Ok(userList);
        }
        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(CreateUserDto user)
        {
            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();
            string confirmationCode = UtilityHelper.GenerateConfirmationCode();


            var result = await this.userService.CreateUser(user, confirmationCode, base.LoggedInUserInfo.UserId);

            EmailDto email = new EmailDto()
            {
                AttachmentList = new List<System.Net.Mail.Attachment>(),
                Bcc = "",
                Cc = "",
                Subject = "News portal Admin user registration email.",
                To = user.UserEmail,
                IsBodyHtml = true
            };

            var viewDataOrViewBag = new Dictionary<string, object>();
            viewDataOrViewBag["WebUrl"] = appConfig.AdminUrl.Replace("/#/", "");

            var emailTemplate = new
            {
                UserGuid = result.ToString(),
                ConfirmationCode = confirmationCode,
                FrontEndUrl = appConfig.AdminUrl,
                user.FirstName,
            };

            email.Mailcontent = await RazorTemplateEngine.RenderAsync("Template/Registration.cshtml", emailTemplate, viewDataOrViewBag);
            if (!appConfig.EmailSettings.IsSimulator)
            {
                emailService.SendEmail(email, appConfig.SMTPSettings, appConfig.EmailSettings);
            }

            return Ok(new { message = $"User created successfully with user id {result}.", id = result });
        }

        [HttpGet]
        [Route("get-user-by-id")]
        public async Task<IActionResult> GetUerById(Guid userId)
        {
            if (!Guid.TryParse(userId.ToString(), out var validGuid))
                return BadRequest(new { errorMessage = $"Invalid User Id!" });

            var userInfo = await this.userService.GetUserInfoByUserId(validGuid);
            if (userInfo == null)
                return BadRequest(new { errorMessage = $"Invalid User Id!" });

            var user = await this.userService.GetUserInfo(userId);

            var returnResut = new
            {
                user.UserGuid,
                user.FirstName,
                user.LastName,
                user.UserEmail,
                user.UserRoleGuid,
                user.IsActive,
                user.TwofactorEnabled,
                user.UserType,
                user.OrganisationGuid
            };
            return Ok(returnResut);
        }

        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto user)
        {
            var userInfo = await this.userService.GetUserInfo(user.UserGuid);
            if (userInfo == null)
                return BadRequest(new { errorMessage = $"Invalid User data!" });

            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();

            var userData = new UserInfoDto()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                OrganisationGuid = user.OrganisationGuid,
                IsActive = user.IsActive,
                TwofactorEnabled = user.TwofactorEnabled,
                UserEmail = userInfo.UserEmail,
                UserGuid = user.UserGuid,
                UserRoleGuid = user.UserRoleGuid,
                EmailOTP = userInfo.EmailOTP,
                EncryptedSecret = userInfo.EncryptedSecret,
                HashedConformationCode = userInfo.HashedConformationCode,
                IsConfirmedRegistration = userInfo.IsConfirmedRegistration,
                LoginAttempts = userInfo.LoginAttempts,
                RefreshTokens = userInfo.RefreshTokens,
                Salt = userInfo.Salt,
                UserPasswordHash = userInfo.UserPasswordHash,
                UserType = userInfo.UserType,
            };

            await this.userService.UpdateUser(userData);
            return Ok(new { message = $"User updated successfully." });
        }

        [HttpGet]
        [Route("get-user-by-role/{role}")]
        public async Task<IActionResult> GetUerByRole([FromQuery] FilterDto filter, string role)
        {
            var result = await this.userService.GetUsersByRole(filter, role);
            return Ok(result);
        }

        [HttpPost("active-inactive-user")]
        public async Task<IActionResult> ActiveInActive([FromQuery] Guid userGuid, [FromBody] bool isActive)
        {
            var userInfo = await this.userService.GetUserInfoByUserId(userGuid);
            if (userInfo == null)
                return BadRequest(new { errorMessage = $"Invalid User data!" });

            base.InitializContext();
            if (base.LoggedInUserInfo == null)
                return Unauthorized();

            userInfo.IsActive = isActive;

            await this.userService.UpdateUser(userInfo);
            return Ok(new { message = $"User updated successfully." });
        }

        [HttpGet("check-email-exists")]
        public async Task<IActionResult> CheckEmailExist([FromQuery] string email)
        {
            var user = await this.userService.GetUserInfoByEmail(email);
            return Ok(new { exists = user != null });
        }
    }
}
