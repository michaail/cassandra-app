using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Cassandra.Mapping;
using cassandra_app.src;

namespace cassandra_app.src.Controllers
{
    public class Event
    {
        private IMapper mapper;

        public Event(BackendSession backend)
        {
            this.mapper = backend.GetMapper();
        }

        public List<Models.Event> GetAllEvents()
        {
            IEnumerable<Models.Event> events;
            try
            {
                events = mapper.Fetch<Models.Event>();
            }
            catch (Exception e)
            {
                events = null;
                Console.WriteLine(e.Message);
                return null;
            }
            return events.ToList();
        }

        public void SoldOut(int eventId)
        {
            
        }
    }
}