using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class UserData : IUserData
	{
		private readonly ISqlDataAccess _sqlDataAccess;

		public UserData(ISqlDataAccess sqlDataAccess)
		{
			_sqlDataAccess = sqlDataAccess;
		}

		public List<UserModel> GetUserById(string id)
		{
			var parameters = new { Id = id };
			return _sqlDataAccess.LoadData<UserModel, dynamic>("TimCo-Data", "dbo.spUserLookup", parameters);
		}

		public void CreateUser(UserModel user)
		{
			var parameters = new
			{
				user.Id,
				user.FirstName,
				user.LastName,
				user.EmailAddress
			};

			_sqlDataAccess.SaveData("TimCo-Data", "dbo.spUserInsert", parameters);
		}
	}
}
