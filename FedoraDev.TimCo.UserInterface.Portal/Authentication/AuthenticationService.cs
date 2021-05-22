using Blazored.LocalStorage;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.Portal.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Portal.Authentication
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly HttpClient _httpClient;
		private readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly ILocalStorageService _localStorageService;
		private readonly IAPIHelper _apiHelper;
		private readonly IConfiguration _configuration;

		public AuthenticationService(IServiceProvider serviceProvider)
		{
			_httpClient = serviceProvider.GetRequiredService<HttpClient>();
			_authenticationStateProvider = serviceProvider.GetRequiredService<AuthenticationStateProvider>();
			_localStorageService = serviceProvider.GetRequiredService<ILocalStorageService>();
			_apiHelper = serviceProvider.GetRequiredService<IAPIHelper>();
			_configuration = serviceProvider.GetRequiredService<IConfiguration>();
		}

		public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel authenticationUser)
		{
			try
			{
				AuthenticatedUserModel authenticatedUser = await _apiHelper.Authenticate(authenticationUser.EmailAddress, authenticationUser.Password);
				await _localStorageService.SetItemAsync(_configuration["AuthTokenStorageKey"], authenticatedUser.Access_Token);

				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticatedUser.Access_Token);

				(_authenticationStateProvider as IAuthenticationStateProvider)?.NotifyUserAuthentication(authenticatedUser.Access_Token);

				return authenticatedUser;
			}
			catch
			{
				return null;
			}
		}

		public async Task Logout()
		{
			await _localStorageService.RemoveItemAsync(_configuration["AuthTokenStorageKey"]);
			_httpClient.DefaultRequestHeaders.Authorization = null;
			(_authenticationStateProvider as IAuthenticationStateProvider)?.NotifyUserLogout();
		}
	}
}
