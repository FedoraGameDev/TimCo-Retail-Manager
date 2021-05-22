using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Portal.Authentication
{
	public class DefaultAuthenticationStateProvider : AuthenticationStateProvider, IAuthenticationStateProvider
	{
		public const string AUTHENTICATION_TOKEN_KEY = "AuthenticationToken";
		private const string CLAIMS_IDENTITY_TYPE = "jwtAuthType";

		private readonly HttpClient _httpClient;
		private readonly ILocalStorageService _localStorageService;
		private readonly AuthenticationState _anonymous;

		public DefaultAuthenticationStateProvider(HttpClient httpClient, ILocalStorageService localStorageService)
		{
			_httpClient = httpClient;
			_localStorageService = localStorageService;
			_anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			string token = await _localStorageService.GetItemAsync<string>(AUTHENTICATION_TOKEN_KEY);

			if (string.IsNullOrWhiteSpace(token))
				return _anonymous;

			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

			return new AuthenticationState(GenerateClaimsPrincipal(token));
		}

		public void NotifyUserAuthentication(string token)
		{
			ClaimsPrincipal authenticatedUser = GenerateClaimsPrincipal(token);
			Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(authenticatedUser));
			NotifyAuthenticationStateChanged(authenticationState);
		}

		public void NotifyUserLogout()
		{
			Task<AuthenticationState> authenticationState = Task.FromResult(_anonymous);
			NotifyAuthenticationStateChanged(authenticationState);
		}

		private ClaimsPrincipal GenerateClaimsPrincipal(string token)
		{
			IEnumerable<Claim> claims = JwtParser.ParseClaimsFromJwt(token);
			ClaimsIdentity identity = new ClaimsIdentity(claims, CLAIMS_IDENTITY_TYPE);
			return new ClaimsPrincipal(identity);
		}
	}
}
