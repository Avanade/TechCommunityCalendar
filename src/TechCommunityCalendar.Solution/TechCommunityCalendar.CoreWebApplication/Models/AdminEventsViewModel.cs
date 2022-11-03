using System.Collections.Generic;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class AdminEventsViewModel
    {
        public IEnumerable<ITechEvent> TechEvents { get; set; } = new List<ITechEvent>();
    }
}
