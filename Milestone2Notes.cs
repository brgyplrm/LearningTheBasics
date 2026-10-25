/*
================================================================================
                    C# NOTES - MILESTONE 2
================================================================================

Milestone 2: Console apps, loops, arrays, files, methods

LEARNING PATHS:
- Get started with C#, Part 2: Create and run simple C# console applications
- Get started with C#, Part 5: Create methods in C# console applications

MODULES:
- Module 1: Repeat actions with loops in C# (for, foreach, while, do-while)
- Module 2: Organize data with arrays and lists in C# (string[], int[], List<T>)
- Module 3: Write reusable code in C# (methods, parameters, return types)
- Module 4: Manage variable scope in C# (local inside loops/methods vs block level)
- Module 5: Organize code with namespaces in C#
- Module 6: Read and write files safely in C# (File, StreamReader, using)

YOU CAN MOVE ON WHEN:
[ ] You can create and populate 1D arrays and iterate over them using foreach and for.
[ ] You know when to use while vs do-while vs for loops.
[ ] You can control loop execution using break and continue.
[ ] You can write modular methods with parameters and return values (void vs typed).
[ ] You understand variable scope (local inside loops/methods vs class level).
[ ] You understand how namespaces group code and how using directives work.
[ ] You can safely read and write files using File methods and the using statement.
================================================================================
*/

#pragma warning disable CS0219 // Variable is assigned but its value is never used
#pragma warning disable CS8321 // The function is declared but never used

using Calc = MathUtilities.Calculator;

public static class Milestone2Notes
{
    public static void Run()
    {
        // =====================================================================
        // MODULE 1: LOOPS (FOR, WHILE, DO-WHILE, BREAK, CONTINUE)
        // =====================================================================

        // ---------------------------------------------------------------------
        // 1. FOR LOOP (KNOWN ITERATION COUNT)
        // ---------------------------------------------------------------------
        // SYNTAX:
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine(i);
        }

        // DESC IN COMMENT:
        // Use a for loop when you want to repeat an action a known number of times.

        // WHAT DOES IT DO:
        // A for loop has three parts: an initializer (int i = 0), a condition (i < 5), and an iterator step (i++).

        // OUTPUT:
        // 0
        // 1
        // 2
        // 3
        // 4

        // NOTE:
        // The loop starts with i set to 0. Before each pass, C# checks whether i < 5. If true, the body runs and i increments.
        // Once i reaches 5, the condition evaluates to false and the loop terminates.


        // ---------------------------------------------------------------------
        // 2. WHILE LOOP (CONDITION-BASED REPETITION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int health = 3;

        while (health > 0)
        {
            Console.WriteLine($"Player is alive. Health: {health}");
            health--;
        }

        // DESC IN COMMENT:
        // Repeats code as long as a boolean condition remains true; checks condition before each pass.

        // WHAT DOES IT DO:
        // Evaluates health > 0 before entering. Decrements health on each iteration until it reaches 0.

        // OUTPUT:
        // Player is alive. Health: 3
        // Player is alive. Health: 2
        // Player is alive. Health: 1

        // NOTE:
        // Always make sure the loop body updates the condition variable; otherwise it creates an infinite loop that freezes execution.


        // ---------------------------------------------------------------------
        // 3. DO-WHILE LOOP (RUNS AT LEAST ONCE)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int attempts = 0;

        do
        {
            attempts++;
            Console.WriteLine($"Attempt number {attempts}");
        }
        while (attempts < 3);

        // DESC IN COMMENT:
        // Executes the body first, then evaluates the condition to decide whether to repeat.

        // WHAT DOES IT DO:
        // Guarantees that the code block runs at least once, even if the condition starts out false.

        // OUTPUT:
        // Attempt number 1
        // Attempt number 2
        // Attempt number 3

        // NOTE:
        // Ideal for user menus and retry prompts where you must present options before validating the user's choice.


        // ---------------------------------------------------------------------
        // 4. LOOP SELECTION CRITERIA
        // ---------------------------------------------------------------------
        // SYNTAX:
        // - For: repetition count is known in advance.
        // - While: repetition depends on external state or input; may run 0 times.
        // - Do-While: body must run at least once before checking continuation.

        // DESC IN COMMENT:
        // Guidelines for selecting the optimal loop construct for any given algorithm.

        // WHAT DOES IT DO:
        // Ensures appropriate control flow structure is chosen to prevent off-by-one errors and redundant code.

        // OUTPUT:
        // (Architectural decision pattern - no console print)

        // NOTE:
        // Reserve short counter names like 'i' or 'j' for for-loops. Use descriptive names like 'attempts' or 'health' for stateful loops.


        // ---------------------------------------------------------------------
        // 5. BREAK STATEMENT (EARLY LOOP TERMINATION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        for (int count = 0; count < 10; count++)
        {
            if (count == 5)
            {
                break;
            }
            Console.WriteLine(count);
        }

        Console.WriteLine("Loop ended.");

        // DESC IN COMMENT:
        // Immediately terminates the enclosing loop, jumping to the first statement following the loop block.

        // WHAT DOES IT DO:
        // Evaluates count on each step; once count reaches 5, break terminates the loop before 5 is printed.

        // OUTPUT:
        // 0
        // 1
        // 2
        // 3
        // 4
        // Loop ended.

        // NOTE:
        // Order matters: place the break check before the operations you want to skip.


        // ---------------------------------------------------------------------
        // 6. CONTINUE STATEMENT (SKIPPING AN ITERATION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        for (int count = 0; count < 6; count++)
        {
            if (count == 3)
            {
                continue;
            }
            Console.WriteLine(count);
        }

        // DESC IN COMMENT:
        // Skips the remaining statements in the current iteration and jumps immediately to the next cycle.

        // WHAT DOES IT DO:
        // When count is 3, continue skips Console.WriteLine(count) and immediately proceeds to count = 4.

        // OUTPUT:
        // 0
        // 1
        // 2
        // 4
        // 5

        // NOTE:
        // Use continue when you want to ignore specific cases without terminating the whole loop.


        // ---------------------------------------------------------------------
        // 7. WHILE LOOP WITH BREAK (SIMULATING USER INPUT TERMINATION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string[] inputSequence = { "view", "edit", "exit" };
        int sequenceIndex = 0;

        while (true)
        {
            string command = inputSequence[sequenceIndex++];

            if (command.ToLower() == "exit")
            {
                break;
            }

            Console.WriteLine($"Processing command: {command}");
        }

        Console.WriteLine("Goodbye!");

        // DESC IN COMMENT:
        // while (true) creates an open loop that continues until an explicit break condition is met.

        // WHAT DOES IT DO:
        // Processes sequential commands until encountering "exit", where break safely exits the loop.

        // OUTPUT:
        // Processing command: view
        // Processing command: edit
        // Goodbye!

        // NOTE:
        // In interactive console applications, replace simulated array elements with Console.ReadLine().


        // ---------------------------------------------------------------------
        // 8. WHILE LOOP WITH CONTINUE (SAFE CONDITION UPDATING)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int currentSlot = 0;

        while (currentSlot < 5)
        {
            currentSlot++; // Crucial: the increment happens BEFORE the continue statement

            if (currentSlot == 3)
            {
                Console.WriteLine($"Skipping item slot {currentSlot}.");
                continue;
            }

            Console.WriteLine($"Successfully processed item in slot {currentSlot}.");
        }

        Console.WriteLine("Inventory scan complete!");

        // DESC IN COMMENT:
        // Demonstrates safe usage of continue in a while loop by updating the counter before skipping.

        // WHAT DOES IT DO:
        // Increments currentSlot first, prints a skip notice for slot 3, and processes slots 1, 2, 4, and 5.

        // OUTPUT:
        // Successfully processed item in slot 1.
        // Successfully processed item in slot 2.
        // Skipping item slot 3.
        // Successfully processed item in slot 4.
        // Successfully processed item in slot 5.
        // Inventory scan complete!

        // NOTE:
        // If currentSlot++ was placed after continue, currentSlot would stay at 3 forever, causing an infinite loop.


        // =====================================================================
        // MODULE 2: ARRAYS & LISTS
        // =====================================================================

        // ---------------------------------------------------------------------
        // 9. CREATING AN ARRAY (FIXED-SIZE SEQUENCES)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string[] primaryColors = { "red", "green", "blue" };
        int[] defaultScores = new int[4];

        // DESC IN COMMENT:
        // Allocates a fixed-size, contiguous block of memory to store values of a single data type.

        // WHAT DOES IT DO:
        // primaryColors initializes with 3 string literals; defaultScores allocates 4 slots initialized to default int value (0).

        // OUTPUT:
        // primaryColors -> ["red", "green", "blue"] (Length: 3)
        // defaultScores -> [0, 0, 0, 0] (Length: 4)

        // NOTE:
        // Array sizes are fixed at creation time and cannot expand or shrink.


        // ---------------------------------------------------------------------
        // 10. ACCESSING ARRAY ITEMS BY INDEX (ZERO-BASED LOOKUP)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string[] colorPalette = { "red", "green", "blue" };
        Console.WriteLine(colorPalette[0]);
        Console.WriteLine(colorPalette[2]);

        // DESC IN COMMENT:
        // Retrieves an element using its zero-based position inside square brackets.

        // WHAT DOES IT DO:
        // Reads the first element at index 0 ("red") and the third element at index 2 ("blue").

        // OUTPUT:
        // red
        // blue

        // NOTE:
        // Accessing an index outside 0 to Length - 1 throws an IndexOutOfRangeException.


        // ---------------------------------------------------------------------
        // 11. ARRAY FIXED SIZE & LENGTH PROPERTY
        // ---------------------------------------------------------------------
        // SYNTAX:
        string[] mutableColors = { "red", "green", "blue" };
        mutableColors[0] = "purple"; // Updates existing element
        Console.WriteLine(mutableColors[0]);
        Console.WriteLine(mutableColors.Length);

        // DESC IN COMMENT:
        // You can overwrite values at existing indexes, but you cannot add or remove items.

        // WHAT DOES IT DO:
        // Replaces "red" with "purple" at index 0 and reads the total count through the .Length property.

        // OUTPUT:
        // purple
        // 3

        // NOTE:
        // If your dataset needs to grow or shrink dynamically at runtime, use List<T> instead of an array.


        // ---------------------------------------------------------------------
        // 12. ITERATING OVER AN ARRAY (FOREACH)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int[] examScores = { 95, 82, 76, 91 };

        foreach (int score in examScores)
        {
            Console.WriteLine(score);
        }

        // DESC IN COMMENT:
        // Steps through every element in an array sequentially without managing manual index counters.

        // WHAT DOES IT DO:
        // Pulls each integer from examScores from beginning to end and writes it to the console.

        // OUTPUT:
        // 95
        // 82
        // 76
        // 91

        // NOTE:
        // The iteration variable ('score') is read-only inside the foreach block; you cannot reassign it.


        // ---------------------------------------------------------------------
        // 13. CREATING A GENERIC LIST (RESIZABLE COLLECTION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        List<string> initialColors = new List<string> { "red", "green", "blue" };
        List<int> emptyScores = new List<int>();

        // DESC IN COMMENT:
        // List<T> is a resizable, ordered generic collection located in System.Collections.Generic.

        // WHAT DOES IT DO:
        // Allocates dynamic lists that can expand and shrink in memory as items are added or removed.

        // OUTPUT:
        // initialColors -> Count: 3 ["red", "green", "blue"]
        // emptyScores -> Count: 0 []

        // NOTE:
        // 'T' is the type argument. List<string> holds strings, List<int> holds ints, and List<decimal> holds decimals.


        // ---------------------------------------------------------------------
        // 14. COMMON LIST OPERATIONS (ADD, REMOVE, COUNT, INDEXER)
        // ---------------------------------------------------------------------
        // SYNTAX:
        List<string> activeColors = new List<string> { "red", "green", "blue" };

        Console.WriteLine(activeColors.Count);
        activeColors.Add("yellow");            // Appends to the end
        Console.WriteLine(activeColors.Count);

        activeColors.Remove("red");            // Removes first matching element
        activeColors[0] = "purple";            // Replaces item at index 0

        // DESC IN COMMENT:
        // Demonstrates dynamic list operations: counting (.Count), appending (.Add), removing (.Remove), and indexing.

        // WHAT DOES IT DO:
        // Modifies size and content: adds "yellow", removes "red" (shifting "green" to index 0), then replaces "green" with "purple".

        // OUTPUT:
        // 3
        // 4
        // (Final list contents: ["purple", "blue", "yellow"])

        // NOTE:
        // Removing an item shifts all subsequent elements down by 1 index.


        // ---------------------------------------------------------------------
        // 15. ITERATING OVER A LIST (FOREACH)
        // ---------------------------------------------------------------------
        // SYNTAX:
        List<int> studentScores = new List<int> { 95, 82, 76, 91 };

        foreach (int score in studentScores)
        {
            Console.WriteLine(score);
        }

        // DESC IN COMMENT:
        // Traverses dynamic list elements cleanly from first to last.

        // WHAT DOES IT DO:
        // Iterates through studentScores sequentially without requiring index lookups.

        // OUTPUT:
        // 95
        // 82
        // 76
        // 91

        // NOTE:
        // Modifying a list (adding or removing items) while inside a foreach loop throws an InvalidOperationException.


        // ---------------------------------------------------------------------
        // 16. CHOOSING BETWEEN ARRAYS AND LISTS
        // ---------------------------------------------------------------------
        // SYNTAX:
        // Array: fixed item count, maximum performance, lowest memory overhead.
        // List<T>: flexible item count, built-in dynamic manipulation methods.

        // DESC IN COMMENT:
        // Practical rules for picking between arrays (fixed) and List<T> (dynamic).

        // WHAT DOES IT DO:
        // Directs memory architecture: choose arrays for fixed datasets, and List<T> when size changes at runtime.

        // OUTPUT:
        // (Architectural rule - no console print)

        // NOTE:
        // If you need .Add(), .Remove(), or .Sort(), always prefer List<T>.


        // =====================================================================
        // MODULE 3: METHODS (PARAMETERS, RETURN VALUES, TUPLES)
        // =====================================================================

        // ---------------------------------------------------------------------
        // 17. DEFINING & CALLING A METHOD (REUSABLE CODE)
        // ---------------------------------------------------------------------
        // SYNTAX:
        void Greet()
        {
            Console.WriteLine("Hello, world!");
        }

        Greet();

        // DESC IN COMMENT:
        // A method encapsulates a reusable block of logic under a descriptive name.

        // WHAT DOES IT DO:
        // Declares a void local method named Greet, then invokes it using Greet().

        // OUTPUT:
        // Hello, world!

        // NOTE:
        // Methods must be invoked with parentheses '()' even when they accept no arguments.


        // ---------------------------------------------------------------------
        // 18. METHOD PARAMETERS (PASSING DATA TO METHODS)
        // ---------------------------------------------------------------------
        // SYNTAX:
        void GreetPerson(string name)
        {
            Console.WriteLine($"Hello, {name}!");
        }

        GreetPerson("Alex");
        GreetPerson("Sam");

        void GreetWithGreeting(string name, string greeting)
        {
            Console.WriteLine($"{greeting}, {name}!");
        }

        GreetWithGreeting("Alex", "Hi");

        // DESC IN COMMENT:
        // Methods accept input parameter variables to dynamically customize their output.

        // WHAT DOES IT DO:
        // Passes caller arguments into parameter variables and embeds them in formatted console prints.

        // OUTPUT:
        // Hello, Alex!
        // Hello, Sam!
        // Hi, Alex!

        // NOTE:
        // Parameter types must match or be implicitly convertible from caller argument types.


        // ---------------------------------------------------------------------
        // 19. OPTIONAL PARAMETERS (DEFAULT FALLBACK VALUES)
        // ---------------------------------------------------------------------
        // SYNTAX:
        void GreetWithDefault(string name, string greeting = "Hello")
        {
            Console.WriteLine($"{greeting}, {name}!");
        }

        GreetWithDefault("Alex");
        GreetWithDefault("Alex", "Welcome");

        // DESC IN COMMENT:
        // Parameters can have default values assigned with '='. Callers can omit arguments that have defaults.

        // WHAT DOES IT DO:
        // Uses "Hello" when the caller omits the greeting parameter, or overrides it when provided.

        // OUTPUT:
        // Hello, Alex!
        // Welcome, Alex!

        // NOTE:
        // Optional parameters must always be positioned AFTER all required parameters in the parameter list.


        // ---------------------------------------------------------------------
        // 20. NAMED ARGUMENTS (EXPLICIT PARAMETER PASSING)
        // ---------------------------------------------------------------------
        // SYNTAX:
        GreetWithGreeting(greeting: "Welcome", name: "Alex");

        // DESC IN COMMENT:
        // Arguments can be passed by specifying parameter names explicitly with a colon (name: value).

        // WHAT DOES IT DO:
        // Decouples argument order from parameter order, allowing arguments to be supplied in any sequence.

        // OUTPUT:
        // Welcome, Alex!

        // NOTE:
        // If combining positional and named arguments, all positional arguments must precede named arguments.


        // ---------------------------------------------------------------------
        // 21. VALUE-RETURNING METHODS (THE RETURN KEYWORD)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int Add(int a, int b)
        {
            return a + b;
        }

        int addResult = Add(3, 4);
        Console.WriteLine(addResult);

        // DESC IN COMMENT:
        // Methods return data back to the caller by declaring a return type instead of void.

        // WHAT DOES IT DO:
        // Calculates a + b and yields the integer result back to the caller via the 'return' keyword.

        // OUTPUT:
        // 7

        // NOTE:
        // Every code path in a value-returning method must hit a return statement with a matching data type.


        // ---------------------------------------------------------------------
        // 22. RETURNING MULTIPLE VALUES WITH TUPLES
        // ---------------------------------------------------------------------
        // SYNTAX:
        (double x, double y) GetCoordinates()
        {
            return (10.0, 20.0);
        }

        var (xCoord, yCoord) = GetCoordinates();
        Console.WriteLine($"x: {xCoord}, y: {yCoord}");

        // DESC IN COMMENT:
        // C# tuples allow a method to bundle and return multiple lightweight values without creating a custom class.

        // WHAT DOES IT DO:
        // Returns a pair of doubles and deconstructs them directly into variables xCoord and yCoord.

        // OUTPUT:
        // x: 10, y: 20

        // NOTE:
        // Tuple elements can be named in the return signature (double x, double y) for readable dot-access.


        // =====================================================================
        // MODULE 4: VARIABLE SCOPE
        // =====================================================================

        // ---------------------------------------------------------------------
        // 23. LOCAL METHOD SCOPE (ENCAPSULATION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        int CalculateTotal(int price, int tax)
        {
            int total = price + tax; // 'total' is local strictly to this method
            return total;
        }

        int totalComputed = CalculateTotal(10, 2);
        // Console.WriteLine(total); // Compile error: 'total' does not exist in the caller's context

        // DESC IN COMMENT:
        // Variables declared inside a method exist only while that method executes.

        // WHAT DOES IT DO:
        // Confines 'total' to CalculateTotal. When the method returns, its local variables are destroyed.

        // OUTPUT:
        // totalComputed -> 12
        // Trying to access 'total' outside CalculateTotal causes compile error CS0103.

        // NOTE:
        // Local variables prevent unintended side-effects and keep methods independent.


        // ---------------------------------------------------------------------
        // 24. BLOCK SCOPE (CURLY BRACE ENCAPSULATION)
        // ---------------------------------------------------------------------
        // SYNTAX:
        if (true)
        {
            int blockMessage = 42;
            Console.WriteLine(blockMessage);
        }

        // Console.WriteLine(blockMessage); // Compile error: 'blockMessage' does not exist outside the if block

        // DESC IN COMMENT:
        // Variables declared inside any pair of curly braces '{ }' exist only within that block.

        // WHAT DOES IT DO:
        // Restricts visibility of 'blockMessage' strictly inside the if statement block.

        // OUTPUT:
        // 42
        // Trying to access 'blockMessage' outside the if block causes compile error CS0103.

        // NOTE:
        // Inner blocks CAN access variables declared in outer blocks, but outer blocks cannot access inner variables.


        // =====================================================================
        // MODULE 5: NAMESPACES
        // =====================================================================

        // ---------------------------------------------------------------------
        // 25. WHAT IS A NAMESPACE & DECLARING NAMESPACES
        // ---------------------------------------------------------------------
        // SYNTAX:
        /*
        namespace MathUtilities
        {
            public static class Calculator
            {
                public static int Add(int a, int b)
                {
                    return a + b;
                }
            }
        }
        */
        // Calling via fully qualified name:
        int directSum = MathUtilities.Calculator.Add(10, 5);
        Console.WriteLine(directSum);

        // DESC IN COMMENT:
        // A namespace groups related classes, structs, and interfaces under a shared domain name.

        // WHAT DOES IT DO:
        // Organizes code logically and prevents naming collisions between your classes and third-party libraries.

        // OUTPUT:
        // 15

        // NOTE:
        // Namespaces can span multiple files within the same project. All files sharing a namespace belong to the same logical container.


        // ---------------------------------------------------------------------
        // 26. BRINGING NAMESPACES INTO SCOPE WITH THE 'USING' DIRECTIVE
        // ---------------------------------------------------------------------
        // SYNTAX:
        // From another file in the same project, bring that namespace into scope:
        // using MathUtilities;
        // Then call its method directly through the class name:
        int addedValues = MathUtilities.Calculator.Add(2, 3);
        Console.WriteLine(addedValues);

        // DESC IN COMMENT:
        // The 'using' directive imports a namespace into the current file so you don't have to type the full path every time.

        // WHAT DOES IT DO:
        // Instructs the C# compiler to search the specified namespace when resolving class and method names.

        // OUTPUT:
        // 5

        // NOTE:
        // Without 'using', you must type the fully qualified name (e.g. System.Console.WriteLine instead of Console.WriteLine).


        // ---------------------------------------------------------------------
        // 27. USING NAMESPACE AND TYPE ALIASES (USING ALIAS = ...)
        // ---------------------------------------------------------------------
        // SYNTAX:
        // If a namespace or type name is long, or clashes with another name, give it a shorter alias:
        // using Calc = MathUtilities.Calculator;
        // Then invoke through the short alias:
        int aliasedResult = Calc.Add(2, 3);
        Console.WriteLine(aliasedResult);

        // DESC IN COMMENT:
        // Creates a short, custom alias for a lengthy namespace or type name, or resolves ambiguity between identical names.

        // WHAT DOES IT DO:
        // Replaces a long or conflicting identifier (MathUtilities.Calculator) with a concise local alias ('Calc').

        // OUTPUT:
        // 5

        // NOTE:
        // Extremely useful when two imported namespaces share a class with the exact same name (e.g., System.Windows.Shapes.Path vs System.IO.Path).


        // ---------------------------------------------------------------------
        // 28. WHY NAMESPACES MATTER (3 CORE PRINCIPLES)
        // ---------------------------------------------------------------------
        // SYNTAX:
        // 1. They keep related code grouped under one name so large projects stay organized.
        // 2. They prevent naming collisions between your code and code from other libraries.
        // 3. They make it clear where a type or method comes from, just by reading a file's using directives.

        // DESC IN COMMENT:
        // Three foundational architectural reasons why C# applications are structured into namespaces.

        // WHAT DOES IT DO:
        // Establishes modular software boundaries, scalable file organization, and clean dependency management.

        // OUTPUT:
        // (Architectural principle - no console print)

        // NOTE:
        // Always group your code into domain-specific namespaces (e.g. CargoApp.Models, CargoApp.Services, CargoApp.Data).


        // =====================================================================
        // MODULE 6: READING AND WRITING FILES SAFELY
        // =====================================================================

        // ---------------------------------------------------------------------
        // 29. THE PROBLEM WITH FORGETTING TO CLOSE A FILE (.CLOSE() ANTI-PATTERN)
        // ---------------------------------------------------------------------
        // SYNTAX:
        // RISKY MANUAL PATTERN (Avoid in production):
        /*
        StreamReader unsafeReader = new StreamReader("notes.txt");
        string content = unsafeReader.ReadToEnd();
        Console.WriteLine(content);
        unsafeReader.Close(); // If an exception occurs before this line, the file stays permanently locked!
        */

        // DESC IN COMMENT:
        // Opening a file gives you an operating system stream resource that MUST be closed when finished.

        // WHAT DOES IT DO:
        // Explains why manual .Close() calls lead to resource leaks, locked file handles, and corrupted writes whenever errors occur.

        // OUTPUT:
        // (Anti-pattern explanation - manual .Close() can leak file handles if errors happen before closing)

        // NOTE:
        // The problem is that if an error happens after opening the file but before closing it, the file stays open, which can lead to lost data, locked files, or resource leaks.


        // ---------------------------------------------------------------------
        // 30. SAFE FILE HANDLING WITH THE 'USING' STATEMENT (USING (...) { })
        // ---------------------------------------------------------------------
        // SYNTAX:
        string sampleFilePath = "notes.txt";
        File.WriteAllText(sampleFilePath, "Line 1: Safe file handling.\nLine 2: Automatically closed.");

        using (StreamReader safeReader = new StreamReader(sampleFilePath))
        {
            string contents = safeReader.ReadToEnd();
            Console.WriteLine(contents);
        } // File is automatically closed here, even if an error occurs

        // DESC IN COMMENT:
        // The safer way to work with files in C# is the using statement. It automatically closes the file when the block finishes, even if an error occurs.

        // WHAT DOES IT DO:
        // Guarantees deterministic disposal: safeReader.Dispose() is called automatically as soon as execution exits the curly brace block.

        // OUTPUT:
        // Line 1: Safe file handling.
        // Line 2: Automatically closed.

        // NOTE:
        // This pattern translates under the hood into a try-finally block where Dispose() is called in the finally clause.


        // ---------------------------------------------------------------------
        // 31. MODERN C# 'USING' DECLARATION (BRACE-FREE DISPOSAL)
        // ---------------------------------------------------------------------
        // SYNTAX:
        void ModernUsingDemo()
        {
            using StreamReader modernReader = new StreamReader(sampleFilePath);
            string contents = modernReader.ReadToEnd();
            Console.WriteLine(contents);
            // File is automatically closed at the end of this block
        }
        ModernUsingDemo();

        // DESC IN COMMENT:
        // Modern C# supports a shorter using declaration that closes the file at the end of the enclosing scope, without needing its own braces.

        // WHAT DOES IT DO:
        // Binds the lifetime of modernReader to its enclosing scope, cleanly closing the stream when the block exits.

        // OUTPUT:
        // Line 1: Safe file handling.
        // Line 2: Automatically closed.

        // NOTE:
        // Reduces nesting indentation and makes code cleaner without sacrificing any disposal safety.


        // ---------------------------------------------------------------------
        // 32. THE STATIC 'FILE' CLASS: AN EVEN SIMPLER OPTION
        // ---------------------------------------------------------------------
        // SYNTAX:
        // For many common tasks, the File class provides static methods that open, read or write, and close a file in a single call:
        File.WriteAllText(sampleFilePath, "Line 1: Hello from notes!\nLine 2: Learning C# file I/O.");
        string fileClassContents = File.ReadAllText(sampleFilePath);
        Console.WriteLine(fileClassContents);

        // DESC IN COMMENT:
        // Static File methods open, perform the read or write, and close the file in a single line without needing an explicit using statement.

        // WHAT DOES IT DO:
        // Behind the scenes, File.ReadAllText() still opens and closes the file safely.

        // OUTPUT:
        // Line 1: Hello from notes!
        // Line 2: Learning C# file I/O.

        // NOTE:
        // Reach for these File methods for straightforward reads and writes, and use StreamReader or StreamWriter with using when you need more control, like reading line by line.
        // Whichever approach you choose, avoid calling .Close() yourself. Use using, or a File method that handles it for you.


        // ---------------------------------------------------------------------
        // 33. READ THE ENTIRE FILE AT ONCE (FILE.READALLTEXT)
        // ---------------------------------------------------------------------
        // SYNTAX:
        if (File.Exists(sampleFilePath))
        {
            string entireFile = File.ReadAllText(sampleFilePath);
            Console.WriteLine(entireFile);
        }

        // DESC IN COMMENT:
        // File.ReadAllText() returns the entire file as a single string.

        // WHAT DOES IT DO:
        // Loads the entire text file into memory in one operation.

        // OUTPUT:
        // Line 1: Hello from notes!
        // Line 2: Learning C# file I/O.

        // NOTE:
        // This approach works well for small files, but for very large files it loads everything into memory at once.
        // If the file doesn't exist, both File.ReadAllText() and StreamReader throw a FileNotFoundException. Be sure the file exists before you try to read it.


        // ---------------------------------------------------------------------
        // 34. READ THE FILE AS AN ARRAY OF LINES (FILE.READALLLINES)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string[] linesArray = File.ReadAllLines(sampleFilePath);

        foreach (string line in linesArray)
        {
            Console.WriteLine(line);
        }

        // DESC IN COMMENT:
        // File.ReadAllLines() returns each line as a separate string in an array.

        // WHAT DOES IT DO:
        // Splits file contents by newline characters, giving you an array of lines you can index into or loop over.

        // OUTPUT:
        // Line 1: Hello from notes!
        // Line 2: Learning C# file I/O.

        // NOTE:
        // Convenient when you need line count (linesArray.Length) or direct index access (linesArray[0]), but loads all lines into RAM at once.


        // ---------------------------------------------------------------------
        // 35. READ ONE LINE AT A TIME (STREAMREADER WITH READLINE)
        // ---------------------------------------------------------------------
        // SYNTAX:
        using (StreamReader reader = new StreamReader(sampleFilePath))
        {
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        // DESC IN COMMENT:
        // The most memory-friendly way is to use a StreamReader and call .ReadLine() in a loop. It returns null once there are no more lines to read.

        // WHAT DOES IT DO:
        // Reads lines one by one. This pattern is recommended for large files because only one line is held in memory at a time.

        // OUTPUT:
        // Line 1: Hello from notes!
        // Line 2: Learning C# file I/O.

        // NOTE:
        // ReadLine() returns null at end-of-file, which cleanly terminates the while loop condition '(line = reader.ReadLine()) != null'.


        // ---------------------------------------------------------------------
        // 36. CHOOSING THE RIGHT APPROACH (SUMMARY GUIDE)
        // ---------------------------------------------------------------------
        // SYNTAX:
        /*
        Choosing the right approach:
        ----------------------------------------------------------------------------
        If you want to...                            Use...
        ----------------------------------------------------------------------------
        Work with the entire file as one string      File.ReadAllText()
        Get an array of lines you can index into     File.ReadAllLines()
        Process each line one at a time efficiently  StreamReader with ReadLine()
        ----------------------------------------------------------------------------
        */

        // DESC IN COMMENT:
        // Summary decision matrix for selecting the optimal file read strategy.

        // WHAT DOES IT DO:
        // Guides the developer in choosing between full buffering, line arrays, or low-memory streaming based on file size and processing needs.

        // OUTPUT:
        // (Decision matrix - no console print)

        // NOTE:
        // - Small files (< a few MB): File.ReadAllText or File.ReadAllLines for simplicity.
        // - Large files (tens or hundreds of MB): StreamReader with ReadLine() to prevent High Memory usage / OutOfMemoryException.


        // =====================================================================
        // MODULE 7: ADVANCED DISCOVERIES & DEFENSIVE PROGRAMMING PATTERNS
        // =====================================================================

        // ---------------------------------------------------------------------
        // 37. TRYPARSE WITH OUT PARAMETERS (SAFE CONVERSION WITHOUT CRASHING)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string rawInput = "42.50";
        if (decimal.TryParse(rawInput, out decimal safeNumber))
        {
            Console.WriteLine($"Successfully parsed: {safeNumber}");
        }
        else
        {
            Console.WriteLine("Invalid number format. Program continues running safely.");
        }

        // DESC IN COMMENT:
        // TryParse converts text to a number safely. Unlike Parse (which throws FormatException on invalid text), TryParse returns a bool and outputs the value via an 'out' variable without crashing.

        // WHAT DOES IT DO:
        // Returns true if the string can be parsed into the target type, false otherwise. When successful, the converted value is assigned to the 'out' variable.

        // OUTPUT:
        // Successfully parsed: 42.50

        // NOTE:
        // The 'out' variable is declared right inside the TryParse call: 'out decimal safeNumber'. Never assign TryParse: 'decimal x = decimal.TryParse(...)' is a compile error because TryParse returns a bool.


        // ---------------------------------------------------------------------
        // 38. ARRAY LENGTH VS STRING LENGTH (THE EGG CARTON ANALOGY)
        // ---------------------------------------------------------------------
        // SYNTAX:
        string csvRow = "PKG-1001,8.0,30,20,20";
        string[] columns = csvRow.Split(',');

        // 1. Array Length (no brackets) = Count of slots/columns:
        int columnCount = columns.Length; // 5 columns

        // 2. String Length (with brackets) = Letters in that specific slot:
        int idLetterCount = columns[0].Length; // 8 letters in "PKG-1001"

        if (columns.Length < 10)
        {
            Console.WriteLine("Warning: Row has fewer than 10 columns. Skipping corrupted record.");
        }

        // DESC IN COMMENT:
        // Distinguishes between collection capacity and element character length.

        // WHAT DOES IT DO:
        // 'columns.Length' checks how many items the array contains.
        // 'columns[0].Length' checks how many characters are in the string at index 0.

        // OUTPUT:
        // Warning: Row has fewer than 10 columns. Skipping corrupted record.

        // NOTE:
        // Defensive indexing: Always guard with 'if (columns.Length < expectedCount) continue;' before accessing high indexes to completely eliminate IndexOutOfRangeException.


        // ---------------------------------------------------------------------
        // 39. MULTI-VALUE C# TUPLES (RETURN TYPES)
        // ---------------------------------------------------------------------
        // SYNTAX:
        (decimal minRate, decimal maxRate) GetRateRange()
        {
            return (120.00m, 780.00m);
        }

        var rates = GetRateRange();
        Console.WriteLine($"Min Rate: {rates.minRate:C}, Max Rate: {rates.maxRate:C}");

        // DESC IN COMMENT:
        // Tuples allow a method to return multiple values without having to create dedicated classes or structs.

        // WHAT DOES IT DO:
        // Bundles multiple typed values into a single returnable unit. Fields can be accessed by their custom names or deconstructed.

        // OUTPUT:
        // Min Rate: $120.00, Max Rate: $780.00

        // NOTE:
        // Replace 'void' in the method signature with the tuple definition:
        // 'public static (decimal vol, decimal chg) CalculateWeight(...)'


        // ---------------------------------------------------------------------
        // 40. DEFENSIVE ARITHMETIC (DIVIDE-BY-ZERO GUARDING)
        // ---------------------------------------------------------------------
        // SYNTAX:
        decimal aggregateWeight = 38.50m;
        int packageCount = 0; // Empty batch scenario

        decimal averageWeight = (packageCount > 0) ? (aggregateWeight / packageCount) : 0m;
        Console.WriteLine($"Calculated average weight: {averageWeight:F2} kg");

        // DESC IN COMMENT:
        // Guarding against division by zero when calculating batch statistics.

        // WHAT DOES IT DO:
        // Checks if the denominator is greater than zero before executing division, preventing DivideByZeroException crashes when batches are empty.

        // OUTPUT:
        // Calculated average weight: 0.00 kg

        // NOTE:
        // In real-world enterprise apps, users can trigger reports before loading data. Always guard denominators with ternary operators or if-conditions.
    }
}

namespace MathUtilities
{
    public static class Calculator
    {
        public static int Add(int a, int b)
        {
            return a + b;
        }
    }
}
