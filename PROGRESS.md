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
- Calendar note: Week 3 started 26 Oct. Week 2 leftovers were folded into Week 3 Session 1.

## Done
- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Session 1: SQL Server 2025 running in Docker (container sql1), connected from SSMS, created EventBooking database, created Events table.
- Week 1, Session 2: designed schema; created all 7 tables with foreign keys.
- Week 1, Session 3: schema.sql and seed.sql in repo, sample data loaded, first JOIN query.
- Week 1, Session 4: wrote a 5-table JOIN myself. Quiz: primary key, PK vs UNIQUE, composite key.
- Week 2, Session 1: blank-file challenge done. GROUP BY query with HAVING. Learned WHERE vs HAVING.
- Week 2, Session 2: indexes (scan vs seek, B-tree idea). Created IX_Bookings_seat_id and IX_Bookings_user_id, added to schema.sql, pushed. Read an execution plan.
- Week 2, Session 3: loaded 5,000 rows, saw Seek with index and Scan without, recreated the index. Learned BEGIN TRAN / COMMIT / ROLLBACK and ACID. Cleaned up load-test data.
- Week 2, Session 4: received TRY...CATCH transaction template and CHECK constraint; understood the idea (TRAN = draft, TRY...CATCH = alarm that triggers cleanup).
- Week 3, Session 1: ran the TRY...CATCH script (booked Z11) and break test, ran the CHECK constraint on Events.total_seats and saw it reject 0, created the .NET project EventBooking.Api (dotnet new webapi), ran it and opened the OpenAPI JSON. Committed and pushed. Learned why an API sits between client and database.

## Current
- Week 3: CRUD APIs, validation, error handling, Swagger, push to GitHub. Project created and running; no EF Core or endpoints of my own yet.

## Next
- Week 3, Session 2: add EF Core packages (SqlServer + Design), create the Event entity and DbContext, connection string (password NOT in repo, use user-secrets or appsettings.Development.json in .gitignore), first GET /events endpoint
- Add CK_Events_total_seats ALTER line to schema.sql if not done yet
- Then: POST/PUT/DELETE events, validation, error handling, Swagger page
- Blank-file challenge (weekend): write the TRY...CATCH template from memory
- Leftover (cut first if tired): SQLBolt lessons 1-6
- Optional: save the SA password outside the repo

## Project state
- Repo: event-booking-api, contains PROGRESS.md, schema.sql (2 indexes at the bottom), seed.sql, EventBooking.Api (default webapi template)
- Tools: .NET 9, Docker Desktop, SSMS; SQL Server 2025 in Docker as sql1 (port 1433, user sa)
- Run it: open Docker Desktop, start container sql1 if stopped, connect SSMS to localhost,1433 (Trust server certificate). API: dotnet run --project EventBooking.Api, then open the printed URL + /openapi/v1.json or /weatherforecast, Ctrl+C to stop
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id (plus automatic ones for PKs and UNIQUEs)
- Constraint: CK_Events_total_seats (total_seats > 0)
- Seeded lookups: Roles (CUSTOMER, ADMIN), SeatStatuses (AVAILABLE, HELD, BOOKED), BookingStatuses (PENDING_PAYMENT, CONFIRMED, CANCELLED, EXPIRED)
- Sample data: seed.sql (deletes Bookings, Seats, Users, Events, then reinserts; never run on real data). Z11 is now BOOKED with a booking from the TRY...CATCH test, so reseed if you want a clean state
- Risk: sql1 has no volume, so deleting the container loses the tables; rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7)

## Decisions
- Stack: .NET 9, SQL Server 2025 image, EF Core
- snake_case column names, plural table names
- IDENTITY(1,1) primary keys (not GUID)
- NVARCHAR for text; phone_number NVARCHAR(20) nullable
- Lookup tables (Roles, SeatStatuses, BookingStatuses) with code, name, is_active
- Passwords hashed in .NET (BCrypt, Week 4), stored in password_hash
- created_at/updated_at default to SYSUTCDATETIME(); updated_at must be set by app code on update
- Bookings has no event_id (seat already knows its event)
- Seats unique on (event_id, row_label, seat_number); its index already covers event_id lookups
- Index naming: IX_Table_column; index foreign keys that are searched often
- Seed script looks up IDs by code/name, not hard-coded IDs
- Transaction pattern: BEGIN TRY / BEGIN TRAN / changes / COMMIT / END TRY / BEGIN CATCH / IF @@TRANCOUNT > 0 ROLLBACK / END CATCH
- API sits between client and database (security, business rules, DB stays private)
- SA password stored outside the repo, never committed

## Stuck on
- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5

## Weak spots
- SQL Server vs MySQL syntax (AUTO_INCREMENT vs IDENTITY)
- PowerShell vs Linux command syntax (no backslash line breaks)
- FOREIGN KEY needs the column declared first
- Missing commas in CREATE TABLE
- JOIN syntax (working; keep practising the SELECT list with aliases)
- Variables only live within one batch; GO ends the batch
- Forgetting to scope GROUP BY queries to one event
- Putting an index on the wrong table
- Clustered vs non-clustered index (basic idea only)
- TRY...CATCH + transaction: ran it, understand the flow, can't write it from scratch yet
- ACID: seen Atomicity and Consistency; Isolation (Week 5) and Durability not yet by experience
- Tired = lower understanding: on tired days do 15-min tasks, not new concepts
- .NET project structure (Program.cs, .csproj) just started

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
- WHERE vs HAVING? (answered: WHERE filters rows before grouping, HAVING filters groups after; only HAVING can use COUNT(*))
- How does GROUP BY work with COUNT(*)? (answered: rows bucketed by group column, COUNT(*) counts rows per bucket)
- What is an index, and what is the downside of too many? (answered: sorted B-tree so lookups take a few steps instead of reading all rows; costs extra storage and slower writes)
- Scan vs seek? (answered: scan reads everything, seek jumps via the index; tiny tables may scan because it's cheaper)
- Clustered vs non-clustered index? (to revisit: clustered = table rows stored in key order, one per table, usually the PK; non-clustered = separate sorted list pointing to rows)
- A 10-million-row query is slow, what do you check first? (answered: filter with WHERE, check indexes; add: look at the execution plan first)
- What is a transaction? (answered: several changes succeed or fail together; BEGIN TRAN, COMMIT makes permanent, ROLLBACK undoes)
- What does ACID stand for? (partly: Atomicity all-or-nothing, Consistency rules always hold, Isolation concurrent transactions don't see each other's half-done work, Durability committed data survives a crash; only A and C seen so far)
- What is the difference between TRAN and TRY...CATCH? (to revisit: TRAN groups changes into all-or-nothing; TRY...CATCH detects an error and jumps to CATCH where you ROLLBACK)
- What happens if one statement in a transaction fails? (to revisit: control jumps to CATCH, ROLLBACK undoes everything, nothing half-done; seen in the Z12 break test)
- What is Swagger/OpenAPI? (to answer: standard JSON description of an API; tools generate docs and a test page from it)
- What is a Web API, and why not let the client talk to the database directly? (to answer: security, business rules, keep the database private)
- What does git add do; staging vs committing? (to answer)