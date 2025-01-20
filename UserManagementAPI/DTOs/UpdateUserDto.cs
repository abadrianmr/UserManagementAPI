namespace UserManagementAPI.DTOs;

public record UpdateUserDto(string Name, string Email)
{
    public string Name { get; init; } =
        !string.IsNullOrWhiteSpace(Name) ? Name : throw new ArgumentException("Name cannot be empty");

    public string Email { get; init; } =
        Email.Contains("@") ? Email : throw new ArgumentException("Invalid email format");
}