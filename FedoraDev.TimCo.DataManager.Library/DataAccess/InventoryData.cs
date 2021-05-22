using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class InventoryData : IInventoryData
	{
		private readonly ISqlDataAccess _sqlDataAccess;

		public InventoryData(ISqlDataAccess sqlDataAccess)
		{
			_sqlDataAccess = sqlDataAccess;
		}

		public List<InventoryModel> GetInventory()
		{
			var parameters = new { };
			return _sqlDataAccess.LoadData<InventoryModel, dynamic>("TimCo-Data", "spInventoryGetAll", parameters);
		}

		public void SaveInventoryItem(InventoryModel itemData)
		{
			_sqlDataAccess.SaveData("TimCo-Data", "spInventoryInsert", itemData);
		}
	}

}
