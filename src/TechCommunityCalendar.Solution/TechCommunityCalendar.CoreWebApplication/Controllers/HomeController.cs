using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
            : base(memoryCache, techEventRepository)
        {
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Canonical = "https://TechCommunityCalendar.com";
            ViewBag.Title = "Tech Community Calendar";
            ViewBag.Description = "A calendar list of upcoming Conferences, Meetups and Hackathons in the Tech Community";

            var events = await GetEventsFromCache();

            var countries = events.Select(x => x.Country)
                .Distinct()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .OrderBy(x => x).ToArray();

            var eventTypes = events.Select(x => x.EventType.ToString())
                .Distinct()
                .OrderBy(x => x);

            var model = new HomeViewModel();
            model.Events = events;
            model.Countries = countries.Select(x => new Tuple<string, string>(x, x.ToLower()));
            model.EventTypes = eventTypes.Select(x => new Tuple<string, string>(x.Replace("_", " "), x.ToLower().Replace("_", "")));

            model.CurrentEvents = TechEventCalendar.GetCurrentEvents(events);
            model.UpcomingEvents = TechEventCalendar.GetUpcomingEvents(events);
            model.RecentEvents = TechEventCalendar.GetRecentEvents(events);

            var eventList = new EventsListViewModel();

            var tempDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            // Group by day in current month
            for (int x = DateTime.Now.Day; x < DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); x++)
            {

                var day = new Day();
                day.Date = tempDate;
                day.Events = TechEventCalendar.GetEventsOnDate(events, tempDate).ToList();

                eventList.Days.Add(day);

                tempDate = tempDate.AddDays(1);
            }

            model.EventList = eventList;

            return View(model);
        }

        [Route("/opensource/")]
        public IActionResult OpenSource()
        {
            ViewBag.Title = "Tech Community Calendar is Open Source";
            ViewBag.MetaDescription = "Tech Community Calendar is Open Source and encourages contributions";
            ViewBag.Canonical = "https://techcommunitycalendar.com/opensource/";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
