using UserManagementAPI.Models;

namespace UserManagementAPI.Interfaces;

internal interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void CreateUser(string name, string email);
    void UpdateUser(User updatedUser);
    void DeleteUser(int id);
}