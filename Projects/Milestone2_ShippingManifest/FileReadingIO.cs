using System;
using System.IO;
using System.Collections.Generic;
using CargoLogistics.Core;

namespace CargoLogistics.IO
{
  public static class ManifestIO
  {
    public static void LoadFromBatchFile(
      List<string> trackingIds,
      List<decimal> weights,
      List<decimal> totals,
      List<string> statuses,
      List<int>? zones = null,
      List<string>? cargoTypes = null,
      string filePath = "batch_input.txt")
    {
      // Check current folder, then check subfolder and project paths if not found
      string targetFile = filePath;
      if (!File.Exists(targetFile))
      {
        string dataPath = Path.Combine("Data", filePath);
        string projectDataPath = Path.Combine("/home/achyllisss/code/development/School/C#/learning/Data", filePath);
        string projectPath = Path.Combine("/home/achyllisss/code/development/School/C#/learning", filePath);

        if (File.Exists(dataPath)) targetFile = dataPath;
        else if (File.Exists(projectDataPath)) targetFile = projectDataPath;
        else if (File.Exists(projectPath)) targetFile = projectPath;
        else
        {
          Console.WriteLine($"\n[ERROR] File not found: {filePath}");
          return;
        }
      }

      string[] batchInputLines = File.ReadAllLines(targetFile);
      int loadedCount = 0;

      foreach (string rawLine in batchInputLines)
      {
        string line = rawLine.Trim();
        if (string.IsNullOrWhiteSpace(line))
        {
          continue;
        }

        string[] parts = line.Split(",");

        if (parts.Length < 10)
        {
          continue; // Skip corrupted rows
        }

        string trackingId = parts[0].Trim();
        decimal.TryParse(parts[1].Trim(), out decimal packageKg);
        int.TryParse(parts[2].Trim(), out int packageLen);
        int.TryParse(parts[3].Trim(), out int packageWdt);
        int.TryParse(parts[4].Trim(), out int packageHt);
        int.TryParse(parts[5].Trim(), out int packageZone);
        string packageType = parts[6].Trim();
        decimal.TryParse(parts[7].Trim(), out decimal packageValue);
        bool.TryParse(parts[8].Trim(), out bool hasInvoice);
        bool.TryParse(parts[9].Trim(), out bool hasInsurance);

        var weightResult = CalculationEngine.CalculateChargeableWeight(packageKg, packageLen, packageWdt, packageHt);
        string status = CalculationEngine.EvaluateStatus(packageKg, packageLen, packageWdt, packageHt, packageZone, packageType, packageValue, hasInvoice, hasInsurance);
        var feeResult = CalculationEngine.CalculateFees(weightResult.chargeableWeight, packageZone, packageType, packageValue, hasInsurance);

        decimal finalTotal = feeResult.totalDue;
        if (status == "SHIPMENT REJECTED")
        {
          finalTotal = 0.00m;
        }

        trackingIds.Add(trackingId);
        weights.Add(weightResult.chargeableWeight);
        totals.Add(finalTotal);
        statuses.Add(status);

        if (zones != null)
        {
          zones.Add(packageZone);
        }
        if (cargoTypes != null)
        {
          cargoTypes.Add(packageType);
        }

        loadedCount++;
      }

      Console.WriteLine($"\nSuccessfully loaded and evaluated {loadedCount} packages from {targetFile}.");
    }

    public static void ExportManifestToFile(
      string filePath,
      List<string> trackingIds,
      List<decimal> weights,
      List<decimal> totals,
      List<string> statuses,
      List<int>? zones = null,
      List<string>? cargoTypes = null)
    {
      if (trackingIds.Count == 0)
      {
        Console.WriteLine("\n[NOTICE] No active manifest records to export.");
        return;
      }

      using (StreamWriter writer = new StreamWriter(filePath))
      {
        writer.WriteLine("AUDITED SHIPMENT MANIFEST");
        writer.WriteLine($"Exported: {DateTime.Now:yyyy-MM-dd}");
        writer.WriteLine(new string('-', 80));

        for (int i = 0; i < trackingIds.Count; i++)
        {
          string zoneStr = (zones != null && i < zones.Count) ? $"Zone {zones[i]}" : "Zone N/A";
          string typeStr = (cargoTypes != null && i < cargoTypes.Count) ? cargoTypes[i] : "General";
          writer.WriteLine($"{trackingIds[i]} | {weights[i]:F2} kg | {zoneStr} | {typeStr} | PHP {totals[i]:N2} | {statuses[i]}");
        }

        writer.WriteLine(new string('-', 80));

        var summary = CalculationEngine.GenerateBatchSummary(weights, totals, statuses);
        writer.WriteLine($"SUMMARY: Total={summary.totalPackages} | Cleared={summary.clearedCount} | Held={summary.heldCount} | Rejected={summary.rejectedCount} | Revenue=PHP {summary.totalRevenue:N2}");
      }

      Console.WriteLine($"\nSuccessfully exported {trackingIds.Count} records to '{filePath}'.");
    }
  }
}
