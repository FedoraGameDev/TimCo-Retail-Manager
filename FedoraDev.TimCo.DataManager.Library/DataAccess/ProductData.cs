using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class ProductData : IProductData
	{
		private readonly ISqlDataAccess _sqlDataAccess;

		public ProductData(ISqlDataAccess sqlDataAccess)
		{
			_sqlDataAccess = sqlDataAccess;
		}

		public List<ProductModel> GetProducts()
		{
			var parameters = new { };
			return _sqlDataAccess.LoadData<ProductModel, dynamic>("TimCo-Data", "dbo.spProductGetAll", parameters);
		}

		public ProductModel GetProductById(int productId)
		{
			var parameters = new { Id = productId };
			return _sqlDataAccess.LoadData<ProductModel, dynamic>("TimCo-Data", "dbo.spProductGetById", parameters).FirstOrDefault();
		}
	}
}
