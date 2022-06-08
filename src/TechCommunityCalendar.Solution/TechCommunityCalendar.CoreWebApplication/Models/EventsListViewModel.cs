using System;
using System.Collections.Generic;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class EventsListViewModel
    {
        public List<Day> Days { get; set; } = new List<Day>();
    }

    public class Day
    {
        public DateTime Date { get; set; }

        public List<ITechEvent> Events { get; set; } = new List<ITechEvent>();
    }
}
