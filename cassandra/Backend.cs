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

        private PreparedStatement _updateCounter;
        private IMapper mapper;

        public Backend()
        {
            cluster = Cluster.Builder()
                .AddContactPoint("127.0.0.1")
                .Build();

            session = cluster.Connect("cineplex");
            mapper = new Mapper(session);

            GetAllMovies();

            GetMovie(Guid.Parse("1871dc17-2216-4be1-b50c-d8cd89362e24"));

        }

        private void InitializeStatements()
        {
            _updateCounter = session.Prepare("UPDATE tickets_counter SET sold=sold+1 WHERE screening_id=?");
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
            bool result = false;
            if (screening == null)
            {
                return false;
            }

            Ticket ticket = new Ticket(TimeUuid.NewId(), screening.Id, user, DateTimeOffset.Now);
            var statement = _updateCounter.Bind();
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