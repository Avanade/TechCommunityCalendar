using System;
using System.Collections.Generic;
using System.Text;
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

                case "callforpaper":
                    return EventType.Call_For_Paper;

                case "website":
                    return EventType.Website;

                default:
                    return EventType.Unknown;
            }
        }

        public static EventFormat ParseEventFormat(string eventFormat)
        {
            switch (eventFormat)
            {
                case "In Person":
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
