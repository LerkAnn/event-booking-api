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

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Session 1: SQL Server 2025 running in Docker (container sql1), connected from SSMS, created EventBooking database, created Events table.
- Week 1, Session 2: designed schema; created all 7 tables with foreign keys.
- Week 1, Session 3: schema.sql and seed.sql in repo, sample data loaded, first JOIN query.
- Week 1, Session 4: wrote a 5-table JOIN myself. Quiz: primary key, PK vs UNIQUE, composite key.
- Week 2, Session 1: blank-file challenge done (Events + Seats from memory). GROUP BY query with HAVING. Learned WHERE vs HAVING.
- Week 2, Session 2: learned indexes (scan vs seek, B-tree idea). Created IX_Bookings_seat_id and IX_Bookings_user_id, added to schema.sql, pushed. Read an execution plan (Ctrl+L).

## Current

- Week 2: Joins, GROUP BY, indexes, transactions, constraints + .NET project + EF Core. Indexes done.

## Next

- Week 2, Session 3: ask me for the script that generates thousands of rows, then compare scan vs seek with and without the index (stretch, 15 min). Then start transactions (BEGIN TRAN / COMMIT / ROLLBACK), needed for Week 5 booking logic
- Then constraints review, then create the .NET project and add EF Core (later in Week 2)
- Leftover (cut first if tired): SQLBolt lessons 1-6
- Optional: save the SA password outside the repo

## Project state

- Repo: event-booking-api, contains PROGRESS.md, schema.sql (with 2 indexes at the bottom), seed.sql
- Tools: .NET 9, Docker Desktop, SSMS; SQL Server 2025 in Docker as sql1 (port 1433, user sa)
- Run it: open Docker Desktop, start container sql1 if stopped, connect SSMS to localhost,1433 (Trust server certificate)
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id (plus automatic ones for PKs and UNIQUEs)
- Seeded lookups: Roles (CUSTOMER, ADMIN), SeatStatuses (AVAILABLE, HELD, BOOKED), BookingStatuses (PENDING_PAYMENT, CONFIRMED, CANCELLED, EXPIRED)
- Sample data: seed.sql (deletes Bookings, Seats, Users, Events, then reinserts; never run on real data)
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
- Index naming: IX_Table_column; index foreign keys that are searched often (SQL Server doesn't do it automatically)
- Seed script looks up IDs by code/name, not hard-coded IDs
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
- Forgetting to scope GROUP BY queries to one event (add the Events join + WHERE)
- Putting an index on the wrong table (index goes on the table holding the column you search by)
- Clustered vs non-clustered index (only the basic idea so far)

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
- What is an index, and what is the downside of too many? (answered: sorted B-tree so lookups take a few steps instead of reading all rows; costs extra storage and slower writes)
- Scan vs seek? (answered: scan reads everything, seek jumps via the index; tiny tables may scan because it's cheaper)
- Clustered vs non-clustered index? (to revisit: clustered = table rows stored in key order, one per table, usually the PK; non-clustered = separate sorted list pointing to rows)
- A 10-million-row query is slow, what do you check first? (answered: filter with WHERE, check indexes; add: look at the execution plan first for scan vs seek)
- What does git add do; staging vs committing? (to answer)
