using System;

namespace cassandra_app.cassandra.Models
{
    public class TicketsCounter
    {
        public Guid MovieId { get; set; }
        public Guid ScreeningId { get; set; }
        public long Counter { get; set; }

        public TicketsCounter(Guid movieId, Guid screeningId, long counter)
        {
            this.MovieId = movieId;
            this.ScreeningId = screeningId;
            this.Counter = counter;
        }
    }
}