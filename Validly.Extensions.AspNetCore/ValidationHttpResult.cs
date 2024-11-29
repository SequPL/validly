using Microsoft.AspNetCore.Http;

namespace Validly.Extensions.AspNetCore;

/// <summary>
/// HTTP result for Validly ValidationResult
/// </summary>
public class ValidationHttpResult : IResult
{
	private readonly ValidationResult _validationResult;

	/// <param name="validationResult"></param>
	public ValidationHttpResult(ValidationResult validationResult)
	{
		_validationResult = validationResult;
	}

	/// <inheritdoc />
	public async Task ExecuteAsync(HttpContext httpContext)
	{
		httpContext.Response.ContentType = "application/problem+json";
		httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

		try
		{
			await httpContext.Response.WriteAsync(
				"""
				{
				  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
				  "title": "One or more validation errors occurred.",
				  "status": 400,
				  "errors": [

				"""
			);

			for (int propIndex = _validationResult.Properties.Count - 1; propIndex >= 0; propIndex--)
			{
				PropertyValidationResult prop = _validationResult.Properties[propIndex];

				for (int errorIndex = prop.Messages.Count - 1; errorIndex >= 0; errorIndex--)
				{
					ValidationMessage error = prop.Messages[errorIndex];

					// TODO: This allocates memory; should be optimized if possible
					await httpContext.Response.WriteAsync(
						$$"""
						    {
						      "detail": "{{error.Message}}",
						      "resourceKey": "{{error.ResourceKey}}",
						      "args": [{{error.ArgsJson}}],
						      "pointer": "#{{prop.PropertyPath}}",
						      "fieldName": "{{prop.PropertyDisplayName}}"
						    }{{(propIndex == 0 && errorIndex == 0 ? string.Empty : ",")}}
						"""
					);
				}
			}

			await httpContext.Response.WriteAsync("\r\n  ]\r\n}");
		}
		finally
		{
			_validationResult.Dispose();
		}
	}
}
