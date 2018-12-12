using System.Collections.Generic;
using System.ComponentModel;

namespace cassandra_app.cassandra.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public List<string> Cast { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
    }
}