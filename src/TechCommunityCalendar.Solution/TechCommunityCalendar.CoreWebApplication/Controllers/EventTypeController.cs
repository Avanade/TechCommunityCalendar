using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class EventTypeController : ControllerBase
    {
        public EventTypeController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
            : base(memoryCache, techEventRepository)
        {
        }

        [Route("eventtype/{eventType}")]
        public async Task<IActionResult> EventType(string eventType)
        {
            var model = new EventTypeViewModel();
            model.EventType = ToTitleCase(eventType);

            Enums.EventType eventTypeEnum = EnumParser.ParseEventType(eventType);

            var allEvents = await GetEventsFromCache();
            var events = allEvents.Where(x => x.EventType.Equals(eventTypeEnum)).ToArray();

            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            ViewBag.Title = $"{model.EventType} Tech Community Events";
            ViewBag.Canonical = $"https://TechCommunityCalendar.com/eventtype/{model.EventType}/";

            return View(model);
        }
    }
}
