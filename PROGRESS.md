# PROGRESS.md

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Session 1: SQL Server 2025 running in Docker (container sql1), connected from SSMS, created EventBooking database, created Events table (IDENTITY primary key). Table confirmed in SSMS.

## Current

- Week 1 (Mon 12 Oct): SQL basics + booking schema + SQL Server in Docker

## Next

- Week 1, Session 2: design the rest of the schema (Seats, Users, Bookings) on paper first, then write the CREATE TABLE statements yourself
- Save your CREATE statements in a schema.sql file in the repo (no passwords in it), commit and push
- SQLBolt lessons 1-6
- Optional: save the SA password outside the repo

## Project state

- Repo: event-booking-api, pushed, only PROGRESS.md so far
- Tools: .NET 9, Docker Desktop, SSMS; SQL Server running in Docker as sql1 (port 1433, user sa)
- Run it: open Docker Desktop, start container sql1 if stopped, connect SSMS to localhost,1433 (Trust server certificate)
- Main tables: Events (event_id IDENTITY PK, name NVARCHAR(200), start_time DATETIME2, total_seats INT). Seats, Users, Bookings not designed yet

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core
- snake_case column names
- IDENTITY(1,1) primary keys (not GUID)
- NVARCHAR for text (supports non-English characters)
- SA password stored outside the repo, never committed

## Stuck on

- (nothing yet)

## Weak spots

- SQL Server vs MySQL syntax (AUTO_INCREMENT vs IDENTITY)
- PowerShell vs Linux command syntax (no backslash line breaks)

## Interview questions I couldn't answer

- What is Docker, and how is a container different from a VM? (to answer)
- What does -p 1433:1433 mean? (answered: host port : container port, traffic on the PC's port is forwarded into the container)
- What is a primary key, and what does IDENTITY do? (partly answered: IDENTITY gives unique auto numbers; need to explain primary key itself)
- IDENTITY vs GUID as primary key (partly answered: GUID is 128-bit; trade-offs: IDENTITY small, ordered, fast, guessable; GUID big, hard to guess, random GUIDs fragment the index)
- IDENTITY in SQL Server vs AUTO_INCREMENT in MySQL (answered: same idea, different syntax)
- What is T-SQL? (answered: Microsoft's SQL dialect with variables, IF/WHILE, TRY...CATCH, TOP, stored procedures)
