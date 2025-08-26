namespace UserApi.Api.Models;

public record User
{
    public int Id { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}

public record CreateUserRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
}