using Validly.Extensions.AspNetCore.Example.Domain.Users.Entities;
using Validly.Extensions.AspNetCore.Example.Users.Dtos;

namespace Validly.Extensions.AspNetCore.Example.Users;

public static class UserEndpoints
{
	private static readonly List<User> Users = new();

	public static WebApplication AddUsersEndpoints(this WebApplication app)
	{
		var group = app.MapGroup("/api/users").WithValidlyValidation();

		group.MapGet("", () => Task.FromResult(Results.Ok(Users)));

		group
			.MapGet(
				"{id:guid}",
				(Guid id) =>
				{
					var user = Users.Find(user => user.Id == id);

					if (user is null)
					{
						return Results.NotFound();
					}

					return Results.Ok(user);
				}
			)
			.WithOpenApi()
			.AddEndpointFilter<ValidlyValidationFilter>();

		group.MapPost(
			"",
			(CreateUserRequest request) =>
			{
				TypedResults.Created();
				// var user = User.Create(request.ToCreateArgs());
				// return Results.Created($"/users/{user.Id}", user);
			}
		);

		// group.MapPut(
		// 	"{id:guid}",
		// 	(Guid id, PutUserRequest request) =>
		// 	{
		// 		var existingUserId = Users.FindIndex(user => user.Id == id);
		//
		// 		if (existingUserId == -1)
		// 		{
		// 			return Results.NotFound();
		// 		}
		//
		// 		Users[existingUserId] = request.ToUser();
		// 		return Results.NoContent();
		// 	}
		// );

		group.MapDelete(
			"{id:guid}",
			(Guid id) =>
			{
				Users.RemoveAll(user => user.Id == id);
				return Results.NoContent();
			}
		);

		return app;
	}
}
