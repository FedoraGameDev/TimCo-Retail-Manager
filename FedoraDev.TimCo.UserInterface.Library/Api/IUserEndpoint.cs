﻿using FedoraDev.TimCo.UserInterface.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Api
{
	public interface IUserEndpoint
	{
		Task<List<UserModel>> GetAll();
		Task<Dictionary<string, string>> GetAllRoles();
		Task AddUserToRole(string userId, string roleName);
		Task RemoveUserFromRole(string userId, string roleName);
		Task RegisterUser(RegisterationUserModel user);
	}
}