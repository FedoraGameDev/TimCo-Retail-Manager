using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class UserDisplayViewModel : Screen
	{
		private readonly IWindowManager _windowManager;
		private BindingList<UserModel> _users;

		public BindingList<UserModel> Users
		{
			get { return _users; }
			set
			{
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		public UserDisplayViewModel(IWindowManager windowManager)
		{
			_windowManager = windowManager;
		}

		protected override async void OnViewLoaded(object view)
		{
			try
			{
				await LoadUsers();
			}
			catch (Exception ex)
			{
				StatusInfoViewModel infoViewModel = IoC.Get<StatusInfoViewModel>();
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;

				if (ex.Message == "Unauthorizedd")
				{
					settings.Title = "Authorization Error";

					infoViewModel.UpdateMessage("Unauthorized Access", "You are not authorized to view the users list.");
					_windowManager.ShowDialog(infoViewModel, null, settings);
					TryClose();
				}
				else
				{
					settings.Title = "Fatal Exception";

					infoViewModel.UpdateMessage("Fatal Exception", ex.Message);
					_windowManager.ShowDialog(infoViewModel, null, settings);
				}
			}
		}

		private async Task LoadUsers()
		{
			IUserEndpoint userEndpoint = IoC.Get<IUserEndpoint>();

			Users = new BindingList<UserModel>(await userEndpoint.GetAll());
		}
	}
}
