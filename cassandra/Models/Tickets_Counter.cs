using System;

namespace cassandra_app.cassandra.Models
{
    public class Tickets_Counter
    {
        public Guid Movie_Id { get; set; }
        public Guid Screening_Id { get; set; }
        public long Sold { get; set; }

        public Tickets_Counter(Guid movieId, Guid screeningId, long sold)
        {
            this.Movie_Id = movieId;
            this.Screening_Id = screeningId;
            this.Sold = sold;
        }
    }
}