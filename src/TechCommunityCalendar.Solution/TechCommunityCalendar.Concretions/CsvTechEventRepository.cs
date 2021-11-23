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
    /// Cons: Whole list could be coppied easily and used elsewhere..?
    /// </summary>
    public class CsvTechEventRepository : ITechEventQueryRepository
    {
        string csvPath;//

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
                    try
                    {
                        string name = csv.GetField(0);
                        EventType eventType = GetEventType(csv.GetField(1));
                        string duration = csv.GetField(3);
                        string url = csv.GetField(4);
                        EventFormat eventFormat = GetEventFormat(csv.GetField(5));
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
                        else
                        {

                        }

                        //TimeSpan duration;
                        //if (TimeSpan.TryParse(csv.GetField(3), out duration))
                        //    record.Duration = duration;

                        techEvents.Add(record);

                    }
                    catch (Exception)
                    {

                    }
                }
            }


            return await Task.FromResult(techEvents.ToArray());
        }

        private EventType GetEventType(string eventType)
        {
            switch (eventType)
            {
                case "Conference":
                    return EventType.Conference;

                case "Hackathon":
                    return EventType.Hackathon;

                case "Meetup":
                    return EventType.Meetup;

                case "CallForPaper":
                    return EventType.Call_For_Paper;

                case "Website":
                    return EventType.Call_For_Paper;

                default:
                    return EventType.Unknown;
            }
        }

        private EventFormat GetEventFormat(string eventFormat)
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

        public async Task<string[]> GetAllCountries()
        {
            var events = await GetAll();

            return events.Select(x => x.Country).Distinct().ToArray();
        }
    }
}
