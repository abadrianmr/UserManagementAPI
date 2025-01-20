var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/users", (IUserRepository repo) => repo.GetAllUsers());
app.MapGet("/users/{id:int}", (int id, IUserRepository repo) =>
{
    var user = repo.GetUserById(id);
    return user is not null ? 
        Results.Ok(user) : 
        Results.NotFound(new { Message = $"User with ID {id} not found." });
});
app.MapPost("/users", (CreateUserDto userDto, IUserRepository repo) =>
{
    repo.CreateUser(userDto.Name, userDto.Email);
    return Results.Created($"/users/{userDto.Name}", userDto);
});
app.MapPut("/users/{id:int}", (int id, UpdateUserDto userDto, IUserRepository repo) =>
{
    var user = repo.GetUserById(id);
    if (user == null)
    {
        return Results.NotFound(new { Message = $"User with ID {id} not found." });
    }
    var updatedUser = new User(id, userDto.Name, userDto.Email);
    repo.UpdateUser(updatedUser);
    return Results.Ok();
});
app.MapDelete("/users/{id:int}", (int id, IUserRepository repo) =>
{
    var user = repo.GetUserById(id);
    if (user == null)
    {
        return Results.NotFound(new { Message = $"User with ID {id} not found." });
    }
    repo.DeleteUser(id);
    return Results.Ok();
});

app.Run();

// User model
public record User(int Id, string Name, string Email);

// CreateUserDto model
public record CreateUserDto(string Name, string Email);
public record UpdateUserDto(string Name, string Email);

// User repository interface
internal interface IUserRepository
{
    IEnumerable<User> GetAllUsers();
    User? GetUserById(int id);
    void CreateUser(string name, string email);
    void UpdateUser(User updatedUser);
    void DeleteUser(int id);
}

// User repository implementation
public class UserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public IEnumerable<User> GetAllUsers() => _users;

    public User? GetUserById(int id) => _users.Find(user => user.Id == id);

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
        if (user != null)
        {
            _users.Remove(user);
        }
    }
}