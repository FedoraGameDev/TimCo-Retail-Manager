using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class SalesViewModel : Screen
	{
		private IProductEndpoint _productEndpoint;
		private BindingList<ProductModel> _cart;
		private BindingList<ProductModel> _products;
		private int _itemQuantity;

		public string SubTotal => "$0.00";
		public string Tax => "$0.00";
		public string Total => "$0.00";
		public bool CanAddToCart => ItemQuantity > 0;
		public bool CanRemoveFromCart => false;
		public bool CanCheckout => false;

		public BindingList<ProductModel> Cart
		{
			get { return _cart; }
			set
			{
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public BindingList<ProductModel> Products
		{
			get { return _products; }
			set {
				_products = value;
				NotifyOfPropertyChange(() => Products);
				NotifyOfPropertyChange(() => CanAddToCart);
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

		public SalesViewModel(IProductEndpoint productEndpoint)
		{
			_productEndpoint = productEndpoint;
		}

		protected override async void OnViewLoaded(object view)
		{
			base.OnViewLoaded(view);
			await LoadProducts();
		}

		private async Task LoadProducts()
		{
			List<ProductModel> productList = await _productEndpoint.GetAll();
			Products = new BindingList<ProductModel>(productList);
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
