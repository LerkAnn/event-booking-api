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

- Week 5, Session 3: first the SQL TRY...CATCH + transaction template (declare variables before BEGIN TRY, no GO; INSERT booking + UPDATE seat to BOOKED for Z11; CATCH with IF @@TRANCOUNT > 0 ROLLBACK and ERROR_MESSAGE()); break test with Z12 and seat_id 99999 (no Bookings row should appear). Then design the booking flow on paper (inputs, checks, steps, errors), then start the booking endpoint
- Week 5: booking logic (service class), POST booking endpoint (user id from the JWT, seat id from the request DTO), handle the duplicate-key error from UX_Bookings_active_seat as 409, concurrency test (never cut); ACID Isolation by experience
- Confirm done: schema.sql has the UX_Bookings_active_seat index and comment (commit it)
- Side practice (outside the project timeline, only if there is spare time): write DECLARE-lookup + INSERT queries from a blank file, e.g. insert a booking for zorus2@gmail.com on seat Z12 with status PENDING_PAYMENT
- Weekend / blank-file challenge: retype the login method + token generation in C# from memory
- Optional: rename the second test to Register_ValidRequest_PassesValidation; make sure Assert.NotEmpty is removed from the first test
- Optional: run a test in Visual Studio Test Explorer with Debug and inspect the results list
- Study on my own: what CreatedAtAction does (builds the Location URL from GetById's route; does not run GetById)
- Confirm done: CK_Events_total_seats ALTER line and "CREATE INDEX" typo fix in schema.sql
- Optional stretch: call /me with a tampered token (change one character) and see the 401
- Later: replace hard-coded RoleId = 1 with a lookup by role code; the filtered index also hard-codes status ids (1, 2), so keep BookingStatuses seed order fixed
- Later: for DbUpdateException, check the inner SQL error number (547 = FK violation, 2601/2627 = duplicate key) instead of assuming every save failure is one cause
- Later (production): set ValidateIssuer/ValidateAudience with real values; use a different Jwt:Key from a secret store
- Leftover (cut first): SQLBolt lessons 1-6

## Project state

- Repo: event-booking-api: PROGRESS.md, schema.sql, seed.sql, EventBooking.sln, EventBooking.Api (Models/Event.cs, Models/User.cs, Models/Role.cs, Dtos/RegisterRequest.cs, Dtos/LoginRequest.cs, Data/AppDbContext.cs, Controllers/EventsController.cs, Controllers/AuthController.cs, Program.cs), EventBooking.Tests (RegisterRequestTests.cs)
- Tools: .NET 9, Docker Desktop, SSMS, Visual Studio 2022 (run/debug C#), VS Code (SQL files, quick edits)
- Packages: EF Core 9 (SqlServer), BCrypt.Net-Next, Microsoft.AspNetCore.Authentication.JwtBearer 9.x (wired up in Program.cs); xUnit in the test project
- Run it: start Docker Desktop + sql1, open EventBooking.sln in VS, pick https profile, F5, go to https://localhost:7032/swagger. Stop the app before renaming files.
- If VS shows "ExecutionEngineException" at login: use Ctrl+F5 or dotnet run --project EventBooking.Api --launch-profile https; update VS if it repeats (happened once, fixed itself)
- Run tests: dotnet test (from the repo root); no Docker or running API needed for the current tests
- Test with a token: POST /api/auth/login, copy the token without quotes, click Authorize in Swagger, paste, Authorize, Close. Log in again after any role change.
- Endpoints: GET /api/events (public), GET /api/events/{id} (public), POST/PUT/DELETE /api/events ([Authorize(Roles = "ADMIN")]; DELETE returns 409 if the event has seats/bookings); POST /api/auth/register; POST /api/auth/login (returns { token }); GET /api/auth/me ([Authorize])
- User-secrets: ConnectionStrings:Default, Jwt:Key (never in the repo, never pasted in chats)
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Users table: user_id, name, email (unique), password_hash, phone_number (nullable), role_id (FK to Roles), created_at, updated_at. Roles seeded: CUSTOMER = 1, ADMIN = 2
- BookingStatuses ids: 1 PENDING_PAYMENT, 2 CONFIRMED, 3 CANCELLED, 4 EXPIRED (active = 1, 2)
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id, UX_Bookings_active_seat (filtered unique on Bookings(seat_id) WHERE booking_status_id IN (1, 2))
- Test users: zorus@gmail.com (seed, fake hash, cannot log in); zorus2@gmail.com (ADMIN, can log in); a second registered user (CUSTOMER)
- Test data note: Z10 now has a CANCELLED and a CONFIRMED booking from the index tests; run seed.sql to reset
- Risk: sql1 has no volume; deleting the container loses the tables. Rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7). After a rebuild, re-register users and re-promote the admin with SQL UPDATE.
- Risk: RoleId = 1 and the index's status ids (1, 2) are hard-coded; break if lookup tables are re-seeded in a different order
- Risk: the Seat entity, Booking entity and booking endpoints do not exist yet (only the tables)

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case DB columns; C# PascalCase with [Table]/[Column]; no [Key] needed (EventId follows ClassName+Id)
- IDENTITY(1,1) primary keys; gaps are normal
- Passwords hashed with BCrypt (one-way, never decrypted, never returned in responses); check with BCrypt.Verify, never compare hashes with ==
- Login returns the same 401 message for unknown email and wrong password (prevents user enumeration)
- JWT (stateless) chosen over cookie sessions; token lifetime 1 hour; payload holds user id, email, role code, never secrets
- Roles: role code (CUSTOMER / ADMIN) goes into the token as ClaimTypes.Role; [Authorize(Roles = "ADMIN")] must match the code exactly. Accepted downside: role changes apply only after the token expires or the user logs in again
- Controllers (not minimal APIs); ControllerBase (not Controller); REST-style routes, lowercase (e.g. [Route("api/auth")], [HttpPost("register")])
- Folders: Models = entities (DB shape), Dtos = request/response shapes (API shape), Data = DbContext, Controllers (plural). Namespace matches folder; file-scoped namespace (namespace X;)
- Never accept an entity as request body; use a DTO. Server decides RoleId and PasswordHash
- Use the full name BCrypt.Net.BCrypt.HashPassword / Verify (the using shortcut conflicts with the namespace name)
- Validation: model attributes give a friendly 400 first; code check (e.g. duplicate email -> 409) next; DB constraint (CHECK/UNIQUE) is the safety net
- Double-booking rule (option B): bookings are kept after cancel/expiry; only one ACTIVE booking (status PENDING_PAYMENT or CONFIRMED) per seat, enforced by a filtered unique index in the DB (works even when two requests race), with a friendly check/409 in C# on top
- Catch specific exceptions (DbUpdateException), not all of Exception; only wrap the SaveChangesAsync call
- Flat early-return style in controller methods (no deep nested ifs)
- Unit tests: Arrange / Act / Assert; test names like Thing_Condition_ExpectedResult; unit tests need no DB or server
- No EF migrations or seed code; schema.sql and seed.sql by hand
- Logic moves to service classes around Week 5; controllers stay thin
- Secrets in user-secrets, never in the repo (JWT key too)
- Test with Swagger UI

## Stuck on

- CreatedAtAction: studying on my own
- Two requests registering the same email at the same moment: DB UNIQUE catches it (concurrency idea, Week 5)

## Weak spots

- SQL Server vs MySQL syntax; PowerShell vs Linux syntax
- Clustered vs non-clustered index (basic idea only)
- TRY...CATCH + transaction in SQL: can't write it from scratch yet (first item in Week 5, Session 3)
- Writing DECLARE-lookup + INSERT queries from memory (know the logic, forget the syntax)
- ACID: Isolation (Week 5) and Durability not yet by experience
- EF Core, DI and LINQ: big picture clear, details still unfamiliar
- JWT setup in Program.cs and token generation: followed the steps, but can't write from memory yet
- Tired = lower understanding: on tired days do 15-min tasks, not new concepts

## Interview questions I couldn't answer

- What is Docker, and how is a container different from a VM? (to answer)
- What does -p 1433:1433 mean? (answered: host port : container port)
- Primary key / PK vs UNIQUE / composite key (answered)
- IDENTITY vs GUID as primary key (partly)
- Why are there gaps in IDENTITY values? (answered)
- What is a foreign key and why use one? (answered; add: default blocks deleting a parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered; hash is one-way, not encryption)
- Hashing vs encryption? (answered)
- Why can't you compare a typed password's hash to the stored hash with ==? (answered: random salt, use Verify)
- Lookup table vs CHECK constraint for statuses? (to answer)
- DELETE vs TRUNCATE? (answered)
- What is GO? (to revisit)
- WHERE vs HAVING; GROUP BY with COUNT(\*) (answered)
- What is an index, downside of too many; scan vs seek (answered)
- Clustered vs non-clustered index? (to revisit)
- What is a filtered unique index, and why use one for bookings? (partly: only rows matching the WHERE are in the index, so cancelled rows don't block rebooking; answer fully next time)
- Why enforce "one active booking per seat" in the DB and not only in C#? (to answer: two simultaneous requests can both pass a C# check; the index rejects the second)
- What is a transaction? (answered); ACID (partly: A and C seen)
- TRAN vs TRY...CATCH? (answered: TRAN = all-or-nothing undo; TRY...CATCH = error handler; usually combined, CATCH runs ROLLBACK)
- What happens if one statement in a transaction fails? (to revisit)
- What is Swagger/OpenAPI? (to answer)
- What is a Web API, and why not let the client talk to the DB directly? (to answer)
- What is EF Core, and what is a DbContext? (partly)
- What is dependency injection? (partly: seen constructor injection of AppDbContext and IConfiguration)
- What is LINQ? (partly)
- FindAsync vs FirstOrDefaultAsync; Add vs SaveChangesAsync; async/await (answered)
- 200, 201, 204, 400, 404 (answered); 409 (answered: conflict with current state, e.g. duplicate email, delete blocked by FK; better than a raw 500)
- 401 vs 403? (answered: 401 = not authenticated; 403 = known user but not allowed)
- API validation vs DB constraint? (answered)
- What is a DTO and why use one? (answered: doorway for client input, keeps entity/DB shape private, blocks extra fields like roleId)
- Entity vs DTO? (answered)
- Controller vs ControllerBase? (answered: API uses ControllerBase)
- Controllers vs minimal APIs? (partly)
- Path vs query parameter? (answered)
- AddControllers vs MapControllers; [ApiController] (answered / partly)
- How does .NET know which environment it is in? (answered: ASPNETCORE_ENVIRONMENT)
- What is xUnit? (answered)
- What is a unit test, and what is Arrange/Act/Assert? (answered)
- Unit test vs integration test? (answered: unit = one component, fast, no DB/server; integration = parts working together)
- Why test validation rules? (answered: guards the API's door; catches accidental rule changes)
- Why catch a specific exception instead of all of Exception? (answered: other failures should stay visible as 500)
- How do you keep DB passwords out of source control? (answered: user-secrets)
- What does git add do; staging vs committing? (to answer)
- Authentication vs authorization? (answered)
- Why the same error for wrong email and wrong password? (answered: user enumeration)
- What is a JWT, and what are its three parts? (answered: header, payload, signature; signature is made from header + payload + secret key)
- Is a JWT payload encrypted? (answered: encoded only, no passwords in it)
- Why can the server trust a token without asking the database? (answered: only the holder of Jwt:Key can make a valid signature)
- Why is a JWT secret key kept out of the repo, and what can someone do with it? (answered: forge tokens)
- What is role-based authorization, and how does [Authorize(Roles = "ADMIN")] know the role? (answered: reads the role claim from the validated token)
- Why put the role in the token instead of asking the DB each request? (answered: saves a DB call per request)
- Downside of a role in the token? (answered: stale until the token expires; fix with short lifetimes or refresh tokens)
