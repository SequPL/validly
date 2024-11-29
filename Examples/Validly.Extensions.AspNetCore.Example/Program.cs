using FluentValidation;
using SharpGrip.FluentValidation.AutoValidation.Endpoints.Extensions;
using Validly.Extensions.AspNetCore.Example.Users;
using Validly.Extensions.AspNetCore.Example.Users.Dtos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestFluentValidationValidator>();
builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ENDPOINTS
app.AddUsersEndpoints();

app.MapPost("/fluent-validation", (CreateUserRequest request) => Results.Ok()).AddFluentValidationAutoValidation();

app.Run();
