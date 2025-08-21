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

    public static readonly LessonDto[] AspNetCoreLessons = new[]
    {
        // Module 1: MVC Fundamentals
        new LessonDto
        {
            Id = 10,
            Title = "MVC Fundamentals",
            Description = "Master the Model-View-Controller pattern in ASP.NET Core - the foundation of modern web application architecture.",
            Content = @"# MVC Fundamentals in ASP.NET Core

## What is MVC?

MVC (Model-View-Controller) is an architectural pattern that separates an application into three interconnected components. This separation helps organize code, promotes reusability, and makes applications easier to test and maintain.

## The Three Components

### 🏗️ Model
The **Model** represents data and business logic. It's responsible for:
- Data validation
- Business rules
- Data access logic
- State management

```csharp
// Example: Product Model
public class Product
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = ""Price must be positive"")]
    public decimal Price { get; set; }
    
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    public bool IsActive { get; set; } = true;
}
```

### 👁️ View
The **View** handles the presentation layer. It's responsible for:
- Displaying data to users
- User interface elements
- Formatting and layout
- User input collection

```html
@* Example: Product List View *@
@model IEnumerable<Product>

<h2>Product Catalog</h2>

<div class=""product-grid"">
    @foreach (var product in Model)
    {
        <div class=""product-card"">
            <h3>@product.Name</h3>
            <p class=""price"">$@product.Price.ToString(""F2"")</p>
            <p class=""description"">@product.Description</p>
            <div class=""actions"">
                <a href=""/Products/Details/@product.Id"" class=""btn btn-primary"">View Details</a>
                <a href=""/Products/Edit/@product.Id"" class=""btn btn-secondary"">Edit</a>
            </div>
        </div>
    }
</div>

@if (!Model.Any())
{
    <p class=""no-products"">No products available.</p>
}
```

### 🎮 Controller
The **Controller** acts as an intermediary between Model and View. It:
- Handles user input
- Processes requests
- Coordinates between Model and View
- Returns appropriate responses

```csharp
// Example: Products Controller
[Route(""Products"")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    // GET: Products
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            var products = await _productService.GetAllActiveProductsAsync();
            return View(products);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ""Error retrieving products"");
            return View(""Error"");
        }
    }

    // GET: Products/Details/5
    [HttpGet(""Details/{id:int}"")]
    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // GET: Products/Create
    [HttpGet(""Create"")]
    public IActionResult Create()
    {
        return View(new Product());
    }

    // POST: Products/Create
    [HttpPost(""Create"")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _productService.CreateProductAsync(product);
                TempData[""SuccessMessage""] = ""Product created successfully!"";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ""Error creating product"");
                ModelState.AddModelError("""", ""An error occurred while creating the product."");
            }
        }

        return View(product);
    }

    // GET: Products/Edit/5
    [HttpGet(""Edit/{id:int}"")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost(""Edit/{id:int}"")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _productService.UpdateProductAsync(product);
                TempData[""SuccessMessage""] = ""Product updated successfully!"";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ""Error updating product"");
                ModelState.AddModelError("""", ""An error occurred while updating the product."");
            }
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost(""Delete/{id:int}"")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            TempData[""SuccessMessage""] = ""Product deleted successfully!"";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ""Error deleting product"");
            TempData[""ErrorMessage""] = ""An error occurred while deleting the product."";
        }

        return RedirectToAction(nameof(Index));
    }
}
```

## MVC Flow in ASP.NET Core

### 1. Request Processing
```
User Request → Routing → Controller → Action Method
```

### 2. Action Execution
```
Controller Action → Model Processing → Business Logic
```

### 3. Response Generation
```
Controller → View Selection → View Rendering → HTML Response
```

## Routing in MVC

### Convention-Based Routing
```csharp
// In Program.cs or Startup.cs
app.MapControllerRoute(
    name: ""default"",
    pattern: ""{controller=Home}/{action=Index}/{id?}"");

// Examples:
// /Products → ProductsController.Index()
// /Products/Details/5 → ProductsController.Details(5)
// /Home → HomeController.Index()
```

### Attribute Routing
```csharp
[Route(""api/[controller]"")]
public class ProductsApiController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok();

    [HttpGet(""{id:int}"")]
    public IActionResult GetById(int id) => Ok();

    [HttpPost]
    public IActionResult Create([FromBody] Product product) => Ok();

    [HttpPut(""{id:int}"")]
    public IActionResult Update(int id, [FromBody] Product product) => Ok();

    [HttpDelete(""{id:int}"")]
    public IActionResult Delete(int id) => Ok();
}
```

## Action Results

### View Results
```csharp
public class HomeController : Controller
{
    // Return a view with model
    public IActionResult Index()
    {
        var model = GetData();
        return View(model);
    }

    // Return a specific view
    public IActionResult About()
    {
        return View(""AboutUs"");
    }

    // Return a partial view
    public IActionResult GetPartial()
    {
        return PartialView(""_ProductCard"", product);
    }
}
```

### Redirect Results
```csharp
public class AccountController : Controller
{
    public IActionResult Login()
    {
        // Redirect to action in same controller
        return RedirectToAction(""Dashboard"");
    }

    public IActionResult Logout()
    {
        // Redirect to action in different controller
        return RedirectToAction(""Index"", ""Home"");
    }

    public IActionResult External()
    {
        // Redirect to external URL
        return Redirect(""https://example.com"");
    }
}
```

### JSON Results
```csharp
[Route(""api/[controller]"")]
public class DataController : ControllerBase
{
    [HttpGet(""products"")]
    public IActionResult GetProducts()
    {
        var products = _service.GetProducts();
        return Json(products);
    }

    [HttpGet(""status"")]
    public IActionResult GetStatus()
    {
        return Ok(new { Status = ""Healthy"", Timestamp = DateTime.Now });
    }
}
```

## Model Binding

### Form Data Binding
```csharp
[HttpPost]
public IActionResult CreateUser(User user)
{
    // ASP.NET Core automatically binds form data to the User model
    if (ModelState.IsValid)
    {
        // Process the user
        return RedirectToAction(""Success"");
    }
    return View(user);
}
```

### Query String Binding
```csharp
// URL: /Products?category=electronics&minPrice=100
public IActionResult Search(string category, decimal? minPrice)
{
    // Parameters automatically bound from query string
    var products = _service.Search(category, minPrice);
    return View(products);
}
```

### Route Data Binding
```csharp
[Route(""Products/{id:int}"")]
public IActionResult Details(int id)
{
    // id parameter bound from route
    var product = _service.GetById(id);
    return View(product);
}
```

## Model Validation

### Data Annotations
```csharp
public class RegisterViewModel
{
    [Required(ErrorMessage = ""Email is required"")]
    [EmailAddress(ErrorMessage = ""Invalid email format"")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = ""Password is required"")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = ""Password must be 6-100 characters"")]
    public string Password { get; set; } = string.Empty;

    [Compare(""Password"", ErrorMessage = ""Passwords do not match"")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Range(18, 120, ErrorMessage = ""Age must be between 18 and 120"")]
    public int Age { get; set; }
}
```

### Controller Validation
```csharp
[HttpPost]
public IActionResult Register(RegisterViewModel model)
{
    if (ModelState.IsValid)
    {
        // Process registration
        return RedirectToAction(""Welcome"");
    }

    // Return view with validation errors
    return View(model);
}
```

### Custom Validation
```csharp
public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var email = value as string;
        var userService = validationContext.GetService<IUserService>();
        
        if (userService.EmailExists(email))
        {
            return new ValidationResult(""Email already exists"");
        }
        
        return ValidationResult.Success;
    }
}

public class User
{
    [UniqueEmail]
    public string Email { get; set; } = string.Empty;
}
```

## Dependency Injection in MVC

### Service Registration
```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(""DefaultConnection"")));

var app = builder.Build();
```

### Constructor Injection
```csharp
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger,
        IMapper mapper)
    {
        _productService = productService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        var viewModels = _mapper.Map<List<ProductViewModel>>(products);
        return View(viewModels);
    }
}
```

## ViewModels and DTOs

### ViewModel Pattern
```csharp
// ViewModel for displaying data
public class ProductListViewModel
{
    public List<ProductSummaryViewModel> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string SearchTerm { get; set; } = string.Empty;
    public string SortBy { get; set; } = ""Name"";
}

public class ProductSummaryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FormattedPrice => $""${Price:F2}"";
    public bool IsOnSale { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

// Controller usage
public async Task<IActionResult> Index(int page = 1, string search = """")
{
    var products = await _productService.GetPagedAsync(page, 10, search);
    
    var viewModel = new ProductListViewModel
    {
        Products = _mapper.Map<List<ProductSummaryViewModel>>(products.Items),
        TotalCount = products.TotalCount,
        PageNumber = page,
        PageSize = 10,
        SearchTerm = search
    };
    
    return View(viewModel);
}
```

## Error Handling in MVC

### Global Error Handling
```csharp
// Program.cs
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(""/Home/Error"");
    app.UseHsts();
}
```

### Custom Error Controller
```csharp
public class ErrorController : Controller
{
    [Route(""Error/{statusCode}"")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                ViewBag.ErrorMessage = ""Sorry, the page you requested could not be found."";
                break;
            case 500:
                ViewBag.ErrorMessage = ""Sorry, an internal server error occurred."";
                break;
            default:
                ViewBag.ErrorMessage = ""An error occurred while processing your request."";
                break;
        }
        
        return View(""Error"");
    }
}
```

### Try-Catch in Actions
```csharp
public async Task<IActionResult> Create(Product product)
{
    try
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateAsync(product);
            TempData[""Success""] = ""Product created successfully!"";
            return RedirectToAction(nameof(Index));
        }
    }
    catch (ValidationException ex)
    {
        ModelState.AddModelError("""", ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, ""Error creating product"");
        ModelState.AddModelError("""", ""An unexpected error occurred."");
    }
    
    return View(product);
}
```

## Best Practices

### 1. Separation of Concerns
- Keep controllers thin - delegate business logic to services
- Use ViewModels for complex view data
- Separate data access logic into repositories

### 2. Naming Conventions
- Controllers: `ProductsController`, `UsersController`
- Actions: `Index`, `Details`, `Create`, `Edit`, `Delete`
- Views: Match action names (`Index.cshtml`, `Details.cshtml`)

### 3. Security
```csharp
[Authorize] // Require authentication
public class AdminController : Controller
{
    [Authorize(Roles = ""Admin"")] // Require specific role
    public IActionResult ManageUsers() => View();

    [ValidateAntiForgeryToken] // Prevent CSRF attacks
    [HttpPost]
    public IActionResult DeleteUser(int id)
    {
        // Implementation
        return RedirectToAction(nameof(Index));
    }
}
```

### 4. Performance
```csharp
public class ProductsController : Controller
{
    // Use async/await for I/O operations
    public async Task<IActionResult> Index()
    {
        var products = await _productService.GetAllAsync();
        return View(products);
    }

    // Cache frequently accessed data
    [ResponseCache(Duration = 300)] // Cache for 5 minutes
    public IActionResult Categories()
    {
        var categories = _categoryService.GetAll();
        return View(categories);
    }
}
```

## Common Patterns

### Repository Pattern
```csharp
public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllActiveAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Where(p => p.IsActive);
    }
}
```

### Unit of Work Pattern
```csharp
public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    Task<int> SaveChangesAsync();
}

public class ProductsController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProductsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            await _unitOfWork.Products.CreateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }
}
```

## Testing MVC Applications

### Unit Testing Controllers
```csharp
[Test]
public async Task Index_ReturnsViewWithProducts()
{
    // Arrange
    var mockService = new Mock<IProductService>();
    var products = new List<Product> { new Product { Name = ""Test"" } };
    mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);
    
    var controller = new ProductsController(mockService.Object);

    // Act
    var result = await controller.Index();

    // Assert
    var viewResult = Assert.IsType<ViewResult>(result);
    var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
    Assert.Single(model);
}
```

### Integration Testing
```csharp
[Test]
public async Task GetProducts_ReturnsSuccessAndCorrectContentType()
{
    // Arrange
    var client = _factory.CreateClient();

    // Act
    var response = await client.GetAsync(""/Products"");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal(""text/html; charset=utf-8"",
        response.Content.Headers.ContentType.ToString());
}
```

## Key Takeaways

- **MVC separates concerns**: Models handle data, Views handle presentation, Controllers handle user input
- **Controllers are the traffic directors**: They coordinate between Models and Views
- **Use dependency injection**: Makes code testable and maintainable
- **Follow naming conventions**: Keeps code organized and predictable
- **Validate user input**: Always validate on both client and server side
- **Handle errors gracefully**: Provide meaningful error messages to users
- **Keep controllers thin**: Move business logic to services
- **Use ViewModels**: Shape data specifically for views
- **Test your code**: Unit test controllers and integration test the full pipeline

MVC is the foundation of ASP.NET Core web applications. Master these concepts and you'll be able to build robust, maintainable web applications!"
        },

        // Module 2: Dependency Injection
        new LessonDto
        {
            Id = 11,
            Title = "Dependency Injection in ASP.NET Core",
            Description = "Master dependency injection - the cornerstone of modern ASP.NET Core applications for building maintainable and testable code.",
            Content = @"# Dependency Injection in ASP.NET Core

## What is Dependency Injection?

Dependency Injection (DI) is a design pattern that implements Inversion of Control (IoC) for resolving dependencies. Instead of a class creating its own dependencies, they are provided (injected) from the outside. This makes code more modular, testable, and maintainable.

## The Problem Without DI

```csharp
// Tightly coupled code - BAD
public class OrderService
{
    private readonly EmailService _emailService;
    private readonly DatabaseContext _dbContext;

    public OrderService()
    {
        // Hard-coded dependencies - difficult to test and maintain
        _emailService = new EmailService();
        _dbContext = new DatabaseContext(""connectionString"");
    }

    public async Task ProcessOrderAsync(Order order)
    {
        // Save to database
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();

        // Send confirmation email
        await _emailService.SendOrderConfirmationAsync(order);
    }
}
```

**Problems with this approach:**
- Hard to unit test (can't mock dependencies)
- Violates Single Responsibility Principle
- Difficult to change implementations
- Creates tight coupling between classes

## The Solution With DI

```csharp
// Loosely coupled code - GOOD
public interface IEmailService
{
    Task SendOrderConfirmationAsync(Order order);
}

public interface IOrderRepository
{
    Task<Order> CreateAsync(Order order);
    Task<Order> GetByIdAsync(int id);
}

public class OrderService
{
    private readonly IEmailService _emailService;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<OrderService> _logger;

    // Dependencies injected through constructor
    public OrderService(
        IEmailService emailService,
        IOrderRepository orderRepository,
        ILogger<OrderService> logger)
    {
        _emailService = emailService;
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<Order> ProcessOrderAsync(Order order)
    {
        try
        {
            _logger.LogInformation(""Processing order {OrderId}"", order.Id);

            // Save to database
            var savedOrder = await _orderRepository.CreateAsync(order);

            // Send confirmation email
            await _emailService.SendOrderConfirmationAsync(savedOrder);

            _logger.LogInformation(""Order {OrderId} processed successfully"", savedOrder.Id);
            return savedOrder;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ""Error processing order {OrderId}"", order.Id);
            throw;
        }
    }
}
```

## ASP.NET Core Built-in DI Container

ASP.NET Core includes a built-in DI container that manages the lifecycle of your services.

### Service Registration

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Register services with different lifetimes
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddTransient<INotificationService, NotificationService>();

// Register DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(""DefaultConnection"")));

// Register built-in services
builder.Services.AddControllers();
builder.Services.AddLogging();

var app = builder.Build();
```

## Service Lifetimes

### 1. Transient
A new instance is created every time the service is requested.

```csharp
builder.Services.AddTransient<ITransientService, TransientService>();

// Usage example - good for lightweight, stateless services
public interface ITransientService
{
    Guid GetInstanceId();
}

public class TransientService : ITransientService
{
    private readonly Guid _instanceId = Guid.NewGuid();
    
    public Guid GetInstanceId() => _instanceId;
}
```

### 2. Scoped
One instance per HTTP request (or scope).

```csharp
builder.Services.AddScoped<IScopedService, ScopedService>();

// Usage example - good for services that maintain state during a request
public interface IUserContext
{
    int UserId { get; set; }
    string UserName { get; set; }
}

public class UserContext : IUserContext
{
    public int UserId { get; set; }
    public string UserName { get; set; }
}
```

### 3. Singleton
One instance for the entire application lifetime.

```csharp
builder.Services.AddSingleton<ISingletonService, SingletonService>();

// Usage example - good for expensive-to-create, stateless services
public interface ICacheService
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan expiration);
}

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public MemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public Task<T> GetAsync<T>(string key)
    {
        _cache.TryGetValue(key, out T value);
        return Task.FromResult(value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        _cache.Set(key, value, expiration);
        return Task.CompletedTask;
    }
}
```

## Constructor Injection

The most common form of DI in ASP.NET Core.

```csharp
[ApiController]
[Route(""api/[controller]"")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;

    public ProductsController(
        IProductService productService,
        ILogger<ProductsController> logger,
        IMapper mapper)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
    {
        try
        {
            var products = await _productService.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ""Error retrieving products"");
            return StatusCode(500, ""An error occurred while retrieving products"");
        }
    }
}
```

## Service Registration Patterns

### 1. Interface and Implementation

```csharp
// Define interface
public interface IProductService
{
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product> GetByIdAsync(int id);
    Task<Product> CreateAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(int id);
}

// Implement interface
public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IProductRepository repository, ILogger<ProductService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        _logger.LogInformation(""Retrieving all products"");
        return await _repository.GetAllAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        _logger.LogInformation(""Retrieving product with ID {ProductId}"", id);
        return await _repository.GetByIdAsync(id);
    }

    // ... other methods
}

// Register in DI container
builder.Services.AddScoped<IProductService, ProductService>();
```

### 2. Generic Services

```csharp
// Generic repository interface
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

// Generic repository implementation
public class Repository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(DbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    // ... other methods
}

// Register generic service
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
```

### 3. Factory Pattern with DI

```csharp
// Factory interface
public interface IServiceFactory
{
    IPaymentProcessor CreatePaymentProcessor(PaymentType type);
}

// Factory implementation
public class ServiceFactory : IServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public ServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentProcessor CreatePaymentProcessor(PaymentType type)
    {
        return type switch
        {
            PaymentType.CreditCard => _serviceProvider.GetRequiredService<ICreditCardProcessor>(),
            PaymentType.PayPal => _serviceProvider.GetRequiredService<IPayPalProcessor>(),
            PaymentType.BankTransfer => _serviceProvider.GetRequiredService<IBankTransferProcessor>(),
            _ => throw new ArgumentException($""Unsupported payment type: {type}"")
        };
    }
}

// Register factory and implementations
builder.Services.AddScoped<IServiceFactory, ServiceFactory>();
builder.Services.AddScoped<ICreditCardProcessor, CreditCardProcessor>();
builder.Services.AddScoped<IPayPalProcessor, PayPalProcessor>();
builder.Services.AddScoped<IBankTransferProcessor, BankTransferProcessor>();
```

## Configuration Injection

```csharp
// Configuration class
public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
}

// appsettings.json
{
  ""EmailSettings"": {
    ""SmtpServer"": ""smtp.gmail.com"",
    ""Port"": 587,
    ""Username"": ""your-email@gmail.com"",
    ""Password"": ""your-password"",
    ""EnableSsl"": true
  }
}

// Register configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(""EmailSettings""));

// Use in service
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port);
        client.EnableSsl = _emailSettings.EnableSsl;
        client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

        var message = new MailMessage(_emailSettings.Username, to, subject, body);
        await client.SendMailAsync(message);

        _logger.LogInformation(""Email sent to {Recipient}"", to);
    }
}
```

## Advanced DI Scenarios

### 1. Conditional Registration

```csharp
// Register different implementations based on environment
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddScoped<IEmailService, MockEmailService>();
}
else
{
    builder.Services.AddScoped<IEmailService, SmtpEmailService>();
}

// Register based on configuration
var useRedisCache = builder.Configuration.GetValue<bool>(""UseRedisCache"");
if (useRedisCache)
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration.GetConnectionString(""Redis"");
    });
    builder.Services.AddScoped<ICacheService, RedisCacheService>();
}
else
{
    builder.Services.AddMemoryCache();
    builder.Services.AddScoped<ICacheService, MemoryCacheService>();
}
```

### 2. Decorator Pattern

```csharp
// Base service
public interface IOrderService
{
    Task<Order> ProcessOrderAsync(Order order);
}

public class OrderService : IOrderService
{
    public async Task<Order> ProcessOrderAsync(Order order)
    {
        // Core business logic
        await Task.Delay(100); // Simulate processing
        return order;
    }
}

// Decorator for logging
public class LoggingOrderServiceDecorator : IOrderService
{
    private readonly IOrderService _orderService;
    private readonly ILogger<LoggingOrderServiceDecorator> _logger;

    public LoggingOrderServiceDecorator(IOrderService orderService, ILogger<LoggingOrderServiceDecorator> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task<Order> ProcessOrderAsync(Order order)
    {
        _logger.LogInformation(""Processing order {OrderId}"", order.Id);
        var result = await _orderService.ProcessOrderAsync(order);
        _logger.LogInformation(""Order {OrderId} processed successfully"", result.Id);
        return result;
    }
}

// Decorator for caching
public class CachingOrderServiceDecorator : IOrderService
{
    private readonly IOrderService _orderService;
    private readonly ICacheService _cacheService;

    public CachingOrderServiceDecorator(IOrderService orderService, ICacheService cacheService)
    {
        _orderService = orderService;
        _cacheService = cacheService;
    }

    public async Task<Order> ProcessOrderAsync(Order order)
    {
        var cacheKey = $""order_{order.Id}"";
        var cachedOrder = await _cacheService.GetAsync<Order>(cacheKey);
        
        if (cachedOrder != null)
        {
            return cachedOrder;
        }

        var result = await _orderService.ProcessOrderAsync(order);
        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
    }
}

// Registration with decorators
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderService>(provider =>
{
    var orderService = provider.GetRequiredService<OrderService>();
    var logger = provider.GetRequiredService<ILogger<LoggingOrderServiceDecorator>>();
    var cacheService = provider.GetRequiredService<ICacheService>();

    // Wrap with decorators
    var loggingDecorator = new LoggingOrderServiceDecorator(orderService, logger);
    var cachingDecorator = new CachingOrderServiceDecorator(loggingDecorator, cacheService);
    
    return cachingDecorator;
});
```

### 3. Named Services

```csharp
// Multiple implementations of the same interface
public interface INotificationService
{
    Task SendNotificationAsync(string message);
}

public class EmailNotificationService : INotificationService
{
    public async Task SendNotificationAsync(string message)
    {
        // Send email notification
        await Task.Delay(100);
    }
}

public class SmsNotificationService : INotificationService
{
    public async Task SendNotificationAsync(string message)
    {
        // Send SMS notification
        await Task.Delay(100);
    }
}

// Factory to resolve named services
public interface INotificationServiceFactory
{
    INotificationService GetService(string serviceType);
}

public class NotificationServiceFactory : INotificationServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotificationService GetService(string serviceType)
    {
        return serviceType.ToLower() switch
        {
            ""email"" => _serviceProvider.GetRequiredService<EmailNotificationService>(),
            ""sms"" => _serviceProvider.GetRequiredService<SmsNotificationService>(),
            _ => throw new ArgumentException($""Unknown service type: {serviceType}"")
        };
    }
}

// Registration
builder.Services.AddScoped<EmailNotificationService>();
builder.Services.AddScoped<SmsNotificationService>();
builder.Services.AddScoped<INotificationServiceFactory, NotificationServiceFactory>();
```

## Testing with DI

### 1. Unit Testing

```csharp
[Test]
public async Task ProcessOrderAsync_ValidOrder_ReturnsProcessedOrder()
{
    // Arrange
    var mockEmailService = new Mock<IEmailService>();
    var mockRepository = new Mock<IOrderRepository>();
    var mockLogger = new Mock<ILogger<OrderService>>();

    var order = new Order { Id = 1, CustomerName = ""John Doe"" };
    mockRepository.Setup(r => r.CreateAsync(It.IsAny<Order>()))
              .ReturnsAsync(order);

    var orderService = new OrderService(
        mockEmailService.Object,
        mockRepository.Object,
        mockLogger.Object);

    // Act
    var result = await orderService.ProcessOrderAsync(order);

    // Assert
    Assert.That(result, Is.Not.Null);
    Assert.That(result.Id, Is.EqualTo(1));
    mockRepository.Verify(r => r.CreateAsync(order), Times.Once);
    mockEmailService.Verify(e => e.SendOrderConfirmationAsync(order), Times.Once);
}
```

### 2. Integration Testing

```csharp
public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the app's ApplicationDbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Add ApplicationDbContext using an in-memory database for testing
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase(""InMemoryDbForTesting"");
            });

            // Replace email service with mock for testing
            services.AddScoped<IEmailService, MockEmailService>();
        });
    }
}

[Test]
public async Task GetProducts_ReturnsSuccessAndCorrectContentType()
{
    // Arrange
    var factory = new CustomWebApplicationFactory<Program>();
    var client = factory.CreateClient();

    // Act
    var response = await client.GetAsync(""/api/products"");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.That(response.Content.Headers.ContentType?.ToString(),
                Is.EqualTo(""application/json; charset=utf-8""));
}
```

## Best Practices

### 1. Interface Segregation
```csharp
// Bad - Fat interface
public interface IUserService
{
    Task<User> GetUserAsync(int id);
    Task CreateUserAsync(User user);
    Task SendEmailAsync(string email, string subject, string body);
    Task LogUserActivityAsync(int userId, string activity);
    Task GenerateReportAsync();
}

// Good - Segregated interfaces
public interface IUserRepository
{
    Task<User> GetUserAsync(int id);
    Task CreateUserAsync(User user);
}

public interface IEmailService
{
    Task SendEmailAsync(string email, string subject, string body);
}

public interface IUserActivityLogger
{
    Task LogUserActivityAsync(int userId, string activity);
}

public interface IReportGenerator
{
    Task GenerateReportAsync();
}
```

### 2. Avoid Service Locator Anti-pattern
```csharp
// Bad - Service Locator (anti-pattern)
public class OrderService
{
    private readonly IServiceProvider _serviceProvider;

    public OrderService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ProcessOrderAsync(Order order)
    {
        // Don't do this - it hides dependencies
        var emailService = _serviceProvider.GetRequiredService<IEmailService>();
        var repository = _serviceProvider.GetRequiredService<IOrderRepository>();
        
        // ... use services
    }
}

// Good - Explicit dependencies
public class OrderService
{
    private readonly IEmailService _emailService;
    private readonly IOrderRepository _repository;

    public OrderService(IEmailService emailService, IOrderRepository repository)
    {
        _emailService = emailService;
        _repository = repository;
    }

    public async Task ProcessOrderAsync(Order order)
    {
        // Dependencies are clear and testable
        // ... use services
    }
}
```

### 3. Validate Dependencies
```csharp
public class OrderService
{
    private readonly IEmailService _emailService;
    private readonly IOrderRepository _repository;

    public OrderService(IEmailService emailService, IOrderRepository repository)
    {
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
}
```

### 4. Use Appropriate Lifetimes
```csharp
// Singleton - Expensive to create, stateless
builder.Services.AddSingleton<IExpensiveService, ExpensiveService>();

// Scoped - Per request, can maintain state during request
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Transient - Lightweight, stateless
builder.Services.AddTransient<IValidator<Order>, OrderValidator>();
```

## Common Pitfalls

### 1. Circular Dependencies
```csharp
// Bad - Circular dependency
public class ServiceA
{
    public ServiceA(ServiceB serviceB) { }
}

public class ServiceB
{
    public ServiceB(ServiceA serviceA) { } // Circular!
}

// Solution - Introduce an interface or refactor
public interface IServiceA
{
    void DoSomething();
}

public class ServiceA : IServiceA
{
    public ServiceA(IServiceB serviceB) { }
    public void DoSomething() { }
}

public interface IServiceB
{
    void DoSomethingElse();
}

public class ServiceB : IServiceB
{
    public ServiceB(IServiceA serviceA) { }
    public void DoSomethingElse() { }
}
```

### 2. Captive Dependencies
```csharp
// Bad - Singleton capturing scoped service
builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Lives for app lifetime
builder.Services.AddScoped<IScopedService, ScopedService>(); // Lives for request

public class SingletonService : ISingletonService
{
    private readonly IScopedService _scopedService; // Problem!

    public SingletonService(IScopedService scopedService)
    {
        _scopedService = scopedService; // Scoped service captured by singleton
    }
}

// Solution - Use factory or IServiceProvider
public class SingletonService : ISingletonService
{
    private readonly IServiceProvider _serviceProvider;

    public SingletonService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void DoWork()
    {
        using var scope = _serviceProvider.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IScopedService>();
        // Use scoped service
    }
}
```

## Key Takeaways

- **DI promotes loose coupling**: Classes depend on abstractions, not concrete implementations
- **Choose appropriate lifetimes**: Singleton for expensive stateless services, Scoped for per-request state, Transient for lightweight services
- **Use constructor injection**: Makes dependencies explicit and testable
- **Register interfaces, not implementations**: Enables easy swapping of implementations
- **Validate dependencies**: Check for null in constructors
- **Avoid service locator pattern**: Inject specific dependencies instead of IServiceProvider
- **Watch for circular dependencies**: Refactor or use interfaces to break cycles
- **Be careful with captive dependencies**: Don't inject shorter-lived services into longer-lived ones
- **Use configuration objects**: Inject IOptions<T> for configuration settings
- **Test with mocks**: DI makes unit testing much easier

Dependency Injection is fundamental to building maintainable, testable ASP.NET Core applications. Master these concepts and your code will be more flexible, easier to test, and simpler to maintain!"
        }
    };
}