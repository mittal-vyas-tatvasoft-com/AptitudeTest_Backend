using AptitudeTest.Core.Entities.Setting;
using APTITUDETEST.Common.Data;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace AptitudeTest.Common.Helpers
{
    public class EmailHelper
    {
        private readonly IConfiguration _config;
        readonly AppDbContext _context;

        public EmailHelper(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }
        public bool SendEmail(string to, string subject, string body, List<string> attachments = null)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(_config["EmailGeneration:Host"], Convert.ToInt16(_config["EmailGeneration:Port"])))
                {
                    SettingConfigurations? settingConfiguration = _context.SettingConfigurations.FirstOrDefault();
                    string email = settingConfiguration.Email;
                    string key = settingConfiguration.Password;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    //smtpClient.Credentials = new NetworkCredential(_config["EmailGeneration:FromEmail"], _config["EmailGeneration:Key"]);
                    smtpClient.Credentials = new NetworkCredential(email, key);

                    using (MailMessage mailMessage = new MailMessage())
                    {

                        //mailMessage.From = new MailAddress(_config["EmailGeneration:FromEmail"], _config["EmailGeneration:DisplayName"]);
                        mailMessage.From = new MailAddress(email, _config["EmailGeneration:DisplayName"]);
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
