using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class InventoryData
	{
		public List<InventoryModel> GetInventory()
		{
			SqlDataAccess sql = new SqlDataAccess();

			var parameters = new { };
			return sql.LoadData<InventoryModel, dynamic>("spInventoryGetAll", parameters, "TimCo-Data");
		}

		public void SaveInventoryItem(InventoryModel itemData)
		{
			SqlDataAccess sql = new SqlDataAccess();

			sql.SaveData("spInventoryInsert", itemData, "TimCo-Data");
		}
	}

}
