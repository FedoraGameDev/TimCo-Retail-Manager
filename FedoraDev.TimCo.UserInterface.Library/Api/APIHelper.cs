using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Helpers
{
	public class APIHelper : IAPIHelper
	{
		private HttpClient _apiClient;
		private ILoggedInUserModel _user;

		public HttpClient ApiClient => _apiClient;

		public APIHelper(ILoggedInUserModel user)
		{
			InitilalizeClient();
			_user = user;
		}

		private void InitilalizeClient()
		{
			string api = ConfigurationManager.AppSettings["api"];

			_apiClient = new HttpClient();
			_apiClient.BaseAddress = new Uri(api);
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task<AuthenticatedUserModel> Authenticate(string username, string password)
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
					AuthenticatedUserModel user = await responseMessage.Content.ReadAsAsync<AuthenticatedUserModel>();
					return user;
				}

				throw new Exception(responseMessage.ReasonPhrase);
			}
		}

		public void LogoutUser()
		{
			_apiClient.DefaultRequestHeaders.Clear();
		}

		public async Task SetLoggedInUserInfo(string token)
		{
			_apiClient.DefaultRequestHeaders.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Clear();
			_apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			_apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

			using (HttpResponseMessage responseMessage = await _apiClient.GetAsync("/api/User"))
			{
				if (responseMessage.IsSuccessStatusCode)
				{
					List<LoggedInUserModel> users = await responseMessage.Content.ReadAsAsync<List<LoggedInUserModel>>();
					LoggedInUserModel user = users[0];
					_user.Token = token;
					_user.Id = user.Id;
					_user.FirstName = user.FirstName;
					_user.LastName = user.LastName;
					_user.EmailAddress = user.EmailAddress;
					_user.CreatedDate = user.CreatedDate;
					return;
				}

				throw new Exception(responseMessage.ReasonPhrase);
			}
		}
	}
}
