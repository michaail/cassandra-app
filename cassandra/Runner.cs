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
            movie = movies[GetRandom(moviesCount)];

            // Get coresponding screenings
            List<Screening> screenings = b.GetScreeningsOfMovie(movie.Id);
            Screening screening;
            if (screenings == null)
            {
                Console.WriteLine("No screening for movie {0}", movie.Title);
                return;
            }
            int screeningsCount = screenings.Count;
            screening = screenings[GetRandom(screeningsCount)];

            
            
            string user = GenerateUser();

            b.BuyTicket(screening.Id, user);
            
            b.Close();
        }

        

        // public void Stress(int threads, int amount)
        public void Stress()
        {
            Backend backend = new Backend();

            Run(backend);
        }
    }
}