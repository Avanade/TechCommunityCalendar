using System.Collections.Generic;

namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class MapViewModel : EventsViewModelBase
    {
        public IEnumerable<string> Cities { get;  set; }
		public string ArrayCode { get; internal set; }
	}
}
