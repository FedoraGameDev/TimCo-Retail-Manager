using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;
using System;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class LoginViewModel : Screen
	{
#if DEBUG
		private string _userName = "dude@place.com";
		private string _password = "Pwd12345.";
#else
		private string _userName;
		private string _password;
#endif
		private IAPIHelper _apiHelper;
		private IEventAggregator _events;
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

		public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
		{
			_apiHelper = apiHelper;
			_events = events;
		}

		public async Task Login()
		{
			try
			{
				ErrorMessage = string.Empty;
				AuthenticatedUser aUser = await _apiHelper.Authenticate(UserName, Password);

				await _apiHelper.SetLoggedInUserInfo(aUser.Access_Token);

				_events.PublishOnUIThread(new LoginEvent());
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.Message);
				Console.WriteLine(exception.StackTrace);
				ErrorMessage = exception.Message;
			}
		}
	}
}
