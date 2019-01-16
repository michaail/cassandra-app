using System;
using Cassandra;
using cassandra_app.src;

namespace cassandra_app.src
{
    public class Event
    {
        private int id;
        private int ticketsCount;
        private ISession session;


        public Event(BackendSession backend, int eventId, int ticketsCount)
        {
            this.session = backend.GetSession();
            this.id = eventId;
            this.ticketsCount = ticketsCount;
        }

        public int GetId()
        {
            return this.id;
        }

        public int GetTicketsCount()
        {
            return this.ticketsCount;
        }

        

    }
}