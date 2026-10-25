# LearningTheBasics: C# Core Systems & Machine Problems

A structured, foundational repository for mastering modern C# programming, architectural patterns, algorithmic problem solving, and system design through real-world machine problems.

---

## Repository Architecture

```text
LearningTheBasics/
├── .gitignore                       # Build binary and temporary file filter
├── README.md                        # Master repository documentation
├── Program.cs                       # Central launcher dispatch switch
├── learning.csproj                  # .NET 8.0 SDK project manifest
│
├── Machine_Problems/                # Problem specifications, prompts, and published PDFs
│   ├── Milestone1_Machine_Problem.pdf
│   ├── Milestone2_Machine_Problem.pdf
│   ├── Milestone3_Machine_Problem.pdf
│   ├── Cybersecurity_IDS_Machine_Problem.pdf
│   └── FinTech_Remittance_Machine_Problem.pdf
│
├── Projects/                        # Source implementations grouped by problem
│   ├── Milestone1_Logistics/
│   │   └── milestone1.cs            # Milestone 1: Cargo Evaluation & Dispatch System
│   ├── Milestone2_ShippingManifest/
│   │   ├── Milestone2.cs            # Milestone 2: Audited Shipping Manifest Dispatcher
│   │   ├── CargoCalculation.cs     # Calculation Engine (dimensional weight & fees)
│   │   └── FileReadingIO.cs         # Batch Ingestion and Export IO tier
│   └── Milestone2_Cybersecurity_IDS/
│       ├── MileStoneMachine2.cs     # Aegis-SOC: Interactive IDS Terminal Entry Point
│       ├── SecurityCore.cs          # ThreatEngine (Tokenization, Signatures, Brute-Force)
│       └── SecurityIO.cs            # ManifestSecIO (Log Reader, Triage, Dossier & Exporter)
│
├── Notes/                           # Companion learning notes & architectural review
│   ├── Milestone1Notes.cs           # Variables, Conditionals & Logical Operators
│   ├── Milestone2Notes.cs           # Loops, Arrays, Methods & File Streams
│   ├── MilestoneMachine2Notes.cs    # Slicing, Temporal Analysis & Defensive Rules
│   ├── Milestone3Notes.cs           # Logic, Scope, Switch-Case, For/Do-While & Defensive Parsing
│   └── csharp-backend-roadmap.md    # C# & Backend Developer Learning Roadmap
│
└── Data/                            # Sample input logs, test manifests, and report outputs
    ├── batch_input.txt              # Sample batch data for Logistics Engine
    ├── remittance_batch_input.txt   # Sample remittance batch data
    ├── pet_inventory_batch.txt      # Sample batch inventory for Pet Sanctuary Engine
    ├── server_access.log            # Sample web access log for Aegis-SOC Engine
    └── security_incident_report.txt # Auto-generated incident report & firewall drop rules
```

---

## Machine Problems Overview

### 1. Milestone 1: Cargo Logistics Calculation Engine
* **Focus:** Console I/O, conditional logic, dimensional weight algorithms, currency formatting, defensive input handling.
* **Core Logic:** Evaluates chargeable weight (Actual vs Dimensional: `(L * W * H) / 5000`), customs holding rules, high-value insurance surcharge, and dimensional penalty surcharges.
* **Run:** Set `Program.cs` to `Milestone1.Run();`

### 2. Milestone 2: Audited Shipping Manifest Processor
* **Focus:** Multi-tier architectural separation (`Core` vs `IO`), loops, parallel arrays/lists, file stream reading and writing.
* **Core Logic:** Ingests batch manifests from text files, calculates batch summaries (total revenue, cleared, held, rejected), and exports formatted audit reports to disk.
* **Run:** Set `Program.cs` to `Milestone2.Run();`

### 3. Aegis-SOC: Cybersecurity Server Log Analyzer & Threat Detection Engine
* **Focus:** Advanced string tokenization, modern C# range slicing, substring pattern scanning, sliding-window temporal analysis, multi-factor risk classification.
* **Core Modules:**
  * **Method A (`ParseLogEntry`):** Tokenizes raw server access logs into named tuples using delimiter slicing (`Split('"')`, `Split('[')`, `Trim`).
  * **Method B (`ScanAttackSignatures`):** Multi-vector signature scanner detecting SQL Injection (SQLi), Path Traversal (LFI), and Automated Scanners (`sqlmap`, `nikto`).
  * **Method C (`AnalyzeBruteForceActivity`):** Temporal frequency analyzer inspecting sliding 60-second windows across historical authentication failures (`401` / `403`).
  * **Method D (`ClassifyIncidentSeverity`):** Escalates incidents based on threat score and brute-force status (`CRITICAL THREAT`, `HIGH RISK`, `SUSPICIOUS`, `BENIGN`).
  * **Method E (`GenerateThreatMatrix`):** Forensics aggregator identifying primary offending attacker IPs and incident distributions.
  * **Interactive SOC Terminal:** Features real-time manual log triage, automated log file ingestion, formatted dossier viewing, and automatic `iptables -A INPUT -s <IP> -j DROP` blacklist script generation.
* **Run:** Set `Program.cs` to `MileStoneMachine2.Run();`

### 4. Milestone 3: Paws & Whiskers — Pet Sanctuary, Adoption & Clinical Registry Terminal
* **Focus:** Advanced control flow, low-level delimiter parsing (`IndexOf`, `Substring`, `Remove`, `TrimStart`), stacked `switch-case`, conditional ternary operators (`?:`), in-place array mutation via indexed `for` loops, and reverse traversal.
* **Core Modules:**
  * **Method A (`ExtractTagToken`):** Extracts key-value tokens from raw barcode tags without regex or `.Split()`.
  * **Method B (`ScreenHealthStatus`):** Normalizes medical notes (`.Trim().ToLower()`), scans risk triggers with `.Contains()`, evaluates boolean negations (`!isVaccinated`), and assigns dietary plans.
  * **Method C (`CalculateAdoptionCosts`):** Multi-species fee routing via stacked `switch-case`, health subsidy calculation via ternary expressions, and VIP adopter discounts.
  * **Method D (`UpdateAdoptionLedger`):** Mutates parallel collection statuses in-place via an indexed `for` loop, proving why read-only `foreach` iterations fail.
  * **Method E (`GenerateSanctuarySummary`):** Aggregates intake volume, adoption rates, quarantine holds, and average days in care with divide-by-zero protection.
* **Specification Document:** [Milestone3_Machine_Problem.pdf](Machine_Problems/Milestone3_Machine_Problem.pdf)
* **Sample Batch Data:** [pet_inventory_batch.txt](Data/pet_inventory_batch.txt)

---

## Comprehensive Learning Notes

Each milestone includes a dedicated companion note file detailing mental models, language mechanics, and common beginner pitfalls:

1. [Milestone1Notes.cs](Notes/Milestone1Notes.cs)
   * Variables, data types (`decimal` vs `double`), type conversions (`TryParse`).
   * Decision structures (`if`, `else if`, `switch`, ternary operator).
   * Short-circuit evaluation (`&&`, `||`).

2. [Milestone2Notes.cs](Notes/Milestone2Notes.cs)
   * Loops: `for`, `foreach`, `while`, `do-while`, loop control (`break`, `continue`).
   * Collections: Fixed arrays (`string[]`) vs dynamic collections (`List<T>`).
   * Modular methods, parameter passing, return types (`void` vs typed returns).
   * Namespaces, using directives, and file handling (`File.ReadAllLines`, `StreamWriter`).

3. [MilestoneMachine2Notes.cs](Notes/MilestoneMachine2Notes.cs)
   * Why printing `string[]` outputs `"System.String[]"` (Object `.ToString()` mechanics).
   * Modern C# Index (`^1`) and Range (`1..^1`) operators compared with Python slicing.
   * Substring searching (`.Contains()`, `.IndexOf()`) vs exact equality (`==`).
   * Sliding-window frequency analysis with `DateTime` and `TimeSpan`.
   * State management: passing collection references from `Main` to `.IO`.
   * Tally counter search patterns vs premature return traps.

4. [Milestone3Notes.cs](Notes/Milestone3Notes.cs)
   * Advanced Boolean evaluation, string normalization (`Trim()`, `ToLower()`), and logical negation (`!`).
   * Conditional ternary expressions (`?:`) and syntax rules inside string interpolation.
   * Variable scope boundaries `{}`, lifetime, CS0103 context errors, and CS0165 definite assignment analysis.
   * Microsoft style standards for omitting curly braces and formatting compound if-else statements.
   * Multi-case matching in `switch-case`, fall-through prevention, and `default` evaluation.
   * Structural anatomy of `for` loops (6 parts, step patterns, reverse iteration, array mutation vs `foreach` read-only constraint).
   * `do-while` (post-condition guarantee) vs `while` (pre-condition gatekeeper), `continue` vs `break`.
   * Defensive input pipelines: non-crashing `int.TryParse`, `string?` null safety, and normalized whitelisting.
   * Low-level string delimiter extraction using `IndexOf()`, `Substring()`, `Remove()`, and `TrimStart()`.

---

## Core Software Engineering Practices to Keep in Mind

### 1. The Assembly Line Pattern (Separation of Concerns)
* **Main Entry Point:** Owns data containers (`List<T>`), controls user workflow, and stays alive for the entire session.
* **I/O Tier (`.IO`):** Handles physical file operations and terminal presentation. Does not perform complex mathematical or business logic.
* **Core Engine (`.Core`):** Pure computation, string parsing, and algorithm execution. Independent of files, sockets, or user interfaces.

### 2. State Management & Reference Passing
* Parameters in methods are requirement contracts, not allocated storage.
* `new List<T>()` allocates heap memory. Passing that list into `.IO` hands over the memory reference, allowing the method to populate data that persists after the method completes.

### 3. Defensive Programming & Guard Clauses
* Always check for null or whitespace (`string.IsNullOrWhiteSpace(rawLine)`) **before** running operations like `.Split()` to prevent `NullReferenceException` crashes.
* In decision trees, evaluate numeric boundaries using a **descending ladder** (`>= 85`, then `>= 65`, then `>= 30`) to avoid redundant compound expressions.

### 4. Search Loop Integrity
* Never place an `else return` inside a search loop. Loops must inspect all items before concluding that a match does not exist.
* Use a tally counter (`count++`) during iteration, and evaluate trigger thresholds (e.g. `count >= 5`) only after the loop completes.

---

## How to Build and Run

### Prerequisites
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) or higher

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run
```
Select the desired milestone runner by toggling comments in [Program.cs](Program.cs):
```csharp
// Milestone1.Run();
// Milestone2.Run();
MileStoneMachine2.Run();
```

---

## Engineering Methodology: Deliberate Pair Programming vs. "Vibe Coding"

A core pillar of this repository is a commitment to **deliberate learning and architectural mastery**, actively rejecting the trap of superficial "vibe coding" (blindly accepting and pasting AI-generated code without understanding the underlying mechanics).

### The Human-AI Pair Programming Contract

1. **Human Driver (Architect & Core Logic Author):**
   * Writes the core business logic, algorithm formulas, loops, and decision trees.
   * Challenges syntax, demands explanation of underlying runtime behavior (e.g., why `System.String[]` prints instead of array elements, how `1..^1` range slicing functions in memory, why state must live in `Main`).
   * Refuses to accept "magic" code dumps; insists on understanding *why* an error occurred before applying a fix.
   * Controls design choices, state ownership, and pipeline workflow.

2. **AI Companion (Code Reviewer, Flaw Detector & Boilerplate Assistant):**
   * **Flaw Detection & Auditing:** Reviews human-written code for subtle bugs, boundary mismatches, execution order traps (e.g., sliding-window timing, guard clause ordering before `.Split()`), and search loop integrity.
   * **Mental Models & Pedagogical Guidance:** Explains C# runtime mechanics using generic conceptual examples without spoon-feeding direct solutions.
   * **Boilerplate & UI Scaffolding:** Constructs repetitive, mechanical components (console table alignment, menu dispatch loops, file export writers) when the human chooses to delegate layout work and focus on algorithm design.
   * **Living Documentation:** Compiles and maintains extensive technical notes, discoveries, and audit logs directly within the repository.

### Why This Workflow Works
By treating AI as an interactive pair-programmer and strict auditor rather than an autopilot code generator:
* No code enters this repository without the developer understanding every single line, variable, and memory reference.
* Bugs become valuable learning discoveries rather than mysterious roadblocks.
* The codebase remains robust, maintainable, and built on genuine software engineering discipline.

---

## License
MIT - Academic and educational use.
