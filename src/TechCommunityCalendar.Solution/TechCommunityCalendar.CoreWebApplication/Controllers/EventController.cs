using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
