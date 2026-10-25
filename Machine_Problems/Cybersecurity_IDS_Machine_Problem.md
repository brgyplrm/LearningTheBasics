# C# Machine Problem: Automated Cybersecurity Intrusion Detection System (IDS) & Log Forensics Engine

## Problem Scenario
You are stationed as a Security Operations Center (SOC) incident response engineer at a financial technology infrastructure provider. Overnight, automated alarms triggered on the perimeter reverse proxy. The primary web application server was targeted by an adversarial campaign consisting of automated vulnerability scanning, SQL injection exploitation probes, directory path traversals, and distributed credential brute-force attacks.

The Chief Information Security Officer (CISO) requires you to build a standalone, modular, console-based **Intrusion Detection System (IDS) & Forensics Analysis Engine** in C#. 

Unlike simple CSV batch applications, this system ingests **unstructured, real-world server access log files**, parses dynamic HTTP tokens, executes multi-vector signature pattern matching, tracks temporal frequency spikes using `DateTime` calculations to detect brute-force attacks, evaluates threat severity ratings, and exports an actionable **Security Incident Response Dossier** with firewall mitigation commands.

---

## Learning Competencies Assessed
* **Advanced Text Tokenization & Parsing:** Extracting structured fields (IP, timestamps, HTTP verbs, URIs, status codes, user-agents) from semi-structured log syntax using string slicing (`IndexOf`, `Substring`, `Split`).
* **Temporal Calculations (`DateTime`):** Parsing real timestamps (`DateTime.ParseExact`), computing elapsed intervals (`TimeSpan`), and detecting rapid-fire brute-force threshold breaches (e.g., $\ge 5$ failed logins within a 60-second window).
* **Cybersecurity Threat Pattern Matching:** Detecting adversarial signatures:
  * SQL Injection (SQLi): `' OR '1'='1`, `UNION SELECT`, `SLEEP(`, `--`, `INFORMATION_SCHEMA`.
  * Path Traversal (LFI): `../`, `..%2f`, `/etc/passwd`, `win.ini`.
  * Malicious Security Scanners: `sqlmap`, `nikto`, `nmap`, `gobuster`, `masscan`.
* **State Machine & Collections Management:** Aggregating offending IPs across parallel `List<T>` collections, maintaining attacker request counts, and computing dynamic threat severity tiers.
* **Modular Methods & Named C# Tuples:** Structuring forensics operations into pure static methods returning multi-value tuples.
* **Defensive Resource Management:** Deterministic stream handling (`using (StreamReader)` / `using (StreamWriter)`) guaranteeing no file lock retention on critical server logs.
* **Namespace Architecture:** Decoupling detection algorithms (`SecurityOps.Core`) from forensic I/O and display handlers (`SecurityOps.IO`).

---

## System Architecture & Required Modular Methods

### Namespace Organization
* `SecurityOps.Core`: Houses log tokenizers, attack signature pattern matchers, temporal brute-force analyzers, and risk matrix aggregators.
* `SecurityOps.IO`: Houses raw log stream ingestion, formatted SOC terminal display tables, and disk export for incident response dossiers.

---

### Required Modular Methods in `SecurityOps.Core.DetectionEngine`

#### Method A: Raw Log Entry Tokenizer
* **Identifier:** `ParseLogEntry`
* **Signature:** `public static (bool isValid, string ipAddress, DateTime timestamp, string httpMethod, string endpoint, int statusCode, int responseBytes, string userAgent) ParseLogEntry(string rawLogLine)`
* **Log Syntax Format:**
  `IP [YYYY-MM-DD HH:mm:ss] "METHOD /endpoint HTTP/1.1" STATUS_CODE BYTES "USER_AGENT"`
  *Example:* `192.168.1.105 [2026-10-23 03:14:02] "GET /products?id=1' UNION SELECT null,password FROM users-- HTTP/1.1" 200 4520 "sqlmap/1.6"`
* **Logic:**
  * Uses string search methods (`IndexOf`, `Substring`, `Split`) to isolate each component.
  * Parses timestamp using `DateTime.ParseExact(tsString, "yyyy-MM-dd HH:mm:ss", null)`.
  * Safely parses status code and byte count using `int.TryParse`.
  * Returns `isValid = false` if the line is corrupted or missing delimiters, allowing the caller to `continue` safely.

#### Method B: Signature & Attack Vector Scanner
* **Identifier:** `ScanAttackSignatures`
* **Signature:** `public static (string threatCategory, int riskScore, string matchedPattern) ScanAttackSignatures(string endpoint, string userAgent)`
* **Threat Rules & Scoring:**
  * **SQL Injection (SQLi) - Score: 85:**
    If endpoint (case-insensitive) contains `' OR '1'='1`, `UNION SELECT`, `SLEEP(`, `--`, or `INFORMATION_SCHEMA`.
    Category: `"SQL Injection (SQLi)"`.
  * **Path Traversal / Local File Inclusion (LFI) - Score: 90:**
    If endpoint contains `../`, `..%2f`, `/etc/passwd`, or `win.ini`.
    Category: `"Path Traversal (LFI)"`.
  * **Automated Reconnaissance Scanner - Score: 70:**
    If userAgent (case-insensitive) contains `sqlmap`, `nikto`, `gobuster`, `nmap`, or `masscan`.
    Category: `"Automated Exploit Scanner"`.
  * **Normal Traffic - Score: 0:**
    Category: `"BENIGN TRAFFIC"`, `riskScore = 0`, `matchedPattern = "None"`.
  * If multiple signatures match, return the highest-scoring threat.

#### Method C: Temporal Brute-Force Frequency Evaluator
* **Identifier:** `AnalyzeBruteForceActivity`
* **Signature:** `public static (bool isBruteForce, int failedAttemptCount) AnalyzeBruteForceActivity(string ipAddress, List<string> ipHistory, List<DateTime> timeHistory, List<string> endpointHistory, List<int> statusHistory, DateTime currentEventTime)`
* **Logic:**
  * Scans backwards through historical requests originating from the same `ipAddress`.
  * Counts requests to login endpoints (`/login`, `/api/auth`, `/admin/login`) with HTTP `401` (Unauthorized) or `403` (Forbidden) status that occurred **within 60 seconds** of `currentEventTime` (`currentEventTime - pastEventTime <= TimeSpan.FromSeconds(60)`).
  * If `failedAttemptCount >= 5`, triggers `isBruteForce = true`.

#### Method D: Comprehensive Threat Severity Classifier
* **Identifier:** `ClassifyIncidentSeverity`
* **Signature:** `public static (string severityLabel, string recommendedAction) ClassifyIncidentSeverity(int threatScore, bool isBruteForce, int statusCode)`
* **Decision Hierarchy:**
  * **CRITICAL THREAT (Score $\ge 85$ or `isBruteForce == true`):**
    Action: `"EMERGENCY FIREWALL IP DROP + ISOLATE HOST"`.
  * **HIGH RISK (Score between 65 and 84):**
    Action: `"RATE LIMIT IP + CAPTCHA CHALLENGE"`.
  * **SUSPICIOUS (Score between 30 and 64, or multiple 404/403 errors):**
    Action: `"FLAG FOR MONITORING + ALERT SOC TIER 2"`.
  * **BENIGN (Score $< 30$):**
    Action: `"PERMIT TRAFFIC"`.

#### Method E: SOC Threat Matrix & Forensics Aggregator
* **Identifier:** `GenerateThreatMatrix`
* **Signature:** `public static (int totalRequests, int criticalCount, int highCount, int suspiciousCount, int benignCount, string topAttackerIp, int maxAttacksBySingleIp) GenerateThreatMatrix(List<string> severities, List<string> ips)`
* **Logic:**
  * Loops through all processed records to aggregate threat counts per severity level.
  * Identifies the primary offending IP address responsible for the highest volume of malicious requests.
  * Guarded against empty log datasets to prevent runtime exceptions.

---

## Interactive SOC Console Menu & Operational Flow

The terminal runs in an interactive `do-while` loop presenting the SOC dispatch menu:

```text
============================================================
    AEGIS-SOC: AUTOMATED INTRUSION DETECTION SYSTEM (M2)
============================================================
  1. Inspect Single HTTP Log Row (Manual Real-Time Triage)
  2. Ingest Server Access Log (server_access.log)
  3. View Threat Intelligence Dossier & Offending IP Ledger
  4. Export Incident Response Dossier (security_incident_report.txt)
  5. Shutdown IDS Terminal
============================================================
Enter option (1-5): 
```

### Menu Behaviors:
* **Option 1 (Manual Real-Time Triage):** Operator pastes or types a raw HTTP access string. Engine extracts tokens, checks attack signatures, determines severity, and displays immediate triage results.
* **Option 2 (Ingest Server Access Log):** Ingests `server_access.log` line by line using `File.ReadAllLines` or `StreamReader`. Parses each entry with Method A, evaluates signatures (Method B), tests temporal brute-force activity (Method C), classifies severity (Method D), and adds records to in-memory audit lists. Skips empty/corrupted lines using `continue`.
* **Option 3 (View Threat Intelligence Dossier):** Prints a formatted tabular ledger of all processed events with color-coded or aligned severity tags, followed by the computed Threat Matrix summary from Method E.
* **Option 4 (Export Incident Response Dossier):** Generates `security_incident_report.txt` containing a summary executive header, itemized attacker logs, and auto-generated Linux firewall remediation commands (`iptables -A INPUT -s <IP> -j DROP`).
* **Option 5 (Shutdown IDS Terminal):** Displays an operational session logout notice and terminates cleanly via `break`.

---

## Output Display Specifications (Option 3 Sample)

```text
========================================================================================================================
                                     AEGIS-SOC INTRUSION DETECTION DOSSIER
========================================================================================================================
Event ID   Timestamp            Source IP        Method  Endpoint / Payload               Risk  Severity Status
------------------------------------------------------------------------------------------------------------------------
EVT-5001   2026-10-23 03:14:02  203.0.113.42     GET     /products?id=1' UNION SELECT...    85  CRITICAL THREAT
EVT-5002   2026-10-23 03:14:05  198.51.100.18    POST    /api/login (Failed Attempt #5)     85  CRITICAL THREAT (BRUTE)
EVT-5003   2026-10-23 03:14:12  45.33.32.156     GET     /../../../../etc/passwd            90  CRITICAL THREAT
EVT-5004   2026-10-23 03:14:18  192.168.1.105    GET     /dashboard/profile                  0  BENIGN TRAFFIC
------------------------------------------------------------------------------------------------------------------------
THREAT MATRIX SUMMARY:
  Total Events Inspected : 4 events
  Critical Threats Blocked: 3
  High-Risk Probes        : 0
  Suspicious Requests    : 0
  Benign Requests        : 1
  Top Malicious IP       : 203.0.113.42 (1 attacks recorded)
  Firewall Drop Targets  : 3 IP addresses scheduled for immediate iptables blacklisting
========================================================================================================================
```

---

## File Storage Specifications

### 1. Raw Server Log Format (`server_access.log`)
Each line represents an HTTP access event with dynamic payload structures:
```text
203.0.113.42 [2026-10-23 03:14:02] "GET /products?id=1' UNION SELECT null,password FROM users-- HTTP/1.1" 200 4520 "Mozilla/5.0"
198.51.100.18 [2026-10-23 03:14:05] "POST /api/login HTTP/1.1" 401 128 "curl/7.68.0"
45.33.32.156 [2026-10-23 03:14:12] "GET /../../../../etc/passwd HTTP/1.1" 404 230 "Nikto/2.1.6"
192.168.1.105 [2026-10-23 03:14:18] "GET /dashboard/profile HTTP/1.1" 200 8920 "Mozilla/5.0 (Windows NT 10.0; Win64)"
```

### 2. Exported Incident Report (`security_incident_report.txt`)
```text
SECURITY OPERATIONS CENTER (SOC) - INCIDENT RESPONSE REPORT
Generated: 2026-10-23 | Severity Level: HIGH ALERT
--------------------------------------------------------------------------------
INCIDENT EVIDENCE LEDGER:
EVT-5001 | 203.0.113.42 | SQL Injection (SQLi) | Payload: ' UNION SELECT | CRITICAL THREAT
EVT-5002 | 198.51.100.18 | Brute-Force Auth | 5 Failed Logins / 60s | CRITICAL THREAT (BRUTE)
EVT-5003 | 45.33.32.156 | Path Traversal (LFI) | Payload: ../../../../etc/passwd | CRITICAL THREAT
--------------------------------------------------------------------------------
RECOMMENDED FIREWALL REMEDIATION SCRIPT:
iptables -A INPUT -s 203.0.113.42 -j DROP
iptables -A INPUT -s 198.51.100.18 -j DROP
iptables -A INPUT -s 45.33.32.156 -j DROP
--------------------------------------------------------------------------------
SUMMARY: Total=4 | Critical=3 | Benign=1 | Incident Status: CONTAINED
```

---

## Verification & Test Scenarios

| Event ID | Log Input Attributes | Detected Attack Signature | Classified Severity & Action |
| :--- | :--- | :--- | :--- |
| **EVT-5001** | IP: 203.0.113.42<br>URI: `/products?id=1' UNION SELECT...`<br>Status: 200 \| Bytes: 4520 | Pattern: `UNION SELECT`<br>Category: SQL Injection (SQLi)<br>Score: 85 | <span class="badge badge-danger">CRITICAL THREAT</span><br>Action: Immediate Firewall Drop |
| **EVT-5002** | IP: 198.51.100.18<br>URI: `/api/login`<br>Status: 401 \| UserAgent: `curl/7.68.0` | 5th Failed Login within 60s window<br>Category: Brute-Force Auth<br>Score: 85 | <span class="badge badge-danger">CRITICAL THREAT (BRUTE)</span><br>Action: Immediate Firewall Drop |
| **EVT-5003** | IP: 45.33.32.156<br>URI: `/../../../../etc/passwd`<br>Status: 404 \| Agent: `Nikto/2.1.6` | Pattern: `../` and Scanner: `Nikto`<br>Category: Path Traversal (LFI)<br>Score: 90 | <span class="badge badge-danger">CRITICAL THREAT</span><br>Action: Immediate Firewall Drop |
| **EVT-5004** | IP: 192.168.1.105<br>URI: `/dashboard/profile`<br>Status: 200 \| Bytes: 8920 | No signatures matched<br>Category: Benign Traffic<br>Score: 0 | <span class="badge badge-success">BENIGN TRAFFIC</span><br>Action: Permit Traffic |

---

## Grading & Verification Checklist
- [ ] **String Tokenization:** Successfully extracts IP, timestamp, method, URI, status code, and user agent from unstructured log syntax without relying on brittle fixed-index offsets.
- [ ] **Temporal Analysis:** Uses `DateTime.ParseExact` and `TimeSpan` math to accurately identify rapid-fire brute-force threshold breaches within rolling 60-second windows.
- [ ] **Pattern Recognition:** Scans for SQLi, directory traversal, and exploit scanners using case-insensitive substring search methods (`Contains`, `IndexOf`).
- [ ] **Tuple Return Values:** Modular methods return strongly typed multi-value C# tuples for parsing, attack classification, and batch threat matrices.
- [ ] **Loop & Error Guarding:** Interactive loop handles invalid input; file reader skips blank lines with `continue`; division calculations guard against zero denominators.
- [ ] **Deterministic Resource Management:** File streams are wrapped in `using` blocks to guarantee file handles release immediately upon completion.
- [ ] **Zero Warnings / Zero Errors:** Application compiles cleanly under .NET 8 / 9 with 0 errors and 0 warnings.
