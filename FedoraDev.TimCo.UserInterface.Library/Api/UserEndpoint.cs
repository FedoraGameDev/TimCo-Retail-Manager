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
	}
}
