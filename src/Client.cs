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

        private Controllers.Reservation reservationController;

        public Client(Controllers.Reservation reservation, int id, int eventId, int delay, int checkDelay, int ticketsCount)
        {
            this.id = id;
            this.eventId = eventId;
            this.beforePlaceDelay = delay;
            this.checkDelay = checkDelay;
            this.ticketsCount = ticketsCount;

            this.reservationController = reservation;
        }

        public void run()
        {
            Statistics.ClientCreated();
            Random rand = new Random();
            // random wait to proceed
            Thread.Sleep(beforePlaceDelay + rand.Next(range));
            Guid reservationId = Guid.NewGuid();
            // Statistics.re(ticketsCount);
            bool result = reservationController.PlaceReservation(new Models.Reservation(reservationId, eventId, id, ticketsCount, false, DateTimeOffset.Now));
            if (result == true)
            {
                Statistics.Placed(ticketsCount);
                Thread.Sleep(checkDelay);

                if(rand.Next(5) == 1)
                {
                    reservationController.CancellReservation(reservationId, eventId);
                    Statistics.Cancelled(ticketsCount);
                }
            }
            else
            {
                Statistics.Declined(ticketsCount);
            }
        }
    }
}