using System.ComponentModel;

namespace FedoraDev.TimCo.UserInterface.WPF.Models
{
	public class ProductWPFModel : INotifyPropertyChanged
	{
		private int _quantityInStock;

		public int Id { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public decimal RetailPrice { get; set; }
		public bool Taxable { get; set; }
		public int QuantityInStock {
			get => _quantityInStock;
			set {
				_quantityInStock = value;
				CallPropertyChanged(nameof(QuantityInStock));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void CallPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
