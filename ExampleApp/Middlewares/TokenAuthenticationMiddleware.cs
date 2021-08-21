using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ExampleApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ExampleApp.Middlewares
{
	public class TokenAuthenticationMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly AuthenticationOptions _options;

		public TokenAuthenticationMiddleware(RequestDelegate next, IOptions<AuthenticationOptions> options)
		{
			_next = next;
			_options = options.Value;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			if (context.Request.Headers.ContainsKey("TOKEN"))
			{
				var header = context.Request.Headers["TOKEN"].FirstOrDefault();
				
				if (string.Equals(header,_options.Token))
				{
					// Call the next delegate/middleware in the pipeline
					await _next(context);
				}
			}
			
			context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
		}
	}
}