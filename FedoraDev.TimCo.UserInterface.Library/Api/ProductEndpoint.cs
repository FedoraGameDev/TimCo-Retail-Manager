using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Api
{
	public class ProductEndpoint : IProductEndpoint
	{
		private readonly IAPIHelper _apiHelper;

		public ProductEndpoint(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task<List<ProductModel>> GetAll()
		{
			using (HttpResponseMessage responseMessage = await _apiHelper.ApiClient.GetAsync("/api/Product"))
			{
				if (responseMessage.IsSuccessStatusCode)
					return await responseMessage.Content.ReadAsAsync<List<ProductModel>>();

				throw new Exception(responseMessage.ReasonPhrase);
			}
		}
	}
}
