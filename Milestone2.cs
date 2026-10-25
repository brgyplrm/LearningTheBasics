using System;
using System.Collections.Generic;
using CargoLogistics.Core;
using CargoLogistics.IO;

public static class Milestone2
{
  public static void Run()
  {
    // Active batch memory collections
    List<string> trackingIds = new List<string>();
    List<decimal> weights = new List<decimal>();
    List<decimal> totals = new List<decimal>();
    List<string> statuses = new List<string>();
    List<int> zones = new List<int>();
    List<string> cargoTypes = new List<string>();

    int manualPackageCounter = 1001;
    bool isRunning = true;

    do
    {
      Console.WriteLine("\n============================================================");
      Console.WriteLine("       CARGO LOGISTICS DISPATCH & AUDIT TERMINAL (M2)");
      Console.WriteLine("============================================================");
      Console.WriteLine("  1. Add Single Package to Batch");
      Console.WriteLine("  2. Load Package Batch from File (batch_input.txt)");
      Console.WriteLine("  3. View Active Batch Manifest & Summary");
      Console.WriteLine("  4. Export Audited Manifest to File (manifest_output.txt)");
      Console.WriteLine("  5. Exit System");
      Console.WriteLine("============================================================");
      Console.Write("Enter option (1-5): ");

      string choice = (Console.ReadLine() ?? "").Trim();

      switch (choice)
      {
        case "1":
          AddSinglePackage(trackingIds, weights, totals, statuses, zones, cargoTypes, ref manualPackageCounter);
          break;

        case "2":
          ManifestIO.LoadFromBatchFile(trackingIds, weights, totals, statuses, zones, cargoTypes, "batch_input.txt");
          break;

        case "3":
          DisplayBatchManifest(trackingIds, weights, totals, statuses, zones, cargoTypes);
          break;

        case "4":
          ManifestIO.ExportManifestToFile("manifest_output.txt", trackingIds, weights, totals, statuses, zones, cargoTypes);
          break;

        case "5":
          isRunning = false;
          Console.WriteLine("\n[TERMINAL] Exiting Cargo Logistics System. Session closed safely.");
          break;

        default:
          Console.WriteLine("\n[ERROR] Invalid option selected. Please enter a valid number (1-5).");
          break;
      }

    } while (isRunning);
  }

  // Option 1: Interactively collect package attributes and add to batch
  private static void AddSinglePackage(
    List<string> trackingIds,
    List<decimal> weights,
    List<decimal> totals,
    List<string> statuses,
    List<int> zones,
    List<string> cargoTypes,
    ref int counter)
  {
    Console.WriteLine("\n--- Add Single Package to Batch ---");

    // 1. Physical Weight
    decimal physicalKg = 0m;
    while (true)
    {
      Console.Write("Actual Weight (kg): ");
      if (decimal.TryParse(Console.ReadLine() ?? "", out physicalKg) && physicalKg > 0)
      {
        break;
      }
      Console.WriteLine("Invalid weight. Please enter a positive number.");
    }

    // 2. Dimensions
    int lengthCm = 0;
    while (true)
    {
      Console.Write("Package Length (cm): ");
      if (int.TryParse(Console.ReadLine() ?? "", out lengthCm) && lengthCm > 0)
      {
        break;
      }
      Console.WriteLine("Invalid length. Please enter a whole positive number.");
    }

    int widthCm = 0;
    while (true)
    {
      Console.Write("Package Width (cm): ");
      if (int.TryParse(Console.ReadLine() ?? "", out widthCm) && widthCm > 0)
      {
        break;
      }
      Console.WriteLine("Invalid width. Please enter a whole positive number.");
    }

    int heightCm = 0;
    while (true)
    {
      Console.Write("Package Height (cm): ");
      if (int.TryParse(Console.ReadLine() ?? "", out heightCm) && heightCm > 0)
      {
        break;
      }
      Console.WriteLine("Invalid height. Please enter a whole positive number.");
    }

    // 3. Destination Zone
    int zone = 0;
    while (true)
    {
      Console.Write("Destination Zone (1=Domestic, 2=Regional, 3=International): ");
      if (int.TryParse(Console.ReadLine() ?? "", out zone) && zone >= 1 && zone <= 3)
      {
        break;
      }
      Console.WriteLine("Invalid zone. Please choose 1, 2, or 3.");
    }

    // 4. Cargo Type
    string cargoType = "General Merchandise";
    while (true)
    {
      Console.WriteLine("Cargo Type:");
      Console.WriteLine("  1. General Merchandise");
      Console.WriteLine("  2. Perishable Goods");
      Console.WriteLine("  3. Hazardous Material");
      Console.WriteLine("  4. High-Value Electronics");
      Console.Write("Select Cargo Type (1-4): ");
      string tChoice = (Console.ReadLine() ?? "").Trim();
      if (tChoice == "1") { cargoType = "General Merchandise"; break; }
      if (tChoice == "2") { cargoType = "Perishable Goods"; break; }
      if (tChoice == "3") { cargoType = "Hazardous Material"; break; }
      if (tChoice == "4") { cargoType = "High-Value Electronics"; break; }
      Console.WriteLine("Invalid selection. Please choose 1 to 4.");
    }

    // 5. Declared Value
    decimal declaredValue = 0m;
    while (true)
    {
      Console.Write("Declared Value (PHP): ");
      if (decimal.TryParse(Console.ReadLine() ?? "", out declaredValue) && declaredValue >= 0)
      {
        break;
      }
      Console.WriteLine("Invalid declared value. Please enter a valid amount.");
    }

    // 6. Commercial Invoice
    bool hasInvoice = false;
    while (true)
    {
      Console.Write("Includes Commercial Invoice? (y/n): ");
      string inv = (Console.ReadLine() ?? "").Trim().ToLower();
      if (inv == "y" || inv == "yes" || inv == "true") { hasInvoice = true; break; }
      if (inv == "n" || inv == "no" || inv == "false") { hasInvoice = false; break; }
      Console.WriteLine("Please enter 'y' or 'n'.");
    }

    // 7. Shipping Insurance
    bool hasInsurance = false;
    while (true)
    {
      Console.Write("Requested Transit Insurance? (y/n): ");
      string ins = (Console.ReadLine() ?? "").Trim().ToLower();
      if (ins == "y" || ins == "yes" || ins == "true") { hasInsurance = true; break; }
      if (ins == "n" || ins == "no" || ins == "false") { hasInsurance = false; break; }
      Console.WriteLine("Please enter 'y' or 'n'.");
    }

    // Calculations
    var weightResult = CalculationEngine.CalculateChargeableWeight(physicalKg, lengthCm, widthCm, heightCm);
    string status = CalculationEngine.EvaluateStatus(physicalKg, lengthCm, widthCm, heightCm, zone, cargoType, declaredValue, hasInvoice, hasInsurance);
    var feeResult = CalculationEngine.CalculateFees(weightResult.chargeableWeight, zone, cargoType, declaredValue, hasInsurance);

    decimal finalTotal = feeResult.totalDue;
    if (status == "SHIPMENT REJECTED")
    {
      finalTotal = 0.00m;
    }

    string generatedId = $"PKG-{counter++}";

    // Add to lists
    trackingIds.Add(generatedId);
    weights.Add(weightResult.chargeableWeight);
    totals.Add(finalTotal);
    statuses.Add(status);
    zones.Add(zone);
    cargoTypes.Add(cargoType);

    Console.WriteLine($"\n[SUCCESS] Package {generatedId} recorded!");
    Console.WriteLine($"  Chargeable Weight : {weightResult.chargeableWeight:F2} kg (Volumetric: {weightResult.volumeMetricWeight:F2} kg)");
    Console.WriteLine($"  Total Due         : PHP {finalTotal:N2}");
    Console.WriteLine($"  Status            : {status}");
  }

  // Option 3: Tabular Manifest View and Analytics
  private static void DisplayBatchManifest(
    List<string> trackingIds,
    List<decimal> weights,
    List<decimal> totals,
    List<string> statuses,
    List<int> zones,
    List<string> cargoTypes)
  {
    if (trackingIds.Count == 0)
    {
      Console.WriteLine("\n[NOTICE] The active batch is currently empty. Please add packages or load batch_input.txt first.");
      return;
    }

    Console.WriteLine("\n========================================================================================");
    Console.WriteLine("                                CURRENT BATCH MANIFEST");
    Console.WriteLine("========================================================================================");
    Console.WriteLine("{0,-12} {1,11}  {2,-4}  {3,-20} {4,16}  {5,-22}", "Tracking ID", "Chg. Weight", "Zone", "Cargo Type", "Total Due (PHP)", "Status");
    Console.WriteLine(new string('-', 88));

    for (int i = 0; i < trackingIds.Count; i++)
    {
      string zoneStr = (i < zones.Count) ? zones[i].ToString() : "N/A";
      string typeStr = (i < cargoTypes.Count) ? cargoTypes[i] : "General";

      // Shorten type string for clean column alignment if needed
      if (typeStr.Length > 20)
      {
        typeStr = typeStr.Substring(0, 17) + "...";
      }

      string formattedWeight = $"{weights[i]:F2} kg";
      string formattedTotal = $"{totals[i]:N2}";

      Console.WriteLine("{0,-12} {1,11}  {2,-4}  {3,-20} {4,16}  {5,-22}",
        trackingIds[i], formattedWeight, zoneStr, typeStr, formattedTotal, statuses[i]);
    }

    Console.WriteLine(new string('-', 88));

    // Call Method D for summary analytics
    var summary = CalculationEngine.GenerateBatchSummary(weights, totals, statuses);

    Console.WriteLine("BATCH ANALYTICS:");
    Console.WriteLine($"  Total Processed    : {summary.totalPackages} packages");
    Console.WriteLine($"  Cleared Packages   : {summary.clearedCount}");
    Console.WriteLine($"  Held for Review    : {summary.heldCount}");
    Console.WriteLine($"  Rejected Packages  : {summary.rejectedCount}");
    Console.WriteLine($"  Total Revenue      : PHP {summary.totalRevenue:N2}");
    Console.WriteLine($"  Average Weight     : {summary.avgWeight:F2} kg");
    Console.WriteLine("========================================================================================");
  }
}


/*
================================================================================
                    CODE REVIEW & ARCHITECTURAL AUDIT NOTES
================================================================================
Score: 98 / 100
Status: APPROVED & MASTERED - MILESTONE 2 (Console Architecture, Loops, Methods, Collections & File I/O)

1. WHAT IMPROVED (CORE ARCHITECTURAL DISCOVERIES):
   - Modular Decomposition (No Monolithic Main): Replaced top-level procedural
     spaghetti with dedicated, reusable static methods with typed parameters,
     return values, and named C# tuples.
   - Loop Flow Control: Mastered interactive loop repetition (do-while), collection
     iteration (for / foreach), early termination (break), and defensive record
     skipping (continue).
   - Defensive File Ingestion: Safely parsed delimited CSV records from disk,
     handling empty lines and malformed rows without crashing the runtime.
   - Parallel Collections: Synchronized multiple generic List<T> collections
     (trackingIds, weights, totals, statuses, zones, cargoTypes) sharing a single index.
   - Deterministic Resource Disposal: Utilized 'using (StreamWriter)' blocks to
     guarantee file streams flush and close immediately, preventing locked file handles.

2. DEEP-DIVE PEDAGOGICAL BREAKTHROUGHS & MENTAL MODELS:

   A. The Building vs. The Worker (Class vs. Method vs. Void):
      - A Class ('public static class') is a Container/Building. It cannot directly
        execute loops, conditionals, or print statements.
      - A Method is the Worker/Action that performs logic.
      - 'void' is a return type indicating the method sends no data back. When a
        method returns a value, 'void' is completely erased and replaced by the
        data type or tuple:
          public static [WHAT COMES OUT] MethodName([WHAT GOES IN])

   B. The Egg Carton Analogy (parts.Length vs. parts[0].Length):
      - 'parts.Length' (no brackets) measures how many slots/items exist in the array.
      - 'parts[0].Length' (with brackets) measures the character count of the word
        inside slot 0.
      - Guarding with 'if (parts.Length < 10) continue;' verifies the carton has
        all 10 required slots before accessing parts[9], completely eliminating
        IndexOutOfRangeException crashes on blank or malformed rows.

   C. Parse vs. TryParse with 'out' Parameters:
      - 'Parse' assumes data is 100% valid; invalid text throws a fatal FormatException.
      - 'TryParse' returns a bool and outputs the converted number via an 'out' variable:
          decimal.TryParse(parts[1], out decimal packageKg);
      - It never crashes, and declares the usable variable right inside the parameter.

   D. Temporary Sticky Notes vs. The Growing Notebook (Scope vs. List<T>):
      - Variables declared inside a 'foreach' loop are temporary sticky notes that
        get overwritten and wiped clean on each loop iteration.
      - A 'List<T>' acts as a permanent notebook. Calling '.Add()' INSIDE the loop
        ensures every single evaluated record is retained in active memory.

   E. The Cash Register Analogy (Summary Reporting):
      - Summary generators ('GenerateBatchSummary') do NOT re-run calculation algorithms.
      - Like a store manager totaling up the receipts at the end of the day, the method
        simply aggregates already-calculated numbers from the parallel lists.

   F. Defensive Arithmetic (Division by Zero):
      - Guarding average calculations with 'if (totalPackages > 0)' prevents
        fatal DivideByZeroException crashes when an operator views an empty batch.

   G. Multi-File Project Architecture & Namespaces:
      - C# does not enforce folder names matching namespace names (unlike Java).
      - Subfolders (e.g., 'MileStone2Methods/') are automatically discovered and
        compiled by the modern .NET SDK.
      - Logical boundaries: 'CargoLogistics.Core' owns mathematical business rules,
        while 'CargoLogistics.IO' owns disk file persistence and tabular presentation.

3. CODE HYGIENE & PRODUCTION CHECKLIST:
   [X] Modular Methods: Standalone calculation, evaluation, and summary methods.
   [X] Named Tuples: Clean multi-value return syntax without creating boilerplate structs.
   [X] Dynamic Collections: Generic List<T> memory storage without fixed array limits.
   [X] Loop Control: do-while menu, for/foreach traversals, continue guard rails.
   [X] File I/O Safety: File.ReadAllLines and StreamWriter enclosed in using statements.
   [X] Namespace Segregation: Clean decoupling of domain logic from I/O operations.
   [X] Zero Warnings / Zero Errors: 100% clean compilation under .NET 8 SDK.

VERDICT:
You have graduated from single-script procedural programming to multi-file,
modular, event-driven C# architecture. You have mastered loops, collections,
methods, tuples, and file I/O. Milestone 2 is officially complete and certified.
================================================================================
*/
