﻿namespace FedoraDev.TimCo.UserInterface.Library.Models
{
	public class ProductModel
	{
		public int Id { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public decimal RetailPrice { get; set; }
		public bool Taxable { get; set; }
		public int QuantityInStock { get; set; }
	}
}
