using Cassandra;

namespace cassandra_app.cassandra
{
    public class Backend
    {
        private Cluster cluster;

        public Backend()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            ISession session = cluster.Connect("demo");

            session.Execute("insert into Test (name, age) values ('Michal', 23)");
        }
        
        
    }
}