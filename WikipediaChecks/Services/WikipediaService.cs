using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using Wikimedia.Utilities.Interfaces;
using WikipediaChecks.Interfaces;
using WikipediaChecks.Models;

namespace WikipediaChecks.Services
{
    public class WikipediaService : IWikipediaService
    {
        private readonly IWikiTextService wikiTextService;

        public WikipediaService(IWikipediaWebClient wikipediaWebClient, IWikiTextService wikiTextService, ILogger<WikipediaService> logger)
        {
            this.wikiTextService = wikiTextService;
        }

        public IEnumerable<CountPerDay> GetDeathsCountPerDay(int year, int month)
        {
            DateTime deathDate = new DateTime(year, month, 1);
            List<CountPerDay> deathsCounts = new List<CountPerDay>();

            string deathsPerMonthText = wikiTextService.GetWikiTextDeathsPerMonth(deathDate, false);

            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                string deathsPerDayText = wikiTextService.GetDaySectionOfMonthList(deathsPerMonthText, day);
                IEnumerable<string> rawDeceased = wikiTextService.GetDeceasedTextAsList(deathsPerDayText);
                deathsCounts.Add(new CountPerDay { Day = day, Count = rawDeceased.Count() });
            }
            return deathsCounts;
        }

        public IEnumerable<CountsPerMonthOfDay> GetDeathsCountPerDay(int year)
        {
            var countsPerDayLists = new List<IEnumerable<CountPerDay>>();

            for (int i = 1; i <= 12; i++)
                countsPerDayLists.Add(GetDeathsCountPerDay(year, i));

            var deathsCountPerDay = new List<CountsPerMonthOfDay>();

            for (int i = 1; i <= 31; i++)
                deathsCountPerDay.Add(CreateCountPerDayList(countsPerDayLists, i));

            return deathsCountPerDay;
        }

        public IEnumerable<WPEntry> GetDeceased(DateTime deathDate)
        {
            string text = wikiTextService.GetWikiTextDeathsPerMonth(deathDate, false);

            text = wikiTextService.GetDaySectionOfMonthList(text, deathDate.Day);

            IEnumerable<string> rawDeceased = wikiTextService.GetDeceasedTextAsList(text);

            List<WPEntry> deceased = new List<WPEntry>();
            foreach (var rawEntry in rawDeceased)
                deceased.Add(ParseEntry(rawEntry, deathDate));

            return deceased;
        }

        public IEnumerable<WPEntry> GetDeceased(int year, int month)
        {
            DateTime deathDate = new DateTime(year, month, 1);
            List<WPEntry> deceased = new List<WPEntry>();

            string deathsPerMonthText = wikiTextService.GetWikiTextDeathsPerMonth(deathDate, false);

            for (int day = 1; day <= DateTime.DaysInMonth(year, month); day++)
            {
                string deathsPerDayText = wikiTextService.GetDaySectionOfMonthList(deathsPerMonthText, day);

                IEnumerable<string> rawDeceased = wikiTextService.GetDeceasedTextAsList(deathsPerDayText);
                List<WPEntry> deceasedPerDay = new List<WPEntry>();
                foreach (var rawEntry in rawDeceased)
                    deceasedPerDay.Add(ParseEntry(rawEntry, new DateTime(year, month, day)));

                deceased.AddRange(deceasedPerDay);
            }

            return deceased;
        }

        private WPEntry ParseEntry(string rawEntry, DateTime deathDate)
        {
            return new WPEntry
            {
                Date_Of_Death = deathDate.ToString("d MMM yyyy"),
                Linked_Name = wikiTextService.GetNameFromEntryText(rawEntry, true),
                Display_Name = wikiTextService.GetNameFromEntryText(rawEntry, false),
                Information = wikiTextService.GetInformationFromEntryText(rawEntry),
                Reference = wikiTextService.GetReferencesFromEntryText(rawEntry),
            };
        }

        private CountsPerMonthOfDay CreateCountPerDayList(List<IEnumerable<CountPerDay>> countsPerDayLists, int day)
        {
            const int jan = 0;
            const int feb = 1;
            const int mar = 2;
            const int apr = 3;
            const int may = 4;
            const int jun = 5;
            const int jul = 6;
            const int aug = 7;
            const int sep = 8;
            const int oct = 9;
            const int nov = 10;
            const int dec = 11;
            var countsPerMonthOfDay = new CountsPerMonthOfDay();

            countsPerMonthOfDay.Day = day;

            countsPerMonthOfDay.Jan = countsPerDayLists.ElementAt(jan).ElementAt(day - 1).Count;
            countsPerMonthOfDay.Mar = countsPerDayLists.ElementAt(mar).ElementAt(day - 1).Count;
            countsPerMonthOfDay.May = countsPerDayLists.ElementAt(may).ElementAt(day - 1).Count;
            countsPerMonthOfDay.Jul = countsPerDayLists.ElementAt(jul).ElementAt(day - 1).Count;
            countsPerMonthOfDay.Aug = countsPerDayLists.ElementAt(aug).ElementAt(day - 1).Count;
            countsPerMonthOfDay.Oct = countsPerDayLists.ElementAt(oct).ElementAt(day - 1).Count;
            countsPerMonthOfDay.Dec = countsPerDayLists.ElementAt(dec).ElementAt(day - 1).Count;

            if (day > countsPerDayLists.ElementAt(feb).Count())
                countsPerMonthOfDay.Feb = null;
            else
                countsPerMonthOfDay.Feb = countsPerDayLists.ElementAt(feb).ElementAt(day - 1).Count;

            if (day > 30)
            {
                countsPerMonthOfDay.Apr = null;
                countsPerMonthOfDay.Jun = null;
                countsPerMonthOfDay.Sep = null;
                countsPerMonthOfDay.Nov = null;
            }
            else
            {
                countsPerMonthOfDay.Apr = countsPerDayLists.ElementAt(apr).ElementAt(day - 1).Count;
                countsPerMonthOfDay.Jun = countsPerDayLists.ElementAt(jun).ElementAt(day - 1).Count;
                countsPerMonthOfDay.Sep = countsPerDayLists.ElementAt(sep).ElementAt(day - 1).Count;
                countsPerMonthOfDay.Nov = countsPerDayLists.ElementAt(nov).ElementAt(day - 1).Count;
            }
            return countsPerMonthOfDay;
        }
    }
}
