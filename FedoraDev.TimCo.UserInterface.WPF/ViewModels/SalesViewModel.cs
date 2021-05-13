using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class SalesViewModel : Screen
	{
		#region Fields
		private IProductEndpoint _productEndpoint;
		private BindingList<CartItemModel> _cart;
		private BindingList<ProductModel> _products;
		private int _itemQuantity = 1;
		private ProductModel _selectedProduct;
		#endregion

		#region Properties
		public string SubTotal => GetSubTotal().ToString("C");
		public string Tax => GetTax().ToString("C");
		public string Total => GetTotal().ToString("C");
		public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
		public bool CanRemoveFromCart => false;
		public bool CanCheckout => false;

		public BindingList<CartItemModel> Cart
		{
			get { return _cart; }
			set {
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
			set {
				_itemQuantity = value;
				NotifyOfPropertyChange(() => ItemQuantity);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public ProductModel SelectedProduct
		{
			get { return _selectedProduct; }
			set {
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}
		#endregion

		public SalesViewModel(IProductEndpoint productEndpoint)
		{
			_productEndpoint = productEndpoint;
			Cart = new BindingList<CartItemModel>();
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
			CartItemModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;

				// HACK - Need to notify of sub-property change within Cart (QuantityInCart)
				_ = Cart.Remove(existingItem);
				Cart.Add(existingItem);
			}
			else
			{
				CartItemModel cartItem = new CartItemModel()
				{
					Product = SelectedProduct,
					QuantityInCart = ItemQuantity
				};
				Cart.Add(cartItem);
			}

			SelectedProduct.QuantityInStock -= ItemQuantity;
			ItemQuantity = 1;
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
		}

		public void RemoveFromCart()
		{
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
		}

		public void Checkout()
		{
			//
		}

		public decimal GetSubTotal()
		{
			decimal subTotal = 0;
			foreach (CartItemModel item in Cart)
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			return subTotal;
		}

		public decimal GetTax()
		{
			return 0;
		}

		public decimal GetTotal()
		{
			return GetSubTotal() + GetTax();
		}
	}
}
