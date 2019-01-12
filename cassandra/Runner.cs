using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cassandra_app.cassandra.Models;

namespace cassandra_app.cassandra
{
    public class Runner
    {
        private string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private int GetRandom(int upperBound)
        {
            Random r = new Random();
            int random = r.Next(0, upperBound);
            return random;
        }

        private string GenerateUser()
        {
            char[] stringChars = new char[8];
            Random r = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[r.Next(chars.Length)];
            }
            return new string(stringChars);
        }
        
        public void Run(Backend b)
        {
            // Get all movies and select random one
            List<Movie> movies = b.GetAllMovies();
            Movie movie;
            if (movies == null)
            {
                Console.WriteLine("No movies found");
                return;
            }
            int moviesCount = movies.Count;
            Console.WriteLine(moviesCount);
            movie = movies[GetRandom(moviesCount)];
            Console.WriteLine("selected movie: {0}", movie.Title);

            // Get coresponding screenings
            List<Screening> screenings = b.GetScreeningsOfMovie(movie.Id);
            Screening screening;
            int screeningsCount = screenings.Count;
            Console.WriteLine(screeningsCount);
            if (screeningsCount == 0)
            {
                Console.WriteLine("No screening for movie {0}", movie.Title);
                return;
            }
            
            screening = screenings[GetRandom(screeningsCount)];

            
            
            string user = GenerateUser();

            b.BuyTicket(screening.Id, user);
            
            b.Close();
        }

        

        // public void Stress(int threads, int amount)
        public void Stress()
        {
            Backend backend = new Backend(false);

            Run(backend);
        }
    }
}