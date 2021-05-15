using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using FedoraDev.TimCo.DataManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FedoraDev.TimCo.DataManager.Controllers
{
	[Authorize]
    public class UserController : ApiController
    {
		#region Get
		[HttpGet]
        public IEnumerable<UserModel> GetById()
        {
            string id = RequestContext.Principal.Identity.GetUserId();
            UserData userData = new UserData();

            return userData.GetUserById(id);
        }

        [HttpGet, Route("api/User/Admin/GetAll")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public List<ApplicationUserModel> GetAllUsers()
		{
            List<ApplicationUserModel> userModels = new List<ApplicationUserModel>();

            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
				UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

				List<ApplicationUser> users = userManager.Users.ToList();
				List<IdentityRole> roles = context.Roles.ToList();

				foreach (ApplicationUser user in users)
				{
                    ApplicationUserModel userModel = new ApplicationUserModel()
                    {
                        Id = user.Id,
                        EmailAddress = user.Email
                    };

					foreach (IdentityUserRole role in user.Roles)
					{
                        userModel.Roles.Add(role.RoleId, roles.Where(r => r.Id == role.RoleId).First().Name);
					}

                    userModels.Add(userModel);
				}
            }

            return userModels;
		}

        [HttpGet, Route("api/User/Admin/GetAllRoles")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public Dictionary<string, string> GetAllRoles()
		{
            using (ApplicationDbContext context = new ApplicationDbContext())
                return context.Roles.ToDictionary(role => role.Id, role => role.Name);
		}
		#endregion

		#region Post
		[HttpPost, Route("api/User/Admin/AddRole")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public void AddRole(UserRolePairModel userRolePair)
		{
            using (ApplicationDbContext context = new ApplicationDbContext())
			{
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

                _ = userManager.AddToRole(userRolePair.UserId, userRolePair.RoleName);
            }
		}

        [HttpPost, Route("api/User/Admin/RemoveRole")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public void RemoveRole(UserRolePairModel userRolePair)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                UserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(context);
                UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(userStore);

                _ = userManager.RemoveFromRole(userRolePair.UserId, userRolePair.RoleName);
            }
        }
		#endregion
	}
}
