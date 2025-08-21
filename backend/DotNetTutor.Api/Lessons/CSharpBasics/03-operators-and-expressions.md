# Operators and Expressions

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
string firstName = "John";
string lastName = "Doe";

// Concatenation
string fullName = firstName + " " + lastName; // "John Doe"

// String interpolation (preferred)
string greeting = $"Hello, {firstName}!"; // "Hello, John!"

// String comparison
bool sameNames = (firstName == "John"); // true
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

Console.WriteLine($"Sum: {sum}");
Console.WriteLine($"Difference: {difference}");
```

### Age Verification
```csharp
int age = 18;
bool hasParentPermission = true;

bool canVote = age >= 18;
bool canWatchMovie = age >= 13 || hasParentPermission;

Console.WriteLine($"Can vote: {canVote}");
Console.WriteLine($"Can watch PG-13 movie: {canWatchMovie}");
```

### Temperature Conversion
```csharp
double celsius = 25.0;
double fahrenheit = (celsius * 9.0 / 5.0) + 32.0;

Console.WriteLine($"{celsius}°C = {fahrenheit}°F");
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
Console.WriteLine($"Area: {area}");
```

2. **Check if a number is even:**
```csharp
int number = 42;
bool isEven = (number % 2 == 0);
Console.WriteLine($"{number} is even: {isEven}");
```

3. **Determine if someone can get a senior discount:**
```csharp
int age = 67;
bool isSenior = age >= 65;
Console.WriteLine($"Eligible for senior discount: {isSenior}");
```

## Key Takeaways

- Operators perform operations on data
- Use parentheses to control order of operations
- Be careful with integer vs. decimal division
- Logical operators help make complex decisions
- Practice with real-world examples to master operators