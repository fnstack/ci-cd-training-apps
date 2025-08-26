using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using UserApi.Api.Models;
using UserApi.Tests.Infrastructure;

namespace UserApi.Tests.Controllers;

public class UsersControllerTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public UsersControllerTests(IntegrationTestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUsers_ShouldReturnUsersList()
    {
        // Act
        var response = await _client.GetAsync("/api/users");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadFromJsonAsync<object>();
        content.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUser_WithValidId_ShouldReturnUser()
    {
        // Act
        var response = await _client.GetAsync("/api/users/1");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("Alice");
    }

    [Fact]
    public async Task GetUser_WithInvalidId_ShouldReturn404()
    {
        // Act
        var response = await _client.GetAsync("/api/users/999");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateUser_WithValidData_ShouldCreateUser()
    {
        // Arrange
        var newUser = new CreateUserRequest 
        { 
            Name = "Test User", 
            Email = "test@example.com" 
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", newUser);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("Test User");
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var newUser = new CreateUserRequest 
        { 
            Name = "Test User", 
            Email = "invalid-email" 
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/users", newUser);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}