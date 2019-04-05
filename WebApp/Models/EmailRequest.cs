using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace ASPNetCoreExampleSendEmailAttachmentWithMailKit.Models
{
    public class EmailRequest
    {
        [Required]
        public string ToAddress { get; set; }
        [Required]
        public string Body { get; set; }
        [Required]
        public string Subject { get; set; }
        public IFormFile Attachment { get; set;  }
    }

}
