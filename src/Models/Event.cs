using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cassandra;

namespace cassandra_app.src.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Total_Tickets { get; set; }
        public bool Sold_Out { get; set; }
    }
}