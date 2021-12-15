using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class MonthController : ControllerBase
    {
        public MonthController(IMemoryCache memoryCache, 
            ITechEventQueryRepository techEventRepository) 
            : base(memoryCache, techEventRepository)
        {
        }

        [Route("{month}/{year}")]
        public async Task<IActionResult> Month(int year, int month)
        {
            var monthDate = new DateTime(year, month, 1);
            var events = await _techEventRepository.GetByMonth(year, month);

            var model = new MonthViewModel();
            model.MonthName = ToTitleCase(monthDate.ToString("MMMM"));
            model.Year = year;
            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            if (new DateTime(year, month, 1).Date > DateTime.Now.Date)
            {
                model.UpcomingEvents = events;
            }

            return View(model);
        }

        [Route("year/{year}")]
        public async Task<IActionResult> Year(int year)
        {
            var events = await _techEventRepository.GetByYear(year);

            var model = new YearViewModel();
            model.Year = year;
            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            if (new DateTime(year, 1, 1).Date > DateTime.Now.Date)
            {
                model.UpcomingEvents = events;
            }

            return View(model);
        }
    }
}
