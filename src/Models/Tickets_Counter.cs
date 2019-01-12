using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cassandra;

namespace cassandra_app.src.Models
{
    public class Tickets_Counter
    {
        public int Event_Id { get; set; }
        public long Remaining { get; set; }
    }
}