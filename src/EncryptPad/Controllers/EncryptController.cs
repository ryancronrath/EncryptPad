using EncryptPad.Shared;
using EncryptPad.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace EncryptPad.Controllers
{
    public class EncryptController : Controller
    {
        private readonly SqliteOneTimePadService _otpService;

        public EncryptController(SqliteOneTimePadService otpService)
        {
            _otpService = otpService;
        }

        public IActionResult Index()
        {
            return View("~/Views/Encrypt/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> EncryptText([FromBody] UnencryptedText unencryptedText)
        {
            try
            {
                var payload = await _otpService.EncryptTextAsync(unencryptedText);
                return Json(payload);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}