using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class SaleData
	{
		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			ProductData product = new ProductData();

			List<SaleDetailDBModel> saleDetails = new List<SaleDetailDBModel>();
			foreach (SaleDetailModel saleDetail in saleInfo.SaleDetails)
			{
				ProductModel productInfo = product.GetProductById(saleDetail.ProductId);

				if (productInfo == null)
					throw new Exception($"The ProductId '{saleDetail.ProductId}' could not be found in the database.");

				decimal purchasePrice = productInfo.RetailPrice * saleDetail.Quantity;
				decimal tax = purchasePrice * (productInfo.Taxable ? ConfigHelper.GetTaxRate() : 0m);

				saleDetails.Add(new SaleDetailDBModel()
				{
					ProductId = saleDetail.ProductId,
					Quantity = saleDetail.Quantity,
					PurchasePrice = purchasePrice,
					Tax = tax
				});
			}

			SaleDBModel sale = new SaleDBModel()
			{
				CashierId = cashierId,
				SubTotal = saleDetails.Sum(saleDetail => saleDetail.PurchasePrice),
				Tax = saleDetails.Sum(saleDetail => saleDetail.Tax)
			};

			SqlDataAccess sql = new SqlDataAccess();
			sql.SaveData("dbo.spSaleInsert", sale, "TimCo-Data");

			var parameters = new { sale.CashierId, sale.SaleDate };

			sale.Id = sql.LoadData<int, dynamic>("dbo.spSaleLookup", parameters, "TimCo-Data").FirstOrDefault();

			foreach (SaleDetailDBModel saleDetail in saleDetails)
			{
				saleDetail.SaleId = sale.Id;
				sql.SaveData("dbo.spSaleDetailInsert", saleDetail, "TimCo-Data");
			}
		}
	}
}
