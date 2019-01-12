using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            backend = new BackendSession(false);

            counterController = new Controllers.Tickets_Counter(backend);
            reservationController = new Controllers.Reservation(backend);
            eventController = new Controllers.Event(backend);

            List<Event> events = eventController.GetAllEvents();

            Event ev = events.Where(e => e.Id == 2).First();

            Console.WriteLine(ev.Total_Tickets);
            for (int i=0; i<ev.Total_Tickets; i++)
            {
                Reservation res = new Reservation(Guid.NewGuid(), ev.Id, 10, 1, false, DateTimeOffset.Now);
                reservationController.PlaceReservation(res);
            }

        }

        Event GetRandomEvent(List<Models.Event> events)
        {
            Random random = new Random();
            // Event randomEvent;
            int r = random.Next(0, events.Count);
            return events[r];
        }

    }
}