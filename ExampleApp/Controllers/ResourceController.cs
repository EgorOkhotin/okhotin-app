using System;
using System.Threading.Tasks;
using ExampleApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Controllers
{
	public class ResourceController : Controller
	{
		public ResourceController()
		{
			
		}

		public async Task<CreateResourceGroupResponse> CreateResourceGroup(CreateResourceGroupRequest request)
		{
			throw new NotImplementedException();
		}
		
		public async Task<ListAllResourcesResponse> ListAllResources(ListAllResourcesRequest request)
		{
			throw new NotImplementedException();
		}
	}
}