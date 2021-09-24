using Caliburn.Micro;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class StatusInfoViewModel : Screen
	{
		public string Header { get; private set; }
		public string Message { get; private set; }
		
		public void UpdateMessage(string header, string message)
		{
			Header = header;
			Message = message;

			NotifyOfPropertyChange(() => Header);
			NotifyOfPropertyChange(() => Message);
		}

		public async Task Close()
		{
			await TryCloseAsync();
		}
	}
}
