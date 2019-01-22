using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using cassandra_app.src;

namespace cassandra_app.src
{
    public static class Statistics
    {
        // private static int reservationsPlaced = 0;
        private static List<int> reservations = new List<int>{0, 0, 0, 0, 0};
        private static List<int> reservationsPlaced = new List<int>{0, 0, 0, 0, 0};
        private static List<int> reservationsCancelled = new List<int>{0, 0, 0, 0, 0};
        private static List<int> reservationsDeclined = new List<int>{0, 0, 0, 0, 0};
        private static List<int> ticketsReserved = new List<int>{0, 0, 0, 0, 0};
        private static List<int> ticketsCancelled = new List<int>{0, 0, 0, 0, 0};
        private static List<int> ticketsDeclined = new List<int>{0, 0, 0, 0, 0};
        private static List<int> reservationsNotFound = new List<int>{0, 0, 0, 0, 0};
        private static int client = 0;

        public static void Reservation(int eventId)
        {
            reservations[eventId - 1] += 1;
        }

        /// <summary>
        /// Reservation placed correctly
        /// </summary>
        public static void Placed(int eventId, int tickets)
        {
            // reservations[eventId - 1] += 1;
            reservationsPlaced[eventId - 1] += 1;
            ticketsReserved[eventId - 1] += tickets;
        }

        /// <summary>
        /// Reservation cancelled
        /// </summary>
        public static void Cancelled(int eventId, int tickets)
        {
            // reservations[eventId - 1] -= 1;
            reservationsCancelled[eventId - 1] += 1;
            ticketsCancelled[eventId - 1] += tickets;
            ticketsReserved[eventId - 1] -= tickets;
            
        }

        /// <summary>
        /// Reservation declined - already sold out
        /// </summary>
        public static void Declined(int eventId, int tickets)
        {
            reservationsDeclined[eventId - 1] += 1;
            ticketsDeclined[eventId - 1] += tickets;
            
        }

        /// <summary>
        /// Reservation with given id not found (not yet replicated)
        /// </summary>
        public static void NotFound(int eventId)
        {
            reservationsNotFound[eventId - 1] += 1;
        }

        /// <summary>
        /// Reservation client created
        /// </summary>
        public static void ClientCreated()
        {
            client += 1;
        }


        /// <summary>
        /// Returns reservation count for given event
        /// </summary>
        public static int GetReservations(int eventId)
        {
            return reservations[eventId - 1];
        }

        /// <summary>
        /// Returns placed reservation count for given event
        /// </summary>
        public static int GetReservationsPlaced(int eventId)
        {
            return reservationsPlaced[eventId - 1];
        }

        /// <summary>
        /// Returns cancelled reservation count for given event
        /// </summary>
        public static int GetReservationsCancelled(int eventId)
        {
            return reservationsCancelled[eventId - 1];
        }

        /// <summary>
        /// Returns declined reservation count for given event
        /// </summary>
        public static int GetReservationsDeclined(int eventId)
        {
            return reservationsDeclined[eventId - 1];
        }

        /// <summary>
        /// Returns reserved tickets count for given event
        /// </summary>
        public static int GetTicketsReserved(int eventId)
        {
            return ticketsReserved[eventId - 1];
        }

        /// <summary>
        /// Returns cancelled tickets count for given event
        /// </summary>
        public static int GetTicketsCancelled(int eventId)
        {
            return ticketsCancelled[eventId - 1];
        }

        /// <summary>
        /// Returns declined tickets count for given event
        /// </summary>
        public static int GetTicketsDeclined(int eventId)
        {
            return ticketsDeclined[eventId - 1];
        }

        /// <summary>
        /// Returns how many times reservations weren't found
        /// </summary>
        public static int GetNotFound(int eventId)
        {
            return reservationsNotFound[eventId - 1];
        }

        /// <summary>
        /// Returns created clients count
        /// </summary>
        public static int GetClientCount()
        {
            return client;
        }

    }
}