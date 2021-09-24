using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace FedoraDev.TimCo.UserInterface.WPF.Controls
{
	public partial class WindowControl : UserControl
	{
        private Window _window;

		public WindowControl()
		{
			InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this))
                return;
            _window = Application.Current.MainWindow;
            _window.StateChanged += WindowStateChanged;
            WindowStateChanged(this, null);
        }

        private void RefreshMaximizeRestoreButton()
        {
            if (_window.WindowState == WindowState.Maximized)
            {
                maximizeButton.Visibility = Visibility.Collapsed;
                restoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                maximizeButton.Visibility = Visibility.Visible;
                restoreButton.Visibility = Visibility.Collapsed;
            }
        }

        public void OnMinimizeButtonClick(object sender, RoutedEventArgs e) => _window.WindowState = WindowState.Minimized;
        public void OnMaximizeRestoreButtonClick(object sender, RoutedEventArgs e) => _window.WindowState = _window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        public void OnCloseButtonClick(object sender, RoutedEventArgs e) => _window.Close();
        public void WindowStateChanged(object sender, EventArgs e) => RefreshMaximizeRestoreButton();
    }
}
