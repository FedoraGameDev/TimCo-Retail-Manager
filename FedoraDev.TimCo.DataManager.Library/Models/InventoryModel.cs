﻿using System;

namespace FedoraDev.TimCo.DataManager.Library.Models
{
	public class InventoryModel
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal PurchasePrice { get; set; }
		public DateTime PurchaseDate { get; set; }
	}
}
