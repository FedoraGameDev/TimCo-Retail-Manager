using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Api
{
	public class UserEndpoint : IUserEndpoint
	{
		private readonly IAPIHelper _apiHelper;

		public UserEndpoint(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task<List<UserModel>> GetAll()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/User/Admin/GetAll"))
			{
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadAsAsync<List<UserModel>>();

				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task<Dictionary<string, string>> GetAllRoles()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/User/Admin/GetAllRoles"))
			{
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadAsAsync<Dictionary<string, string>>();

				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task AddUserToRole(string userId, string roleName)
		{
			var data = new
			{
				UserId = userId,
				RoleName = roleName
			};

			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/User/Admin/AddRole", data))
			{
				if (!response.IsSuccessStatusCode)
					throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task RemoveUserFromRole(string userId, string roleName)
		{
			var data = new
			{
				UserId = userId,
				RoleName = roleName
			};

			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/User/Admin/RemoveRole", data))
			{
				if (!response.IsSuccessStatusCode)
					throw new Exception(response.ReasonPhrase);
			}
		}
	}
}
