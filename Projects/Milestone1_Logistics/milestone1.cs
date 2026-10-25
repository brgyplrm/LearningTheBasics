
public static class Milestone1
{
  public static void Run()
  {
    Console.WriteLine("============================================================");
    Console.WriteLine("              CARGO EVALUATION & DISPATCH SYSTEM            ");
    Console.WriteLine("============================================================\n");


    // Inputs
    Console.Write("\nActual weight: ");
    decimal physicalKg = decimal.Parse(Console.ReadLine() ?? "");

    Console.Write("\nPackage Dimension (Length): ");
    int packageLength = int.Parse(Console.ReadLine() ?? "");

    Console.Write("\nPackage Dimension (Width): ");
    int packageWidth = int.Parse(Console.ReadLine() ?? "");

    Console.Write("\nPackage Dimension (Height): ");
    int packageHeight = int.Parse(Console.ReadLine() ?? "");

    int packageVol = packageHeight * packageLength * packageWidth;
    decimal dVol = 5000.0m;
    // volumemetricweight
    decimal volumeMetricWt = packageVol / dVol;

    decimal chargeableWt = 0m;

    if (physicalKg >= volumeMetricWt)
    {
      chargeableWt = physicalKg;
    }
    else
    {
      chargeableWt = volumeMetricWt;
    }

    Console.Write("\nDomestic Value: ( 1 | 2 | 3 ) ");
    int dValue = int.Parse(Console.ReadLine() ?? "");

    decimal fixedRate = 0m;

    if (dValue == 1)
    {
      fixedRate = 120.00m;
    }
    else if (dValue == 2)
    {
      fixedRate = 350.00m;
    }
    else if (dValue == 3)
    {
      fixedRate = 780.00m;
    }

    decimal baseShipping = chargeableWt * fixedRate;

    Console.Write("\nCargo Classification: ");
    string cargoClass = Console.ReadLine() ?? "";

    decimal surCharges = 0m;

    if (cargoClass == "Perishable Goods")
    {
      surCharges = baseShipping * 0.25m;
    }
    else if (cargoClass == "Hazardous Material")
    {
      surCharges = baseShipping * 0.50m;
    }
    else if (cargoClass == "High Value Electronics")
    {
      surCharges = 1500.00m;
    }
    else if (cargoClass == "General Merchandise")
    {
      surCharges = 0.00m;
    }

    Console.Write("\nDoes the package includes official commercial invoice? (yes or no): ");
    string commercialInv = Console.ReadLine() ?? "";


    Console.Write("\nDeclared Value:");
    decimal declaredValue = decimal.Parse(Console.ReadLine() ?? "");

    Console.Write("\nDoes the sender requested an transit insurance (yes or no):  ");
    string transInsurance = Console.ReadLine() ?? "";

    decimal CalcInsu = 0.00m;

    if (transInsurance == "Yes")
    {
      CalcInsu = declaredValue * 0.015m;
      if (CalcInsu < 500.00m){
        CalcInsu = 500.00m;
      }
    }
    else
    {
      CalcInsu = 0.00m;
    }

    decimal customDuty = 0.00m;
    if ((dValue == 2) || (dValue == 3))
    {
      if (declaredValue > 10000.00m && declaredValue <= 100000.00m)
      {
        customDuty = declaredValue * 0.05m;
      }
      else if (declaredValue > 100000.00m)
      {
        customDuty = declaredValue * 0.12m;
      }
    }

    bool isPackageRejected = false;
    bool isPackageAccepted = false;
    bool isPackageManual = false;

    if (packageLength > 150 || packageHeight > 150 || packageWidth > 150 || packageVol > 300000)
    {
      isPackageRejected = true;
    }
    else if (physicalKg > 70 || physicalKg <= 0)
    {
      isPackageRejected = true;
    }
    else if ((cargoClass == "Hazardous Material") && (dValue == 3))
    {
      isPackageRejected = true;
    }
    else if ((dValue < 1) || (dValue > 3))
    {
      isPackageRejected = true;
    }
    else if (cargoClass == "Hazardous Material")
    {
      if ((dValue == 1) || (dValue == 2))
      {
        isPackageManual = true;
      }
    }
    else if (commercialInv == "No")
    {
      if (declaredValue <= 10000.00m)
      {
        isPackageManual = true;
      } else if (declaredValue > 500000.00m && transInsurance == "No")
      {
        isPackageManual = true;
      } else if (declaredValue > 10000.00m){
        isPackageRejected = true;
      }
    }
    else
    {
      isPackageAccepted = true;
    }

      // Output Display
      decimal totalAmountDue = baseShipping + surCharges + CalcInsu + customDuty;

      string finalDecision = "CLEARED FOR DISPATCH";
      if (isPackageRejected)
      {
        finalDecision = "SHIPMENT REJECTED";
      }
      else if (isPackageManual)
      {
        finalDecision = "HELD FOR MANUAL REVIEW";
      }

      string zoneName = dValue switch
      {
        1 => "Domestic",
        2 => "Regional",
        3 => "International Long-Haul",
        _ => "Invalid Zone"
      };

      Console.WriteLine("\n============================================================");
      Console.WriteLine("           CARGO DISPATCH & CUSTOMS INVOICE                 ");
      Console.WriteLine("============================================================");
      Console.WriteLine("Package Metrics:");
      Console.WriteLine($"  Actual Weight      : {physicalKg:N2} kg");
      Console.WriteLine($"  Dimensions         : {packageLength} x {packageWidth} x {packageHeight} cm (Volumetric: {volumeMetricWt:N2} kg)");
      Console.WriteLine($"  Chargeable Weight  : {chargeableWt:N2} kg");
      Console.WriteLine($"  Destination        : Zone {dValue} ({zoneName})");
      Console.WriteLine($"  Classification     : {cargoClass}");
      Console.WriteLine("\nFinancial Breakdown:");
      Console.WriteLine($"  Declared Value     : PHP {declaredValue:N2}");
      Console.WriteLine($"  Base Shipping Fee  : PHP {baseShipping:N2}");
      Console.WriteLine($"  Surcharge Applied  : PHP {surCharges:N2}");
      Console.WriteLine($"  Insurance Cost     : PHP {CalcInsu:N2}");
      Console.WriteLine($"  Customs Duty       : PHP {customDuty:N2}");
      Console.WriteLine("  ----------------------------------------------------------");
      Console.WriteLine($"  TOTAL AMOUNT DUE   : PHP {totalAmountDue:N2}");
      Console.WriteLine("\nOperational Status:");
      Console.WriteLine($"  Decision           : {finalDecision}");
      Console.WriteLine("============================================================\n");
    }
}

/*
================================================================================
                    CODE REVIEW & ARCHITECTURAL AUDIT NOTES
================================================================================
Score: 93 / 100
Status: APPROVED TO PROCEED TO MILESTONE 2 (Console Apps, Loops, Arrays & Clean Code)

1. WHAT IMPROVED (CORE PROGRAMMING PRINCIPLES):
   - Control Flow Flattening: Successfully eliminated deeply nested conditionals
     (the Arrow Anti-Pattern) in favor of linear, readable decision branches.
   - Boundary & Floor Clamping: Mastered conditional value clamping (ensuring
     calculated amounts enforce a mandatory lower-bound minimum threshold).
   - Continuous Range Evaluation: Correctly modeled interval checks using compound
     inequalities (> and <=), preventing precision gaps across decimal domains.
   - Multi-Condition Disjunctions: Implemented compound boolean logic (using ||
     and >) to evaluate multi-dimensional bounds and guard against non-positive inputs.
   - Type Precision Discipline: Maintained clean separation between financial precision
     (decimal) and geometric/physical measurements without integer truncation errors.

2. LOGIC REFINEMENTS FOR PRODUCTION QUALITY:
   A. Independent Rule Evaluation vs. Coupled Scope:
      - The high-liability check (> 500,000 without insurance) should not be scoped
        inside the missing invoice block. In software engineering, independent business
        rules must remain decoupled so one attribute does not accidentally mask another.
   B. Hierarchy and Precedence in Decision Trees:
      - When chaining 'else if' branches, ensure strict precedence: fatal errors
        (Rejections) must evaluate before warnings (Manual Reviews), or be handled via
        independent validation passes so lower-severity branches cannot short-circuit
        critical security filters.

3. CODE HYGIENE & CONVENTIONS:
   - Warning CS0219: Unused local flag 'isPackageAccepted' can be safely pruned.
   - C# Naming Conventions: Follow standard camelCase for local variables without
     abbreviations ('destinationZone' over 'dValue', 'insuranceFee' over 'CalcInsu').

VERDICT:
You have demonstrated solid mastery over primitive types, arithmetic operators,
string formatting, and complex branching logic. You are officially cleared to
advance to Milestone 2.
================================================================================
*/


