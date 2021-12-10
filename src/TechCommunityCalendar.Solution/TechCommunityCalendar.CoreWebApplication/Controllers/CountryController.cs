using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class CountryController : ControllerBase
    {
        private readonly ITechEventQueryRepository _techEventRepository;

        public CountryController(ITechEventQueryRepository techEventRepository)
        {
            _techEventRepository = techEventRepository;
        }

        [Route("country/{country}")]
        public async Task<IActionResult> Country(string country)
        {
            var events = await _techEventRepository.GetByCountry(Enums.EventType.Any, country);

            var model = new CountryViewModel();
            model.Country = ToTitleCase(country);
            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            return View(model);
        }
    }
}
