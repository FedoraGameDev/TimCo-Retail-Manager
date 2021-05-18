using Blazored.LocalStorage;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.Portal.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Portal.Authentication
{
	public class AuthenticationService : IAuthenticationService
	{
		private readonly HttpClient _httpClient;
		private readonly IAuthenticationStateProvider _authenticationStateProvider;
		private readonly ILocalStorageService _localStorageService;
		private readonly IAPIHelper _apiHelper;

		public AuthenticationService(HttpClient httpClient, IAuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService, IAPIHelper apiHelper)
		{
			_httpClient = httpClient;
			_authenticationStateProvider = authenticationStateProvider;
			_localStorageService = localStorageService;
			_apiHelper = apiHelper;
		}

		public async Task<AuthenticatedUserModel> Login(AuthenticationUserModel authenticationUser)
		{
			AuthenticatedUserModel authenticatedUser = await _apiHelper.Authenticate(authenticationUser.EmailAddress, authenticationUser.Password);
			await _localStorageService.SetItemAsync(DefaultAuthenticationStateProvider.AUTHENTICATION_TOKEN_KEY, authenticatedUser.Access_Token);

			_authenticationStateProvider.NotifyUserAuthentication(authenticatedUser.Access_Token);

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authenticatedUser.Access_Token);

			return authenticatedUser;
		}

		public async Task Logout()
		{
			await _localStorageService.RemoveItemAsync(DefaultAuthenticationStateProvider.AUTHENTICATION_TOKEN_KEY);
			_authenticationStateProvider.NotifyUserLogout();
			_httpClient.DefaultRequestHeaders.Authorization = null;
		}
	}
}
