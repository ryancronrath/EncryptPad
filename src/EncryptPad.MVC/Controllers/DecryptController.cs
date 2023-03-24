using EncryptPad.Shared;
using EncryptPad.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace EncryptPad.MVC.Controllers
{
    public class DecryptController : Controller
    {
        private readonly SqliteOneTimePadService _otpService;

        public DecryptController(SqliteOneTimePadService otpService)
        {
            _otpService = otpService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> DecryptText([FromBody] EncryptedText encryptedText)
        {
            try
            {
                var decryptedText = await _otpService.DecryptTextAsync(encryptedText);
                return Json(decryptedText);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}