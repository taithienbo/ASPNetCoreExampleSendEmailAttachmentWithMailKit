using System;
using System.Threading.Tasks;
using ASPNetCoreExampleSendEmailAttachmentWithMailKit.Models;
using MimeKit;
using NETCore.MailKit.Core;

namespace ASPNetCoreExampleSendEmailAttachmentWithMailKit.Services
{
    public interface IAppEmailService : IEmailService
    {
        Task SendAsync(MimeMessage message);
        Task SendAsync(EmailRequest emailRequest);
    }
}
