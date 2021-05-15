using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	[ApiController]
	public class InventoryController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public InventoryController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		[Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
		public List<InventoryModel> Get()
		{
			InventoryData inventory = new InventoryData(_configuration);
			return inventory.GetInventory();
		}

		[HttpPost]
		[Authorize(Roles = Roles.ADMIN)]
		public void Post(InventoryModel itemData)
		{
			InventoryData inventory = new InventoryData(_configuration);
			inventory.SaveInventoryItem(itemData);
		}
	}
}
