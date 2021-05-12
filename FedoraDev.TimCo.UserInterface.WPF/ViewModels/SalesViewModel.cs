using Caliburn.Micro;
using System.ComponentModel;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class SalesViewModel : Screen
	{
		private BindingList<string> _cart;
		private BindingList<string> _products;
		private int _itemQuantity;

		public string SubTotal => "$0.00";
		public string Tax => "$0.00";
		public string Total => "$0.00";
		public bool CanAddToCart => ItemQuantity > 0;
		public bool CanRemoveFromCart => false;
		public bool CanCheckout => false;

		public BindingList<string> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public BindingList<string> Products
		{
			get { return _products; }
			set {
				_products = value;
				NotifyOfPropertyChange(() => Products);
			}
		}

		public int ItemQuantity
		{
			get { return _itemQuantity; }
			set
			{
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public void AddToCart()
		{
			//
		}

		public void RemoveFromCart()
		{
			//
		}

		public void Checkout()
		{
			//
		}
	}
}
