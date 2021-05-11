using Caliburn.Micro;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class ShellViewModel : Conductor<object>
	{
		private LoginViewModel _loginVM;

		public ShellViewModel(LoginViewModel loginVM)
		{
			_loginVM = loginVM;
			ActivateItem(_loginVM);
		}
	}
}
