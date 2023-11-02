using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AptitudeTest.Common.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;

        public EmailHelper(IConfiguration config)
        {
            _config = config;
        }
        public bool SendEmail(string to, string subject, string body, List<string> attachments = null)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(_config["EmailGeneration:Host"], Convert.ToInt16(_config["EmailGeneration:Port"])))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(_config["EmailGeneration:FromEmail"], _config["EmailGeneration:Key"]);

                    using (MailMessage mailMessage = new MailMessage())
                    {

                        mailMessage.From = new MailAddress(_config["EmailGeneration:FromEmail"], _config["EmailGeneration:DisplayName"]);
                        mailMessage.To.Add(to);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;

                        if (attachments != null)
                        {
                            foreach (string attachmentPath in attachments)
                            {
                                mailMessage.Attachments.Add(new Attachment(attachmentPath));
                            }
                        }

                        smtpClient.Send(mailMessage);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }
    }
}
