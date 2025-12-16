-- Sample SQL script to populate Flights table as backup for CSV importer
-- Assumes Flight table with columns: Id (int, auto), Airline (nvarchar), Origin (nvarchar), Destination (nvarchar), ScheduledDeparture (datetime), ActualDeparture (datetime), DelayMinutes (int)

INSERT INTO Flights (Airline, Origin, Destination, ScheduledDeparture, ActualDeparture, DelayMinutes) VALUES
('DL', 'Tampa International', 'John F. Kennedy International', '2013-09-16 15:39:00', '2013-09-16 15:43:00', 4),
('WN', 'Pittsburgh International', 'Chicago Midway International', '2013-09-23 07:10:00', '2013-09-23 07:13:00', 3),
('AS', 'Seattle/Tacoma International', 'Ronald Reagan Washington National', '2013-09-07 08:10:00', '2013-09-07 08:07:00', -3),
('OO', 'Chicago O''Hare International', 'Cleveland-Hopkins International', '2013-07-22 08:04:00', '2013-07-22 08:39:00', 35),
('DL', 'Norfolk International', 'Hartsfield-Jackson Atlanta International', '2013-05-16 05:45:00', '2013-05-16 05:44:00', -1);