using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

            var model = new HomeViewModel();
            model.UpcomingEvents =
                events.Where(x => x.StartDate.Date >= DateTime.Now.Date  // Future events
                && x.StartDate.Date < DateTime.Now.AddDays(14)).OrderBy(x=>x.StartDate);        // No more than 14 days in the future

            model.RecentEvents = events.Where(x => x.StartDate.Date < DateTime.Now.Date).OrderByDescending(x => x.StartDate);
            model.Events = events;

            return View(model);
        }

        [Route("{month}/{year}")]
        public async Task<IActionResult> Month(int year, int month)
        {
            var monthDate = new DateTime(year, month, 1);

            var model = new MonthViewModel();
            model.MonthName = monthDate.ToString("MMMM");
            model.Year = year;

            var events = await _techEventRepository.GetByMonth(year, month);
            model.Events = events;

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
