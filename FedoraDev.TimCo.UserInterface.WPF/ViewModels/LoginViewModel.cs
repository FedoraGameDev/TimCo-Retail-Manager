using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.ViewModels
{
	public class LoginViewModel : Screen
	{
		private string _userName;
		private string _password;
		private readonly IAPIHelper _apiHelper;
		private string _errorMessage;

		public bool IsErrorVisible => ErrorMessage?.Length > 0;
		public bool CanLogin => UserName?.Length > 0 && Password?.Length > 0;

		public string ErrorMessage
		{
			get { return _errorMessage; }
			set
			{
				_errorMessage = value;
				NotifyOfPropertyChange(() => ErrorMessage);
				NotifyOfPropertyChange(() => IsErrorVisible);
			}
		}

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

		public LoginViewModel(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}

		public async Task Login()
		{
			try
			{
				ErrorMessage = string.Empty;
				AuthenticatedUser aUser = await _apiHelper.Authenticate(UserName, Password);

				await _apiHelper.SetLoggedInUserInfo(aUser.Access_Token);
			}
			catch (Exception exception)
			{
				ErrorMessage = exception.Message;
			}
		}
	}
}
