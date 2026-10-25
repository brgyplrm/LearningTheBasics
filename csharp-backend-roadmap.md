# C# + Backend Career Roadmap

**Goal:** become an employable backend / software developer on C# and .NET, with enough security and systems depth to keep full stack and cybersec/networking open as options.

**MaChip's role:** the capstone. You rebuild it after you understand the whole stack, not while learning it.

## How every phase works (anti-tutorial-hell loop)

1. **Learn the concept** from official docs or one book chapter (~30 min).
2. **Build a small standalone project** for that phase. Keep it separate from MaChip and put it on GitHub with a real README.
3. **Get stuck, then look up only the gap.**
4. **Break it on purpose** (bad input, concurrency, killed DB) and fix it.
5. **Checkpoint:** can you explain it and redo it without help? If not, repeat before moving on.

**Rule:** no new video until the last thing you watched has become code.

## Parallel tracks (all phases)

| Track | Weekly time | What |
|---|---|---|
| Algorithms and data structures | 2–3 sessions | Exercism C# first, then LeetCode (easy, then medium). Arrays, hash maps, trees, graphs, sorting, complexity. |
| Cybersecurity | 1–2 sessions | TryHackMe (pre-security, then web fundamentals, OWASP) and OverTheWire (Bandit first). Doubles as Linux and networking practice. |
| Linux and tooling | ongoing | Fedora daily driver, shell scripting, Git workflow (branches, PRs, rebase). |
| Rust | later | Start after Phase 3, a few hours per week. Systems-level thinking will deepen your C# memory and concurrency understanding. |

## Setup (day 1)

- [ ] Install the current LTS SDK: `sudo dnf install dotnet-sdk-<LTS version>`
- [ ] Rider or VS Code with C# Dev Kit
- [ ] GitHub profile, with a profile README and a repo per project from here on

---

## Phase 0: Foundations (weeks 1–2, overlaps Phase 1)

**Roadmap:** Internet, HTTP, DNS, domains, hosting, browsers, version control

- Trace a request end to end: browser → DNS → TCP/TLS → server → response. Use `dig`, `curl -v`, and Wireshark.
- Linux basics, processes, ports, `ss`, `systemd`.
- **Project:** a write-up (blog post or README) explaining what happens when you type a URL, with your own packet captures.

## Phase 1: C# language (weeks 2–6)

**Roadmap:** Pick a Language

**Learn:** types, records vs classes, nullable reference types, collections, LINQ, `async`/`await` and `Task`, generics, delegates and events, `IDisposable`, pattern matching, exceptions, unit testing with xUnit.

**Resources:** Microsoft Learn C# docs, *The C# Player's Guide* (RB Whitaker), Exercism C# track.

**Projects:**
- CLI expense tracker with file persistence and tests.
- Text/CSV parser and report generator (good LINQ practice).

**Checkpoint:** explain value vs reference types, `async` state machines at a high level, and when to use a record vs a class.

## Phase 2: HTTP and APIs (weeks 7–9)

**Roadmap:** REST, JSON APIs, OpenAPI, gRPC (awareness), GraphQL (awareness)

**Learn:** ASP.NET Core minimal APIs and controllers, routing, model binding, validation, middleware, dependency injection, options pattern, error handling (ProblemDetails).

**Project:** URL shortener or notes API, in-memory first, documented with OpenAPI.

**Checkpoint:** explain the middleware pipeline and DI lifetimes (singleton, scoped, transient).

## Phase 3: Databases (weeks 10–14)

**Roadmap:** PostgreSQL, relational DBs, normalization, ACID, transactions, indexes, N+1, migrations, ORMs, NoSQL overview

**Learn:** plain SQL first (joins, aggregates, window functions, `EXPLAIN ANALYZE`), then EF Core with Npgsql, migrations, change tracking, Dapper for hot paths.

**Project:** booking or library system (users, resources, reservations). Needs constraints, transactions, and a concurrency problem to solve (double booking).

**Reference:** *Designing Data-Intensive Applications*, storage and transactions chapters.

**Checkpoint:** design a schema in 3NF, explain isolation levels, and fix an N+1 and a missing index with evidence.

## Phase 4: Auth and security (weeks 15–17)

**Roadmap:** authentication (JWT, cookies, OAuth/OIDC), hashing (bcrypt, scrypt, Argon2id), HTTPS/TLS, CORS, CSP, OWASP Top 10, API security

**Learn:** ASP.NET Core Identity, JWT bearer, policy-based authorization, rate limiting, secrets management.

**Project:** add full auth, roles, and an audit log to your Phase 3 project, then attack it yourself (injection, broken access control, IDOR).

**Checkpoint, decision point:** after this phase, note which parts you enjoyed most.
- Building features and data modelling → backend or full stack
- Breaking things and networking → cybersec or networking
- Both → backend with a security focus (a strong, employable combination)

## Phase 5: Testing, CI/CD, containers, deploy (weeks 18–21)

**Roadmap:** unit, integration, and functional testing, CI/CD, Docker, web servers (Nginx/Caddy), twelve-factor apps

**Learn:** Testcontainers with real Postgres, GitHub Actions, multi-stage Dockerfile, reverse proxy, environment config.

**Project:** deploy your Phase 3/4 project to a VPS behind Caddy with CI that tests, builds, and ships on push.

## Phase 6: Caching, messaging, real-time, background work (weeks 22–26)

**Roadmap:** Redis, message brokers (RabbitMQ, Kafka awareness), WebSockets, SSE, long/short polling, caching strategies

**Learn:** `BackgroundService`, `Channel<T>`, SignalR, Redis caching, RabbitMQ basics, idempotency.

**Project (pick one):**
- Job queue with retries and a dead-letter queue.
- Live notifications/chat service.
- **Hardware option:** RFID tap ingest pipeline (reader → MQTT/serial → validated, idempotent write → live dashboard). This is a strong portfolio piece that shows hardware and software together.

## Phase 7: Observability, scale, architecture (weeks 27–30 and ongoing)

**Roadmap:** monitoring, logging, telemetry, instrumentation, scaling types, replication, sharding, CAP, circuit breaker, throttling, backpressure, design patterns, DDD, CQRS, monolith vs microservices

**Learn:** OpenTelemetry, Serilog structured logging, load testing (k6), clean/modular architecture.

**Rule:** study scale topics as reading and small experiments. Do not microservice your projects without a real problem.

## Phase 8: Capstone, the MaChip rebuild (weeks 31+)

By now you know what each piece is for, so MaChip becomes an exercise in applying it.

- Reuse the Node version as a behavioural reference, and write characterization tests against it first.
- Rebuild the normalized schema with EF Core migrations, payroll rules as pure tested C# domain logic, auth and audit, RFID ingest, CI/CD, observability.
- Optionally run both versions against the same Postgres and move endpoints over one at a time (strangler pattern).
- **Write a case study:** what you changed, why, and what you measured (query times, test coverage, throughput).

---

## Career track (alongside the technical work)

**Portfolio (target 3–4 solid projects, not 20)**
1. The Phase 3/4 API, deployed and documented.
2. The Phase 6 project (RFID pipeline if possible).
3. The MaChip rebuild with its case study.
4. One security write-up (TryHackMe walkthrough or your own pentest of your API).

**Each repo needs:** README with purpose, architecture diagram, how to run, and tradeoffs; tests; CI badge.

**Opportunities**
- Use your PUP network for OJT/internship, and look for .NET shops in the Philippines.
- Start applying for internships or junior roles around Phase 5–6. You do not need to finish the roadmap first.
- Contribute small fixes to open-source .NET projects once you are comfortable (docs fixes count).

**Certifications (optional, lower priority than projects)**
- Backend/cloud: AZ-900 then AZ-204.
- Security: CompTIA Security+ or eJPT if you lean cybersec.

**Habits**
- Keep a learning log: one short note per week (what you built, what broke).
- Read other people's code on GitHub weekly.
- Practice explaining your projects aloud. Interviews test that.

## Suggested weekly rhythm (adjust around school)

| Day | Focus |
|---|---|
| Mon, Wed | Concept study + build the phase project (2–3 h) |
| Tue, Thu | Algorithms (1 h) + TryHackMe/OverTheWire (1 h) |
| Fri | Break it / refactor / write tests |
| Sat | Longer project block, README and notes |
| Sun | Rest, or light reading (DDIA, docs) |

Timelines are guides. Move on when the checkpoint passes, not when the calendar says so.
