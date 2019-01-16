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
        // private Models.Reservation reservationModel;
        // private Models.Event eventModel;
        // private Models.Tickets_Counter counterModel;

        private Controllers.Tickets_Counter counterController;
        private Controllers.Reservation reservationController;
        private Controllers.Event eventController;
    
        // zrobić parę operacji w wątkach rezerwacja i natychmiast odczyt i anulowanie



        private int clientsCount = 200;
        private static int clientIdOffset = 100;

        public Runner()
        {
            backend = new BackendSession(false);

            counterController = new Controllers.Tickets_Counter(backend);
            reservationController = new Controllers.Reservation(backend);
            eventController = new Controllers.Event(backend);

            CancellationTokenSource source = new CancellationTokenSource();
            List<Models.Event> events = eventController.GetAllEvents();
            Models.Event ev = events.Where(e => e.Id == 2).First();

            Random rand = new Random();

            Console.WriteLine("Counter value: {0}", counterController.GetRemainingTicketsValue(2));

            CancellationToken token = source.Token;
            
            // GetStatistics(200, token);

            for (int k=0; k<100; k++)
            {
                List<Thread> threads = new List<Thread>();
                for (int i = 0; i < 1000; i++)
                {
                    Thread t = new Thread(() => {
                        Client c = new Client(reservationController, clientIdOffset, 2, 20, 10, 1);
                        c.run();
                    
                    });
                    t.Start();
                    threads.Add(t);

                }
                foreach (Thread th in threads)
                {
                    th.Join();
                }
                GetStatistics(1, token);
            }

            
            
            

            // List<Task> tasks = new List<Task>();
            // for (int i = 0; i < 40; i++)
            // {
            //     Task task = Task.Factory.StartNew(() => {
            //         clientIdOffset += 1;
                    
            //             Client c = new Client(reservationController, clientIdOffset, 2, 4000, 1500, rand.Next(9)+1);
            //             c.run();
            //             // await Task.Delay(50);
                    
                    
                    
            //     });
            //     tasks.Add(task);
            // }
            
            // Task.WaitAll(tasks.ToArray());
            source.Cancel();

            Console.WriteLine("[END] Counter value: {0}", counterController.GetRemainingTicketsValue(2));
            Console.WriteLine("[END] Statistics: reserved: {0}\n" +
                    "[END] Statistics: placed: {1}\n" +
                    "[END] Statistics: cancelled: {2}\n" +
                    "[END] Statistics: declined: {3}", 
                    Statistics.GetReserved(), Statistics.GetPlaced(), 
                    Statistics.GetCancelled(), Statistics.GetDeclined());
            Console.WriteLine("[END] clients created {0}", Statistics.GetClientCount());
            


            // Models.Event ev = events.Where(e => e.Id == 3).First();

            // Console.WriteLine(ev.Total_Tickets);
            // Stopwatch sW = Stopwatch.StartNew();

            // List<Task> tasks = new List<Task>();
            // for (int j=0; j<5; j++)
            // {
            //     Task task = Task.Factory.StartNew(() => run(36800));
            //     tasks.Add(task);

            // }
            // // Task task = Task.Factory.StartNew(() => )

            // Task.WaitAll(tasks.ToArray());
            // sW.Stop();
            // Console.WriteLine(sW.ElapsedMilliseconds);

            // long? value = c;
            // Console.WriteLine(value);
            
            // for (int i=0; i<ev.Total_Tickets; i++)
            // {
            //     Reservation res = new Reservation(Guid.NewGuid(), ev.Id, 10, 1, false, DateTimeOffset.Now);
            //     reservationController.PlaceReservation(res);
            // }
        
        }

        Models.Event GetRandomEvent(List<Models.Event> events)
        {
            Random random = new Random();
            // Event randomEvent;
            int r = random.Next(0, events.Count);
            return events[r];
        }

        public void run(int count)
        {
            for (int i=0; i<count; i++)
            {
                Reservation res = new Reservation(Guid.NewGuid(), 3, 10, 1, false, DateTimeOffset.Now);
                reservationController.PlaceReservation(res);
            }
        }

        private void GetStatistics(int interval, CancellationToken cancellation)
        {
            // while (true)
            // {
                // await Task.Delay(interval, cancellation);
                Console.WriteLine("Counter value: {0}", counterController.GetRemainingTicketsValue(2));
                Console.WriteLine("Statistics: reserved: {0}\n" +
                    "Statistics: placed: {1}\n" +
                    "Statistics: cancelled: {2}\n" +
                    "Statistics: declined: {3}", 
                    Statistics.GetReserved(), Statistics.GetPlaced(), 
                    Statistics.GetCancelled(), Statistics.GetDeclined());
                
                
            // }


        }

    }
}