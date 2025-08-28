using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DotNetTutor.Api.Services;
using DotNetTutor.Api.Models;

namespace DotNetTutor.Tests.Services;

public class JwtServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly Mock<IConfigurationSection> _mockJwtSection;
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _mockJwtSection = new Mock<IConfigurationSection>();
        
        // Setup default JWT configuration values
        _mockJwtSection.Setup(x => x["SecretKey"]).Returns("this-is-a-very-long-secret-key-for-testing-purposes-12345");
        _mockJwtSection.Setup(x => x["Issuer"]).Returns("TestIssuer");
        _mockJwtSection.Setup(x => x["Audience"]).Returns("TestAudience");
        _mockJwtSection.Setup(x => x["ExpiryMinutes"]).Returns("30");
        
        _mockConfiguration.Setup(x => x.GetSection("JwtSettings")).Returns(_mockJwtSection.Object);
        
        _jwtService = new JwtService(_mockConfiguration.Object);
    }

    [Fact]
    public void GenerateToken_WithValidUser_ReturnsValidJwtToken()
    {
        // Arrange
        var user = new User
        {
            Id = "123",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var token = _jwtService.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        // Validate JWT structure
        var tokenHandler = new JwtSecurityTokenHandler();
        tokenHandler.CanReadToken(token).Should().BeTrue();
        
        var jsonToken = tokenHandler.ReadJwtToken(token);
        
        // Verify claims
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
        jsonToken.Claims.Should().Contain(c => c.Type == "firstName" && c.Value == user.FirstName);
        jsonToken.Claims.Should().Contain(c => c.Type == "lastName" && c.Value == user.LastName);
        
        // Verify issuer and audience
        jsonToken.Issuer.Should().Be("TestIssuer");
        jsonToken.Audiences.Should().Contain("TestAudience");
        
        // Verify expiration
        jsonToken.ValidTo.Should().BeAfter(DateTime.UtcNow.AddMinutes(25));
        jsonToken.ValidTo.Should().BeBefore(DateTime.UtcNow.AddMinutes(35));
    }

    [Fact]
    public void GenerateToken_WithNullEmailUser_HandlesGracefully()
    {
        // Arrange
        var user = new User
        {
            Id = "123",
            Email = null,
            UserName = null,
            FirstName = null,
            LastName = null
        };

        // Act
        var token = _jwtService.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);
        
        // Verify claims with empty values
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id);
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "");
        jsonToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "");
        jsonToken.Claims.Should().Contain(c => c.Type == "firstName" && c.Value == "");
        jsonToken.Claims.Should().Contain(c => c.Type == "lastName" && c.Value == "");
    }

    [Fact]
    public void GenerateToken_WithDefaultSettings_UsesDefaultValues()
    {
        // Arrange
        var mockJwtSectionWithDefaults = new Mock<IConfigurationSection>();
        mockJwtSectionWithDefaults.Setup(x => x["SecretKey"]).Returns((string?)null);
        mockJwtSectionWithDefaults.Setup(x => x["Issuer"]).Returns((string?)null);
        mockJwtSectionWithDefaults.Setup(x => x["Audience"]).Returns((string?)null);
        mockJwtSectionWithDefaults.Setup(x => x["ExpiryMinutes"]).Returns((string?)null);
        
        var mockConfigWithDefaults = new Mock<IConfiguration>();
        mockConfigWithDefaults.Setup(x => x.GetSection("JwtSettings")).Returns(mockJwtSectionWithDefaults.Object);
        
        var jwtServiceWithDefaults = new JwtService(mockConfigWithDefaults.Object);
        
        var user = new User
        {
            Id = "123",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe"
        };

        // Act
        var token = jwtServiceWithDefaults.GenerateToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jsonToken = tokenHandler.ReadJwtToken(token);
        
        // Verify default values are used
        jsonToken.Issuer.Should().Be("DotNetTutor");
        jsonToken.Audiences.Should().Contain("DotNetTutorUsers");
        
        // Default expiry is 60 minutes
        jsonToken.ValidTo.Should().BeAfter(DateTime.UtcNow.AddMinutes(55));
        jsonToken.ValidTo.Should().BeBefore(DateTime.UtcNow.AddMinutes(65));
    }

    [Fact]
    public void GetTokenExpiry_ReturnsCorrectExpiryTime()
    {
        // Arrange
        var beforeCall = DateTime.UtcNow;

        // Act
        var expiry = _jwtService.GetTokenExpiry();

        // Assert
        var afterCall = DateTime.UtcNow;
        
        expiry.Should().BeAfter(beforeCall.AddMinutes(25));
        expiry.Should().BeBefore(afterCall.AddMinutes(35));
    }

    [Fact]
    public void GetTokenExpiry_WithCustomExpiryMinutes_ReturnsCorrectTime()
    {
        // Arrange
        _mockJwtSection.Setup(x => x["ExpiryMinutes"]).Returns("120");
        
        var beforeCall = DateTime.UtcNow;

        // Act
        var expiry = _jwtService.GetTokenExpiry();

        // Assert
        var afterCall = DateTime.UtcNow;
        
        expiry.Should().BeAfter(beforeCall.AddMinutes(115));
        expiry.Should().BeBefore(afterCall.AddMinutes(125));
    }

    [Fact]
    public void GetTokenExpiry_WithDefaultExpiryMinutes_ReturnsOneHour()
    {
        // Arrange
        _mockJwtSection.Setup(x => x["ExpiryMinutes"]).Returns((string?)null);
        
        var beforeCall = DateTime.UtcNow;

        // Act
        var expiry = _jwtService.GetTokenExpiry();

        // Assert
        var afterCall = DateTime.UtcNow;
        
        // Default is 60 minutes
        expiry.Should().BeAfter(beforeCall.AddMinutes(55));
        expiry.Should().BeBefore(afterCall.AddMinutes(65));
    }
}