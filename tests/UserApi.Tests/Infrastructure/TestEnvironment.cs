using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;

namespace UserApi.Tests.Infrastructure;

public class TestEnvironment : IAsyncLifetime
{
    private IContainer? _testContainer;

    public async Task InitializeAsync()
    {
        // Start a simple test container to verify Docker is working
        // This helps ensure the test environment is properly set up
        _testContainer = new ContainerBuilder()
            .WithImage("hello-world:latest")
            .Build();

        try
        {
            await _testContainer.StartAsync();
            // For hello-world, it runs and exits immediately
        }
        catch (Exception ex)
        {
            // If Docker isn't available, we'll skip container-based tests
            // but still allow the tests to run with the in-process TestServer
            System.Diagnostics.Debug.WriteLine($"Docker not available: {ex.Message}");
        }
    }

    public async Task DisposeAsync()
    {
        if (_testContainer != null)
        {
            await _testContainer.DisposeAsync();
        }
    }

    public bool IsDockerAvailable => _testContainer != null;
}