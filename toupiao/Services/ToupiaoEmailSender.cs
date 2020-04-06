using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace toupiao.Services
{
    public static class ToupiaoEmailSender
    {
        public static Task SendEmailAnync( 
            
            IConfiguration _configuration , 
            string sendTo,
            string sendSubject,
            string sendBody
            )
        {
            var message = new MimeMessage ()
            {
                Subject = sendSubject,
                Body = new TextPart("html")
                {
                    Text = sendBody
                },
            };

			message.From.Add (new MailboxAddress ( 
                _configuration["EmailSender:SmtpUserName"] , 
                _configuration["EmailSender:SmtpUserName"]));
                
			message.To.Add (new MailboxAddress (sendTo, sendTo));

			var client = new SmtpClient ();
            client.Connect (
                _configuration["EmailSender:SmtpHost"], 
                Int32.Parse( _configuration["EmailSender:SmtpPort"]), 
                true
            );

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate (
                _configuration["EmailSender:SmtpUserName"], 
                _configuration["EmailSender:SmtpPassword"]);

            return client.SendAsync(message);
			
        }
    }
}