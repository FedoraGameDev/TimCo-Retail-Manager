using FedoraDev.TimCo.UserInterface.Library.Models;
using FedoraDev.TimCo.UserInterface.WPF.Models;
using System.Collections.ObjectModel;

namespace FedoraDev.TimCo.UserInterface.WPF.MockViewModels
{
	public class MockSalesVM
	{
		public ObservableCollection<ProductWPFModel> Products { get; set; }
		public ObservableCollection<CartItemWPFModel> Cart { get; set; }

		public MockSalesVM()
		{
			ProductWPFModel productMock1 = new ProductWPFModel()
			{
				ProductName = "Cool Item Bro",
				RetailPrice = 15.32m,
				QuantityInStock = 30
			};

			ProductWPFModel productMock2 = new ProductWPFModel()
			{
				ProductName = "Another Item",
				RetailPrice = 102.99m,
				QuantityInStock = 5
			};

			CartItemWPFModel cartItemMock1 = new CartItemWPFModel()
			{
				Product = productMock1,
				QuantityInCart = 3
			};

			Products = new ObservableCollection<ProductWPFModel>() { productMock1, productMock2 };
			Cart = new ObservableCollection<CartItemWPFModel>() { cartItemMock1 };
		}
	}
}
