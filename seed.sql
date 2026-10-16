USE EventBooking;
GO

-- 1. Clear sample data (children first). Lookup tables are kept.
DELETE FROM Bookings;
DELETE FROM Seats;
DELETE FROM Users;
DELETE FROM Events;

-- 2. Make sure lookup values exist
IF NOT EXISTS (SELECT 1 FROM Roles WHERE code = 'CUSTOMER') INSERT INTO Roles (code, name) VALUES ('CUSTOMER', 'Customer');
IF NOT EXISTS (SELECT 1 FROM Roles WHERE code = 'ADMIN') INSERT INTO Roles (code, name) VALUES ('ADMIN', 'Admin');
IF NOT EXISTS (SELECT 1 FROM SeatStatuses WHERE code = 'AVAILABLE') INSERT INTO SeatStatuses (code, name) VALUES ('AVAILABLE', 'Available');
IF NOT EXISTS (SELECT 1 FROM SeatStatuses WHERE code = 'HELD') INSERT INTO SeatStatuses (code, name) VALUES ('HELD', 'Held');
IF NOT EXISTS (SELECT 1 FROM SeatStatuses WHERE code = 'BOOKED') INSERT INTO SeatStatuses (code, name) VALUES ('BOOKED', 'Booked');
IF NOT EXISTS (SELECT 1 FROM BookingStatuses WHERE code = 'PENDING_PAYMENT') INSERT INTO BookingStatuses (code, name) VALUES ('PENDING_PAYMENT', 'Pending Payment');
IF NOT EXISTS (SELECT 1 FROM BookingStatuses WHERE code = 'CONFIRMED') INSERT INTO BookingStatuses (code, name) VALUES ('CONFIRMED', 'Confirmed');
IF NOT EXISTS (SELECT 1 FROM BookingStatuses WHERE code = 'CANCELLED') INSERT INTO BookingStatuses (code, name) VALUES ('CANCELLED', 'Cancelled');
IF NOT EXISTS (SELECT 1 FROM BookingStatuses WHERE code = 'EXPIRED') INSERT INTO BookingStatuses (code, name) VALUES ('EXPIRED', 'Expired');

-- 3. Events
INSERT INTO Events (name, start_time, total_seats) VALUES ('COMIC FIESTA', '2026-12-19 09:00:00', 200);
INSERT INTO Events (name, start_time, total_seats) VALUES ('GAMING NIGHT', '2026-11-20 21:00:00', 20);

-- 4. Look up IDs by name/code instead of hard-coding
DECLARE @event_id INT = (SELECT event_id FROM Events WHERE name = 'COMIC FIESTA');
DECLARE @role_id INT = (SELECT role_id FROM Roles WHERE code = 'CUSTOMER');
DECLARE @available INT = (SELECT seat_status_id FROM SeatStatuses WHERE code = 'AVAILABLE');
DECLARE @booked INT = (SELECT seat_status_id FROM SeatStatuses WHERE code = 'BOOKED');
DECLARE @confirmed INT = (SELECT booking_status_id FROM BookingStatuses WHERE code = 'CONFIRMED');

-- 5. User
INSERT INTO Users (name, email, password_hash, phone_number, role_id)
VALUES ('Zorus', 'zorus@gmail.com', 'dummy-hash-not-real', '0123456789', @role_id);

-- 6. Five seats for COMIC FIESTA (Z10 is already booked)
INSERT INTO Seats (event_id, row_label, seat_number, seat_status_id) VALUES
(@event_id, 'Z', 10, @booked),
(@event_id, 'Z', 11, @available),
(@event_id, 'Z', 12, @available),
(@event_id, 'Z', 13, @available),
(@event_id, 'Z', 14, @available);

-- 7. One booking for seat Z10
DECLARE @user_id INT = (SELECT user_id FROM Users WHERE email = 'zorus@gmail.com');
DECLARE @seat_id INT = (SELECT seat_id FROM Seats WHERE event_id = @event_id AND row_label = 'Z' AND seat_number = 10);
INSERT INTO Bookings (user_id, seat_id, booking_status_id) VALUES (@user_id, @seat_id, @confirmed);

-- 8. Check results
SELECT * FROM Events;
SELECT * FROM Seats;
SELECT * FROM Bookings;