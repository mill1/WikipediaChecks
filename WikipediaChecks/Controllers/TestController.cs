using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using WikipediaChecks.Models;
namespace WikipediaChecks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly ILogger<TestController> logger;

        public TestController(ILogger<TestController> logger)
        {
            this.logger = logger;
        }

        [HttpPost("AddEmployee")]
        public IActionResult InsertEmployeeData([FromForm] Employee employee)
        {
            //return Content($"Hello {employee.FirstName}");
            return Ok(employee);
        }



        [HttpGet("billionbirthday/{datetime}")]
        public IActionResult AddBillionSecondsToDateTime(DateTime datetime)
        {
            return Ok($"YOUR BILLIONTH SECOND: {datetime.AddSeconds(Math.Pow(10, 9)).ToString("d MMMM yyyy HH:mm:ss")}");
        }
    }
}
