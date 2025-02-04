﻿using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;
using System.Threading;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LoginEvent>, IHandle<CheckoutEvent>
	{
		private readonly ILoggedInUserModel _loggedInUser;

		public bool IsLoggedOut => !IsLoggedIn;
		public bool IsLoggedIn => !string.IsNullOrWhiteSpace(_loggedInUser.Token);
		public bool CanViewUsers => IsLoggedIn;

		public ShellViewModel(ILoggedInUserModel loggedInUser)
		{
			_loggedInUser = loggedInUser;

			IoC.Get<IEventAggregator>().SubscribeOnPublishedThread(this);
			_ = ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
		}

		public async Task HandleAsync(LoginEvent loginEvent, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
			NotifyOfPropertyChange(() => IsLoggedIn);
			NotifyOfPropertyChange(() => IsLoggedOut);
			NotifyOfPropertyChange(() => CanViewUsers);
		}

		public async Task HandleAsync(CheckoutEvent checkoutEvent, CancellationToken cancellationToken)
		{
			await ActivateItemAsync(IoC.Get<SalesViewModel>(), cancellationToken);
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
			NotifyOfPropertyChange(() => IsLoggedOut);
			NotifyOfPropertyChange(() => CanViewUsers);
		}

		public async Task Login()
		{
			await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
		}

		public async Task ViewUsersPage()
		{
			await ActivateItemAsync(IoC.Get<UserDisplayViewModel>(), new CancellationToken());
		}

		public async Task ViewSalePage()
		{
			await ActivateItemAsync(IoC.Get<SalesViewModel>(), new CancellationToken());
		}
	}
}
