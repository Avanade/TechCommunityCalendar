using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    public class ElasticSearchTechEventRepository : ITechEventQueryRepository
    {
        public Task<ITechEvent> Get(int year, int month, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByCountry(int year, int month, EventType eventType, string country)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByEventType(int year, int month, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByMonth(int year, int month)
        {
            throw new NotImplementedException();
        }
    }
}
