using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cassandra;
using Cassandra.Mapping;
using cassandra_app.src;

namespace cassandra_app.src.Controllers
{
    public class Reservation
    {
        private IMapper mapper;
        private CqlQueryOptions queryOptions;
        private Tickets_Counter tickets_Counter;
        public Reservation(BackendSession backend)
        {
            this.mapper = backend.GetMapper();
            
            this.tickets_Counter = new Tickets_Counter(backend);
        }

        public bool PlaceReservation(Models.Reservation reservation, int eventCapacity)
        {
            long? remainingTickets = tickets_Counter.GetCurrentTicketsCount(reservation.Event_Id);
            if (remainingTickets != null)   // counter exists
            {
                // are tickets still available
                if (eventCapacity - remainingTickets >= reservation.Tickets_count)
                {
                    mapper.Insert(reservation);
                    // get tickets from pool
                    tickets_Counter.DecrementRemainingTicketsCountBy(reservation.Event_Id, 
                        reservation.Tickets_count);
                    Statistics.Placed(reservation.Event_Id, reservation.Tickets_count);
                }
                else
                {
                    // Console.WriteLine("Tickets count exceeded");
                    Statistics.Declined(reservation.Event_Id, reservation.Tickets_count);
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Couldn't find ticketsCounter");
                return false;
            }
            return true;
        }

        public void CancellReservation(Guid reservationId, int eventId)
        {
            Models.Reservation reservation = GetReservation(reservationId, eventId);
            if (reservation != null)    // reservation exists
            {
                reservation.Cancelled = true;
                try
                {
                    mapper.Update(reservation);
                    // return tickets to pool
                    tickets_Counter.IncrementRemainingTicketsCountBy(reservation.Event_Id,
                        reservation.Tickets_count);
                    Statistics.Cancelled(reservation.Event_Id, reservation.Tickets_count);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("No such reservation found");
                Statistics.NotFound(eventId);
            }
        }

        public List<Models.Reservation> GetAllReservations()
        {
            IEnumerable<Models.Reservation> reservations;
            try
            {
                reservations = mapper.Fetch<Models.Reservation>();
            }
            catch (Exception e)
            {
                reservations = null;
                Console.WriteLine(e.Message);
                return null;
            }

            return reservations.ToList();
        }

        public Models.Reservation GetReservation(Guid id, int eventId)
        {
            Models.Reservation reservation;
            try
            {
                reservation = mapper.Fetch<Models.Reservation>("WHERE id = ? AND event_id = ?", id, eventId).First();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            return reservation;
        }


    }
}