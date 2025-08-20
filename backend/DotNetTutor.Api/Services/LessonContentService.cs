using DotNetTutor.Api.Controllers;

namespace DotNetTutor.Api.Services;

public class LessonContentService
{
    public static readonly LessonDto[] CSharpBasicsLessons = new[]
    {
        // Module 1: Getting Started
        new LessonDto
        {
            Id = 1,
            Title = "Welcome to C# Programming",
            Description = "Introduction to C# and the .NET ecosystem for complete beginners.",
            Content = @"# Welcome to C# Programming

## What is C#?

C# (pronounced ""C-Sharp"") is a modern, object-oriented programming language developed by Microsoft. It's part of the .NET ecosystem and is used to build:

- **Desktop Applications** (Windows Forms, WPF)
- **Web Applications** (ASP.NET Core)
- **Mobile Apps** (Xamarin, .NET MAUI)
- **Games** (Unity)
- **Cloud Applications** (Azure)

## Why Learn C#?

✅ **Easy to Learn**: C# has a clean, readable syntax
✅ **Versatile**: Build almost any type of application
✅ **Strong Community**: Huge ecosystem and support
✅ **Career Opportunities**: High demand in the job market
✅ **Cross-Platform**: Runs on Windows, macOS, and Linux

## Your First C# Program

```csharp
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine(""Hello, World!"");
        Console.WriteLine(""Welcome to C# programming!"");
    }
}
```

**What this code does:**
- `using System;` - Imports the System namespace
- `class Program` - Defines a class named Program
- `static void Main()` - The entry point of every C# program
- `Console.WriteLine()` - Prints text to the console

## Key Concepts to Remember

1. **Case Sensitive**: `Main` is different from `main`
2. **Semicolons**: Every statement ends with `;`
3. **Curly Braces**: `{}` group code blocks together
4. **Comments**: Use `//` for single line or `/* */` for multi-line

## Next Steps

In the next lesson, we'll explore variables and data types - the building blocks of any program!"
        },

        // Module 2: Variables and Data Types
        new LessonDto
        {
            Id = 2,
            Title = "Variables and Data Types",
            Description = "Learn how to store and work with different types of data in C#.",
            Content = @"# Variables and Data Types

## What are Variables?

Variables are containers that store data values. Think of them as labeled boxes where you can put different types of information.

## Declaring Variables

```csharp
// Syntax: dataType variableName = value;
int age = 25;
string name = ""John"";
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
string firstName = ""Alice"";
string lastName = ""Johnson"";
string fullName = firstName + "" "" + lastName; // ""Alice Johnson""
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
string greeting = ""Hello"";
string name = ""World"";

// Concatenation
string message = greeting + "", "" + name + ""!""; // ""Hello, World!""

// String interpolation (modern way)
string modernMessage = $""{greeting}, {name}!""; // ""Hello, World!""

// String properties and methods
int length = name.Length;           // 5
string upper = name.ToUpper();      // ""WORLD""
string lower = name.ToLower();      // ""world""
```

## Practice Exercise

Try creating variables for:
1. Your age (integer)
2. Your name (string)
3. Whether you like programming (boolean)
4. Your height in meters (double)

```csharp
int myAge = 25;
string myName = ""Your Name"";
bool likesProgramming = true;
double heightInMeters = 1.75;

Console.WriteLine($""Name: {myName}"");
Console.WriteLine($""Age: {myAge}"");
Console.WriteLine($""Height: {heightInMeters}m"");
Console.WriteLine($""Likes Programming: {likesProgramming}"");
```

## Key Takeaways

- Variables store data temporarily in memory
- Choose the right data type for your data
- Use descriptive variable names
- C# is strongly typed - each variable has a specific type
- String interpolation (`$""""`) is the modern way to combine strings"
        },

        // Module 3: Operators
        new LessonDto
        {
            Id = 3,
            Title = "Operators and Expressions",
            Description = "Master arithmetic, comparison, and logical operators in C#.",
            Content = @"# Operators and Expressions

## What are Operators?

Operators are symbols that perform operations on variables and values. They're like mathematical symbols but for programming.

## Arithmetic Operators

```csharp
int a = 10;
int b = 3;

int addition = a + b;       // 13
int subtraction = a - b;    // 7
int multiplication = a * b; // 30
int division = a / b;       // 3 (integer division)
int remainder = a % b;      // 1 (modulus - remainder after division)

// For decimal division
double preciseDiv = (double)a / b; // 3.333...
```

## Assignment Operators

```csharp
int x = 10;

x += 5;  // Same as: x = x + 5;  (x becomes 15)
x -= 3;  // Same as: x = x - 3;  (x becomes 12)
x *= 2;  // Same as: x = x * 2;  (x becomes 24)
x /= 4;  // Same as: x = x / 4;  (x becomes 6)
x %= 5;  // Same as: x = x % 5;  (x becomes 1)
```

## Increment and Decrement

```csharp
int counter = 5;

counter++;  // Post-increment: counter becomes 6
++counter;  // Pre-increment: counter becomes 7
counter--;  // Post-decrement: counter becomes 6
--counter;  // Pre-decrement: counter becomes 5

// The difference matters in expressions
int a = 5;
int b = a++;  // b = 5, then a becomes 6
int c = ++a;  // a becomes 7, then c = 7
```

## Comparison Operators

```csharp
int x = 10;
int y = 20;

bool isEqual = (x == y);        // false
bool isNotEqual = (x != y);     // true
bool isLess = (x < y);          // true
bool isGreater = (x > y);       // false
bool isLessOrEqual = (x <= y);  // true
bool isGreaterOrEqual = (x >= y); // false
```

## Logical Operators

```csharp
bool isAdult = true;
bool hasLicense = false;
bool isWeekend = true;

// AND operator (&&) - both conditions must be true
bool canDrive = isAdult && hasLicense;  // false

// OR operator (||) - at least one condition must be true
bool canRelax = isWeekend || !hasLicense; // true

// NOT operator (!) - reverses the boolean value
bool isNotAdult = !isAdult;  // false
```

## String Operators

```csharp
string firstName = ""John"";
string lastName = ""Doe"";

// Concatenation
string fullName = firstName + "" "" + lastName; // ""John Doe""

// String interpolation (preferred)
string greeting = $""Hello, {firstName}!""; // ""Hello, John!""

// String comparison
bool sameNames = (firstName == ""John""); // true
```

## Operator Precedence

Operations are performed in a specific order (like math):

```csharp
int result = 2 + 3 * 4;  // 14, not 20 (multiplication first)
int result2 = (2 + 3) * 4; // 20 (parentheses first)

// Order of precedence (highest to lowest):
// 1. Parentheses ()
// 2. Multiplication *, Division /, Modulus %
// 3. Addition +, Subtraction -
// 4. Comparison <, >, <=, >=
// 5. Equality ==, !=
// 6. Logical AND &&
// 7. Logical OR ||
```

## Practical Examples

### Calculator Operations
```csharp
double num1 = 15.5;
double num2 = 4.2;

double sum = num1 + num2;           // 19.7
double difference = num1 - num2;    // 11.3
double product = num1 * num2;       // 65.1
double quotient = num1 / num2;      // 3.69...

Console.WriteLine($""Sum: {sum}"");
Console.WriteLine($""Difference: {difference}"");
```

### Age Verification
```csharp
int age = 18;
bool hasParentPermission = true;

bool canVote = age >= 18;
bool canWatchMovie = age >= 13 || hasParentPermission;

Console.WriteLine($""Can vote: {canVote}"");
Console.WriteLine($""Can watch PG-13 movie: {canWatchMovie}"");
```

### Temperature Conversion
```csharp
double celsius = 25.0;
double fahrenheit = (celsius * 9.0 / 5.0) + 32.0;

Console.WriteLine($""{celsius}°C = {fahrenheit}°F"");
```

## Common Mistakes to Avoid

❌ **Integer Division Surprise**
```csharp
int result = 5 / 2;  // Result is 2, not 2.5!
```

✅ **Correct Way**
```csharp
double result = 5.0 / 2.0;  // Result is 2.5
// or
double result = (double)5 / 2;  // Type casting
```

❌ **Assignment vs Comparison**
```csharp
if (x = 5)  // Wrong! This assigns 5 to x
```

✅ **Correct Way**
```csharp
if (x == 5)  // Correct! This compares x with 5
```

## Practice Exercises

1. **Calculate the area of a rectangle:**
```csharp
double length = 10.5;
double width = 7.2;
double area = length * width;
Console.WriteLine($""Area: {area}"");
```

2. **Check if a number is even:**
```csharp
int number = 42;
bool isEven = (number % 2 == 0);
Console.WriteLine($""{number} is even: {isEven}"");
```

3. **Determine if someone can get a senior discount:**
```csharp
int age = 67;
bool isSenior = age >= 65;
Console.WriteLine($""Eligible for senior discount: {isSenior}"");
```

## Key Takeaways

- Operators perform operations on data
- Use parentheses to control order of operations
- Be careful with integer vs. decimal division
- Logical operators help make complex decisions
- Practice with real-world examples to master operators"
        },

        // Module 4: Control Structures - Conditionals
        new LessonDto
        {
            Id = 4,
            Title = "Making Decisions with If Statements",
            Description = "Learn how to make your programs smart with conditional logic.",
            Content = @"# Making Decisions with If Statements

## What are Conditional Statements?

Conditional statements allow your program to make decisions and execute different code based on certain conditions. It's like giving your program the ability to think!

## The Basic If Statement

```csharp
int temperature = 25;

if (temperature > 20)
{
    Console.WriteLine(""It's a warm day!"");
}
```

**Structure:**
- `if` keyword
- Condition in parentheses `()`
- Code block in curly braces `{}`

## If-Else Statements

```csharp
int age = 17;

if (age >= 18)
{
    Console.WriteLine(""You can vote!"");
}
else
{
    Console.WriteLine(""You're too young to vote."");
}
```

## If-Else If-Else Chain

```csharp
int score = 85;

if (score >= 90)
{
    Console.WriteLine(""Grade: A"");
}
else if (score >= 80)
{
    Console.WriteLine(""Grade: B"");
}
else if (score >= 70)
{
    Console.WriteLine(""Grade: C"");
}
else if (score >= 60)
{
    Console.WriteLine(""Grade: D"");
}
else
{
    Console.WriteLine(""Grade: F"");
}
```

## Nested If Statements

```csharp
bool hasTicket = true;
int age = 16;

if (hasTicket)
{
    if (age >= 18)
    {
        Console.WriteLine(""Welcome to the R-rated movie!"");
    }
    else
    {
        Console.WriteLine(""Sorry, you need to be 18 or older."");
    }
}
else
{
    Console.WriteLine(""Please purchase a ticket first."");
}
```

## Logical Operators in Conditions

### AND Operator (&&)
```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18 && hasLicense)
{
    Console.WriteLine(""You can rent a car!"");
}
```

### OR Operator (||)
```csharp
string day = ""Saturday"";

if (day == ""Saturday"" || day == ""Sunday"")
{
    Console.WriteLine(""It's the weekend!"");
}
```

### NOT Operator (!)
```csharp
bool isRaining = false;

if (!isRaining)
{
    Console.WriteLine(""Great day for a picnic!"");
}
```

## Complex Conditions

```csharp
int temperature = 22;
bool isSunny = true;
bool hasUmbrella = false;

if ((temperature > 20 && isSunny) || (temperature > 15 && hasUmbrella))
{
    Console.WriteLine(""Good day to go outside!"");
}
else
{
    Console.WriteLine(""Maybe stay inside today."");
}
```

## The Ternary Operator (Conditional Operator)

A shorthand way to write simple if-else statements:

```csharp
int age = 20;

// Traditional if-else
string message;
if (age >= 18)
{
    message = ""Adult"";
}
else
{
    message = ""Minor"";
}

// Ternary operator (shorter)
string message2 = age >= 18 ? ""Adult"" : ""Minor"";

// Another example
int a = 10, b = 20;
int max = a > b ? a : b;  // Gets the larger number
```

## Switch Statements

When you have many conditions based on the same variable:

```csharp
int dayOfWeek = 3;

switch (dayOfWeek)
{
    case 1:
        Console.WriteLine(""Monday"");
        break;
    case 2:
        Console.WriteLine(""Tuesday"");
        break;
    case 3:
        Console.WriteLine(""Wednesday"");
        break;
    case 4:
        Console.WriteLine(""Thursday"");
        break;
    case 5:
        Console.WriteLine(""Friday"");
        break;
    case 6:
    case 7:
        Console.WriteLine(""Weekend!"");
        break;
    default:
        Console.WriteLine(""Invalid day"");
        break;
}
```

## Modern Switch Expression (C# 8+)

```csharp
string dayName = dayOfWeek switch
{
    1 => ""Monday"",
    2 => ""Tuesday"",
    3 => ""Wednesday"",
    4 => ""Thursday"",
    5 => ""Friday"",
    6 or 7 => ""Weekend"",
    _ => ""Invalid day""
};
```

## Real-World Examples

### 1. Login System
```csharp
string username = ""admin"";
string password = ""secret123"";

if (username == ""admin"" && password == ""secret123"")
{
    Console.WriteLine(""Login successful!"");
    Console.WriteLine(""Welcome to the admin panel."");
}
else
{
    Console.WriteLine(""Invalid credentials!"");
    Console.WriteLine(""Please try again."");
}
```

### 2. Shopping Discount Calculator
```csharp
double totalAmount = 150.00;
bool isMember = true;
bool isFirstPurchase = false;

double discount = 0;

if (isMember)
{
    discount = 0.10; // 10% member discount
}

if (isFirstPurchase)
{
    discount += 0.05; // Additional 5% for first purchase
}

if (totalAmount > 100)
{
    discount += 0.05; // Additional 5% for orders over $100
}

double finalAmount = totalAmount * (1 - discount);

Console.WriteLine($""Original Amount: ${totalAmount:F2}"");
Console.WriteLine($""Discount: {discount * 100:F1}%"");
Console.WriteLine($""Final Amount: ${finalAmount:F2}"");
```

### 3. Weather Recommendation System
```csharp
int temperature = 18;
bool isRaining = true;
bool isWindy = false;

string recommendation;

if (temperature < 0)
{
    recommendation = ""Stay inside, it's freezing!"";
}
else if (temperature < 10)
{
    recommendation = ""Wear a heavy coat!"";
}
else if (temperature < 20)
{
    if (isRaining)
    {
        recommendation = ""Bring an umbrella and wear a jacket!"";
    }
    else
    {
        recommendation = ""Perfect weather for a walk!"";
    }
}
else
{
    if (isRaining)
    {
        recommendation = ""Light rain gear should be enough."";
    }
    else
    {
        recommendation = ""Great day to be outside!"";
    }
}

Console.WriteLine($""Weather recommendation: {recommendation}"");
```

## Best Practices

✅ **Do:**
- Use meaningful variable names
- Keep conditions simple and readable
- Use parentheses to clarify complex conditions
- Consider using switch for multiple exact matches

❌ **Don't:**
- Write overly complex nested conditions
- Forget the `break` statement in switch cases
- Compare floating-point numbers with `==` (use ranges instead)

## Common Mistakes

### 1. Assignment vs Comparison
```csharp
int x = 5;

// Wrong - this assigns 10 to x!
if (x = 10)  // Compilation error in C#

// Correct - this compares x with 10
if (x == 10)
{
    // code here
}
```

### 2. Floating Point Comparison
```csharp
double a = 0.1 + 0.2;
double b = 0.3;

// Wrong - might not work due to floating point precision
if (a == b)  // Might be false!

// Better approach
if (Math.Abs(a - b) < 0.0001)
{
    Console.WriteLine(""Numbers are equal"");
}
```

## Practice Exercises

1. **Age Category Classifier:**
```csharp
int age = 25;

if (age < 13)
    Console.WriteLine(""Child"");
else if (age < 20)
    Console.WriteLine(""Teenager"");
else if (age < 60)
    Console.WriteLine(""Adult"");
else
    Console.WriteLine(""Senior"");
```

2. **Simple Calculator:**
```csharp
double num1 = 10;
double num2 = 5;
char operation = '+';

if (operation == '+')
    Console.WriteLine($""Result: {num1 + num2}"");
else if (operation == '-')
    Console.WriteLine($""Result: {num1 - num2}"");
else if (operation == '*')
    Console.WriteLine($""Result: {num1 * num2}"");
else if (operation == '/' && num2 != 0)
    Console.WriteLine($""Result: {num1 / num2}"");
else
    Console.WriteLine(""Invalid operation or division by zero"");
```

## Key Takeaways

- If statements control program flow based on conditions
- Use logical operators to combine multiple conditions
- Switch statements are great for multiple exact value comparisons
- Always consider edge cases and invalid inputs
- Keep your conditions readable and well-organized"
        },

        // Module 5: Loops
        new LessonDto
        {
            Id = 5,
            Title = "Loops - Repeating Code Efficiently",
            Description = "Master for loops, while loops, and foreach loops to automate repetitive tasks.",
            Content = @"# Loops - Repeating Code Efficiently

## What are Loops?

Loops allow you to execute the same block of code multiple times without writing it repeatedly. They're essential for automating repetitive tasks.

## The For Loop

Perfect when you know exactly how many times you want to repeat something.

### Basic For Loop Structure
```csharp
for (initialization; condition; increment)
{
    // Code to repeat
}
```

### Simple Examples
```csharp
// Print numbers 1 to 5
for (int i = 1; i <= 5; i++)
{
    Console.WriteLine($""Number: {i}"");
}

// Output:
// Number: 1
// Number: 2
// Number: 3
// Number: 4
// Number: 5
```

### Counting Backwards
```csharp
// Countdown from 10 to 1
for (int i = 10; i >= 1; i--)
{
    Console.WriteLine($""Countdown: {i}"");
}
Console.WriteLine(""Blast off!"");
```

## The While Loop

Perfect when you don't know exactly how many iterations you need.

```csharp
int count = 1;

while (count <= 5)
{
    Console.WriteLine($""Count: {count}"");
    count++; // Don't forget to increment!
}
```

## The Foreach Loop

Perfect for iterating through collections like arrays.

```csharp
string[] fruits = { ""apple"", ""banana"", ""orange"", ""grape"" };

foreach (string fruit in fruits)
{
    Console.WriteLine($""I like {fruit}s!"");
}
```

## Loop Control Statements

### Break Statement
Exits the loop immediately.

```csharp
for (int i = 1; i <= 10; i++)
{
    if (i == 6)
    {
        break; // Exit the loop when i equals 6
    }
    Console.WriteLine(i);
}
// Output: 1, 2, 3, 4, 5
```

### Continue Statement
Skips the rest of the current iteration and moves to the next one.

```csharp
for (int i = 1; i <= 10; i++)
{
    if (i % 2 == 0) // If i is even
    {
        continue; // Skip the rest and go to next iteration
    }
    Console.WriteLine(i);
}
// Output: 1, 3, 5, 7, 9 (only odd numbers)
```

## Key Takeaways

- For loops are best when you know the number of iterations
- While loops are best when the condition determines when to stop
- Foreach loops are perfect for collections
- Use break to exit loops early
- Use continue to skip iterations
- Always ensure your loop will eventually end to avoid infinite loops"
        },

        // Module 6: Methods and Functions
        new LessonDto
        {
            Id = 6,
            Title = "Methods and Functions",
            Description = "Learn to organize your code with reusable methods and functions.",
            Content = @"# Methods and Functions

## What are Methods?

Methods are reusable blocks of code that perform specific tasks. They help organize your code and avoid repetition.

## Basic Method Structure

```csharp
// Method declaration
public static void SayHello()
{
    Console.WriteLine(""Hello, World!"");
}

// Calling the method
SayHello(); // Output: Hello, World!
```

## Methods with Parameters

```csharp
public static void Greet(string name)
{
    Console.WriteLine($""Hello, {name}!"");
}

// Usage
Greet(""Alice""); // Output: Hello, Alice!
Greet(""Bob"");   // Output: Hello, Bob!
```

## Methods with Return Values

```csharp
public static int Add(int a, int b)
{
    return a + b;
}

// Usage
int result = Add(5, 3); // result = 8
Console.WriteLine($""5 + 3 = {result}"");
```

## Method Overloading

```csharp
public static int Add(int a, int b)
{
    return a + b;
}

public static double Add(double a, double b)
{
    return a + b;
}

public static int Add(int a, int b, int c)
{
    return a + b + c;
}

// Usage
int sum1 = Add(1, 2);        // Uses first method
double sum2 = Add(1.5, 2.3); // Uses second method
int sum3 = Add(1, 2, 3);     // Uses third method
```

## Key Takeaways

- Methods organize code into reusable blocks
- Parameters allow methods to work with different data
- Return values let methods provide results
- Method overloading allows multiple methods with the same name"
        },

        // Module 7: Arrays and Collections
        new LessonDto
        {
            Id = 7,
            Title = "Arrays and Collections",
            Description = "Master arrays, lists, and other collections to store multiple values.",
            Content = @"# Arrays and Collections

## What are Arrays?

Arrays store multiple values of the same type in a single variable.

## Creating Arrays

```csharp
// Method 1: Declare and initialize
int[] numbers = { 1, 2, 3, 4, 5 };

// Method 2: Declare then assign
string[] names = new string[3];
names[0] = ""Alice"";
names[1] = ""Bob"";
names[2] = ""Charlie"";

// Method 3: Declare with size
double[] prices = new double[5];
```

## Working with Arrays

```csharp
int[] scores = { 85, 92, 78, 96, 88 };

// Access elements
Console.WriteLine(scores[0]); // 85 (first element)
Console.WriteLine(scores[4]); // 88 (last element)

// Array length
Console.WriteLine($""Array has {scores.Length} elements"");

// Loop through array
for (int i = 0; i < scores.Length; i++)
{
    Console.WriteLine($""Score {i + 1}: {scores[i]}"");
}
```

## Lists (Dynamic Arrays)

```csharp
using System.Collections.Generic;

List<string> fruits = new List<string>();

// Add items
fruits.Add(""apple"");
fruits.Add(""banana"");
fruits.Add(""orange"");

// Access items
Console.WriteLine(fruits[0]); // apple

// Remove items
fruits.Remove(""banana"");

// Count items
Console.WriteLine($""We have {fruits.Count} fruits"");
```

## Key Takeaways

- Arrays store multiple values of the same type
- Use square brackets [] to access elements
- Arrays have fixed size, Lists are dynamic
- Both use zero-based indexing (first element is index 0)"
        },

        // Module 8: Object-Oriented Programming Basics
        new LessonDto
        {
            Id = 8,
            Title = "Introduction to Classes and Objects",
            Description = "Learn the fundamentals of object-oriented programming with classes and objects.",
            Content = @"# Introduction to Classes and Objects

## What is Object-Oriented Programming?

Object-Oriented Programming (OOP) is a way of organizing code around objects that represent real-world things.

## Classes and Objects

A **class** is like a blueprint, and an **object** is something built from that blueprint.

```csharp
// Class definition (blueprint)
public class Car
{
    // Properties (characteristics)
    public string Brand { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    
    // Method (behavior)
    public void Start()
    {
        Console.WriteLine($""The {Brand} {Model} is starting..."");
    }
}

// Creating objects (instances)
Car myCar = new Car();
myCar.Brand = ""Toyota"";
myCar.Model = ""Camry"";
myCar.Year = 2022;

myCar.Start(); // Output: The Toyota Camry is starting...
```

## Constructors

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
    
    // Constructor
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    public void Introduce()
    {
        Console.WriteLine($""Hi, I'm {Name} and I'm {Age} years old."");
    }
}

// Usage
Person person1 = new Person(""Alice"", 25);
person1.Introduce(); // Output: Hi, I'm Alice and I'm 25 years old.
```

## Key OOP Concepts

- **Encapsulation**: Keeping data and methods together
- **Inheritance**: Creating new classes based on existing ones
- **Polymorphism**: Objects can take multiple forms
- **Abstraction**: Hiding complex implementation details

## Key Takeaways

- Classes are blueprints for creating objects
- Objects have properties (data) and methods (behavior)
- Constructors initialize objects when they're created
- OOP helps organize and structure larger programs"
        },

        // Module 9: ASP.NET Core Overview
        new LessonDto
        {
            Id = 9,
            Title = "ASP.NET Core Fundamentals",
            Description = "Learn the core concepts of ASP.NET Core - Microsoft's modern web framework for building high-performance applications.",
            Content = @"# ASP.NET Core Fundamentals

## What is ASP.NET Core?

ASP.NET Core is a cross-platform, high-performance, open-source framework for building modern, cloud-enabled, Internet-connected applications. It's a complete rewrite of ASP.NET that runs on .NET Core.

## Key Benefits

✅ **Cross-Platform**: Runs on Windows, macOS, and Linux
✅ **High Performance**: Optimized for speed and scalability
✅ **Cloud-Ready**: Built for modern cloud deployment
✅ **Open Source**: Available on GitHub with community contributions
✅ **Modular**: Use only what you need with minimal overhead

## Core Architecture

### The Request Pipeline

ASP.NET Core uses a middleware pipeline to handle HTTP requests:

```csharp
public class Startup
{
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

### Dependency Injection

Built-in DI container for managing dependencies:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddScoped<IUserService, UserService>();
    services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
}
```

## MVC Pattern in ASP.NET Core

### Controllers

Controllers handle HTTP requests and return responses:

```csharp
[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet(""{id}"")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdProduct = await _productService.CreateAsync(product);
        return CreatedAtAction(nameof(GetProduct),
            new { id = createdProduct.Id }, createdProduct);
    }
}
```

## Routing

### Attribute Routing

Define routes directly on controllers and actions:

```csharp
[Route(""api/products"")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok();

    [HttpGet(""{id:int}"")]
    public IActionResult GetById(int id) => Ok();

    [HttpGet(""search/{term}"")]
    public IActionResult Search(string term) => Ok();

    [HttpPost]
    public IActionResult Create([FromBody] Product product) => Ok();
}
```

## Configuration

### appsettings.json

Store configuration in JSON files:

```json
{
  ""ConnectionStrings"": {
    ""DefaultConnection"": ""Server=localhost;Database=MyApp;Trusted_Connection=true;""
  },
  ""Logging"": {
    ""LogLevel"": {
      ""Default"": ""Information"",
      ""Microsoft"": ""Warning""
    }
  }
}
```

## Web APIs

### RESTful API Design

Follow REST conventions:

```csharp
[ApiController]
[Route(""api/[controller]"")]
public class BooksController : ControllerBase
{
    // GET api/books
    [HttpGet]
    public ActionResult<IEnumerable<Book>> GetBooks()
    {
        // Return all books
    }

    // GET api/books/5
    [HttpGet(""{id}"")]
    public ActionResult<Book> GetBook(int id)
    {
        // Return specific book
    }

    // POST api/books
    [HttpPost]
    public ActionResult<Book> PostBook(Book book)
    {
        // Create new book
    }

    // PUT api/books/5
    [HttpPut(""{id}"")]
    public IActionResult PutBook(int id, Book book)
    {
        // Update existing book
    }

    // DELETE api/books/5
    [HttpDelete(""{id}"")]
    public IActionResult DeleteBook(int id)
    {
        // Delete book
    }
}
```

## Authentication & Authorization

### JWT Authentication

Configure JWT authentication:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration[""Jwt:Issuer""],
                ValidAudience = Configuration[""Jwt:Audience""],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(Configuration[""Jwt:Key""]))
            };
        });
}
```

## Best Practices

### 1. Use Dependency Injection
- Register services in `ConfigureServices`
- Inject dependencies through constructors
- Use appropriate service lifetimes (Singleton, Scoped, Transient)

### 2. Follow REST Conventions
- Use appropriate HTTP verbs
- Return meaningful status codes
- Use consistent URL patterns

### 3. Implement Proper Error Handling
- Use global exception handling
- Return appropriate error responses
- Log errors for debugging

### 4. Secure Your Application
- Use HTTPS
- Implement authentication and authorization
- Validate all inputs
- Protect against common attacks (XSS, CSRF, SQL Injection)

### 5. Optimize Performance
- Use async/await for I/O operations
- Implement caching where appropriate
- Minimize database queries
- Use compression for responses

## Next Steps

After mastering these fundamentals, explore:

1. **Entity Framework Core** - Data access and ORM
2. **SignalR** - Real-time web functionality
3. **Background Services** - Long-running tasks
4. **Microservices** - Distributed architecture
5. **Azure Integration** - Cloud deployment and services

## Practice Exercise

Create a simple Book API with the following endpoints:
- GET /api/books - Get all books
- GET /api/books/{id} - Get book by ID
- POST /api/books - Create a new book
- PUT /api/books/{id} - Update a book
- DELETE /api/books/{id} - Delete a book

Include proper validation, error handling, and documentation with Swagger.

## Key Takeaways

- ASP.NET Core is a modern, cross-platform web framework
- It uses a middleware pipeline for request processing
- Built-in dependency injection promotes loose coupling
- MVC pattern separates concerns effectively
- RESTful APIs follow standard HTTP conventions
- Security and performance are built-in considerations
- Testing and deployment are streamlined with modern tooling"
        }
    };
}