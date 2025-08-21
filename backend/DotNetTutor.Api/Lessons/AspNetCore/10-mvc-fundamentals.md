# MVC Fundamentals in ASP.NET Core

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
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be positive")]
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

<div class="product-grid">
    @foreach (var product in Model)
    {
        <div class="product-card">
            <h3>@product.Name</h3>
            <p class="price">$@product.Price.ToString("F2")</p>
            <p class="description">@product.Description</p>
            <div class="actions">
                <a href="/Products/Details/@product.Id" class="btn btn-primary">View Details</a>
                <a href="/Products/Edit/@product.Id" class="btn btn-secondary">Edit</a>
            </div>
        </div>
    }
</div>

@if (!Model.Any())
{
    <p class="no-products">No products available.</p>
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
[Route("Products")]
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
            _logger.LogError(ex, "Error retrieving products");
            return View("Error");
        }
    }

    // GET: Products/Details/5
    [HttpGet("Details/{id:int}")]
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
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View(new Product());
    }

    // POST: Products/Create
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _productService.CreateProductAsync(product);
                TempData["SuccessMessage"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError("", "An error occurred while creating the product.");
            }
        }

        return View(product);
    }

    // GET: Products/Edit/5
    [HttpGet("Edit/{id:int}")]
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
    [HttpPost("Edit/{id:int}")]
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
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating product");
                ModelState.AddModelError("", "An error occurred while updating the product.");
            }
        }

        return View(product);
    }

    // POST: Products/Delete/5
    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _productService.DeleteProductAsync(id);
            TempData["SuccessMessage"] = "Product deleted successfully!";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product");
            TempData["ErrorMessage"] = "An error occurred while deleting the product.";
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Examples:
// /Products → ProductsController.Index()
// /Products/Details/5 → ProductsController.Details(5)
// /Home → HomeController.Index()
```

### Attribute Routing
```csharp
[Route("api/[controller]")]
public class ProductsApiController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAll() => Ok();

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id) => Ok();

    [HttpPost]
    public IActionResult Create([FromBody] Product product) => Ok();

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Product product) => Ok();

    [HttpDelete("{id:int}")]
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
        return View("AboutUs");
    }

    // Return a partial view
    public IActionResult GetPartial()
    {
        return PartialView("_ProductCard", product);
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
        return RedirectToAction("Dashboard");
    }

    public IActionResult Logout()
    {
        // Redirect to action in different controller
        return RedirectToAction("Index", "Home");
    }

    public IActionResult External()
    {
        // Redirect to external URL
        return Redirect("https://example.com");
    }
}
```

### JSON Results
```csharp
[Route("api/[controller]")]
public class DataController : ControllerBase
{
    [HttpGet("products")]
    public IActionResult GetProducts()
    {
        var products = _service.GetProducts();
        return Json(products);
    }

    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.Now });
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
        return RedirectToAction("Success");
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
[Route("Products/{id:int}")]
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
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be 6-100 characters")]
    public string Password { get; set; } = string.Empty;

    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
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
        return RedirectToAction("Welcome");
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
            return new ValidationResult("Email already exists");
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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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
    public string SortBy { get; set; } = "Name";
}

public class ProductSummaryViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string FormattedPrice => $"${Price:F2}";
    public bool IsOnSale { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

// Controller usage
public async Task<IActionResult> Index(int page = 1, string search = "")
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
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
```

### Custom Error Controller
```csharp
public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 404:
                ViewBag.ErrorMessage = "Sorry, the page you requested could not be found.";
                break;
            case 500:
                ViewBag.ErrorMessage = "Sorry, an internal server error occurred.";
                break;
            default:
                ViewBag.ErrorMessage = "An error occurred while processing your request.";
                break;
        }
        
        return View("Error");
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
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
    catch (ValidationException ex)
    {
        ModelState.AddModelError("", ex.Message);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error creating product");
        ModelState.AddModelError("", "An unexpected error occurred.");
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
    [Authorize(Roles = "Admin")] // Require specific role
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
    var products = new List<Product> { new Product { Name = "Test" } };
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
    var response = await client.GetAsync("/Products");

    // Assert
    response.EnsureSuccessStatusCode();
    Assert.Equal("text/html; charset=utf-8", 
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

MVC is the foundation of ASP.NET Core web applications. Master these concepts and you'll be able to build robust, maintainable web applications!