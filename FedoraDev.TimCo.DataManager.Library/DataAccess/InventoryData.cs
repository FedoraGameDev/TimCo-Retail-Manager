using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class InventoryData
	{
		private readonly IConfiguration _configuration;

		public InventoryData(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public List<InventoryModel> GetInventory()
		{
			SqlDataAccess sql = new SqlDataAccess(_configuration);

			var parameters = new { };
			return sql.LoadData<InventoryModel, dynamic>("spInventoryGetAll", parameters, "TimCo-Data");
		}

		public void SaveInventoryItem(InventoryModel itemData)
		{
			SqlDataAccess sql = new SqlDataAccess(_configuration);

			sql.SaveData("spInventoryInsert", itemData, "TimCo-Data");
		}
	}

}
