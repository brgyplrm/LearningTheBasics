/*
================================================================================
                    C# NOTES - MILESTONE 3
================================================================================

Milestone 3: Advanced Control Flow, Logic, and Defensive Operations

LEARNING PATH:
Path: Add logic to C# console applications (Part 3)
- Module 1: Evaluate Boolean expressions to make decisions in C#
- Module 2: Control variable scope and logic using code blocks in C#
- Module 3: Branch the flow of code using the switch-case construct in C#
- Module 4: Iterate through a block of code using the for statement in C#
- Module 5: Add looping logic to your code using do-while and while statements in C#
- Module 6: Defensive coding challenges (Input validation, data sanitization, string parsing)

YOU CAN MOVE ON WHEN:
[ ] You can explain the difference between equality (==), inequality (!=), and logical negation (!).
[ ] You know how to safely evaluate user string inputs using Trim(), ToLower(), and ToUpper().
[ ] You can write and read ternary expressions (?:) and know when they hurt code readability.
[ ] You understand variable scope, block lifetimes {}, CS0103 context errors, and CS0165 unassigned variable rules.
[ ] You know Microsoft's official style guidelines regarding single-line if statements and curly brace usage.
[ ] You can build switch-case constructs, use multiple case labels per section, and avoid fall-through compilation errors.
[ ] You can break down the 6 components of a for loop (keyword, condition parentheses, initializer, condition, iterator, body).
[ ] You can explain why foreach iteration variables cannot be modified and how a for loop overcomes this.
[ ] You understand do-while vs while execution guarantees (at least once vs zero or more).
[ ] You can implement continue to skip loop iterations and break to terminate loops prematurely.
[ ] You can defensively validate numeric input with int.TryParse and safely handle nullable strings (string?).
[ ] You can parse delimited strings using IndexOf, Substring, Remove, and TrimStart without throwing runtime exceptions.
================================================================================
*/

#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS8321 // The local function is declared but never used
#pragma warning disable CS0168 // Variable is declared but never used

public static class Milestone3Notes
{
    public static void Run()
    {
        Console.WriteLine("================================================================================");
        Console.WriteLine("                         MILESTONE 3 LEARNING DEMONSTRATION                     ");
        Console.WriteLine("================================================================================\n");

        // =====================================================================
        // MODULE 1: BOOLEAN EXPRESSIONS & STRING COMPARISONS
        // =====================================================================

        // ---------------------------------------------------------------------
        // 1. EQUALITY AND INEQUALITY OPERATORS (==, !=)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("--- 1. Equality and Inequality Operators ---");
        Console.WriteLine("a" == "a");
        Console.WriteLine("a" == "A");
        Console.WriteLine(1 == 2);

        string testLetter = "a";
        Console.WriteLine(testLetter == "a");

        Console.WriteLine("a" != "a");
        Console.WriteLine("a" != "A");
        Console.WriteLine(1 != 2);
        Console.WriteLine(testLetter != "a");

        // DESC IN COMMENT:
        // Evaluates whether two operands hold identical or differing values.

        // WHAT DOES IT DO:
        // The equality operator (==) returns true if both operands are equivalent, false otherwise.
        // The inequality operator (!=) returns true if the operands differ, false otherwise.
        // For value types (int, bool), values are compared directly.
        // For strings, C# checks ordinal value equivalence (character by character).

        // OUTPUT:
        // True
        // False
        // False
        // True
        // False
        // True
        // True
        // False

        // NOTE:
        // C# string comparisons are case-sensitive by default: "a" does not equal "A".


        // ---------------------------------------------------------------------
        // 2. STRING SANITIZATION AND COMPARISON (Trim(), ToLower(), ToUpper())
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 2. String Sanitization and Normalization ---");
        string rawInput1 = " a";
        string rawInput2 = "A ";

        Console.WriteLine($"Raw comparison: {rawInput1 == rawInput2}");

        string normalized1 = rawInput1.Trim().ToLower();
        string normalized2 = rawInput2.Trim().ToLower();
        Console.WriteLine($"Normalized comparison: {normalized1 == normalized2}");

        // DESC IN COMMENT:
        // Cleans up whitespace and normalizes character casing before checking equality.

        // WHAT DOES IT DO:
        // Trim() strips leading and trailing whitespace characters.
        // ToLower() converts all alphabetical characters to lowercase.
        // Chaining .Trim().ToLower() ensures comparisons match user intent regardless of casing or accidental spaces.

        // OUTPUT:
        // Raw comparison: False
        // Normalized comparison: True

        // NOTE:
        // Always sanitize user keyboard input before comparison. Without normalization, " yes " will not match "yes".


        // ---------------------------------------------------------------------
        // 3. RELATIONAL COMPARISON OPERATORS (>, <, >=, <=)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 3. Relational Comparison Operators ---");
        Console.WriteLine(1 > 2);
        Console.WriteLine(1 < 2);
        Console.WriteLine(1 >= 1);
        Console.WriteLine(1 <= 1);

        // DESC IN COMMENT:
        // Compares the relative magnitude of numeric operands.

        // WHAT DOES IT DO:
        // Returns a boolean indicating if the left operand is strictly greater (>), strictly less (<),
        // greater than or equal (>=), or less than or equal (<=) to the right operand.

        // OUTPUT:
        // False
        // True
        // True
        // True

        // NOTE:
        // Relational operators work on numerical types (int, double, decimal, float, long) and char (based on ASCII/Unicode value).


        // ---------------------------------------------------------------------
        // 4. BOOLEAN-RETURNING METHODS (Contains, StartsWith, EndsWith)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 4. Boolean-Returning Methods ---");
        string pangram = "The quick brown fox jumps over the lazy dog.";

        Console.WriteLine(pangram.Contains("fox"));
        Console.WriteLine(pangram.Contains("cow"));
        Console.WriteLine(pangram.StartsWith("The"));
        Console.WriteLine(pangram.EndsWith("dog."));

        // DESC IN COMMENT:
        // Calls built-in type methods that return a boolean value for use in decisions.

        // WHAT DOES IT DO:
        // Contains checks whether a substring exists anywhere inside the target string.
        // StartsWith and EndsWith check boundaries at index 0 and the final character.

        // OUTPUT:
        // True
        // False
        // True
        // True

        // NOTE:
        // Any method returning bool can serve directly as the condition of an if statement or loop without needing == true.


        // ---------------------------------------------------------------------
        // 5. LOGICAL NEGATION OPERATOR (!) VS INEQUALITY (!=)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 5. Logical Negation Operator (!) ---");
        Console.WriteLine(!pangram.Contains("fox"));
        Console.WriteLine(!pangram.Contains("cow"));

        // Comparing logical negation vs explicit equality check:
        bool isFoxAbsentVerbose = pangram.Contains("fox") == false;
        bool isFoxAbsentIdiomatic = !pangram.Contains("fox");
        Console.WriteLine($"Negation equality: {isFoxAbsentVerbose == isFoxAbsentIdiomatic}");

        // DESC IN COMMENT:
        // The unary negation operator (!) inverts the truth value of any boolean expression.

        // WHAT DOES IT DO:
        // If an expression evaluates to true, ! turns it into false; if false, ! turns it into true.
        // Do not confuse the unary negation operator (!) with the binary inequality operator (!=).
        // x != y evaluates whether two values are distinct.
        // !(x == y) inverts the result of an equality evaluation, achieving the exact same result.

        // OUTPUT:
        // False
        // True
        // Negation equality: True

        // NOTE:
        // Avoid comparing booleans with == false or == true. Write !isReady instead of isReady == false.


        // =====================================================================
        // MODULE 2: THE CONDITIONAL (TERNARY) OPERATOR (?:)
        // =====================================================================

        // ---------------------------------------------------------------------
        // 6. BASIC CONDITIONAL OPERATOR USAGE
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 6. Basic Conditional Operator Usage ---");
        int saleAmount = 1001;
        int discount = saleAmount > 1000 ? 100 : 50;
        Console.WriteLine($"Discount: {discount}");

        // DESC IN COMMENT:
        // Evaluates a binary condition inline and returns one of two values.

        // WHAT DOES IT DO:
        // Syntax pattern: <condition> ? <value_if_true> : <value_if_false>
        // If saleAmount > 1000 evaluates to true, 100 is returned; otherwise 50 is returned.
        // The conditional operator is an expression, meaning it returns a value and can be assigned to a variable.

        // OUTPUT:
        // Discount: 100

        // NOTE:
        // Both the true and false branch expressions must evaluate to compatible types that the compiler can resolve.


        // ---------------------------------------------------------------------
        // 7. INLINE INTERPOLATED TERNARY EXPRESSIONS
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 7. Inline Interpolated Ternary Expressions ---");
        int orderTotal = 850;
        Console.WriteLine($"Discount: {(orderTotal > 1000 ? 100 : 50)}");

        // DESC IN COMMENT:
        // Embeds the ternary operator directly inside a string interpolation expression.

        // WHAT DOES IT DO:
        // Parentheses around the ternary expression ((orderTotal > 1000 ? 100 : 50)) are mandatory.
        // Without parentheses, C# interprets the colon (:) as a string format specifier delimiter, causing a compilation error.

        // OUTPUT:
        // Discount: 50

        // NOTE:
        // Keep inline ternary expressions brief. If the expression is long or complex, assign it to a named variable first for clarity.


        // ---------------------------------------------------------------------
        // 8. TERNARY VS IF-ELSE: READABILITY & ANTI-PATTERNS
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 8. Ternary vs If-Else Decisions ---");
        int customerAge = 25;

        // Clean ternary for simple assignment:
        string ticketType = customerAge >= 65 ? "Senior" : "Standard";
        Console.WriteLine($"Ticket: {ticketType}");

        // ANTI-PATTERN TO AVOID: Nested Ternaries
        // int points = 150;
        // string tier = points > 200 ? "Platinum" : points > 100 ? "Gold" : points > 50 ? "Silver" : "Bronze"; // Hard to read

        // RECOMMENDED: Use if-else if-else or switch for 3+ branches:
        string tier;
        int points = 150;
        if (points > 200) tier = "Platinum";
        else if (points > 100) tier = "Gold";
        else if (points > 50) tier = "Silver";
        else tier = "Bronze";
        Console.WriteLine($"Member Tier: {tier}");

        // DESC IN COMMENT:
        // Establishes clear boundaries for when to choose ternary over structured if-else blocks.

        // WHAT DOES IT DO:
        // Use the ternary operator strictly for concise binary value assignments.
        // Never nest ternary operators in production code; nested ternaries degrade code readability and conceal edge cases.

        // OUTPUT:
        // Ticket: Standard
        // Member Tier: Gold

        // NOTE:
        // The ternary operator is NOT a replacement for all if-else logic. It is specifically a value-returning expression.


        // =====================================================================
        // MODULE 3: CODE BLOCKS & VARIABLE SCOPE
        // =====================================================================

        // ---------------------------------------------------------------------
        // 9. CODE BLOCKS AND BLOCK-LEVEL VARIABLE SCOPE
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 9. Block-Level Variable Scope ---");
        bool isUserAuthenticated = true;

        if (isUserAuthenticated)
        {
            int sessionTimeoutSeconds = 300;
            Console.WriteLine($"Inside code block: session timeout = {sessionTimeoutSeconds}s");
        }

        // DESC IN COMMENT:
        // Statements enclosed within curly braces {} form a lexical code block.

        // WHAT DOES IT DO:
        // Variables declared inside a code block are allocated on the stack and visible only within that block.
        // Once execution exits the closing brace }, the variable leaves scope and ceases to exist.

        // OUTPUT:
        // Inside code block: session timeout = 300s

        // NOTE:
        // Outer scopes cannot see variables declared in inner scopes. Inner scopes CAN see variables declared in outer scopes.


        // ---------------------------------------------------------------------
        // 10. COMPILER ERROR CS0103: ACCESSING VARIABLES OUTSIDE SCOPE
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 10. Scope Boundaries and CS0103 ---");
        bool conditionFlag = true;

        if (conditionFlag)
        {
            int localSecret = 42;
            Console.WriteLine($"Local secret accessible inside: {localSecret}");
        }

        // The following line would fail to compile:
        // Console.WriteLine(localSecret);
        // COMPILER ERROR CS0103: The name 'localSecret' does not exist in the current context.

        // DESC IN COMMENT:
        // Attempting to access an inner variable from an outer scope causes compile-time error CS0103.

        // WHAT DOES IT DO:
        // The C# compiler enforces strict lexical scope rules. Because localSecret was declared inside the if block,
        // any line outside that block does not have access to its memory symbol.

        // OUTPUT:
        // Local secret accessible inside: 42

        // NOTE:
        // If a variable needs to be accessed after a code block, declare it in the outer scope ABOVE the code block.


        // ---------------------------------------------------------------------
        // 11. VARIABLE HOISTING AND COMPILER ERROR CS0165 (DEFINITE ASSIGNMENT)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 11. Definite Assignment and CS0165 ---");
        bool triggerFlag = true;

        // INCORRECT PATTERN:
        // int uninitializedVal;
        // if (triggerFlag) uninitializedVal = 10;
        // Console.WriteLine(uninitializedVal); // COMPILER ERROR CS0165: Use of unassigned local variable

        // CORRECT PATTERN: Always initialize local variables
        int properlyInitializedVal = 0;
        if (triggerFlag)
        {
            properlyInitializedVal = 10;
            Console.WriteLine($"Inside block: {properlyInitializedVal}");
        }
        Console.WriteLine($"Outside block: {properlyInitializedVal}");

        // DESC IN COMMENT:
        // Local variables in C# must be definitely assigned before their values can be read.

        // WHAT DOES IT DO:
        // The compiler performs static analysis on every possible execution branch.
        // If there exists even one branch where the variable might remain unassigned, reading it generates error CS0165.
        // Initializing the variable at declaration (e.g. int value = 0;) satisfies definite assignment.

        // OUTPUT:
        // Inside block: 10
        // Outside block: 10

        // NOTE:
        // Class fields receive default values (0, null, false) automatically, but LOCAL variables inside methods do NOT.


        // ---------------------------------------------------------------------
        // 12. COMPILER EXECUTION PATH ANALYSIS (if (flag) vs if (true))
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 12. Compiler Static Branch Analysis ---");

        // Case A: Variable condition
        // bool runtimeFlag = true;
        // int valA;
        // if (runtimeFlag) { valA = 50; }
        // Console.WriteLine(valA); // ERROR CS0165: Compiler treats runtimeFlag as potentially false at runtime.

        // Case B: Constant literal condition
        int valB;
        if (true)
        {
            valB = 100;
        }
        Console.WriteLine($"Constant branch proven assigned: {valB}");

        // DESC IN COMMENT:
        // Demonstrates how the C# compiler evaluates determinism in code branches.

        // WHAT DOES IT DO:
        // When checking `if (runtimeFlag)`, the compiler assumes runtimeFlag could evaluate to false,
        // leaving valA unassigned.
        // When checking `if (true)`, the compiler proves during compilation that the block is 100% guaranteed to run,
        // so valB is recognized as definitely assigned.

        // OUTPUT:
        // Constant branch proven assigned: 100

        // NOTE:
        // Never rely on runtime conditions to satisfy compile-time definite assignment. Always initialize your locals.


        // =====================================================================
        // MODULE 4: CODE BLOCK STYLISTIC CONVENTIONS & SINGLE-LINE IF
        // =====================================================================

        // ---------------------------------------------------------------------
        // 13. OMITTING CURLY BRACES FOR SINGLE-STATEMENT BODIES
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 13. Single-Statement If Without Braces ---");
        bool isDeviceOnline = true;

        // Single-statement body on a separate indented line:
        if (isDeviceOnline)
            Console.WriteLine("Telemetry received: Device is online.");

        // DESC IN COMMENT:
        // C# syntax allows omitting curly braces {} when an if branch contains exactly one statement.

        // WHAT DOES IT DO:
        // Executes the single statement immediately following the condition when true.
        // The subsequent statement belongs to the outer scope regardless of indentation.

        // OUTPUT:
        // Telemetry received: Device is online.

        // NOTE:
        // Indentation in C# is visual whitespace only; it does not define code blocks like in Python.


        // ---------------------------------------------------------------------
        // 14. READABILITY HAZARDS & MICROSOFT OFFICIAL STYLE GUIDELINES
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 14. Readability and Style Conventions ---");
        string selectedUser = "steve";

        // BAD PRACTICE: Single-line compressed statement (Dense and error-prone)
        // if (selectedUser == "bob") Console.WriteLine("Bob"); else if (selectedUser == "steve") Console.WriteLine("Steve");

        // ACCEPTABLE: Single statements on separate lines
        if (selectedUser == "bob")
            Console.WriteLine("Matched: Bob");
        else if (selectedUser == "steve")
            Console.WriteLine("Matched: Steve");
        else
            Console.WriteLine("Matched: Unknown");

        // INDUSTRY BEST PRACTICE: Explicit curly braces for all compound branches
        if (selectedUser == "bob")
        {
            Console.WriteLine("Target identified as Bob.");
        }
        else if (selectedUser == "steve")
        {
            Console.WriteLine("Target identified as Steve.");
        }
        else
        {
            Console.WriteLine("Target is unclassified.");
        }

        // DESC IN COMMENT:
        // Evaluates formatting choices against Microsoft's official C# coding standards.

        // WHAT DOES IT DO:
        // Microsoft Style Guidelines:
        // 1. Never use single-line form (e.g. if (flag) DoSomething();).
        // 2. Put single statement bodies on their own line with an indent.
        // 3. If any branch in an if-else if-else chain uses braces, ALL branches must use braces.
        // 4. Using curly braces everywhere prevents dangling-else bugs and makes maintenance safe.

        // OUTPUT:
        // Matched: Steve
        // Target identified as Steve.

        // NOTE:
        // The famous Apple "goto fail" security bug was caused by omitting curly braces. Always prefer explicit braces.


        // =====================================================================
        // MODULE 5: BRANCHING WITH SWITCH-CASE
        // =====================================================================

        // ---------------------------------------------------------------------
        // 15. SWITCH-CASE WITH DEFAULT & FALL-THROUGH PREVENTION
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 15. Switch-Case with Default ---");
        int securityClearance = 300;
        string clearanceLevel = "";

        switch (securityClearance)
        {
            case 100:
                clearanceLevel = "Public Access";
                break;
            case 200:
                clearanceLevel = "Confidential";
                break;
            case 300:
                clearanceLevel = "Secret";
                break;
            case 400:
                clearanceLevel = "Top Secret";
                break;
            default:
                clearanceLevel = "Unclassified / Pending Review";
                break;
        }

        Console.WriteLine($"Clearance: {securityClearance} -> {clearanceLevel}");

        // DESC IN COMMENT:
        // Directs execution flow by comparing a single expression against matching case labels.

        // WHAT DOES IT DO:
        // Evaluates the match expression (securityClearance) from top to bottom.
        // Executes the matching case section until encountering break, which exits the switch block.
        // If no case label matches, the default section executes.
        // C# strictly prevents accidental fall-through: non-empty case sections MUST terminate with break, return, or throw.

        // OUTPUT:
        // Clearance: 300 -> Secret

        // NOTE:
        // The default section is logically evaluated last, even if placed at the top of the switch statement.


        // ---------------------------------------------------------------------
        // 16. MULTIPLE CASE LABELS MATCHING A SINGLE CODE BLOCK
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 16. Multiple Labels Sharing a Switch Section ---");
        int employeeTier = 100;
        string corporateTitle = "";

        switch (employeeTier)
        {
            case 100:
            case 200:
                corporateTitle = "Senior Associate";
                break;
            case 300:
                corporateTitle = "Manager";
                break;
            case 400:
                corporateTitle = "Director";
                break;
            default:
                corporateTitle = "Contractor";
                break;
        }

        Console.WriteLine($"Employee Tier {employeeTier}: {corporateTitle}");

        // DESC IN COMMENT:
        // Stacks multiple case labels together so that either value triggers the same code block.

        // WHAT DOES IT DO:
        // If employeeTier is 100 OR 200, execution lands on the same assignment: corporateTitle = "Senior Associate".
        // This is permitted because case 100 contains no statements; empty labels can fall through directly to the next label.

        // OUTPUT:
        // Employee Tier 100: Senior Associate

        // NOTE:
        // Stacking empty case labels avoids duplicate logic and keeps code DRY (Don't Repeat Yourself).


        // =====================================================================
        // MODULE 6: THE FOR ITERATION LOOP
        // =====================================================================

        // ---------------------------------------------------------------------
        // 17. THE SIX STRUCTURAL PARTS OF A FOR LOOP
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 17. Structural Parts of a For Loop ---");
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"Iteration index: {i}");
        }

        // DESC IN COMMENT:
        // Executes a block of code a predetermined number of times using explicit loop control.

        // WHAT DOES IT DO:
        // The 6 structural components:
        // 1. 'for' keyword: instructs the compiler to begin a counted loop.
        // 2. Parentheses (): encapsulates the iteration contract.
        // 3. Initializer (int i = 0): runs once before the first iteration to declare the counter variable.
        // 4. Condition (i < 5): evaluated before each pass; if true, the body runs; if false, loop ends.
        // 5. Iterator (i++): runs after each pass to increment the counter.
        // 6. Body {}: the code statements executed during each cycle.

        // OUTPUT:
        // Iteration index: 0
        // Iteration index: 1
        // Iteration index: 2
        // Iteration index: 3
        // Iteration index: 4

        // NOTE:
        // The loop counter variable (i) is locally scoped to the for loop and is invisible once the loop terminates.


        // ---------------------------------------------------------------------
        // 18. CUSTOM ITERATION PATTERNS: COUNTDOWN & STEP STRIDES
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 18. Countdown and Custom Step Strides ---");

        // Countdown pattern:
        Console.Write("Countdown: ");
        for (int count = 5; count >= 0; count--)
        {
            Console.Write($"{count} ");
        }
        Console.WriteLine();

        // Custom step stride pattern (increment by 3):
        Console.Write("Step by 3: ");
        for (int step = 0; step < 10; step += 3)
        {
            Console.Write($"{step} ");
        }
        Console.WriteLine();

        // DESC IN COMMENT:
        // Configures the initializer, condition, and step expression to traverse sequences non-sequentially.

        // WHAT DOES IT DO:
        // Countdowns initialize at the upper bound and decrement with count-- until reaching 0.
        // Step strides use compound assignment (step += 3) to skip intermediate numbers.

        // OUTPUT:
        // Countdown: 5 4 3 2 1 0 
        // Step by 3: 0 3 6 9 

        // NOTE:
        // Always verify that the step direction matches the condition to prevent unintended infinite loops.


        // ---------------------------------------------------------------------
        // 19. PREMATURE LOOP TERMINATION WITH BREAK
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 19. Premature Termination with Break ---");
        for (int index = 0; index < 10; index++)
        {
            if (index == 4)
            {
                Console.WriteLine($"Early exit triggered at index: {index}");
                break;
            }
            Console.WriteLine($"Processing item: {index}");
        }

        // DESC IN COMMENT:
        // Halts loop execution immediately when a specific condition is fulfilled.

        // WHAT DOES IT DO:
        // Evaluates index on each pass. Once index reaches 4, the break statement terminates the loop,
        // transferring control to the statement directly following the loop block.

        // OUTPUT:
        // Processing item: 0
        // Processing item: 1
        // Processing item: 2
        // Processing item: 3
        // Early exit triggered at index: 4

        // NOTE:
        // In nested loops, break only exits the innermost loop containing the statement.


        // ---------------------------------------------------------------------
        // 20. REVERSE ARRAY TRAVERSAL WITH ZERO-BASED INDEXING
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 20. Reverse Array Traversal ---");
        string[] fleetVehicles = { "Truck-A", "Van-B", "Sedan-C", "Drone-D" };

        for (int idx = fleetVehicles.Length - 1; idx >= 0; idx--)
        {
            Console.WriteLine($"Position [{idx}]: {fleetVehicles[idx]}");
        }

        // DESC IN COMMENT:
        // Iterates through array elements from the last element to the first.

        // WHAT DOES IT DO:
        // Arrays in C# are zero-indexed. An array of 4 items has indices 0, 1, 2, 3.
        // The upper bound index is fleetVehicles.Length - 1 (4 - 1 = 3).
        // Initializing at Length - 1 and decrementing with idx-- traverses the array in reverse.

        // OUTPUT:
        // Position [3]: Drone-D
        // Position [2]: Sedan-C
        // Position [1]: Van-B
        // Position [0]: Truck-A

        // NOTE:
        // Attempting to start at fleetVehicles.Length causes an IndexOutOfRangeException runtime crash.


        // ---------------------------------------------------------------------
        // 21. MUTATING ARRAY ELEMENTS: FOR VS FOREACH LIMITATION
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 21. Array Mutation: For vs Foreach ---");
        string[] engineers = { "Alex", "Eddie", "David", "Michael" };

        // FOREACH LIMITATION:
        // foreach (var person in engineers)
        // {
        //     if (person == "David") person = "Sammy"; // COMPILER ERROR CS1656: Cannot assign to 'person' because it is a foreach iteration variable
        // }

        // FOR LOOP SOLUTION:
        for (int i = 0; i < engineers.Length; i++)
        {
            if (engineers[i] == "David")
            {
                engineers[i] = "Sammy"; // Mutating underlying array cell via index
            }
        }

        Console.Write("Updated array contents: ");
        foreach (var person in engineers)
        {
            Console.Write($"{person} ");
        }
        Console.WriteLine();

        // DESC IN COMMENT:
        // Demonstrates why in-place collection modification requires a for loop rather than foreach.

        // WHAT DOES IT DO:
        // The foreach iteration variable is a read-only local reference managed by the IEnumerable enumerator.
        // Trying to assign to it triggers compiler error CS1656.
        // A for loop accesses the array's underlying memory slot by index (engineers[i]), allowing direct mutation.

        // OUTPUT:
        // Updated array contents: Alex Eddie Sammy Michael 

        // NOTE:
        // Use foreach for read-only scans. Use for when updating, replacing, or deleting items by index.


        // =====================================================================
        // MODULE 7: WHILE & DO-WHILE LOOPS
        // =====================================================================

        // ---------------------------------------------------------------------
        // 22. DO-WHILE LOOP: GUARANTEED MINIMUM EXECUTION (POST-TEST)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 22. Do-While Loop (Guaranteed At Least Once) ---");
        Random deterministicRng = new Random(101); // Fixed seed for reproducible output
        int rolledValue = 0;
        int rollsCount = 0;

        do
        {
            rolledValue = deterministicRng.Next(1, 11);
            rollsCount++;
            Console.WriteLine($"Roll #{rollsCount}: Generated {rolledValue}");
        } while (rolledValue != 7);

        Console.WriteLine($"Target 7 reached after {rollsCount} rolls.");

        // DESC IN COMMENT:
        // Executes the code block first, then evaluates the continuation condition.

        // WHAT DOES IT DO:
        // Because the boolean condition is evaluated after the block executes, a do-while loop is guaranteed
        // to execute at least one time, even if the condition initially starts out false.

        // OUTPUT:
        // Roll #1: Generated 2
        // Roll #2: Generated 10
        // Roll #3: Generated 7
        // Target 7 reached after 3 rolls.

        // NOTE:
        // Always ensure the loop body modifies variables involved in the continuation condition to avoid infinite loops.


        // ---------------------------------------------------------------------
        // 23. WHILE LOOP: CONDITIONAL GATEKEEPER (PRE-TEST)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 23. While Loop (Pre-Condition Gatekeeper) ---");
        int energyLevel = 5;

        while (energyLevel >= 2)
        {
            Console.WriteLine($"Device active. Energy remaining: {energyLevel}");
            energyLevel -= 2;
        }

        Console.WriteLine($"Device suspended. Final energy: {energyLevel}");

        // DESC IN COMMENT:
        // Evaluates the boolean condition before entering the code block.

        // WHAT DOES IT DO:
        // If energyLevel starts below 2, the loop body executes zero times.
        // The loop acts as a gatekeeper, admitting execution only while the condition evaluates to true.

        // OUTPUT:
        // Device active. Energy remaining: 5
        // Device active. Energy remaining: 3
        // Device suspended. Final energy: 1

        // NOTE:
        // Select while when you need to prevent execution under invalid initial conditions.


        // ---------------------------------------------------------------------
        // 24. LOOP CONTROL: CONTINUE VS BREAK
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 24. Continue vs Break in Loops ---");
        int iterationStep = 0;

        while (iterationStep < 8)
        {
            iterationStep++;

            // Skip even numbers using continue:
            if (iterationStep % 2 == 0)
            {
                continue;
            }

            // Stop loop early when reaching 7 using break:
            if (iterationStep == 7)
            {
                Console.WriteLine($"Break triggered at: {iterationStep}");
                break;
            }

            Console.WriteLine($"Odd number processed: {iterationStep}");
        }

        // DESC IN COMMENT:
        // Contrasts skipping an iteration (continue) with terminating the loop entirely (break).

        // WHAT DOES IT DO:
        // continue halts the current cycle and jumps directly to the condition test (in while/do-while) or iterator (in for).
        // break aborts the loop entirely and transfers control out of the loop block.

        // OUTPUT:
        // Odd number processed: 1
        // Odd number processed: 3
        // Odd number processed: 5
        // Break triggered at: 7

        // NOTE:
        // When using continue inside a while loop, make sure the loop counter increments BEFORE the continue statement;
        // otherwise, you will create an accidental infinite loop.


        // =====================================================================
        // MODULE 8: DEFENSIVE USER INPUT VALIDATION
        // =====================================================================

        // ---------------------------------------------------------------------
        // 25. CAPTURING USER INPUT SAFELY WITH NULLABLE STRINGS (string?)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 25. Handling Nullable User Input ---");
        // In interactive runs: string? rawTerminalInput = Console.ReadLine();
        // Demonstration with simulated input stream:
        string? simulatedInput = "   ValidDataEntry   ";

        string safeInput = simulatedInput?.Trim() ?? string.Empty;
        Console.WriteLine($"Sanitized string: '{safeInput}' (Length: {safeInput.Length})");

        // DESC IN COMMENT:
        // Safely handles potential null references returned by terminal input streams.

        // WHAT DOES IT DO:
        // Console.ReadLine() returns string? (a nullable reference type), which is null if end-of-stream (EOF/Ctrl+Z/Ctrl+D) is reached.
        // Using null-conditional operator (?.) and null-coalescing (?? string.Empty) prevents NullReferenceException crashes.

        // OUTPUT:
        // Sanitized string: 'ValidDataEntry' (Length: 14)

        // NOTE:
        // Always treat user input as untrusted and potentially null or empty.


        // ---------------------------------------------------------------------
        // 26. NON-CRASHING NUMERIC PARSING WITH int.TryParse
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 26. Safe Numeric Parsing with int.TryParse ---");
        string[] sampleInputs = { "two", "2", "7", "15" };
        int acceptedValue = 0;

        foreach (string candidate in sampleInputs)
        {
            Console.WriteLine($"Evaluating user input: '{candidate}'");

            if (!int.TryParse(candidate, out int parsedNumber))
            {
                Console.WriteLine("  Error: Input is not a valid integer. Please try again.");
                continue;
            }

            if (parsedNumber < 5 || parsedNumber > 10)
            {
                Console.WriteLine($"  Error: Value {parsedNumber} is out of range. Must be between 5 and 10.");
                continue;
            }

            acceptedValue = parsedNumber;
            Console.WriteLine($"  Success: Input value ({acceptedValue}) has been accepted.");
            break;
        }

        // DESC IN COMMENT:
        // Converts text to an integer without throwing exceptions on malformed input.

        // WHAT DOES IT DO:
        // int.Parse("abc") throws a FormatException that crashes your program if unhandled.
        // int.TryParse("abc", out int val) returns false on failure and assigns 0 to val, never throwing an exception.
        // Combined with range checks (5 <= val && val <= 10), it builds resilient defensive input pipelines.

        // OUTPUT:
        // Evaluating user input: 'two'
        //   Error: Input is not a valid integer. Please try again.
        // Evaluating user input: '2'
        //   Error: Value 2 is out of range. Must be between 5 and 10.
        // Evaluating user input: '7'
        //   Success: Input value (7) has been accepted.

        // NOTE:
        // In high-performance backend APIs, int.TryParse avoids costly exception generation on invalid payloads.


        // ---------------------------------------------------------------------
        // 27. NORMALIZED STRING WHITELIST VALIDATION (ROLE FILTER)
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 27. Normalized Role Whitelist Validation ---");
        string[] roleEntries = { "Admin", "   Administrator   ", "Guest" };
        string acceptedRole = "";

        foreach (string entry in roleEntries)
        {
            string sanitizedEntry = entry.Trim().ToLower();
            Console.WriteLine($"Evaluating role entry: '{entry}'");

            if (sanitizedEntry == "administrator" || sanitizedEntry == "manager" || sanitizedEntry == "user")
            {
                // Format canonical display name:
                acceptedRole = char.ToUpper(sanitizedEntry[0]) + sanitizedEntry.Substring(1);
                Console.WriteLine($"  Success: Role '{acceptedRole}' has been accepted.");
                break;
            }
            else
            {
                Console.WriteLine($"  Error: '{entry.Trim()}' is not a recognized role (Administrator, Manager, or User).");
            }
        }

        // DESC IN COMMENT:
        // Validates input against an explicit whitelist of allowed values after sanitizing casing and whitespace.

        // WHAT DOES IT DO:
        // Normalizes the raw text with .Trim().ToLower().
        // Evaluates against authorized choices ("administrator", "manager", "user").
        // Rejects unapproved values like "Admin" or "Guest" defensively.

        // OUTPUT:
        // Evaluating role entry: 'Admin'
        //   Error: 'Admin' is not a recognized role (Administrator, Manager, or User).
        // Evaluating role entry: '   Administrator   '
        //   Success: Role 'Administrator' has been accepted.

        // NOTE:
        // Whitelisting (explicit allow-list) is fundamentally more secure than blacklisting (deny-list).


        // =====================================================================
        // MODULE 9: STRING PARSING & DELIMITER EXTRACTION
        // =====================================================================

        // ---------------------------------------------------------------------
        // 28. DELIMITER PARSING WITH IndexOf, Substring, Remove, AND TrimStart
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("\n--- 28. String Delimiter Parsing and Extraction ---");
        string[] rawSentenceBlocks = {
            "I like pizza. I like roast chicken. I like salad",
            "I like all three of the menu choices"
        };

        foreach (string rawBlock in rawSentenceBlocks)
        {
            string workingText = rawBlock;
            int periodLocation = workingText.IndexOf(".");

            while (periodLocation != -1)
            {
                // Extract sentence up to the period:
                string sentence = workingText.Substring(0, periodLocation);
                Console.WriteLine(sentence);

                // Remove the extracted sentence and the period:
                workingText = workingText.Remove(0, periodLocation + 1).TrimStart();

                // Locate the next period:
                periodLocation = workingText.IndexOf(".");
            }

            // Print any remaining text that didn't end with a period:
            if (workingText.Length > 0)
            {
                Console.WriteLine(workingText);
            }
        }

        // DESC IN COMMENT:
        // Parses sentences delimited by periods using fundamental string indexing and slicing methods.

        // WHAT DOES IT DO:
        // 1. IndexOf(".") returns the 0-based index of the first period character, or -1 if not found.
        // 2. Substring(0, periodLocation) extracts all characters from index 0 up to (but excluding) the period.
        // 3. Remove(0, periodLocation + 1) drops the sentence and period from workingText.
        // 4. TrimStart() cleans up any leading spaces preceding the next sentence.
        // 5. The loop repeats until IndexOf returns -1, after which any remaining trailing text is output.

        // OUTPUT:
        // I like pizza
        // I like roast chicken
        // I like salad
        // I like all three of the menu choices

        // NOTE:
        // Understanding low-level pointer/index operations with IndexOf and Substring builds the mental foundation
        // for high-performance memory slicing with ReadOnlySpan<char> in modern C# backend systems.

        Console.WriteLine("\n================================================================================");
        Console.WriteLine("                     END OF MILESTONE 3 NOTES & CODE SAMPLES                    ");
        Console.WriteLine("================================================================================");
    }
}
