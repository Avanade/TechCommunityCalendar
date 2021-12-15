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

            var allEvents = await GetEventsFromCache();
            var events = allEvents.Where(x => x.StartDate.Year == year && x.StartDate.Month == month).ToArray();

            var model = new MonthViewModel();
            model.MonthName = ToTitleCase(monthDate.ToString("MMMM"));
            model.Year = year;
            model.Events = events;
            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            if (new DateTime(year, month, 1).Date > DateTime.Now.Date) {
                model.UpcomingEvents = events;
            }

            var lastMonth = DateTime.Now.AddMonths(-1);
            var thisMonth = DateTime.Now.AddMonths(0);
            var nextMonth = DateTime.Now.AddMonths(1);

            var lastMonthName = lastMonth.ToString("MMMM");
            var thisMonthName = thisMonth.ToString("MMMM");
            var nextMonthName = nextMonth.ToString("MMMM");

            var lastMonthNumber = lastMonth.Date.Month;
            var thisMonthNumber = thisMonth.Date.Month;
            var nextMonthNumber = nextMonth.Date.Month;

            var lastMonthYear = lastMonth.Date.Year;
            var thisMonthYear = thisMonth.Date.Year;
            var nextMonthYear = nextMonth.Date.Year;

            ViewBag.Title = $"Tech Community Events in {thisMonthName} {thisMonthYear}";

            return View(model);
        }

        [Route("year/{year}")]
        public async Task<IActionResult> Year(int year)
        {
            var allEvents = await GetEventsFromCache();
            var events = allEvents.Where(x => x.StartDate.Year == year).OrderBy(x=>x.StartDate).ToArray();

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

            ViewBag.Title = $"Tech Community Events in {model.Year}";


            return View(model);
        }
    }
}
