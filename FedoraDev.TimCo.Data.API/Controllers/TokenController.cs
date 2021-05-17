using FedoraDev.TimCo.Data.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	public class TokenController : Controller
	{
		public static string SecretKey => "MySecretKeyIsSecretSoDoNotTell";

		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IServiceProvider _serviceProvider;

		public TokenController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IServiceProvider serviceProvider)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_serviceProvider = serviceProvider;
		}

		[Route("/token")]
		[HttpPost]
		public async Task<IActionResult> Create(string username, string password, string grant_type)
		{
			if (await IsValidUserInfo(username, password))
			{
				return new ObjectResult(await GenerateToken(username));
			}
			else
			{
				return BadRequest();
			}
		}

		private async Task<bool> IsValidUserInfo(string username, string password)
		{
			IdentityUser user = await _userManager.FindByNameAsync(username);
			return await _userManager.CheckPasswordAsync(user, password);
		}

		private async Task<dynamic> GenerateToken(string username)
		{
			IdentityUser identityUser = await _userManager.FindByNameAsync(username);
			var roles = from user in _dbContext.UserRoles
						join role in _dbContext.Roles on user.RoleId equals role.Id
						where user.UserId == identityUser.Id
						select new { user.UserId, user.RoleId, role.Name };

			List<Claim> claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username),
				new Claim(ClaimTypes.NameIdentifier, identityUser.Id),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.UtcNow.AddDays(1)).ToUnixTimeSeconds().ToString())
			};

			foreach (var role in roles)
				claims.Add(new Claim(ClaimTypes.Role, role.Name));

			SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_serviceProvider.GetRequiredService<IConfiguration>().GetValue<string>("Secrets:SecurityKey")));
			SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			JwtHeader header = new JwtHeader(credentials);
			JwtPayload payload = new JwtPayload(claims);
			JwtSecurityToken token = new JwtSecurityToken(header, payload);

			return new
			{
				Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
				UserName = username
			};
		}
	}
}
