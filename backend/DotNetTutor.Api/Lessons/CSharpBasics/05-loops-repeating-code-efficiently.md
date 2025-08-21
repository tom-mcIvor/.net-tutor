# Loops - Repeating Code Efficiently

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
    Console.WriteLine($"Number: {i}");
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
    Console.WriteLine($"Countdown: {i}");
}
Console.WriteLine("Blast off!");
```

## The While Loop

Perfect when you don't know exactly how many iterations you need.

```csharp
int count = 1;

while (count <= 5)
{
    Console.WriteLine($"Count: {count}");
    count++; // Don't forget to increment!
}
```

## The Foreach Loop

Perfect for iterating through collections like arrays.

```csharp
string[] fruits = { "apple", "banana", "orange", "grape" };

foreach (string fruit in fruits)
{
    Console.WriteLine($"I like {fruit}s!");
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
- Always ensure your loop will eventually end to avoid infinite loops