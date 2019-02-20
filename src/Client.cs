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
    public class Client
    {
        private int id;
        private int ticketsCount;
        private int eventId;
        private int beforePlaceDelay;
        private int checkDelay;
        private int range = 100;
        private int eventCapacity;

        private Controllers.Reservation reservationController;

        public Client(Controllers.Reservation reservation, int id, int eventId, int delay, int checkDelay, int ticketsCount, int eventCapacity)
        {
            this.id = id;
            this.eventId = eventId;
            this.beforePlaceDelay = delay;
            this.checkDelay = checkDelay;
            this.ticketsCount = ticketsCount;
            this.reservationController = reservation;
            this.eventCapacity = eventCapacity;
        }

        public void run()
        {
            Statistics.ClientCreated();
            Random rand = new Random();

            // random wait to proceed
            Thread.Sleep(beforePlaceDelay + rand.Next(range));
            Guid reservationId = Guid.NewGuid();

            bool result = reservationController.PlaceReservation(new Models.Reservation(reservationId, eventId, id, ticketsCount, false, DateTimeOffset.Now), eventCapacity);
            if (result == true)
            {
                Thread.Sleep(checkDelay);
                if(rand.Next(10) == 1)
                {
                    reservationController.CancellReservation(reservationId, eventId);
                    // Statistics.Cancelled(eventId, ticketsCount);
                }
                else // test purposer only
                {
                    if (reservationController.GetReservation(reservationId, eventId) != null)
                    {
                        Statistics.Placed(eventId, ticketsCount);
                    }
                    else
                    {
                        Statistics.NotFound(eventId);
                    }
                }
            }
            else
            {
                Statistics.Declined(eventId, ticketsCount);
            }
        }
    }
}