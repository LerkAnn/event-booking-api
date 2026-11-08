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
- Schedule note: Week 4 sessions 0-4 done (auth + roles work). The xUnit tests, friendly error handling and blank-file challenge moved to Week 5, Session 1 (tests first; they are never cut). Booking logic starts after that.

## Done

- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1: SQL Server 2025 in Docker (sql1), EventBooking DB, 7 tables with FKs, schema.sql, seed.sql, JOINs, PK/UNIQUE/composite key.
- Week 2: GROUP BY/HAVING, indexes (scan vs seek), BEGIN TRAN/COMMIT/ROLLBACK, ACID.
- Week 3: .NET project, EF Core 9, user-secrets connection string, Event entity, AppDbContext, EventsController with GET list, GET by id, POST, PUT, DELETE (tested in Swagger), validation attributes ([Required], [StringLength], [Range]) returning 400, Swagger UI, Program.cs understood (AddControllers vs MapControllers), controllers vs minimal APIs, path vs query parameters. Committed and pushed.
- Week 4, Session 0: Visual Studio 2022 set up, EventBooking.sln created and opened, API runs from VS (https profile, https://localhost:7032/swagger). Understood why there are two ports (http 5166, https 7032, from launchSettings.json) and why https on the http port fails.
- Week 4, Session 1: BCrypt.Net-Next package added; User entity (Models/User.cs) + DbSet<User>; RegisterRequest DTO (Dtos/RegisterRequest.cs); AuthController with POST /api/auth/register (hash with BCrypt, RoleId = 1 set by server, returns only UserId/Name/Email); duplicate email check returns 409 Conflict; tested in Swagger, hash confirmed in DB. Understood entity vs DTO, hash vs encryption, constructor injection, Controller vs ControllerBase. Committed and pushed.
- Week 4, Session 2: JwtBearer package added; Jwt:Key stored in user-secrets; LoginRequest DTO; POST /api/auth/login (FirstOrDefaultAsync by email, BCrypt.Verify, same 401 message for unknown email and wrong password). Understood why hashes can't be compared with == (random salt), authentication vs authorization, user enumeration. Got the full login code after being stuck; will retype from memory in the blank-file challenge. Committed and pushed.
- Week 4, Session 3: login builds and returns a JWT (claims: NameIdentifier = user id, Email; expires in 1 hour; HmacSha256; key from Jwt:Key via IConfiguration injected in AuthController). Program.cs: AddAuthentication + AddJwtBearer (validates signature and lifetime), UseAuthentication, UseAuthorization before MapControllers; Swagger Authorize button (Bearer scheme). GET /api/auth/me with [Authorize]. Tested: no token = 401, valid token = 200. Understood JWT = header.payload.signature, payload is encoded not encrypted, signature makes it trustworthy, stateless auth. Committed and pushed.
- Week 4, Session 4: Role entity (Models/Role.cs, generated as boilerplate) + DbSet<Role>; login looks up the role code and adds ClaimTypes.Role to the token; /me returns userId, email, role; zorus2@gmail.com made ADMIN with SQL UPDATE; second CUSTOMER user registered; [Authorize(Roles = "ADMIN")] on POST/PUT/DELETE events, GET stays public. Tested: no token = 401, customer = 403, admin = 200/201/204, GET public. Register with 3-char password / bad email returns 400 (checked). Understood 401 vs 403, role-based authorization, why the role is a token claim (no DB call per request) and its downside (stale until the token expires).

## Current

- Week 4 auth and roles done. Remaining Week 4 items (xUnit tests, friendly error handling, blank-file challenge) moved to Week 5, Session 1.

## Next

- Week 5, Session 1: first xUnit tests (tests first, never cut); small friendly error handling (delete event blocked by foreign key -> 409 instead of raw 500, try/catch DbUpdateException in Delete); blank-file challenge (retype login + token generation from memory; TRY...CATCH template from memory). Start booking logic after if there is time.
- Week 5: booking logic, double-booking prevention, concurrency test
- Study on my own: what CreatedAtAction does (builds the Location URL from GetById's route; does not run GetById)
- Confirm done: CK_Events_total_seats ALTER line and "CREATE INDEX" typo fix in schema.sql
- Confirm done: TRY...CATCH script (Z11 booking + Z12 break test) run
- Optional stretch: call /me with a tampered token (change one character) and see the 401
- Later: replace hard-coded RoleId = 1 with a lookup by role code
- Later (production): set ValidateIssuer/ValidateAudience with real values; use a different Jwt:Key from a secret store
- Leftover (cut first): SQLBolt lessons 1-6

## Project state

- Repo: event-booking-api: PROGRESS.md, schema.sql, seed.sql, EventBooking.sln, EventBooking.Api (Models/Event.cs, Models/User.cs, Models/Role.cs, Dtos/RegisterRequest.cs, Dtos/LoginRequest.cs, Data/AppDbContext.cs, Controllers/EventsController.cs, Controllers/AuthController.cs, Program.cs)
- Tools: .NET 9, Docker Desktop, SSMS, Visual Studio 2022 (run/debug C#), VS Code (SQL files, quick edits)
- Packages: EF Core 9 (SqlServer), BCrypt.Net-Next, Microsoft.AspNetCore.Authentication.JwtBearer 9.x (wired up in Program.cs)
- Run it: start Docker Desktop + sql1, open EventBooking.sln in VS, pick https profile, F5, go to https://localhost:7032/swagger. Stop the app before renaming files.
- Test with a token: POST /api/auth/login, copy the token without quotes, click Authorize in Swagger, paste, Authorize, Close. Log in again after any role change (old tokens keep the old role).
- Endpoints: GET /api/events (public), GET /api/events/{id} (public), POST/PUT/DELETE /api/events ([Authorize(Roles = "ADMIN")]); POST /api/auth/register; POST /api/auth/login (returns { token }); GET /api/auth/me ([Authorize])
- User-secrets: ConnectionStrings:Default, Jwt:Key (never in the repo, never pasted in chats)
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Users table: user_id, name, email (unique), password_hash, phone_number (nullable), role_id (FK to Roles), created_at, updated_at. Roles seeded: CUSTOMER = 1, ADMIN = 2
- Test users: zorus@gmail.com (seed, fake hash, cannot log in); zorus2@gmail.com (ADMIN, can log in); a second registered user (CUSTOMER)
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id
- Risk: sql1 has no volume; deleting the container loses the tables. Rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7). After a rebuild, re-register users and re-promote the admin with SQL UPDATE.
- Risk: RoleId = 1 is hard-coded; breaks if Roles are re-seeded with different ids
- Risk: deleting an event that has seats/bookings gives a raw 500 (FK); fix in Week 5, Session 1

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
- Flat early-return style in controller methods (no deep nested ifs)
- No EF migrations or seed code; schema.sql and seed.sql by hand
- Logic moves to service classes around Week 5; controllers stay thin
- Secrets in user-secrets, never in the repo (JWT key too)
- Test with Swagger UI

## Stuck on

- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5
- CreatedAtAction: studying on my own
- Two requests registering the same email at the same moment: DB UNIQUE catches it (concurrency idea, Week 5)

## Weak spots

- SQL Server vs MySQL syntax; PowerShell vs Linux syntax
- Clustered vs non-clustered index (basic idea only)
- TRY...CATCH + transaction: can't write it from scratch yet
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
- What is a transaction? (answered); ACID (partly: A and C seen)
- TRAN vs TRY...CATCH? (to revisit)
- What happens if one statement in a transaction fails? (to revisit)
- What is Swagger/OpenAPI? (to answer)
- What is a Web API, and why not let the client talk to the DB directly? (to answer)
- What is EF Core, and what is a DbContext? (partly)
- What is dependency injection? (partly: seen constructor injection of AppDbContext and IConfiguration)
- What is LINQ? (partly)
- FindAsync vs FirstOrDefaultAsync; Add vs SaveChangesAsync; async/await (answered)
- 200, 201, 204, 400, 404 (answered); 409 (answered: duplicate email)
- 401 vs 403? (answered: 401 = not authenticated (no/bad/expired token); 403 = known user but not allowed, e.g. CUSTOMER on an admin endpoint)
- API validation vs DB constraint? (answered)
- What is a DTO and why use one? (answered: doorway for client input, keeps entity/DB shape private, blocks extra fields like roleId)
- Entity vs DTO? (answered)
- Controller vs ControllerBase? (answered: API uses ControllerBase)
- Controllers vs minimal APIs? (partly)
- Path vs query parameter? (answered)
- AddControllers vs MapControllers; [ApiController] (answered / partly)
- How does .NET know which environment it is in? (answered: ASPNETCORE_ENVIRONMENT)
- What is xUnit? (answered)
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
