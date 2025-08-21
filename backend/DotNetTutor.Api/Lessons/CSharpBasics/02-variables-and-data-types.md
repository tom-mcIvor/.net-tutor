# Variables and Data Types

## What are Variables?

Variables are containers that store data values. Think of them as labeled boxes where you can put different types of information.

## Declaring Variables

```csharp
// Syntax: dataType variableName = value;
int age = 25;
string name = "John";
bool isStudent = true;
```

## Common Data Types

### Numeric Types

```csharp
// Integers (whole numbers)
int score = 100;           // -2,147,483,648 to 2,147,483,647
long population = 8000000000L;  // Very large numbers

// Decimal numbers
double price = 19.99;      // Double precision
float temperature = 36.5f; // Single precision
decimal money = 1000.50m;  // High precision for financial calculations
```

### Text and Characters

```csharp
// Single character
char grade = 'A';

// Text (string of characters)
string firstName = "Alice";
string lastName = "Johnson";
string fullName = firstName + " " + lastName; // "Alice Johnson"
```

### Boolean (True/False)

```csharp
bool isOnline = true;
bool isComplete = false;
bool hasPermission = true;
```

## Variable Naming Rules

✅ **Good Names:**
```csharp
int studentAge;
string customerName;
bool isLoggedIn;
```

❌ **Bad Names:**
```csharp
int a;        // Not descriptive
string 123name; // Can't start with number
bool is-valid;  // Can't use hyphens
```

## Working with Variables

```csharp
// Declaring and initializing
int x = 10;
int y = 20;

// Using variables in calculations
int sum = x + y;        // 30
int difference = y - x; // 10
int product = x * y;    // 200

// Updating variables
x = x + 5;  // x is now 15
y++;        // y is now 21 (increment by 1)
```

## String Operations

```csharp
string greeting = "Hello";
string name = "World";

// Concatenation
string message = greeting + ", " + name + "!"; // "Hello, World!"

// String interpolation (modern way)
string modernMessage = $"{greeting}, {name}!"; // "Hello, World!"

// String properties and methods
int length = name.Length;           // 5
string upper = name.ToUpper();      // "WORLD"
string lower = name.ToLower();      // "world"
```

## Practice Exercise

Try creating variables for:
1. Your age (integer)
2. Your name (string)
3. Whether you like programming (boolean)
4. Your height in meters (double)

```csharp
int myAge = 25;
string myName = "Your Name";
bool likesProgramming = true;
double heightInMeters = 1.75;

Console.WriteLine($"Name: {myName}");
Console.WriteLine($"Age: {myAge}");
Console.WriteLine($"Height: {heightInMeters}m");
Console.WriteLine($"Likes Programming: {likesProgramming}");
```

## Key Takeaways

- Variables store data temporarily in memory
- Choose the right data type for your data
- Use descriptive variable names
- C# is strongly typed - each variable has a specific type
- String interpolation (`$""`) is the modern way to combine strings