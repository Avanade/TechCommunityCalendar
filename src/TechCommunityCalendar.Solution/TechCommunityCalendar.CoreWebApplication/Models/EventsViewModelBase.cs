using System.Collections.Generic;
using System.Linq;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public abstract class EventsViewModelBase
    {
        public IEnumerable<ITechEvent> Events { get; set; } = new List<ITechEvent>();
        public IEnumerable<ITechEvent> CurrentEvents { get; set; } = new List<ITechEvent>();
        public IEnumerable<ITechEvent> UpcomingEvents { get; set; } = new List<ITechEvent>();
        public IEnumerable<ITechEvent> RecentEvents { get; set; } = new List<ITechEvent>();
        public IEnumerable<ITechEvent> MonthEvents { get; set; } = new List<ITechEvent>();

        public int EventsCount { get { return Events.Count(); } }
        public int CurrentEventsCount { get { return CurrentEvents.Count(); } }
        public int UpcomingEventsCount { get { return UpcomingEvents.Count(); } }
        public int RecentEventsCount { get { return RecentEvents.Count(); } }
        public int MonthEventsCount { get { return MonthEvents.Count(); } }
    }
}
