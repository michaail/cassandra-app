using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Cassandra;

namespace cassandra_app.cassandra
{
    public class Backend
    {
        private Cluster cluster;


        private ISession session;

        private PreparedStatement _getAllTheaters, _getAllMovies, _getAllScreenings, _getTickets;
        private PreparedStatement _getTicketsCounters;
        private PreparedStatement _getMovie, _getScreening;
        private PreparedStatement _getScreeningsOfMovie, _getTheaterScreenings;
        private PreparedStatement _getSoldTicketsCount;

        private PreparedStatement _buyTicket, _updateTicketCounter;
        
        public Backend()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            session = cluster.Connect("cineplex");

            InitializeStatements();
            
        }

        private void InitializeStatements()
        {
            _getAllTheaters = session.Prepare("SELECT * FROM theaters");
            _getAllMovies = session.Prepare("SELECT * FROM movies");
            _getAllScreenings = session.Prepare("SELECT * FROM screenings");
            
            _getTickets = session.Prepare("SELECT * FROM tickets WHERE screening_id=? LIMIT ?");
            _getTicketsCounters = session.Prepare("SELECT * FROM tickets_counter");
            _getMovie = session.Prepare("SELECT * FROM movies WHERE id=?");
            _getScreening = session.Prepare("SELECT * FROM screenings WHERE id=?");
            _getScreeningsOfMovie = session.Prepare("SELECT * FROM screenings WHERE movie_id=? AND date=?");
            _getTheaterScreenings =
                session.Prepare("SELECT * FROM theater_screenings WHERE movie_id=? AND date=? AND theater_id=?");

            _getSoldTicketsCount =
                session.Prepare("SELECT sold, screening_id FROM tickets_counter WHERE screening_id=?");


            _buyTicket = session.Prepare("INSERT INTO tickets (screening_id, id, user, timestamp) VALUES (?, ?, ?, ?)");
            _updateTicketCounter = session.Prepare("UPDATE tickets_counter SET sold=sold+1 WHERE screening_id=?");
            
        }
        
    }
}