using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ExampleApp.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ExampleApp.Extensions
{
	public static class ServiceCollectionExtensions
	{
		private const string ACCESS_TOKEN = nameof(ACCESS_TOKEN);
		public static IServiceCollection ConfigureSecrets(this IServiceCollection services)
		{
			var config = AuthenticationOptions.ReadFromJsonFile("appsettings.json");
			
			X509Certificate2 certificate = ReadCertificate(config.CertificateName);
			
			IConfidentialClientApplication app;
			
			app = ConfidentialClientApplicationBuilder.Create(config.ClientId)
				.WithCertificate(certificate)
				.WithAuthority(new Uri(config.Authority))
				.Build();

			// var authCountext = new AuthenticationContext(config.Instance + config.TenantId);
			//
			// var clientAssertionCertificate =
			// 	new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientAssertionCertificate(config.ClientId,
			// 		certificate);
			//
			//
			// var resource = "https://graph.microsoft.com/Subscription.Read.All";
			// var task = await authCountext.AcquireTokenAsync(resource, clientAssertionCertificate);
			
			string[] scopes =
			{
				//config.BaseScope,
				"https://graph.microsoft.com/.default"
			};
			
			var task = app.AcquireTokenForClient(scopes)
				.ExecuteAsync();
			task.Wait();

			Environment.SetEnvironmentVariable(ACCESS_TOKEN, task.Result.AccessToken);

			GetAllResources().Wait();

			return services;
		}
		
		private static X509Certificate2 ReadCertificate(string certificateName)
		{
			if (string.IsNullOrWhiteSpace(certificateName))
			{
				throw new ArgumentException("certificateName should not be empty. Please set the CertificateName setting in the appsettings.json", "certificateName");
			}

			var cert = new X509Certificate2(certificateName, "OFB6780Liga");
			return cert;
		}




		private static async Task GetAllResources()
		{
			var httpClient = new HttpClient();
			var defaultRequestHeaders = httpClient.DefaultRequestHeaders;
			
			if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
			{
				httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			}
			
			defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable(ACCESS_TOKEN));
			var subscriptionId = "3c79dbd0-536e-404c-aed9-86f8eef2bd6c";
			var result = await httpClient.GetAsync(
				$"https://management.azure.com/subscriptions/{subscriptionId}/resources?api-version=2021-04-01");

			var status = result.StatusCode;
		}
	}
}