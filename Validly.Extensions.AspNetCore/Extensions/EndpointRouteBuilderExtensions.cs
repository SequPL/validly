using Microsoft.AspNetCore.Http;
using Validly.Extensions.AspNetCore;

namespace Microsoft.AspNetCore.Builder
{
	/// <summary>
	/// RouteHandlerBuilder extensions
	/// </summary>
	public static class EndpointRouteBuilderExtensions
	{
		/// <summary>
		/// Add Validly validation to the specified <see cref="RouteHandlerBuilder"/>
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static RouteHandlerBuilder WithValidlyValidation(this RouteHandlerBuilder builder)
		{
			return builder.AddEndpointFilter<ValidlyValidationFilter>();
		}
	}
}

namespace Microsoft.AspNetCore.Routing
{
	/// <summary>
	/// RouteGroupBuilder extensions
	/// </summary>
	public static class EndpointRouteBuilderExtensions
	{
		/// <summary>
		/// Add Validly validation to the specified <see cref="RouteGroupBuilder"/>
		/// </summary>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static RouteGroupBuilder WithValidlyValidation(this RouteGroupBuilder builder)
		{
			return builder.AddEndpointFilter<ValidlyValidationFilter>();
		}
	}
}
