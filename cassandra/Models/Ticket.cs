using System;
using Cassandra;

namespace cassandra_app.cassandra.Models
{
    public class Ticket
    {
        public TimeUuid Id { get; set; }
        public Guid Screening_Id { get; set; }
        public string User { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        public Ticket(TimeUuid id, Guid screeningId, string user, DateTimeOffset timestamp)
        {
            this.Id = id;
            this.Screening_Id = screeningId;
            this.User = user;
            this.Timestamp = timestamp;
        }
    }
}