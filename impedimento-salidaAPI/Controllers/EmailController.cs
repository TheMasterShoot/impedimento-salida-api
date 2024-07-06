using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using impedimento_salidaAPI.Services;
using impedimento_salidaAPI.Models;
using impedimento_salidaAPI.Models.DTOs;

namespace impedimento_salidaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDTO request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
