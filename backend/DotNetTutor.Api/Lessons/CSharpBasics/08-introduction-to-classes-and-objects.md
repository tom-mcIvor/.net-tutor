# Introduction to Classes and Objects

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
        Console.WriteLine($"The {Brand} {Model} is starting...");
    }
}

// Creating objects (instances)
Car myCar = new Car();
myCar.Brand = "Toyota";
myCar.Model = "Camry";
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
        Console.WriteLine($"Hi, I'm {Name} and I'm {Age} years old.");
    }
}

// Usage
Person person1 = new Person("Alice", 25);
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
- OOP helps organize and structure larger programs