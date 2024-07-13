namespace Np.ViewModel
{
    public class AppConfig
    {
        public string AdminUrl { get; set; }
        public string AdminUrlValue
        {
            get
            {
#if DEBUG
                return this.AdminUrl;
#else
   return    Environment.GetEnvironmentVariable("AdminUrl");
#endif
            }
        }
        public Password Password { get; set; }

        public DBConfig ConnectionStrings { get; set; }

        public DomainConfig DomainConfig { get; set; }

        public MicrosoftAuthenticationApp MicrosoftAuthenticationApp { get; set; }

        public JWT Jwt { get; set; }

        public EmailSettings EmailSettings { get; set; }
        public SMTPSettings SMTPSettings { get; set; }
        public FilePath FilePath { get; set; }
    }

    public class JWT
    {
        public string Key { get; set; }
        public int TokenExpiryMins { get; set; }
        public int RefreshTokenExpiryMins { get; set; }
    }

    public class DBConfig
    {
        public string NewsSystemDatabase { get; set; }
        public string NewsSystemDatabaseValue
        {
            get
            {
#if DEBUG
                return this.NewsSystemDatabase;
#else
   return Environment.GetEnvironmentVariable("NewsSystemDatabase");
#endif
            }
        }

    }


    public class Password
    {
        public int ResetPasswordExpiryMins { get; set; }
    }

    public class DomainConfig
    {
        public string DomainName { get; set; } = string.Empty;
        public string DomainPath { get; set; } = string.Empty;
        public List<string> Properties { get; set; } = new List<string>();
    }

    public class MicrosoftAuthenticationApp
    {
        public string Issure { get; set; } = string.Empty;

        public int ValidationWindow { get; set; } = 1;

        public string SecreteKey { get; set; } = string.Empty;
    }

    public class EmailSettings
    {
        public string FromName { get; set; } = string.Empty;
        public bool IsSimulator { get; set; } = false;
    }

    public class SMTPSettings
    {
        public string SmtpServerName { get; set; } = string.Empty;
        public string MailFrom { get; set; } = string.Empty;
        public bool EnableSsl { get; set; } = false;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class FilePath
    {
        public string PostImage { get; set; } = string.Empty;
        public string CategoryImage { get; set; } = string.Empty;
    }
}
