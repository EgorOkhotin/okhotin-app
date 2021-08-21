using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace ExampleApp.Models
{
	public class AuthenticationOptions
	{
		public string Instance { get; set; } = "https://login.microsoftonline.com/{0}";
		public string Token { get; set; }
		
		public string ClientId { get; set; }
		public string TenantId { get; set; }
		public string CertificateName { get; set; }
		public string Authority => String.Format(CultureInfo.InvariantCulture, Instance, TenantId);
		
		public string BaseScope { get; set; }

		public static AuthenticationOptions ReadFromJsonFile(string path)
		{
			IConfigurationRoot Configuration;

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(path);

			Configuration = builder.Build();
			return Configuration.GetSection("AuthenticationOptions").Get<AuthenticationOptions>();
		}
	}
}