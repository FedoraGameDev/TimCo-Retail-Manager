using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly IServiceProvider _serviceProvider;

		public InventoryController(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		[HttpGet]
		[Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
		public List<InventoryModel> Get()
		{
			IInventoryData inventory = _serviceProvider.GetRequiredService<IInventoryData>();
			return inventory.GetInventory();
		}

		[HttpPost]
		[Authorize(Roles = Roles.ADMIN)]
		public void Post(InventoryModel itemData)
		{
			IInventoryData inventory = _serviceProvider.GetRequiredService<IInventoryData>();
			inventory.SaveInventoryItem(itemData);
		}
	}
}
