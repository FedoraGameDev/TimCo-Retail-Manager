using System.ComponentModel;

namespace FedoraDev.TimCo.UserInterface.WPF.Models
{
	public class CartItemWPFModel : INotifyPropertyChanged
	{
		private int _quantityInCart;

		public ProductWPFModel Product { get; set; }
		public int QuantityInCart {
			get => _quantityInCart;
			set {
				_quantityInCart = value;
				CallPropertyChanged(nameof(QuantityInCart));
				CallPropertyChanged(nameof(DisplayText));
			}
		}
		public string DisplayText => $"{QuantityInCart}x {Product.ProductName}";

		public event PropertyChangedEventHandler PropertyChanged;

		public void CallPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
