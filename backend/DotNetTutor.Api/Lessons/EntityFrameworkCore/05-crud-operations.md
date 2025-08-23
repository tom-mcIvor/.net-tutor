# CRUD Operations with Entity Framework Core

CRUD stands for **Create, Read, Update, Delete** - the four basic operations you'll perform on your data. EF Core makes these operations intuitive and type-safe.

## Setting Up for Examples

Let's use this simple model for our examples:

```csharp
public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    
    public List<Post> Posts { get; set; } = new List<Post>();
}

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime PublishedDate { get; set; }
    public int BlogId { get; set; }
    
    public Blog Blog { get; set; }
}
```

## Create Operations

### Adding Single Entities

```csharp
// Method 1: Using Add()
using (var context = new BloggingContext())
{
    var blog = new Blog
    {
        Title = "My First Blog",
        Description = "Learning EF Core",
        IsPublished = true
    };
    
    context.Blogs.Add(blog);
    context.SaveChanges();
    
    Console.WriteLine($"Blog created with ID: {blog.Id}");
}
```

```csharp
// Method 2: Using AddAsync() for better performance
using (var context = new BloggingContext())
{
    var blog = new Blog
    {
        Title = "Async Blog",
        Description = "Using async operations"
    };
    
    await context.Blogs.AddAsync(blog);
    await context.SaveChangesAsync();
}
```

### Adding Multiple Entities

```csharp
using (var context = new BloggingContext())
{
    var blogs = new List<Blog>
    {
        new Blog { Title = "Tech Blog", Description = "About technology" },
        new Blog { Title = "Food Blog", Description = "About recipes" },
        new Blog { Title = "Travel Blog", Description = "About adventures" }
    };
    
    context.Blogs.AddRange(blogs);
    context.SaveChanges();
    
    Console.WriteLine($"Added {blogs.Count} blogs");
}
```

### Adding Related Entities

```csharp
using (var context = new BloggingContext())
{
    var blog = new Blog
    {
        Title = "Programming Blog",
        Description = "All about coding",
        Posts = new List<Post>
        {
            new Post 
            { 
                Title = "Getting Started with C#", 
                Content = "C# is awesome...",
                PublishedDate = DateTime.UtcNow
            },
            new Post 
            { 
                Title = "EF Core Basics", 
                Content = "Learn EF Core...",
                PublishedDate = DateTime.UtcNow.AddDays(1)
            }
        }
    };
    
    context.Blogs.Add(blog);
    context.SaveChanges();
    
    // Both blog and posts are saved with proper foreign keys
}
```

## Read Operations

### Finding by Primary Key

```csharp
using (var context = new BloggingContext())
{
    // Find by ID (returns null if not found)
    var blog = context.Blogs.Find(1);
    
    if (blog != null)
    {
        Console.WriteLine($"Found: {blog.Title}");
    }
    
    // Async version
    var blogAsync = await context.Blogs.FindAsync(1);
}
```

### Basic Queries

```csharp
using (var context = new BloggingContext())
{
    // Get all blogs
    var allBlogs = context.Blogs.ToList();
    
    // Get first blog (throws exception if none found)
    var firstBlog = context.Blogs.First();
    
    // Get first or default (returns null if none found)
    var firstOrDefault = context.Blogs.FirstOrDefault();
    
    // Get single blog (throws exception if 0 or more than 1 found)
    var singleBlog = context.Blogs.Single(b => b.Id == 1);
    
    // Get single or default (returns null if not found)
    var singleOrDefault = context.Blogs.SingleOrDefault(b => b.Id == 1);
}
```

### Filtering with Where

```csharp
using (var context = new BloggingContext())
{
    // Get published blogs
    var publishedBlogs = context.Blogs
        .Where(b => b.IsPublished)
        .ToList();
    
    // Get blogs created this year
    var thisYearBlogs = context.Blogs
        .Where(b => b.CreatedDate.Year == DateTime.Now.Year)
        .ToList();
    
    // Complex filtering
    var filteredBlogs = context.Blogs
        .Where(b => b.IsPublished && 
                   b.Title.Contains("Tech") && 
                   b.CreatedDate >= DateTime.Now.AddDays(-30))
        .ToList();
}
```

### Sorting and Paging

```csharp
using (var context = new BloggingContext())
{
    // Sort by title
    var sortedBlogs = context.Blogs
        .OrderBy(b => b.Title)
        .ToList();
    
    // Sort by multiple fields
    var complexSort = context.Blogs
        .OrderByDescending(b => b.CreatedDate)
        .ThenBy(b => b.Title)
        .ToList();
    
    // Paging (skip first 10, take next 5)
    var pagedBlogs = context.Blogs
        .OrderBy(b => b.Id)
        .Skip(10)
        .Take(5)
        .ToList();
}
```

### Loading Related Data

```csharp
using (var context = new BloggingContext())
{
    // Eager loading with Include
    var blogsWithPosts = context.Blogs
        .Include(b => b.Posts)
        .ToList();
    
    // Multiple level include
    var postsWithBlog = context.Posts
        .Include(p => p.Blog)
        .ToList();
    
    // Conditional include
    var blogsWithRecentPosts = context.Blogs
        .Include(b => b.Posts.Where(p => p.PublishedDate >= DateTime.Now.AddDays(-7)))
        .ToList();
}
```

### Projection (Select)

```csharp
using (var context = new BloggingContext())
{
    // Select specific properties
    var blogTitles = context.Blogs
        .Select(b => b.Title)
        .ToList();
    
    // Select to anonymous type
    var blogSummaries = context.Blogs
        .Select(b => new 
        { 
            b.Id, 
            b.Title, 
            PostCount = b.Posts.Count(),
            IsRecent = b.CreatedDate >= DateTime.Now.AddDays(-30)
        })
        .ToList();
    
    // Select to DTO
    var blogDtos = context.Blogs
        .Select(b => new BlogDto
        {
            Id = b.Id,
            Title = b.Title,
            PostCount = b.Posts.Count()
        })
        .ToList();
}
```

## Update Operations

### Updating Single Entity

```csharp
using (var context = new BloggingContext())
{
    // Find and update
    var blog = context.Blogs.Find(1);
    if (blog != null)
    {
        blog.Title = "Updated Title";
        blog.Description = "Updated Description";
        
        context.SaveChanges();
    }
}
```

### Updating Multiple Properties

```csharp
using (var context = new BloggingContext())
{
    var blog = context.Blogs.Find(1);
    if (blog != null)
    {
        // Update multiple properties
        blog.Title = "New Title";
        blog.IsPublished = true;
        blog.Description = "Updated content";
        
        // EF Core tracks all changes automatically
        context.SaveChanges();
    }
}
```

### Bulk Updates (EF Core 7+)

```csharp
using (var context = new BloggingContext())
{
    // Update all unpublished blogs
    await context.Blogs
        .Where(b => !b.IsPublished)
        .ExecuteUpdateAsync(b => b
            .SetProperty(blog => blog.IsPublished, true)
            .SetProperty(blog => blog.Description, "Auto-published"));
}
```

### Partial Updates

```csharp
using (var context = new BloggingContext())
{
    // Update only if entity exists
    var blogToUpdate = new Blog 
    { 
        Id = 1, 
        Title = "New Title",
        Description = "New Description" 
    };
    
    context.Blogs.Attach(blogToUpdate);
    context.Entry(blogToUpdate).Property(b => b.Title).IsModified = true;
    context.Entry(blogToUpdate).Property(b => b.Description).IsModified = true;
    
    context.SaveChanges();
}
```

## Delete Operations

### Deleting Single Entity

```csharp
using (var context = new BloggingContext())
{
    // Find and delete
    var blog = context.Blogs.Find(1);
    if (blog != null)
    {
        context.Blogs.Remove(blog);
        context.SaveChanges();
    }
}
```

### Deleting Multiple Entities

```csharp
using (var context = new BloggingContext())
{
    // Delete all unpublished blogs
    var unpublishedBlogs = context.Blogs
        .Where(b => !b.IsPublished)
        .ToList();
    
    context.Blogs.RemoveRange(unpublishedBlogs);
    context.SaveChanges();
}
```

### Bulk Delete (EF Core 7+)

```csharp
using (var context = new BloggingContext())
{
    // Delete all blogs older than 1 year
    await context.Blogs
        .Where(b => b.CreatedDate < DateTime.Now.AddYears(-1))
        .ExecuteDeleteAsync();
}
```

### Soft Delete Pattern

```csharp
public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}

// Soft delete implementation
using (var context = new BloggingContext())
{
    var blog = context.Blogs.Find(1);
    if (blog != null)
    {
        blog.IsDeleted = true;
        blog.DeletedDate = DateTime.UtcNow;
        
        context.SaveChanges(); // Blog is marked as deleted, not removed
    }
}

// Query only non-deleted blogs
var activeBlogs = context.Blogs
    .Where(b => !b.IsDeleted)
    .ToList();
```

## Advanced CRUD Patterns

### Repository Pattern

```csharp
public interface IBlogRepository
{
    Task<Blog> GetByIdAsync(int id);
    Task<IEnumerable<Blog>> GetAllAsync();
    Task<Blog> AddAsync(Blog blog);
    Task UpdateAsync(Blog blog);
    Task DeleteAsync(int id);
}

public class BlogRepository : IBlogRepository
{
    private readonly BloggingContext _context;
    
    public BlogRepository(BloggingContext context)
    {
        _context = context;
    }
    
    public async Task<Blog> GetByIdAsync(int id)
    {
        return await _context.Blogs
            .Include(b => b.Posts)
            .FirstOrDefaultAsync(b => b.Id == id);
    }
    
    public async Task<IEnumerable<Blog>> GetAllAsync()
    {
        return await _context.Blogs
            .Include(b => b.Posts)
            .ToListAsync();
    }
    
    public async Task<Blog> AddAsync(Blog blog)
    {
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();
        return blog;
    }
    
    public async Task UpdateAsync(Blog blog)
    {
        _context.Entry(blog).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        if (blog != null)
        {
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
    }
}
```

### Unit of Work Pattern

```csharp
public class UnitOfWork : IDisposable
{
    private readonly BloggingContext _context;
    private IBlogRepository _blogRepository;
    private IPostRepository _postRepository;
    
    public UnitOfWork(BloggingContext context)
    {
        _context = context;
    }
    
    public IBlogRepository Blogs => 
        _blogRepository ??= new BlogRepository(_context);
    
    public IPostRepository Posts => 
        _postRepository ??= new PostRepository(_context);
    
    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}
```

## Performance Best Practices

### Use Async Methods
```csharp
// Good - Non-blocking
var blogs = await context.Blogs.ToListAsync();

// Avoid - Blocks thread
var blogs = context.Blogs.ToList();
```

### Track Only What You Need
```csharp
// For read-only queries
var blogs = context.Blogs
    .AsNoTracking()
    .ToList();
```

### Use Compiled Queries for Repeated Queries
```csharp
private static readonly Func<BloggingContext, int, Blog> GetBlogById = 
    EF.CompileQuery((BloggingContext context, int id) => 
        context.Blogs.First(b => b.Id == id));

// Usage
var blog = GetBlogById(context, 1);
```

### Batch Operations
```csharp
// Good - Single database round trip
context.Blogs.AddRange(manyBlogs);
context.SaveChanges();

// Avoid - Multiple round trips
foreach (var blog in manyBlogs)
{
    context.Blogs.Add(blog);
    context.SaveChanges(); // Don't do this!
}
```

## Error Handling

```csharp
public async Task<Blog> CreateBlogAsync(Blog blog)
{
    try
    {
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();
        return blog;
    }
    catch (DbUpdateException ex)
    {
        // Handle database-specific errors
        if (ex.InnerException is SqlException sqlEx)
        {
            if (sqlEx.Number == 2627) // Unique constraint violation
            {
                throw new InvalidOperationException("Blog with this title already exists");
            }
        }
        throw;
    }
    catch (Exception ex)
    {
        // Log error
        _logger.LogError(ex, "Error creating blog");
        throw;
    }
}
```

## Testing CRUD Operations

```csharp
[Test]
public async Task Should_Create_Blog_Successfully()
{
    // Arrange
    var options = new DbContextOptionsBuilder<BloggingContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;
        
    using var context = new BloggingContext(options);
    
    var blog = new Blog
    {
        Title = "Test Blog",
        Description = "Test Description"
    };
    
    // Act
    context.Blogs.Add(blog);
    await context.SaveChangesAsync();
    
    // Assert
    Assert.That(blog.Id, Is.GreaterThan(0));
    
    var savedBlog = await context.Blogs.FindAsync(blog.Id);
    Assert.That(savedBlog, Is.Not.Null);
    Assert.That(savedBlog.Title, Is.EqualTo("Test Blog"));
}
```

## Summary

CRUD operations in EF Core are straightforward and powerful:

- ✅ **Create**: Use `Add()`, `AddRange()`, and `SaveChanges()`
- ✅ **Read**: Use `Find()`, LINQ queries, and `Include()` for related data
- ✅ **Update**: Modify tracked entities and call `SaveChanges()`
- ✅ **Delete**: Use `Remove()`, `RemoveRange()`, or bulk operations
- ✅ Always use async methods for better performance
- ✅ Handle errors appropriately
- ✅ Consider patterns like Repository and Unit of Work for larger applications

## What's Next?

In the next lesson, you'll learn about:
- Defining relationships between entities
- Navigation properties
- Foreign keys and constraints
- Working with complex object graphs

You now have the foundation for all basic database operations. Time to explore entity relationships!