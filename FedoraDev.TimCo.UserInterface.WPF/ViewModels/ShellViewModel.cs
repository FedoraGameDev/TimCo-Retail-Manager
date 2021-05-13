using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LoginEvent>
	{
		private SalesViewModel _salesVM;
		private IEventAggregator _events;

		public ShellViewModel(SalesViewModel salesVM, IEventAggregator events)
		{
			_salesVM = salesVM;
			_events = events;

			_events.Subscribe(this);
			ActivateItem(IoC.Get<LoginViewModel>());
		}

		public void Handle(LoginEvent message)
		{
			ActivateItem(_salesVM);
		}
	}
}
