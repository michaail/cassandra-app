using System;
using Cassandra.Mapping.Attributes;

namespace cassandra_app.cassandra.Models
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public int ProdYear { get; set; }

        public Movie(Guid id, string title, string description, int duration, int prodYear)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.Duration = duration;
            this.ProdYear = prodYear;
        }
    }
}