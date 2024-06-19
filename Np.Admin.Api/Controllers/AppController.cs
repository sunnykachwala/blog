namespace Np.Admin.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Np.Admin.Service.AppSettings;
    using System.Globalization;

    [Route("api/app")]
    [ApiController]
    public class AppController : NpBaseController
    {
        public AppController(IAppSettingService appSettingService) : base(appSettingService)
        {
        }

        [HttpGet("appconfig")]
        public  async Task<IActionResult> AppConfig()
        {
            var appSetting = await GetAppSetting();
            return Ok(new
            {
                appSetting.ApplicationName,
                appSetting.Logo,
                appSetting.AppIcon,
                appSetting.Copyright,
                appSetting.DefaultLanguage,
                appSetting.ThemeAppBar,
                appSetting.ThemeColor,
                appSetting.ThemeLayout,
                appSetting.ThemeSideBar,
                Version = "v1.0.0"
            });
        }

        // Get localization data based on the requested or default language
        [HttpGet("localizationdata/{name}")]
        public async Task<IActionResult> LocalizationData(string name)
        {
            var appSetting = await GetAppSetting();
            var defaultLanguage = appSetting.DefaultLanguage;
            //if (!string.IsNullOrEmpty(name) && name != "default")
            //{
            //    var language = _dbContext.Languages.Find(name);
            //    if (language != null)
            //    {
            //        defaultLanguage = language.Name;
            //    }
            //}

            //var languages = _dbContext.Languages.ToList();

            //var stringResources = _dbContext.StringResources
            //    .Where(x => x.LanguageName == defaultLanguage)
            //    .ToDictionary(x => x.Name, x => x.Value);

            var numberFormat = CultureInfo.GetCultureInfo(defaultLanguage).NumberFormat;
            var isRightToLeft = CultureInfo.GetCultureInfo(defaultLanguage).TextInfo.IsRightToLeft;

            return Ok(new
            {
                StringResources = new List<dynamic>(),
                NumberFormat = numberFormat,
                RegionName = defaultLanguage,
                CurrencyName = appSetting.DefaultCurrency,
                IsRtl = isRightToLeft,
                Languages = new  List<dynamic>(),
                TimeZone = appSetting.DefaultTimezone
            });
        }
    }
}
