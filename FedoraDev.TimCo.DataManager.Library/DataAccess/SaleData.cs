using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class SaleData : ISaleData
	{
		private readonly ISqlDataAccess _sqlDataAccess;
		private readonly IProductData _productData;
		private readonly IConfiguration _configuration;

		public SaleData(ISqlDataAccess sqlDataAccess, IProductData productData, IConfiguration configuration)
		{
			_sqlDataAccess = sqlDataAccess;
			_productData = productData;
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
			var parameters = new { };
			return _sqlDataAccess.LoadData<SaleReportModel, dynamic>("TimCo-Data", "dbo.spSaleReport", parameters);
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

		public decimal GetTaxRate()
		{
			bool isValid = decimal.TryParse(_configuration.GetValue<string>("TaxRate"), out decimal taxRate);

			if (!isValid)
				throw new ConfigurationErrorsException("The tax rate is not a valid double value. It should be in the form of '8.75'");

			return taxRate / 100m;
		}

		private List<SaleDetailDBModel> GenerateSaleDetails(SaleModel saleInfo)
		{
			List<SaleDetailDBModel> saleDetails = new List<SaleDetailDBModel>();
			foreach (SaleDetailModel saleDetail in saleInfo.SaleDetails)
			{
				ProductModel productInfo = _productData.GetProductById(saleDetail.ProductId);

				if (productInfo == null)
					throw new Exception($"The ProductId '{saleDetail.ProductId}' could not be found in the database.");

				decimal purchasePrice = productInfo.RetailPrice * saleDetail.Quantity;
				decimal tax = purchasePrice * (productInfo.Taxable ? GetTaxRate() : 0m);

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
			try
			{
				_sqlDataAccess.StartTransaction("TimCo-Data");
				_sqlDataAccess.SaveDataInTransaction("dbo.spSaleInsert", sale);

				var parameters = new { sale.CashierId, sale.SaleDate };
				sale.Id = _sqlDataAccess.LoadDataInTransaction<int, dynamic>("dbo.spSaleLookup", parameters).FirstOrDefault();

				foreach (SaleDetailDBModel saleDetail in saleDetails)
				{
					saleDetail.SaleId = sale.Id;
					_sqlDataAccess.SaveDataInTransaction("dbo.spSaleDetailInsert", saleDetail);
				}

				_sqlDataAccess.CommitTransaction();
			}
			catch
			{
				_sqlDataAccess.RollbackTransaction();
				throw;
			}
		}
	}
}
