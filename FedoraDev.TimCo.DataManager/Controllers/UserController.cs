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
    }
}
