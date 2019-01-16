using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cassandra_app.src;

namespace cassandra_app.src
{
    public static class Statistics
    {
        private static int reservationsPlaced = 0;
        private static int reservations = 0;
        private static int reservationsCancelled = 0;
        private static int reservationsDeclined = 0;
        private static int ticketsReserved = 0;
        private static int ticketsDeclined = 0;
        private static int client = 0;

        private static void log(int user, string logMsg)
        {
            Console.WriteLine("[{0}] -> {1}", user, logMsg);
        }

        public static void Placed(int tickets)
        {
            reservations += tickets;
            reservationsPlaced += tickets;
        }

        // public static void Reserved(int tickets)
        // {
        //     reservationsRequested += tickets;
        // }

        public static void Cancelled(int tickets)
        {
            reservations -= tickets;
            reservationsCancelled += tickets;
        }

        public static void Declined(int tickets)
        {
            reservationsDeclined += tickets;
        }

        public static void ClientCreated()
        {
            client += 1;
        }

        public static int GetPlaced()
        {
            return reservationsPlaced;
        }

        public static int GetReserved()
        {
            return reservations;
        }

        public static int GetCancelled()
        {
            return reservationsCancelled;
        }

        public static int GetDeclined()
        {
            return reservationsDeclined;
        }

        public static int GetClientCount()
        {
            return client;
        }

    }
}