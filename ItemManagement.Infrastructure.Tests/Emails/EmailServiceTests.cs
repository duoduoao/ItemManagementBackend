using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemManagement.Infrastructure.Tests.Emails
{
    public class EmailServiceTests
    {
        public interface ISendGridClientWrapper
        {
            Task<Response> SendEmailAsync(SendGridMessage msg);
        }

        public class SendGridClientWrapper : ISendGridClientWrapper
        {
            private readonly SendGridClient _client;
            public SendGridClientWrapper(string apiKey) => _client = new SendGridClient(apiKey);
            public Task<Response> SendEmailAsync(SendGridMessage msg) => _client.SendEmailAsync(msg);
        }

        public class EmailService
        {
            private readonly ISendGridClientWrapper _clientWrapper;
            private readonly EmailAddress _from = new EmailAddress("hello@a.com", "name");

            public EmailService(ISendGridClientWrapper clientWrapper)
            {
                _clientWrapper = clientWrapper;
            }

            public async Task<bool> SendEmailAsync(string email, string subject, string message)
            {
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(_from, to, subject, "", message);
                var response = await _clientWrapper.SendEmailAsync(msg);
                return response.StatusCode == System.Net.HttpStatusCode.Accepted
                    || response.StatusCode == System.Net.HttpStatusCode.OK;
            }
        }

    }
}
