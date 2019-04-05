using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreExampleSendEmailAttachmentWithMailKit.Models;
using ASPNetCoreExampleSendEmailAttachmentWithMailKit.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASPNetCoreExampleSendEmailAttachmentWithMailKit.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : Controller
    {
        private IAppEmailService _appEmailService;

        [HttpGet]
        public string Status()
        {
            return "Service is up.";
        }


        public EmailController(IAppEmailService appEmailService)
        {
            _appEmailService = appEmailService;
        }
        
        [HttpPost]
        public async Task<IActionResult> SendEmail([FromForm] EmailRequest emailRequest)
        {
            await _appEmailService.SendAsync(emailRequest);
            return new OkResult();
        }
    }
}
