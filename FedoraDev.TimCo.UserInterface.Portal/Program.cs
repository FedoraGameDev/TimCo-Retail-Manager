using Blazored.LocalStorage;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.Portal.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Portal
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			_ = builder.Services
				.AddScoped<IAuthenticationService, AuthenticationService>()
				.AddScoped<AuthenticationStateProvider, DefaultAuthenticationStateProvider>();

			_ = builder.Services
				.AddSingleton<IAPIHelper, APIHelper>()
				.AddSingleton<ILoggedInUserModel, LoggedInUserModel>()
				.AddSingleton<IUserEndpoint, UserEndpoint>();

			_ = builder.Services.AddBlazoredLocalStorage();
			_ = builder.Services.AddAuthorizationCore();

			_ = builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			await builder.Build().RunAsync();
		}
	}
}
