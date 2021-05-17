using FedoraDev.TimCo.Data.API.Controllers;
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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace FedoraDev.TimCo.Data.API
{
	public class Startup
	{
		#region Properties
		public IConfiguration Configuration { get; }
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
					  Configuration.GetConnectionString("TimCo-Auth")
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
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Secrets:SecurityKey"))),
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
				setup.SwaggerDoc("v1", new OpenApiInfo { Title = "TimCo Retail Manager API", Version = "v1" });
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
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
		}

		private void UseRouting(IApplicationBuilder app)
		{
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();
		}

		private void UseSwagger(IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(swaggerOptions =>
			{
				swaggerOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "TimCo API v1");
			});
		}

		private void UseEndpoints(IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
		#endregion
	}
}
