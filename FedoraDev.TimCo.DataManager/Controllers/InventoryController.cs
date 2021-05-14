using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace FedoraDev.TimCo.DataManager.Controllers
{
    [Authorize]
	public class InventoryController : ApiController
    {
        [HttpGet]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public List<InventoryModel> Get()
		{
            InventoryData inventory = new InventoryData();
            return inventory.GetInventory();
		}

        [HttpPost]
        [Authorize(Roles = Roles.ADMIN)]
        public void Post(InventoryModel itemData)
		{
            InventoryData inventory = new InventoryData();
            inventory.SaveInventoryItem(itemData);
		}
    }
}
