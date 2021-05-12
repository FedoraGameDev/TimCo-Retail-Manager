using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
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
    }
}
