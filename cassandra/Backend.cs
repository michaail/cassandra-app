using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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

        private PreparedStatement _incrementCounter, _decrementCounter;
        private PreparedStatement _deleteTicket;
        private IMapper mapper;

        public Backend()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            session = cluster.Connect("cineplex");
            mapper = new Mapper(session);
            InitializeStatements();

            bool res;
            // bool res = BuyTicket(Guid.Parse("9e1b8d2b-85f3-44fb-acda-bfdd64b84532"), "galeo");
            // Console.WriteLine(res);

            res = DeleteTicket(Guid.Parse("9e1b8d2b-85f3-44fb-acda-bfdd64b84532"), 
                Guid.Parse("67dcd76c-020d-11e9-8a31-ae2f169ca2f5"));
            Console.WriteLine(res);

            Close();
        }

        private void InitializeStatements()
        {
            _incrementCounter = session.Prepare("UPDATE tickets_counter SET sold=sold+1 WHERE screening_id = ? AND movie_id = ?");
            _decrementCounter = session.Prepare("UPDATE tickets_counter SET sold=sold-1 WHERE screening_id = ? AND movie_id = ?");

            _deleteTicket = session.Prepare("DELETE FROM ticket WHERE screening_id = ? AND id = ?");
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
                tickets = mapper.Fetch<Ticket>("WHERE screening_id = ?", screeningId);
            }
            catch (Exception e)
            {
                tickets = null;
                Console.WriteLine(e.Message);
            }

            return tickets;
        }

        public Ticket GetTicket(Guid ticketId, Guid screeningId)
        {
            Ticket ticket;
            try
            {
                ticket = mapper.Fetch<Ticket>("WHERE id = ? AND screening_id = ?", 
                    ticketId, screeningId).First();
            }
            catch (Exception e)
            {
                ticket = null;
                Console.WriteLine(e.Message);
            }

            return ticket;
        }

        public void InsertMovie(string title, string description, int duration, int prodYear)
        {
            Guid movieId = Guid.NewGuid();
            Movie newMovie = new Movie(movieId, title, description, duration, prodYear);
            mapper.Insert(newMovie);
            Console.WriteLine("Inserted movie: {0} ({1}) : {2} min | id: {3}", 
                title, prodYear.ToString(), duration.ToString(), movieId.ToString());
            
            
        }

        public bool BuyTicket(Guid screeningId, string user)
        {
            Screening screening = GetScreening(screeningId);
            if (screening == null)
                return false;
            
            bool result = false;

            Ticket ticket = new Ticket(TimeUuid.NewId(), screeningId, user, DateTimeOffset.Now);
            var statement = _incrementCounter.Bind(screeningId, screening.Movie_Id);
            try
            {
                mapper.Insert(ticket);
                session.Execute(statement);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Console.WriteLine(e);
            }

            return result;
        }

        public bool DeleteTicket(Guid screeningId, Guid ticketId)
        {
            Screening screening = GetScreening(screeningId);
            if (screening == null)
                return false;
            
            bool result = false;

            Ticket ticket = GetTicket(ticketId, screeningId);
            if (ticket == null)
                return false;

            var decrementStatement = _decrementCounter.Bind(screeningId, screening.Movie_Id);
            var deleteStatement = _deleteTicket.Bind(screeningId, ticketId);
            try
            {
                session.Execute(deleteStatement);
                session.Execute(decrementStatement);
                result = true;
            }
            catch (Exception e)
            {
                result = false;
                Console.WriteLine(e.Message);
            }
            
            return result;
        }

        public void Close()
        {
            try
            {
                session.Cluster.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        
        
    }
}