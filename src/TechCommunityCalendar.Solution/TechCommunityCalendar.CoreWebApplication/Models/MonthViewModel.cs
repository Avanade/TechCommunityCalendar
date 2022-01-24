namespace TechCommunityCalendar.CoreWebApplication.Models
{
    public class MonthViewModel : EventsViewModelBase
    {
        public string MonthName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public bool ShowCurrentEvents { get; set; } = true;
        public bool ShowUpcomingEvents { get; set; } = true;
        public bool ShowRecentEvents { get; set; } = true;
        public bool ShowMonthEvents { get; set; } = false;
    }
}
