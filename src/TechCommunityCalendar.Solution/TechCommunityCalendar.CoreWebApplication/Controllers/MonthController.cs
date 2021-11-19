using Microsoft.AspNetCore.Mvc;
using System;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class MonthController : Controller
    {
        private readonly ITechEventQueryRepository _techEventRepository;

        public MonthController(ITechEventQueryRepository techEventRepository)
        {
            _techEventRepository = techEventRepository;
        }

       

        public IActionResult Index()
        {
            return View();
        }
    }
}
