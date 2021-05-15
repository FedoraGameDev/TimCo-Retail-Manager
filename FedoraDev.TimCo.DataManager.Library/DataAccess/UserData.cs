using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class UserData
	{
		private readonly IConfiguration _configuration;

		public UserData(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public List<UserModel> GetUserById(string id)
		{
			SqlDataAccess sql = new SqlDataAccess(_configuration);

			var parameters = new { Id = id };

			return sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "TimCo-Data");
		}
	}
}
