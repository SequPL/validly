namespace Validly.Extensions.AspNetCore.Example.Domain.Users.Entities;

public class User
{
	public Guid Id { get; private set; }
	public string Username { get; private set; } = string.Empty;
	public string Password { get; private set; } = string.Empty;
	public string Email { get; private set; } = string.Empty;
	public int Age { get; private set; }
	public string? FirstName { get; private set; }
	public string? LastName { get; private set; }
}
