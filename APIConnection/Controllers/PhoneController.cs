using APIConnection.Interfaces;
using APIConnection.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIConnection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneService _phoneService;

        public PhoneController(IPhoneService phoneService)
        {
            _phoneService = phoneService;
        }

        [HttpGet(Name = "Phone")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var phones = await _phoneService.GetPhones();
                return Ok(phones);
            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { message = ex.Message, innerException = ex.InnerException.Message });
            }
        }
    }
}
