/*
================================================================================
                    C# NOTES - MILESTONE 1
================================================================================

Milestone 1: First code, types, math, decisions

LEARNING PATH:
Path: Write your first code using C# (Part 1)
- Module 1: Write your first C# code
- Module 2: Store data with variables in C#
- Module 3: Perform basic operations on numbers in C#
- Module 4: Add decision logic to your code in C#
- Extra: Perform basic string formatting in C# (escape sequences, concatenation, interpolation)

YOU CAN MOVE ON WHEN:
- You can explain why 7 / 2 is 3 and how to get 3.5.
- You can explain int, double, decimal and when each is right (money is decimal).
- You can write if / else if / else with &&, ||, ! without checking syntax.
================================================================================
*/

#pragma warning disable CS0219 // Variable is assigned but its value is never used

public static class Milestone1Notes
{
    public static void Run()
    {
        // ---------------------------------------------------------------------
        // 1. CONSOLE OUTPUT
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.WriteLine("Hello, World!");
        Console.Write("Loading... ");
        Console.WriteLine("Done!");

        // DESC IN COMMENT:
        // Prints messages to the console; WriteLine adds a newline, Write does not.

        // WHAT DOES IT DO:
        // Console.WriteLine outputs the given text and moves the cursor to the next line.
        // Console.Write outputs the text and keeps the cursor on the same line.

        // OUTPUT:
        // Hello, World!
        // Loading... Done!

        // NOTE:
        // Use escape sequences inside strings: '\n' creates a line break, '\t' creates a tab stop, and '\"' escapes quotes.


        // ---------------------------------------------------------------------
        // 2. READING USER INPUT
        // ---------------------------------------------------------------------
        // SYNTAX:
        Console.Write("Enter your name: ");
        string userInput = Console.ReadLine() ?? "";
        Console.WriteLine($"Welcome, {userInput}!");

        // DESC IN COMMENT:
        // Pauses execution and captures user text input from the terminal until Enter is pressed.

        // WHAT DOES IT DO:
        // Reads standard keyboard input as a string. The '?? ""' ensures that if input is null, it defaults to an empty string.

        // OUTPUT:
        // Enter your name: Alex
        // Welcome, Alex!

        // NOTE:
        // Console.ReadLine() ALWAYS returns a string. It never directly returns a number. You must parse it to do calculations.


        // ---------------------------------------------------------------------
        // 3. PRIMITIVE DATA TYPES
        // ---------------------------------------------------------------------
        // SYNTAX:
        int wholeNumber = 42;
        double measurement = 12.5;
        decimal monetaryAmount = 1500.50m;
        bool isCompleted = true;
        char gradeLetter = 'A';
        string greeting = "Good day";

        // DESC IN COMMENT:
        // Allocates strongly typed memory variables for specific data categories.

        // WHAT DOES IT DO:
        // Stores distinct categories of data in system memory with strict compiler type safety.

        // OUTPUT:
        // Stored in memory:
        // wholeNumber: 42 (int)
        // measurement: 12.5 (double)
        // monetaryAmount: 1500.50 (decimal)
        // isCompleted: true (bool)
        // gradeLetter: 'A' (char)
        // greeting: "Good day" (string)

        // NOTE:
        // - int: 32-bit whole numbers (-2 billion to +2 billion). Use for counts, dimensions, indexes.
        // - double: 64-bit floating point. Fast for scientific/physical measurements (speed, weight, temperature).
        // - decimal: 128-bit high-precision. MUST use the 'm' suffix. MUST be used for financial and monetary calculations.
        // - bool: true or false.
        // - char: single Unicode character in single quotes ('A').
        // - string: text characters in double quotes ("Hello").


        // ---------------------------------------------------------------------
        // 4. STRING PARSING (TEXT TO NUMBERS)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int parsedInt = int.Parse("100");
        decimal parsedDecimal = decimal.Parse("25.99");

        // DESC IN COMMENT:
        // Converts a numeric string into a concrete numeric data type.

        // WHAT DOES IT DO:
        // Scans through string characters and produces the equivalent numeric representation.

        // OUTPUT:
        // parsedInt: 100 (int)
        // parsedDecimal: 25.99 (decimal)

        // NOTE:
        // Type.Parse() strictly expects a string. Never pass an int into decimal.Parse().
        // If the string contains non-numeric letters (e.g. "abc"), Parse throws a runtime FormatException.


        // ---------------------------------------------------------------------
        // 5. EXPLICIT TYPE CASTING
        // ---------------------------------------------------------------------
        // SYNTAX:
        double rawMeasurement = 12.75;
        decimal safeDecimal = (decimal)rawMeasurement;
        int truncatedNumber = (int)rawMeasurement; // Result: 12

        // DESC IN COMMENT:
        // Explicitly forces conversion between compatible numeric types using (targetType).

        // WHAT DOES IT DO:
        // Instructs the C# compiler to convert a value where data precision or type ranges differ.

        // OUTPUT:
        // safeDecimal: 12.75 (decimal)
        // truncatedNumber: 12 (int - decimal portion .75 is dropped)

        // NOTE:
        // Casting a floating point number to an int cuts off (truncates) the decimal part; it does NOT round (12.75 becomes 12).
        // Casting does not work on strings: (int)"42" will cause a compilation error.


        // ---------------------------------------------------------------------
        // 6. ARITHMETIC OPERATORS & THE INTEGER DIVISION TRAP
        // ---------------------------------------------------------------------
        // SYNTAX:
        int integerDivision = 7 / 2;         // Result: 3
        double decimalDivision = 7 / 2.0;    // Result: 3.5
        decimal financialDivision = 7m / 2m; // Result: 3.5m

        // DESC IN COMMENT:
        // Performs standard arithmetic: +, -, *, /, % (modulus/remainder).

        // WHAT DOES IT DO:
        // Computes mathematical operations following standard algebraic order of operations.

        // OUTPUT:
        // integerDivision: 3 (7 / 2 discards remainder 1)
        // decimalDivision: 3.5 (7 / 2.0 performs floating-point division)
        // financialDivision: 3.5 (7m / 2m preserves exact decimal precision)

        // NOTE:
        // Why is 7 / 2 equal to 3? Because both 7 and 2 are integers! In integer division, C# drops the remainder.
        // To get 3.5, at least ONE operand must have a decimal point: '7 / 2.0' or '7.0 / 2' or '7m / 2m'.


        // ---------------------------------------------------------------------
        // 7. STRING INTERPOLATION & FORMAT SPECIFIERS
        // ---------------------------------------------------------------------
        // SYNTAX:
        decimal invoiceTotal = 22075.50m;
        Console.WriteLine($"Package Weight: {measurement:N2} kg");
        Console.WriteLine($"Total Due: PHP {invoiceTotal:N2}");

        // DESC IN COMMENT:
        // Combines text with variables using the '$' prefix and applies number formatting.

        // WHAT DOES IT DO:
        // Embeds expressions directly inside '{variable}' and formats the visual representation.

        // OUTPUT:
        // Package Weight: 12.50 kg
        // Total Due: PHP 22,075.50

        // NOTE:
        // - :N2 formats numbers with thousand commas and 2 decimal places (e.g., 22,075.50).
        // - :C formats numbers as local currency symbols (e.g., $22,075.50 or PHP 22,075.50).
        // - :P1 formats a fraction as a percentage with 1 decimal place (e.g., 0.15 becomes 15.0%).


        // ---------------------------------------------------------------------
        // 8. COMPARISON OPERATORS
        // ---------------------------------------------------------------------
        // SYNTAX:
        bool checkEqual    = (wholeNumber == 42); // true
        bool checkNotEqual = (wholeNumber != 50); // true
        bool checkGreater  = (wholeNumber > 100); // false
        bool checkLessOrEq = (wholeNumber <= 42); // true

        // DESC IN COMMENT:
        // Relational operators that evaluate relationships between two values and return a bool.

        // WHAT DOES IT DO:
        // Evaluates comparison conditions producing either true or false.

        // OUTPUT:
        // checkEqual: true (42 == 42)
        // checkNotEqual: true (42 != 50)
        // checkGreater: false (42 is not > 100)
        // checkLessOrEq: true (42 <= 42)

        // NOTE:
        // '=' is for assignment (storing a value in a variable).
        // '==' is for comparison (asking if two values are equal).


        // ---------------------------------------------------------------------
        // 9. LOGICAL OPERATORS (&&, ||, !)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int zone = 2;
        double weightKg = 15.0;

        // AND (&&): Both sides must evaluate to true
        bool isAllowedWeight = (weightKg > 0) && (weightKg <= 70);

        // OR (||): At least one side must evaluate to true
        bool isInvalidZone = (zone < 1) || (zone > 3);

        // NOT (!): Inverts the boolean value
        bool isInvoiceMissing = !isCompleted;

        // DESC IN COMMENT:
        // Connects multiple boolean expressions into compound logical conditions.

        // WHAT DOES IT DO:
        // - && (AND): Evaluates the right side only if the left side is true (short-circuiting).
        // - || (OR): Evaluates the right side only if the left side is false (short-circuiting).
        // - ! (NOT): Flips true to false, or false to true.

        // OUTPUT:
        // isAllowedWeight: true (15.0 > 0 && 15.0 <= 70)
        // isInvalidZone: false (zone 2 is neither < 1 nor > 3)
        // isInvoiceMissing: false (!true evaluates to false)

        // NOTE:
        // Common logic trap: '!(zone >= 1) && !(zone >= 3)' does not check invalid zones; it only checks zone < 1.
        // To check an out-of-bounds range, always write: 'zone < 1 || zone > 3'.


        // ---------------------------------------------------------------------
        // 10. CONDITIONAL BRANCHING (if, else if, else)
        // ---------------------------------------------------------------------
        // SYNTAX:
        bool isRejected = false;
        bool isManual = true;
        string decisionStatus;

        if (isRejected)
        {
            decisionStatus = "SHIPMENT REJECTED";
        }
        else if (isManual)
        {
            decisionStatus = "HELD FOR MANUAL REVIEW";
        }
        else
        {
            decisionStatus = "CLEARED FOR DISPATCH";
        }

        // DESC IN COMMENT:
        // Directs code execution along a single branch based on evaluated boolean conditions.

        // WHAT DOES IT DO:
        // Tests conditions in top-down sequence. The first true condition executes its block and skips all remaining branches.

        // OUTPUT:
        // decisionStatus: "HELD FOR MANUAL REVIEW"

        // NOTE:
        // - Nested if statements act as AND: The inner block is only reachable if the outer block was true.
        // - Independent if statements (without 'else'): Every if condition is evaluated independently.
        // - Rejection rules should be independent or prioritized first so manual reviews never mask rejections.


        // ---------------------------------------------------------------------
        // 11. SWITCH EXPRESSION (CLEAN VALUE MAPPING)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string zoneDescription = zone switch
        {
          1 => "Domestic",
          2 => "Regional Asia-Pacific",
          3 => "International Long-Haul",
          _ => "Unknown Zone"
        };

        // DESC IN COMMENT:
        // Evaluates a single value against multiple patterns and returns a matched result.

        // WHAT DOES IT DO:
        // Replaces lengthy if-else chains when matching a single variable against constants.

        // OUTPUT:
        // zoneDescription: "Regional Asia-Pacific" (matched arm 2)

        // NOTE:
        // The discard symbol '_' acts as the default fallback case when no other pattern matches.
    }
}
