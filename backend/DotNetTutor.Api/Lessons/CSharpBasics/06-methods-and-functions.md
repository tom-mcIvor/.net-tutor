# Methods and Functions

## What are Methods?

Methods are reusable blocks of code that perform specific tasks. They help organize your code and avoid repetition.

## Basic Method Structure

```csharp
// Method declaration
public static void SayHello()
{
    Console.WriteLine("Hello, World!");
}

// Calling the method
SayHello(); // Output: Hello, World!
```

## Methods with Parameters

```csharp
public static void Greet(string name)
{
    Console.WriteLine($"Hello, {name}!");
}

// Usage
Greet("Alice"); // Output: Hello, Alice!
Greet("Bob");   // Output: Hello, Bob!
```

## Methods with Return Values

```csharp
public static int Add(int a, int b)
{
    return a + b;
}

// Usage
int result = Add(5, 3); // result = 8
Console.WriteLine($"5 + 3 = {result}");
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
- Method overloading allows multiple methods with the same name