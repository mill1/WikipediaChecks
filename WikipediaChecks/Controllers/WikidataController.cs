using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Wikimedia.Utilities.Interfaces;
using WikipediaChecks.Models;

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
                return Ok(wikidataService.GetItemsPerDeathDate(date, false));
            }
            catch (Exception e)
            {
                string message = $"Getting the deceased by date failed. Requested date of death: {date.ToShortDateString()}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }

        [HttpGet("deathscountperday/{year}/{monthId}")]
        public IActionResult GetDeathsCountPerDay(int year, int monthId)
        {
            try
            {
                return Ok(GetWikidataItemsPerDay(year, monthId));
            }
            catch (Exception e)
            {
                string message = $"Getting the deaths count per day by month failed. Requested month of death: {monthId} {year}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }

        private IEnumerable<CountPerDay> GetWikidataItemsPerDay(int year, int month)
        {
            List<CountPerDay> deathsCounts = new List<CountPerDay>();

            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                var items = wikidataService.GetItemsPerDeathDate(new DateTime(year, month, day), true);

                deathsCounts.Add(new CountPerDay { Day = day, Count = items.Count() });
            }
            return deathsCounts;
        }
    }
}
