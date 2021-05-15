using AutoMapper;
using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Helpers;
using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class SalesViewModel : Screen
	{
		#region Fields
		private BindingList<CartItemWPFModel> _cart;
		private BindingList<ProductWPFModel> _products;
		private int _itemQuantity = 1;
		private ProductWPFModel _selectedProduct;
		private CartItemWPFModel _selectedCartItem;
		#endregion

		#region Properties
		public string SubTotal => CalculateSubTotal().ToString("C");
		public string Tax => CalculateTax().ToString("C");
		public string Total => CalculateTotal().ToString("C");
		public bool CanAddToCart => ItemQuantity > 0 && SelectedProduct?.QuantityInStock >= ItemQuantity;
		public bool CanRemoveFromCart => ItemQuantity > 0 && SelectedCartItem?.QuantityInCart >= ItemQuantity;
		public bool CanCheckout => Cart.Count > 0;

		public BindingList<CartItemWPFModel> Cart
		{
			get { return _cart; }
			set {
				_cart = value;
				NotifyOfPropertyChange(() => Cart);
			}
		}

		public BindingList<ProductWPFModel> Products
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
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}

		public ProductWPFModel SelectedProduct
		{
			get { return _selectedProduct; }
			set {
				_selectedProduct = value;
				NotifyOfPropertyChange(() => SelectedProduct);
				NotifyOfPropertyChange(() => CanAddToCart);
			}
		}

		public CartItemWPFModel SelectedCartItem
		{
			get { return _selectedCartItem; }
			set {
				_selectedCartItem = value;
				NotifyOfPropertyChange(() => SelectedCartItem);
				NotifyOfPropertyChange(() => CanRemoveFromCart);
			}
		}
		#endregion

		#region Life Cycle
		public SalesViewModel()
		{
			Cart = new BindingList<CartItemWPFModel>();
		}

		protected override async void OnViewLoaded(object view)
		{
			try
			{
				base.OnViewLoaded(view);
				await LoadProducts();
			}
			catch (Exception ex)
			{
				StatusInfoViewModel infoViewModel = IoC.Get<StatusInfoViewModel>();
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;

				if (ex.Message == "Unauthorizedd")
				{
					settings.Title = "Authorization Error";

					infoViewModel.UpdateMessage("Unauthorized Access", "You are not authorized to interact with the sales form.");
					IoC.Get<IWindowManager>().ShowDialog(infoViewModel, null, settings);
					TryClose(); 
				}
				else
				{
					settings.Title = "Fatal Exception";

					infoViewModel.UpdateMessage("Fatal Exception", ex.Message);
					IoC.Get<IWindowManager>().ShowDialog(infoViewModel, null, settings);
				}
			}
		}

		private async Task LoadProducts()
		{
			List<ProductModel> productList = await IoC.Get<IProductEndpoint>().GetAll();
			List<ProductWPFModel> products = IoC.Get<IMapper>().Map<List<ProductWPFModel>>(productList);
			Products = new BindingList<ProductWPFModel>(products);
		}

		private async Task ResetSalesViewModel()
		{
			Cart.Clear();
			await LoadProducts();

			NotifyOfPropertyChange(() => CanCheckout);
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
		}
		#endregion

		#region WPF Buttons
		public void AddToCart()
		{
			CartItemWPFModel existingItem = Cart.FirstOrDefault(x => x.Product == SelectedProduct);
			if (existingItem != null)
			{
				existingItem.QuantityInCart += ItemQuantity;
			}
			else
			{
				CartItemWPFModel cartItem = new CartItemWPFModel()
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
			SelectedCartItem.Product.QuantityInStock += ItemQuantity;

			if (SelectedCartItem.QuantityInCart > ItemQuantity)
				SelectedCartItem.QuantityInCart -= ItemQuantity;
			else
				_ = Cart.Remove(SelectedCartItem);

			ItemQuantity = 1;
			NotifyOfPropertyChange(() => CanCheckout);
			NotifyOfPropertyChange(() => SubTotal);
			NotifyOfPropertyChange(() => Tax);
			NotifyOfPropertyChange(() => Total);
		}

		public async Task Checkout()
		{
			SaleModel sale = new SaleModel();

			foreach (CartItemWPFModel cartItem in Cart)
			{
				sale.SaleDetails.Add(new SaleDetailModel()
				{
					ProductId = cartItem.Product.Id,
					Quantity = cartItem.QuantityInCart
				});
			}

			await IoC.Get<ISaleEndpoint>().PostSale(sale);
			await ResetSalesViewModel();
		}
		#endregion

		#region Property Helpers
		private decimal CalculateSubTotal()
		{
			decimal subTotal = 0;
			foreach (CartItemWPFModel item in Cart)
				subTotal += item.Product.RetailPrice * item.QuantityInCart;
			return subTotal;
		}

		private decimal CalculateTax()
		{
			decimal taxAmount = 0;
			decimal taxRate = IoC.Get<IConfigHelper>().GetTaxRate();

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
