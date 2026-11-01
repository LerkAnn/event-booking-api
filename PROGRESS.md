# PROGRESS.md

## Plan (10 weeks, Week 1 = 12 Oct)

- W1: SQL basics + booking schema + SQL Server in Docker
- W2: Joins, GROUP BY, indexes, transactions, constraints + .NET project + EF Core
- W3: CRUD APIs, validation, error handling, Swagger, push to GitHub
- W4: JWT auth + roles + basic tests
- W5: Booking logic, double-booking prevention, concurrency test
- W6: Seat hold + background job + fake payment endpoint
- W7: Redis caching, full docker compose up, logging
- W8: System design basics + architecture diagram
- W9: README, cleanup, resume, LinkedIn, start applying (~7 Dec)
- W10: Mock interviews, behavioral stories, more applications
- Never cut: booking logic, auth, concurrency test, Docker Compose, system design basics
- Cut first if behind: CI, deploy, Hangfire, LeetCode
- If early: add a stretch task inside the current week, don't jump ahead
- Resign only after the bonus is paid in Feb (2-month notice)
- Sessions: 3x40 min weekdays + 1x90 min weekend; 15 min minimum on bad days
- Calendar note: Week 3 started 26 Oct. Week 2 leftovers folded into Week 3.
- Full-day study sessions are planned with breaks (not cramming)

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Session 1: SQL Server 2025 in Docker (container sql1), connected from SSMS, created EventBooking database, Events table.
- Week 1, Session 2: designed schema; created all 7 tables with foreign keys.
- Week 1, Session 3: schema.sql and seed.sql in repo, sample data, first JOIN.
- Week 1, Session 4: wrote a 5-table JOIN myself. Quiz: primary key, PK vs UNIQUE, composite key.
- Week 2, Session 1: blank-file challenge, GROUP BY + HAVING.
- Week 2, Session 2: indexes (scan vs seek, B-tree idea); created IX_Bookings_seat_id and IX_Bookings_user_id.
- Week 2, Session 3: 5,000-row load test, seek vs scan, BEGIN TRAN / COMMIT / ROLLBACK, ACID. Cleaned up.
- Week 3, Session 1: created .NET project EventBooking.Api, ran it, opened OpenAPI JSON. Learned why an API sits between client and database.
- Week 3, Session 2: added EF Core packages (SqlServer + Design, version 9), connection string in user-secrets, wrote Event entity (Models/Event.cs), AppDbContext (Data/AppDbContext.cs), registered it in Program.cs, wrote GET /events and got my 2 events back as JSON. Committed.

## Current

- Week 3: CRUD APIs, validation, error handling, Swagger. GET /events works. Still to do: GET /events/{id}, POST, PUT, DELETE, validation, error handling, Swagger page.

## Next

- Week 3, Session 3: start with the 2 carry-over items (teach me again, whole code + brief explanation, then I run them):
    1. TRY...CATCH transaction: book seat Z11 for Zorus (then break test with user 999 on seat Z12)
    2. CHECK constraint: ALTER TABLE Events ADD CONSTRAINT CK_Events_total_seats CHECK (total_seats > 0); then test INSERT with total_seats = 0 and read the error; add the ALTER line to schema.sql
- Then the normal session task: GET /events/{id} and POST /events (LINQ, 404 when not found, 201 Created)
- Fix typo in schema.sql: "REATE INDEX" must be "CREATE INDEX"
- Blank-file challenge (weekend): write the TRY...CATCH template from memory
- Leftover (cut first if tired): SQLBolt lessons 1-6
- Optional: confirm SA password is saved outside the repo

## Project state

- Repo: event-booking-api, contains PROGRESS.md, schema.sql, seed.sql, EventBooking.Api (Models/Event.cs, Data/AppDbContext.cs, Program.cs with GET /events)
- Tools: .NET 9, Docker Desktop, SSMS; SQL Server 2025 in Docker as sql1 (port 1433, user sa)
- Run it: start Docker Desktop + sql1, then dotnet run --project EventBooking.Api, open printed URL + /events. Ctrl+C to stop
- Connection string is in user-secrets (ConnectionStrings:Default), not in the repo
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id
- CHECK constraint CK_Events_total_seats: NOT in the database yet (query returned empty)
- Seeded lookups: Roles (CUSTOMER, ADMIN), SeatStatuses (AVAILABLE, HELD, BOOKED), BookingStatuses (PENDING_PAYMENT, CONFIRMED, CANCELLED, EXPIRED)
- Sample data: seed.sql (deletes Bookings, Seats, Users, Events, then reinserts; never run on real data). Z11 booking status unknown, check before relying on it
- Risk: sql1 has no volume, so deleting the container loses the tables; rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7)

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case column names, plural table names; C# uses PascalCase with [Table] and [Column] attributes
- IDENTITY(1,1) primary keys (not GUID)
- NVARCHAR for text; phone_number NVARCHAR(20) nullable
- Lookup tables (Roles, SeatStatuses, BookingStatuses) with code, name, is_active
- Passwords hashed in .NET (BCrypt, Week 4), stored in password_hash
- created_at/updated_at default to SYSUTCDATETIME(); updated_at must be set by app code on update
- Bookings has no event_id (seat already knows its event)
- Seats unique on (event_id, row_label, seat_number)
- Index naming: IX_Table_column; index foreign keys that are searched often
- Seed script looks up IDs by code/name, not hard-coded IDs
- Transaction pattern: BEGIN TRY / BEGIN TRAN / changes / COMMIT / END TRY / BEGIN CATCH / IF @@TRANCOUNT > 0 ROLLBACK / END CATCH
- Secrets (DB password) live in user-secrets, never in the repo
- Minimal API style (app.MapGet etc.) in Program.cs for now

## Stuck on

- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5
- TRY...CATCH and CHECK constraint: not run/understood yet, re-teach next session

## Weak spots

- SQL Server vs MySQL syntax (AUTO_INCREMENT vs IDENTITY)
- PowerShell vs Linux command syntax (no backslash line breaks)
- FOREIGN KEY needs the column declared first
- Missing commas / typos in SQL (REATE INDEX)
- JOIN syntax (working; keep practising)
- Variables only live within one batch; GO ends the batch
- Clustered vs non-clustered index (basic idea only)
- TRY...CATCH + transaction: can't write it from scratch yet
- ACID: Isolation (Week 5) and Durability not yet by experience
- EF Core flow: understand the big picture (request -> MapGet -> db.Events -> SQL -> JSON); DbContext, DI and LINQ code still unfamiliar
- Tired = lower understanding: on tired days do 15-min tasks, not new concepts

## Interview questions I couldn't answer

- What is Docker, and how is a container different from a VM? (to answer)
- What does -p 1433:1433 mean? (answered: host port : container port)
- What is a primary key? (answered: uniquely identifies each row, no duplicates, no NULLs, referenced by foreign keys; one per table, can be composite)
- Primary key vs UNIQUE constraint? (answered: one PK per table, many UNIQUEs allowed; PK can't be NULL, UNIQUE allows one NULL in SQL Server)
- What is a composite key? (answered: PK or UNIQUE across 2+ columns; the combination must be unique)
- IDENTITY vs GUID as primary key (partly; IDENTITY small, ordered, fast, guessable; GUID big, hard to guess, random GUIDs fragment the index)
- IDENTITY vs AUTO_INCREMENT (answered: same idea, different syntax)
- What is T-SQL? (answered: Microsoft's SQL dialect with variables, IF/WHILE, TRY...CATCH, TOP, stored procedures)
- What is a foreign key and why use one? (answered; add: default blocks deleting a parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered: one-way hash, random salt so identical passwords give different hashes)
- Lookup table vs CHECK constraint for statuses? (to answer: lookup = FK-enforced, add values with INSERT, extra join; CHECK = simpler, needs schema change to add values)
- DELETE vs TRUNCATE? (to revisit: DELETE row by row, allows WHERE, keeps IDENTITY counter; TRUNCATE empties fast, no WHERE, resets IDENTITY, fails if a FK points to the table)
- What is GO? (to revisit: SSMS batch separator, not T-SQL; variables don't carry across batches)
- WHERE vs HAVING? (answered: WHERE filters rows before grouping, HAVING filters groups after; only HAVING can use COUNT(\*))
- How does GROUP BY work with COUNT(_)? (answered: rows bucketed by group column, COUNT(_) counts rows per bucket)
- What is an index, and what is the downside of too many? (answered: sorted B-tree, few steps instead of reading all rows; extra storage and slower writes)
- Scan vs seek? (answered: scan reads everything, seek jumps via the index; tiny tables may scan)
- Clustered vs non-clustered index? (to revisit: clustered = table rows stored in key order, one per table, usually the PK; non-clustered = separate sorted list pointing to rows)
- A 10-million-row query is slow, what do you check first? (answered: filter with WHERE, check indexes; add: look at the execution plan first)
- What is a transaction? (answered: several changes succeed or fail together)
- What does ACID stand for? (partly: only A and C seen so far)
- TRAN vs TRY...CATCH? (to revisit: TRAN groups changes all-or-nothing; TRY...CATCH detects an error and jumps to CATCH where you ROLLBACK)
- What happens if one statement in a transaction fails? (to revisit: jump to CATCH, ROLLBACK undoes everything)
- What is Swagger/OpenAPI? (to answer: standard JSON description of an API; tools generate docs and a test page)
- What is a Web API, and why not let the client talk to the database directly? (to answer: security, business rules, keep the database private)
- What is EF Core, and what is a DbContext? (partly: EF Core translates C# to SQL; DbContext is the gateway to the database, DbSet = a table as a list)
- What is dependency injection? (to answer: .NET creates objects like AppDbContext and hands them in, e.g. the db parameter in an endpoint)
- What is LINQ, and how does EF Core know what SQL to write? (partly: C# queries like Where(...) are translated to SQL; nothing runs until ToListAsync)
- How do you keep database passwords out of source control? (answered: user-secrets for local dev; environment variables / secret manager in production)
- What does git add do; staging vs committing? (to answer)
