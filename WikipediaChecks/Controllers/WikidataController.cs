using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Wikimedia.Utilities.Interfaces;

namespace WikipediaChecks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WikidataController : ControllerBase
    {

        private readonly IWikidataService wikidataService;
        private readonly ILogger<WikidataController> logger;

        public WikidataController(IWikidataService wikipediaService, ILogger<WikidataController> logger)
        {
            this.wikidataService = wikipediaService;
            this.logger = logger;
        }

        [HttpGet("deceased/{date}")]
        public IActionResult GetDeceasedByDate(DateTime date)
        {
            try
            {
                return Ok(wikidataService.GetItemsPerDeathDate(date));
            }
            catch (Exception e)
            {
                string message = $"Getting the deceased by date failed. Requested date of death: {date.ToShortDateString()}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }
    }
}
