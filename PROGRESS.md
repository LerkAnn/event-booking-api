# PROGRESS.md

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Session 1: SQL Server 2025 running in Docker (container sql1), connected from SSMS, created EventBooking database, created Events table (IDENTITY primary key).
- Week 1, Session 2: designed schema; created Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings (all 7 tables confirmed in SSMS). Lookup tables seeded with inserts.

## Current

- Week 1 (Mon 12 Oct): SQL basics + booking schema + SQL Server in Docker

## Next

- Week 1, Session 3: save all CREATE TABLE + lookup INSERTs in schema.sql in the repo (no passwords), commit and push (if not done yet)
- Insert sample data (a few events, users, seats, a booking) and write basic SELECT queries on them
- SQLBolt lessons 1-6
- Week 1 weekend: blank-file challenge (write the Events + Seats tables from memory)
- Optional: save the SA password outside the repo

## Project state

- Repo: event-booking-api, pushed, only PROGRESS.md (schema.sql pending)
- Tools: .NET 9, Docker Desktop, SSMS; SQL Server running in Docker as sql1 (port 1433, user sa)
- Run it: open Docker Desktop, start container sql1 if stopped, connect SSMS to localhost,1433 (Trust server certificate)
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings (all created, FKs in place)
- Seeded: Roles (CUSTOMER, ADMIN), SeatStatuses (AVAILABLE, HELD, BOOKED), BookingStatuses (PENDING_PAYMENT, CONFIRMED, CANCELLED, EXPIRED)

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core
- snake_case column names, plural table names
- IDENTITY(1,1) primary keys (not GUID)
- NVARCHAR for text; phone_number is NVARCHAR(20) nullable
- Lookup tables (Roles, SeatStatuses, BookingStatuses) with code, name, is_active
- Passwords: hashed in .NET (BCrypt, Week 4), stored in password_hash
- created_at/updated_at default to SYSUTCDATETIME()
- Bookings has no event_id (seat already knows its event)
- Seats unique on (event_id, row_label, seat_number)
- SA password stored outside the repo, never committed

## Stuck on

- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5

## Weak spots

- SQL Server vs MySQL syntax (AUTO_INCREMENT vs IDENTITY)
- PowerShell vs Linux command syntax (no backslash line breaks)
- Column vs table-level constraint syntax (FOREIGN KEY needs the column declared first)
- Missing commas in CREATE TABLE

## Interview questions I couldn't answer

- What is Docker, and how is a container different from a VM? (to answer)
- What does -p 1433:1433 mean? (answered: host port : container port)
- What is a primary key, and what does IDENTITY do? (partly answered: IDENTITY gives unique auto numbers; need to explain primary key itself: uniquely identifies each row, no nulls)
- IDENTITY vs GUID as primary key (partly answered; IDENTITY small, ordered, fast, guessable; GUID big, hard to guess, random GUIDs fragment the index)
- IDENTITY vs AUTO_INCREMENT (answered: same idea, different syntax)
- What is T-SQL? (answered: Microsoft's SQL dialect with variables, IF/WHILE, TRY...CATCH, TOP, stored procedures)
- What is a foreign key and why use one? (answered; add: default blocks deleting a parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered: one-way hash, salt is random value mixed in so identical passwords give different hashes)
- Lookup table vs CHECK constraint for statuses? (to answer: lookup table = FK-enforced, add values with INSERT, extra join; CHECK = simpler, needs a schema change to add values)
