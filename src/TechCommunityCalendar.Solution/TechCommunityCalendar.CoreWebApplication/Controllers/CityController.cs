using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class CityController : ControllerBase
    {
        public CityController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
            : base(memoryCache, techEventRepository)
        {

        }

        [Route("city/{city}")]
        public async Task<IActionResult> View(string city)
        {
            var model = new CityViewModel();
            model.City = ToTitleCase(city);

            ViewBag.Title = $"Tech Community Events in {model.City}";
            ViewBag.Canonical = $"https://TechCommunityCalendar.com/country/{city}/";

            var allEvents = await _techEventRepository.GetAll();
            var cityEvents = allEvents.Where(x => x.City.ToLower() == city.ToLower());

            model.Events = cityEvents;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(cityEvents);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(cityEvents);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(cityEvents, 365);

            return View(model);
        }
    }
}
