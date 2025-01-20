using UserManagementAPI.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAllUsers()
    {
        return _users.AsReadOnly();
    }

    public User? GetUserById(int id)
    {
        return _users.Find(user => user.Id == id);
    }

    public void CreateUser(string name, string email)
    {
        var newId = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        var newUser = new User(newId, name, email);
        _users.Add(newUser);
    }

    public void UpdateUser(User updatedUser)
    {
        var user = GetUserById(updatedUser.Id);
        if (user == null) return;
        user = user with { Name = updatedUser.Name, Email = updatedUser.Email };
        _users[_users.FindIndex(u => u.Id == updatedUser.Id)] = user;
    }

    public void DeleteUser(int id)
    {
        var user = GetUserById(id);
        if (user != null) _users.Remove(user);
    }
}