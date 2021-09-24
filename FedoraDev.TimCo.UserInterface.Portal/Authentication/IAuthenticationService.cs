using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.Portal.Models;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Portal.Authentication
{
	public interface IAuthenticationService
	{
		Task<AuthenticatedUserModel> Login(AuthenticationUserModel authenticationUser);
		Task Logout();
	}
}