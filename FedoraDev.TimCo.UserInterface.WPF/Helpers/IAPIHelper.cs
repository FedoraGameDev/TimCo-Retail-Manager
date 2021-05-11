using FedoraDev.TimCo.UserInterface.WPF.Models;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.Helpers
{
	public interface IAPIHelper
	{
		Task<AuthenticatedUser> Authenticate(string username, string password);
	}
}