﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TechCommunityCalendar.Concretions;
using TechCommunityCalendar.CoreWebApplication.Models;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class MonthController : ControllerBase
    {
        private readonly ITechEventQueryRepository _techEventRepository;

        public MonthController(ITechEventQueryRepository techEventRepository)
        {
            _techEventRepository = techEventRepository;
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
    }
}
