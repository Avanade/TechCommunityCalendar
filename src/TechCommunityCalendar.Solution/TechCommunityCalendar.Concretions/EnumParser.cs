using TechCommunityCalendar.Enums;

namespace TechCommunityCalendar.Concretions
{
    public class EnumParser
    {
        public static EventType ParseEventType(string eventType)
        {
            switch (eventType.ToLower())
            {
                case "conference":
                    return EventType.Conference;

                case "hackathon":
                    return EventType.Hackathon;

                case "meetup":
                    return EventType.Meetup;

                case "call_for_papers":
                case "callforpapers":
                    return EventType.Call_For_Papers;

                case "website":
                    return EventType.Website;

                default:
                    return EventType.Unknown;
            }
        }

        public static EventFormat ParseEventFormat(string eventFormat)
        {
            switch (eventFormat.ToLower())
            {
                case "in_person":
                    return EventFormat.In_Person;

                case "virtual":
                    return EventFormat.Virtual;

                case "hybrid":
                    return EventFormat.Hybrid;

                default:
                    return EventFormat.Unknown;
            }
        }
    }
}
