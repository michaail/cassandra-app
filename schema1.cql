CREATE KEYSPACE if NOT EXISTS cineplex
  WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 3 };

USE cineplex;

DROP TABLE IF EXISTS cineplex.theaters;
DROP TABLE IF EXISTS cineplex.movies;
DROP TABLE IF EXISTS cineplex.screenings;
DROP TABLE IF EXISTS cineplex.theater_screenings;
DROP TABLE IF EXISTS cineplex.tickets;
DROP TABLE IF EXISTS cineplex.tickets_counter;

CREATE TABLE IF NOT EXISTS theater (
  id uuid,
  name text,
  PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS movie (
  id uuid,
  title text,
  prodyear int,
  description text,
  duration int,
  PRIMARY KEY ((id), title)
);

CREATE TABLE IF NOT EXISTS screening (
  id uuid,
  movie_id uuid,
  theater_id uuid,
  date date,
  time time,
  hall text,
  hall_capacity int,
  PRIMARY KEY ((movie_id), date, time, id)
);

CREATE TABLE IF NOT EXISTS theater_screening (
  id uuid,
  movie_id uuid,
  theater_id uuid,
  date date,
  time time,
  hall text,
  hall_capacity int,
  PRIMARY KEY ((movie_id, theater_id), date, time, id)
);

CREATE INDEX ON screenings (id);

CREATE TABLE IF NOT EXISTS ticket (
  id timeuuid,
  screening_id uuid,
  user text,
  timestamp timestamp,
  PRIMARY KEY ((screening_id), id, user)
);

CREATE TABLE IF NOT EXISTS tickets_counter (
  movie_id uuid,
  screening_id uuid,
  sold counter,
  PRIMARY KEY ((movie_id), screening_id)
);