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
- Don't leave optional items out by default: do them when there is time, cut them first only if a week gets tight
- When I change schema, tell me exactly what to add or change in schema.sql / seed.sql
- Before writing queries or code with many ids, list which tables and ids each step needs
- Session format: I send "Week X, Session Y, goal: ___" plus PROGRESS.md
- Never cut: booking logic, auth, concurrency test, Docker Compose, system design basics

## Plan (10 weeks, Week 1 = 12 Oct)

- W1: SQL basics + booking schema + SQL Server in Docker
- W2: Joins, GROUP BY, indexes, transactions, constraints + .NET project + EF Core
- W3: CRUD APIs, validation, error handling, Swagger, push to GitHub
- W4: JWT auth + roles + basic tests (xUnit)
- W5: Booking logic, double-booking prevention, concurrency test
- W6: Seat hold + fake payment + double-payment fail-safe (done); expiry job logic done
- W7: S1 expiry worker (done); S2 expiry test + real values + Docker Compose SQL Server with volume (done); S3 API on compose DB, test users, timer values in appsettings (done); S4 (90 min) API in Docker Compose, then logging + environment settings (Development/Production), then Redis if time allows; blank-file challenge only if time remains (cut first)
- W8: S1 system design basics; S2 architecture diagram; S3 repo cleanup, secrets check, screenshots, README; S4 final CV/LinkedIn polish and applications (CV already drafted early in W7)
- W9: mock interviews, behavioral stories, stretch tasks, apply
- W10: interview practice for incoming calls, more applications
- Cut first if behind: CI, deploy, Hangfire, LeetCode
- Resign only after the bonus is paid in Feb (2-month notice)
- Sessions: 3x40 min weekdays + 1x90 min weekend; 15 min minimum on bad days
- Calendar note: Week 3 started 26 Oct. Full-day study sessions are planned with breaks (not cramming).
- CV notes: list only what works today and has been seen running. Event-booking project goes first under Technical Projects. Add "containerised the API and SQL Server with Docker Compose" only after API-in-Compose runs; add a Redis caching bullet and drop "(basic, used at work)" from the Redis skill only after caching runs in this project. Project is an ASP.NET Core Web API (not MVC with views). Udemy .NET Core MVC cert goes under "Certifications" only; honest answer = completed the course, then built a hands-on Web API; have not built Razor views. Playwright (work, small login test) can be listed honestly as "basic end-to-end login test". Node.js was a tutorial only: leave it out for .NET roles; if a Node role, "Node.js (basic, tutorial)" under Backend. Redis: basic use at work, so "Redis (basic, used at work)". Repo is public; .env is gitignored and not in history; check history for secrets before each CV update.
- Recruiter note: a recruiter (Rebecca Grace) contacted me about a TypeScript/Node.js (Express/Hono), React/Next.js, PostgreSQL/Prisma role asking for 5 years; she said 5 years is not strict. My stack is C#/.NET, SQL Server, Angular, so the stack gap is bigger than the years gap. Be honest on the call: do not claim Node, React or Next.js. Before the call, check the company (LinkedIn, JobStreet/Glassdoor); ask about the team, the real stack, and why a junior is being considered. CV goes to her Monday; update it after API-in-Compose (Week 7 S4)

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
- Week 5, Session 1: xUnit test project EventBooking.Tests (added to the .sln, references EventBooking.Api). RegisterRequestTests: Register_ShortPassword_FailsValidation (Assert.False) and a valid-request test (Assert.True), using Validator.TryValidateObject with all-properties true; proved the test can fail. Understood Arrange/Act/Assert, unit vs integration tests, why test validation rules. Friendly error handling: EventsController.Delete wraps SaveChangesAsync in try/catch DbUpdateException and returns 409 Conflict; tested 409, 204, 401, 403. Committed and pushed.
- Week 5, Session 2: decided the double-booking rule = option B (keep cancelled/expired bookings; only one ACTIVE booking per seat). Added filtered unique index UX_Bookings_active_seat ON Bookings(seat_id) WHERE booking_status_id IN (1, 2) (1 = PENDING_PAYMENT, 2 = CONFIRMED; 3 = CANCELLED, 4 = EXPIRED). Tested in SSMS. schema.sql has the index and a comment. Committed and pushed.
- Week 5, Session 3: wrote the SQL TRY...CATCH + transaction script myself (DECLARE lookups before BEGIN TRY; BEGIN TRAN; INSERT Bookings + UPDATE Seats to BOOKED; COMMIT; CATCH with IF @@TRANCOUNT > 0 ROLLBACK and ERROR_MESSAGE()). Break tests passed (FK failure on INSERT; failing UPDATE undoes the INSERT). Learned TRAN vs TRY...CATCH, open transactions hold locks, an UPDATE matching 0 rows is not an error. Designed the booking flow on paper. Understood who wins a race (lock on the index key, exactly one winner). Committed and pushed.
- Week 5, Session 4: entities Seat, Booking, SeatStatus, BookingStatus (Models/, boilerplate generated) + DbSets in AppDbContext; OnModelCreating sets Booking CreatedAt/UpdatedAt to HasDefaultValueSql("SYSUTCDATETIME()"); BookingRequest DTO (SeatId only, [Range(1, int.MaxValue)]); BookingsController: POST /api/bookings with [Authorize]; user id from the token, seat lookup (404 if missing), seat must be AVAILABLE (409), CONFIRMED and BOOKED status lookups, Bookings.Add + seat.SeatStatusId = BOOKED, one SaveChangesAsync in try/catch DbUpdateException -> 409. Tested in Swagger and SSMS. Understood: user id from the token (signed), atomic = all or nothing, one SaveChangesAsync = one transaction. Committed and pushed.
- Week 6, Session 1: concurrency test = console project EventBooking.ConcurrencyTest (in the .sln): logs in with HttpClient, sets the Bearer header, fires 10 POST /api/bookings for one seat with Select + Task.WhenAll (no await per request, so they overlap), prints status code counts. Result: one 200 and nine 409; SQL shows exactly one active booking. Explained why: late requests are stopped by the C# AVAILABLE check; requests that pass the check together reach the unique index, the DB locks the key, one wins, the others get a duplicate-key DbUpdateException, caught and returned as 409. Then BookingService: Services/IBookingService.cs (contract), Services/BookingResult.cs (enum), Services/BookingService.cs (all booking logic, AppDbContext injected, no HTTP types); registered with builder.Services.AddScoped<IBookingService, BookingService>() in Program.cs; BookingsController injects only IBookingService and maps results to status codes with a switch. Understood interface = contract, DI = ask for the interface, .NET gives the class registered for it, layered structure (controller, service, data). Committed and pushed.
- Week 6, Session 2: seat hold. Seat status while held = HELD (BOOKED is final, a hold can still be released); HELD (id 2) already existed in SeatStatuses, schema.sql and seed.sql. New bookings are PENDING_PAYMENT (status 1) and the seat becomes HELD. Test console reads TEST_EMAIL and TEST_PASSWORD from environment variables (set per terminal with $env: in PowerShell). Re-tested Swagger and the concurrency test. Committed and pushed.
- Week 6, Session 3: fake payment endpoint POST /api/bookings/{id}/pay ([Authorize], no body). IBookingService.PayBookingAsync(userId, bookingId) returns PaymentResults (Paid, BookingNotFound, NotTheOwner, BookingNotPending, Conflict; Services/PaymentResults.cs). Owner check uses the user id from the token; controller maps BookingNotFound and NotTheOwner to 404 with the same message (hides which ids exist), BookingNotPending and Conflict to 409, Paid to 200. Tested in Swagger and SSMS. Understood 404 vs 403 trade-off, status code comes from the controller method, IDENTITY jumps (rollbacks burn numbers; restart of sql1 skips about 1000). Found the double-payment gap. Committed and pushed.
- Week 6, Session 4: double-payment fail-safe. PayBookingAsync now keeps the finds, owner check and pending check, then: BeginTransactionAsync (await using var tx); conditional ExecuteUpdateAsync on Bookings (WHERE booking_id AND status still PENDING_PAYMENT; sets CONFIRMED + UpdatedAt) returns rows changed; rows == 0 -> BookingNotPending (409); seat -> BOOKED via SaveChangesAsync in try/catch DbUpdateException -> Conflict; tx.CommitAsync() at the end (early return = dispose = rollback). POST /api/bookings now returns { bookingId, seatId }: Services/CreateBookingOutcome.cs is a record (BookingResult Result, int? BookingId = null); IBookingService.CreateBookingAsync returns Task<CreateBookingOutcome>; controller switches on outcome.Result. ConcurrencyTest Program.cs now has modes via env vars: TEST_MODE (book | pay, default book), TEST_SEAT_ID (default 5); pay mode creates one booking then fires 10 parallel pays: result 1 x 200 and 9 x 409, booking CONFIRMED and seat BOOKED in SQL (test code generated by AI, I read and understand it). Also wrote ExpireHoldsAsync(TimeSpan maxAge) in BookingService (loads PENDING_PAYMENT bookings older than maxAge, sets EXPIRED + UpdatedAt, seat -> AVAILABLE, one SaveChangesAsync, returns count; 0 on DbUpdateException); method body generated by AI because I was tired (my logic matched). Understood: row lock in SQL Server makes the second UPDATE wait, then re-check its WHERE and match 0 rows; optimistic vs pessimistic concurrency; tx is the handle to the transaction on the same connection; record vs enum. Committed and pushed.
- Week 7, Session 1: expiry worker. Services/HoldExpiryWorker.cs is a BackgroundService: constructor takes IServiceScopeFactory; ExecuteAsync loops while !stoppingToken.IsCancellationRequested; each round creates a scope (using var scope = _scopeFactory.CreateScope()), gets IBookingService with GetRequiredService, calls ExpireHoldsAsync, prints the count, then await Task.Delay(..., stoppingToken). Registered in Program.cs with builder.Services.AddHostedService<HoldExpiryWorker>(). AI gave the skeleton with three blanks (interface, minutes, seconds); I filled them and my version matched. Understood (after a simpler explanation): scoped services live per request, a BackgroundService lives for the whole app, so it creates its own mini-request scope each round. Committed and pushed.
- Week 7, Session 2: tested the expiry worker: booking created (status 1, seat HELD), after the timer the booking became EXPIRED (4) and the seat AVAILABLE (1); same seat bookable again (200). Found and fixed a bug (AI-generated line): EF Core could not translate DateTime.UtcNow - maxAge inside the query (InvalidOperationException); fix = compute var cutoff = DateTime.UtcNow - maxAge before the query and compare b.CreatedAt < cutoff. Understood: nobody calls ExecuteAsync, .NET calls it once at startup via AddHostedService and the loop keeps it alive; the hold age is not stored anywhere, each round computes a cutoff and asks the DB (polling), so expiry happens between hold age and hold age + interval; GetRequiredService asks the scope's lookup table for the registered class; TimeSpan = a length of time; Task.Delay sits below the work (pause between rounds) and stoppingToken ends the delay early on shutdown (no extra final round); our timer loop vs cron (a scheduler tool; Hangfire in between). Docker Compose: docker-compose.yml in the repo root (service sqlserver, image mcr.microsoft.com/mssql/server:2025-latest, container_name sql-compose, ACCEPT_EULA, MSSQL_SA_PASSWORD: ${MSSQL_SA_PASSWORD}, ports "1434:1433", volume sqldata:/var/opt/mssql); password in .env (gitignored, no <> or quotes or spaces); sql1 still on 1433, separate container and password. Created EventBooking DB on localhost,1434 and ran schema.sql and seed.sql. Volume proof passed: docker compose down then up -d (no -v), data still there. Understood: image = template, container = running copy, docker run creates a new container, start button restarts the same one, a volume is a folder outside the container that survives deletion, down -v deletes volumes, -d = detached (background). Lessons: SQL Server reads the SA password only the first time it creates files in the volume (changing .env needs down -v); the connection Database Name must be an existing database (leave <default> or master), a wrong name gives "Login failed". Committed and pushed.
- Week 7, Session 3 (Parts 1-3): switched the API connection string (user-secrets) to the compose DB (localhost,1434); GET /api/events returns the seed events. Re-registered a CUSTOMER and an ADMIN user on the compose DB (register, then promoted the admin with a SQL UPDATE using a role-code lookup). Moved the worker's timer values to config: appsettings.json has "HoldExpiry": { "HoldMinutes": 10, "IntervalSeconds": 60 }; appsettings.Development.json overrides HoldMinutes to 1 for quick testing (found it nested under appsettings.json in Solution Explorer); HoldExpiryWorker injects IConfiguration and reads both values once before the while loop with GetValue<int>; tested: hold expires after about 1 minute in Development. Understood: appsettings is for non-secret settings only; passwords and the JWT key stay in user-secrets or environment variables; Development file overrides the base file. Also (outside the plan): drafted CV updates for a recruiter contact (skills by category, event-booking project bullets, honest notes on Redis/Node/Docker). Part 4 (API in Compose) not started, moved to Week 7 S4. Committed and pushed.

## Current

- Week 7, Session 3 done except Part 4. API runs from VS against the compose DB (localhost,1434); worker timing comes from appsettings. docker-compose.yml still has only the SQL Server service.

## Next

- Week 7, Session 4 (90 min): (1) API in Docker Compose (about 40 min, never cut): Dockerfile in EventBooking.Api (AI gives the boilerplate skeleton); add an api service to docker-compose.yml (build, ports "8080:8080", environment ConnectionStrings__Default with host sqlserver and port 1433 (not localhost or 1434), Jwt__Key from .env, depends_on sqlserver); add the new values to .env (gitignored); docker compose up -d --build; open http://localhost:8080/swagger; note the container can't read user-secrets, and localhost inside the API container means the API container itself. (2) Logging + environment settings (Development/Production): ILogger in services and the worker; log the failed save in ExpireHoldsAsync; try/catch inside the worker loop so a crash doesn't kill it silently. (3) Redis (cache GET /api/events) only if time remains; blank-file challenge is cut first
- After API-in-Compose works: update the CV bullet ("containerised the API and SQL Server with Docker Compose (persistent volume), so one command starts the full stack") and send the updated CV to the recruiter if not yet sent
- Blank-file challenge: retype BookingService.CreateBookingAsync from memory; retype ExpireHoldsAsync, PayBookingAsync and HoldExpiryWorker from memory; retype the login method + token generation in C# from memory
- Known gap: a user paying at the moment the hold expires (pay vs expire race). Pay is protected by its conditional WHERE, but ExpireHoldsAsync loads bookings then saves with a plain UPDATE. Fix later with a conditional update (same idea as pay) or RowVersion. Think about it before the interview
- Known gap: ExpireHoldsAsync returns 0 on a failed save, same as "nothing expired"; log it once logging exists (Week 7 S4)
- Known gap: if ExpireHoldsAsync throws, the worker dies silently (no try/catch around the loop body); handle with logging in Week 7 S4
- Confirm done: schema.sql has the UX_Bookings_active_seat index and comment
- Confirm done: CK_Events_total_seats ALTER line and "CREATE INDEX" typo fix in schema.sql
- Side practice (outside the project timeline, only if there is spare time): write DECLARE-lookup + INSERT queries from a blank file; retype the TRY...CATCH script from memory
- Optional: RowVersion concurrency token on Booking as an alternative to the conditional UPDATE (interview alternative; needs a schema change)
- Optional: skim the Udemy MVC course (controllers, routing, EF Core, Razor views) after the project, so I can talk about MVC with views
- Optional stretch: log inside the DbUpdateException catch to count how many requests reach the index (proves the index, not the C# check, stopped them)
- Optional stretch: run the parallel pay test with the old pay code once, to see several 200s (before/after for interviews)
- Optional: a join query (Bookings + Users + Seats + statuses) to read booking data with names instead of ids
- Optional: rename the second test to Register_ValidRequest_PassesValidation; make sure Assert.NotEmpty is removed from the first test
- Optional: run a test in Visual Studio Test Explorer with Debug and inspect the results list
- Optional stretch: call /me with a tampered token (change one character) and see the 401
- Study on my own: what CreatedAtAction does (builds the Location URL from GetById's route; does not run GetById)
- Later: replace hard-coded RoleId = 1 with a lookup by role code; the filtered index also hard-codes status ids (1, 2), so keep BookingStatuses seed order fixed
- Later: for DbUpdateException, check the inner SQL error number (547 = FK violation, 2601/2627 = duplicate key) instead of assuming every save failure is one cause
- Later: null checks on status lookups and on the seat (FirstOrDefaultAsync / FindAsync return null if no row; using the result would crash with a 500) in CreateBookingAsync, PayBookingAsync and ExpireHoldsAsync (the ! operator only silences the warning)
- Later (production): set ValidateIssuer/ValidateAudience with real values; use a different Jwt:Key from a secret store
- Leftover (cut first): SQLBolt lessons 1-6

## Project state

- Repo (public): event-booking-api: PROGRESS.md, schema.sql, seed.sql, docker-compose.yml, .env (gitignored), .gitignore, EventBooking.sln, EventBooking.Api (Models/Event.cs, User.cs, Role.cs, Seat.cs, Booking.cs, SeatStatus.cs, BookingStatus.cs; Dtos/RegisterRequest.cs, LoginRequest.cs, BookingRequest.cs; Data/AppDbContext.cs; Services/IBookingService.cs, BookingResult.cs, CreateBookingOutcome.cs, PaymentResults.cs, BookingService.cs, HoldExpiryWorker.cs; Controllers/EventsController.cs, AuthController.cs, BookingsController.cs; Program.cs; appsettings.json, appsettings.Development.json), EventBooking.Tests (RegisterRequestTests.cs), EventBooking.ConcurrencyTest (Program.cs)
- Tools: .NET 9, Docker Desktop, SSMS (and the VS Code SQL extension), Visual Studio 2022 (run/debug C#), VS Code (SQL files, YAML, quick edits)
- Packages: EF Core 9 (SqlServer), BCrypt.Net-Next, Microsoft.AspNetCore.Authentication.JwtBearer 9.x (wired up in Program.cs); xUnit in the test project
- Run it: start Docker Desktop and the compose DB (docker compose up -d from the repo root), open EventBooking.sln in VS, pick https profile, F5, go to https://localhost:7032/swagger. Stop the app before renaming files. Save files before running (VS has no auto save). The expiry worker starts with the API; in Development holds expire after 1 minute (appsettings.Development.json), otherwise 10 minutes; it checks every 60 seconds
- If VS shows "ExecutionEngineException" at login: use Ctrl+F5 or dotnet run --project EventBooking.Api --launch-profile https; update VS if it repeats (happened once, fixed itself)
- Run tests: dotnet test (from the repo root); no Docker or running API needed for the current tests
- Run the concurrency tests: reset one seat to AVAILABLE with no active booking (SQL: seat status AVAILABLE, old bookings CANCELLED), start the API (https profile), open a second terminal in the repo root, then set: $env:TEST_EMAIL = "..."; $env:TEST_PASSWORD = "..."; $env:TEST_MODE = "book" or "pay"; $env:TEST_SEAT_ID = "<free seat id>"; then dotnet run --project EventBooking.ConcurrencyTest (variables last only for that terminal; use a CUSTOMER user). Expect 200: 1 and 409: 9 in both modes. Each run needs a fresh free seat
- Docker Compose: from the repo root, docker compose up -d (start, in the background), docker compose down (stop and remove containers, keep data), docker compose down -v (also deletes the volume = data loss, only for a reset). Compose SQL Server: localhost,1434, login sa, password from .env; container sql-compose, volume event-booking-api_sqldata. Old sql1 is on localhost,1433 with its own password and is no longer used by the API (can be stopped)
- Test with a token: POST /api/auth/login, copy the token without quotes, click Authorize in Swagger, paste, Authorize, Close. Log in again after any role change.
- Endpoints: GET /api/events (public), GET /api/events/{id} (public), POST/PUT/DELETE /api/events ([Authorize(Roles = "ADMIN")]; DELETE returns 409 if the event has seats/bookings); POST /api/auth/register; POST /api/auth/login (returns { token }); GET /api/auth/me ([Authorize]); POST /api/bookings ([Authorize]; body { seatId }; creates a PENDING_PAYMENT booking and holds the seat; 200 returns { bookingId, seatId } / 404 / 409 / 401); POST /api/bookings/{id}/pay ([Authorize]; owner only; 200 / 404 (missing or not yours) / 409 (not pending or conflict) / 401)
- User-secrets: ConnectionStrings:Default (now the compose DB, localhost,1434), Jwt:Key (never in the repo, never pasted in chats). .env holds MSSQL_SA_PASSWORD (gitignored, never pasted in chats)
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Users table: user_id, name, email (unique), password_hash, phone_number (nullable), role_id (FK to Roles), created_at, updated_at. Roles seeded: CUSTOMER = 1, ADMIN = 2
- SeatStatuses ids: 1 AVAILABLE, 2 HELD, 3 BOOKED
- BookingStatuses ids: 1 PENDING_PAYMENT, 2 CONFIRMED, 3 CANCELLED, 4 EXPIRED (active = 1, 2)
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id, UX_Bookings_active_seat (filtered unique on Bookings(seat_id) WHERE booking_status_id IN (1, 2))
- Test users on the compose DB: a CUSTOMER and an ADMIN (re-registered this week; ADMIN promoted with SQL UPDATE); seed user zorus@gmail.com (fake hash, cannot log in). Old users on sql1 are not on the compose DB
- Test data note: reset a seat before each concurrency test; seed.sql resets data (then re-register users). Booking ids jump (IDENTITY gaps); normal
- Risk: after any DB rebuild (down -v), re-register users and re-promote the admin with SQL UPDATE
- Risk: RoleId = 1 and the index's status ids (1, 2) are hard-coded; break if lookup tables are re-seeded in a different order
- Risk: .env must never be committed (gitignored; check git status before each commit); repo is public, so never commit passwords, the JWT key or real emails

## Decisions

- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case DB columns; C# PascalCase with [Table]/[Column]; no [Key] needed (EventId follows ClassName+Id)
- IDENTITY(1,1) primary keys; gaps are normal (rollbacks burn numbers; SQL Server reserves blocks of 1000, so a restart of the container jumps by about 1000); never use the id as a count
- Passwords hashed with BCrypt (one-way, never decrypted, never returned in responses); check with BCrypt.Verify, never compare hashes with ==
- Login returns the same 401 message for unknown email and wrong password (prevents user enumeration)
- JWT (stateless) chosen over cookie sessions; token lifetime 1 hour; payload holds user id, email, role code, never secrets
- Roles: role code (CUSTOMER / ADMIN) goes into the token as ClaimTypes.Role; [Authorize(Roles = "ADMIN")] must match the code exactly. Accepted downside: role changes apply only after the token expires or the user logs in again
- Controllers (not minimal APIs); ControllerBase (not Controller); REST-style routes, lowercase (e.g. [Route("api/auth")], [HttpPost("register")])
- Structure: layered Web API (Controller -> Service -> DbContext), not full MVC because there are no Views. Controller = HTTP in/out (token, DTO, status codes); Service = business rules; DbContext = database
- Folders: Models = entities (DB shape), Dtos = request/response shapes (API shape), Data = DbContext, Services = business logic + interfaces + result types + background worker, Controllers (plural). Namespace matches folder; file-scoped namespace (namespace X;)
- Never accept an entity as request body; use a DTO. Server decides RoleId, PasswordHash, user id (from token) and booking status
- Use the full name BCrypt.Net.BCrypt.HashPassword / Verify (the using shortcut conflicts with the namespace name)
- Validation: model attributes give a friendly 400 first; code check (e.g. duplicate email -> 409) next; DB constraint (CHECK/UNIQUE) is the safety net
- Double-booking rule (option B): bookings are kept after cancel/expiry; only one ACTIVE booking per seat, enforced by a filtered unique index in the DB, with a friendly check/409 in C# on top
- Booking flow (POST /api/bookings): user id from the token, seat id from the body; seat exists (404), seat AVAILABLE (409); Bookings.Add (PENDING_PAYMENT) + seat status HELD in one SaveChangesAsync (one transaction); DbUpdateException -> 409 (not 400); response returns bookingId and seatId
- Payment flow (POST /api/bookings/{id}/pay): user id from the token; booking exists (404), owner (404, same message as missing, so ids are not revealed), status PENDING_PAYMENT (409); inside one transaction: conditional UPDATE booking -> CONFIRMED (+ UpdatedAt), rows == 0 -> BookingNotPending (409); seat -> BOOKED; commit. Fake: no amount, no payment table, no gateway. In a real system the gateway would be called only after the conditional update wins, so the loser never charges anyone
- Not-your-booking returns 404, not 403: 403 would reveal that an id exists (ids are simple increasing numbers). 403 is acceptable if explained. The service keeps NotTheOwner as its own enum name; only the controller picks the status code
- Seat statuses: AVAILABLE (free), HELD (reserved, unpaid, can be released or expire), BOOKED (paid, final)
- Race rule (booking): the DB serializes writes to the same index key with locks; exactly one winner. The C# check alone is not enough, so the index is the final guard and the catch makes the loser's failure a clean 409. Proven with the concurrency test (1 x 200, 9 x 409)
- Race rule (payment): the unique index does not help (status 2 is allowed), so the guard is a conditional UPDATE (WHERE status still PENDING_PAYMENT) and a rows-affected check. SQL Server's row lock makes the second UPDATE wait, then re-check its WHERE and match 0 rows. Proven with the parallel pay test (1 x 200, 9 x 409). Always ask "what in the DB stops the second request?"
- Optimistic concurrency (assume nobody else changes it; check at write time with a conditional WHERE or RowVersion) is used, not pessimistic locking (UPDLOCK). RowVersion is the interview alternative
- Transactions in EF Core: one SaveChangesAsync = one transaction by itself; use BeginTransactionAsync (await using var tx ... tx.CommitAsync()) when several commands (ExecuteUpdateAsync + SaveChangesAsync) must be one unit; everything the same _db sends between begin and commit joins it; returning without commit rolls back on dispose
- Service rule: services never return HTTP types (NotFound(), Conflict()); they return a result label (BookingResult / PaymentResults enums) or a record that carries a label plus data (CreateBookingOutcome) and the controller maps it to a status code
- Enum = one name from a fixed list; record = small container holding several values together
- DI: services are registered in Program.cs with AddScoped<IInterface, Class>() (one instance per request, same lifetime as AppDbContext); controllers ask for the interface in the constructor and never use new
- Expiry job: HoldExpiryWorker (BackgroundService, singleton, registered with AddHostedService) creates a scope each round through IServiceScopeFactory to get the scoped IBookingService (a singleton can't hold a scoped service); loop with Task.Delay(interval, stoppingToken) so shutdown is quick; config read once before the loop; plain BackgroundService first, Hangfire is cut-first
- Config: non-secret settings (HoldExpiry:HoldMinutes, HoldExpiry:IntervalSeconds) live in appsettings.json; appsettings.Development.json overrides them in Development (1 minute); secrets (passwords, Jwt:Key) stay in user-secrets locally and environment variables in Docker (ConnectionStrings__Default, Jwt__Key: double underscore = colon)
- EF Core cannot translate arithmetic like DateTime.UtcNow - maxAge inside a LINQ query: compute the value in C# first (var cutoff = ...) and compare with the plain DateTime
- Docker Compose: one docker-compose.yml describes the containers (image, port, environment, volume); a named volume keeps the DB files outside the container; secrets (SA password) live in .env, gitignored, referenced as ${NAME} in the YAML; host port 1434 so it doesn't clash with sql1 on 1433; never use down -v unless I want to wipe the data; inside Compose containers reach each other by service name and container port (sqlserver:1433), never localhost
- Concurrency tests = separate console project using HttpClient + Task.WhenAll (parallel); not part of dotnet test because they need the API and DB running; modes and seat id come from env vars
- Test credentials come from environment variables (TEST_EMAIL, TEST_PASSWORD), never hard-coded
- Booking CreatedAt use HasDefaultValueSql("SYSUTCDATETIME()") so the DB fills them on insert; UpdatedAt must be set in C# on later updates (ExecuteUpdateAsync skips EF tracking, so set it in SetProperty)
- Catch specific exceptions (DbUpdateException), not all of Exception; only wrap the SaveChangesAsync call
- Flat early-return style in controller and service methods (no deep nested ifs; separate ifs, not else if)
- Unit tests: Arrange / Act / Assert; test names like Thing_Condition_ExpectedResult; unit tests need no DB or server
- No EF migrations or seed code; schema.sql and seed.sql by hand
- Secrets in user-secrets, never in the repo (JWT key too)
- Test with Swagger UI

## Stuck on

- CreatedAtAction: studying on my own
- Two requests registering the same email at the same moment: DB UNIQUE catches it (same idea as the booking race)

## Weak spots

- SQL Server vs MySQL syntax; PowerShell vs Linux syntax
- Clustered vs non-clustered index (basic idea only)
- Writing DECLARE-lookup + INSERT queries from memory (know the logic, forget which table/ids); TRY...CATCH script done with a template, retype from memory later
- ACID: remember what each letter means (A all-or-nothing, C rules always hold, I parallel transactions don't see half-done work, D committed survives a crash); Isolation seen by experience (concurrency tests), Durability not yet
- Lots of ids at once (user, seat, statuses) is confusing: list the tables and which ids each step needs before writing
- EF Core, DI and LINQ: big picture clear, details still unfamiliar; DI: understand the idea (interface = contract) but the AddScoped "ask for X, get Y" lookup is still hard to visualize; scoped vs singleton and IServiceScopeFactory understood after a simpler explanation (staff member vs customer analogy); can't yet write a service + controller or the worker from a blank file
- Transactions in EF Core (tx handle, ExecuteUpdateAsync inside a transaction) and optimistic vs pessimistic: understood the idea after explanations, can't write from memory yet; retype PayBookingAsync later
- JWT setup in Program.cs and token generation: followed the steps, but can't write from memory yet
- Race conditions: first assumed the payment race would be caught by a conflict like the booking race; the booking race has a DB guard (unique index), the payment race needs a conditional UPDATE. Always ask "what in the DB stops the second request?"
- Docker concepts (image vs container vs volume, compose) understood only after step-by-step explanations; re-explain in my own words before the interview
- Tired = lower understanding: on tired days do 15-min tasks, not new concepts

## Interview questions I couldn't answer

- What is Docker, and how is a container different from a VM? (partly: image = template, container = running copy of an image; a VM has its own full OS, a container shares the host kernel; to finish)
- What does -p 1433:1433 mean? (answered: host port : container port; I use 1434:1433 for the compose DB)
- What is Docker Compose and why use it? (answered: a recipe file listing every container (service) with image, ports, secrets and volume; docker compose up starts everything, down stops it; anyone cloning the repo gets the same setup with one command)
- What is a Docker volume and why does SQL Server need one? (answered: a folder outside the container managed by Docker; data survives deleting the container; without it the tables are lost with the container)
- docker compose down vs down -v; what does -d do? (answered: down removes containers and keeps the volume; -v also deletes the volume; -d = run in the background)
- How do you keep the DB password out of the repo with Docker Compose? (answered: .env file, gitignored, referenced as ${VAR} in the YAML)
- appsettings.json vs user-secrets vs environment variables? (partly: appsettings = non-secret settings in the repo, Development file overrides the base; user-secrets = local secrets outside the repo; environment variables = how Docker passes secrets, double underscore for the colon)
- How does a container reach another container in Compose? (to answer in Week 7 S4: by service name and container port, e.g. sqlserver:1433; localhost inside a container means itself)
- Primary key / PK vs UNIQUE / composite key (answered)
- IDENTITY vs GUID as primary key (partly)
- Why are there gaps in IDENTITY values? (answered: rollbacks burn numbers; SQL Server reserves a block of 1000, so a restart jumps by about 1000; IDENTITY guarantees unique and increasing, not gap-free)
- What is a foreign key and why use one? (answered; add: default blocks deleting a parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered; hash is one-way, not encryption)
- Hashing vs encryption? (answered)
- Why can't you compare a typed password's hash to the stored hash with ==? (answered: random salt, use Verify)
- Lookup table vs CHECK constraint for statuses? (to answer)
- DELETE vs TRUNCATE? (answered)
- What is GO? (to revisit)
- WHERE vs HAVING; GROUP BY with COUNT(*) (answered)
- What is an index, downside of too many; scan vs seek (answered)
- Clustered vs non-clustered index? (to revisit)
- What is a filtered unique index, and why use one for bookings? (answered: only rows matching the WHERE are in the index, so cancelled rows don't block rebooking)
- Why enforce "one active booking per seat" in the DB and not only in C#? (answered: index = prevents double booking, catch = clean 409 for the loser)
- What is a transaction? (answered); ACID (partly: know the four letters' meaning; A, C and I seen in practice, D later)
- What does "atomic" mean, and how does one SaveChangesAsync give it? (answered: all or nothing; EF sends all pending changes in one transaction)
- TRAN vs TRY...CATCH? (answered: TRAN = one unit, all-or-nothing; TRY...CATCH = error handler; the CATCH must run ROLLBACK because the transaction does not undo itself)
- What happens if one statement in a transaction fails? (answered: jump to CATCH, ROLLBACK undoes earlier statements too)
- What happens if a transaction never gets COMMIT or ROLLBACK? (answered: stays open holding locks; rolled back if the connection drops)
- Two requests book the same seat at the same instant: who wins? (answered: the DB locks the index key, one gets the lock first, the other waits and then fails with a duplicate key; exactly one winner, 409 for the loser; add: the C# check stops the late ones, the index stops the simultaneous ones)
- What is a race condition? (answered: several requests compete and the result depends on who finishes first)
- Why isn't a C# check enough to prevent double booking? (answered: two requests can both pass the check before either saves; the DB index is the final guard)
- How would you test a race condition? (answered: parallel requests with Task.WhenAll, many more than 2, then count status codes and check the DB)
- Why does Task.WhenAll make requests parallel, and what if you await inside a foreach? (answered: Select only starts the tasks, WhenAll waits for all; await in a loop runs them one by one, so no race)
- What is a service class and why use one? (answered: holds the business logic; controller stays thin and only handles HTTP)
- What is an interface? (partly: a contract listing method names with no code)
- What is dependency injection, and what does AddScoped do? (partly: controller asks for the interface, .NET gives the registered class; Scoped = one instance per request)
- Is this MVC? (answered: no Views, so a Web API with controllers; layered = controller, service, data)
- What is an MVC View? (answered: the Razor page that shows data, not a SQL view)
- Why must a service not return NotFound() or Conflict()? (answered: those are HTTP; the service returns a result label and the controller maps it)
- Enum vs record? (answered: enum = one name from a fixed list; record = small container of several values, e.g. result + booking id)
- Why does the user id come from the token and not the request body? (answered: token is signed, client can't change it, so nobody can book or pay as someone else)
- How do you stop users touching other users' data? (answered: compare the owner id on the row with the user id from the token)
- 403 or 404 when the booking belongs to someone else? (answered: 404 hides that the id exists; 403 is acceptable if explained)
- What is idempotency, and what happens if a user pays twice at the same moment? (answered: both requests pass the C# pending check; the conditional UPDATE lets only one match the WHERE, the rest get 0 rows and 409; in a real system call the payment gateway only after the conditional update wins)
- How does a conditional UPDATE stop a double payment, and how does the second request "wait"? (answered: the UPDATE locks the row in SQL Server, the second waits for the lock, then re-checks the WHERE against the current row and matches 0 rows; await in C# only pauses the method)
- Optimistic vs pessimistic concurrency? (partly: optimistic = no lock while thinking, check at write time with a conditional WHERE or RowVersion; pessimistic = lock first, e.g. UPDLOCK)
- What is a RowVersion / concurrency token? (partly: a column SQL Server changes on every update; EF adds it to the WHERE and throws DbUpdateConcurrencyException for the loser)
- What does BeginTransactionAsync do, and why use tx? (answered: starts a transaction on the connection _db uses; commands between begin and commit join it; tx.CommitAsync makes it permanent; dispose without commit rolls back)
- Why update the booking and the seat in one transaction? (answered: all or nothing, so a booking is never CONFIRMED while the seat is still HELD)
- What does ExecuteUpdateAsync do, and how is it different from Find + SaveChangesAsync? (partly: runs one SQL UPDATE immediately and returns rows changed, no tracking)
- Why add a HELD seat status and not use BOOKED for unpaid seats? (answered: BOOKED is final, HELD can still be released or expire)
- How do you expire unpaid holds? (answered: find PENDING_PAYMENT bookings older than N minutes, set EXPIRED and free the seats in one save; a BackgroundService runs it on a timer; tested)
- What is a BackgroundService, and why does it need IServiceScopeFactory? (answered: a BackgroundService is a singleton that lives for the whole app; scoped services like AppDbContext live per request, so the worker creates its own scope each round to get a fresh IBookingService; the scope is disposed after use)
- How do you stop a background loop cleanly? (answered: the stoppingToken is cancelled on shutdown; loop on !IsCancellationRequested and pass the token to Task.Delay so the pause ends early)
- Polling vs cron / scheduled job? (answered: our worker polls the DB every minute inside the app; cron or Hangfire runs a job at fixed times; short holds need frequent checks, nightly jobs suit non-urgent work)
- Why can't EF Core translate DateTime.UtcNow - maxAge in a query? (answered: it can't turn DateTime minus TimeSpan into SQL; compute a cutoff DateTime in C# first)
- How do you make a setting configurable without changing code? (answered: put it in appsettings.json, inject IConfiguration, read with GetValue<int>("Section:Key"); a Development file can override it; secrets do not go there)
- How do you keep test credentials out of the repo? (answered: environment variables, set per terminal)
- What is Swagger/OpenAPI? (to answer)
- What is a Web API, and why not let the client talk to the DB directly? (to answer)
- What is EF Core, and what is a DbContext? (partly)
- What is LINQ? (partly)
- FindAsync vs FirstOrDefaultAsync; Add vs SaveChangesAsync; async/await (answered)
- 200, 201, 204, 400, 404 (answered); 409 (answered: conflict with current state, e.g. duplicate email, delete blocked by FK, seat already booked, booking not pending; better than a raw 500)
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
- What is an end-to-end test (Playwright)? (answered: drives a real browser through the app, like a user; unit = one component, no server)
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
- Why do you need a null check after FirstOrDefaultAsync? (answered: it returns null when no row matches, and using it would crash with a 500)