using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LoginEvent>
	{
		private readonly SalesViewModel _salesVM;
		private readonly ILoggedInUserModel _loggedInUser;

		public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_loggedInUser.Token);
		public bool CanViewUsers => true;

		public ShellViewModel(SalesViewModel salesVM, ILoggedInUserModel loggedInUser)
		{
			_salesVM = salesVM;
			_loggedInUser = loggedInUser;

			IoC.Get<IEventAggregator>().Subscribe(this);
			ActivateItem(IoC.Get<LoginViewModel>());
		}

		public void Handle(LoginEvent loginEvent)
		{
			ActivateItem(_salesVM);
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public void ExitApplication()
		{
			TryClose();
		}

		public void Logout()
		{
			_loggedInUser.Clear();
			IoC.Get<IAPIHelper>().LogoutUser();
			ActivateItem(IoC.Get<LoginViewModel>());
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public void ViewUsersPage()
		{
			ActivateItem(IoC.Get<UserDisplayViewModel>());
		}
	}
}
