using System.Collections.Generic;

namespace FedoraDev.TimCo.UserInterface.Library.Models
{
	public class SaleModel
	{
		public List<SaleDetailModel> SaleDetails { get; set; } = new List<SaleDetailModel>();
	}
}
