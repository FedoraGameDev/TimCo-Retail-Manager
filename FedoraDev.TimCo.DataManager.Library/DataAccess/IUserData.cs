using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public interface IUserData
	{
		void CreateUser(UserModel user);
		List<UserModel> GetUserById(string id);
	}
}