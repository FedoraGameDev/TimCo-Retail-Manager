using FedoraDev.TimCo.Data.API.Data;
using FedoraDev.TimCo.Data.API.Models;
using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
		private readonly IConfiguration _configuration;

		public UserController(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IConfiguration configuration)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_configuration = configuration;
		}

        #region Get
        [HttpGet]
        public IEnumerable<UserModel> GetById()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserData userData = new UserData(_configuration);

            return userData.GetUserById(id);
        }

        [HttpGet, Route("api/User/Admin/GetAll")]
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

        [HttpGet, Route("api/User/Admin/GetAllRoles")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public Dictionary<string, string> GetAllRoles()
        {
            return _dbContext.Roles.ToDictionary(role => role.Id, role => role.Name);
        }
        #endregion

        #region Post
        [HttpPost, Route("api/User/Admin/AddRole")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public async Task AddRole(UserRolePairModel userRolePair)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userRolePair.UserId);
			_ = await _userManager.AddToRoleAsync(user, userRolePair.RoleName);
        }

        [HttpPost, Route("api/User/Admin/RemoveRole")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public async Task RemoveRole(UserRolePairModel userRolePair)
        {
            IdentityUser user = await _userManager.FindByIdAsync(userRolePair.UserId);
            _ = await _userManager.RemoveFromRoleAsync(user, userRolePair.RoleName);
        }
        #endregion
    }
}
