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
        private static int reservationsRequested = 0;
        private static int reservationsCancelled = 0;
        private static int reservationDeclined = 0;
        private static int ticketsReserved = 0;

        private static void log(int user, string logMsg)
        {
            Console.WriteLine("[{0}] -> {1}", user, logMsg);
        }

        public static void Placed()
        {
            reservationsPlaced += 1;
        }

        public static void Requested()
        {
            reservationsRequested += 1;
        }

        public static void Cancelled()
        {
            reservationsCancelled += 1;
        }


    }
}