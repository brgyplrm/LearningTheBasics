using SecurityOps.Core;

namespace SecurityOps.IO
{
  /*
  ==============================================================================
  SECURITY I/O LAYER (PIPELINE & DISPATCH OPERATOR)
  ==============================================================================
  The I/O layer handles physical disk files, stream readers/writers, and console
  presentation. It serves as the "Conveyor Belt" of the architecture:
  - Ingests raw lines from disk
  - Passes each row through the Core Engine (Methods A -> B -> C -> D)
  - Records clean forensic tokens into the caller's memory ledgers
  - Renders formatted console reports and exports firewall mitigation rules
  ==============================================================================
  */
  public static class ManifestSecIO
  {
    // =========================================================================
    // 1. BATCH ACCESS LOG INGESTION & PIPELINE EVALUATOR
    // =========================================================================
    public static void LoadFromLogFile(
      List<string> eventIds,
      List<string> ipAddresses,
      List<DateTime> timeStamps,
      List<string> httpMethods,
      List<string> endpointList,
      List<int> statusesCodes,
      List<int> responsesBytes,
      List<string> userAgents,
      List<string> threatCategories,
      List<int> riskScores,
      List<string> severities,
      List<string> actions,
      string filePath = "server_access.log")
    {
      // Defensive File Resolution: Search current folder, subfolder, and project root
      string targetFile = filePath;
      if (!File.Exists(targetFile))
      {
        string subfolderPath = Path.Combine("MileStone2Methods", filePath);
        string projectPath = Path.Combine("/home/achyllisss/code/development/School/C#/learning", filePath);
        string methodPath = Path.Combine("/home/achyllisss/code/development/School/C#/learning/MileStone2Methods", filePath);

        if (File.Exists(subfolderPath)) targetFile = subfolderPath;
        else if (File.Exists(projectPath)) targetFile = projectPath;
        else if (File.Exists(methodPath)) targetFile = methodPath;
        else
        {
          Console.WriteLine($"\n[ERROR] File not found: {filePath}");
          return;
        }
      }

      string[] batchInputLines = File.ReadAllLines(targetFile);
      int loadedCount = 0;
      int nextEventNum = eventIds.Count + 5001;

      foreach (string rawLine in batchInputLines)
      {
        // Pipeline Station A: Tokenize raw log entry
        var entry = ThreatEngine.ParseLogEntry(rawLine);

        // Guard: Skip corrupt or unparseable lines immediately
        if (!entry.isValid)
        {
          Console.WriteLine($"[WARN] Skipped invalid line: {rawLine}");
          continue;
        }

        string eventId = $"EVT-{nextEventNum++}";

        // Add to history first so sliding-window analysis includes this attempt
        eventIds.Add(eventId);
        ipAddresses.Add(entry.ipAddress);
        timeStamps.Add(entry.timestamp);
        httpMethods.Add(entry.httpMethod);
        endpointList.Add(entry.endpoint);
        statusesCodes.Add(entry.statusCode);
        responsesBytes.Add(entry.responseBytes);
        userAgents.Add(entry.userAgent);

        // Pipeline Station B: Signature & Attack Vector Analysis
        var attack = ThreatEngine.ScanAttackSignatures(entry.endpoint, entry.userAgent);

        // Pipeline Station C: Temporal Brute-Force Frequency Evaluation
        var bruteForce = ThreatEngine.AnalyzeBruteForceActivity(
          entry.ipAddress,
          ipAddresses,
          timeStamps,
          endpointList,
          statusesCodes,
          entry.timestamp);

        // Pipeline Station D: Comprehensive Severity Classification
        var incident = ThreatEngine.ClassifyIncidentSeverity(
          attack.riskScore,
          bruteForce.isBruteForce,
          entry.statusCode);

        string finalSeverity = incident.severityLabel;
        if (bruteForce.isBruteForce)
        {
          finalSeverity = "CRITICAL THREAT (BRUTE)";
        }

        threatCategories.Add(attack.threatCategory);
        riskScores.Add(attack.riskScore);
        severities.Add(finalSeverity);
        actions.Add(incident.recommendedAction);

        loadedCount++;
      }

      Console.WriteLine($"\nSuccessfully ingested {loadedCount} log events into Aegis-SOC database from '{targetFile}'.");
    }
    /*
    ----------------------------------------------------------------------------
    LOADFROMLOGFILE ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. THE CONVEYOR BELT PATTERN:
       - LoadFromLogFile reads each raw line and coordinates Station A -> B -> C -> D.
       - It does not contain string slicing or threat formulas itself; it delegates
         pure logic to ThreatEngine in .Core.

    2. STATE MANAGEMENT & REFERENCE PASSING:
       - The 12 lists passed into the method parameters originate in Main.
       - Because List<T> is a Reference Type, any .Add(...) operation populates
         the caller's memory directly.

    3. ROBUST PATH DISCOVERY:
       - Checks the current working directory, subfolders, and absolute paths to
         guarantee file loading works whether launched from root, bin, or subfolder.
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // 2. REAL-TIME MANUAL HTTP LOG TRIAGE (OPTION 1)
    // =========================================================================
    public static void TriageSingleLogEntry(
      List<string> eventIds,
      List<string> ipAddresses,
      List<DateTime> timeStamps,
      List<string> httpMethods,
      List<string> endpointList,
      List<int> statusesCodes,
      List<int> responsesBytes,
      List<string> userAgents,
      List<string> threatCategories,
      List<int> riskScores,
      List<string> severities,
      List<string> actions)
    {
      Console.WriteLine("\n=== MANUAL REAL-TIME HTTP LOG TRIAGE ===");
      Console.WriteLine("Paste or type a standard server access log line:");
      Console.Write("> ");
      string? input = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(input))
      {
        Console.WriteLine("[ERROR] Empty log row provided. Returning to menu.");
        return;
      }

      var entry = ThreatEngine.ParseLogEntry(input);
      if (!entry.isValid)
      {
        Console.WriteLine("[ERROR] Invalid log format. Could not tokenize entry.");
        return;
      }

      string eventId = $"EVT-{eventIds.Count + 5001}";

      eventIds.Add(eventId);
      ipAddresses.Add(entry.ipAddress);
      timeStamps.Add(entry.timestamp);
      httpMethods.Add(entry.httpMethod);
      endpointList.Add(entry.endpoint);
      statusesCodes.Add(entry.statusCode);
      responsesBytes.Add(entry.responseBytes);
      userAgents.Add(entry.userAgent);

      var attack = ThreatEngine.ScanAttackSignatures(entry.endpoint, entry.userAgent);
      var bruteForce = ThreatEngine.AnalyzeBruteForceActivity(
        entry.ipAddress,
        ipAddresses,
        timeStamps,
        endpointList,
        statusesCodes,
        entry.timestamp);

      var incident = ThreatEngine.ClassifyIncidentSeverity(
        attack.riskScore,
        bruteForce.isBruteForce,
        entry.statusCode);

      string finalSeverity = incident.severityLabel;
      if (bruteForce.isBruteForce)
      {
        finalSeverity = "CRITICAL THREAT (BRUTE)";
      }

      threatCategories.Add(attack.threatCategory);
      riskScores.Add(attack.riskScore);
      severities.Add(finalSeverity);
      actions.Add(incident.recommendedAction);

      Console.WriteLine("\n------------------------------------------------------------");
      Console.WriteLine($"  Event ID          : {eventId}");
      Console.WriteLine($"  Source IP         : {entry.ipAddress}");
      Console.WriteLine($"  Timestamp         : {entry.timestamp:yyyy-MM-dd HH:mm:ss}");
      Console.WriteLine($"  HTTP Request      : {entry.httpMethod} {entry.endpoint}");
      Console.WriteLine($"  Status Code       : {entry.statusCode} ({entry.responseBytes} bytes)");
      Console.WriteLine($"  User Agent        : {entry.userAgent}");
      Console.WriteLine($"  Threat Category   : {attack.threatCategory}");
      Console.WriteLine($"  Matched Signature : {attack.matchedPattern}");
      Console.WriteLine($"  Risk Score        : {attack.riskScore}/100");
      Console.WriteLine($"  Brute Force Tally : {bruteForce.failedAttemptCount} failed attempts");
      Console.WriteLine($"  Severity Rating   : {finalSeverity}");
      Console.WriteLine($"  Action Directive  : {incident.recommendedAction}");
      Console.WriteLine("------------------------------------------------------------");
      Console.WriteLine("Event committed to live forensic memory.");
    }
    /*
    ----------------------------------------------------------------------------
    TRIAGESINGLELOGENTRY ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. CODE REUSE ACROSS WORKFLOWS:
       - Uses the exact same Core methods (A, B, C, D) used by the batch file reader.
       - Demonstrates that well-designed Core methods work identically for a single
         console input or a 1,000,000-line batch file.
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // 3. TABULAR DOSSIER DISPLAY & FORENSIC LEDGER (OPTION 3)
    // =========================================================================
    public static void DisplayDossier(
      List<string> eventIds,
      List<string> ipAddresses,
      List<DateTime> timeStamps,
      List<string> httpMethods,
      List<string> endpointList,
      List<int> riskScores,
      List<string> severities)
    {
      if (eventIds.Count == 0)
      {
        Console.WriteLine("\n[NOTICE] No security events logged in memory yet. Please ingest a log file first (Option 2).");
        return;
      }

      Console.WriteLine("\n========================================================================================================================");
      Console.WriteLine("                                     AEGIS-SOC INTRUSION DETECTION DOSSIER");
      Console.WriteLine("========================================================================================================================");
      Console.WriteLine("{0,-10} {1,-20} {2,-16} {3,-7} {4,-35} {5,5}  {6,-24}",
        "Event ID", "Timestamp", "Source IP", "Method", "Endpoint / Payload", "Risk", "Severity Status");
      Console.WriteLine(new string('-', 120));

      for (int i = 0; i < eventIds.Count; i++)
      {
        string ep = endpointList[i];
        if (ep.Length > 33)
        {
          ep = ep.Substring(0, 30) + "...";
        }

        Console.WriteLine("{0,-10} {1,-20} {2,-16} {3,-7} {4,-35} {5,5}  {6,-24}",
          eventIds[i],
          timeStamps[i].ToString("yyyy-MM-dd HH:mm:ss"),
          ipAddresses[i],
          httpMethods[i],
          ep,
          riskScores[i],
          severities[i]);
      }

      Console.WriteLine(new string('-', 120));

      // Method E: Threat Matrix Summary
      var matrix = ThreatEngine.GenerateThreatMatrix(severities, ipAddresses);

      // Collect unique IPs targeted for firewall drops
      HashSet<string> firewallDropIps = new HashSet<string>();
      for (int i = 0; i < severities.Count; i++)
      {
        if (severities[i].Contains("CRITICAL"))
        {
          firewallDropIps.Add(ipAddresses[i]);
        }
      }

      Console.WriteLine("THREAT MATRIX SUMMARY:");
      Console.WriteLine($"  Total Events Inspected : {matrix.totalRequests} events");
      Console.WriteLine($"  Critical Threats Blocked: {matrix.criticalCount}");
      Console.WriteLine($"  High-Risk Probes        : {matrix.highCount}");
      Console.WriteLine($"  Suspicious Requests    : {matrix.suspiciousCount}");
      Console.WriteLine($"  Benign Requests        : {matrix.benignCount}");
      Console.WriteLine($"  Top Malicious IP       : {matrix.topAttackerIp} ({matrix.maxAttacksBySingleIp} attacks recorded)");
      Console.WriteLine($"  Firewall Drop Targets  : {firewallDropIps.Count} IP addresses scheduled for immediate iptables blacklisting");
      Console.WriteLine("========================================================================================================================");
    }
    /*
    ----------------------------------------------------------------------------
    DISPLAYDOSSIER ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. COMPOSITE FORMATTING WITH PADDING:
       - Uses format strings like "{0,-10} {1,-20} {2,5}".
       - Negative width (-10) means LEFT-align text.
       - Positive width (5) means RIGHT-align numbers.
       - Guarantees columns line up perfectly regardless of variable string lengths.

    2. STRING CLIPPING (DEFENSIVE PRESENTATION):
       - Long query strings (/products?id=1' UNION SELECT...) can break table layout.
       - Clamping long strings with Substring(0, 30) + "..." keeps tables clean.

    3. DEDUPLICATION WITH HASHSET<T>:
       - HashSet<string> automatically ignores duplicate attacker IPs.
       - Ensures the firewall blacklisting rule only lists each IP once.
    ----------------------------------------------------------------------------
    */


    // =========================================================================
    // 4. INCIDENT REPORT EXPORTER & FIREWALL RULE GENERATOR (OPTION 4)
    // =========================================================================
    public static void ExportIncidentReportToFile(
      string filePath,
      List<string> eventIds,
      List<string> ipAddresses,
      List<string> threatCategories,
      List<string> endpointList,
      List<string> severities)
    {
      if (eventIds.Count == 0)
      {
        Console.WriteLine("\n[NOTICE] No incident records available to export. Ingest a log first.");
        return;
      }

      HashSet<string> droppedIps = new HashSet<string>();
      for (int i = 0; i < severities.Count; i++)
      {
        if (severities[i].Contains("CRITICAL") || severities[i].Contains("HIGH"))
        {
          droppedIps.Add(ipAddresses[i]);
        }
      }

      string resolvedPath = filePath;
      if (!Path.IsPathRooted(resolvedPath))
      {
        resolvedPath = Path.Combine("/home/achyllisss/code/development/School/C#/learning", filePath);
      }

      using (StreamWriter writer = new StreamWriter(resolvedPath))
      {
        writer.WriteLine("SECURITY OPERATIONS CENTER (SOC) - INCIDENT RESPONSE REPORT");
        writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Severity Level: HIGH ALERT");
        writer.WriteLine(new string('-', 80));
        writer.WriteLine("INCIDENT EVIDENCE LEDGER:");

        for (int i = 0; i < eventIds.Count; i++)
        {
          if (!severities[i].Contains("BENIGN"))
          {
            writer.WriteLine($"{eventIds[i]} | {ipAddresses[i]} | {threatCategories[i]} | Target: {endpointList[i]} | {severities[i]}");
          }
        }

        writer.WriteLine(new string('-', 80));
        writer.WriteLine("RECOMMENDED FIREWALL REMEDIATION SCRIPT:");

        foreach (string ip in droppedIps)
        {
          writer.WriteLine($"iptables -A INPUT -s {ip} -j DROP");
        }

        writer.WriteLine(new string('-', 80));
        var matrix = ThreatEngine.GenerateThreatMatrix(severities, ipAddresses);
        string incidentStatus = matrix.criticalCount > 0 ? "CONTAINED / ACTION REQUIRED" : "NORMAL";
        writer.WriteLine($"SUMMARY: Total={matrix.totalRequests} | Critical={matrix.criticalCount} | High={matrix.highCount} | Suspicious={matrix.suspiciousCount} | Benign={matrix.benignCount} | Incident Status: {incidentStatus}");
      }

      Console.WriteLine($"\nSuccessfully exported incident response dossier with {droppedIps.Count} firewall drop rules to '{resolvedPath}'.");
    }
    /*
    ----------------------------------------------------------------------------
    EXPORTINCIDENTREPORTTOFILE ARCHITECTURAL & DISCOVERY NOTES:
    ----------------------------------------------------------------------------
    1. DETERMINISTIC DISPOSAL WITH 'using':
       - Wrapping StreamWriter inside 'using (StreamWriter writer = ...)' guarantees
         that the file buffer is flushed and closed, even if an unexpected exception occurs.

    2. AUDITED LINUX REMEDIATION AUTOMATION:
       - Automatically translates detected malicious IPs into production-ready
         Linux firewall commands (iptables -A INPUT -s <IP> -j DROP).
    ----------------------------------------------------------------------------
    */
  }
}
