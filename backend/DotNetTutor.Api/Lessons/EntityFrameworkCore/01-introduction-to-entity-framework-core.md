# Introduction to Entity Framework Core

## What is Entity Framework Core?

Entity Framework Core (EF Core) is a modern, lightweight, extensible, and cross-platform Object-Relational Mapping (ORM) framework for .NET. It serves as a bridge between your C# objects and your database, allowing you to work with data using .NET objects without writing most of the data-access code.

## Why Use EF Core?

### 1. **Productivity** 🚀
- Write less code for database operations
- Focus on business logic instead of SQL queries
- Automatic generation of database schemas

### 2. **Type Safety** 🛡️
- Compile-time checking of your database queries
- IntelliSense support for database operations
- Reduced runtime errors

### 3. **Cross-Platform** 🌍
- Works on Windows, macOS, and Linux
- Supports multiple database providers
- Cloud-ready architecture

## How EF Core Works

```
Your C# Classes  ←→  EF Core  ←→  Database
    (Models)         (ORM)        (Tables)
```

EF Core maps your C# classes (called **entities**) to database tables, and your class properties to table columns.

## Key Concepts

### Entities
Classes that represent data in your database:

```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime EnrollmentDate { get; set; }
}
```

### DbContext
The main class that coordinates EF Core functionality:

```csharp
public class SchoolContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("connection-string-here");
    }
}
```

### DbSet
Represents a table in the database:

```csharp
// This represents the Students table
public DbSet<Student> Students { get; set; }
```

## Database Providers

EF Core supports multiple database systems:

- **SQL Server** - Microsoft's flagship database
- **SQLite** - Lightweight, file-based database
- **PostgreSQL** - Popular open-source database
- **MySQL** - World's most popular open-source database
- **Oracle** - Enterprise database system
- **In-Memory** - For testing purposes

## Code First vs Database First

### Code First (Recommended for new projects)
- Write C# classes first
- Generate database from code
- Easy version control

### Database First
- Start with existing database
- Generate C# classes from database
- Good for legacy systems

## Your First EF Core Example

Here's a simple example of reading data:

```csharp
using (var context = new SchoolContext())
{
    // Query all students
    var students = context.Students.ToList();
    
    // Query specific student
    var student = context.Students
        .FirstOrDefault(s => s.Name == "John Doe");
    
    Console.WriteLine($"Found {students.Count} students");
    Console.WriteLine($"Student: {student?.Name}");
}
```

## Benefits Over Raw SQL

| Raw SQL | EF Core |
|---------|---------|
| `SELECT * FROM Students WHERE Id = @id` | `context.Students.Find(id)` |
| `INSERT INTO Students...` | `context.Students.Add(student)` |
| Manual connection management | Automatic connection management |
| No compile-time checking | Full IntelliSense and type safety |

## When NOT to Use EF Core

- **High-performance scenarios** requiring fine-tuned SQL
- **Complex reporting** with heavy aggregations
- **Bulk operations** with millions of records
- **Legacy systems** with complex stored procedures

For these cases, consider using **Dapper** or **raw ADO.NET**.

## What's Next?

In the next lesson, you'll learn how to:
- Install EF Core packages
- Set up your first DbContext
- Configure database connections
- Create your first entity

## Quick Quiz

1. **What does ORM stand for?**
   - Object-Relational Mapping

2. **Name three database providers supported by EF Core:**
   - SQL Server, SQLite, PostgreSQL (MySQL, Oracle, In-Memory)

3. **What class do you inherit from to create your database context?**
   - DbContext

Ready to start building with EF Core? Let's set up your development environment in the next lesson!