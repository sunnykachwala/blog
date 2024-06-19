namespace Np.Auth.MFA.Common
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using OtpNet;
    using System.Drawing;
    using System.Security.Cryptography;

    public class SecurityHelper
    {
        public static string GenerateSecurePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,./<>?";
            var random = new Random();
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }

            return new string(chars);
        }

        public static string EncodedRandomKey()
        {
            var secret = KeyGeneration.GenerateRandomKey();

            return Base32Encoding.ToString(secret);
        }

        public static MSAuthenticatorQRImage GenerateQRImage(SecretData secretData)
        {
            var qrCodeImgFile = Path.GetTempFileName() + ".png";
            var fileName = Path.GetFileName(qrCodeImgFile);

            secretData.GenQRCode().Save(qrCodeImgFile, System.Drawing.Imaging.ImageFormat.Png);
            Image img = Image.FromFile(qrCodeImgFile);

            byte[] fileContents;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                fileContents = ms.ToArray();
            }
            return new MSAuthenticatorQRImage("image/png", fileName, fileContents);
        }
        public static string GetHasedText(byte[] salt,string randomPassword)
        {           
            // Hash the password
            string password = randomPassword;
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashedPassword;
        }

        public static string GetConfirmationCode()
        {
            // generate confirmation code
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var confirmationCode = new string(
                Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());


            return confirmationCode;
        }

        public (string hashedPassword, string hashedConfirmationCode) GetHasedPasswordAndConfirmation(string randomPassword)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // generate confirmation code
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var confirmationCode = new string(
                Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());

            // Hash the password
            string password = randomPassword;
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            string hashedConfirmationCode = Convert.ToBase64String(KeyDerivation.Pbkdf2(
              password: confirmationCode,
              salt: salt,
              prf: KeyDerivationPrf.HMACSHA256,
              iterationCount: 10000,
              numBytesRequested: 256 / 8));

            return (hashedPassword, hashedConfirmationCode);
        }
    }
}