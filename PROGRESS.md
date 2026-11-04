# PROGRESS.md

## How to teach me (read this first in every new chat)

- I write the core code (SQL, booking logic, auth, tests); AI only for boilerplate/UI
- Hints before solutions; I try 15 min first; full solutions only if I ask
- After my code works, ask me to explain it; weekend = blank-file challenge
- Keep answers short; at most one question at a time (interview questions can be several, but give me time to answer)
- Tired = 15-min task (one SQL query, fix one error, write one test, or one quiz question)
- "wrap up" = output my FULL updated PROGRESS.md
- Tell me which interview questions each topic covers; quiz me occasionally
- Don't let me overbuild. If behind, cut optional first. If early, suggest stretch tasks, not the next week
- Session format: I send "Week X, Session Y, goal: \_\_\_" plus PROGRESS.md
- Never cut: booking logic, auth, concurrency test, Docker Compose, system design basics

## Plan (10 weeks, Week 1 = 12 Oct)

- W1: SQL basics + booking schema + SQL Server in Docker
- W2: Joins, GROUP BY, indexes, transactions, constraints + .NET project + EF Core
- W3: CRUD APIs, validation, error handling, Swagger, push to GitHub
- W4: JWT auth + roles + basic tests (xUnit)
- W5: Booking logic, double-booking prevention, concurrency test
- W6: Seat hold + background job + fake payment endpoint
- W7: Redis caching, full docker compose up, logging, environment settings (Development/UAT/Production)
- W8: System design basics + architecture diagram
- W9: README, cleanup, resume, LinkedIn, start applying (~7 Dec)
- W10: Mock interviews, behavioral stories, more applications
- Cut first if behind: CI, deploy, Hangfire, LeetCode
- Resign only after the bonus is paid in Feb (2-month notice)
- Sessions: 3x40 min weekdays + 1x90 min weekend; 15 min minimum on bad days
- Calendar note: Week 3 started 26 Oct. Full-day study sessions are planned with breaks (not cramming).
- Schedule note: Week 4 = 4 sessions + 1 extra setup session (Session 0); worst case slips into Week 5, but auth and tests are not cut.

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1: SQL Server 2025 in Docker (sql1), EventBooking DB, 7 tables with FKs, schema.sql, seed.sql, JOINs, PK/UNIQUE/composite key.
- Week 2: GROUP BY/HAVING, indexes (scan vs seek), BEGIN TRAN/COMMIT/ROLLBACK, ACID.
- Week 3: .NET project, EF Core 9, user-secrets connection string, Event entity, AppDbContext, EventsController with GET list, GET by id, POST, PUT, DELETE (tested in Swagger), validation attributes ([Required], [StringLength], [Range]) returning 400, Swagger UI, Program.cs understood (AddControllers vs MapControllers), controllers vs minimal APIs, path vs query parameters. Committed and pushed.

## Current

- Week 4: JWT auth + roles + BCrypt + [Authorize] + first xUnit tests. Not started.

## Next

- Week 4, Session 0 (setup): install/check Visual Studio Community (workload: ASP.NET and web development), create solution (dotnet new sln -n EventBooking, dotnet sln add EventBooking.Api), open .sln in VS, run the API from VS
- Week 4, Session 1: basic error handling (friendly response instead of raw 500, e.g. delete blocked by foreign key); start auth: User entity, register endpoint with BCrypt
- Then: login + JWT, roles (Admin-only create/update/delete events), [Authorize], first xUnit tests
- Study on my own: what CreatedAtAction does (builds the Location URL from GetById's route; does not run GetById)
- Confirm done: CK_Events_total_seats ALTER line and "CREATE INDEX" typo fix in schema.sql
- Confirm done: TRY...CATCH script (Z11 booking + Z12 break test) run
- Blank-file challenge (weekend): write the TRY...CATCH template from memory
- Leftover (cut first): SQLBolt lessons 1-6

## Project state

- Repo: event-booking-api: PROGRESS.md, schema.sql, seed.sql, EventBooking.Api (Models/Event.cs, Data/AppDbContext.cs, Controllers/EventsController.cs, Program.cs)
- Tools: .NET 9, Docker Desktop, SSMS, VS Code (moving to Visual Studio)
- Run it: start Docker Desktop + sql1, dotnet run --project EventBooking.Api, open printed URL + /swagger in the browser, Ctrl+C to stop
- Endpoints: GET /api/events, GET /api/events/{id}, POST /api/events, PUT /api/events/{id}, DELETE /api/events/{id} (no auth yet)
- Connection string in user-secrets (ConnectionStrings:Default), not in the repo
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Users table: user_id, name, email (unique), password_hash, phone_number, role_id (FK to Roles), created_at, updated_at. Roles seeded: CUSTOMER, ADMIN
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id
- Risk: sql1 has no volume; deleting the container loses the tables. Rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7)

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case DB columns; C# PascalCase with [Table]/[Column]; no [Key] needed (EventId follows ClassName+Id)
- IDENTITY(1,1) primary keys; gaps are normal
- Passwords hashed with BCrypt (Week 4)
- Controllers (not minimal APIs); REST-style routes under [Route("api/events")]
- Validation: model attributes give a friendly 400 first; DB CHECK constraint is the safety net
- Logic moves to service classes around Week 5; controllers stay thin
- Secrets in user-secrets, never in the repo (JWT key goes there too)
- Test with Swagger UI

## Stuck on

- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5
- CreatedAtAction: studying on my own

## Weak spots

- SQL Server vs MySQL syntax; PowerShell vs Linux syntax
- FOREIGN KEY needs the column declared first; typos in SQL and C# names
- Variables only live within one batch; GO ends the batch
- Clustered vs non-clustered index (basic idea only)
- TRY...CATCH + transaction: can't write it from scratch yet
- ACID: Isolation (Week 5) and Durability not yet by experience
- EF Core, DI and LINQ: big picture clear, details still unfamiliar
- Mixing up HTTP attributes (HttpPut vs HttpDelete); IDE autocomplete can suggest the wrong one
- Tired = lower understanding: on tired days do 15-min tasks, not new concepts

## Interview questions I couldn't answer

- What is Docker, and how is a container different from a VM? (to answer)
- What does -p 1433:1433 mean? (answered: host port : container port)
- Primary key / PK vs UNIQUE / composite key (answered)
- IDENTITY vs GUID as primary key (partly)
- Why are there gaps in IDENTITY values? (answered)
- What is a foreign key and why use one? (answered; add: default blocks deleting a parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered)
- Lookup table vs CHECK constraint for statuses? (to answer)
- DELETE vs TRUNCATE? (answered)
- What is GO? (to revisit)
- WHERE vs HAVING; GROUP BY with COUNT(\*) (answered)
- What is an index, downside of too many; scan vs seek (answered)
- Clustered vs non-clustered index? (to revisit)
- What is a transaction? (answered); ACID (partly: A and C seen)
- TRAN vs TRY...CATCH? (to revisit)
- What happens if one statement in a transaction fails? (to revisit)
- What is Swagger/OpenAPI? (to answer)
- What is a Web API, and why not let the client talk to the DB directly? (to answer)
- What is EF Core, and what is a DbContext? (partly)
- What is dependency injection? (to answer)
- What is LINQ? (partly)
- FindAsync vs FirstOrDefaultAsync; Add vs SaveChangesAsync; async/await (answered)
- 200, 201, 204, 400, 404 (answered)
- API validation vs DB constraint? (answered)
- Controllers vs minimal APIs? (partly)
- Path vs query parameter? (answered)
- AddControllers vs MapControllers; [ApiController] (answered / partly)
- How does .NET know which environment it is in? (answered: ASPNETCORE_ENVIRONMENT)
- What is xUnit? (answered)
- How do you keep DB passwords out of source control? (answered: user-secrets)
- What does git add do; staging vs committing? (to answer)
