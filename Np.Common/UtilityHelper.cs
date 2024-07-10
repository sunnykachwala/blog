namespace Np.Common
{
    using Microsoft.AspNetCore.Cryptography.KeyDerivation;
    using System.Security.Cryptography;
    using System.Text.RegularExpressions;
    using System.Text;
    using System.Reflection;

    public static class UtilityHelper
    {
        public static byte[] GenerateSalt()
        {
            // Generate a salt
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

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

        public static string GenerateConfirmationCode()
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

        public static string GenerateHashedPassword(byte[] salt, string password)
        {
            // Hash the password
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashedPassword;
        }
        public static string GenerateHashedConfirmationCode(string confirmationCode, byte[] salt)
        {
            string hashedConfirmationCode = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: confirmationCode,
                 salt: salt,
                 prf: KeyDerivationPrf.HMACSHA256,
                 iterationCount: 10000,
                 numBytesRequested: 256 / 8));

            return hashedConfirmationCode;
        }

        public static string GenerateRandomCode(int length = 7)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string ToUrlSlug(this string value)
        {
            //First to lower case 
            value = value.ToLowerInvariant();

            //Remove all accents
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);

            value = Encoding.ASCII.GetString(bytes);

            //Convert multiple spaces into one space   
            value = Regex.Replace(value, @"\s+", " ").Trim();

            //Remove all non valid chars
            value = Regex.Replace(value, @"[^a-z0-9\s-]", "");

            //Replace spaces 
            value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);

            //Remove invalid chars 
            value = Regex.Replace(value, @"[^\w\s\p{Pd}]", "", RegexOptions.Compiled);

            //Trim dashes from end 
            value = value.Trim('-', '_');

            //Replace double occurences of - or \_ 
            value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);

            return value;
        }
        public static List<AuditedDataDto> GetUpdatedPropertiesData<T>(T oldObject, T newObject)
        {
            List<string> propertyNames = new List<string>() { "SystemUser", "AppName", "ModifiedAt", "Richmond", "MedicalEvent", "MedicalEventAdditionalData", "EventDetail", "DairyCard" };

            var updatedProperties = new List<AuditedDataDto>();
            var properties = oldObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(IgnoreForAuditAttribute)))
                {
                    continue;
                }
                var oldValue = property.GetValue(oldObject);
                var newValue = property.GetValue(newObject);


                if (!Equals(oldValue, newValue) && !propertyNames.Contains(property.Name))
                {
                    updatedProperties.Add(new AuditedDataDto()
                    {
                        Key = property.Name,
                        OldValue = oldValue != null ? oldValue.ToString() : "",
                        NewValue = newValue != null ? newValue.ToString() : "",
                    });
                }
            }

            return updatedProperties;
        }

    }
    public class AuditedDataDto
    {
        public string Key { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}
