# Dependency Injection in ASP.NET Core

## What is Dependency Injection?

Dependency Injection (DI) is a design pattern that helps create loosely coupled, testable, and maintainable applications. ASP.NET Core has a built-in DI container that manages object creation and lifetime.

## Why Use Dependency Injection?

✅ **Loose Coupling**: Classes don't create their own dependencies
✅ **Testability**: Easy to mock dependencies for unit testing
✅ **Maintainability**: Changes to dependencies don't affect dependent classes
✅ **Flexibility**: Easy to swap implementations
✅ **Single Responsibility**: Classes focus on their core functionality

## Basic Concepts

### Without Dependency Injection
```csharp
// Tightly coupled - hard to test and maintain
public class OrderService
{
    private readonly EmailService _emailService;
    private readonly DatabaseContext _context;

    public OrderService()
    {
        _emailService = new EmailService(); // Hard dependency
        _context = new DatabaseContext();   // Hard dependency
    }

    public void ProcessOrder(Order order)
    {
        _context.Orders.Add(order);
        _context.SaveChanges();
        _emailService.SendConfirmation(order.CustomerEmail);
    }
}
```

### With Dependency Injection
```csharp
// Loosely coupled - easy to test and maintain
public class OrderService
{
    private readonly IEmailService _emailService;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IEmailService emailService, IOrderRepository orderRepository)
    {
        _emailService = emailService;
        _orderRepository = orderRepository;
    }

    public async Task ProcessOrderAsync(Order order)
    {
        await _orderRepository.AddAsync(order);
        await _emailService.SendConfirmationAsync(order.CustomerEmail);
    }
}
```

## Service Registration

### Registering Services in Program.cs
```csharp
var builder = WebApplication.CreateBuilder(args);

// Register built-in services
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom services
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddTransient<INotificationService, NotificationService>();

var app = builder.Build();
```

## Service Lifetimes

### Transient
A new instance is created every time the service is requested.

```csharp
builder.Services.AddTransient<ITransientService, TransientService>();

// Usage: New instance each time
public class HomeController : Controller
{
    private readonly ITransientService _service1;
    private readonly ITransientService _service2;

    public HomeController(ITransientService service1, ITransientService service2)
    {
        _service1 = service1; // Different instance
        _service2 = service2; // Different instance
    }
}
```

**When to use:**
- Lightweight, stateless services
- Services that are expensive to keep in memory
- Services that should not be shared

### Scoped
One instance per HTTP request (or scope).

```csharp
builder.Services.AddScoped<IScopedService, ScopedService>();

// Usage: Same instance within the same HTTP request
public class ProductsController : Controller
{
    private readonly IScopedService _service;
    private readonly IOrderService _orderService; // Also scoped

    public ProductsController(IScopedService service, IOrderService orderService)
    {
        _service = service;        // Same instance as in OrderService
        _orderService = orderService; // if both are in same request
    }
}
```

**When to use:**
- Database contexts (Entity Framework)
- Repository patterns
- Services that maintain state during a request

### Singleton
One instance for the entire application lifetime.

```csharp
builder.Services.AddSingleton<ISingletonService, SingletonService>();

// Usage: Same instance across all requests
public class CacheService : ISingletonService
{
    private readonly Dictionary<string, object> _cache = new();

    public void Set(string key, object value)
    {
        _cache[key] = value;
    }

    public T Get<T>(string key)
    {
        return _cache.TryGetValue(key, out var value) ? (T)value : default(T);
    }
}
```

**When to use:**
- Configuration services
- Logging services
- Caching services
- Services that are expensive to create

## Interface-Based Design

### Defining Interfaces
```csharp
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendConfirmationAsync(string email);
}

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(int id);
    Task<IEnumerable<Order>> GetAllAsync();
    Task AddAsync(Order order);
    Task UpdateAsync(Order order);
    Task DeleteAsync(int id);
}

public interface IPaymentService
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<bool> RefundAsync(string transactionId);
}
```

### Implementing Services
```csharp
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            // Email sending logic
            _logger.LogInformation($"Email sent to {to}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to send email to {to}");
            throw;
        }
    }

    public async Task SendConfirmationAsync(string email)
    {
        var subject = "Order Confirmation";
        var body = "Thank you for your order!";
        await SendEmailAsync(email, subject, body);
    }
}
```

## Constructor Injection

### Basic Constructor Injection
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

### Service Dependencies
```csharp
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IEmailService _emailService;
    private readonly IPaymentService _paymentService;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IEmailService emailService,
        IPaymentService paymentService,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _emailService = emailService;
        _paymentService = paymentService;
        _logger = logger;
    }

    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            // Process payment
            var paymentResult = await _paymentService.ProcessPaymentAsync(request.Payment);
            if (!paymentResult.Success)
            {
                return OrderResult.Failed("Payment failed");
            }

            // Create order
            var order = new Order
            {
                CustomerId = request.CustomerId,
                Items = request.Items,
                Total = request.Total,
                PaymentId = paymentResult.TransactionId
            };

            await _orderRepository.AddAsync(order);

            // Send confirmation
            await _emailService.SendConfirmationAsync(request.CustomerEmail);

            _logger.LogInformation($"Order {order.Id} created successfully");
            return OrderResult.Success(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            return OrderResult.Failed("An error occurred while processing your order");
        }
    }
}
```

## Configuration and Options Pattern

### Strongly Typed Configuration
```csharp
// appsettings.json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Username": "your-email@gmail.com",
    "Password": "your-password",
    "EnableSsl": true
  },
  "PaymentSettings": {
    "ApiKey": "your-api-key",
    "BaseUrl": "https://api.payment-provider.com",
    "TimeoutSeconds": 30
  }
}
```

```csharp
// Configuration classes
public class EmailSettings
{
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; }
}

public class PaymentSettings
{
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; }
}
```

```csharp
// Register configuration
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));
builder.Services.Configure<PaymentSettings>(
    builder.Configuration.GetSection("PaymentSettings"));
```

```csharp
// Use in services
public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // Use _emailSettings.SmtpServer, _emailSettings.Port, etc.
    }
}
```

## Advanced DI Patterns

### Factory Pattern
```csharp
public interface INotificationFactory
{
    INotificationService CreateNotificationService(NotificationType type);
}

public class NotificationFactory : INotificationFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public INotificationService CreateNotificationService(NotificationType type)
    {
        return type switch
        {
            NotificationType.Email => _serviceProvider.GetRequiredService<IEmailNotificationService>(),
            NotificationType.Sms => _serviceProvider.GetRequiredService<ISmsNotificationService>(),
            NotificationType.Push => _serviceProvider.GetRequiredService<IPushNotificationService>(),
            _ => throw new ArgumentException($"Unknown notification type: {type}")
        };
    }
}
```

### Decorator Pattern
```csharp
public interface IOrderService
{
    Task<OrderResult> CreateOrderAsync(CreateOrderRequest request);
}

public class OrderService : IOrderService
{
    // Base implementation
}

public class CachedOrderService : IOrderService
{
    private readonly IOrderService _orderService;
    private readonly ICacheService _cacheService;

    public CachedOrderService(IOrderService orderService, ICacheService cacheService)
    {
        _orderService = orderService;
        _cacheService = cacheService;
    }

    public async Task<OrderResult> CreateOrderAsync(CreateOrderRequest request)
    {
        // Add caching logic
        var result = await _orderService.CreateOrderAsync(request);
        // Cache the result
        return result;
    }
}

// Registration
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderService, CachedOrderService>(provider =>
    new CachedOrderService(
        provider.GetRequiredService<OrderService>(),
        provider.GetRequiredService<ICacheService>()));
```

## Testing with Dependency Injection

### Unit Testing with Mocks
```csharp
[Test]
public async Task CreateOrder_ValidRequest_ReturnsSuccess()
{
    // Arrange
    var mockRepository = new Mock<IOrderRepository>();
    var mockEmailService = new Mock<IEmailService>();
    var mockPaymentService = new Mock<IPaymentService>();
    var mockLogger = new Mock<ILogger<OrderService>>();

    mockPaymentService
        .Setup(x => x.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
        .ReturnsAsync(new PaymentResult { Success = true, TransactionId = "123" });

    var orderService = new OrderService(
        mockRepository.Object,
        mockEmailService.Object,
        mockPaymentService.Object,
        mockLogger.Object);

    var request = new CreateOrderRequest
    {
        CustomerId = 1,
        CustomerEmail = "test@example.com",
        Items = new List<OrderItem>(),
        Total = 100m
    };

    // Act
    var result = await orderService.CreateOrderAsync(request);

    // Assert
    Assert.True(result.Success);
    mockRepository.Verify(x => x.AddAsync(It.IsAny<Order>()), Times.Once);
    mockEmailService.Verify(x => x.SendConfirmationAsync("test@example.com"), Times.Once);
}
```

### Integration Testing
```csharp
public class OrderServiceIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public OrderServiceIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Test]
    public async Task CreateOrder_WithRealServices_Success()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

        // Act & Assert
        var request = new CreateOrderRequest { /* ... */ };
        var result = await orderService.CreateOrderAsync(request);
        
        Assert.True(result.Success);
    }
}
```

## Best Practices

### 1. Use Interfaces
```csharp
// Good: Interface-based design
public interface IUserService
{
    Task<User> GetUserAsync(int id);
}

public class UserService : IUserService
{
    public async Task<User> GetUserAsync(int id)
    {
        // Implementation
    }
}

// Registration
builder.Services.AddScoped<IUserService, UserService>();
```

### 2. Avoid Service Locator Anti-Pattern
```csharp
// Bad: Service Locator (anti-pattern)
public class OrderController : Controller
{
    public IActionResult Create()
    {
        var orderService = HttpContext.RequestServices.GetService<IOrderService>();
        // Use orderService
    }
}

// Good: Constructor Injection
public class OrderController : Controller
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public IActionResult Create()
    {
        // Use _orderService
    }
}
```

### 3. Keep Constructors Simple
```csharp
// Good: Simple constructor
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
}
```

### 4. Use Appropriate Lifetimes
```csharp
// Database contexts - Scoped
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(connectionString));

// Business services - Scoped
builder.Services.AddScoped<IOrderService, OrderService>();

// Configuration - Singleton
builder.Services.AddSingleton<IConfiguration>(configuration);

// Lightweight services - Transient
builder.Services.AddTransient<IEmailValidator, EmailValidator>();
```

## Common Pitfalls

### 1. Circular Dependencies
```csharp
// Bad: Circular dependency
public class ServiceA
{
    public ServiceA(ServiceB serviceB) { }
}

public class ServiceB
{
    public ServiceB(ServiceA serviceA) { }
}

// Solution: Introduce an interface or refactor
public interface IServiceC
{
    void DoSomething();
}

public class ServiceA
{
    public ServiceA(IServiceC serviceC) { }
}

public class ServiceB : IServiceC
{
    public void DoSomething() { }
}
```

### 2. Captive Dependencies
```csharp
// Bad: Singleton depending on Scoped service
builder.Services.AddSingleton<SingletonService>(); // Lives for app lifetime
builder.Services.AddScoped<ScopedService>();       // Lives for request

public class SingletonService
{
    public SingletonService(ScopedService scopedService) // Problem!
    {
        // ScopedService will be captured and live longer than intended
    }
}
```

## Key Takeaways

- **Dependency Injection promotes loose coupling** and makes code more testable
- **Use appropriate service lifetimes**: Transient for stateless, Scoped for per-request, Singleton for app-wide
- **Design with interfaces** to enable easy testing and implementation swapping
- **Keep constructors simple** and avoid complex logic in them
- **Use the Options pattern** for strongly-typed configuration
- **Avoid service locator anti-pattern** - prefer constructor injection
- **Be aware of common pitfalls** like circular dependencies and captive dependencies

Dependency Injection is fundamental to building maintainable ASP.NET Core applications. Master these concepts to create robust, testable, and flexible applications!