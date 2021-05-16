using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;
using System.Threading;
using System.Threading.Tasks;

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

			IoC.Get<IEventAggregator>().SubscribeOnPublishedThread(this);
			_ = ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
		}

		public async Task HandleAsync(LoginEvent loginEvent, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(_salesVM, cancellationToken);
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public async Task ExitApplication()
		{
			await TryCloseAsync();
		}

		public async Task Logout()
		{
			_loggedInUser.Clear();
			IoC.Get<IAPIHelper>().LogoutUser();
			await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
			NotifyOfPropertyChange(() => IsLoggedIn);
		}

		public async Task ViewUsersPage()
		{
			await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
		}

	}
}
