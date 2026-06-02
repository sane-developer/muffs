# Muffs — Project Roadmap

## Phase 1 — Modular Monolith (C#)

### M0 — Walking Skeleton
**Goal:** Solo, in-process, no UI. Wire the existing expression engine to a minimal `GameSession`: generate an expression, take an answer from a console/test harness, score it, serve the next. One player, no opponent, no network.

**Done when:** You can play a solo round-loop in a console app. Proves the engine ↔ session boundary. Reuse the engine as-is — don't touch what already works.

---

### M1 — Two Players, In-Process
**Goal:** `GameSession` now holds two players' state; both submit answers (sequential/simulated input is fine); first-correct-or-scored rules; match ends after N rounds with a winner.

**Done when:** A full 2-player match resolves with a winner inside one process. This is where the match becomes your unit of concurrency.

---

### M2 — Real-Time Over the Network
**Goal:** ASP.NET Core host + a WebSocket (or SignalR) endpoint. Trivial matchmaking (pair the first two who connect). Two clients play a live match with real-time updates.

Frontend decision lands here — start with the dumbest possible thing (plain JS + WebSocket or minimal HTMX); React is optional and later.

**Done when:** Two browser tabs play Muffs against each other in real time, served by the monolith. This is the "it's real" milestone.

---

### M3 — Harden the Seams
**Goal:** Make Matchmaking, GameSession, ExpressionEngine, and Scoring clean modules with explicit interfaces. Handle the ugly bits: disconnects, answer races, round timeouts. Model the live match as a per-match async loop / Channel-based actor.

**Done when:** The monolith is robust and you can point at exactly where the match-session seam is. That seam is what gets cut in Phase 2.

---

## Phase 2 — Extract the Live-Match Service (Go)

### M4 — Define the Contract
**Goal:** Carve the boundary between "the rest" (matchmaking, identity, results, client-serving — stays C#) and the live-match service (holds match state, runs the loop, handles both players' real-time input — becomes Go).

Transport decision lands here: likely gRPC for the live low-latency path (excellent C#↔Go, supports streaming), a broker for async events (results, matchmaking). Pick when we see it concretely.

**Done when:** A written contract (proto/schema) exists for the match service.

---

### M5 — Reimplement the Match Loop in Go
**Goal:** Goroutine-per-match, channels for player events — the exact thing Go is for. One open call: whether the Go service requests expressions from the C# engine over the wire or gets fed them. Cleanest is it requests per round. Engine stays C#.

**Done when:** The Go match service runs a live match over the chosen transport, and two clients play exactly as in M2 — but the live loop is now Go.

---

### M6 — Operationalize the Split
**Goal:** Two deployables, real cross-language comms, logging/tracing across the boundary, partial-failure handling (match service down, etc.).

**Done when:** A genuine two-service, two-language system — and you've felt both the power and the tax of distribution firsthand.

---

## The Arc

```
Monolith → Working real-time game → Clean seams → One Go service → Real distributed system
```

Nothing abstract until you've already built the concrete version of it.
