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
    

        public Runner()
        {
            backend = new BackendSession(true);

            counterController = new Controllers.Tickets_Counter(backend);
            reservationController = new Controllers.Reservation(backend);
            eventController = new Controllers.Event(backend);

            List<Event> events = eventController.GetAllEvents();

            Event ev = events.Where(e => e.Id == 3).First();

            Console.WriteLine(ev.Total_Tickets);
            Stopwatch sW = Stopwatch.StartNew();

            List<Task> tasks = new List<Task>();
            for (int j=0; j<5; j++)
            {
                Task task = Task.Factory.StartNew(() => run(36800));
                tasks.Add(task);

            }
            Task.WaitAll(tasks.ToArray());
            sW.Stop();
            Console.WriteLine(sW.ElapsedMilliseconds);

            long? value = counterController.GetRemainingTicketsValue(ev.Id);
            Console.WriteLine(value);
            
            // for (int i=0; i<ev.Total_Tickets; i++)
            // {
            //     Reservation res = new Reservation(Guid.NewGuid(), ev.Id, 10, 1, false, DateTimeOffset.Now);
            //     reservationController.PlaceReservation(res);
            // }
        
        }

        Event GetRandomEvent(List<Models.Event> events)
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

    }
}