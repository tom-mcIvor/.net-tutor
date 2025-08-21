# Arrays and Collections

## What are Arrays?

Arrays store multiple values of the same type in a single variable.

## Creating Arrays

```csharp
// Method 1: Declare and initialize
int[] numbers = { 1, 2, 3, 4, 5 };

// Method 2: Declare then assign
string[] names = new string[3];
names[0] = "Alice";
names[1] = "Bob";
names[2] = "Charlie";

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
Console.WriteLine($"Array has {scores.Length} elements");

// Loop through array
for (int i = 0; i < scores.Length; i++)
{
    Console.WriteLine($"Score {i + 1}: {scores[i]}");
}
```

## Lists (Dynamic Arrays)

```csharp
using System.Collections.Generic;

List<string> fruits = new List<string>();

// Add items
fruits.Add("apple");
fruits.Add("banana");
fruits.Add("orange");

// Access items
Console.WriteLine(fruits[0]); // apple

// Remove items
fruits.Remove("banana");

// Count items
Console.WriteLine($"We have {fruits.Count} fruits");
```

## Key Takeaways

- Arrays store multiple values of the same type
- Use square brackets [] to access elements
- Arrays have fixed size, Lists are dynamic
- Both use zero-based indexing (first element is index 0)