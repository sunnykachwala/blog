namespace Np.Auth.MFA
{
    public class MSAuthenticatorQRImage
    {
        public string ContentType { get; private set; }
        public string FileName { get; private set; }
        public byte[] FileContents { get; private set; }

        public MSAuthenticatorQRImage(string contentType, string fileName, byte[] fileContents)
        {
            ContentType = contentType;
            FileName = fileName;
            FileContents = fileContents;
        }
    }
}