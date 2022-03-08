using System;
using System.Collections.Generic;
using WikipediaChecks.Models;

namespace WikipediaChecks.Interfaces
{
    public interface IWikipediaService
    {
        public IEnumerable<CountPerDay> GetDeathsCountPerDay(int year, int month);
        public IEnumerable<CountsPerMonthOfDay> GetDeathsCountPerDay(int year);
        public IEnumerable<WPEntry> GetDeceased(DateTime deathDate);
        public IEnumerable<WPEntry> GetDeceased(int year, int month);
    }
}
