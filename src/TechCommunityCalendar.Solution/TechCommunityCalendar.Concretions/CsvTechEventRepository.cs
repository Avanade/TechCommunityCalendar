using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    /// <summary>
    /// May store data in single Csv in the short term
    /// Pros: Can be updated easily by git commits
    /// Cons: Whole list could be copied easily and used elsewhere..?
    /// </summary>
    public class CsvTechEventRepository : ITechEventQueryRepository
    {
        readonly string csvPath;

        public CsvTechEventRepository(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            csvPath = path;
        }

        public Task<ITechEvent> Get(int year, int month, Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ITechEvent[]> GetByCountry(EventType eventType, string country)
        {
            var events = await GetAll();

            return events.Where(x => x.Country.Equals(country, StringComparison.InvariantCultureIgnoreCase)).ToArray();
        }

        public Task<ITechEvent[]> GetByEventType(int year, int month, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public async Task<ITechEvent[]> GetByMonth(int year, int month)
        {
            var results = await GetAll();

            return results.Where(x => x.StartDate.Year == year && x.StartDate.Month == month).ToArray();
        }

        public async Task<ITechEvent[]> GetByYear(int year)
        {
            var results = await GetAll();

            return results.Where(x => x.StartDate.Year == year).OrderBy(x=>x.StartDate).ToArray();
        }

        public async Task<ITechEvent[]> GetAll()
        {
            var techEvents = new List<ITechEvent>();


            using (var reader = new StreamReader(csvPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                //var records = new List<Foo>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    string name = csv.GetField(0);
                    EventType eventType = EnumParser.ParseEventType(csv.GetField(1));
                    string duration = csv.GetField(3);
                    string url = csv.GetField(4);
                    EventFormat eventFormat = EnumParser.ParseEventFormat(csv.GetField(5));
                    string city = csv.GetField(6);
                    string country = csv.GetField(7);

                    ITechEvent record = new TechEvent
                    {
                        Name = name,
                        EventType = eventType,
                        Duration = duration,
                        Url = url,
                        EventFormat = eventFormat,
                        City = city,
                        Country = country

                    };

                    DateTime startDate;
                    if (DateTime.TryParse(csv.GetField(2), out startDate))
                    {
                        record.StartDate = startDate;
                    }

                    record.EndDate = record.StartDate.Add(TryParseTimeSpan(duration));

                    techEvents.Add(record);
                }
            }

            return await Task.FromResult(techEvents.ToArray());
        }

        private TimeSpan TryParseTimeSpan(string duration)
        {
            // Formats could be
            // 3 days
            // 1 day
            // 30 days
            // 2 hours
            // 12 hours
            // 1 hour
            // etc

            var parts = duration.Split(" ");

            if (duration.Contains("day"))
                return TimeSpan.FromDays(int.Parse(parts[0]));

            if (duration.Contains("hour"))
                return TimeSpan.FromHours(int.Parse(parts[0]));

            return TimeSpan.Zero;
        }



        public async Task<string[]> GetAllCountries()
        {
            var events = await GetAll();

            return events.Select(x => x.Country).Distinct().ToArray();
        }

        public async Task<ITechEvent[]> GetByEventType(EventType eventType)
        {
            var events = await GetAll();

            return events.Where(x => x.EventType.Equals(eventType)).ToArray();
        }
    }
}
