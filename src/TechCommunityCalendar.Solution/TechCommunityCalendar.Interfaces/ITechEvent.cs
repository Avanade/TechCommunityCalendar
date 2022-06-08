using System;
using TechCommunityCalendar.Enums;

namespace TechCommunityCalendar.Interfaces
{
    public interface ITechEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Url { get; set; }
        public string TwitterHandle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Duration { get; set; }
        public EventType EventType { get; set; }
        public EventFormat EventFormat { get; set; }
        public bool Hidden { get; set; }
        public bool HappeningOnDate(DateTime dateTime);
        public bool HapenningNextXDays(int days);
        public string[] Tags { get; set; }
    }
}
