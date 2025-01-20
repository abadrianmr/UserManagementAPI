using UserManagementAPI.Endpoints;
using UserManagementAPI.Interfaces;
using UserManagementAPI.Middlewares;
using UserManagementAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddOpenApi();
builder.Services.AddLogging();

var app = builder.Build();

app.UseCustomExceptionHandler();
app.UseCustomAuthorization();
app.UseCustomLogging();

// Use the logging middleware


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapEndpoints();

app.Run();