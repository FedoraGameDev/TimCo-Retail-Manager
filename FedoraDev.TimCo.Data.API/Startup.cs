using FedoraDev.TimCo.Data.API.Data;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace FedoraDev.TimCo.Data.API
{
	public class Startup
	{
		#region Fields
		private const string CORS_POLICY_NAME = "OpenCorsPolicy";
		private const string AUTH_DB_NAME = "TimCo-Auth";
		private const string SWAGGER_API_TITLE = "TimCo Retail Manager API";
		private const string SWAGGER_API_VERSION = "v1";
		private const string SECURITY_KEY_LOCATION = "Secrets:SecurityKey";
		private const string WEB_EXCEPTION_HANDLER = "/Home/Error";
		private const string ENDPOINT_MAP_NAME = "default";
		private const string ENDPOINT_MAP_PATTERN = "{controller=Home}/{action=Index}/{id?}";
		#endregion

		#region Properties
		public IConfiguration Configuration { get; }
		private static string SwaggerEndpointURL => $"/swagger/{SWAGGER_API_VERSION}/swagger.json";
		private static string SwaggerEndpointName => $"TimCo API {SWAGGER_API_VERSION.ToUpper()}";
		#endregion

		#region Configuration at a Glance
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			AddDatabase(services);
			AddEntityFramework(services);
			AddWeb(services);
			AddTransientServices(services);
			AddAuthentication(services);
			AddSwagger(services);
			AddLogging(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			UseExceptionHandlers(app, env);
			UseRouting(app);
			UseSwagger(app);
			UseEndpoints(app);
		}
		#endregion

		#region Add Services
		private void AddDatabase(IServiceCollection services)
		{
			_ = services.AddDbContext<ApplicationDbContext>(options =>
				  options.UseSqlServer(
					  Configuration.GetConnectionString(AUTH_DB_NAME)
				  )
			);
		}

		private void AddEntityFramework(IServiceCollection services)
		{
			_ = services
				.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
		}

		private void AddWeb(IServiceCollection services)
		{
			_ = services.AddCors(policy =>
			{
				policy.AddPolicy(CORS_POLICY_NAME, options =>
				{
					_ = options
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});
			_ = services.AddControllersWithViews();
			_ = services.AddRazorPages();
		}

		private void AddTransientServices(IServiceCollection services)
		{
			_ = services
				.AddTransient<ISqlDataAccess, SqlDataAccess>()
				.AddTransient<IUserData, UserData>()
				.AddTransient<ISaleData, SaleData>()
				.AddTransient<IProductData, ProductData>()
				.AddTransient<IInventoryData, InventoryData>();
		}

		private void AddAuthentication(IServiceCollection services)
		{
			_ = services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(jwtBearerOptions =>
			{
				jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>(SECURITY_KEY_LOCATION))),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(5)
				};
			});
		}

		private void AddSwagger(IServiceCollection services)
		{
			_ = services.AddSwaggerGen(setup =>
			{
				setup.SwaggerDoc(SWAGGER_API_VERSION, new OpenApiInfo { Title = SWAGGER_API_TITLE, Version = SWAGGER_API_VERSION });
			});
		}

		private void AddLogging(IServiceCollection services)
		{
			_ = services.AddLogging();
		}
		#endregion

		#region Use Middleware
		private void UseExceptionHandlers(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				_ = app
					.UseDeveloperExceptionPage()
					.UseDatabaseErrorPage();
			}
			else
			{
				_ = app
					.UseExceptionHandler(WEB_EXCEPTION_HANDLER)
					.UseHsts();
			}
		}

		private void UseRouting(IApplicationBuilder app)
		{
			_ = app
				.UseHttpsRedirection()
				.UseCors(CORS_POLICY_NAME)
				.UseStaticFiles()
				.UseRouting()
				.UseAuthentication()
				.UseAuthorization();
		}

		private void UseSwagger(IApplicationBuilder app)
		{
			_ = app
				.UseSwagger()
				.UseSwaggerUI(swaggerOptions =>
				{
					swaggerOptions.SwaggerEndpoint(SwaggerEndpointURL, SwaggerEndpointName);
				});
		}

		private void UseEndpoints(IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				_ = endpoints.MapControllerRoute(
					name: ENDPOINT_MAP_NAME,
					pattern: ENDPOINT_MAP_PATTERN
				);
				_ = endpoints.MapRazorPages();
			});
		}
		#endregion
	}
}
