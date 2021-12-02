using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    public class TechEventCalendar
    {
        public static IEnumerable<ITechEvent> GetCurrentEvents(IEnumerable<ITechEvent> events)
        {
            return events.Where(x => 
                DateTime.Now.Date >= x.StartDate
                && DateTime.Now <= x.EndDate)
                    .OrderBy(x => x.StartDate);
        }

        public static IEnumerable<ITechEvent> GetUpcomingEvents(IEnumerable<ITechEvent> events)
        {
            return events.Where(x => x.StartDate.Date > DateTime.Now.Date  // Future events
                && x.StartDate.Date < DateTime.Now.AddDays(14)) // No more than 14 days in the future
                    .OrderBy(x => x.StartDate);
        }

        public static IEnumerable<ITechEvent> GetRecentEvents(IEnumerable<ITechEvent> events)
        {
            return events.Where(x => DateTime.Now.Date > x.EndDate)
                .OrderByDescending(x => x.StartDate);
        }
    }
}
