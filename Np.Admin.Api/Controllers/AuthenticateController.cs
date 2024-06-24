namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Np.Admin.Service.AdminUsers;
    using Np.Admin.Service.Emails;
    using Np.Admin.Service.LoginHistory;
    using Np.Admin.Service.Organisations;
    using Np.Admin.Service.Token;
    using Np.Admin.WebApi.Utilities;
    using Np.Auth.MFA;
    using Np.Auth.MFA.Common;
    using Np.Common;
    using Np.ViewModel;
    using OtpNet;
    using Razor.Templating.Core;
    using System;
    using System.Security.Cryptography;


    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAdminUserService userService;
        private readonly ITokenService tokenService;
        private readonly ILogger<AuthenticateController> logger;
        private readonly ILoginHistoryService loginHistoryService;
        private readonly IEmailService emailService;
        private readonly IOrganisationService organisationService;
        private readonly AppConfig appConfig;

        public AuthenticateController(
         IAdminUserService userService,
          IOptions<AppConfig> appConfig,
            ILogger<AuthenticateController> logger,
            ITokenService tokenService,
            ILoginHistoryService loginHistoryService,
            IOrganisationService organisationService,
             IEmailService emailService)
        {
            this.userService = userService;
            this.appConfig = appConfig.Value;
            this.tokenService = tokenService;
            this.logger = logger;
            this.loginHistoryService = loginHistoryService;
            this.emailService = emailService;
            this.organisationService = organisationService;
        }


        [AllowAnonymous]
        [HttpPost()]
        public async Task<IActionResult> Login(LoginDto loginInfo)
        {
            string ipRange = string.Empty;

            var orgInfo = await this.organisationService.GetOrganisatonByName(loginInfo.OrganisationName) ?? throw new Exception($"Provided Organisation {loginInfo.OrganisationName} dose not exists");
            var userInfo = await this.userService.GetUserInfoByEmailWithRole(loginInfo.UserEmail);
            if (userInfo == null)
            {
                return BadRequest(new { errorMessage = $"User with email {loginInfo.UserEmail} is not registered!" });
            }

            if (orgInfo.OrganisationGuid != userInfo.OrganisationGuid) throw new Exception($"User does not belong to the organisation");

            if (userInfo.IsConfirmedRegistration.HasValue && !userInfo.IsConfirmedRegistration.Value)
                return BadRequest(new { errorMessage = $" This acccount is not yet confirmed. Please check your email and confirm registration process." });

            if (userInfo.LoginAttempts > 5)
                return BadRequest(new { errorMessage = $"This account is blocked due to retry many times. Please contact system administrator to unlock." });

            bool isAuthenticated = await this.userService.IsAuthenticatedExternalUser(userInfo, loginInfo.Password);

            if (!isAuthenticated)
            {
                await this.userService.UpdateLoginAttempts(userInfo.UserEmail, userInfo.UserGuid);
                return BadRequest(new { errorMessage = $"Unrecognized User email / Password combination." });
            }
            ///check for IP ranged then check for two factor enabled or not 
            ///if not in a range then no need to check for Twofactor just redirect to token page

            if (!string.IsNullOrWhiteSpace(ipRange))
            {
                string ipAddress = CommonHelper.GetIPAddress(HttpContext);
                string[] range = ipRange.Split("-");
                if (ipAddress != null && IPRangeChecker.IsInRange(ipAddress, range[0], range[1]))
                {
                    return GenerateJWT(userInfo);
                }
                else
                {
                    // The client's IP is not within the range
                    if (userInfo.TwofactorEnabled)
                    {
                        await this.userService.ResetLoginAttemps(userInfo.UserEmail, userInfo.UserGuid);
                        var response = new AuthenticateResponse(userInfo, string.Empty, string.Empty);
                        return Ok(response);
                    }
                }
            }

            if (userInfo.TwofactorEnabled)
            {
                await this.userService.ResetLoginAttemps(userInfo.UserEmail, userInfo.UserGuid);
                var response = new AuthenticateResponse(userInfo, string.Empty, string.Empty);
                return Ok(response);
            }
            return GenerateJWT(userInfo);
        }


        [AllowAnonymous]
        [HttpPost("validate-token-login")]
        public async Task<IActionResult> ExternalLogin2FA(ExternalLoginDto loginInfo)
        {
            var userInfo = await this.userService.GetUserInfoByEmailWithRole(loginInfo.UserEmail);
            if (userInfo == null)
                return BadRequest(new { errorMessage = $"User with Email {loginInfo.UserEmail} is not registered!" });

            try
            {
                if (userInfo.IsConfirmedRegistration.HasValue && !userInfo.IsConfirmedRegistration.Value)
                    return BadRequest(new { errorMessage = $" This acccount is not yet confirmed. Please check your email and confirm registration process." });

                if (userInfo.LoginAttempts > 5)
                    return BadRequest(new { errorMessage = $"This account is blocked due to retry many times. Please contact system administrator to unlock." });

                bool isAuthenticated = await this.userService.IsAuthenticatedExternalUser(userInfo, loginInfo.Password);

                if (!isAuthenticated)
                    return BadRequest(new { errorMessage = $"Unrecognized User email / Password combination." });

                var encryptedSecretString = userInfo.EncryptedSecret;

                if (string.IsNullOrEmpty(encryptedSecretString))
                    return StatusCode(StatusCodes.Status500InternalServerError, new { errorMessage = $"User's secret cannot be found." });

                var issuer = appConfig.MicrosoftAuthenticationApp.Issure;
                var accountName = userInfo.UserEmail.Split('@')[0];
                var secretString = EncryptDecrypt.Decrypt(encryptedSecretString, appConfig.MicrosoftAuthenticationApp.SecreteKey);
                var sd = new SecretData(Uri.EscapeDataString(issuer), Uri.EscapeDataString(userInfo.UserEmail), secretString, accountName);

                Totp totpInstance;
                if (userInfo.EmailOTP)
                {
                    totpInstance = new Totp(Base32Encoding.ToBytes(secretString), step: 150);
                }
                else
                {
                    totpInstance = new Totp(Base32Encoding.ToBytes(secretString));
                }

                if (totpInstance.VerifyTotp(loginInfo.Token, out long timedWindowUsed, new VerificationWindow(appConfig.MicrosoftAuthenticationApp.ValidationWindow)))
                {
                    //if login successfully then reset login history then 

                    return GenerateJWT(userInfo);
                }
                else
                {
                    return BadRequest(new { errorMessage = "Token Validation Fail." });
                }
            }
            catch (Exception ex)
            {
                await this.userService.UpdateLoginAttempts(userInfo.UserEmail, userInfo.UserGuid);
                return BadRequest(new { errorMessage = $"Error occured. {ex.Message}" });
            }
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = string.IsNullOrWhiteSpace(model.Token) ? Request.Cookies["refreshToken"] : model.Token;
            string ipAddress = CommonHelper.GetIPAddress(HttpContext);
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest(new { errorMessage = "Token is required" });

            // replace old refresh token with a new one and save
            var newRefreshToken = this.tokenService.GenerateRefreshToken(ipAddress, appConfig.Jwt);
            newRefreshToken.ReplacedByToken = string.Empty;
            newRefreshToken.RevokedByIp = string.Empty;

            var userId = this.userService.UpdateRefreshToken(refreshToken, newRefreshToken, ipAddress);

            var userInfo = await this.userService.GetUserInfoByUserId(userId);
            if (userInfo == null)
                return BadRequest(new { errorMessage = "User Not found" });

            // generate new jwt
            var jwtToken = this.tokenService.GenerateJwtToken(userInfo, appConfig.Jwt);

            var response = new AuthenticateResponse(userInfo, jwtToken, newRefreshToken.Token);

            if (response == null)
                return Unauthorized(new { errorMessage = "Invalid token" });

            SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("revoke-token")]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest model)
        {
            // accept token from request body or cookie
            var token = string.IsNullOrWhiteSpace(model.Token) ? Request.Cookies["refreshToken"] : model.Token;

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { errorMessage = "Token is required" });

            this.userService.RevokeToken(token, CommonHelper.GetIPAddress(HttpContext));

            return Ok(new { message = "Token revoked" });
        }

        [AllowAnonymous]
        [HttpPost("confirm-registration")]
        public async Task<IActionResult> ConfirmRegistration(ConfirmRegistrationDto model)
        {
            var user = await this.userService.GetUserInfo(model.LoginGuid);

            if (user == null) return BadRequest(new { errorMessage = "User does not exist." });

            if (user.Salt is null) return BadRequest(new { errorMessage = "Something went wrong." });
            //check passed confirmation code is correct or not
            byte[] salt = user.Salt;

            var hashedConfirmationCode = !string.IsNullOrWhiteSpace(user.HashedConformationCode) ? user.HashedConformationCode.ToString() : string.Empty;

            string hashedInputConfirmationCode = Convert.ToBase64String(KeyDerivation.Pbkdf2(
             password: model.ConfirmationCode,
             salt: salt,
             prf: KeyDerivationPrf.HMACSHA256,
             iterationCount: 10000,
             numBytesRequested: 256 / 8));

            if (hashedInputConfirmationCode != hashedConfirmationCode)
            {
                return Unauthorized(new { errorMessage = "The confirmation code is not correct." });
            }

            var hashedInputPassword = SecurityHelper.GetHasedText(salt, model.Password);
            await this.userService.ConfirmRegistration(hashedInputPassword, salt, model.LoginGuid);

            return Ok(new { messsage = $"Thank you for your email confirmation." });
        }

        [AllowAnonymous]
        [HttpPost("generate-qr")]
        public async Task<IActionResult> GenerateQr(ConfirmLinkWithPasswordDto model)
        {
            #region Vaidate Guid 
            LoginHistoryDto? history;
            try
            {
                history = await this.loginHistoryService.GetLoginHistory(model.LoginGuid, true, "QR");
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { errorMessage = "This link is not valid any more. Please request again." });
            }

            if (history == null)
            {
                return BadRequest(new { errorMessage = "This link is not valid any more. Please request again." });
            }

            if (DateTime.UtcNow.CompareTo(history.ResetExpiryTime) > 1)
            {
                return BadRequest("The reset password history link is not valid any more. Please request again.");
            }
            #endregion

            var user = await this.userService.GetUserInfoByEmail(model.UserEmail);

            if (user == null)
                return BadRequest(new { errorMessage = "User does not exists." });

            if (user.LoginAttempts > 5)
                return BadRequest(new { errorMessage = "This account is blocked due to retry many times. Please contact system administrator to unlock." });

            if (user.IsConfirmedRegistration.HasValue && !user.IsConfirmedRegistration.Value)
                return BadRequest(new { errorMessage = "This acccount is not yet confirmed." });

            if (history.LoginGuid != user.UserGuid)
                return BadRequest(new { errorMessage = "This link is not valid." });


            if (user.Salt is null) return BadRequest(new { errorMessage = "Something went wrong." });

            byte[] salt = user.Salt;

            var hashedPassword = user.UserPasswordHash;

            // Hash the user input password 
            string hashedInputPassword = SecurityHelper.GetHasedText(salt, model.Password);

            if (hashedInputPassword != hashedPassword)
            {
                await this.userService.UpdateLoginAttempts(user.UserEmail, user.UserGuid);
                return BadRequest(new { errorMessage = "Unrecognized User email / Password combination." });
            }


            if (user.EncryptedSecret is not null)
            {
                var existingSecretString = EncryptDecrypt.Decrypt(user.EncryptedSecret, appConfig.MicrosoftAuthenticationApp.SecreteKey);
                var userName = user.UserEmail.Split('@')[0];
                var existingSd = new SecretData(
                                 Uri.EscapeDataString(appConfig.MicrosoftAuthenticationApp.Issure),
                                 Uri.EscapeDataString(user.UserEmail),
                                 existingSecretString,
                                 userName
                                 );
                var qrResult = SecurityHelper.GenerateQRImage(existingSd);
                await this.userService.ResetLoginAttemps(user.UserEmail, user.UserGuid);
                if (qrResult != null)
                {
                    var file = File(qrResult.FileContents, qrResult.ContentType, qrResult.FileName);
                    HttpContext.Items["GenerateQRActionResult"] = file;
                    return file;
                }
            }
            else
            {
                // if user doesn't register 2FA
                var secret = KeyGeneration.GenerateRandomKey();

                var secretString = Base32Encoding.ToString(secret);
                var encryptSecretString = EncryptDecrypt.Encrypt(secretString, appConfig.MicrosoftAuthenticationApp.SecreteKey);
                await this.userService.AddTwoFactorAuthentication(user.UserEmail, encryptSecretString, user.IsActive);
                var accountName = user.UserEmail.Split('@')[0];
                var sd = new SecretData(
                    Uri.EscapeDataString(appConfig.MicrosoftAuthenticationApp.Issure),
                    Uri.EscapeDataString(user.UserEmail),
                    secretString,
                    accountName
                    );

                await this.userService.ResetLoginAttemps(user.UserEmail, user.UserGuid);
                var qrResult = SecurityHelper.GenerateQRImage(sd);
                if (qrResult != null)
                {
                    var file = File(qrResult.FileContents, qrResult.ContentType, qrResult.FileName);
                    HttpContext.Items["GenerateQRActionResult"] = file;
                    return file;
                }
            }
            return BadRequest(new { errorMessage = $"Some error occured." });
        }


        [AllowAnonymous]
        [HttpPost("reset-qr")]
        public async Task<IActionResult> RetrieveQr(LoginDto model)
        {
            var user = await this.userService.GetUserInfoByEmail(model.UserEmail);

            if (user == null)
                return BadRequest(new { errorMessage = "User does not exists." });

            if (user.LoginAttempts > 5)
                return BadRequest(new { errorMessage = "This account is blocked due to retry many times. Please contact system administrator to unlock." });

            if (user.IsConfirmedRegistration.HasValue && !user.IsConfirmedRegistration.Value)
                return BadRequest(new { errorMessage = "This acccount is not yet confirmed." });


            if (user.Salt is null) return BadRequest(new { errorMessage = "Something went wrong." });

            byte[] salt = user.Salt;

            var hashedPassword = user.UserPasswordHash;

            // Hash the user input password 
            string hashedInputUserPassword = SecurityHelper.GetHasedText(salt, model.Password);

            if (hashedInputUserPassword != hashedPassword)
            {
                await this.userService.UpdateLoginAttempts(user.UserEmail, user.UserGuid);
                return BadRequest(new { errorMessage = "Unrecognized User email / Password combination." });
            }
            user.UserPasswordHash = hashedInputUserPassword;


            byte[] saltnew = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltnew);
            }

            var confirmationCode = SecurityHelper.GetConfirmationCode();
            string hashedConfirmationCode = SecurityHelper.GetHasedText(saltnew, confirmationCode);

            user.HashedConformationCode = hashedConfirmationCode;
            user.Salt = saltnew;
            user.IsConfirmedRegistration = true;
            // Hash the user input password 
            string hashedInputPassword = SecurityHelper.GetHasedText(saltnew, model.Password);

            user.UserPasswordHash = hashedInputPassword;

            await userService.UpdateUser(user);
            this.loginHistoryService.ResetLoginHistory(user.UserGuid, user.UserGuid);
            var expiryMins = appConfig.Password.ResetPasswordExpiryMins;
            var expiryDateTime = DateTime.UtcNow.AddHours(expiryMins);
            var history = new LoginResetHistoryDto()
            {
                HashedConfirmationCode = hashedConfirmationCode,
                IsActive = true,
                LoginGuid = user.UserGuid,
                ResetExpiryTime = expiryDateTime,
                ResetType = "QR",
                Salt = saltnew
            };
            this.loginHistoryService.CreateLoginHistory(history, user.UserGuid);

            var viewDataOrViewBag = new Dictionary<string, object>();
            viewDataOrViewBag["WebUrl"] = appConfig.AdminUrl.Replace("/#/", "");
            var emailTemplate = new
            {
                UserGuid = user.UserGuid,
                ConfirmationCode = confirmationCode,
                FrontEndUrl = appConfig.AdminUrl,
                user.FirstName,
            };

            EmailDto email = new EmailDto()
            {
                AttachmentList = new List<System.Net.Mail.Attachment>(),
                Bcc = "",
                Cc = "",
                Subject = "Request to retrieve your QR code.",
                To = user.UserEmail,
                IsBodyHtml = true
            };

            email.Mailcontent = await RazorTemplateEngine.RenderAsync("Template/RetriveQr.cshtml", emailTemplate, viewDataOrViewBag);

            if (!appConfig.EmailSettings.IsSimulator)
            {
                emailService.SendEmail(email, appConfig.SMTPSettings, appConfig.EmailSettings);
            }

            return Ok(new { message = $"System will send a email to you. Please check." });
        }

        [AllowAnonymous]
        [HttpPost("retrive-password")]
        public async Task<IActionResult> RetrivePassword(string userEmail)
        {
            var user = await this.userService.GetUserInfoByEmail(userEmail);

            if (user == null)
                return BadRequest(new { errorMessage = "User does not exists." });

            if (user.LoginAttempts > 5)
                return BadRequest(new { errorMessage = "This account is blocked due to retry many times. Please contact system administrator to unlock." });

            if (user.IsConfirmedRegistration.HasValue && !user.IsConfirmedRegistration.Value)
                return BadRequest(new { errorMessage = "This acccount is not yet confirmed." });


            byte[] saltnew = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltnew);
            }

            var confirmationCode = SecurityHelper.GetConfirmationCode();
            string hashedConfirmationCode = SecurityHelper.GetHasedText(saltnew, confirmationCode);

            this.loginHistoryService.ResetLoginHistory(user.UserGuid, user.UserGuid);

            var expiryMins = appConfig.Password.ResetPasswordExpiryMins;
            var expiryDateTime = DateTime.UtcNow.AddHours(expiryMins);
            var history = new LoginResetHistoryDto()
            {
                HashedConfirmationCode = hashedConfirmationCode,
                IsActive = true,
                LoginGuid = user.UserGuid,
                ResetExpiryTime = expiryDateTime,
                ResetType = "Password",
                Salt = saltnew
            };

            this.loginHistoryService.CreateLoginHistory(history, user.UserGuid);
            var viewDataOrViewBag = new Dictionary<string, object>();
            viewDataOrViewBag["WebUrl"] = appConfig.AdminUrl.Replace("/#/", "");
            EmailDto email = new EmailDto()
            {
                AttachmentList = new List<System.Net.Mail.Attachment>(),
                Bcc = "",
                Cc = "",
                Subject = "Request to reset your password.",
                To = user.UserEmail,
                IsBodyHtml = true
            };

            var emailTemplate = new
            {
                UserGuid = user.UserGuid,
                ConfirmationCode = confirmationCode,
                FrontEndUrl = appConfig.AdminUrl,
                user.FirstName,
            };
            email.Mailcontent = await RazorTemplateEngine.RenderAsync("Template/ResetPassword.cshtml", emailTemplate, viewDataOrViewBag);

            if (!appConfig.EmailSettings.IsSimulator)
            {
                emailService.SendEmail(email, appConfig.SMTPSettings, appConfig.EmailSettings);
            }

            return Ok(new { message = $"System will send a email to you. Please check." });
        }

        [AllowAnonymous]
        [HttpPost("confirm-link")]
        public async Task<IActionResult> ConfirmLink(ConfirmLinkDto confirm)
        {
            LoginHistoryDto? history;
            try
            {
                history = await this.loginHistoryService.GetLoginHistory(confirm.LoginGuid, true, confirm.ResetType);
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { errorMessage = "This link is not valid any more. Please request again." });
            }

            if (history == null)
            {
                return BadRequest(new { errorMessage = "This link is not valid any more. Please request again." });
            }

            if (DateTime.UtcNow.CompareTo(history.ResetExpiryTime) > 1)
            {
                return BadRequest("The reset password history link is not valid any more. Please request again.");
            }

            byte[] salt = history.Salt;
            var hashedConfirmationCode = history.HashedConfirmationCode;

            string hashedInputConfirmationCode = UtilityHelper.GenerateHashedConfirmationCode(confirm.ConfirmationCode, salt);

            if (hashedInputConfirmationCode != hashedConfirmationCode)
            {
                return BadRequest("The confirmation code is not correct.");
            }
            return Ok(new { message = $"Success", userEmail = history.UserEmail });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ConfirmLinkWithPasswordDto confirm)
        {
            #region Validate Link
            LoginHistoryDto? history;
            try
            {
                history = await this.loginHistoryService.GetLoginHistory(confirm.LoginGuid, true, "QR");
            }
            catch (InvalidOperationException)
            {
                return BadRequest(new { errorMessage = "This link is not valid any more. Please request again." });
            }

            if (history == null)
            {
                return BadRequest(new { errorMessage = "This link is not valid any more. Please request again." });
            }

            if (DateTime.UtcNow.CompareTo(history.ResetExpiryTime) > 1)
            {
                return BadRequest("The reset password history link is not valid any more. Please request again.");
            }

            byte[] salt = history.Salt;
            var hashedConfirmationCode = history.HashedConfirmationCode;

            string hashedInputConfirmationCode = UtilityHelper.GenerateHashedConfirmationCode(confirm.ConfirmationCode, salt);

            if (hashedInputConfirmationCode != hashedConfirmationCode)
            {
                return BadRequest("The confirmation code is not correct.");
            }

            #endregion

            var user = await this.userService.GetUserInfoByEmail(history.UserEmail);

            if (user == null)
                return BadRequest(new { errorMessage = "User does not exists." });

            if (user.LoginAttempts > 5)
                return BadRequest(new { errorMessage = "This account is blocked due to retry many times. Please contact system administrator to unlock." });

            if (user.IsConfirmedRegistration.HasValue && !user.IsConfirmedRegistration.Value)
                return BadRequest(new { errorMessage = "This acccount is not yet confirmed." });

            if (history.LoginGuid != user.UserGuid)
                return BadRequest(new { errorMessage = "This link is not valid." });

            byte[] saltnew = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltnew);
            }

            user.HashedConformationCode = hashedConfirmationCode;
            user.Salt = saltnew;
            user.IsConfirmedRegistration = true;
            // Hash the user input password 
            string hashedInputPassword = SecurityHelper.GetHasedText(saltnew, confirm.Password);

            user.UserPasswordHash = hashedInputPassword;

            await userService.UpdateUser(user);
            this.loginHistoryService.ResetLoginHistory(user.UserGuid, user.UserGuid);

            return Ok(new { message = $"Success", userEmail = history.UserEmail });
        }
        #region Private 
        private IActionResult GenerateJWT(UserInfoWithRoleDto userInfo)
        {
            var ipAddress = CommonHelper.GetIPAddress(HttpContext);
            var jwtToken = this.tokenService.GenerateJwtToken(userInfo, appConfig.Jwt);
            var refreshToken = this.tokenService.GenerateRefreshToken(ipAddress, appConfig.Jwt);
            refreshToken.ReplacedByToken = string.Empty;
            refreshToken.RevokedByIp = string.Empty;
            refreshToken.UserGuid = userInfo.UserGuid;
            this.userService.AddRefreshToken(refreshToken);
            var response = new AuthenticateResponse(userInfo, jwtToken, refreshToken.Token);
            SetTokenCookie(response.RefreshToken);
            var accountName = userInfo.UserEmail.Split('@')[0];

            var userClaims = new LoggedInUserInfo()
            {
                UserName = accountName,
                UserId = userInfo.UserGuid,
                IPAddress = ipAddress,
                Role = userInfo.RoleName,
                UserEmail = userInfo.UserEmail,
            };
            HttpContext.Items["UserClaims"] = userClaims as LoggedInUserInfo;
            this.userService.ResetLoginAttemps(userInfo.UserEmail, userInfo.UserGuid);
            return Ok(response);
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        #endregion
    }
}
