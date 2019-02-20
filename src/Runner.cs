using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using cassandra_app.src;
using cassandra_app.src.Models;

namespace cassandra_app.src
{
    public class Runner
    {
        private BackendSession backend;

        private Controllers.Tickets_Counter counterController;
        private Controllers.Reservation reservationController;
        private Controllers.Event eventController;
    
        private int clientsCount = 200;
        private static int clientIdOffset = 1000;

        public Runner()
        {
            backend = new BackendSession(false);

            counterController = new Controllers.Tickets_Counter(backend);
            reservationController = new Controllers.Reservation(backend);
            eventController = new Controllers.Event(backend);

            CancellationTokenSource source = new CancellationTokenSource();
            
            // Get one of the events to consider
            List<Models.Event> events = eventController.GetAllEvents();
            Models.Event ev = events.Where(e => e.Id == 2).First();
            Random rand = new Random();

            // get initial tickets counter
            Console.WriteLine("Counter value: {0}", counterController.GetCurrentTicketsCount(ev.Id));

            // run 5 rounds
            for (int k=0; k<5; k++)
            {
                // in 1150 threads
                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < 1000; i++)
                {
                    var clientIdOffset_ = clientIdOffset;
                    // run thread to place reservation and cancel 10% of them
                    Thread t = new Thread(() => {
                        Client c = new Client(reservationController, clientIdOffset_, ev.Id, 10, 5, 1, ev.Total_Tickets);
                        c.run();
                    });

                    clientIdOffset += 1;
                    t.Start();
                    threads.Add(t);

                }
                foreach (Thread th in threads)
                {
                    th.Join();
                }
                GetStatistics(ev.Id);
            }

            Thread.Sleep(1000);
            GetStatistics(ev.Id);

        }

        Models.Event GetRandomEvent(List<Models.Event> events)
        {
            Random random = new Random();
            // Event randomEvent;
            int r = random.Next(0, events.Count);
            return events[r];
        }


        private void GetStatistics(int eventId)
        {
            Console.WriteLine("Counter value: {0}", counterController.GetCurrentTicketsCount(eventId));
            Console.WriteLine(
                "Statistics: reservations: {0}\n" +
                "Statistics: tickets reserved: {1}\n" +
                "Statistics: reservations cancelled: {2}\n" +
                "Statistics: tickets cancelled: {3}\n" +
                "Statistics: reservations declined: {4}\n" +
                "Statistics: tickets declined: {5}\n" +
                "Statistics: not found (not yer replicated): {6}\n",
                Statistics.GetReservations(eventId), 
                Statistics.GetTicketsReserved(eventId), 
                Statistics.GetReservationsCancelled(eventId),
                Statistics.GetTicketsCancelled(eventId), 
                Statistics.GetReservationsDeclined(eventId),
                Statistics.GetTicketsDeclined(eventId),
                Statistics.GetNotFound(eventId)
            );
        }
    }
}