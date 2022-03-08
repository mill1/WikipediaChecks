using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WikipediaChecks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblyController : ControllerBase
    {
        private readonly ILogger<AssemblyController> logger;

        public AssemblyController(ILogger<AssemblyController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult GetAssemblyInfo()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName();

            return Ok(new { value = $"{assemblyName.Name} version: { assemblyName.Version}" });
        }
    }
}
