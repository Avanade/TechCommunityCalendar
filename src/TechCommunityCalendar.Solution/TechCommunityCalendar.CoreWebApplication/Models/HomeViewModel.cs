using System;
using System.Collections.Generic;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class HomeViewModel : EventsViewModelBase
    {
        public IEnumerable<Tuple<string, string>> Countries { get; set; } = new List<Tuple<string, string>>();
        public IEnumerable<Tuple<string, string>> EventTypes { get; set; } = new List<Tuple<string, string>>();
    }
}
