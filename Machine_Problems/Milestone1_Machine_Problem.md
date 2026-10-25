# C# Machine Problem: International Cargo Freight and Customs Clearance System

## Problem Scenario
A global freight forwarding and logistics company requires a console-based application to evaluate cargo packages, calculate shipping costs, determine customs tariffs, and issue an operational clearance decision.

Your task is to develop this evaluation system using only foundational C# concepts: choosing appropriate data types, handling precision and conversions, calculating arithmetic expressions, formatting output strings, and organizing conditional logic using decision branches.

---

## Program Requirements

The program must collect the following information from the user or test variables:

* **Physical Weight:** The measured parcel weight in kilograms (may contain decimals).
* **Package Dimensions:** The length, width, and height of the parcel in centimeters (whole numbers).
* **Destination Zone:** The destination region code:
  * 1 = Domestic
  * 2 = Regional Asia-Pacific
  * 3 = International Long-Haul
* **Declared Value:** The total commercial value of the contents in Philippine Pesos (money).
* **Cargo Classification:** The cargo category:
  * General merchandise
  * Perishable goods
  * Hazardous materials
  * High-value electronics
* **Commercial Customs Invoice:** Whether the package includes an official commercial invoice (yes or no).
* **Shipping Insurance:** Whether the sender requested transit insurance coverage (yes or no).

---

## Assessment and Calculation Rules

Implement the following rules strictly in the order described.

### 1. Volumetric Weight and Chargeable Weight
Freight carriers calculate costs using whichever weight is larger: the physical weight or the space the package occupies in the cargo hold.

* The volume of the parcel is calculated by multiplying its length, width, and height in centimeters.
* The volumetric weight in kilograms is calculated by dividing the total volume by 5,000. Ensure your division does not truncate decimal points.
* The chargeable weight is determined by comparing the physical weight and the volumetric weight. Whichever is greater becomes the chargeable weight.

---

### 2. Physical Limits and Safety Restrictions
* Any package where any single dimension (length, width, or height) exceeds 150 cm, or where the total volume exceeds 300,000 cubic centimeters, is automatically **REJECTED** for exceeding conveyor belt limits.
* The parcel's physical weight must be greater than zero and cannot exceed 70 kilograms. Any package outside this range is automatically **REJECTED**.
* Hazardous cargo restrictions:
  * Hazardous materials destined for International Long-Haul (Zone 3) are automatically **REJECTED** due to commercial airline safety regulations.
  * Hazardous materials destined for Domestic (Zone 1) or Regional (Zone 2) must be **HELD FOR MANUAL REVIEW** to verify dangerous goods handling certification.

---

### 3. Customs Compliance and Documentation
* Packages without an official commercial invoice:
  * If the declared value exceeds ₱10,000, the package is automatically **REJECTED** for customs non-compliance.
  * If the declared value is ₱10,000 or below, the package must be **HELD FOR MANUAL REVIEW** for manual declaration processing.
* Packages with a declared value exceeding ₱500,000 are classified as high-liability. If the sender did not purchase shipping insurance, the shipment must be **HELD FOR MANUAL REVIEW**.

---

### 4. Shipping Fee and Tariff Computations

#### Base Shipping Cost
The base rate per chargeable kilogram is determined by the destination zone:
* **Zone 1 (Domestic):** ₱120.00 per kilogram
* **Zone 2 (Regional Asia-Pacific):** ₱350.00 per kilogram
* **Zone 3 (International Long-Haul):** ₱780.00 per kilogram
* If an invalid zone number is provided, the shipment is immediately **REJECTED**.

Calculate the base shipping cost by multiplying the chargeable weight by the rate for that zone.

#### Cargo Surcharges
Additional handling fees are applied based on the cargo classification:
* **Perishable goods:** Add a 25% surcharge to the base shipping cost.
* **Hazardous materials:** Add a 50% surcharge to the base shipping cost.
* **High-value electronics:** Add a fixed security handling surcharge of ₱1,500.00.
* **General merchandise:** No surcharge (₱0.00).

#### Transit Insurance Fee
* If insurance was requested: The fee is 1.5% of the declared value, with a minimum required fee of ₱500.00 (whichever is greater).
* If insurance was not requested: The insurance fee is ₱0.00.

#### Customs Duties and Taxes
* **Domestic shipments (Zone 1):** Completely exempt from customs duties (₱0.00).
* **Regional and International shipments (Zone 2 and Zone 3):**
  * Declared value up to ₱10,000.00: Duty-free under the de minimis threshold (₱0.00 duty).
  * Declared value between ₱10,000.01 and ₱100,000.00: 5% customs duty on the declared value.
  * Declared value exceeding ₱100,000.00: 12% customs duty on the declared value.

#### Total Amount Due
Calculate the final total billable charge by summing:
* Base shipping cost
* Cargo surcharge
* Transit insurance fee
* Customs duty

---

### 5. Final Operational Decision Hierarchy
The system must assign exactly one operational outcome:
* **SHIPMENT REJECTED**
* **HELD FOR MANUAL REVIEW**
* **CLEARED FOR DISPATCH**

**Priority Rule:**
Rejection takes priority over Manual Review, and Manual Review takes priority over Clearance.
* If any rule flags the package for rejection, the final decision is **SHIPMENT REJECTED**.
* If no rejection occurred, but one or more rules flagged the package for inspection, the final decision is **HELD FOR MANUAL REVIEW**.
* If the package meets all acceptance rules and requires no special intervention, the final decision is **CLEARED FOR DISPATCH**.

---

## Output Display Requirements

The program must display a structured summary report in the console with formatted numbers, aligned columns, and currency amounts displayed to two decimal places:

```text
============================================================
           CARGO DISPATCH & CUSTOMS INVOICE
============================================================
Package Metrics:
  Actual Weight      : 12.50 kg
  Dimensions         : 40 x 30 x 25 cm (Volumetric: 6.00 kg)
  Chargeable Weight  : 12.50 kg
  Destination        : Zone 2 (Regional)
  Classification     : High-Value Electronics

Financial Breakdown:
  Declared Value     : PHP 120,000.00
  Base Shipping Fee  : PHP 4,375.00
  Surcharge Applied  : PHP 1,500.00
  Insurance Cost     : PHP 1,800.00
  Customs Duty       : PHP 14,400.00
  ----------------------------------------------------------
  TOTAL AMOUNT DUE   : PHP 22,075.00

Operational Status:
  Decision           : CLEARED FOR DISPATCH
============================================================
```

---

## Sample Test Scenarios for Verification

### Scenario A: Standard Regional Shipment
* A parcel weighs 8.0 kg with dimensions 30 cm by 20 cm by 20 cm.
* It is being sent to Zone 2 (Regional Asia-Pacific) carrying general merchandise valued at ₱8,500.00.
* It includes a commercial invoice, and transit insurance was not selected.
* **Expected Result:**
  * Volumetric weight: 2.40 kg, so the chargeable weight is 8.00 kg.
  * Base shipping fee: ₱2,800.00.
  * Surcharges, insurance, and customs duties are all ₱0.00.
  * Total amount due: ₱2,800.00.
  * Decision: **CLEARED FOR DISPATCH**.

### Scenario B: Hazardous Cargo on Long-Haul Route
* A parcel weighs 15.0 kg with dimensions 50 cm by 40 cm by 30 cm.
* It is addressed to Zone 3 (International Long-Haul) containing hazardous materials valued at ₱25,000.00.
* It includes a commercial invoice, and transit insurance was selected.
* **Expected Result:**
  * Hazardous materials are not allowed on Zone 3 passenger routes.
  * Decision: **SHIPMENT REJECTED**.

### Scenario C: Uninvoiced Low-Value Domestic Parcel
* A parcel weighs 3.0 kg with dimensions 20 cm by 15 cm by 10 cm.
* It is shipping domestically (Zone 1) with general merchandise valued at ₱4,000.00.
* The sender did not attach a commercial invoice, and insurance was not selected.
* **Expected Result:**
  * Chargeable weight is 3.00 kg.
  * Missing a commercial invoice for an amount of ₱10,000 or below requires manual verification.
  * Decision: **HELD FOR MANUAL REVIEW**.
