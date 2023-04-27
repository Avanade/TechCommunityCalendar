using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Enums;
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
            model.Url = techEvent.Url;
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
            techEvent.StartDate = model.StartDate;
            techEvent.EndDate = model.EndDate;
            techEvent.EventType = (EventType)Enum.Parse(typeof(EventType), model.EventType);
            techEvent.Url = model.Url;
            techEvent.Name = model.Name;

            // Calculate new duration
            if (model.StartDate == model.EndDate)
            {
                model.Duration = "1 day";
            }
            else if (model.EndDate.Subtract(model.StartDate).TotalHours <= 7)
            {
                model.Duration = (model.EndDate.Subtract(model.StartDate).TotalHours).ToString("0.0") + " hour";                
            }
            else
            {
                model.Duration = (model.EndDate.Subtract(model.StartDate).TotalDays + 1).ToString("0.0") + " day";
            }

            if (model.Duration.Contains(".0"))
                model.Duration = model.Duration.Replace(".0", "");

            techEvent.Duration = model.Duration;

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

        [Route("/admin/avanadecalendar")]
        public async Task<IActionResult> Calendar()
        {
            var techEvents = await _techEventRepository.GetAll();
            techEvents = techEvents.Where(x => x.StartDate.Year == 2023).ToArray();
            
            return View("/Views/Admin/Calendar.cshtml", techEvents.ToList());
        }
    }
}
