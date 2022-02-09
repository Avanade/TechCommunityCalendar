using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    public class TechEventCalendar
    {
        public static IEnumerable<ITechEvent> GetCurrentEvents(IEnumerable<ITechEvent> events)
        {
            return events.Where(x =>
                DateTime.Now.Date >= x.StartDate // It is or after the start date
                && DateTime.Now <= x.EndDate // And it is or before the end date
                || DateTime.Now.Date == x.StartDate.Date) // To catch short events today
                    .OrderByDescending(x => x.StartDate);
        }

        public static IEnumerable<ITechEvent> GetUpcomingEvents(IEnumerable<ITechEvent> events)
        {
            return events.Where(x => x.StartDate.Date > DateTime.Now.Date  // Future events
                && x.StartDate.Date <= DateTime.Now.AddDays(30)) // No more than 30 days in the future
                    .OrderBy(x => x.StartDate);
        }

        public static IEnumerable<ITechEvent> GetRecentEvents(IEnumerable<ITechEvent> events)
        {
            return events.Where(x => DateTime.Now.Date > x.EndDate
                && DateTime.Now.Date.Subtract(x.EndDate).TotalDays <= 30)
                    .OrderByDescending(x => x.StartDate);
        }
        public static string GetEventTypeIcon(EventType eventType)
        {
            switch (eventType)
            {
                case EventType.Call_For_Papers:
                    return "📣";

                case EventType.Conference:
                    return "🧠";

                case EventType.Hackathon:
                    return "👩‍💻";

                case EventType.Meetup:
                    return "🤝";

                case EventType.Website:
                    return "💻";
            }

            return string.Empty;
        }

        public static string GetFlag(string country)
        {
            var dictionary = new Dictionary<string, string>()
            {
                {"Online", ""},
                    {"Afghanistan",""},
                    {"Albania",""},
                    {"Algeria",""},
                    {"Andorra",""},
                    {"Angola",""},
                    {"Antigua & Deps",""},
                    {"Argentina",""},
                    {"Armenia",""},
                    {"Australia","au"},
                    {"Austria",""},
                    {"Azerbaijan",""},
                    {"Bahamas",""},
                    {"Bahrain",""},
                    {"Bangladesh",""},
                    {"Barbados",""},
                    {"Belarus",""},
                    {"Belgium","be"},
                    {"Belize",""},
                    {"Benin",""},
                    {"Bhutan",""},
                    {"Bolivia",""},
                    {"Bosnia Herzegovina",""},
                    {"Botswana",""},
                    {"Brazil",""},
                    {"Brunei",""},
                    {"Bulgaria",""},
                    {"Burkina",""},
                    {"Burundi",""},
                    {"Cambodia",""},
                    {"Cameroon",""},
                    {"Canada",""},
                    {"Cape Verde",""},
                    {"Central African Rep",""},
                    {"Chad",""},
                    {"Chile",""},
                    {"China",""},
                    {"Colombia",""},
                    {"Comoros",""},
                    {"Congo",""},
                    {"Congo (Democratic Rep)",""},
                    {"Costa Rica",""},
                    {"Croatia",""},
                    {"Cuba",""},
                    {"Cyprus",""},
                    {"Czech Republic",""},
                    {"Denmark","dk"},
                    {"Djibouti",""},
                    {"Dominica",""},
                    {"Dominican Republic",""},
                    {"East Timor",""},
                    {"Ecuador",""},
                    {"Egypt",""},
                    {"El Salvador",""},
                    {"Equatorial Guinea",""},
                    {"Eritrea",""},
                    {"Estonia",""},
                    {"Ethiopia",""},
                    {"Fiji",""},
                    {"Finland",""},
                    {"France",""},
                    {"Gabon",""},
                    {"Gambia",""},
                    {"Georgia",""},
                    {"Germany","de"},
                    {"Ghana",""},
                    {"Greece",""},
                    {"Grenada",""},
                    {"Guatemala",""},
                    {"Guinea",""},
                    {"Guinea-Bissau",""},
                    {"Guyana",""},
                    {"Haiti",""},
                    {"Honduras",""},
                    {"Hungary",""},
                    {"Iceland",""},
                    {"India","in"},
                    {"Indonesia",""},
                    {"Iran",""},
                    {"Iraq",""},
                    {"Ireland (Republic)",""},
                    {"Israel",""},
                    {"Italy",""},
                    {"Ivory Coast",""},
                    {"Jamaica",""},
                    {"Japan",""},
                    {"Jordan",""},
                    {"Kazakhstan",""},
                    {"Kenya",""},
                    {"Kiribati",""},
                    {"Korea North",""},
                    {"Korea South",""},
                    {"Kosovo",""},
                    {"Kuwait",""},
                    {"Kyrgyzstan",""},
                    {"Laos",""},
                    {"Latvia",""},
                    {"Lebanon",""},
                    {"Lesotho",""},
                    {"Liberia",""},
                    {"Libya",""},
                    {"Liechtenstein",""},
                    {"Lithuania",""},
                    {"Luxembourg",""},
                    {"Macedonia",""},
                    {"Madagascar",""},
                    {"Malawi",""},
                    {"Malaysia",""},
                    {"Maldives",""},
                    {"Mali",""},
                    {"Malta",""},
                    {"Marshall Islands",""},
                    {"Mauritania",""},
                    {"Mauritius",""},
                    {"Mexico",""},
                    {"Micronesia",""},
                    {"Moldova",""},
                    {"Monaco",""},
                    {"Mongolia",""},
                    {"Montenegro",""},
                    {"Morocco",""},
                    {"Mozambique",""},
                    {"Myanmar (Burma)",""},
                    {"Namibia",""},
                    {"Nauru",""},
                    {"Nepal",""},
                    {"Netherlands","nl"},
                    {"New Zealand",""},
                    {"Nicaragua",""},
                    {"Niger",""},
                    {"Nigeria",""},
                    {"Norway","no"},
                    {"Oman",""},
                    {"Pakistan",""},
                    {"Palau",""},
                    {"Panama",""},
                    {"Papua New Guinea",""},
                    {"Paraguay",""},
                    {"Peru",""},
                    {"Philippines",""},
                    {"Poland",""},
                    {"Portugal","pt"},
                    {"Qatar",""},
                    {"Romania",""},
                    {"Russian Federation",""},
                    {"Rwanda",""},
                    {"St Kitts & Nevis",""},
                    {"St Lucia",""},
                    {"Saint Vincent & the Grenadines",""},
                    {"Samoa",""},
                    {"San Marino",""},
                    {"Sao Tome & Principe",""},
                    {"Saudi Arabia",""},
                    {"Senegal",""},
                    {"Serbia",""},
                    {"Seychelles",""},
                    {"Sierra Leone",""},
                    {"Singapore",""},
                    {"Slovakia",""},
                    {"Slovenia",""},
                    {"Solomon Islands",""},
                    {"Somalia",""},
                    {"South Africa","za"},
                    {"South Sudan",""},
                    {"Spain",""},
                    {"Sri Lanka",""},
                    {"Sudan",""},
                    {"Suriname",""},
                    {"Swaziland",""},
                    {"Sweden",""},
                    {"Switzerland",""},
                    {"Syria",""},
                    {"Taiwan",""},
                    {"Tajikistan",""},
                    {"Tanzania",""},
                    {"Thailand",""},
                    {"Togo",""},
                    {"Tonga",""},
                    {"Trinidad & Tobago",""},
                    {"Tunisia",""},
                    {"Turkey",""},
                    {"Turkmenistan",""},
                    {"Tuvalu",""},
                    {"Uganda",""},
                    {"Ukraine",""},
                    {"United Arab Emirates",""},
                    {"UK","gb"},
                    {"USA","us"},
                    {"Uruguay",""},
                    {"Uzbekistan",""},
                    {"Vanuatu",""},
                    {"Vatican City",""},
                    {"Venezuela",""},
                    {"Vietnam",""},
                    {"Yemen",""},
                    {"Zambia",""},
                    {"Zimbabwe",""},
            };

            return $@"{dictionary[country]}.png";
        }
    }
}
