using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Cassandra.Mapping;

namespace cassandra_app.src
{
    public class BackendSession
    {
        private Cluster cluster;
        private ISession session;
        private IMapper mapper;
        private ConsistencyLevel consistencyLevel;
        private PreparedStatement incrementCounter, decrementCounter;

        public BackendSession(bool quorum) 
        {
            consistencyLevel = new ConsistencyLevel();

            if (quorum)
                consistencyLevel = ConsistencyLevel.Quorum;
            else
                consistencyLevel = ConsistencyLevel.One;

            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .WithQueryOptions(new QueryOptions().SetConsistencyLevel(consistencyLevel))
                .Build();

            try 
            {
                session = cluster.Connect("ticketer");
            }
            catch (Exception e)
            {
                Console.WriteLine("Couldn't connect to cluster: {0}", e.Message);
                return;
            }
            
            // var config = new MappingConfiguration();
            // config.Define(new Map<Models.Reservation>()
            //     .TableName("reservation")
            //     .PartitionKey(r => r.Id));

            mapper = new Mapper(session);
            InitializeStatements();

        }

        private void InitializeStatements() 
        {
            // on cancelled reservation
            incrementCounter = session.Prepare("UPDATE tickets_counter SET remaining = remaining + ? WHERE event_id = ?");
            // on placed reservation
            decrementCounter = session.Prepare("UPDATE tickets_counter SET remaining = remaining - ? WHERE event_id = ?");

        }

        public ISession GetSession()
        {
            return session;
        }

        // public CqlQueryOptions GetQueryOptions()
        // {
        //     return queryOptions;
        // }

        public IMapper GetMapper()
        {
            return mapper;
        }

        public List<PreparedStatement> GetPreparedStatements()
        {
            PreparedStatement[] arr = {incrementCounter, decrementCounter};
            return new List<PreparedStatement>(arr);
        }
    }
}