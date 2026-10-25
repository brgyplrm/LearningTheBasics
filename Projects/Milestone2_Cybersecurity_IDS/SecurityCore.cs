using SecurityOps.IO;

namespace SecurityOps.Core
{
  public static class ThreatEngine
  {
    // =========================================================================
    // METHOD A: RAW LOG ENTRY TOKENIZER & DELIMITER SLICER
    // =========================================================================
    public static (bool isValid, string ipAddress, DateTime timestamp, string httpMethod,
    string endpoint, int statusCode, int responseBytes, string userAgent) ParseLogEntry(string rawLine)
    {
      // Defensive Guard: Check for null/empty string before attempting any string operations
      if (string.IsNullOrWhiteSpace(rawLine))
      {
        return (false, "", DateTime.MinValue, "", "", 0, 0, "");
      }

      string[] parts = rawLine.Split('"');

      // Defensive Guard: Verify the line contains at least 4 quote-delimited segments
      if (parts.Length < 4)
      {
        return (false, "", DateTime.MinValue, "", "", 0, 0, "");
      }

      // 1. Parse IP and Timestamp from parts[0]
      string[] ipTimestamps = parts[0].Split('[');
      if (ipTimestamps.Length < 2)
      {
        return (false, "", DateTime.MinValue, "", "", 0, 0, "");
      }

      string ipAddress = ipTimestamps[0].Trim();
      DateTime.TryParse(ipTimestamps[1].Trim(' ', ']'), out DateTime timestamp);

      // 2. Parse Method and Endpoint from parts[1] using Range Slicing
      string[] requestParts = parts[1].Trim().Split(' ');
      if (requestParts.Length < 2)
      {
        return (false, "", DateTime.MinValue, "", "", 0, 0, "");
      }

      string httpMethod = requestParts[0];
      // Range 1..^1 starts at index 1 and stops before the last item (protocol),
      // preserving all spaces inside complex SQL injection query payloads!
      string endpoint = string.Join(" ", requestParts[1..^1]);

      // 3. Parse Status Code and Response Bytes from parts[2]
      string[] statusParts = parts[2].Trim().Split(' ');
      if (statusParts.Length < 2)
      {
        return (false, "", DateTime.MinValue, "", "", 0, 0, "");
      }

      int.TryParse(statusParts[0].Trim(), out int statusCode);
      int.TryParse(statusParts[1].Trim(), out int requestBytes);

      // 4. Parse User-Agent from parts[3]
      string agent = parts[3].Trim();

      return (true, ipAddress, timestamp, httpMethod, endpoint, statusCode, requestBytes, agent);
    }
    /*
    ----------------------------------------------------------------------------
    METHOD A ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. WHY PRINTING ARRAYS GIVES "System.String[]":
       - In .NET, arrays do not override Object.ToString().
       - Printing an array ($"{parts}") prints the type name to avoid freezing
         memory on massive arrays. To see contents, access indices (parts[0])
         or use string.Join().

    2. THE "LEADING SPACE" SPLIT TRAP:
       - In raw logs: ...HTTP/1.1" 200 4520 "Mozilla...
       - Notice the space right after the closing quote. parts[2] becomes " 200 4520 ".
       - Calling .Split(' ') on text with a leading space puts an empty string ("")
         at index 0! That made statusCode empty and shifted the real status into
         requestBytes.
       - FIX: Always call .Trim() before .Split(), or use StringSplitOptions.RemoveEmptyEntries.

    3. C# RANGE SLICING (1..^1) VS PYTHON:
       - Python: words[1:-1]  <==>  C#: words[1..^1]
       - ^1 means "1st item from the end" (the protocol bookend).
       - 1..^1 slices from index 1 up to ^1, keeping injected spaces intact.

    4. DEFENSIVE GUARD ORDERING:
       - Always check string.IsNullOrWhiteSpace(rawLine) BEFORE calling .Split()!
         Calling .Split() on a null string causes a fatal NullReferenceException.
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // METHOD B: MULTI-VECTOR SIGNATURE & ATTACK VECTOR SCANNER
    // =========================================================================
    public static (string threatCategory, int riskScore, string matchedPattern)
    ScanAttackSignatures(string endpoint, string userAgent)
    {
      string endpointParts = (endpoint ?? "").ToLower();
      string userAgentsParts = (userAgent ?? "").ToLower();

      string[] sqliPatterns = { "' OR '1'='1", "UNION SELECT", "SLEEP(", "--", "INFORMATION_SCHEMA" };
      string[] lfiPatterns = { "../", "..%2f", "/etc/passwd", "win.ini" };
      string[] exploitScannerPatterns = { "sqlmap", "nikto", "gobuster", "nmap", "masscan" };

      // Priority 1: Check Path Traversal / LFI (Highest Risk Score: 90)
      foreach (string patterns in lfiPatterns)
      {
        if (endpointParts.Contains(patterns.ToLower()))
        {
          return ("Path Traversal (LFI)", 90, patterns);
        }
      }

      // Priority 2: Check SQL Injection (Risk Score: 85)
      foreach (string patterns in sqliPatterns)
      {
        if (endpointParts.Contains(patterns.ToLower()))
        {
          return ("SQL INJECTION (SQLi)", 85, patterns);
        }
      }

      // Priority 3: Check Automated Reconnaissance Scanners (Risk Score: 70)
      foreach (string patterns in exploitScannerPatterns)
      {
        if (userAgentsParts.Contains(patterns.ToLower()))
        {
          return ("Automated Exploit Scanner", 70, patterns);
        }
      }

      // Default: Clean / Normal Traffic
      return ("BENIGN TRAFFIC", 0, "None");
    }
    /*
    ----------------------------------------------------------------------------
    METHOD B ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. WHY '==' FAILS FOR WEB ATTACKS:
       - '==' requires 100% exact equality from start to finish.
       - Real web requests attach attacks to paths: "/products?id=1' UNION SELECT..."
       - Testing ("/products?id=1' UNION SELECT" == "UNION SELECT") is FALSE.
       - Substring searching via .Contains() or .IndexOf() >= 0 is mandatory.

    2. THE PREMATURE RETURN TRAP:
       - Putting an 'else return "BENIGN"' inside a search loop forces C# to exit
         on pattern #1, skipping all remaining signatures!
       - In search loops: only return when a match is FOUND. Return "BENIGN"
         at the very bottom only after ALL loops have finished searching.

    3. CASE NORMALIZATION:
       - Calling .ToLower() once at the start normalizes the input so attacker
         variations (e.g. "uNiOn SeLeCt", "NiKtO") are matched consistently.

    4. MULTI-VECTOR PRIORITY ORDER:
       - The specification dictates: "If multiple signatures match, return the highest score."
       - By evaluating LFI (90) before SQLi (85) before Scanners (70), the highest
         risk score is guaranteed to win naturally without complex sorting.
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // METHOD C: TEMPORAL BRUTE-FORCE FREQUENCY EVALUATOR (SLIDING WINDOW)
    // =========================================================================
    public static (bool isBruteForce, int failedAttemptCount)
    AnalyzeBruteForceActivity(string ipAddress, List<string> ipHistory, List<DateTime> timeHistory,
    List<string> endpointHistory, List<int> statusHistory, DateTime currentEventTime)
    {
      int failedAttempCount = 0;
      string[] loginEndpoints = { "/login", "/api/login", "/api/auth", "/admin/login" };

      for (int i = 0; i < ipHistory.Count; i++)
      {
        // 1. Filter: Check if the historical event originated from the same IP
        if (ipHistory[i] != ipAddress)
        {
          continue;
        }

        // 2. Filter: Check if the event occurred within the 60-second sliding window
        TimeSpan timeDifference = currentEventTime - timeHistory[i];
        if (timeDifference > TimeSpan.FromSeconds(60) || timeDifference < TimeSpan.Zero)
        {
          continue;
        }

        // 3. Filter: Check if the target was an authentication endpoint
        foreach (string logEndpoints in loginEndpoints)
        {
          if (endpointHistory[i] == logEndpoints)
          {
            // 4. Filter: Check for failed HTTP authentication status codes
            if (statusHistory[i] == 401 || statusHistory[i] == 403)
            {
              failedAttempCount++;
              break;
            }
          }
        }
      }

      bool isBruteForce = failedAttempCount >= 5;
      return (isBruteForce, failedAttempCount);
    }
    /*
    ----------------------------------------------------------------------------
    METHOD C ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. THE TALLY COUNTER PATTERN VS PREMATURE RETURN:
       - Inside the loop, you must never 'return' on finding a failed attempt.
       - A bouncer clicks a tally counter (failedAttemptCount++) and keeps watching!
       - The threshold comparison (failedAttemptCount >= 5) evaluates only at the end.

    2. SLIDING-WINDOW TEMPORAL ARITHMETIC:
       - Subtracting two DateTime structs (currentEventTime - timeHistory[i]) yields
         a TimeSpan struct.
       - TimeSpan.FromSeconds(60) cleanly evaluates time windows without manual
         millisecond or epoch math.

    3. EXECUTION TIMING IN .IO:
       - If you call AnalyzeBruteForceActivity BEFORE adding the current event to
         ipHistory, the 5th attempt isn't in memory yet, leaving the count at 4!
       - Adding to the list first ensures the current attempt is included in the tally.
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // METHOD D: COMPREHENSIVE THREAT SEVERITY CLASSIFIER
    // =========================================================================
    public static (string severityLabel, string recommendedAction)
    ClassifyIncidentSeverity(int threatScore, bool isBruteForce, int statusCode)
    {
      // Tier 1: Critical Threat (Score >= 85 or detected Brute-Force activity)
      if (isBruteForce || threatScore >= 85)
      {
        return ("CRITICAL THREAT", "EMERGENCY FIREWALL IP DROP + ISOLATE HOST");
      }
      // Tier 2: High Risk (Score between 65 and 84)
      else if (threatScore >= 65)
      {
        return ("HIGH RISK", "RATE LIMIT IP + CAPTCHA CHALLENGE");
      }
      // Tier 3: Suspicious Traffic (Score between 30 and 64, or reconnaissance errors)
      else if (threatScore >= 30 || statusCode == 404 || statusCode == 403)
      {
        return ("SUSPICIOUS", "FLAG FOR MONITORING + ALERT SOC TIER 2");
      }
      // Tier 4: Benign / Permitted Traffic (Score < 30)
      else
      {
        return ("BENIGN", "PERMIT TRAFFIC");
      }
    }
    /*
    ----------------------------------------------------------------------------
    METHOD D ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. THE JUDGE PATTERN (MULTI-FACTOR ESCALATION):
       - Method B alone gave "/api/login" a score of 0 because the URL had no SQLi.
       - Method C discovered that the IP failed 5 logins in 20 seconds (isBruteForce = true).
       - Method D combines both signals: even with a score of 0, isBruteForce == true
         escalates the event directly to CRITICAL THREAT!

    2. THE DESCENDING LADDER DECISION PATTERN:
       - Because if-else evaluates top-down, checking >= 85 first means the next
         branch (else if threatScore >= 65) is automatically capped below 85!
       - Eliminates redundant conditions like "&& threatScore <= 84".
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // METHOD E: SOC THREAT MATRIX & FORENSICS AGGREGATOR
    // =========================================================================
    public static (int totalRequests, int criticalCount, int highCount, int suspiciousCount, int benignCount, string topAttackerIp, int maxAttacksBySingleIp)
    GenerateThreatMatrix(List<string> severities, List<string> ips)
    {
      // Defensive Guard: Handle empty or uninitialized datasets
      if (severities == null || severities.Count == 0 || ips == null || ips.Count == 0)
      {
        return (0, 0, 0, 0, 0, "N/A", 0);
      }

      int totalRequests = severities.Count;
      int criticalCount = 0;
      int highCount = 0;
      int suspiciousCount = 0;
      int benignCount = 0;

      Dictionary<string, int> attackCountsPerIp = new Dictionary<string, int>();

      for (int i = 0; i < severities.Count; i++)
      {
        string sev = severities[i];
        string ip = (i < ips.Count) ? ips[i] : "UNKNOWN";

        if (sev.Contains("CRITICAL"))
        {
          criticalCount++;
        }
        else if (sev.Contains("HIGH"))
        {
          highCount++;
        }
        else if (sev.Contains("SUSPICIOUS"))
        {
          suspiciousCount++;
        }
        else
        {
          benignCount++;
        }

        // Count malicious events per IP to identify primary threat actor
        if (!sev.Contains("BENIGN"))
        {
          if (!attackCountsPerIp.ContainsKey(ip))
          {
            attackCountsPerIp[ip] = 0;
          }
          attackCountsPerIp[ip]++;
        }
      }

      string topAttackerIp = "None";
      int maxAttacksBySingleIp = 0;

      foreach (var kvp in attackCountsPerIp)
      {
        if (kvp.Value > maxAttacksBySingleIp)
        {
          maxAttacksBySingleIp = kvp.Value;
          topAttackerIp = kvp.Key;
        }
      }

      if (maxAttacksBySingleIp == 0 && ips.Count > 0)
      {
        topAttackerIp = ips[0];
      }

      return (totalRequests, criticalCount, highCount, suspiciousCount, benignCount, topAttackerIp, maxAttacksBySingleIp);
    }
    /*
    ----------------------------------------------------------------------------
    METHOD E ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. FREQUENCY COUNTING WITH DICTIONARIES:
       - Uses Dictionary<string, int> to maintain key-value mappings of (IP -> Count).
       - Automatically tracks multiple incidents per IP without hardcoding IP addresses.

    2. AGGREGATION VS ROW-LEVEL PROCESSING:
       - Methods A, B, C, D operate on a SINGLE ROW (Micro analysis).
       - Method E operates across ALL ROWS (Macro analytics & executive summary).
    ----------------------------------------------------------------------------
    */
  }
}
