CREATE KEYSPACE if not EXISTS CarR 
  WITH replication = {'class': 'SimpleStrategy', 'replication_factor' : 3 };

cqlsh:demo> use carr ;

CREATE TABLE car ( 
  carID int, 
  manufacture text, 
  model text, 
  seats int, 
  baggage int, 
  location text, 
  class text, 
  primary key (carID) 
);

CREATE TABLE user ( 
  phone int, 
  first_name text, 
  last_name text, 
  birth_date int, 
  email text, 
  primary key (phone) 
);

CREATE TABLE carbycity (
  city text, 
  country text, 
  carid int, 
  cars int, 
  primary key (city, country, carid) 
);

CREATE TABLE reservation (
  resid int, 
  carid int, 
  phone int, 
  class text, 
  start timestamp, 
  end timestamp, 
  primary key (resid) 
);