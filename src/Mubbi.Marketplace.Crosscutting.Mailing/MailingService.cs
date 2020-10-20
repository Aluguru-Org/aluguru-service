using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.Crosscutting.Mailing
{
    public interface IMailingService
    {
        Task<bool> SendMessageHtml(string sender, string senderEmail, string receiver, string receiverEmail, string subject, string body);
    }

    public class MailingService : IMailingService
    {
        private readonly MailingSettings _settings;
        public MailingService(IOptions<MailingSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<bool> SendMessageHtml(string sender, string senderEmail, string receiver, string receiverEmail, string subject, string body)
        {
            var client = new SendGridClient(_settings.ApiKey);
            var message = new SendGridMessage();

            message.SetFrom(new EmailAddress(senderEmail, sender));
            message.AddTo(new EmailAddress(receiverEmail, receiver));

            message.SetSubject(subject);

            message.AddContent(MimeType.Html, body);
            var response = await client.SendEmailAsync(message);
            return (int)response.StatusCode >= 200 && (int)response.StatusCode <= 299;
        }
    }
}
