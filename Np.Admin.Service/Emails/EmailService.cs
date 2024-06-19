namespace Np.Admin.Service.Emails
{
    using Np.ViewModel;
    using System.Net.Mail;

    public class EmailService : IEmailService
    {
        public bool SendEmail(EmailDto email, SMTPSettings smtpSettings, EmailSettings emailSettings)
        {
            string fromDisplayName = "KampherTech";

            if (!string.IsNullOrEmpty(emailSettings.FromName))
            {
                fromDisplayName = emailSettings.FromName;
            }
            using (MailMessage mail = new MailMessage())
            {
                try
                {
                    if (!emailSettings.IsSimulator)
                    {
                        mail.From = new MailAddress(smtpSettings.MailFrom, fromDisplayName);


                        string[] strToAddresses = email.To.Split(';');
                        if (strToAddresses.Length > 0)
                        {
                            foreach (string strAddress in strToAddresses)
                            {
                                if (!string.IsNullOrEmpty(strAddress))
                                {
                                    mail.To.Add(strAddress);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(email.Bcc))
                        {
                            mail.Bcc.Add(email.Bcc);
                        }
                        if (!string.IsNullOrEmpty(email.Cc))
                        {
                            mail.CC.Add(email.Cc);
                        }

                        if (email.AttachmentList != null)
                        {
                            for (int iCnt = 0; iCnt < email.AttachmentList.Count; iCnt++)
                            {
                                mail.Attachments.Add(email.AttachmentList[iCnt]);
                            }
                        }

                        mail.Subject = email.Subject;
                        mail.Body = email.Mailcontent;
                        mail.IsBodyHtml = email.IsBodyHtml;

                        using (SmtpClient SmtpServer = new SmtpClient(smtpSettings.SmtpServerName))
                        {
                            //  SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
                            // SmtpServer.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SMTPUserName"].ToString(), ConfigurationManager.AppSettings["SMTPPassword"].ToString());
                            //  SmtpServer.EnableSsl = true;
                            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                            SmtpServer.Send(mail);
                        }
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
