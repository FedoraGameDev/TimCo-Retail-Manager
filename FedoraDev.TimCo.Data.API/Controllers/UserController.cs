using FedoraDev.TimCo.Data.API.Data;
using FedoraDev.TimCo.Data.API.Models;
using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IServiceProvider _serviceProvider;
        private readonly IUserData _userData;
        private readonly ILogger<UserController> _logger;

		public UserController(IServiceProvider serviceProvider, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
			_serviceProvider = serviceProvider;
			_dbContext = dbContext;
			_userManager = userManager;

            _userData = _serviceProvider.GetRequiredService<IUserData>();
            _logger = _serviceProvider.GetRequiredService<ILogger<UserController>>();
		}

        #region Get
        [HttpGet]
        public IEnumerable<UserModel> GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _userData.GetUserById(userId);
        }

        [HttpGet, Route("Admin/GetAll")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> userModels = new List<ApplicationUserModel>();

            List<IdentityUser> users = _userManager.Users.ToList();
            var userRoles = from user in _dbContext.UserRoles
                            join role in _dbContext.Roles on user.RoleId equals role.Id
                            select new { user.UserId, user.RoleId, role.Name };

            foreach (IdentityUser user in users)
            {
                ApplicationUserModel userModel = new ApplicationUserModel()
                {
                    Id = user.Id,
                    EmailAddress = user.Email
                };

                userModel.Roles = userRoles.Where(role => role.UserId == user.Id).ToDictionary(key => key.RoleId, value => value.Name);

                userModels.Add(userModel);
            }

            return userModels;
        }

        [HttpGet, Route("Admin/GetAllRoles")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public Dictionary<string, string> GetAllRoles()
        {
            return _dbContext.Roles.ToDictionary(role => role.Id, role => role.Name);
        }
        #endregion

        #region Post
        [HttpPost, Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationModel userRegistration)
		{
            if (!ModelState.IsValid)
                return BadRequest();

			IdentityUser existingUser = await _userManager.FindByEmailAsync(userRegistration.EmailAddress);
            if (existingUser != null)
                return BadRequest();

            IdentityUser newUser = new()
            {
                Email = userRegistration.EmailAddress,
                EmailConfirmed = true,
                UserName = userRegistration.EmailAddress
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, userRegistration.Password);
            if (!result.Succeeded)
                return BadRequest();

            existingUser = await _userManager.FindByEmailAsync(userRegistration.EmailAddress);
            if (existingUser == null)
                return BadRequest();

            UserModel user = new UserModel()
            {
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,
                EmailAddress = userRegistration.EmailAddress,
                Id = existingUser.Id
            };
            _userData.CreateUser(user);
            return Ok();
		}

        [HttpPost, Route("Admin/AddRole")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public async Task AddRole(UserRolePairModel userRolePair)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			UserModel loggedInUser = _userData.GetUserById(loggedInUserId).First();
            IdentityUser user = await _userManager.FindByIdAsync(userRolePair.UserId);

            _logger.LogInformation("ADMIN: {Admin} added user {User} to role {Role}.", loggedInUser.EmailAddress, user.Email, userRolePair.RoleName);

            _ = await _userManager.AddToRoleAsync(user, userRolePair.RoleName);
        }

        [HttpPost, Route("Admin/RemoveRole")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public async Task RemoveRole(UserRolePairModel userRolePair)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserModel loggedInUser = _userData.GetUserById(loggedInUserId).First();
            IdentityUser user = await _userManager.FindByIdAsync(userRolePair.UserId);

            _logger.LogInformation("ADMIN: {Admin} removed user {User} from role {Role}.", loggedInUser.EmailAddress, user.Email, userRolePair.RoleName);

            _ = await _userManager.RemoveFromRoleAsync(user, userRolePair.RoleName);
        }
        #endregion
    }
}
