using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Cassandra.Mapping;
using cassandra_app.src;

namespace cassandra_app.src.Controllers
{
    public class Tickets_Counter
    {
        private ISession session;
        private IMapper mapper;
        // private CqlQueryOptions queryOptions;
        private PreparedStatement incrementCounter, decrementCounter;
        private ConsistencyLevel consistencyLevel;

        public Tickets_Counter(BackendSession backend)
        {
            this.session = backend.GetSession();
            this.mapper = backend.GetMapper();
            // this.queryOptions = backend.GetQueryOptions();
            var statements = backend.GetPreparedStatements();
            this.incrementCounter = statements[0];
            this.decrementCounter = statements[1];


        }

        public List<Models.Tickets_Counter> GetAllTickets_Counters()
        {
            IEnumerable<Models.Tickets_Counter> ticketsCounters;
            try 
            {
                ticketsCounters = mapper.Fetch<Models.Tickets_Counter>();
            }
            catch (Exception e)
            {
                ticketsCounters = null;
                Console.WriteLine(e.Message);
                return null;
            }
            return ticketsCounters.ToList();
        }

        public long? GetCurrentTicketsCount(int eventId)
        {
            Models.Tickets_Counter counter;
            try
            {
                counter = mapper.Fetch<Models.Tickets_Counter>("WHERE event_id = ?", 
                    eventId).First();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            return counter.Remaining;
        }

        public void IncrementRemainingTicketsCountBy(int eventId, int value)
        {
            var statement = decrementCounter.Bind((long)value, eventId);
            try
            {
                session.Execute(statement);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void DecrementRemainingTicketsCountBy(int eventId, int value)
        {
            var statement = incrementCounter.Bind((long)value, eventId);
            try
            {
                session.Execute(statement);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}