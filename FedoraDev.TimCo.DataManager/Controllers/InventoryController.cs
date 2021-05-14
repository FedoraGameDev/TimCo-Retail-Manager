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
        public List<InventoryModel> Get()
		{
            InventoryData inventory = new InventoryData();
            return inventory.GetInventory();
		}

        [HttpPost]
        public void Post(InventoryModel itemData)
		{
            InventoryData inventory = new InventoryData();
            inventory.SaveInventoryItem(itemData);
		}
    }
}
