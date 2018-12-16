using System;
using System.Collections.Generic;
using System.ComponentModel;
using Cassandra;

namespace cassandra_app.cassandra.Models
{  
  // On DB primary key is Phone
  public class TheaterScreening
  {
    public Guid Id { get; set; }
    public Guid Movie_Id { get; set; }
    public Guid Theater_Id { get; set; }

    public LocalDate Date { get; set; }
    public LocalTime Time { get; set; }

    public string Hall { get; set; }
    public int Hall_Capacity { get; set; }

    public TheaterScreening(Guid id, Guid movieId, Guid theaterId, LocalDate date, LocalTime time, string hall,
      int hallCapacity)
    {
      this.Id = id;
      this.Movie_Id = movieId;
      this.Theater_Id = theaterId;

      this.Date = date;
      this.Time = time;

      this.Hall = hall;
      this.Hall_Capacity = hallCapacity;
    }
  }
}