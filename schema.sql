USE EventBooking;
GO

CREATE TABLE Events(
event_id INT IDENTITY(1,1) PRIMARY KEY,
name NVARCHAR(200) NOT NULL,
start_time DATETIME2 NOT NULL,
total_seats INT NOT NULL
)

CREATE TABLE Roles (
    role_id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL UNIQUE,
    name NVARCHAR(100) NOT NULL,
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE SeatStatuses (
    seat_status_id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL UNIQUE,
    name NVARCHAR(100) NOT NULL,
    is_active BIT NOT NULL DEFAULT 1
);

CREATE TABLE BookingStatuses (
    booking_status_id INT IDENTITY(1,1) PRIMARY KEY,
    code NVARCHAR(50) NOT NULL UNIQUE,
    name NVARCHAR(100) NOT NULL,
    is_active BIT NOT NULL DEFAULT 1
);

INSERT INTO Roles (code, name) VALUES ('CUSTOMER', 'Customer'), ('ADMIN', 'Admin');
INSERT INTO SeatStatuses (code, name) VALUES ('AVAILABLE', 'Available'), ('HELD', 'Held'), ('BOOKED', 'Booked');
INSERT INTO BookingStatuses (code, name) VALUES ('PENDING_PAYMENT', 'Pending Payment'), ('CONFIRMED', 'Confirmed'), ('CANCELLED', 'Cancelled'), ('EXPIRED', 'Expired');

CREATE TABLE Users(
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    name NVARCHAR(200) NOT NULL,
    email NVARCHAR (200) UNIQUE NOT NULL,
    password_hash NVARCHAR(200) NOT NULL,
    phone_number NVARCHAR(20) NULL,
    role_id INT NOT NULL,
    FOREIGN KEY (role_id) REFERENCES Roles(role_id),
    created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    updated_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)

CREATE TABLE Seats (
    seat_id INT IDENTITY(1,1) PRIMARY KEY,
    event_id INT NOT NULL,
    row_label NVARCHAR(10) NOT NULL,
    seat_number INT NOT NULL,
    seat_status_id INT NOT NULL,
    FOREIGN KEY (event_id) REFERENCES Events(event_id),
    FOREIGN KEY (seat_status_id) REFERENCES SeatStatuses(seat_status_id),
    UNIQUE (event_id, row_label, seat_number)
);


CREATE TABLE Bookings(
    booking_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    seat_id INT NOT NULL,
    FOREIGN KEY (seat_id) REFERENCES Seats(seat_id),
    booking_status_id INT NOT NULL,
    FOREIGN KEY (booking_status_id) REFERENCES BookingStatuses(booking_status_id),
    created_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    updated_at DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
)

CREATE INDEX IX_Bookings_seat_id ON Bookings (seat_id);
REATE INDEX IX_Bookings_user_id ON Bookings (user_id);