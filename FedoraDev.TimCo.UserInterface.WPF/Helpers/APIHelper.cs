using FedoraDev.TimCo.UserInterface.WPF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.Helpers
{
	public class APIHelper : IAPIHelper
	{
		private HttpClient _apiClient;

		public APIHelper()
		{
			InitilalizeClient();
		}

		private void InitilalizeClient()
		{
			string api = ConfigurationManager.AppSettings["api"];

			_apiClient = new HttpClient();
			_apiClient.BaseAddress = new Uri(api);
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<AuthenticatedUser> Authenticate(string username, string password)
		{
			FormUrlEncodedContent data = new FormUrlEncodedContent(new[] {
				new KeyValuePair<string, string>("grant_type", "password"),
				new KeyValuePair<string, string>("username", username),
				new KeyValuePair<string, string>("password", password)
			});

			using (HttpResponseMessage responseMessage = await _apiClient.PostAsync("/token", data))
			{
				if (responseMessage.IsSuccessStatusCode)
				{
					AuthenticatedUser result = await responseMessage.Content.ReadAsAsync<AuthenticatedUser>();
					return result;
				}

				throw new Exception(responseMessage.ReasonPhrase);
			}
		}
	}
}
