using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class AdminController : Controller
    {
        readonly IWebHostEnvironment currentEnvironment;

        static ITechEventAdminRepository _techEventAdminRepository;
        static ITechEventQueryRepository _techEventRepository;

        public AdminController(IWebHostEnvironment env,
            ITechEventAdminRepository techEventAdminRepository,
            ITechEventQueryRepository techEventRepository)
        {
            currentEnvironment = env;
            _techEventAdminRepository = techEventAdminRepository;
            _techEventRepository = techEventRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Tech Events Admin";

            // Show list of events allow person to toggle or edit

            var techEvents = await _techEventRepository.GetAll();

            var model = new AdminEventsViewModel();
            model.TechEvents = techEvents.OrderByDescending(x => x.StartDate);

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var techEvent = await _techEventRepository.Get(id);

            var model = new EditEventViewModel();
            model.Name = techEvent.Name;
            model.City = techEvent.City;
            model.Country = techEvent.Country;
            model.EventFormat = techEvent.EventFormat.ToString();
            model.EventType = techEvent.EventType.ToString();
            model.Duration = techEvent.Duration;
            model.TwitterHandle = techEvent.TwitterHandle;
            model.StartDate = techEvent.StartDate;
            model.EndDate = techEvent.EndDate;
            model.Hidden = techEvent.Hidden;

            return View("/Views/EditEvent/Index.cshtml", model);
        }

        [HttpPost]
        public IActionResult Edit(EditEventViewModel model)
        {
            // Update

            ITechEvent techEvent = new TechEvent();
            techEvent.Id = model.Id;
            techEvent.Hidden = model.Hidden;

            if (model.AdminPassword == Environment.GetEnvironmentVariable("AdminPassword")
                && !String.IsNullOrEmpty(model.AdminPassword))
            {
                if (model.Delete)
                {
                    _techEventAdminRepository.Remove(techEvent.Id);
                }
                else
                {
                    _techEventAdminRepository.Update(techEvent);
                }
            }           

            return RedirectToAction("Index");
        }
    }
}
