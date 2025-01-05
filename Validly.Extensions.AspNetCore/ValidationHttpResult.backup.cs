// using System.Buffers;
// using System.IO.Pipelines;
// using System.Text;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Http.Features;
//
// namespace Validly.Extensions.AspNetCore;
//
// /// <summary>
// /// HTTP result for Validly ValidationResult
// /// </summary>
// public class ValidationHttpResult : IResult
// {
// 	// private static ResultParts Parts => default;
//
// 	private readonly ValidationResult _validationResult;
//
// 	public ValidationHttpResult(ValidationResult validationResult)
// 	{
// 		_validationResult = validationResult;
// 	}
//
// 	public async Task ExecuteAsync(HttpContext httpContext)
// 	{
// 		httpContext.Response.ContentType = "application/problem+json";
// 		httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
//
// 		try
// 		{
// 			var syncIoFeature = httpContext.Features.Get<IHttpBodyControlFeature>();
//
// 			if (syncIoFeature is not null)
// 			{
// 				syncIoFeature.AllowSynchronousIO = true;
// 				await WriteSync(httpContext.Response);
// 			}
// 			else
// 			{
// 				// await WriteAsync();
// 			}
// 		}
// 		finally
// 		{
// 			_validationResult.Dispose();
// 		}
// 	}
//
// 	// private async Task WriteSync(HttpResponse response)
// 	// {
// 	// 	response.Body.Write(Parts.BodyStartBytes);
// 	// 	// await response.Body.WriteAsync(ResultParts.BodyStartBytes);
// 	//
// 	// 	for (int propIndex = _validationResult.Properties.Count - 1; propIndex >= 0; propIndex--)
// 	// 	{
// 	// 		PropertyValidationResult prop = _validationResult.Properties[propIndex];
// 	//
// 	// 		for (int errorIndex = prop.Messages.Count - 1; errorIndex >= 0; errorIndex--)
// 	// 		{
// 	// 			ValidationMessage error = prop.Messages[errorIndex];
// 	//
// 	// 			response.Body.Write(Parts.ErrorPart1);
// 	// 			response.Body.Write(Encoding.UTF8.GetBytes(error.Message));
// 	// 			response.Body.Write(Parts.ErrorPart2);
// 	// 			response.Body.Write(Encoding.UTF8.GetBytes(error.ResourceKey));
// 	// 			response.Body.Write(Parts.ErrorPart3);
// 	// 			response.Body.Write(Encoding.UTF8.GetBytes(error.ArgsJson));
// 	// 			response.Body.Write(Parts.ErrorPart4);
// 	// 			response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyPath));
// 	// 			response.Body.Write(Parts.ErrorPart5);
// 	// 			response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyDisplayName));
// 	// 			response.Body.Write(Parts.ErrorPart6);
// 	// 			response.Body.Write(propIndex == 0 && errorIndex == 0 ? ReadOnlySpan<byte>.Empty : Parts.Comma);
// 	//
// 	// 			// response.Body.Write(ResultParts.ErrorPart1);
// 	// 			// response.Body.Write(Encoding.UTF8.GetBytes(error.Message));
// 	// 			// response.Body.Write(ResultParts.ErrorPart2);
// 	// 			// response.Body.Write(Encoding.UTF8.GetBytes(error.ResourceKey));
// 	// 			// response.Body.Write(ResultParts.ErrorPart3);
// 	// 			// response.Body.Write(Encoding.UTF8.GetBytes(error.ArgsJson));
// 	// 			// response.Body.Write(ResultParts.ErrorPart4);
// 	// 			// response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyPath));
// 	// 			// response.Body.Write(ResultParts.ErrorPart5);
// 	// 			// response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyDisplayName));
// 	// 			// response.Body.Write(ResultParts.ErrorPart6);
// 	// 			// response.Body.Write(propIndex == 0 && errorIndex == 0 ? Array.Empty<byte>() : ResultParts.Comma);
// 	//
// 	// 			// 				await response.WriteAsync(
// 	// 			// 					$$"""
// 	// 			// 					    {
// 	// 			// 					      "detail": "{{error.Message}}",
// 	// 			// 					      "resourceKey": "{{error.ResourceKey}}",
// 	// 			// 					      "args": [{{error.ArgsJson}}],
// 	// 			// 					      "pointer": "#{{prop.PropertyPath}}",
// 	// 			// 					      "fieldName": "{{prop.PropertyDisplayName}}"
// 	// 			// 					    }{{(propIndex == 0 && errorIndex == 0 ? string.Empty : ",")}}
// 	// 			// 					"""
// 	// 			// 				);
// 	// 		}
// 	// 	}
// 	//
// 	// 	response.Body.Write(Parts.BodyEndBytes);
// 	// 	// await response.Body.WriteAsync(ResultParts.BodyEndBytes);
// 	// }
//
// 	private async Task WriteSync(HttpResponse response)
// 	{
// 		await response.WriteAsync(
// 			"""
// 			{
// 			  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
// 			  "title": "One or more validation errors occurred.",
// 			  "status": 400,
// 			  "errors": [
//
// 			"""
// 		);
// 		// await response.Body.WriteAsync(ResultParts.BodyStartBytes);
//
// 		for (int propIndex = _validationResult.Properties.Count - 1; propIndex >= 0; propIndex--)
// 		{
// 			PropertyValidationResult prop = _validationResult.Properties[propIndex];
//
// 			for (int errorIndex = prop.Messages.Count - 1; errorIndex >= 0; errorIndex--)
// 			{
// 				ValidationMessage error = prop.Messages[errorIndex];
//
// 				// response.Body.Write("    {\r\n      \"detail\": \""u8);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(error.Message));
// 				// response.Body.Write("\",\r\n      \"resourceKey\": \""u8);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(error.ResourceKey));
// 				// response.Body.Write("\",\r\n      \"args\": ["u8);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(error.ArgsJson));
// 				// response.Body.Write("],\r\n      \"pointer\": \""u8);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyPath));
// 				// response.Body.Write("\",\r\n      \"fieldName\": \""u8);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyDisplayName));
// 				// response.Body.Write("\"\r\n    }\r\n"u8);
// 				// response.Body.Write(propIndex == 0 && errorIndex == 0 ? ReadOnlySpan<byte>.Empty : ","u8);
//
// 				// await response.Body.WriteAsync(ResultParts.ErrorPart1);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(error.Message));
// 				// await response.Body.WriteAsync(ResultParts.ErrorPart2);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(error.ResourceKey));
// 				// await response.Body.WriteAsync(ResultParts.ErrorPart3);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(error.ArgsJson));
// 				// await response.Body.WriteAsync(ResultParts.ErrorPart4);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyPath));
// 				// await response.Body.WriteAsync(ResultParts.ErrorPart5);
// 				// response.Body.Write(Encoding.UTF8.GetBytes(prop.PropertyDisplayName));
// 				// await response.Body.WriteAsync(ResultParts.ErrorPart6);
// 				// await response.Body.WriteAsync(
// 				// 	propIndex == 0 && errorIndex == 0 ? Array.Empty<byte>() : ResultParts.Comma
// 				// );
//
// 				await response.WriteAsync(
// 					$$"""
// 					    {
// 					      "detail": "{{error.Message}}",
// 					      "resourceKey": "{{error.ResourceKey}}",
// 					      "args": [{{error.ArgsJson}}],
// 					      "pointer": "#{{prop.PropertyPath}}",
// 					      "fieldName": "{{prop.PropertyDisplayName}}"
// 					    }{{(propIndex == 0 && errorIndex == 0 ? string.Empty : ",")}}
// 					"""
// 				);
// 			}
// 		}
//
// 		// response.Body.Write("  ]\r\n}"u8);
// 		await response.Body.WriteAsync(ResultParts.BodyEndBytes);
// 	}
// }
//
// // static class ResultParts
// // {
// // 	public static readonly byte[] BodyStartBytes =
// // 		"""
// // 		{
// // 		  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
// // 		  "title": "One or more validation errors occurred.",
// // 		  "status": 400,
// // 		  "errors": [
// //
// // 		"""u8.ToArray();
// // 	public static readonly byte[] BodyEndBytes = "\r\n  ]\r\n}"u8.ToArray();
// //
// // 	public static readonly byte[] ErrorPart1 = "    {\r\n      \"detail\": \""u8.ToArray();
// // 	public static readonly byte[] ErrorPart2 = "\",\r\n      \"resourceKey\": \""u8.ToArray();
// // 	public static readonly byte[] ErrorPart3 = "\",\r\n      \"args\": ["u8.ToArray();
// // 	public static readonly byte[] ErrorPart4 = "],\r\n      \"pointer\": \""u8.ToArray();
// // 	public static readonly byte[] ErrorPart5 = "\",\r\n      \"fieldName\": \""u8.ToArray();
// // 	public static readonly byte[] ErrorPart6 = "\"\r\n    }"u8.ToArray();
// // 	public static readonly byte[] Comma = ",\r\n"u8.ToArray();
// // }
//
// static class ResultParts
// {
// 	public static readonly ReadOnlyMemory<byte> BodyStartBytes =
// 		"""
// 		{
// 		  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
// 		  "title": "One or more validation errors occurred.",
// 		  "status": 400,
// 		  "errors": [
//
// 		"""u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> BodyEndBytes = "\r\n  ]\r\n}"u8.ToArray();
//
// 	public static readonly ReadOnlyMemory<byte> ErrorPart1 = "    {\r\n      \"detail\": \""u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> ErrorPart2 = "\",\r\n      \"resourceKey\": \""u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> ErrorPart3 = "\",\r\n      \"args\": ["u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> ErrorPart4 = "],\r\n      \"pointer\": \""u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> ErrorPart5 = "\",\r\n      \"fieldName\": \""u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> ErrorPart6 = "\"\r\n    }"u8.ToArray();
// 	public static readonly ReadOnlyMemory<byte> Comma = ",\r\n"u8.ToArray();
// }
//
// // ref struct ResultParts
// // {
// // 	public readonly ReadOnlySpan<byte> BodyStartBytes =
// // 		"""
// // 		{
// // 		  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
// // 		  "title": "One or more validation errors occurred.",
// // 		  "status": 400,
// // 		  "errors": [
// //
// // 		"""u8;
// // 	public readonly ReadOnlySpan<byte> BodyEndBytes = "  ]\r\n}"u8;
// //
// // 	public readonly ReadOnlySpan<byte> ErrorPart1 = "    {\r\n      \"detail\": \""u8;
// // 	public readonly ReadOnlySpan<byte> ErrorPart2 = "\",\r\n      \"resourceKey\": \""u8;
// // 	public readonly ReadOnlySpan<byte> ErrorPart3 = "\",\r\n      \"args\": ["u8;
// // 	public readonly ReadOnlySpan<byte> ErrorPart4 = "],\r\n      \"pointer\": \""u8;
// // 	public readonly ReadOnlySpan<byte> ErrorPart5 = "\",\r\n      \"fieldName\": \""u8;
// // 	public readonly ReadOnlySpan<byte> ErrorPart6 = "\"\r\n    }\r\n"u8;
// // 	public readonly ReadOnlySpan<byte> Comma = ","u8;
// //
// // 	public ResultParts() { }
// // }
