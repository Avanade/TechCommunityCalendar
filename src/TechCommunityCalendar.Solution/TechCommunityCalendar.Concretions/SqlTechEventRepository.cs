using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TechCommunityCalendar.Enums;
using TechCommunityCalendar.Interfaces;

namespace TechCommunityCalendar.Concretions
{
    public class SqlTechEventRepository : ITechEventQueryRepository, ITechEventAdminRepository
    {
        string _connectionString;

        public SqlTechEventRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private ITechEvent ReadSingleRow(IDataRecord dataRecord)
        {
            var techEvent = new TechEvent();
            techEvent.Name = (string)dataRecord["Name"];
            techEvent.City = dataRecord["city"] == DBNull.Value ? null : (string)dataRecord["city"];
            techEvent.Country = (string)dataRecord["country"];
            techEvent.Duration = (string)dataRecord["duration"];
            techEvent.StartDate = DateTime.Parse(dataRecord["StartDate"].ToString());
            techEvent.EndDate = DateTime.Parse(dataRecord["EndDate"].ToString());
            techEvent.EventFormat = (EventFormat)Enum.Parse(typeof(EventFormat), dataRecord["EventFormat"].ToString());
            techEvent.EventType = (EventType)Enum.Parse(typeof(EventType), dataRecord["EventType"].ToString());
            techEvent.TwitterHandle = dataRecord["TwitterHandle"] == DBNull.Value ? null : (string)dataRecord["TwitterHandle"];
            techEvent.Url = dataRecord["Url"] == DBNull.Value ? null : (string)dataRecord["Url"];
            techEvent.Hidden = dataRecord["Hidden"] == DBNull.Value ? false : (bool)dataRecord["Hidden"];

            return techEvent;
        }

        public void AppendTrailingComma()
        {
            throw new NotImplementedException();
        }

        public async Task ClearData()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand($"DELETE FROM Events", connection);
                connection.Open();

                await command.ExecuteNonQueryAsync();

            }
        }

        public async Task<ITechEvent> Get(string id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand($"SELECT * FROM Events WHERE Id = @id", connection);
                command.Parameters.AddWithValue("id", id);
                connection.Open();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    var techEvent = ReadSingleRow((IDataRecord)reader);

                    return techEvent;
                }

                reader.Close();
            }

            return null;
        }

        public async Task<ITechEvent[]> GetAll()
        {
            var techEvents = new List<ITechEvent>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Events", connection);
                connection.Open();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                // Call Read before accessing data.
                while (reader.Read())
                {
                    var techEvent = ReadSingleRow((IDataRecord)reader);

                    try
                    {
                        techEvents.Add(techEvent);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

                reader.Close();
            }

            return techEvents.ToArray();
        }

        public Task<string[]> GetAllCountries()
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByCountry(EventType eventType, string country)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByEventType(int year, int month, EventType eventType)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByEventType(EventType eventType)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByMonth(int year, int month)
        {
            throw new NotImplementedException();
        }

        public Task<ITechEvent[]> GetByYear(int year)
        {
            throw new NotImplementedException();
        }

        public async Task Add(ITechEvent techEvent)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO [Events] (Id, [Name], City, Country, Url, TwitterHandle, StartDate, EndDate, Duration, EventType, EventFormat, Verified, Hidden) " +
                    "VALUES (@id,@name,@city,@country,@url,@twitterHandle,@startDate,@endDate,@duration,@eventType,@eventFormat,@verified,@hidden)";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("id", techEvent.FriendlyName);
                command.Parameters.AddWithValue("name", techEvent.Name);
                command.Parameters.AddWithValue("city", (object)techEvent.City ?? DBNull.Value);
                command.Parameters.AddWithValue("country", techEvent.Country);
                command.Parameters.AddWithValue("url", (object)techEvent.Url ?? DBNull.Value);
                command.Parameters.AddWithValue("twitterHandle", (object)techEvent.TwitterHandle ?? DBNull.Value);
                command.Parameters.AddWithValue("startDate", techEvent.StartDate);
                command.Parameters.AddWithValue("endDate", techEvent.EndDate);
                command.Parameters.AddWithValue("duration", techEvent.Duration);
                command.Parameters.AddWithValue("eventType", techEvent.EventType);
                command.Parameters.AddWithValue("eventFormat", techEvent.EventFormat);
                command.Parameters.AddWithValue("verified", 0);
                command.Parameters.AddWithValue("hidden", techEvent.Hidden);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Update(ITechEvent techEvent)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE [Events] " +
                    "SET Hidden = @hidden, " +
                    "EndDate = @startDate, " +
                    "StartDate = @endDate, " +
                    "Url = @url, " +
                    "Name = @name " +
                    "WHERE id = @id";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("id", techEvent.Id);
                command.Parameters.AddWithValue("hidden", techEvent.Hidden);
                command.Parameters.AddWithValue("startDate", techEvent.StartDate);
                command.Parameters.AddWithValue("endDate", techEvent.EndDate);
                command.Parameters.AddWithValue("url", techEvent.Url);
                command.Parameters.AddWithValue("name", techEvent.Name);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task Remove(string id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM [Events] " +
                    "WHERE id = @id";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("id", id);
                connection.Open();
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
