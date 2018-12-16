CREATE KEYSPACE if NOT EXISTS cineplex
  WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 3 };

USE cineplex;

DROP TABLE IF EXISTS cineplex.theater;
DROP TABLE IF EXISTS cineplex.movie;
DROP TABLE IF EXISTS cineplex.screening;
DROP TABLE IF EXISTS cineplex.theater_screening;
DROP TABLE IF EXISTS cineplex.ticket;
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

CREATE INDEX ON screening (id);

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

INSERT INTO theater (id, name) VALUES (cf340926-d925-4820-abed-49db9f07d7ac, 'Apollo');
INSERT INTO theater (id, name) VALUES (e150983c-b83b-4569-a0e1-8c2a74f1b6ef, 'Rialto');
INSERT INTO theater (id, name) VALUES (910d0199-3d1f-4d01-8ccd-6e6ba127b32e, 'Muza');
INSERT INTO theater (id, name) VALUES (3af63abb-4140-4480-b0b8-c9339f1f8ab6, 'NH');
INSERT INTO theater (id, name) VALUES (105653f8-5802-45d3-8dd9-892fc79662b3, 'Palacowe');


INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (03ff10fc-6e00-4ddf-a768-199e8a316ce1, 'Jaws', 1975, 'Movie about sharks', 124);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (8be2acac-541b-4f1e-8d52-cb0cfb6b3c4b, 'Kursk', 2018, 'Russian submarine', 117);

INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (4134c8a0-6d7b-4526-af09-de63f078d200, 'Godfather', 1972, 'Mafia classic', 175);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (4b304bed-dfc9-4e4f-8e9f-12279b5f24b4, 'Blade Runner', 1982, 'Blade Runner', 118);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (9858655d-a03f-479b-b2ba-02035f88a076, 'La Grande Bellezza', 2013, 'Author life', 144);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (f82a26c2-8c00-457e-be79-7801de327bd4, 'Drive', 2011, 'He just drives', 100);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (c533ed41-5e04-4fcc-b516-657fe7339359, 'Baby Driver', 2017, 'Was he slow?', 113);

INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (82207f8b-21ba-472b-ad40-dfa447cdbfb3, 'Fantastic Mr. Fox', 2009, 'Movie about sharks', 87);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (61304204-6d50-4886-81f0-1b536341aa84, 'Senna', 2010, 'Best driver ever', 104);
  
INSERT INTO movie (id, title, prodyear, description, duration)
  VALUES  (1f4b7f51-e909-4954-82cd-b5a33b4d0e88, 'Blues Brothers', 1980, 'Blues Brothers', 133);


INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (9e1b8d2b-85f3-44fb-acda-bfdd64b84532, 03ff10fc-6e00-4ddf-a768-199e8a316ce1, cf340926-d925-4820-abed-49db9f07d7ac, '2018-12-20', '20:00:00', 'A', 200);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (d2b2f48d-febb-4997-91fc-9cc752193210, 03ff10fc-6e00-4ddf-a768-199e8a316ce1, cf340926-d925-4820-abed-49db9f07d7ac, '2018-12-21', '20:00:00', 'A', 200);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (9e69e236-da80-472d-980a-323608a118c6, 4134c8a0-6d7b-4526-af09-de63f078d200, cf340926-d925-4820-abed-49db9f07d7ac, '2018-12-21', '20:00:00', 'B', 1000);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (c5ca95db-cd92-4e80-845a-8dfd3686dfe1, 4134c8a0-6d7b-4526-af09-de63f078d200, cf340926-d925-4820-abed-49db9f07d7ac, '2018-12-20', '20:00:00', 'B', 1000);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (9011f141-565d-48bf-a20b-30c79cf979ef, 9858655d-a03f-479b-b2ba-02035f88a076, e150983c-b83b-4569-a0e1-8c2a74f1b6ef, '2018-12-20', '18:00:00', 'A', 3000);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (6900c782-3400-4c46-bff0-5b49f0c118bd, 9858655d-a03f-479b-b2ba-02035f88a076, e150983c-b83b-4569-a0e1-8c2a74f1b6ef, '2018-12-21', '18:00:00', 'A', 3000);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (77b9357b-5151-4dd3-b871-e4afa0c83b06, f82a26c2-8c00-457e-be79-7801de327bd4, e150983c-b83b-4569-a0e1-8c2a74f1b6ef, '2018-12-20', '21:00:00', 'A', 3000);
   
INSERT INTO screening (id, movie_id, theater_id, date, time, hall, hall_capacity) 
  VALUES (019621dd-5a1d-40fd-ad03-652e65fd5b8f, f82a26c2-8c00-457e-be79-7801de327bd4, e150983c-b83b-4569-a0e1-8c2a74f1b6ef, '2018-12-21', '20:00:00', 'B', 300);

   