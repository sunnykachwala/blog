namespace Np.ViewModel
{
    using System.Net.Mail;
    public class EmailDto
    {
        public string Mailcontent { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;
        public string Cc { get; set; } = string.Empty;
        public string Bcc { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public List<Attachment> AttachmentList { get; set; } = new List<Attachment>();
        public bool IsBodyHtml { get; set; } = true;
    }
}
