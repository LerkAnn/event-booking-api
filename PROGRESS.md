# PROGRESS.md

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
- Never cut: booking logic, auth, concurrency test, Docker Compose, system design basics
- Cut first if behind: CI, deploy, Hangfire, LeetCode
- If early: add a stretch task inside the current week, don't jump ahead
- Resign only after the bonus is paid in Feb (2-month notice)
- Sessions: 3x40 min weekdays + 1x90 min weekend; 15 min minimum on bad days
- Calendar note: Week 3 started 26 Oct. Full-day study sessions are planned with breaks (not cramming).
- Schedule note: Week 4 stays 4 sessions + 1 extra setup session; worst case slips into Week 5, but auth and tests are not cut.

## Done
- Week 0: GitHub, .NET 9, Docker, SSMS set up. Repo created and pushed.
- Week 1, Sessions 1-4: SQL Server 2025 in Docker (sql1), EventBooking DB, 7 tables with FKs, schema.sql, seed.sql, JOINs, PK/UNIQUE/composite key quiz.
- Week 2, Sessions 1-3: GROUP BY/HAVING, indexes (scan vs seek), BEGIN TRAN/COMMIT/ROLLBACK, ACID.
- Week 3, Session 1: created .NET project EventBooking.Api.
- Week 3, Session 2: EF Core 9, user-secrets connection string, Event entity, AppDbContext, GET /events.
- Week 3, Session 3: GET by id, POST, Swagger UI, IDENTITY gaps, reseeded Events.
- Week 3, Session 4: walked through Program.cs, deleted WeatherForecast code, converted to EventsController (GET list, GET by id, POST, DELETE written by me; PUT code received), learned AddControllers vs MapControllers, [ApiController], [Route], dependency injection basics, path vs query parameters.

## Current
- Week 3: CRUD APIs, validation, error handling, Swagger, controllers. Remaining: run/test PUT and DELETE, confirm validation attributes, basic error handling.

## Next
- Week 3 leftovers (first 15-20 min of next session):
  1. Change Delete to [HttpDelete("{id}")] and return NoContent(); add PUT (Update) to the controller; test both in Swagger on the TEST EVENT (not COMIC FIESTA, which has seats)
  2. Add [Required, StringLength(200)] on Name and [Range(1, int.MaxValue, ErrorMessage = "TotalSeats must be greater than 0")] on TotalSeats in Event.cs; confirm totalSeats 0 and empty name return 400
  3. Basic error handling: clean error response instead of a raw 500 (can slide to Week 4)
  4. Commit and push
- Study on my own: what CreatedAtAction does (builds the Location URL from GetById's route; does not run GetById)
- Confirm done: CK_Events_total_seats ALTER line and "CREATE INDEX" typo fix in schema.sql
- Confirm done: TRY...CATCH script (Z11 booking + Z12 break test) run
- Week 4, Session 0 (extra setup): install/check Visual Studio Community (workload: ASP.NET and web development), create solution (dotnet new sln -n EventBooking, dotnet sln add EventBooking.Api), open .sln in VS
- Week 4: JWT auth + roles + BCrypt + [Authorize] + first xUnit tests
- Blank-file challenge (weekend): write the TRY...CATCH template from memory
- Leftover (cut first): SQLBolt lessons 1-6

## Project state
- Repo: event-booking-api: PROGRESS.md, schema.sql, seed.sql, EventBooking.Api (Models/Event.cs, Data/AppDbContext.cs, Controllers/EventsController.cs, Program.cs)
- Tools: .NET 9, Docker Desktop, SSMS, VS Code (moving to Visual Studio in Week 4)
- Run it: start Docker Desktop + sql1, dotnet run --project EventBooking.Api, open printed URL + /swagger in the normal browser, Ctrl+C to stop
- Endpoints: GET /api/events, GET /api/events/{id}, POST /api/events, PUT /api/events/{id}, DELETE /api/events/{id}
- Connection string in user-secrets, not in the repo
- Main tables: Events, Roles, Users, SeatStatuses, BookingStatuses, Seats, Bookings
- Indexes: IX_Bookings_seat_id, IX_Bookings_user_id
- Sample data: seed.sql; Events reseeded manually (3 events)
- Risk: sql1 has no volume; deleting the container loses the tables. Rebuild with CREATE DATABASE EventBooking + schema.sql + seed.sql. Fix with a volume in Docker Compose (Week 7)

## Decisions
- Stack: .NET 9, SQL Server 2025 image, EF Core 9 packages
- snake_case DB columns; C# PascalCase with [Table]/[Column]; no [Key] needed (EventId follows ClassName+Id)
- IDENTITY(1,1) primary keys; gaps are normal
- Passwords hashed with BCrypt (Week 4)
- Controllers (not minimal APIs); REST-style routes: [HttpGet], [HttpGet("{id}")], [HttpPost], [HttpPut("{id}")], [HttpDelete("{id}")] under [Route("api/events")]
- Validation: attributes on the model give a friendly 400 first; DB CHECK constraint is the safety net
- Logic will move to service classes around Week 5; controllers stay thin
- Secrets in user-secrets, never in the repo
- Use Swagger UI to test

## Stuck on
- Double-booking rule: UNIQUE on seat_id would block rebooking after cancel; decide in Week 5
- CreatedAtAction: still studying (see Next)

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
- What is a primary key? (answered)
- Primary key vs UNIQUE? (answered)
- What is a composite key? (answered)
- IDENTITY vs GUID as primary key (partly)
- IDENTITY vs AUTO_INCREMENT (answered)
- Why are there gaps in IDENTITY values? (answered)
- What is T-SQL? (answered)
- What is a foreign key and why use one? (answered; add: default blocks deleting a parent row, ON DELETE CASCADE removes children)
- How should you store passwords, and what is a salt? (answered)
- Lookup table vs CHECK constraint for statuses? (to answer)
- DELETE vs TRUNCATE? (answered)
- What is GO? (to revisit)
- WHERE vs HAVING? (answered)
- How does GROUP BY work with COUNT(*)? (answered)
- What is an index, and the downside of too many? (answered)
- Scan vs seek? (answered)
- Clustered vs non-clustered index? (to revisit)
- A 10-million-row query is slow, what first? (answered)
- What is a transaction? (answered)
- What does ACID stand for? (partly)
- TRAN vs TRY...CATCH? (to revisit)
- What happens if one statement in a transaction fails? (to revisit)
- What is Swagger/OpenAPI? (to answer)
- What is a Web API, and why not let the client talk to the DB directly? (to answer)
- What is EF Core, and what is a DbContext? (partly)
- What is dependency injection? (to answer)
- What is LINQ? (partly)
- FindAsync vs FirstOrDefaultAsync? (answered)
- Add vs SaveChangesAsync? (answered)
- async/await? (answered)
- 200, 201, 400, 404, 204? (answered; 204 = success with no body)
- API validation vs DB constraint? (answered: API returns friendly 400, DB CHECK is the last safety net)
- Controllers vs minimal APIs? (partly: controllers are class-based with attributes)
- Path vs query parameter? (answered: path = which one thing, query = filter/search)
- What do AddControllers and MapControllers do? (answered: Add registers the controller system before Build; Map turns attributes into URLs after Build)
- What does [ApiController] do? (partly: switches on API behaviours like automatic 400 on invalid body)
- How does .NET know which environment it is in? (answered: ASPNETCORE_ENVIRONMENT; default Production)
- What is xUnit? (answered)
- How do you keep DB passwords out of source control? (answered: user-secrets locally)
- What does git add do; staging vs committing? (to answer)