using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class SaleData
	{
		private readonly IConfiguration _configuration;

		public SaleData(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public void SaveSale(SaleModel saleInfo, string cashierId)
		{
			List<SaleDetailDBModel> saleDetails = GenerateSaleDetails(saleInfo);
			SaleDBModel sale = GenerateSale(cashierId, saleDetails);

			SaveToDatabase(saleDetails, sale);
		}

		public List<SaleReportModel> GetSaleReport()
		{
			SqlDataAccess sql = new SqlDataAccess(_configuration);

			var parameters = new { };
			return sql.LoadData<SaleReportModel, dynamic>("dbo.spSaleReport", parameters, "TimCo-Data");
		}

		private SaleDBModel GenerateSale(string cashierId, List<SaleDetailDBModel> saleDetails)
		{
			return new SaleDBModel()
			{
				CashierId = cashierId,
				SubTotal = saleDetails.Sum(saleDetail => saleDetail.PurchasePrice),
				Tax = saleDetails.Sum(saleDetail => saleDetail.Tax)
			};
		}

		private List<SaleDetailDBModel> GenerateSaleDetails(SaleModel saleInfo)
		{
			ProductData product = new ProductData(_configuration);

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

			return saleDetails;
		}

		private void SaveToDatabase(List<SaleDetailDBModel> saleDetails, SaleDBModel sale)
		{
			using (SqlDataAccess sql = new SqlDataAccess(_configuration))
			{
				try
				{
					sql.StartTransaction("TimCo-Data");
					sql.SaveDataInTransaction("dbo.spSaleInsert", sale);

					var parameters = new { sale.CashierId, sale.SaleDate };
					sale.Id = sql.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", parameters).FirstOrDefault();

					foreach (SaleDetailDBModel saleDetail in saleDetails)
					{
						saleDetail.SaleId = sale.Id;
						sql.SaveDataInTransaction("dbo.spSaleDetailInsert", saleDetail);
					}

					sql.CommitTransaction();
				}
				catch
				{
					sql.RollbackTransaction();
					throw;
				}
			}
		}
	}
}
