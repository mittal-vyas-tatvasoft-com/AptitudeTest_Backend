using System.Net.Mail;

namespace AptitudeTest.Core.ViewModels
{
    public class EmailDataVm
    {
        public MailAddress? FromAddress { get; set; }
        public MailAddress? ToAddress { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
