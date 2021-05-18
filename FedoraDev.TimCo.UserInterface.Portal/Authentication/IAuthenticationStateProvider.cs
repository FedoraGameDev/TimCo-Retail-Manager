namespace FedoraDev.TimCo.UserInterface.Portal.Authentication
{
	public interface IAuthenticationStateProvider
	{
		void NotifyUserAuthentication(string token);
		void NotifyUserLogout();
	}
}