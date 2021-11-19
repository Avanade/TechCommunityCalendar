using System;
using System.Threading.Tasks;
using TechCommunityCalendar.Enums;

namespace TechCommunityCalendar.Interfaces
{
    public interface ITechEventQueryRepository
    {
        Task<ITechEvent[]> GetByMonth(int year, int month);
        Task<ITechEvent[]> GetByEventType(int year, int month, EventType eventType);
        Task<ITechEvent[]> GetByCountry(int year, int month, EventType eventType, string country);
        Task<ITechEvent> Get(int year, int month, Guid id);

    }
}
