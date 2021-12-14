using AzureTableStorageHelper.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechCommunityCalendar.Data.Azure;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;


namespace TechCommunityCalendar.Concretions
{
    public class AzureTechEventRepository : ITechEventQueryRepository
    {
        string _connectionString;

        public AzureTechEventRepository(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));

            _connectionString = connectionString;
        }
        
        

        public async Task<ITechEvent> Get(int year, int month, Guid id)
        {
            string partitionKey = $"{year}-{month}";
            string rowKey = id.ToString();

            var techEvent = await TableStorageHelper.RetrieveSingle<EventEntity>("", "", partitionKey, rowKey);

            return techEvent;
        }

        public async Task<IEnumerable<ITechEvent>> GetByMonth(int year, int month)
        {
            var techEvents = await TableStorageHelper.RetrieveManyByPartition<EventEntity>("{ConnectionString}", "Events", $"{year}-{month}");

            return techEvents;
        }

        public Task<ITechEvent[]> GetByCountry(EventType eventType, string country)
        {
            throw new NotImplementedException();
        }

        Task<ITechEvent[]> ITechEventQueryRepository.GetByMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        Task<ITechEvent[]> ITechEventQueryRepository.GetByEventType(int year, int month, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetAll()
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
    }
}
