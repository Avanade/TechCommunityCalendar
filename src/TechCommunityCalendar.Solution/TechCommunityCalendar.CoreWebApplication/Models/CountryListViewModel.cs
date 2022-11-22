using System.Collections.Generic;
using System.Linq;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class CountryListViewModel
    {
        public CountryListViewModel(string countryName, int month, List<ITechEvent> events)
        {
            CountryName = countryName;
            Events = events.Where(x=>x.Country.Equals(countryName) && x.StartDate.Month == month).ToList();
            //Year = year;
            Month = month;
        }

        public string CountryName { get; set; }
        public List<ITechEvent>  Events { get; set; }
        public int Month { get; set; }
        //public int Year { get; set; }
    }
}
