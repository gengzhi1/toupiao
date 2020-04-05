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
            IConfiguration _configuration , //在startup。cs中的参数
            string sendTo,//接收邮件的人
            string sendSubject,// 邮件的主题
            string sendBody// 邮件的内容
            )
        {
            //一封电子邮件
            var message = new MimeMessage ()
            {
                Subject = sendSubject,
                Body = new TextPart("html")
                {
                    Text = sendBody
                },
            };

            // 发送人
			message.From.Add (new MailboxAddress ( 
                "臧林投票 邮件处", 
                _configuration["EmailSender:SmtpUserName"]));
            
            //注册人的名称和电子邮件
			message.To.Add (new MailboxAddress (sendTo, sendTo));

			var client = new SmtpClient ();
            client.Connect (
                host: _configuration["EmailSender:SmtpHost"], 
                //Int32.Parse作用是将端口字符串的转换成整数
                port: Int32.Parse( _configuration["EmailSender:SmtpPort"]), 
                useSsl :true
            );

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate (
                _configuration["EmailSender:SmtpUserName"], 
                _configuration["EmailSender:SmtpPassword"]);

            return client.SendAsync(message);  //发送邮件
			
        }
    }
}