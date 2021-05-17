using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class LoginViewModel : Screen
	{
		#region Fields
#if DEBUG
		private string _userName = "dude@place.com";
		private string _password = "Pwd12345.";
#else
		private string _userName;
		private string _password;
#endif
		private string _errorMessage;
		#endregion

		#region Properties
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
		#endregion

		public async Task Login()
		{
			try
			{
				ErrorMessage = string.Empty;
				IAPIHelper apiHelper = IoC.Get<IAPIHelper>();
				AuthenticatedUser aUser = await apiHelper.Authenticate(UserName, Password);

				await apiHelper.SetLoggedInUserInfo(aUser.Access_Token);

				await IoC.Get<IEventAggregator>().PublishOnUIThreadAsync(new LoginEvent(), new CancellationToken());
				;
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
