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
		private readonly IEventAggregator _events;
		private readonly IAPIHelper _apiHelper;

		public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_loggedInUser.Token);

		public ShellViewModel(SalesViewModel salesVM, ILoggedInUserModel loggedInUser, IEventAggregator events, IAPIHelper apiHelper)
		{
			_salesVM = salesVM;
			_loggedInUser = loggedInUser;
			_events = events;
			_apiHelper = apiHelper;
			_events.Subscribe(this);
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
			_apiHelper.LogoutUser();
			ActivateItem(IoC.Get<LoginViewModel>());
			NotifyOfPropertyChange(() => IsLoggedIn);
		}
	}
}
