using System.Collections.Generic;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class MonthViewModel
    {
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public IEnumerable<ITechEvent> Events { get; set; } = new List<ITechEvent>();
        public IEnumerable<ITechEvent> UpcomingEvents { get; set; } = new List<ITechEvent>();
        public IEnumerable<ITechEvent> RecentEvents { get; set; } = new List<ITechEvent>();
    }
}
