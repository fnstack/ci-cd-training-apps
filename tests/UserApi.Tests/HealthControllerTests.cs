using FluentAssertions;
using System.Net;
using UserApi.Tests.Infrastructure;

namespace UserApi.Tests;

public class HealthControllerTests : IClassFixture<IntegrationTestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HealthControllerTests(IntegrationTestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Health_ShouldReturnOk()
    {
        // Act
        var response = await _client.GetAsync("/health");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("ok");
    }

    [Fact]
    public async Task Root_ShouldReturnWelcomeMessage()
    {
        // Act
        var response = await _client.GetAsync("/");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadAsStringAsync();
        result.Should().Contain("Hello from .NET API!");
    }
}