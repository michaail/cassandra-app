using System;

namespace cassandra_app.cassandra.Models
{
    public class TicketsCounter
    {
        public Guid Movie_Id { get; set; }
        public Guid Screening_Id { get; set; }
        public long Counter { get; set; }

        public TicketsCounter(Guid movieId, Guid screeningId, long counter)
        {
            this.Movie_Id = movieId;
            this.Screening_Id = screeningId;
            this.Counter = counter;
        }
    }
}