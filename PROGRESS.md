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
- Session format: I send "Week X, Session Y, goal: ___" plus PROGRESS.md
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
- Schedule note: Sessions 0-2 done. Remaining Week 4 sessions planned for Sunday (2 sessions) or Monday holiday (1 session).

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1: SQL Server 2025 in Docker (sql1), EventBooking DB, 7 tables with FKs, schema.sql, seed.sql, JOINs, PK/UNIQUE/composite key.
- Week 2: GROUP BY/HAVING, indexes (scan vs seek), BEGIN TRAN/COMMIT/ROLLBACK, ACID.
- Week 3: .NET project, EF Core 9, user-secrets connection string, Event entity, AppDbContext, EventsController with GET list, GET by id, POST, PUT, DELETE (tested in Swagger), validation attributes ([Required], [StringLength], [Range]) returning 400, Swagger UI, Program.cs understood (AddControllers vs MapControllers), controllers vs minimal APIs, path vs query parameters. Committed and pushed.
- Week 4, Session 0: Visual Studio 2022 set up, EventBooking.sln created and opened, API runs from VS (https profile, https://localhost:7032/swagger). Understood why there are two ports (http 5166, https 7032, from launchSettings.json) and why https on the http port fails.
- Week 4, Session 1: BCrypt.Net-Next package added; User entity (Models/User.cs) + DbSet<User>; RegisterRequest DTO (Dtos/RegisterRequest.cs); AuthController with POST /api/auth/register (hash with BCrypt, RoleId = 1 set by server, returns only UserId/Name/Email); duplicate email check returns 409 Conflict; tested in Swagger, hash confirmed in DB. Understood entity vs DTO, hash vs encryption, constructor injection, Controller vs ControllerBase. Committed and pushed.
- Week 4, Session 2: JwtBearer package added; Jwt:Key stored in user-secrets; LoginRequest DTO (Dtos/LoginRequest.cs); POST /api/auth/login (FirstOrDefaultAsync by email, BCrypt.Verify, same 401 message for unknown email and wrong password, returns "Login works" for now). Tested in Swagger: right password 200, wrong password 401, unknown email 401. Understood why hashes can't be compared with == (random salt; Verify reads the salt from the stored hash), authentication vs authorization, user enumeration. Got the full login code after being stuck; will retype from memory in the blank-file challenge.

## Current

- Week 4: JWT auth + roles + BCrypt + [Authorize] + first xUnit tests. In progress (Sessions 0-2 done; login works but returns no token yet).

## Next

- Confirm done: Session 2 commit pushed (git add ., git commit, git push, git status clean)
- Confirm done: Jwt:Key in user-secrets is a fresh key, not the one pasted in chat
- Quick check: register with a 3-char password and a bad email returns 400
- Week 4, Session 3: generate the JWT in login (claims: user id, email, role; expiry; sign with Jwt:Key); AddAuthentication().AddJwtBearer in Program.cs + UseAuthentication/UseAuthorization; Swagger "Authorize" button; [Authorize] on one endpoint
- Week 4, Session 4: roles (Admin-only POST/PUT/DELETE events, GET public); need a way to make an ADMIN user (update role_id in SQL); friendly error handling (e.g. delete event blocked by foreign key instead of raw 500)
- Week 4, weekend: first xUnit tests; blank-file challenge
- Study on my own: what CreatedAtAction does (builds the Location URL from GetById's route; does not run GetById)
- Confirm done: CK_Events_total_seats ALTER line and "CREATE INDEX" typo fix in schema.sql
- Confirm done: TRY...CATCH script (Z11 booking + Z12 break test) run
- Blank-file challenge (weekend): write the TRY...CATCH template from memory; retype the login method from memory
- Later: replace hard-coded RoleId = 1 with a lookup by role code
- Leftover (cut first): SQLBolt lessons 1-6

## Project state

- Repo: event-booking-api: PROGRESS.md, schema.sql, seed.sql, EventBooking.sln, EventBooking.Api (Models/Event.cs, Models/User.cs, Dtos/RegisterRequest.cs, Dtos/LoginRequest.cs, Data/AppDbContext.cs, Controllers/EventsController.cs, Controllers/AuthController.cs, Program.cs)
- Tools: .NET 9, Docker Desktop, SSMS, Visual Studio 2022 (run/debug C#), VS Code (SQL files, quick edits)
- Packages: EF Core 9 (SqlServer), BCrypt.Net-Next, Microsoft.AspNetCore.Authentication.JwtBearer 9.x (installed, not wired up yet)
- Run it: start Docker Desktop + sql1, open EventBooking.sln in VS, pick https profile, F5, go to https://localhost:7032/swagger. Stop the app before renaming files.
- Endpoints: GET /api/events, GET /api/events/{id}, POST /api/events, PUT /api/events/{id}, DELETE /api/events/{id} (no auth yet); POST /api/auth/register; POST /api/auth/login (no token yet)
- User-secrets: ConnectionStrings:Default, Jwt:Key (never in the repo, never pasted in chats)
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Users table: user_id, name, email (unique), password_hash, phone_number (nullable), role_id (FK to Roles), created_at, updated_at. Roles seeded: CUSTOMER = 1, ADMIN = 2
- seed.sql user "zorus@gmail.com" has a fake hash (cannot log in); users created via register (e.g. zorus2@gmail.com) have real BCrypt hashes and can log in
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id
- Risk: sql1 has no volume; deleting the container loses the tables. Rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7)
- Risk: RoleId = 1 is hard-coded; breaks if Roles are re-seeded with different ids

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case DB columns; C# PascalCase with [Table]/[Column]; no [Key] needed (EventId follows ClassName+Id)
- IDENTITY(1,1) primary keys; gaps are normal
- Passwords hashed with BCrypt (one-way, never decrypted, never returned in responses); check with BCrypt.Verify, never compare hashes with ==
- Login returns the same 401 message for unknown email and wrong password (prevents user enumeration)
- Controllers (not minimal APIs); ControllerBase (not Controller); REST-style routes, lowercase (e.g. [Route("api/auth")], [HttpPost("register")])
- Folders: Models = entities (DB shape), Dtos = request/response shapes (API shape), Data = DbContext, Controllers (plural). Namespace matches folder; file-scoped namespace (namespace X;)
- Never accept an entity as request body; use a DTO. Server decides RoleId and PasswordHash
- Use the full name BCrypt.Net.BCrypt.HashPassword / Verify (the using shortcut conflicts with the namespace name)
- Validation: model attributes give a friendly 400 first;