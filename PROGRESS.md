# PROGRESS.md

## Plan (10 weeks, Week 1 = 12 Oct)
- W1: SQL basics + booking schema + SQL Server in Docker
- W2: Joins, GROUP BY, indexes, transactions, constraints + .NET project + EF Core
- W3: CRUD APIs, validation, error handling, Swagger, push to GitHub
- W4: JWT auth + roles + basic tests (xUnit)
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
- Calendar note: Week 3 started 26 Oct. Full-day study sessions are planned with breaks (not cramming).
- Schedule note: Week 3 Session 4 may happen tonight or tomorrow; Week 4 stays 4 sessions + 1 extra setup session; worst case slips into Week 5, but auth and tests are not cut.

## Done
- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Sessions 1-4: SQL Server 2025 in Docker (sql1), EventBooking DB, 7 tables with FKs, schema.sql, seed.sql, first JOINs, PK/UNIQUE/composite key quiz.
- Week 2, Sessions 1-3: GROUP BY/HAVING, indexes (scan vs seek), IX_Bookings_seat_id and IX_Bookings_user_id, BEGIN TRAN/COMMIT/ROLLBACK, ACID.
- Week 3, Session 1: created .NET project EventBooking.Api, ran it.
- Week 3, Session 2: EF Core 9 packages, user-secrets connection string, Event entity, AppDbContext, GET /events.
- Week 3, Session 3: CHECK constraint exists in DB (POST with totalSeats 0 trips it); wrote GET /events/{id} and POST /events; added Swagger UI (Swashbuckle.AspNetCore.SwaggerUI 7.*) and tested POST there; learned IDENTITY gaps; reseeded Events so there are 3 events (latest id 3).

## Current
- Week 3: CRUD APIs, validation, error handling, Swagger, controllers. GET list, GET by id, POST work. Still to do: API validation (400), PUT, DELETE, error handling, convert to EventsController.

## Next
- Week 3, Session 4 (tonight or tomorrow):
  1. Walkthrough of Program.cs line by line (builder, AddOpenApi, AddDbContext, Build, pipeline, UseHttpsRedirection, MapGet, the WeatherForecast template code)
  2. Re-explain URLs: path parameter (/events/2) vs query parameter (/events?id=2)
  3. Controllers vs minimal APIs: convert the 3 endpoints to EventsController ([ApiController], [Route], [HttpGet], [HttpPost], constructor injection)
  4. API validation: return 400 with a clear message for empty name or totalSeats <= 0 (before the DB CHECK fires)
  5. PUT /events/{id} and DELETE /events/{id}; basic error handling (clean error response instead of raw 500)
  6. Commit and push
- Confirm done: CK_Events_total_seats ALTER line and the "CREATE INDEX" typo fix in schema.sql
- Confirm done: TRY...CATCH script (Z11 booking + Z12 break test) actually run
- Week 4, Session 0 (extra setup): install/check Visual Studio Community (workload: ASP.NET and web development), create solution (dotnet new sln -n EventBooking, dotnet sln add EventBooking.Api), open .sln in VS. Optional: delete the WeatherForecast template code.
- Week 4: JWT auth + roles + BCrypt + [Authorize] + first xUnit tests
- Blank-file challenge (weekend): write the TRY...CATCH template from memory
- Leftover (cut first): SQLBolt lessons 1-6

## Project state
- Repo: event-booking-api: PROGRESS.md, schema.sql, seed.sql, EventBooking.Api (Models/Event.cs, Data/AppDbContext.cs, Program.cs with GET /events, GET /events/{id}, POST /events, Swagger UI)
- Tools: .NET 9, Docker Desktop, SSMS, VS Code (C# Dev Kit needed for red-line errors; moving to Visual Studio in Week 4)
- Run it: start Docker Desktop + sql1, dotnet run --project EventBooking.Api, open the printed URL + /swagger in the normal browser (copy the URL from the terminal), Ctrl+C to stop
- Connection string in user-secrets (ConnectionStrings:Default), not in the repo
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id
- Seeded lookups: Roles (CUSTOMER, ADMIN), SeatStatuses (AVAILABLE, HELD, BOOKED), BookingStatuses (PENDING_PAYMENT, CONFIRMED, CANCELLED, EXPIRED)
- Sample data: seed.sql (deletes Bookings, Seats, Users, Events, then reinserts; never run on real data). Events currently reseeded manually (3 events)
- Risk: sql1 has no volume; deleting the container loses the tables. Rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7)

## Decisions
- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case DB columns; C# PascalCase with [Table]/[Column]; no [Key] needed because EventId follows the ClassName+Id convention
- IDENTITY(1,1) primary keys; gaps are normal
- NVARCHAR for text; lookup tables with code, name, is_active
- Passwords hashed with BCrypt in .NET (Week 4)
- created_at/updated_at default to SYSUTCDATETIME(); app sets updated_at on update
- Bookings has no event_id (seat knows its event)
- Seats unique on (event_id, row_label, seat_number)
- Transaction pattern: BEGIN TRY / BEGIN TRAN / changes / COMMIT / END TRY / BEGIN CATCH / IF @@TRANCOUNT > 0 ROLLBACK / END CATCH
- Secrets live in user-secrets, never in the repo
- Minimal API style for now; switch to controllers (EventsController) in Session 4, since job code and interviews use controllers
- Validation: API returns clean 400 first; DB CHECK constraint is the safety net
- Use Swagger UI in the browser to test POST/PUT/DELETE

## Stuck on
- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5
- Program.cs: only understand my 3 endpoints, not the template code
- Controllers vs minimal APIs and URL path vs query parameters: need re-explaining

## Weak spots
- SQL Server vs MySQL syntax; PowerShell vs Linux syntax
- FOREIGN KEY needs the column declared first; typos in SQL
- Variables only live within one batch; GO ends the batch
- Clustered vs non-clustered index (basic idea only)
- TRY...CATCH + transaction: can't write it from scratch yet
- ACID: Isolation (Week 5) and Durability not yet by experience
- EF Core: big picture clear (request -> MapGet -> db.Events -> SQL -> JSON); DbContext, DI, LINQ code still unfamiliar
- Typos in C# names (AppDbCOntext) and using MapGet instead of MapPost
- Tired = lower understanding: on tired days do 15-min tasks, not new concepts

## Interview questions I couldn't answer
- What is Docker, and how is a container different from a VM? (to answer)
- What does -p 1433:1433 mean? (answered: host port : container port)
- What is a primary key? (answered: unique, no NULLs, referenced by FKs; one per table, can be composite)
- Primary key vs UNIQUE? (answered: one PK, many UNIQUEs; UNIQUE allows one NULL in SQL Server)
- What is a composite key? (answered: 2+ columns, combination unique)
- IDENTITY vs GUID as primary key (partly; IDENTITY small, ordered, guessable; GUID big, hard to guess, fragments index)
- IDENTITY vs AUTO_INCREMENT (answered: same idea, different syntax)
- Why are there gaps in IDENTITY values? (answered: counter never goes back; deletes, failed inserts, rollbacks, restarts leave gaps; DELETE doesn't reset it, TRUNCATE does)
- What is T-SQL? (answered)
- What is a foreign key and why use one? (answered; add: default blocks deleting parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered: one-way hash, random salt)
- Lookup table vs CHECK constraint for statuses? (to answer)
- DELETE vs TRUNCATE? (answered: DELETE row by row, WHERE allowed, keeps IDENTITY; TRUNCATE fast, no WHERE, resets IDENTITY, blocked by FKs)
- What is GO? (to revisit: SSMS batch separator, not T-SQL)
- WHERE vs HAVING? (answered)
- How does GROUP BY work with COUNT(*)? (answered)
- What is an index, and the downside of too many? (answered)
- Scan vs seek? (answered)
- Clustered vs non-clustered index? (to revisit)
- A 10-million-row query is slow, what first? (answered: check the execution plan, WHERE, indexes)
- What is a transaction? (answered)
- What does ACID stand for? (partly: A and C seen)
- TRAN vs TRY...CATCH? (to revisit: TRAN groups changes; TRY...CATCH detects errors and triggers ROLLBACK)
- What happens if one statement in a transaction fails? (to revisit)
- What is Swagger/OpenAPI? (to answer: standard JSON description of an API; tools generate docs and a test page)
- What is a Web API, and why not let the client talk to the DB directly? (to answer: security, business rules, DB stays private)
- What is EF Core, and what is a DbContext? (partly)
- What is dependency injection? (to answer: .NET creates objects like AppDbContext and hands them in)
- What is LINQ? (partly: Language Integrated Query; C# Where(...) translated to SQL; nothing runs until ToListAsync)
- FindAsync vs FirstOrDefaultAsync? (answered: FindAsync = by primary key only; FirstOrDefault = any condition)
- What does Add vs SaveChangesAsync do? (answered: Add stages in memory, SaveChangesAsync sends it to the DB in one transaction)
- What do async/await mean? (answered: wait for the result without blocking the server thread)
- 200, 201, 400, 404? (200 OK, 201 Created, 400 client sent bad data, 404 not found)
- API validation vs DB constraint? (answered: API returns friendly 400, DB CHECK is the last safety net)
- Controllers vs minimal APIs? (to revisit: same job; controllers are class-based with attributes, minimal APIs are lightweight lambdas)
- Path vs query parameter? (to revisit: /events/2 vs /events?id=2)
- In a controller, what makes a method respond to POST? (to answer: [HttpPost])
- What is xUnit? (answered: .NET test framework, [Fact] methods check results)
- How do you keep DB passwords out of source control? (answered: user-secrets locally; env variables/secret manager in production)
- What does git add do; staging vs committing? (to answer)