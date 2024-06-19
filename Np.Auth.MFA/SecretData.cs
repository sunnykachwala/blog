namespace Np.Auth.MFA
{
    using QRCoder;
    using System.Drawing;
    using System.Web;

    public class SecretData
    {

        public SecretData(string issuer, string email, string secret, string accountName)
        {
            Issuer = issuer;
            Email = email;
            Secret = secret;
            AccountName = accountName;
        }

        public string GenQRCodeUrl() =>
           $"otpauth://totp/{HttpUtility.UrlPathEncode(Issuer)}:{HttpUtility.UrlPathEncode(Email)}?secret={Secret}&issuer={HttpUtility.UrlPathEncode(Issuer)}&algorithm=SHA1&digits=6&period=30&name={HttpUtility.UrlPathEncode(AccountName)}";
        public string Issuer { get; private set; }
        public string Email { get; private set; }

        public string Secret { get; private set; }
        public string AccountName { get; private set; }

        public Bitmap GenQRCode()
        {
            var qrcg = new QRCodeGenerator();
            var data = qrcg.CreateQrCode(GenQRCodeUrl(), QRCodeGenerator.ECCLevel.Q);
            var qrc = new QRCode(data);
            return qrc.GetGraphic(20);
        }
    }
}