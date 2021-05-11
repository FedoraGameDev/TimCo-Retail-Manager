using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace FedoraDev.TimCo.UserInterface.WPF
{
	public class Bootstrapper : BootstrapperBase
	{
		private SimpleContainer _container = new SimpleContainer();

		public Bootstrapper()
		{
			Initialize();
		}

		protected override void Configure()
		{
			_ = _container.Instance(_container);

			_ = _container.Singleton<IWindowManager, WindowManager>();
			_ = _container.Singleton<IEventAggregator, EventAggregator>();

			RegisterViewModels();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			DisplayRootViewFor<ShellViewModel>();
		}

		protected override object GetInstance(Type service, string key)
		{
			return _container.GetInstance(service, key);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.GetAllInstances(service);
		}

		protected override void BuildUp(object instance)
		{
			_container.BuildUp(instance);
		}

		void RegisterViewModels()
		{
			IEnumerable<Type> types = GetType().Assembly.GetTypes();
			types = types.Where(type => type.IsClass);
			types = types.Where(type => type.Name.EndsWith("ViewModel"));

			types.ToList().ForEach(viewModelType =>
				_container.RegisterPerRequest(
					viewModelType,
					viewModelType.ToString(),
					viewModelType
				)
			);
		}
	}
}
