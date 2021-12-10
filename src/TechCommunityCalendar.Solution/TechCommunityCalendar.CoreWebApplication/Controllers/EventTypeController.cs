using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class EventTypeController : ControllerBase
    {
        private readonly ITechEventQueryRepository _techEventRepository;

        public EventTypeController(ITechEventQueryRepository techEventRepository)
        {
            _techEventRepository = techEventRepository;
        }

        [Route("eventtype/{eventType}")]
        public async Task<IActionResult> EventType(string eventType)
        {
            var model = new EventTypeViewModel();
            model.EventType = ToTitleCase(eventType);

            Enums.EventType eventTypeEnum = EnumParser.ParseEventType(eventType);

            var events = await _techEventRepository.GetByEventType(eventTypeEnum);
            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            return View(model);
        }
    }
}
