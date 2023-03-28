using Microsoft.AspNetCore.Mvc;
using OESRegistraionAPI.IRepository;
using OESRegistraionAPI.Models;

namespace OESRegistraionAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly IRgistrationRepository _rgistration;
        public RegistrationController(IRgistrationRepository rgistration)
        {
            _rgistration = rgistration;
        }
        
        [HttpPost]
        public async Task<string> GetUserLogin(Registration reg) 
        {
            string str = string.Empty;
            _rgistration.VerifyUserLogin(reg.EnrollmentId,reg.Password);
            return str;
        }
    }
}
