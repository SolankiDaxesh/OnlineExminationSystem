using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OESRegistraionAPI.IRepository;
using OESRegistraionAPI.Models;
using System.Text;

namespace OnlineExaminationSystem.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IRgistrationRepository _rgistration;
        private readonly IConfiguration _configuration;
        private string _regApi;
        public RegistrationController(IRgistrationRepository rgistration, IConfiguration configuration)
        {
            _rgistration = rgistration;
            _configuration = configuration;
            _regApi = _configuration.GetValue<string>("APIPath:RegApi").ToString();

        }
        [HttpPost]
        public async Task<IActionResult> StudentLogin(Registration registration)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
                using (var response = await httpClient.PostAsync(_regApi, content))
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return View();
                    }
                    else
                        ViewBag.StatusCode = response.StatusCode;
                }
            }
            return View();
        }
    }
}
