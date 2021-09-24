using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public interface IInventoryData
	{
		List<InventoryModel> GetInventory();
		void SaveInventoryItem(InventoryModel itemData);
	}
}