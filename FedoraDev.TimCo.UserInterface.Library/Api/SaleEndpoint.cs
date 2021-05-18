using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Api
{
	public class SaleEndpoint : ISaleEndpoint
	{
		private readonly IAPIHelper _apiHelper;

		public SaleEndpoint(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task PostSale(SaleModel sale)
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync("/api/Sale", sale))
			{
				if (response.IsSuccessStatusCode)
					return;

				throw new Exception(response.ReasonPhrase);
			}
		}

		public async Task<decimal> GetTaxRate()
		{
			using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Sale/GetTaxRate"))
			{
				if (response.IsSuccessStatusCode)
					return await response.Content.ReadAsAsync<decimal>();

				throw new Exception(response.ReasonPhrase);
			}
		}
	}
}
