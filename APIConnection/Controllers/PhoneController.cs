using APIConnection.Interfaces;
using APIConnection.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIConnection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhoneController : ControllerBase
    {
        private readonly IPhoneService phoneService;

        public PhoneController(IPhoneService phoneService)
        {
            this.phoneService = phoneService;
        }

        [HttpGet(Name = "Phone")]
        public IEnumerable<Phone> Get()
        {
            var result = this.phoneService.GetPhones();
            return result.Result;
        }
    }
}
