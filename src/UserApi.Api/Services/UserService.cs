using UserApi.Api.Models;

namespace UserApi.Api.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(CreateUserRequest request);
    Task<User?> UpdateUserAsync(int id, CreateUserRequest request);
    Task<bool> DeleteUserAsync(int id);
}

public class UserService : IUserService
{
    private readonly List<User> _users = new()
    {
        new User { Id = 1, Name = "Alice", Email = "alice@example.com", CreatedAt = DateTime.UtcNow.AddDays(-1) },
        new User { Id = 2, Name = "Bob", Email = "bob@example.com", CreatedAt = DateTime.UtcNow.AddDays(-1) }
    };

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return Task.FromResult<IEnumerable<User>>(_users);
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User> CreateUserAsync(CreateUserRequest request)
    {
        var existingUser = _users.FirstOrDefault(u => u.Email == request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var user = new User
        {
            Id = _users.Count > 0 ? _users.Max(u => u.Id) + 1 : 1,
            Name = request.Name,
            Email = request.Email
        };

        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User?> UpdateUserAsync(int id, CreateUserRequest request)
    {
        var userIndex = _users.FindIndex(u => u.Id == id);
        if (userIndex == -1)
        {
            return Task.FromResult<User?>(null);
        }

        var existingUser = _users.FirstOrDefault(u => u.Email == request.Email && u.Id != id);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var updatedUser = _users[userIndex] with 
        { 
            Name = request.Name,
            Email = request.Email
        };
        
        _users[userIndex] = updatedUser;
        return Task.FromResult<User?>(updatedUser);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        var userIndex = _users.FindIndex(u => u.Id == id);
        if (userIndex == -1)
        {
            return Task.FromResult(false);
        }

        _users.RemoveAt(userIndex);
        return Task.FromResult(true);
    }
}