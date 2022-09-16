using System;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;
using static System.Net.Mime.MediaTypeNames;

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

        public string Description
        {
            get
            {
                string description = "";


                if (EventType == TechCommunityCalendar.Enums.EventType.Call_For_Papers)
                {
                    description += "Call For Papers";

                    var daysRemaining = EndDate.Subtract(DateTime.Now.Date).Days;
                    if (daysRemaining > 0)
                    {
                        description += $" ({@daysRemaining} days remaining)";
                    }
                }
                else
                {
                    description += $"{Duration} {EventFormat.ToString().Replace("_", " ")} {EventType.ToString().Replace("_", " ")}";
                }

                return description;
            }
        }

        public string FriendlyName { get { return TechEventCleaner.MakeFriendlyBranchName(Name); } }

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
