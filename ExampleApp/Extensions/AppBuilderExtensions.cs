using ExampleApp.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace ExampleApp.Extensions
{
	public static class AppBuilderExtensions
	{
		public static IApplicationBuilder UseTokenCheck(
			this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<TokenAuthenticationMiddleware>();
		}
	}
}