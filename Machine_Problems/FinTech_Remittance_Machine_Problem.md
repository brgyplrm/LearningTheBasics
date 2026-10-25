# C# Machine Problem: Cross-Border FinTech Remittance Processing & AML Compliance Audit Terminal

## Problem Scenario
You are engaged as a software engineer by an international financial technology (FinTech) corporation that manages an automated cross-border remittance gateway. The gateway processes inbound foreign currency wire transfers, verifies sender identity records, converts foreign currencies to Philippine Pesos (PHP), screens transactions against strict Anti-Money Laundering (AML) regulatory rules, calculates tiered service commissions, and produces audited daily settlement manifests.

Your task is to build a robust, modular, multi-file console application that demonstrates industrial software craftsmanship:
1. Decomposing financial formulas and compliance screening into standalone **modular static methods returning typed C# tuples**.
2. Safeguarding the system against crashes using **defensive parsing (`TryParse`)** and array boundary guards.
3. Managing multi-record batch manifests using **generic `List<T>` collections**.
4. Executing an **interactive, loop-driven console dispatch terminal** with loop control keywords (`break`, `continue`).
5. Reading batch records from disk (`remittance_batch_input.txt`) and exporting audited compliance manifests (`remittance_manifest_output.txt`) using **deterministic resource disposal (`using`)**.
6. Organizing the codebase across segregated logical namespaces (`FinTechRemittance.Core` and `FinTechRemittance.IO`).

---

## Learning Competencies Assessed
* **Defensive Data Ingestion:** Token boundary verification (`parts.Length >= 10`), safe type conversion via `decimal.TryParse`, `int.TryParse`, and `bool.TryParse` with `out` parameters.
* **Control Flow & Looping:** Continuous menu operation (`do-while`), batch accumulation (`for` / `foreach`), skipping corrupt records (`continue`), and graceful termination (`break`).
* **Collections Management:** In-memory batch synchronization using parallel generic lists (`List<string>`, `List<decimal>`, `List<int>`).
* **Modular Methods & Tuples:** Refactoring complex business logic into pure static methods returning multi-value named C# tuples.
* **Separation of Concerns:** Clean architecture separating financial domain rules (`FinTechRemittance.Core`) from persistence and presentation (`FinTechRemittance.IO`).
* **File I/O Safety:** Stream reading and writing with `using (StreamWriter)` to prevent resource leaks and unreleased file locks.

---

## System Architecture & Required Modular Methods

### Namespace Architecture
* `FinTechRemittance.Core`: Houses currency exchange algorithms, AML compliance evaluators, fee calculators, and batch summary analytics.
* `FinTechRemittance.IO`: Houses batch CSV file ingestion, audited manifest file export, and tabular console report generators.

---

### Required Modular Methods in `FinTechRemittance.Core.RemittanceEngine`

#### Method A: Currency Conversion & FX Spread Engine
* **Identifier:** `ConvertCurrencyToPhp`
* **Signature:** `public static (decimal exchangeRate, decimal grossPhpAmount, decimal fxSpreadFee) ConvertCurrencyToPhp(decimal sourceAmount, string currencyCode)`
* **Logic:**
  * Supported Currency Rates (PHP baseline):
    * `USD`: ₱58.20
    * `EUR`: ₱62.50
    * `JPY`: ₱0.38
    * `CAD`: ₱42.10
    * Other / Invalid: ₱0.00
  * Gross PHP Amount = `sourceAmount * exchangeRate`.
  * FX Spread Fee = `grossPhpAmount * 0.0125m` (1.25% institutional conversion buffer).
  * Returns tuple `(exchangeRate, grossPhpAmount, fxSpreadFee)`.

#### Method B: AML Compliance & Sanctions Screening Evaluator
* **Identifier:** `EvaluateAmlStatus`
* **Signature:** `public static string EvaluateAmlStatus(decimal grossPhpAmount, int corridorCode, string transferPurpose, bool isKycVerified, bool isSanctionedEntity, int senderRiskScore)`
* **Return Values:** `"SETTLEMENT REJECTED"`, `"HELD FOR COMPLIANCE REVIEW"`, or `"APPROVED FOR SETTLEMENT"`
* **Strict Priority Hierarchy:**
  1. **Immediate Rejection (`SETTLEMENT REJECTED`):**
     * Any transaction flagged as a sanctioned entity (`isSanctionedEntity == true`).
     * Any single transfer where `grossPhpAmount` exceeds the regulatory ceiling of ₱1,000,000.00.
     * Any sender whose risk score exceeds 85 (valid score range: 1–100).
     * Any invalid destination corridor (`corridorCode < 1 || corridorCode > 3`).
  2. **Hold for Manual Compliance Audit (`HELD FOR COMPLIANCE REVIEW`):**
     * Customer lacks full identity verification (`isKycVerified == false`) on transfers exceeding ₱20,000.00.
     * High-risk destination corridor (`corridorCode == 3` - Non-FATF Monitored Jurisdictions) where `grossPhpAmount > 50,000.00m`.
     * High-risk transfer purposes (`"Cryptocurrency Arbitrage"` or `"Offshore Real Estate"`).
     * Moderate sender risk score between 60 and 85 inclusive.
  3. **Settlement Approval (`APPROVED FOR SETTLEMENT`):**
     * If no rejection and no compliance hold conditions trigger, the transaction is cleared.

#### Method C: Service Commission & Net Payout Calculator
* **Identifier:** `CalculateRemittanceFees`
* **Signature:** `public static (decimal baseCommission, decimal channelFee, decimal expressSurcharge, decimal totalFeeDeducted, decimal netPayoutPhp) CalculateRemittanceFees(decimal grossPhpAmount, string deliveryChannel, bool isExpressTransfer, string amlStatus)`
* **Logic:**
  * If `amlStatus == "SETTLEMENT REJECTED"`, all fees and net payouts evaluate to ₱0.00 (transaction cancelled).
  * **Base Service Commission (Tiered):**
    * Gross PHP up to ₱10,000.00: Flat ₱150.00.
    * Gross PHP between ₱10,000.01 and ₱100,000.00: 1.5% of gross PHP amount.
    * Gross PHP exceeding ₱100,000.00: 1.0% of gross PHP amount.
  * **Delivery Channel Fee:**
    * `"Bank Deposit"`: Flat ₱50.00.
    * `"Cash Pickup"`: Flat ₱120.00.
    * `"Mobile Wallet"` (e.g., GCash / Maya): Flat ₱30.00.
    * Unknown / Other: Flat ₱100.00.
  * **Express Priority Surcharge:**
    * If `isExpressTransfer == true`, add flat ₱250.00 expedited routing fee; otherwise ₱0.00.
  * **Total Fee Deducted:** `baseCommission + channelFee + expressSurcharge`.
  * **Net Payout to Beneficiary:** `grossPhpAmount - totalFeeDeducted`.

#### Method D: Batch Settlement Analytics Generator
* **Identifier:** `GenerateSettlementSummary`
* **Signature:** `public static (int totalTransactions, int approvedCount, int heldCount, int rejectedCount, decimal totalVolumePhp, decimal totalRevenueEarned, decimal avgTransferPhp) GenerateSettlementSummary(List<decimal> grossAmounts, List<decimal> totalFees, List<string> amlStatuses)`
* **Logic:**
  * Iterates across parallel collection indices.
  * Tallies total transactions, counts per status category, total gross PHP volume (only for approved and held transfers), and total fee revenue collected.
  * Calculates average transfer volume: guarded with `if (totalTransactions > 0)` to eliminate `DivideByZeroException` crashes on empty ledgers.

---

## Interactive Console Menu & Operational Flow

The terminal must execute inside a `do-while` or `while` loop displaying:

```text
============================================================
     FINTECH CROSS-BORDER REMITTANCE & AML GATEWAY (M2)
============================================================
  1. Process Single Wire Transfer (Interactive Prompt)
  2. Load Batch Transfers from File (remittance_batch_input.txt)
  3. View Active Settlement Ledger & Compliance Analytics
  4. Export Audited Manifest to File (remittance_manifest_output.txt)
  5. Exit Gateway Terminal
============================================================
Enter option (1-5): 
```

### Menu Behaviors:
* **Option 1 (Process Single Wire Transfer):** Prompts for transaction details (sender name, currency, foreign amount, corridor, purpose, KYC status, sanction status, risk score, channel, express). Computes currency conversion, status, and fees. Automatically assigns incremental tracking identifier (e.g., `TXN-3001`). Appends records to active memory collections.
* **Option 2 (Load Batch Transfers from File):** Ingests `remittance_batch_input.txt` line by line using `File.ReadAllLines` or `StreamReader`. Parses each token, validates record health (`parts.Length >= 10`), evaluates financials and compliance, and adds valid transfers to active lists. Skips corrupt rows cleanly using `continue`.
* **Option 3 (View Active Settlement Ledger & Analytics):** Prints formatted tabular ledger followed by the calculated batch summary metrics from Method D.
* **Option 4 (Export Audited Manifest to File):** Writes pipe-delimited settlement records and footer metrics to `remittance_manifest_output.txt` using `StreamWriter` with `using`.
* **Option 5 (Exit Gateway Terminal):** Closes the session safely and breaks the loop.

---

## Output Display Specifications (Option 3 Sample)

```text
========================================================================================================
                                     ACTIVE SETTLEMENT MANIFEST
========================================================================================================
Txn ID    Sender Name        Curr  Gross (PHP)      Total Fee (PHP)  Net Payout (PHP)  Compliance Status
--------------------------------------------------------------------------------------------------------
TXN-3001  Alice Smith        USD     58,200.00             1,173.00         57,027.00  APPROVED FOR SETTLEMENT
TXN-3002  Victor Markov      EUR     93,750.00                 0.00              0.00  SETTLEMENT REJECTED
TXN-3003  Chen Wei           JPY      7,600.00               180.00          7,420.00  HELD FOR COMPLIANCE REVIEW
TXN-3004  Elena Rostova      CAD    126,300.00             1,563.00        124,737.00  APPROVED FOR SETTLEMENT
--------------------------------------------------------------------------------------------------------
COMPLIANCE & SETTLEMENT ANALYTICS:
  Total Processed    : 4 transactions
  Approved Transfers : 2
  Held for Compliance: 1
  Rejected Transfers : 1
  Total Gross Volume : PHP 192,100.00
  Total Gateway Fees : PHP 2,916.00
  Average Transfer   : PHP 48,025.00
========================================================================================================
```

---

## File Storage Specifications

### 1. Input Batch File (`remittance_batch_input.txt`)
Comma-separated schema with 10 tokens per line:
`TxnId,SenderName,Currency,ForeignAmount,Corridor,Purpose,IsKycVerified,IsSanctioned,RiskScore,DeliveryChannel,IsExpress`

```text
TXN-3001,Alice Smith,USD,1000.00,1,Family Support,true,false,25,Bank Deposit,true
TXN-3002,Victor Markov,EUR,1500.00,2,Commercial Import,true,true,92,Cash Pickup,false
TXN-3003,Chen Wei,JPY,20000.00,1,Tuition Payment,false,false,45,Mobile Wallet,false
TXN-3004,Elena Rostova,CAD,3000.00,2,Consulting Services,true,false,30,Bank Deposit,false
```

### 2. Exported Manifest File (`remittance_manifest_output.txt`)
```text
AUDITED CROSS-BORDER SETTLEMENT MANIFEST
Exported: 2026-10-23
--------------------------------------------------------------------------------
TXN-3001 | Alice Smith | USD 1,000.00 | PHP 58,200.00 | Net: PHP 57,027.00 | APPROVED FOR SETTLEMENT
TXN-3002 | Victor Markov | EUR 1,500.00 | PHP 93,750.00 | Net: PHP 0.00 | SETTLEMENT REJECTED
TXN-3003 | Chen Wei | JPY 20,000.00 | PHP 7,600.00 | Net: PHP 7,420.00 | HELD FOR COMPLIANCE REVIEW
TXN-3004 | Elena Rostova | CAD 3,000.00 | PHP 126,300.00 | Net: PHP 124,737.00 | APPROVED FOR SETTLEMENT
--------------------------------------------------------------------------------
SUMMARY: Total=4 | Approved=2 | Held=1 | Rejected=1 | GrossVol=PHP 192,100.00 | TotalFees=PHP 2,916.00
```

---

## Verification & Test Scenarios

| Transaction ID | Input Parameters | Computed Financials | Compliance Decision |
| :--- | :--- | :--- | :--- |
| **TXN-3001** | Alice Smith \| USD 1,000.00 \| Corridor 1 (Low Risk)<br>Kyc: Yes \| Sanction: No \| Score: 25<br>Channel: Bank Deposit \| Express: Yes | Rate: ₱58.20 \| Gross: ₱58,200.00<br>Comm: ₱873.00 \| Chnl: ₱50.00 \| Exp: ₱250.00<br>Total Fee: ₱1,173.00 \| **Net: ₱57,027.00** | <span class="badge badge-success">APPROVED FOR SETTLEMENT</span> |
| **TXN-3002** | Victor Markov \| EUR 1,500.00 \| Corridor 2 (Moderate)<br>Kyc: Yes \| Sanction: Yes \| Score: 92<br>Channel: Cash Pickup \| Express: No | Rate: ₱62.50 \| Gross: ₱93,750.00<br>*Sanctioned entity violation + Risk Score > 85*<br>Total Fee: ₱0.00 \| **Net: ₱0.00** | <span class="badge badge-danger">SETTLEMENT REJECTED</span> |
| **TXN-3003** | Chen Wei \| JPY 20,000.00 \| Corridor 1 (Low Risk)<br>Kyc: No \| Sanction: No \| Score: 45<br>Channel: Mobile Wallet \| Express: No | Rate: ₱0.38 \| Gross: ₱7,600.00<br>Comm: ₱150.00 \| Chnl: ₱30.00 \| Exp: ₱0.00<br>Total Fee: ₱180.00 \| **Net: ₱7,420.00** | <span class="badge badge-warning">HELD FOR COMPLIANCE REVIEW</span> |
| **TXN-3004** | Elena Rostova \| CAD 3,000.00 \| Corridor 2 (Moderate)<br>Kyc: Yes \| Sanction: No \| Score: 30<br>Channel: Bank Deposit \| Express: No | Rate: ₱42.10 \| Gross: ₱126,300.00<br>Comm: ₱1,263.00 \| Chnl: ₱50.00 \| Exp: ₱0.00<br>Total Fee: ₱1,563.00 \| **Net: ₱124,737.00** | <span class="badge badge-success">APPROVED FOR SETTLEMENT</span> |

---

## Grading & Verification Checklist
- [ ] **Method Modularization:** Business algorithms (FX conversion, AML screening, fee computation, summary generation) reside in standalone static methods returning C# tuples. No monolithic code.
- [ ] **Defensive Ingestion:** Ingests CSV files, verifies token count (`parts.Length >= 10`), and safely parses numeric types with `TryParse`.
- [ ] **Loop Mastery:** Interactive menu runs continuously via `do-while`/`while`; corrupt records are skipped with `continue`; program exits cleanly with `break`.
- [ ] **Parallel Collections:** Maintains synchronization across parallel generic `List<T>` collections without index desynchronization.
- [ ] **Deterministic Resource Management:** File streams are wrapped in `using` blocks to guarantee file handles close immediately.
- [ ] **Namespace Separation:** Domain computation lives under `FinTechRemittance.Core`, while file I/O lives under `FinTechRemittance.IO`.
- [ ] **Zero Warnings / Zero Errors:** Project builds with 0 compilation errors under .NET 8 / 9.
