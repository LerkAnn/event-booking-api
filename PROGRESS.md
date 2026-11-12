# PROGRESS.md

## How to teach me (read this first in every new chat)

- I write the core code (SQL, booking logic, auth, tests); AI only for boilerplate/UI
- Hints before solutions; I try 15 min first; full solutions only if I ask
- After my code works, ask me to explain it; weekend = blank-file challenge
- Keep answers short; at most one question at a time (interview questions can be several, but give me time to answer)
- Tired = 15-min task (one SQL query, fix one error, write one test, or one quiz question)
- "wrap up" = output my FULL updated PROGRESS.md plus the git commit commands
- When a session's goal is done, just say it is done and wait for me to say "wrap up"
- Don't log one-off small mistakes in PROGRESS.md; only things that keep costing me time
- Tell me which interview questions each topic covers; quiz me occasionally
- Don't let me overbuild. If behind, cut optional first. If early, suggest stretch tasks, not the next week
- When I change schema, tell me exactly what to add or change in schema.sql / seed.sql
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
- Schedule note: Week 4 done (auth, roles). Week 5, Sessions 1-2 done (xUnit, error handling, double-booking rule in the DB). Remaining Week 5: SQL TRY...CATCH template, booking logic in C#, concurrency test (never cut).

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1: SQL Server 2025 in Docker (sql1), EventBooking DB, 7 tables with FKs, schema.sql, seed.sql, JOINs, PK/UNIQUE/composite key.
- Week 2: GROUP BY/HAVING, indexes (scan vs seek), BEGIN TRAN/COMMIT/ROLLBACK, ACID.
- Week 3: .NET project, EF Core 9, user-secrets connection string, Event entity, AppDbContext, EventsController with GET list, GET by id, POST, PUT, DELETE (tested in Swagger), validation attributes ([Required], [StringLength], [Range]) returning 400, Swagger UI, Program.cs understood (AddControllers vs MapControllers), controllers vs minimal APIs, path vs query parameters. Committed and pushed.
- Week 4, Session 0: Visual Studio 2022 set up, EventBooking.sln created and opened, API runs from VS (https profile, https://localhost:7032/swagger). Understood why there are two ports (http 5166, https 7032, from launchSettings.json) and why https on the http port fails.
- Week 4, Session 1: BCrypt.Net-Next package added; User entity (Models/User.cs) + DbSet<User>; RegisterRequest DTO (Dtos/RegisterRequest.cs); AuthController with POST /api/auth/register (hash with BCrypt, RoleId = 1 set by server, returns only UserId/Name/Email); duplicate email check returns 409 Conflict; tested in Swagger, hash confirmed in DB. Understood entity vs DTO, hash vs encryption, constructor injection, Controller vs ControllerBase. Committed and pushed.
- Week 4, Session 2: JwtBearer package added; Jwt:Key stored in user-secrets; LoginRequest DTO; POST /api/auth/login (FirstOrDefaultAsync by email, BCrypt.Verify, same 401 message for unknown email and wrong password). Understood why hashes can't be compared with == (random salt), authentication vs authorization, user enumeration. Committed and pushed.
- Week 4, Session 3: login builds and returns a JWT (claims: NameIdentifier = user id, Email; expires in 1 hour; HmacSha256; key from Jwt:Key via IConfiguration injected in AuthController). Program.cs: AddAuthentication + AddJwtBearer (validates signature and lifetime), UseAuthentication, UseAuthorization before MapControllers; Swagger Authorize button (Bearer scheme). GET /api/auth/me with [Authorize]. Understood JWT = header.payload.signature, payload is encoded not encrypted, stateless auth. Committed and pushed.
- Week 4, Session 4: Role entity (Models/Role.cs) + DbSet<Role>; login looks up the role code and adds ClaimTypes.Role to the token; /me returns userId, email, role; zorus2@gmail.com is ADMIN; [Authorize(Roles = "ADMIN")] on POST/PUT/DELETE events, GET public. Tested 401 / 403 / 200. Understood 401 vs 403, role-based authorization, role-in-token tradeoff (stale until expiry). Committed and pushed.
- Week 5, Session 1: xUnit test project EventBooking.Tests (added to the .sln, references EventBooking.Api). RegisterRequestTests: Register_ShortPassword_FailsValidation (Assert.False) and a valid-request test (Assert.True), using Validator.TryValidateObject with all-properties true; proved the test can fail. Understood Arrange/Act/Assert, unit vs integration tests, why test validation rules. Friendly error handling: EventsController.Delete wraps SaveChangesAsync in try/catch DbUpdateException and returns 409 Conflict; tested 409 (event with seats), 204 (new event), 401, 403. Understood 409 vs 500, why catch a specific exception, C# try/catch = SQL TRY...CATCH idea, TRAN (all-or-nothing, undo) vs TRY...CATCH (error handler). Committed and pushed.
- Week 5, Session 2: decided the double-booking rule = option B (keep cancelled/expired bookings; only one ACTIVE booking per seat). Added filtered unique index UX_Bookings_active_seat ON Bookings(seat_id) WHERE booking_status_id IN (1, 2) (1 = PENDING_PAYMENT, 2 = CONFIRMED; 3 = CANCELLED, 4 = EXPIRED). Tested in SSMS: second active booking for Z10 rejected with duplicate key error; after cancelling the old booking, rebooking succeeded. Understood how a filtered unique index works (only qualifying rows are in the index; DB checks it during the write, so it is safe under concurrent requests). schema.sql updated with the index and an explanatory comment. TRY...CATCH + transaction script postponed to next session (tired).

## Current

- Week 5: booking logic, double-booking prevention, concurrency test. Sessions 1-2 done. DB rule for double-booking is in place; C# booking logic not started.

## Next

- Week 5, Session 3: first the SQL TRY...CATCH + transaction template (declare variables before BEGIN TRY, no