using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.WPF.Helpers;
using FedoraDev.TimCo.UserInterface.WPF.Models;
using System;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class LoginViewModel : Screen
	{
		private string _userName;
		private string _password;
		private readonly IAPIHelper _apiHelper;

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogin);
			}
		}

		public bool CanLogin
		{
			get
			{
				bool canLogIn = false;

				if (UserName?.Length > 0 && Password?.Length > 0)
					canLogIn = true;

				return canLogIn;
			}
		}

		public LoginViewModel(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task Login()
		{
			try
			{
				AuthenticatedUser aUser = await _apiHelper.Authenticate(UserName, Password);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
			}
		}
	}
}
