# Making Decisions with If Statements

## What are Conditional Statements?

Conditional statements allow your program to make decisions and execute different code based on certain conditions. It's like giving your program the ability to think!

## The Basic If Statement

```csharp
int temperature = 25;

if (temperature > 20)
{
    Console.WriteLine("It's a warm day!");
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
    Console.WriteLine("You can vote!");
}
else
{
    Console.WriteLine("You're too young to vote.");
}
```

## If-Else If-Else Chain

```csharp
int score = 85;

if (score >= 90)
{
    Console.WriteLine("Grade: A");
}
else if (score >= 80)
{
    Console.WriteLine("Grade: B");
}
else if (score >= 70)
{
    Console.WriteLine("Grade: C");
}
else if (score >= 60)
{
    Console.WriteLine("Grade: D");
}
else
{
    Console.WriteLine("Grade: F");
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
        Console.WriteLine("Welcome to the R-rated movie!");
    }
    else
    {
        Console.WriteLine("Sorry, you need to be 18 or older.");
    }
}
else
{
    Console.WriteLine("Please purchase a ticket first.");
}
```

## Logical Operators in Conditions

### AND Operator (&&)
```csharp
int age = 25;
bool hasLicense = true;

if (age >= 18 && hasLicense)
{
    Console.WriteLine("You can rent a car!");
}
```

### OR Operator (||)
```csharp
string day = "Saturday";

if (day == "Saturday" || day == "Sunday")
{
    Console.WriteLine("It's the weekend!");
}
```

### NOT Operator (!)
```csharp
bool isRaining = false;

if (!isRaining)
{
    Console.WriteLine("Great day for a picnic!");
}
```

## Complex Conditions

```csharp
int temperature = 22;
bool isSunny = true;
bool hasUmbrella = false;

if ((temperature > 20 && isSunny) || (temperature > 15 && hasUmbrella))
{
    Console.WriteLine("Good day to go outside!");
}
else
{
    Console.WriteLine("Maybe stay inside today.");
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
    message = "Adult";
}
else
{
    message = "Minor";
}

// Ternary operator (shorter)
string message2 = age >= 18 ? "Adult" : "Minor";

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
        Console.WriteLine("Monday");
        break;
    case 2:
        Console.WriteLine("Tuesday");
        break;
    case 3:
        Console.WriteLine("Wednesday");
        break;
    case 4:
        Console.WriteLine("Thursday");
        break;
    case 5:
        Console.WriteLine("Friday");
        break;
    case 6:
    case 7:
        Console.WriteLine("Weekend!");
        break;
    default:
        Console.WriteLine("Invalid day");
        break;
}
```

## Modern Switch Expression (C# 8+)

```csharp
string dayName = dayOfWeek switch
{
    1 => "Monday",
    2 => "Tuesday",
    3 => "Wednesday",
    4 => "Thursday",
    5 => "Friday",
    6 or 7 => "Weekend",
    _ => "Invalid day"
};
```

## Real-World Examples

### 1. Login System
```csharp
string username = "admin";
string password = "secret123";

if (username == "admin" && password == "secret123")
{
    Console.WriteLine("Login successful!");
    Console.WriteLine("Welcome to the admin panel.");
}
else
{
    Console.WriteLine("Invalid credentials!");
    Console.WriteLine("Please try again.");
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

Console.WriteLine($"Original Amount: ${totalAmount:F2}");
Console.WriteLine($"Discount: {discount * 100:F1}%");
Console.WriteLine($"Final Amount: ${finalAmount:F2}");
```

### 3. Weather Recommendation System
```csharp
int temperature = 18;
bool isRaining = true;
bool isWindy = false;

string recommendation;

if (temperature < 0)
{
    recommendation = "Stay inside, it's freezing!";
}
else if (temperature < 10)
{
    recommendation = "Wear a heavy coat!";
}
else if (temperature < 20)
{
    if (isRaining)
    {
        recommendation = "Bring an umbrella and wear a jacket!";
    }
    else
    {
        recommendation = "Perfect weather for a walk!";
    }
}
else
{
    if (isRaining)
    {
        recommendation = "Light rain gear should be enough.";
    }
    else
    {
        recommendation = "Great day to be outside!";
    }
}

Console.WriteLine($"Weather recommendation: {recommendation}");
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
    Console.WriteLine("Numbers are equal");
}
```

## Practice Exercises

1. **Age Category Classifier:**
```csharp
int age = 25;

if (age < 13)
    Console.WriteLine("Child");
else if (age < 20)
    Console.WriteLine("Teenager");
else if (age < 60)
    Console.WriteLine("Adult");
else
    Console.WriteLine("Senior");
```

2. **Simple Calculator:**
```csharp
double num1 = 10;
double num2 = 5;
char operation = '+';

if (operation == '+')
    Console.WriteLine($"Result: {num1 + num2}");
else if (operation == '-')
    Console.WriteLine($"Result: {num1 - num2}");
else if (operation == '*')
    Console.WriteLine($"Result: {num1 * num2}");
else if (operation == '/' && num2 != 0)
    Console.WriteLine($"Result: {num1 / num2}");
else
    Console.WriteLine("Invalid operation or division by zero");
```

## Key Takeaways

- If statements control program flow based on conditions
- Use logical operators to combine multiple conditions
- Switch statements are great for multiple exact value comparisons
- Always consider edge cases and invalid inputs
- Keep your conditions readable and well-organized