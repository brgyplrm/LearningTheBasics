namespace CargoLogistics.Core
{
  public static class CalculationEngine
  {
    public static (decimal volumeMetricWeight, decimal chargeableWeight) CalculateChargeableWeight(decimal packageKg, int packageLen, int packageWdt, int packageHt)
    {
      // Calculate the volumeMetricWeight
      int packageVol = packageLen * packageWdt * packageHt;
      decimal dVol = 5000.0m;
      // volumemetricweight
      decimal volumeMetricWeight = packageVol / dVol;

      decimal chargeableWeight = 0m;

      if (packageKg >= volumeMetricWeight)
      {
        chargeableWeight = packageKg;
      }
      else
      {
        chargeableWeight = volumeMetricWeight;
      }

      return (volumeMetricWeight, chargeableWeight);
    }

    public static string EvaluateStatus(decimal packageKg, int packageLen, int packageWdt, int packageHt, int packageZone, string packageType, decimal packageValue, bool hasInvoice, bool hasInsurance)
    {

      int packageVol = packageLen * packageWdt * packageHt;



      if (packageLen > 150 || packageHt > 150 || packageWdt > 150 || packageVol > 300000)
      {
        return "SHIPMENT REJECTED";
      }

      else if (packageKg > 70 || packageKg <= 0)
      {
        return "SHIPMENT REJECTED";
      }
      else if ((packageType == "Hazardous Material") && (packageZone == 3))
      {
        return "SHIPMENT REJECTED";
      }
      else if ((packageZone < 1) || (packageZone > 3))
      {
        return "SHIPMENT REJECTED";
      }
      else if (packageType == "Hazardous Material")
      {
        if ((packageZone == 1) || (packageZone == 2))
        {
          return "HELD FOR MANUAL REVIEW";
        }
      }
      else if (hasInvoice == false)
      {
        if (packageValue <= 10000.00m)
        {
          return "HELD FOR MANUAL REVIEW";
        }
        else if (packageValue > 500000.00m && hasInsurance == false)
        {
          return "HELD FOR MANUAL REVIEW";
        }
        else if (packageValue > 10000.00m)
        {
          return "SHIPMENT REJECTED";
        }
      }
      return "CLEARED FOR DISPATCH";
    }

    public static (decimal baseFee, decimal surcharge, decimal insuranceFee, decimal
    customsDuty, decimal totalDue) CalculateFees(decimal chargeableWeight, int packageZone, string
    packageType, decimal packageValue, bool hasInsurance)
    {

      // CALCULATE THE BASE FEE
      decimal fixedRate = 0m;

      if (packageZone == 1)
      {
        fixedRate = 120.00m;
      }
      else if (packageZone == 2)
      {
        fixedRate = 350.00m;
      }
      else if (packageZone == 3)
      {
        fixedRate = 780.00m;
      }

      decimal baseFee = chargeableWeight * fixedRate;

      // Calculate the Surcharges 
      decimal surcharge = 0m;

      if (packageType == "Perishable Goods")
      {
        surcharge = baseFee * 0.25m;
      }
      else if (packageType == "Hazardous Material")
      {
        surcharge = baseFee * 0.50m;
      }
      else if (packageType == "High-Value Electronics")
      {
        surcharge = 1500.00m;
      }
      else if (packageType == "General Merchandise")
      {
        surcharge = 0.00m;
      }
      
      // Calculate Insurance Fee

      decimal insuranceFee = 0.00m;

      if (hasInsurance == true)
      {
        insuranceFee = packageValue * 0.015m;
        if (insuranceFee < 500.00m)
        {
          insuranceFee = 500.00m;
        }
      }
      else
      {
        insuranceFee = 0.00m;
      }

      //  Custom Duty
      
      decimal customsDuty = 0.00m;
      if ((packageZone == 2) || (packageZone == 3))
      {
        if (packageValue > 10000.00m && packageValue <= 100000.00m)
        {
          customsDuty = packageValue * 0.05m;
        }
        else if (packageValue > 100000.00m)
        {
          customsDuty = packageValue * 0.12m;
        }
      }

      // Total Due
      decimal totalDue = baseFee + surcharge + insuranceFee + customsDuty;
      
      return (baseFee, surcharge, insuranceFee, customsDuty, totalDue);
    }
    public static (int totalPackages, int clearedCount, int heldCount, int
    rejectedCount, decimal totalRevenue, decimal avgWeight) GenerateBatchSummary(List<decimal>
    weights, List<decimal> totals, List<string> statuses)
    {
      int clearedCount = 0;
      int heldCount = 0;
      int rejectedCount = 0;
      decimal totalRevenue = 0m;
      decimal totalWeight = 0m;
      for (int i = 0; i < statuses.Count; i++)
      {

        totalRevenue += totals[i];
        totalWeight += weights[i];

        if (statuses[i] == "CLEARED FOR DISPATCH")
        {
          clearedCount++;
        }
        else if (statuses[i] == "HELD FOR MANUAL REVIEW")
        {
          heldCount++;
        }
        else if (statuses[i] == "SHIPMENT REJECTED")
        {
          rejectedCount++;
        }
      }
      int totalPackages = statuses.Count;
      decimal avgWeight = 0m;
          if (totalPackages > 0)
          {
              avgWeight = totalWeight / totalPackages;
          }
      return (totalPackages, clearedCount, heldCount, rejectedCount, totalRevenue,
    avgWeight);
    }
  }
}