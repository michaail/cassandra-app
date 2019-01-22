CREATE KEYSPACE if NOT EXISTS ticketer
  WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 3 };

USE ticketer;

DROP TABLE IF EXISTS ticketer.event;
DROP TABLE IF EXISTS ticketer.reservation;
DROP TABLE IF EXISTS ticketer.tickets_counter;

CREATE TABLE IF NOT EXISTS event (
  id int,
  name text,
  location text,
  total_tickets int,
  sold_out boolean,
  PRIMARY KEY (name, id)
);

CREATE TABLE IF NOT EXISTS reservation (
  id uuid,
  event_id int,
  user int,
  tickets_count int,
  cancelled boolean,
  timestamp timestamp,
  PRIMARY KEY (event_id, id)
);

CREATE TABLE IF NOT EXISTS tickets_counter (
  event_id int,
  remaining counter,
  PRIMARY KEY (event_id)
);


INSERT INTO event (id, name, location, total_tickets, sold_out) 
  VALUES (1, 'Phil Collins', 'PGE Narodowy', 10000, false);

INSERT INTO event (id, name, location, total_tickets, sold_out) 
  VALUES (2, 'Nick Cave', 'Auckland Town Hall', 10000, false);

INSERT INTO event (id, name, location, total_tickets, sold_out) 
  VALUES (3, 'Paul McCartney', 'Maracana Stadium', 10000, false);

INSERT INTO event (id, name, location, total_tickets, sold_out) 
  VALUES (4, 'The Rolling Stones', 'O2 Arena', 10000, false);

INSERT INTO event (id, name, location, total_tickets, sold_out) 
  VALUES (5, 'Live Aid', 'Wembley Stadium', 10000, false);


UPDATE tickets_counter SET remaining = remaining + 0 WHERE event_id = 1;
UPDATE tickets_counter SET remaining = remaining + 0 WHERE event_id = 2;
UPDATE tickets_counter SET remaining = remaining + 0 WHERE event_id = 3;
UPDATE tickets_counter SET remaining = remaining + 0 WHERE event_id = 4;
UPDATE tickets_counter SET remaining = remaining + 0 WHERE event_id = 5;
