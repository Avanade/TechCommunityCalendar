using System;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    public class TechEvent : ITechEvent
    {
        public TechEvent() { }

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

        public bool HappeningOnDate(DateTime dateTime)
        {
            if (dateTime.Date >= StartDate.Date && dateTime.Date <= EndDate.Date)
                return true;

            return false;
        }

        public bool HapenningNextXDays(int days)
        {
            var periodEnd = DateTime.Now.AddDays(days);

            if (StartDate <= periodEnd && DateTime.Now.Date <= EndDate.Date)
                return true;

            return false;
        }
    }
}
