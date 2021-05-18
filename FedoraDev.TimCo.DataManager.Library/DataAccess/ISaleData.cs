using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public interface ISaleData
	{
		List<SaleReportModel> GetSaleReport();
		void SaveSale(SaleModel saleInfo, string cashierId);
		decimal GetTaxRate();
	}
}