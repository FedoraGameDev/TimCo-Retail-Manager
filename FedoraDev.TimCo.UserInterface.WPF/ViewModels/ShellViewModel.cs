using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.WPF.EventModels;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class ShellViewModel : Conductor<object>, IHandle<LoginEvent>
	{
		private SalesViewModel _salesVM;
		private IEventAggregator _events;
		private SimpleContainer _container;

		public ShellViewModel(SalesViewModel salesVM, IEventAggregator events, SimpleContainer container)
		{
			_salesVM = salesVM;
			_events = events;
			_container = container;

			_events.Subscribe(this);
			ActivateItem(_container.GetInstance<LoginViewModel>());
		}

		public void Handle(LoginEvent message)
		{
			ActivateItem(_salesVM);
		}
	}
}
