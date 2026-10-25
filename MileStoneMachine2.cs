using SecurityOps.IO;

/*
================================================================================
           AEGIS-SOC: AUTOMATED INTRUSION DETECTION SYSTEM (MILESTONE 2)
================================================================================
Entry Point & Terminal Dispatch Controller
Responsible for:
- Allocating and holding in-memory forensic ledgers (Parallel Lists)
- Orchestrating the interactive terminal loop (Options 1 to 5)
- Passing memory handles to SecurityIO for ingestion, triage, and reporting
================================================================================
*/

public static class MileStoneMachine2
{
  public static void Run()
  {
    // Central In-Memory Forensic Ledgers (Parallel Lists / Columnar Storage)
    List<string> eventIds = new List<string>();
    List<string> ipAddresses = new List<string>();
    List<DateTime> timeStamps = new List<DateTime>();
    List<string> httpMethods = new List<string>();
    List<string> endpointList = new List<string>();
    List<int> statusesCodes = new List<int>();
    List<int> responsesBytes = new List<int>();
    List<string> userAgents = new List<string>();
    List<string> threatCategories = new List<string>();
    List<int> riskScores = new List<int>();
    List<string> severities = new List<string>();
    List<string> actions = new List<string>();

    bool isRunning = true;

    while (isRunning)
    {
      Console.WriteLine("\n============================================================");
      Console.WriteLine("    AEGIS-SOC: AUTOMATED INTRUSION DETECTION SYSTEM (M2)");
      Console.WriteLine("============================================================");
      Console.WriteLine("  1. Inspect Single HTTP Log Row (Manual Real-Time Triage)");
      Console.WriteLine("  2. Ingest Server Access Log (server_access.log)");
      Console.WriteLine("  3. View Threat Intelligence Dossier & Offending IP Ledger");
      Console.WriteLine("  4. Export Incident Response Dossier (security_incident_report.txt)");
      Console.WriteLine("  5. Shutdown IDS Terminal");
      Console.WriteLine("============================================================");
      Console.Write("Enter option (1-5): ");

      string? choice = Console.ReadLine()?.Trim();

      switch (choice)
      {
        case "1":
          ManifestSecIO.TriageSingleLogEntry(
            eventIds,
            ipAddresses,
            timeStamps,
            httpMethods,
            endpointList,
            statusesCodes,
            responsesBytes,
            userAgents,
            threatCategories,
            riskScores,
            severities,
            actions);
          break;

        case "2":
          Console.Write("\nEnter log file path (Press Enter for 'server_access.log'): ");
          string? filePathInput = Console.ReadLine()?.Trim();
          string logFile = string.IsNullOrWhiteSpace(filePathInput) ? "server_access.log" : filePathInput;

          ManifestSecIO.LoadFromLogFile(
            eventIds,
            ipAddresses,
            timeStamps,
            httpMethods,
            endpointList,
            statusesCodes,
            responsesBytes,
            userAgents,
            threatCategories,
            riskScores,
            severities,
            actions,
            logFile);
          break;

        case "3":
          ManifestSecIO.DisplayDossier(
            eventIds,
            ipAddresses,
            timeStamps,
            httpMethods,
            endpointList,
            riskScores,
            severities);
          break;

        case "4":
          Console.Write("\nEnter export file name (Press Enter for 'security_incident_report.txt'): ");
          string? exportInput = Console.ReadLine()?.Trim();
          string exportFile = string.IsNullOrWhiteSpace(exportInput) ? "security_incident_report.txt" : exportInput;

          ManifestSecIO.ExportIncidentReportToFile(
            exportFile,
            eventIds,
            ipAddresses,
            threatCategories,
            endpointList,
            severities);
          break;

        case "5":
          Console.WriteLine("\n[SYSTEM] De-authenticating from Aegis SOC Core...");
          Console.WriteLine("[SYSTEM] Forensic memory safely unmounted.");
          Console.WriteLine("[SYSTEM] Terminal session closed. Stand down, Operator.");
          isRunning = false;
          break;

        default:
          Console.WriteLine("\n[ERROR] Invalid option. Please enter a number between 1 and 5.");
          break;
      }

      if (isRunning)
      {
        Console.Write("\nPress Enter to return to the SOC menu...");
        Console.ReadLine();
      }
    }
  }
}

/*
================================================================================
                    CODE REVIEW & ARCHITECTURAL AUDIT NOTES
================================================================================
Project: Aegis-SOC Cybersecurity Intrusion Detection System
Status : OFFICIALLY COMPLETED & VERIFIED (Score: 100 / 100)
Clearance: FULLY CLEARED TO ADVANCE TO MILESTONE 3 (Classes, Objects & OOP)

1. WHAT YOU MASTERED IN THIS MACHINE PROBLEM:
   - Delimiter Tokenization & Sanitization: Cleanly separated multi-delimiter logs
     (quotes, brackets, spaces) into typed variables.
   - Substring Pattern Matching: Understood why '==' fails for web attack queries
     and mastered case-insensitive .Contains() searching.
   - Temporal Frequency Analysis: Modeled sliding 60-second time windows with
     DateTime and TimeSpan to detect multi-attempt brute-force attacks.
   - State Management & Lifetime: Mastered why Main holds the memory buckets (Lists)
     and hands the handles to .IO so data persists across menu choices.
   - Multi-Tier Separation of Concerns: Main (Lifecycle) -> .IO (Conveyor Belt)
     -> .Core (Pure Logic Stations).

2. THE "PARALLEL LISTS" FRICTION & THE BRIDGE TO MILESTONE 3:
   - In this project, you successfully coordinated 12 parallel lists:
     (eventIds, ipAddresses, timeStamps, httpMethods, endpointList, statusesCodes...)
   - Passing 12 lists across methods works, but feels heavy and repetitive.
   - This exact friction is why Object-Oriented Programming (OOP) was invented!
   - In Milestone 3, you will learn to replace all 12 parallel lists with a single
     custom Class/Object: List<SecurityIncident> or List<LogRecord>.
   - Having experienced parallel lists firsthand, OOP will now make total sense.

3. CORE PROGRAMMING PRACTICES TO KEEP IN MIND:
   A. Defensive Guard Ordering: Always check string.IsNullOrWhiteSpace() before .Split().
   B. Search Loop Integrity: Never place 'else return' inside search loops; let the
      loop finish checking all items before concluding benign traffic.
   C. Tally Counter Pattern: Increment counters (failedCount++) during loops; evaluate
      thresholds (>= 5) only after the loop completes.
   D. Descending Decision Ladders: Test highest thresholds first (>= 85, >= 65, >= 30)
      to eliminate redundant compound conditions.
   E. Safe Resource Management: Always wrap stream file writes in 'using' blocks.
================================================================================
*/
