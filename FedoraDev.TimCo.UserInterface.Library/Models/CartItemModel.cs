namespace FedoraDev.TimCo.UserInterface.Library.Models
{
	public class CartItemModel
	{
		public ProductModel Product { get; set; }
		public int QuantityInCart { get; set; }
		public string DisplayText => $"{QuantityInCart}x {Product.ProductName}";
	}
}
