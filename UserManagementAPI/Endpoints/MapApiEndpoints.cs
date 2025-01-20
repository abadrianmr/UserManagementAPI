using UserManagementAPI.DTOs;
using UserManagementAPI.Interfaces;
using UserManagementAPI.Models;

namespace UserManagementAPI.Endpoints;

public static class MapApiEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/users", (IUserRepository repo) =>
        {
            var users = repo.GetAllUsers();
            return Results.Ok(users);
        });
        endpoints.MapGet("/users/{id:int}", (int id, IUserRepository repo) =>
        {
            var user = repo.GetUserById(id);
            return user is not null
                ? Results.Ok(user)
                : Results.NotFound(new { Message = $"User with ID {id} not found." });
        });
        endpoints.MapPost("/users", (CreateUserDto userDto, IUserRepository repo) =>
        {
            repo.CreateUser(userDto.Name, userDto.Email);
            return Results.Created($"/users/{userDto.Name}", userDto);
        });
        endpoints.MapPut("/users/{id:int}", (int id, UpdateUserDto userDto, IUserRepository repo) =>
        {
            var user = repo.GetUserById(id);
            if (user == null) return Results.NotFound(new { Message = $"User with ID {id} not found." });
            var updatedUser = new User(id, userDto.Name, userDto.Email);
            repo.UpdateUser(updatedUser);
            return Results.Ok();
        });
        endpoints.MapDelete("/users/{id:int}", (int id, IUserRepository repo) =>
        {
            var user = repo.GetUserById(id);
            if (user == null) return Results.NotFound(new { Message = $"User with ID {id} not found." });
            repo.DeleteUser(id);
            return Results.Ok();
        });
    }
}