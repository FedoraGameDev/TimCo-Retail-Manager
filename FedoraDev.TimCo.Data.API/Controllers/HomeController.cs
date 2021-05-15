using FedoraDev.TimCo.Data.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
		{
			_logger = logger;
			_roleManager = roleManager;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Privacy()
		{
			//// TODO: <Get this outta here>
			//string[] roles = { "Admin", "Manager", "Cashier" };

			//foreach (string role in roles)
			//{
			//	bool roleExist = await _roleManager.RoleExistsAsync(role);
			//	if (!roleExist)
			//		_ = await _roleManager.CreateAsync(new IdentityRole(role));
			//}

			//IdentityUser user = await _userManager.FindByEmailAsync("some@guy.com");
			//if (user != null)
			//{
			//	_ = await _userManager.AddToRoleAsync(user, roles[0]);
			//	_ = await _userManager.AddToRoleAsync(user, roles[2]);
			//}
			//// TODO: </Get this outta here>

			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
