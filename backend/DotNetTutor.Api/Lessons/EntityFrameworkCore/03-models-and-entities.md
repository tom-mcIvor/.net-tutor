# Models and Entities

## What are Entities?

In Entity Framework Core, an **entity** is a C# class that represents data stored in your database. Each entity class typically maps to a database table, and each property maps to a column.

## Entity Conventions

EF Core follows conventions to automatically configure your entities:

### 1. Table Names
- Class name becomes the table name
- `Blog` class → `Blogs` table (pluralized)
- Use `DbSet` property name: `DbSet<Blog> BlogPosts` → `BlogPosts` table

### 2. Primary Keys
EF Core automatically detects primary keys using these patterns:
- Property named `Id`
- Property named `{ClassName}Id` (e.g., `BlogId`)

```csharp
public class Blog
{
    public int Id { get; set; }          // Primary key
    public string Title { get; set; }
}

// OR

public class Blog
{
    public int BlogId { get; set; }      // Primary key
    public string Title { get; set; }
}
```

### 3. Property Data Types

| C# Type | SQL Server Type | SQLite Type |
|---------|-----------------|-------------|
| `int` | `int` | `INTEGER` |
| `string` | `nvarchar(max)` | `TEXT` |
| `DateTime` | `datetime2` | `TEXT` |
| `bool` | `bit` | `INTEGER` |
| `decimal` | `decimal(18,2)` | `TEXT` |
| `Guid` | `uniqueidentifier` | `TEXT` |

## Creating Your First Entity

Let's create a comprehensive `Student` entity:

```csharp
// Models/Student.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreDemo.Models
{
    public class Student
    {
        // Primary key (auto-detected)
        public int Id { get; set; }
        
        // Required string with max length
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        
        // Unique email
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        // Date of birth
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        // Enrollment date with default value
        public DateTime EnrollmentDate { get; set; } = DateTime.Now;
        
        // Nullable property
        public string? PhoneNumber { get; set; }
        
        // Boolean property
        public bool IsActive { get; set; } = true;
        
        // Calculated property (not stored in database)
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        
        [NotMapped]
        public int Age => DateTime.Now.Year - DateOfBirth.Year;
        
        // Navigation properties
        public List<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public StudentProfile Profile { get; set; }
    }
}
```

## Data Annotations

Data annotations are attributes that provide metadata about your entity properties:

### Validation Annotations
```csharp
public class Course
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Course title is required")]
    [StringLength(200, MinimumLength = 3)]
    public string Title { get; set; }
    
    [Range(1, 10, ErrorMessage = "Credits must be between 1 and 10")]
    public int Credits { get; set; }
    
    [EmailAddress]
    public string InstructorEmail { get; set; }
    
    [Url]
    public string? CourseWebsite { get; set; }
    
    [RegularExpression(@"^[A-Z]{2,4}\d{3}$", ErrorMessage = "Invalid course code format")]
    public string CourseCode { get; set; }
}
```

### Database Configuration Annotations
```csharp
public class Product
{
    [Key]  // Explicitly mark as primary key
    public int ProductId { get; set; }
    
    [Column("ProductName")]  // Different column name
    public string Name { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]  // Specify SQL type
    public decimal Price { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedAt { get; set; }
    
    [NotMapped]  // Don't include in database
    public string DisplayName { get; set; }
}
```

## Entity Relationships

### One-to-One Relationship
```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // One student has one profile
    public StudentProfile Profile { get; set; }
}

public class StudentProfile
{
    public int Id { get; set; }
    public string Bio { get; set; }
    public string ProfilePicture { get; set; }
    
    // Foreign key
    public int StudentId { get; set; }
    public Student Student { get; set; }
}
```

### One-to-Many Relationship
```csharp
public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    // One blog has many posts
    public List<Post> Posts { get; set; } = new List<Post>();
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    
    // Foreign key
    public int BlogId { get; set; }
    
    // Navigation property
    public Blog Blog { get; set; }
}
```

### Many-to-Many Relationship
```csharp
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Many students can take many courses
    public List<Course> Courses { get; set; } = new List<Course>();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    
    // Many courses can have many students
    public List<Student> Students { get; set; } = new List<Student>();
}
```

## Complex Entity Example

Here's a comprehensive entity that demonstrates multiple concepts:

```csharp
// Models/Order.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CustomerOrders")]  // Custom table name
public class Order
{
    [Key]
    public int OrderId { get; set; }
    
    [Required]
    [StringLength(50)]
    public string OrderNumber { get; set; }
    
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalAmount { get; set; }
    
    [DataType(DataType.Date)]
    public DateTime OrderDate { get; set; }
    
    [StringLength(20)]
    public string Status { get; set; } = "Pending";
    
    // Customer information
    [Required]
    [StringLength(100)]
    public string CustomerName { get; set; }
    
    [EmailAddress]
    public string CustomerEmail { get; set; }
    
    [Phone]
    public string? CustomerPhone { get; set; }
    
    // Calculated properties
    [NotMapped]
    public bool IsRecent => OrderDate >= DateTime.Now.AddDays(-30);
    
    [NotMapped]
    public string FormattedTotal => TotalAmount.ToString("C");
    
    // Navigation properties
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ShippingAddress ShippingAddress { get; set; }
}

public class OrderItem
{
    public int Id { get; set; }
    
    [Required]
    public string ProductName { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
    
    [Column(TypeName = "decimal(8,2)")]
    public decimal UnitPrice { get; set; }
    
    // Foreign key
    public int OrderId { get; set; }
    public Order Order { get; set; }
    
    // Calculated property
    [NotMapped]
    public decimal LineTotal => Quantity * UnitPrice;
}
```

## Entity Configuration with Fluent API

Sometimes you need more control than data annotations provide:

```csharp
// Data/BloggingContext.cs
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Configure Student entity
    modelBuilder.Entity<Student>(entity =>
    {
        entity.HasKey(s => s.Id);
        
        entity.Property(s => s.FirstName)
            .IsRequired()
            .HasMaxLength(100);
            
        entity.Property(s => s.Email)
            .IsRequired()
            .HasMaxLength(255);
            
        entity.HasIndex(s => s.Email)
            .IsUnique();
            
        entity.Property(s => s.EnrollmentDate)
            .HasDefaultValueSql("GETDATE()");
    });
    
    // Configure relationships
    modelBuilder.Entity<Post>()
        .HasOne(p => p.Blog)
        .WithMany(b => b.Posts)
        .HasForeignKey(p => p.BlogId)
        .OnDelete(DeleteBehavior.Cascade);
}
```

## Best Practices for Entities

### 1. Use Meaningful Names
```csharp
// Good
public class CustomerOrder { }

// Avoid
public class CO { }
```

### 2. Initialize Collections
```csharp
public class Blog
{
    public List<Post> Posts { get; set; } = new List<Post>();
}
```

### 3. Use Nullable Reference Types
```csharp
public class Student
{
    public string Name { get; set; } = null!;  // Required
    public string? Nickname { get; set; }      // Optional
}
```

### 4. Keep Entities Focused
- One entity per table concept
- Avoid business logic in entities
- Use separate DTOs for API responses

### 5. Use Consistent Naming
- PascalCase for class and property names
- Meaningful foreign key names (`BlogId`, not `ForeignKey1`)

## Entity States

EF Core tracks the state of each entity:

```csharp
using (var context = new BloggingContext())
{
    var blog = new Blog { Title = "My Blog" };
    
    Console.WriteLine(context.Entry(blog).State);  // Detached
    
    context.Blogs.Add(blog);
    Console.WriteLine(context.Entry(blog).State);  // Added
    
    context.SaveChanges();
    Console.WriteLine(context.Entry(blog).State);  // Unchanged
    
    blog.Title = "Updated Title";
    Console.WriteLine(context.Entry(blog).State);  // Modified
    
    context.Blogs.Remove(blog);
    Console.WriteLine(context.Entry(blog).State);  // Deleted
}
```

## Common Entity Patterns

### Base Entity Pattern
```csharp
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class Blog : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
}
```

### Audit Trail Pattern
```csharp
public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    string CreatedBy { get; set; }
    DateTime? ModifiedAt { get; set; }
    string? ModifiedBy { get; set; }
}

public class Product : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}
```

## Summary

Entities are the foundation of your EF Core application. Key takeaways:

- ✅ Follow EF Core conventions for automatic configuration
- ✅ Use data annotations for validation and basic configuration
- ✅ Use Fluent API for complex configuration
- ✅ Initialize navigation properties as empty collections
- ✅ Keep entities focused and avoid business logic
- ✅ Use meaningful names and consistent patterns

## What's Next?

In the next lesson, you'll learn how to:
- Create and apply migrations
- Generate your database from entities
- Handle schema changes over time
- Use migration commands effectively

Your entities are ready - let's bring them to life in the database!