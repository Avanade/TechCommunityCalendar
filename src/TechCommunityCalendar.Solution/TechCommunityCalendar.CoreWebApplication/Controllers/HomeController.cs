using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITechEventQueryRepository _techEventRepository;

        public HomeController(ILogger<HomeController> logger, ITechEventQueryRepository techEventRepository)
        {
            _logger = logger;
            _techEventRepository = techEventRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Canonical = "https://TechCommunityCalendar.com";
            ViewBag.Title = "Tech Community Calendar";
            ViewBag.Description = "A calendar list of upcoming Conferences, Meetups and Hackathons in the Tech Community";

            var events = await _techEventRepository.GetAll();

            var countries = events.Select(x => x.Country)
                .Distinct()
                .Where(x => !String.IsNullOrWhiteSpace(x))
                .OrderBy(x => x);

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

            return View(model);
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

            if(new DateTime(year,month, 1).Date > DateTime.Now.Date)
            {
                model.UpcomingEvents = events;
            }


            return View(model);
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

        private string ToTitleCase(string originalText)
        {
            string[] acronyms = { "USA", "UK" };

            if (acronyms.Contains(originalText.ToUpper()))
                return originalText.ToUpper();

            if (originalText == "callforpaper")
                return "Call For Paper";

            TextInfo englishTextInfo = new CultureInfo("en-GB", false).TextInfo;
            return englishTextInfo.ToTitleCase(originalText);
        }
    }
}
