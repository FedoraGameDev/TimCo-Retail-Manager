using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class UserData
	{
		public List<UserModel> GetUserById(string id)
		{
			SqlDataAccess sql = new SqlDataAccess();

			var parameters = new { Id = id };

			return sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "TimCo-Data");
		}
	}
}
