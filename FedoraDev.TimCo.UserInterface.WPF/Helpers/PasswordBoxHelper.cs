using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace FedoraDev.TimCo.UserInterface.WPF.Helpers
{
	public static class PasswordBoxHelper
    {
		public static DependencyProperty BoundPasswordProperty { get; } = DependencyProperty.RegisterAttached("BoundPassword",
				typeof(string),
				typeof(PasswordBoxHelper),
				new FrameworkPropertyMetadata(string.Empty, OnBoundPasswordChanged));

		public static string GetBoundPassword(DependencyObject dependencyObject)
        {
			if (dependencyObject is PasswordBox box)
			{
				// this funny little dance here ensures that we've hooked the
				// PasswordChanged event once, and only once.
				box.PasswordChanged -= PasswordChanged;
				box.PasswordChanged += PasswordChanged;
			}

			return (string)dependencyObject.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject dependencyObject, string value)
        {
            if (string.Equals(value, GetBoundPassword(dependencyObject)))
                return; // and this is how we prevent infinite recursion

            dependencyObject.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs eventArgs)
        {
			if (!(dependencyObject is PasswordBox box))
				return;

			box.Password = GetBoundPassword(dependencyObject);
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            SetBoundPassword(passwordBox, passwordBox.Password);

			// set cursor past the last character in the password box
			_ = passwordBox.GetType().GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(passwordBox, new object[] { passwordBox.Password.Length, 0 });
        }
    }
}
