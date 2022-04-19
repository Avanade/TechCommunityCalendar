using System;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    public class FakeTechEventRepository : ITechEventQueryRepository
    {
        public async Task<ITechEvent> Get(int yeat, int month, Guid id)
        {
            var events = await GetAll();
            return events.FirstOrDefault(x => x.Id == id);
        }

        public Task<ITechEvent[]> GetAll()
        {
            var events = new ITechEvent[]
            {
                new TechEvent() { Name= "DevRelCon", EventType = EventType.Conference, EventFormat= EventFormat.Virtual, Country="Worldwide", City="", Id=Guid.NewGuid(), Url = "https://2021.devrel.net/", StartDate = new DateTime(2021, 11, 8) },
                new TechEvent() { Name= ".NET Conf", EventType = EventType.Conference, EventFormat= EventFormat.Virtual, Country="Worldwide", City="", Id=Guid.NewGuid(), Url = "https://www.dotnetconf.net/", StartDate = new DateTime(2021, 11, 9) },
                new TechEvent() { Name= "Introduction to .NET 6 (with Scott H)", EventType = EventType.Meetup, EventFormat= EventFormat.Virtual, Country="Worldwide", City="", Id=Guid.NewGuid(), Url = "https://www.meetup.com/Nottingham-IoT-Meetup/events/281658143/", StartDate = new DateTime(2021, 11, 17) },
                new TechEvent() { Name= "NDC Oslo", EventType = EventType.Conference, EventFormat= EventFormat.In_Person, Country="Norway", City="Oslo", Id=Guid.NewGuid(), Url = "https://ndcoslo.com/", StartDate = new DateTime(2021, 11, 29) },
            };

            return Task.FromResult(events);
        }

        public async Task<ITechEvent[]> GetByMonth(int year, int month)
        {
            var events = await GetAll();

            return events.Where(x => x.StartDate.Year == year && x.StartDate.Month == month).ToArray();
        }

        public async Task<ITechEvent[]> GetByEventType(int year, int month, EventType eventType)
        {
            var events = await GetAll();

            return events.Where(x => x.StartDate.Year == year && x.StartDate.Month == month && x.EventType == eventType).ToArray();
        }

        public Task<ITechEvent[]> GetByCountry(EventType eventType, string country)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetAllCountries()
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByEventType(EventType eventType)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByYear(int year)
        {
            throw new NotImplementedException();
        }

        public void AppendTrailingComma()
        {
            throw new NotImplementedException();
        }
    }
}
