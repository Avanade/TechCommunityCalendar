using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class CountryController : ControllerBase
    {
        public CountryController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository) : base(memoryCache, techEventRepository)
        {
        }

        [Route("country/{country}")]
        public async Task<IActionResult> Country(string country)
        {
            var allEvents = await GetEventsFromCache();
            var events = allEvents.Where(x => x.Country.Equals(country, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            var model = new CountryViewModel();
            model.Country = ToTitleCase(country);
            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            ViewBag.Title = $"Tech Community Events in {model.Country}";
            ViewBag.Canonical = $"https://TechCommunityCalendar.com/country/{country}/";

            return View(model);
        }
    }
}
