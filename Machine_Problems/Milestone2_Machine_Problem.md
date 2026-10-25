# C# Machine Problem 2: Automated Cargo Batch Processing, Manifest Auditing & File Archiving System

## Problem Scenario
In Milestone 1, you built a single-package evaluation engine. In a busy logistics distribution terminal, processing one parcel at a time and terminating the application is impractical. 

The terminal manager requires a comprehensive, modular console application capable of:
1. Processing **batches of multiple cargo parcels** in a single session without crashing.
2. Managing dynamic manifest records using **C# arrays and generic `List<T>` collections**.
3. Decomposing business calculations into clean, reusable **modular methods with parameters, return types, and tuples**.
4. Providing an **interactive, loop-driven console menu** with input validation and loop control (`break`, `continue`).
5. **Persisting and loading manifest records from disk files** safely using `File` utilities and the `using` statement for resource disposal.
6. Organizing the codebase logically using **namespaces**.

---

## Milestone 2 Learning Competencies Assessed
* **Loops:** `for`, `foreach`, `while`, `do-while`, `break`, `continue`.
* **Collections:** 1D arrays (`string[]`, `decimal[]`), generic lists (`List<string>`, `List<decimal>`).
* **Modular Methods:** Refactoring calculation algorithms into methods with typed parameters, return values, optional arguments, and C# tuples.
* **Scope & Encapsulation:** Enforcing method local scope and block scope `{ }`.
* **Namespaces:** Logical grouping under `CargoLogistics.Core` and `CargoLogistics.IO`.
* **File I/O:** Reading and writing manifests safely (`File.ReadAllLines`, `File.WriteAllLines`, `StreamReader` with `using`).

---

## System Requirements & Architecture

### 1. Namespace Architecture
Organize your application into two logical namespaces:
* `CargoLogistics.Core`: Houses calculation engines, decision evaluators, and batch aggregation algorithms.
* `CargoLogistics.IO`: Houses file-reading, manifest export, and formatted console report generators.

---

### 2. Required Modular Methods
Refactor all operational logic out of top-level code into standalone, testable static methods:

#### Method A: Volumetric & Chargeable Weight Calculator
* **Name:** `CalculateChargeableWeight`
* **Parameters:** `decimal physicalKg`, `int lengthCm`, `int widthCm`, `int heightCm`
* **Return Type:** `(decimal volumetricWeight, decimal chargeableWeight)` *(Tuple)*
* **Logic:** Computes volume ($L \times W \times H$), volumetric weight ($\text{volume} / 5000.0\text{m}$), and returns both the volumetric weight and the greater of physical vs. volumetric weight.

#### Method B: Operational Status Evaluator
* **Name:** `EvaluateStatus`
* **Parameters:** `decimal physicalKg`, `int lengthCm`, `int widthCm`, `int heightCm`, `int zone`, `string cargoType`, `decimal declaredValue`, `bool hasInvoice`, `bool hasInsurance`
* **Return Type:** `string` (`"CLEARED FOR DISPATCH"`, `"HELD FOR MANUAL REVIEW"`, or `"SHIPMENT REJECTED"`)
* **Logic:** Applies Milestone 1 safety, customs, and documentation rules. Ensures rejections take priority over reviews.

#### Method C: Fee & Tariff Computation Engine
* **Name:** `CalculateFees`
* **Parameters:** `decimal chargeableWeight`, `int zone`, `string cargoType`, `decimal declaredValue`, `bool hasInsurance`
* **Return Type:** `(decimal baseFee, decimal surcharge, decimal insuranceFee, decimal customsDuty, decimal totalDue)` *(Tuple)*
* **Logic:**
  * Base fee = chargeable weight $\times$ zone rate (Zone 1: ₱120.00, Zone 2: ₱350.00, Zone 3: ₱780.00).
  * Cargo surcharge = Perishable (+25%), Hazardous (+50%), High-Value Electronics (+₱1,500.00 fixed), General (₱0.00).
  * Insurance fee = If selected, 1.5% of declared value or ₱500.00 minimum (whichever is greater); otherwise ₱0.00.
  * Customs duty = Zone 1: ₱0.00. Zones 2 & 3: ₱0.00 if declared value $\le$ ₱10,000; 5% if between ₱10,000.01 and ₱100,000; 12% if > ₱100,000.
  * Total amount due = Sum of base fee + surcharge + insurance fee + customs duty.

#### Method D: Batch Analytics Generator
* **Name:** `GenerateBatchSummary`
* **Parameters:** `List<decimal> weights`, `List<decimal> totals`, `List<string> statuses`
* **Return Type:** `(int totalPackages, int clearedCount, int heldCount, int rejectedCount, decimal totalRevenue, decimal avgWeight)` *(Tuple)*
* **Logic:** Uses loops to aggregate counts, total revenue, and average weight across the entire batch.

---

### 3. Interactive Main Menu (Loop-Controlled)
The program must run continuously inside a `do-while` or `while` loop until the user explicitly selects option 5 (Exit).

```text
============================================================
       CARGO LOGISTICS DISPATCH & AUDIT TERMINAL (M2)
============================================================
  1. Add Single Package to Batch
  2. Load Package Batch from File (batch_input.txt)
  3. View Active Batch Manifest & Summary
  4. Export Audited Manifest to File (manifest_output.txt)
  5. Exit System
============================================================
Enter option (1-5): 
```

#### Menu Option Behaviors:
* **Option 1 (Add Single Package):** Prompts for physical weight, dimensions ($L, W, H$), zone (1-3), cargo type, declared value, invoice presence, and insurance request. Adds the calculated package record into active memory lists (`List<string> trackingIds`, `List<decimal> chargeableWeights`, `List<decimal> totalFees`, `List<string> decisions`). Automatically generates an incremental tracking ID (e.g., `PKG-1001`, `PKG-1002`).
* **Option 2 (Load Batch from File):** Reads `batch_input.txt` line by line using `File.ReadAllLines` or a `StreamReader` wrapped in a `using` block. Each line represents a package with comma-separated values:
  `TrackingId,PhysicalKg,Length,Width,Height,Zone,CargoType,DeclaredValue,HasInvoice,HasInsurance`
  Parses each record, evaluates the package, and appends it to active memory. If any line has corrupted data, uses `continue` to skip the invalid record without crashing the application.
* **Option 3 (View Active Batch Manifest & Summary):** Iterates over the stored collections with `for` or `foreach` loops to print a table of all processed packages and displays the summary analytics (total packages, cleared count, held count, rejected count, total revenue, and average weight).
* **Option 4 (Export Audited Manifest to File):** Writes all evaluated package data and audit statuses into `manifest_output.txt` using safe file writing methods. Confirms how many records were successfully written.
* **Option 5 (Exit):** Displays a farewell message and breaks out of the main loop.
* **Invalid Input:** If the user enters an unrecognized menu choice, alerts the user and loops back to the menu.

---

## Output Display Specifications

### Active Batch Manifest Table (Menu Option 3):
```text
========================================================================================
                               CURRENT BATCH MANIFEST
========================================================================================
Tracking ID  Chg. Weight  Zone  Cargo Type         Total Due (PHP)  Status
----------------------------------------------------------------------------------------
PKG-1001         8.00 kg     2  General                 2,800.00    CLEARED FOR DISPATCH
PKG-1002        15.00 kg     3  Hazardous                   0.00    SHIPMENT REJECTED
PKG-1003         3.00 kg     1  General                   360.00    HELD FOR MANUAL REVIEW
PKG-1004        12.50 kg     2  High-Value Elect.      22,075.00    CLEARED FOR DISPATCH
----------------------------------------------------------------------------------------
BATCH ANALYTICS:
  Total Processed    : 4 packages
  Cleared Packages   : 2
  Held for Review    : 1
  Rejected Packages  : 1
  Total Revenue      : PHP 25,235.00
  Average Weight     : 9.63 kg
========================================================================================
```

---

## File Format Specifications

### Input Batch File (`batch_input.txt`):
```text
PKG-2001,8.0,30,20,20,2,General,8500,true,false
PKG-2002,15.0,50,40,30,3,Hazardous,25000,true,true
PKG-2003,3.0,20,15,10,1,General,4000,false,false
PKG-2004,12.5,40,30,25,2,High-Value Electronics,120000,true,true
```

### Exported Manifest File (`manifest_output.txt`):
```text
AUDITED SHIPMENT MANIFEST
Exported: 2026-10-22
--------------------------------------------------------------------------------
PKG-2001 | 8.00 kg | Zone 2 | General | PHP 2,800.00 | CLEARED FOR DISPATCH
PKG-2002 | 15.00 kg | Zone 3 | Hazardous | PHP 0.00 | SHIPMENT REJECTED
PKG-2003 | 3.00 kg | Zone 1 | General | PHP 360.00 | HELD FOR MANUAL REVIEW
PKG-2004 | 12.50 kg | Zone 2 | High-Value Electronics | PHP 22,075.00 | CLEARED FOR DISPATCH
--------------------------------------------------------------------------------
SUMMARY: Total=4 | Cleared=2 | Held=1 | Rejected=1 | Revenue=PHP 25,235.00
```

---

## Grading & Verification Checklist
Before submitting or marking Milestone 2 as complete, verify that your code satisfies:
- [ ] No monolithic code: Calculations, status evaluations, and file operations are extracted into modular methods.
- [ ] All loops are used purposefully: `do-while` for menu, `for`/`foreach` for manifests, `while` for streams/validation.
- [ ] Uses C# tuples to return multi-value results from weight, fee, and summary methods.
- [ ] Dynamic lists (`List<T>`) store batch records in memory without fixed array constraints.
- [ ] All file streams utilize `using` statements or static `File` helpers to guarantee proper resource disposal.
- [ ] Code is separated into logical namespaces (`CargoLogistics.Core` and `CargoLogistics.IO`).
- [ ] Application compiles with 0 errors and 0 warnings.
