using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WikipediaChecks.Interfaces;

namespace WikipediaChecks.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WikipediaController : ControllerBase
    {

        private readonly IWikipediaService wikipediaService;
        private readonly ILogger<WikipediaController> logger;

        public WikipediaController(IWikipediaService wikipediaService, ILogger<WikipediaController> logger)
        {
            this.wikipediaService = wikipediaService;
            this.logger = logger;
        }

        [HttpGet("deathscountperday/{year}/{monthId}")]
        public IActionResult GetDeathsCountPerDay(int year, int monthId)
        {
            try
            {
                return Ok(wikipediaService.GetDeathsCountPerDay(year, monthId));
            }
            catch (Exception e)
            {
                string message = $"Getting the deaths count per day by month failed. Requested month of death: {monthId} {year}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }

        [HttpGet("deathscountperday/{year}")]
        public IActionResult GetDeathsCountPerDay(int year)
        {
            try
            {
                return Ok(wikipediaService.GetDeathsCountPerDay(year));
            }
            catch (Exception e)
            {
                string message = $"Getting the deaths count per day by year failed. Requested year of death: {year}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }


        [HttpGet("deceased/{date}")]
        public IActionResult GetDeceasedByDate(DateTime date)
        {
            try
            {
                return Ok(wikipediaService.GetDeceased(date));
            }
            catch (Exception e)
            {
                string message = $"Getting the deceased by date failed. Requested date of death: {date.ToShortDateString()}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }

        [HttpGet("deceased/{year}/{monthId}")]
        public IActionResult GetDeceasedByMonth(int year, int monthId)
        {
            try
            {
                return Ok(wikipediaService.GetDeceased(year, monthId));
            }
            catch (Exception e)
            {
                string message = $"Getting the deceased by month failed. Requested month of death: {monthId} {year}.\r\n" +
                                 $"Exception:\r\n{e.Message}";
                logger.LogError($"{message}", e);
                return Ok(new List<Error> { new Error { Error_Type = e.GetType().Name, Error_Message = message, InnerExceptionMessage = e.InnerException?.Message } });
            }
        }
    }
}
