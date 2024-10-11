
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY,
    Username VARCHAR(50) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(20) NOT NULL
);

CREATE TABLE Flights (
    FlightID INT PRIMARY KEY IDENTITY,
    FlightNumber VARCHAR(10) NOT NULL,
    Departure VARCHAR(100) NOT NULL,
    Destination VARCHAR(100) NOT NULL,
    DepartureTime DATETIME NOT NULL,
    ArrivalTime DATETIME NOT NULL,
    TotalSeats INT NOT NULL,
    AvailableSeats INT NOT NULL
);

CREATE TABLE Passengers (
    PassengerID INT PRIMARY KEY IDENTITY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    Phone VARCHAR(15) NOT NULL,
    UserID INT,  -- Kết nối với bảng Users
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
);
EXEC sp_rename 'Passengers.Email', 'CCCD', 'COLUMN';

CREATE TABLE Tickets (
    TicketID INT PRIMARY KEY IDENTITY,
    FlightID INT NOT NULL,
    PassengerID INT NOT NULL,
    BookingDate DATETIME NOT NULL,
    SeatNumber VARCHAR(5) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (FlightID) REFERENCES Flights(FlightID),
    FOREIGN KEY (PassengerID) REFERENCES Passengers(PassengerID)
);
ALTER TABLE Tickets
ADD CONSTRAINT UC_FlightSeat UNIQUE (FlightID, SeatNumber);

-- Thêm dữ liệu vào bảng Users
INSERT INTO Users (Username, Password, Role) VALUES
('user1', 'password1', 'customer'),
('user2', 'password2', 'customer'),
('user3', 'password3', 'customer'),
('user4', 'password4', 'admin'),
('user5', 'password5', 'customer'),
('user6', 'password6', 'customer'),
('user7', 'password7', 'customer'),
('user8', 'password8', 'admin'),
('user9', 'password9', 'customer'),
('user10', 'password10', 'customer');

-- Thêm dữ liệu vào bảng Flights
INSERT INTO Flights (FlightNumber, Departure, Destination, DepartureTime, ArrivalTime, TotalSeats, AvailableSeats) VALUES
('FL001', 'Hanoi', 'Ho Chi Minh', '2024-10-20 08:00', '2024-10-20 10:00', 100, 80),
('FL002', 'Hanoi', 'Da Nang', '2024-10-21 09:00', '2024-10-21 10:30', 120, 100),
('FL003', 'Ho Chi Minh', 'Nha Trang', '2024-10-22 07:30', '2024-10-22 09:00', 150, 130),
('FL004', 'Haiphong', 'Hue', '2024-10-23 12:00', '2024-10-23 14:00', 80, 60),
('FL005', 'Da Nang', 'Can Tho', '2024-10-24 15:00', '2024-10-24 16:30', 90, 70),
('FL006', 'Hanoi', 'Phu Quoc', '2024-10-25 10:00', '2024-10-25 12:00', 100, 50),
('FL007', 'Ho Chi Minh', 'Hue', '2024-10-26 11:00', '2024-10-26 13:00', 120, 90),
('FL008', 'Da Nang', 'Hanoi', '2024-10-27 08:30', '2024-10-27 10:00', 110, 100),
('FL009', 'Nha Trang', 'Can Tho', '2024-10-28 09:00', '2024-10-28 11:00', 150, 130),
('FL010', 'Ho Chi Minh', 'Da Nang', '2024-10-29 14:00', '2024-10-29 15:30', 140, 120);

-- Thêm dữ liệu vào bảng Passengers
INSERT INTO Passengers (FirstName, LastName, Email, Phone, UserID) VALUES
('Nguyen', 'Van A', 'nguyenvana@example.com', '0123456789', 1),
('Tran', 'Thi B', 'tranthib@example.com', '0123456780', 2),
('Le', 'Van C', 'levanc@example.com', '0123456781', 3),
('Phan', 'Thi D', 'phantid@example.com', '0123456782', 4),
('Hoang', 'Van E', 'hoangvane@example.com', '0123456783', 5),
('Bui', 'Thi F', 'buithif@example.com', '0123456784', 6),
('Ngo', 'Van G', 'ngovan@example.com', '0123456785', 7),
('Vu', 'Thi H', 'vuthih@example.com', '0123456786', 8),
('Dang', 'Van I', 'dangvani@example.com', '0123456787', 9),
('Ngo', 'Van J', 'ngovanJ@example.com', '0123456788', 10);

-- Thêm dữ liệu vào bảng Tickets
INSERT INTO Tickets (FlightID, PassengerID, BookingDate, SeatNumber, Price) VALUES
(1, 1, '2024-10-01 12:00', '1A', 100.00),
(1, 2, '2024-10-02 12:00', '1B', 100.00),
(2, 3, '2024-10-03 12:00', '2A', 120.00),
(2, 4, '2024-10-04 12:00', '2B', 120.00),
(3, 5, '2024-10-05 12:00', '3A', 150.00),
(3, 6, '2024-10-06 12:00', '3B', 150.00),
(4, 7, '2024-10-07 12:00', '4A', 80.00),
(4, 8, '2024-10-08 12:00', '4B', 80.00),
(5, 9, '2024-10-09 12:00', '5A', 90.00),
(5, 10, '2024-10-10 12:00', '5B', 90.00);

ALTER TABLE Flights
ADD Airline VARCHAR(50) ;
UPDATE Flights SET Airline = 'Vietnam Airlines' WHERE FlightID = 1;
UPDATE Flights SET Airline = 'Bamboo Airways' WHERE FlightID = 2;
UPDATE Flights SET Airline = 'VietJet Air' WHERE FlightID = 3;
UPDATE Flights SET Airline = 'Vietnam Airlines' WHERE FlightID = 4;
UPDATE Flights SET Airline = 'Bamboo Airways' WHERE FlightID = 5;
UPDATE Flights SET Airline = 'VietJet Air' WHERE FlightID = 6;
UPDATE Flights SET Airline = 'Vietnam Airlines' WHERE FlightID = 7;
UPDATE Flights SET Airline = 'Bamboo Airways' WHERE FlightID = 8;
UPDATE Flights SET Airline = 'VietJet Air' WHERE FlightID = 9;
UPDATE Flights SET Airline = 'Vietnam Airlines' WHERE FlightID = 10;

