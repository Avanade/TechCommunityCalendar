using TechCommunityCalendar.Enums;

namespace TechCommunityCalendar.Concretions
{
    public class EnumParser
    {
        public static EventType ParseEventType(string eventType)
        {
            switch (eventType)
            {
                case "Conference":
                    return EventType.Conference;

                case "Hackathon":
                    return EventType.Hackathon;

                case "Meetup":
                    return EventType.Meetup;

                case "Call_For_Papers":
                    return EventType.Call_For_Papers;

                case "Website":
                    return EventType.Website;

                default:
                    return EventType.Unknown;
            }
        }

        public static EventFormat ParseEventFormat(string eventFormat)
        {
            switch (eventFormat)
            {
                case "In_Person":
                    return EventFormat.In_Person;

                case "Virtual":
                    return EventFormat.Virtual;

                case "Hybrid":
                    return EventFormat.Hybrid;

                default:
                    return EventFormat.Unknown;
            }
        }
    }
}
