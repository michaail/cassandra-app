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

        public void PlaceReservation(Models.Reservation reservation)
        {
            long? remainingTickets = tickets_Counter.GetRemainingTicketsValue(reservation.Event_Id);
            if (remainingTickets != null)   // counter exists
            {
                if (remainingTickets >= reservation.Tickets_count)
                {
                    mapper.InsertAsync(reservation);
                    // get tickets from pool
                    tickets_Counter.DecrementRemainingTicketsCountBy(reservation.Event_Id, 
                        reservation.Tickets_count);
                }
                else
                {
                    Console.WriteLine("Tickets count exceeded");
                }
            }
            else
            {
                Console.WriteLine("Couldn't find ticketsCounter");
            }
        }

        public void CancellReservation(Guid reservationId)
        {
            Models.Reservation reservation = GetReservation(reservationId);
            if (reservation != null)    // reservation exists
            {
                reservation.Cancelled = true;
                mapper.Update(reservation);
                // return tickets to pool
                tickets_Counter.IncrementRemainingTicketsCountBy(reservation.Event_Id,
                    reservation.Tickets_count);
            }
            else
            {
                Console.WriteLine("No such reservation found");
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

        public Models.Reservation GetReservation(Guid id)
        {
            Models.Reservation reservation;
            try
            {
                reservation = mapper.Fetch<Models.Reservation>("WHERE id = ?", id).First();
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