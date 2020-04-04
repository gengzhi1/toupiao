using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebPWrecover.Services
{
    public class EmailSender : IEmailSender
    {
    var serviceProvidersFeature = HttpContext.Features.Get<IServiceProvidersFeature>();
    var services = serviceProvidersFeature.RequestServices;
    var service = (IServiceProvider)services.GetService(typeof(IServiceProvider));

        var app_json = JsonSerializer.Deserialize( File.ReadAllText() );
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient( "smtp.163.com" );

        }

        public Task Execute(string apiKey, string subject, string message, string email)
        { 
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Joe@contoso.com", "Joe Smith"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}