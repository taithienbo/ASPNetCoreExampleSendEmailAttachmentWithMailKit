using System;
using System.Threading.Tasks;
using ASPNetCoreExampleSendEmailAttachmentWithMailKit.Models;
using NETCore.MailKit;
using MimeKit;
using System.IO;
using NETCore.MailKit.Core;

namespace ASPNetCoreExampleSendEmailAttachmentWithMailKit.Services
{
    public class AppEmailService : EmailService, IAppEmailService
    {
        private readonly IMailKitProvider _mailKitProvider; 

        public AppEmailService(IMailKitProvider provider) : base(provider)
        {
            _mailKitProvider = provider; 
        }

        public async Task SendAsync(MimeMessage message)
        {
            message.From.Add(new MailboxAddress(_mailKitProvider.Options.SenderEmail));
            using (var emailClient = _mailKitProvider.SmtpClient)
            {
                if (!emailClient.IsConnected)
                {
                    await emailClient.AuthenticateAsync(_mailKitProvider.Options.Account, 
                    _mailKitProvider.Options.Password);
                    await emailClient.ConnectAsync(_mailKitProvider.Options.Server,
                    _mailKitProvider.Options.Port, _mailKitProvider.Options.Security);
                }
                await emailClient.SendAsync(message);
                await emailClient.DisconnectAsync(true);
            }
        }

        public async Task SendAsync(EmailRequest emailRequest)
        {
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.To.Add(new MailboxAddress(emailRequest.ToAddress));
            mimeMessage.Subject = emailRequest.Subject;
            var builder = new BodyBuilder { HtmlBody = emailRequest.Body };
            if ( emailRequest.Attachment != null)
            { 
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await emailRequest.Attachment.CopyToAsync(memoryStream);
                    builder.Attachments.Add(emailRequest.Attachment.FileName, memoryStream.ToArray());
                }
            
            }
            mimeMessage.Body = builder.ToMessageBody();
            await SendAsync(mimeMessage);
        }
    }
}
