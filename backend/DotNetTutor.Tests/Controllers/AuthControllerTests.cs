using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using DotNetTutor.Api.Controllers;
using DotNetTutor.Api.Models;
using DotNetTutor.Api.Services;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace DotNetTutor.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<SignInManager<User>> _mockSignInManager;
    private readonly Mock<JwtService> _mockJwtService;
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        // Setup UserManager mock
        var userStore = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(
            userStore.Object, null, null, null, null, null, null, null, null);

        // Setup SignInManager mock
        var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        _mockSignInManager = new Mock<SignInManager<User>>(
            _mockUserManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

        // Setup JwtService mock
        var mockConfig = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
        _mockJwtService = new Mock<JwtService>(mockConfig.Object);

        _controller = new AuthController(_mockUserManager.Object, _mockSignInManager.Object, _mockJwtService.Object);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe"
        };

        var user = new User
        {
            Id = "123",
            UserName = registerDto.Email,
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName
        };

        var identityResult = IdentityResult.Success;
        var expectedToken = "fake-jwt-token";
        var expectedExpiry = DateTime.UtcNow.AddHours(1);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(identityResult);
        _mockJwtService.Setup(x => x.GenerateToken(It.IsAny<User>()))
            .Returns(expectedToken);
        _mockJwtService.Setup(x => x.GetTokenExpiry())
            .Returns(expectedExpiry);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as AuthResponseDto;
        
        response.Should().NotBeNull();
        response!.Token.Should().Be(expectedToken);
        response.Email.Should().Be(registerDto.Email);
        response.FirstName.Should().Be(registerDto.FirstName);
        response.LastName.Should().Be(registerDto.LastName);
        response.ExpiresAt.Should().Be(expectedExpiry);
    }

    [Fact]
    public async Task Register_WithInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto { Email = "invalid-email" };
        _controller.ModelState.AddModelError("Password", "Password is required");

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Register_WithFailedUserCreation_ReturnsBadRequest()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "weak",
            FirstName = "John",
            LastName = "Doe"
        };

        var identityError = new IdentityError { Description = "Password too weak" };
        var identityResult = IdentityResult.Failed(identityError);

        _mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), registerDto.Password))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOkResult()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "Password123!"
        };

        var user = new User
        {
            Id = "123",
            Email = loginDto.Email,
            UserName = loginDto.Email,
            FirstName = "John",
            LastName = "Doe"
        };

        var expectedToken = "fake-jwt-token";
        var expectedExpiry = DateTime.UtcNow.AddHours(1);

        _mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);
        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
            .ReturnsAsync(IdentitySignInResult.Success);
        _mockJwtService.Setup(x => x.GenerateToken(user))
            .Returns(expectedToken);
        _mockJwtService.Setup(x => x.GetTokenExpiry())
            .Returns(expectedExpiry);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as AuthResponseDto;
        
        response.Should().NotBeNull();
        response!.Token.Should().Be(expectedToken);
        response.Email.Should().Be(loginDto.Email);
        response.FirstName.Should().Be(user.FirstName);
        response.LastName.Should().Be(user.LastName);
        response.ExpiresAt.Should().Be(expectedExpiry);
    }

    [Fact]
    public async Task Login_WithInvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        var loginDto = new LoginDto { Email = "invalid-email" };
        _controller.ModelState.AddModelError("Password", "Password is required");

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "nonexistent@example.com",
            Password = "Password123!"
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid email or password");
    }

    [Fact]
    public async Task Login_WithInvalidPassword_ReturnsUnauthorized()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            Email = "test@example.com",
            Password = "WrongPassword"
        };

        var user = new User
        {
            Id = "123",
            Email = loginDto.Email,
            UserName = loginDto.Email
        };

        _mockUserManager.Setup(x => x.FindByEmailAsync(loginDto.Email))
            .ReturnsAsync(user);
        _mockSignInManager.Setup(x => x.CheckPasswordSignInAsync(user, loginDto.Password, false))
            .ReturnsAsync(IdentitySignInResult.Failed);

        // Act
        var result = await _controller.Login(loginDto);

        // Assert
        result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid email or password");
    }

    [Fact]
    public async Task Logout_ReturnsOkResult()
    {
        // Arrange
        _mockSignInManager.Setup(x => x.SignOutAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Logout();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        
        // Verify the response contains the expected message
        var responseValue = okResult!.Value;
        responseValue.Should().NotBeNull();
        
        // Use reflection to check the anonymous object
        var messageProperty = responseValue!.GetType().GetProperty("message");
        messageProperty.Should().NotBeNull();
        messageProperty!.GetValue(responseValue).Should().Be("Logged out successfully");
        
        _mockSignInManager.Verify(x => x.SignOutAsync(), Times.Once);
    }
}