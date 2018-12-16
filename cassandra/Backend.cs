using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Cassandra;
using cassandra_app.cassandra.Models;
using Cassandra.Mapping;

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
        private PreparedStatement _addMovie, _addTheater, _addScreening, _addTheaterScreening;

        private IMapper mapper;

        public Backend()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            session = cluster.Connect("cineplex");
            mapper = new Mapper(session);
                
            InitializeStatements();

            GetAllMovies();
            InsertMovie();
            GetAllMovies();

        }

        private void InitializeStatements()
        {
//            _getAllTheaters = session.Prepare("SELECT * FROM theaters");
//            _getAllMovies = session.Prepare("SELECT * FROM movies");
//            _getAllScreenings = session.Prepare("SELECT * FROM screenings");
//
//            _getTickets = session.Prepare("SELECT * FROM tickets WHERE screening_id=? LIMIT ?");
//            _getTicketsCounters = session.Prepare("SELECT * FROM tickets_counter");
//            _getMovie = session.Prepare("SELECT * FROM movies WHERE id=?");
//            _getScreening = session.Prepare("SELECT * FROM screenings WHERE id=?");
//            _getScreeningsOfMovie = session.Prepare("SELECT * FROM screenings WHERE movie_id=? AND date=?");
//            _getTheaterScreenings = session.Prepare("SELECT * FROM theater_screenings " +
//                                                    "WHERE movie_id=? AND date=? AND theater_id=?");
//            _getSoldTicketsCount = session.Prepare("SELECT sold, screening_id FROM tickets_counter " +
//                                                   "WHERE screening_id=?");
//            _buyTicket = session.Prepare("INSERT INTO tickets (screening_id, id, user, timestamp) VALUES (?, ?, ?, ?)");
//            _updateTicketCounter = session.Prepare("UPDATE tickets_counter SET sold=sold+1 WHERE screening_id=?");
//            _addMovie = session.Prepare("INSERT INTO movies (id, title, prodyear, description, duration) " +
//                                        "VALUES (uuid(), ?, ?, ?, ?");
//            _addTheater = session.Prepare("INSERT INTO theaters (id, name) VALUES (uuid(), ?)");
//            _addScreening = session.Prepare(
//                "INSERT INTO screenings (id, movie_id, theater_id, date, time, hall, hall_capacity) " +
//                "VALUES (uuid(), ?, ?, ?, ?, ?, ?)");
//            _addTheaterScreening = session.Prepare(
//                "INSERT INTO theater_screenings (id, movie_id, theater_id, date, time, hall, hall_capacity) " +
//                "VALUES (?, ?, ?, ?, ?, ?, ?)");

        }

        public IEnumerable<Movie> GetAllMovies()
        {
            IEnumerable<Movie> movies = mapper.Fetch<Movie>();
            Movie movie = movies.First();
            Console.WriteLine(movie.Title);
            return movies;
        }

        public void InsertMovie()
        {
            Movie newMovie = new Movie(Guid.NewGuid(), "Jaws", "Movie about sharks", 120, 1980);
            mapper.Insert(newMovie);
        }
        
        
    }
}