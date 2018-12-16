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

//        private PreparedStatement _getAllTheaters, _getAllMovies, _getAllScreenings, _getTickets;
//        private PreparedStatement _getTicketsCounters;
//        private PreparedStatement _getMovie, _getScreening;
//        private PreparedStatement _getScreeningsOfMovie, _getTheaterScreenings;
//        private PreparedStatement _getSoldTicketsCount;
//        private PreparedStatement _buyTicket, _updateTicketCounter;
//        private PreparedStatement _addMovie, _addTheater, _addScreening, _addTheaterScreening;

        private IMapper mapper;

        public Backend()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            session = cluster.Connect("cineplex");
            mapper = new Mapper(session);
                
//            InitializeStatements();

            GetAllMovies();

            GetMovie(Guid.Parse("1871dc17-2216-4be1-b50c-d8cd89362e24"));


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
            IEnumerable<Movie> movies;
            try
            {
                movies = mapper.Fetch<Movie>();
            }
            catch (Exception e)
            {
                movies = null;
                Console.WriteLine(e.Message);
            }
            
            return movies;
        }

        public IEnumerable<Theater> GetAllTheaters()
        {
            IEnumerable<Theater> theaters;
            try
            {
                theaters = mapper.Fetch<Theater>();
            }
            catch (Exception e)
            {
                theaters = null;
                Console.WriteLine(e.Message);
            }
           
            return theaters;
        }

        public IEnumerable<Screening> GetAllScreenings()
        {
            IEnumerable<Screening> screenings;
            try
            {
                screenings = mapper.Fetch<Screening>();
            }
            catch (Exception e)
            {
                screenings = null;
                Console.WriteLine(e.Message);
            }
            
            return screenings;
        }

        public Movie GetMovie(Guid id)
        {
            Movie movie;
            try
            {
                movie = mapper.Fetch<Movie>("WHERE id = ?", id).First();
            }
            catch (Exception e)
            {
                movie = null;
                Console.WriteLine(e.Message);
            }

            return movie;
        }

        public Screening GetScreening(Guid id)
        {
            Screening screening;
            try
            {
                screening = mapper.Fetch<Screening>("WHERE id = ?", id).First();
            }
            catch (Exception e)
            {
                screening = null;
                Console.WriteLine(e.Message);
            }

            return screening;
        }

        public IEnumerable<Screening> GetScreenings(Guid movieId, Guid theaterId, LocalDate date)
        {
            IEnumerable<Screening> screenings;
            try
            {
                screenings = mapper.Fetch<Screening>("WHERE movie_id = ? AND theater_id = ? AND date = ?",
                    movieId, theaterId, date);
            }
            catch (Exception e)
            {
                screenings = null;
                Console.WriteLine(e);
            }

            return screenings;
        }

        public long GetSoldTickets(Guid screeningId)
        {
            long count;
            try
            {
                count = mapper.Fetch<Tickets_Counter>("WHERE screening_id = ?", screeningId).First().Sold;
            }
            catch (Exception e)
            {
                count = 0;
                Console.WriteLine(e);
            }

            return count;
        }

        public IEnumerable<Ticket> GetTickets(Guid screeningId)
        {
            Screening screening = GetScreening(screeningId);

            if (screening == null)
            {
                return null;
            }

            IEnumerable<Ticket> tickets;
            try
            {
                //TODO
                tickets = mapper.Fetch<Ticket>("WHERE screening_id = ? AND date = ? AND ");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void InsertMovie(string title, string description, int duration, int prodYear)
        {
            Guid movieId = Guid.NewGuid();
            Movie newMovie = new Movie(movieId, title, description, duration, prodYear);
            mapper.Insert(newMovie);
            Console.WriteLine("Inserted movie: {0} ({1}) : {2} min | id: {3}", 
                title, prodYear.ToString(), duration.ToString(), movieId.ToString());
            
            
        }

        //TODO
        public bool BuyTicket(Guid screeningId, string user)
        {
            Screening screening = GetScreening(screeningId);

            if (screening == null)
            {
                return false;
            }

            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        
    }
}