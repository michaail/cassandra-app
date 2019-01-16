using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cassandra;
using Cassandra.Mapping.Attributes;

namespace cassandra_app.src.Models
{
    public class Reservation
    {
        [PartitionKey]
        public Guid Id { get; set; }
        [PartitionKey]
        public int Event_Id { get; set; }
        public int User { get; set; }
        public int Tickets_count { get; set; }
        public bool Cancelled { get; set; }
        public DateTimeOffset Timestamp { get; set; } 
        
        public Reservation(Guid id, int eventId, int user, int ticketsCount, bool cancelled, DateTimeOffset timestamp) {
            Id = id;
            Event_Id = eventId;
            User = user;
            Tickets_count = ticketsCount;
            Cancelled = cancelled;
            Timestamp = timestamp;
        }
    }
}