using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.CoreWebApplication.Controllers
{
    public class SocialPostContentController : ControllerBase
    {
        public SocialPostContentController(IMemoryCache memoryCache,
            ITechEventQueryRepository techEventRepository)
            : base(memoryCache, techEventRepository)
        {

        }

        [Route("/ThisWeek")]
        public async Task<IActionResult> IndexAsync()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Some great #TechCommunityCalendar events w/c 14th February 2022");
            message.AppendLine();

            var allEvents = await GetEventsFromCache();

            var thisWeekDateRange = ThisWeek(DateTime.Now);

            var thisWeeksEvents = allEvents.Where(x => x.StartDate >= thisWeekDateRange.Start
            && x.StartDate <= thisWeekDateRange.End);

            foreach(var techEvent in thisWeeksEvents.OrderBy(x=>x.EventType))
            {
                if(techEvent.EventType == Enums.EventType.Conference)
                {
                    message.AppendLine("🧠 " + techEvent.Name);
                }
                else if (techEvent.EventType == Enums.EventType.Meetup)
                {
                    message.AppendLine("🤝 " + techEvent.Name);
                }
                else if (techEvent.EventType == Enums.EventType.Hackathon)
                {
                    message.AppendLine("👩‍💻 " + techEvent.Name);
                }
                else if (techEvent.EventType == Enums.EventType.Call_For_Papers)
                {
                    message.AppendLine("📣 " + techEvent.Name);
                }                
            }
            
            //🧠 Voxxed Days Melbourne
            //🧠 Azure Open Source day
            //🧠 Global Power Platform Bootcamp 
            //📣 Data Weekender CFP
            //👩‍💻 Hard of Hearing hackathon
            message.AppendLine();
            message.AppendLine("Check out TechCommunityCalendar.com for more!");

message.AppendLine("#techcommunity #devcommunity");

            return Content(message.ToString());
        }

        public static DateRange ThisWeek(DateTime date)
        {
            DateRange range = new DateRange();

            range.Start = date.Date.AddDays(-(int)date.DayOfWeek);
            range.End = range.Start.AddDays(7).AddSeconds(-1);

            return range;
        }
    }

    public struct DateRange
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
