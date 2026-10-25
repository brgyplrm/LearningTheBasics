/*
================================================================================
                    C# NOTES - MILESTONE MACHINE 2 (CYBERSECURITY IDS)
================================================================================

PROJECT: AEGIS-SOC: Server Access Log Analyzer & Threat Detection Engine
DOMAIN : Cybersecurity / Security Operations Center (SOC) Log Analytics

LEARNING MODULES:
- Module 1: Advanced String Tokenization & Delimiter Slicing (Quotes, Brackets, Spaces)
- Module 2: Modern C# Index (^) and Range (..) Slicing Operators (C# vs Python)
- Module 3: Substring Search & Attack Signature Scanning (.Contains vs == Trap)
- Module 4: Multi-Tier Architecture & Separation of Concerns (.Core vs .IO vs Main)
- Module 5: Memory Ownership, Scope, and Reference Passing (The Suitcase / Box Analogy)
- Module 6: Sliding-Window Temporal Analysis with DateTime and TimeSpan
- Module 7: Tally Counter Loop Patterns vs Premature Return Traps
- Module 8: Professional Engineering & Defensive Programming Rules

================================================================================
*/

#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS8321 // The function is declared but never used

using System;
using System.IO;
using System.Collections.Generic;

public static class MilestoneMachine2Notes
{
    public static void Run()
    {
        Console.WriteLine("\n[AEGIS-SOC ARCHITECTURAL & DISCOVERY NOTES LOADED]");

        // =====================================================================
        // MODULE 1: ADVANCED STRING TOKENIZATION & DELIMITER SLICING
        // =====================================================================

        // 1. WHY PRINTING AN ARRAY PRINTS "System.String[]"
        // ---------------------------------------------------------------------
        // DISCOVERY:
        // When you write Console.WriteLine(parts) or $"{parts}", C# calls .ToString().
        // In .NET, arrays (string[]) do NOT override .ToString() because converting
        // an array with 1,000,000 elements to text could freeze memory.
        // Instead, it returns its type descriptor name: "System.String[]".
        // To inspect contents: use indexing (parts[0]), loop (foreach), or string.Join(", ", parts).

        string[] sampleArray = { "Alpha", "Beta", "Gamma" };
        string arrayTypeLabel = sampleArray.ToString()!; // "System.String[]"
        string joinedContent = string.Join(" | ", sampleArray); // "Alpha | Beta | Gamma"

        // 2. DELIMITER SLICING ON LOGS (Quotes, Brackets, Spaces)
        // ---------------------------------------------------------------------
        // Standard Web Access Log format:
        // IP [Timestamp] "METHOD /endpoint HTTP/1.1" STATUS BYTES "USER_AGENT"
        // Splitting by quotes ('"') cleanly isolates:
        //   parts[0] -> IP and bracketed Timestamp
        //   parts[1] -> The HTTP Request line
        //   parts[2] -> Status Code and Response Byte Size
        //   parts[3] -> User-Agent String

        string sampleLog = "192.168.1.105 [2026-10-23 03:14:40] \"GET /dashboard HTTP/1.1\" 200 8920 \"Mozilla/5.0\"";
        string[] logParts = sampleLog.Split('"');

        // Extracting IP and Time using bracket splitting:
        string[] ipTimeTokens = logParts[0].Split('[');
        string extractedIp = ipTimeTokens[0].Trim();
        string extractedTime = ipTimeTokens[1].Trim(' ', ']');

        // Guarding against empty splits (The Leading Space Trap):
        // " 200 8920 ".Split(' ') produces an empty string at index 0!
        // Solution: Trim first, or use StringSplitOptions.RemoveEmptyEntries
        string[] numberParts = logParts[2].Trim().Split(' ');
        int.TryParse(numberParts[0], out int extractedStatus);
        int.TryParse(numberParts[1], out int extractedBytes);


        // =====================================================================
        // MODULE 2: MODERN C# INDEX (^) AND RANGE (..) SLICING OPERATORS
        // =====================================================================

        // DISCOVERY & PYTHON COMPARISON:
        // C# 8+ introduced Index (^) and Range (..) operators, equivalent to Python slicing:
        // Python: words[-1]    ==> C#: words[^1]     (1st from the end / last item)
        // Python: words[1:-1]  ==> C#: words[1..^1]  (Start at 1, stop before last)
        // Python: words[:3]    ==> C#: words[..3]    (From beginning up to index 3)
        // Python: words[2:]    ==> C#: words[2..]    (From index 2 to end)

        string requestLine = "GET /products?id=1' UNION SELECT null FROM users-- HTTP/1.1";
        string[] reqWords = requestLine.Split(' ');

        string httpMethod = reqWords[0];                          // Index 0: First word ("GET")
        string protocol = reqWords[^1];                           // Index ^1: Last word ("HTTP/1.1")
        string endpoint = string.Join(" ", reqWords[1..^1]);      // Middle slice: keeps injected spaces intact!

        // PITFALL AVOIDED (The Zero-Based Slicing Trap):
        // If you grab reqWords[1] for method AND reqWords[1..^1] for endpoint,
        // you duplicate index 1 and completely miss index 0!
        // Always remember: Index 0 is the 1st element. Range 1..^1 starts at the 2nd element.


        // =====================================================================
        // MODULE 3: SUBSTRING SEARCH & ATTACK SIGNATURE SCANNING (.Contains vs ==)
        // =====================================================================

        // CRITICAL DISCOVERY (Why == Fails on Web Logs):
        // - '==' tests for EXACT equality from start to finish.
        // - In web traffic, attacks are embedded in paths: "/products?id=1' UNION SELECT..."
        // - Testing ("/products?id=1' UNION SELECT" == "UNION SELECT") evaluates to FALSE!
        // - Therefore, '==' lets 100% of real-world web attacks bypass the IDS.
        // - Substring search via .Contains() or .IndexOf() >= 0 is MANDATORY.

        string attackUrl = "/products?id=1' UNION SELECT null,password FROM users--";
        string cleanLowerUrl = attackUrl.ToLower(); // Normalizes case (case-insensitive detection)

        bool isSqlInjection = cleanLowerUrl.Contains("union select") || cleanLowerUrl.Contains("' or '1'='1");
        bool isPathTraversal = cleanLowerUrl.Contains("../") || cleanLowerUrl.Contains("/etc/passwd");


        // =====================================================================
        // MODULE 4: MULTI-TIER ARCHITECTURE & SEPARATION OF CONCERNS
        // =====================================================================

        // THE ASSEMBLY LINE PATTERN:
        // 1. Entry Point (MileStoneMachine2.cs) = THE WAREHOUSE
        //    Owns the memory ledgers (Lists), manages user menus, and controls lifecycle.
        // 2. IO Layer (SecurityIO.cs) = THE CONVEYOR BELT OPERATOR
        //    Reads physical files from disk, passes rows down the line, prints reports.
        // 3. Core Engine (SecurityCore.cs) = THE SPECIALIST STATIONS
        //    - Method A (ParseLogEntry)            : String tokenizer & type parser.
        //    - Method B (ScanAttackSignatures)     : Signature & pattern matcher.
        //    - Method C (AnalyzeBruteForceActivity): Temporal history inspector.
        //    - Method D (ClassifyIncidentSeverity) : Multi-factor decision matrix.
        //    - Method E (GenerateThreatMatrix)     : Forensics aggregator & metrics.

        // WHY NOT COMBINE EVERYTHING IN ONE METHOD?
        // - Single Responsibility Principle (SRP): Each method does one job and does it right.
        // - Isolated Testing: You can test the SQLi scanner without needing a fake file or clock.
        // - Maintainability: If the log format changes tomorrow, you only edit Method A.


        // =====================================================================
        // MODULE 5: MEMORY OWNERSHIP, SCOPE, AND REFERENCE PASSING
        // =====================================================================

        // THE TRAVELER AND SUITCASE METAPHOR:
        // - Main is the Traveler. It creates the suitcases: List<string> ipAddresses = new();
        // - LoadFromLogFile is the Hotel Valet.
        // - Traveler hands the suitcase handles (references) to the Valet.
        // - Valet packs the items into the Traveler's suitcase (ipAddresses.Add(...)).
        // - Because lists in C# are Reference Types, when the Valet leaves (method finishes),
        //   the Traveler is still holding the populated suitcases!
        // - If LoadFromLogFile created its own lists internally without returning them,
        //   those lists would be erased by the Garbage Collector the moment the method ended.


        // =====================================================================
        // MODULE 6: SLIDING-WINDOW TEMPORAL ANALYSIS (DateTime & TimeSpan)
        // =====================================================================

        // HOW BRUTE FORCE IS DETECTED:
        // Attackers hammer authentication endpoints within short bursts.
        // By subtracting two DateTime structs, C# returns a TimeSpan:
        //   TimeSpan diff = currentEventTime - pastEventTime;
        //   if (diff <= TimeSpan.FromSeconds(60)) { ... }

        DateTime eventTime1 = DateTime.Parse("2026-10-23 03:14:05");
        DateTime eventTime2 = DateTime.Parse("2026-10-23 03:14:25");
        TimeSpan elapsed = eventTime2 - eventTime1; // 20 seconds
        bool isWithinWindow = elapsed <= TimeSpan.FromSeconds(60); // True!


        // =====================================================================
        // MODULE 7: TALLY COUNTER PATTERNS VS PREMATURE RETURN TRAPS
        // =====================================================================

        // THE PREMATURE RETURN TRAP:
        // In search loops, NEVER put an 'else return' inside the loop!
        // Doing so forces the method to exit on item #1, skipping the rest of the array.
        // Only return when you match a positive trigger, or increment a tally counter.
        // You return the final verdict only AFTER all loops have finished checking.

        int failedCounter = 0;
        int[] historicalStatuses = { 401, 401, 401, 401, 401 };

        foreach (int statusCode in historicalStatuses)
        {
            if (statusCode == 401 || statusCode == 403)
            {
                failedCounter++; // Click the counter, do not return yet!
            }
        }

        bool bruteForceTriggered = (failedCounter >= 5); // Evaluated at the end!


        // =====================================================================
        // MODULE 8: CORE PROGRAMMING PRACTICES TO KEEP IN MIND
        // =====================================================================
        /*
        1. DEFENSIVE GUARD CLAUSES FIRST:
           Always check string.IsNullOrWhiteSpace(rawLine) BEFORE calling .Split()!
           Calling .Split() on a null string causes a NullReferenceException crash.

        2. LADDER DECISION PATTERNS:
           When classifying numbers in ranges (>=85, 65-84, 30-64, <30), evaluate in
           descending order. You don't need redundant conditions like '&& score <= 84'
           because previous else-if branches already filtered out numbers >= 85.

        3. CASE NORMALIZATION:
           Always use .ToLower() once at the start of a scanning function rather than
           calling it repeatedly on every keyword.

        4. EXECUTION TIMING MATTERS:
           When evaluating an incoming event against history, ensure the incoming event
           is recorded into your history lists before or during the temporal evaluation
           so the 5th attempt is counted properly.

        5. CLEAN CONSOLE FORMATTING:
           Use aligned composite formatting:
           Console.WriteLine("{0,-10} {1,-20} {2,5}", id, timestamp, score);
           Negative numbers left-align; positive numbers right-align.
        */
    }
}
