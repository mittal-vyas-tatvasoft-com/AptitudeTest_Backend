using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AptitudeTest.Common.Helpers
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public bool SendEmail(string to, string subject, string body, List<string> attachments = null)
        {
            string host = _config["EmailGeneration:SmtpHost"];
            string userName = _config["EmailGeneration:SmtpUserName"];
            string password = _config["EmailGeneration:SmtpPassword"];
            string serverPort = _config["EmailGeneration:SmtpPort"];
            string mailFrom = _config["EmailGeneration:SmtpUserName"];
            bool sslEnabled = Convert.ToBoolean(_config["EmailGeneration:SmtpSslEnabled"]);
            bool byPassAuthentication = Convert.ToBoolean(_config["EmailGeneration:SmtpBypassAuthentication"]);

            SmtpClient? smtp = null;
            MailMessage mailMessage = new();
            bool isMailSend = false;


            try
            {
                smtp = new SmtpClient(host, Convert.ToInt32(serverPort));
                if (!byPassAuthentication)
                {
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(userName, password);
                }
                else
                {
                    smtp.UseDefaultCredentials = true;
                }

                smtp.EnableSsl = sslEnabled;
                ServicePointManager.SecurityProtocol = sslEnabled ? SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13
                                                        : SecurityProtocolType.SystemDefault;
                smtp.TargetName = "STARTTLS/" + host;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailMessage.From = new MailAddress(mailFrom);
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

                mailMessage.Priority = MailPriority.Normal;
                smtp.Send(mailMessage);
                isMailSend = true;
                return isMailSend;
            }
            catch
            {
                return false;
            }
        }
    }
}
