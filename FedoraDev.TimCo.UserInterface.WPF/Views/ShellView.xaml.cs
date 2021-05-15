using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FedoraDev.TimCo.UserInterface.WPF.Views
{
	/// <summary>
	/// Interaction logic for ShellView.xaml
	/// </summary>
	public partial class ShellView : Window
	{
		public ShellView()
		{
			InitializeComponent();
			this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
		}

		public void WindowSizeChanged(object sender, SizeChangedEventArgs args) => BorderThickness = WindowState == WindowState.Maximized ? new Thickness(7) : new Thickness(0);
	}
}
