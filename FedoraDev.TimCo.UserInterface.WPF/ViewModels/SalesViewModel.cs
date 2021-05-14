using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
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
		private readonly ISaleEndpoint _saleEndpoint;
		private readonly IConfigHelper _configHelper;
		private BindingList<CartItemModel> _cart;
		private BindingList<ProductModel> _products;
		private int _itemQuantity = 1;
		private ProductModel _selectedProduct;
		#endregion

		#region Properties
		public string SubTotal => CalculateSubTotal().ToString("C");
		public string Tax => CalculateTax().ToString("C");
		public string Total => CalculateTotal().ToString("C");
		public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
		public bool CanRemoveFromCart => false;
		public bool CanCheckout => Cart.Count > 0;

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

		#region Initialization
		public SalesViewModel(IProductEndpoint productEndpoint, ISaleEndpoint saleEndpoint, IConfigHelper configHelper)
		{
			_productEndpoint = productEndpoint;
			_saleEndpoint = saleEndpoint;
			_configHelper = configHelper;
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
		#endregion

		#region WPF Buttons
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
			NotifyOfPropertyChange(() => CanCheckout);
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
		}

		public void RemoveFromCart()
		{
			NotifyOfPropertyChange(() => CanCheckout);
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
		}

		public async Task Checkout()
		{
			SaleModel sale = new SaleModel();

			foreach (CartItemModel cartItem in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel()
				{
					ProductId = cartItem.Product.Id,
					Quantity = cartItem.QuantityInCart
				});
			}

			await _saleEndpoint.PostSale(sale);
		}
		#endregion

		#region Property Helpers
		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;
			foreach (CartItemModel item in Cart)
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			return subTotal;
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = _configHelper.GetTaxRate();

			taxAmount = Cart
				.Where(item => item.Product.Taxable)
				.Sum(item => item.Product.RetailPrice * item.QuantityInCart * taxRate);

			return Math.Round(taxAmount + 0.005m, 2);
		}

		private decimal CalculateTotal()
		{
			return CalculateSubTotal() + CalculateTax();
		}
		#endregion
	}
}
