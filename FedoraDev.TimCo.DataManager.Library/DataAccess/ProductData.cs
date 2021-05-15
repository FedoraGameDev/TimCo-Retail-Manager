using FedoraDev.TimCo.DataManager.Library.Internal.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public class ProductData
	{
		private readonly IConfiguration _configuration;

		public ProductData(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public List<ProductModel> GetProducts()
		{
			SqlDataAccess sql = new SqlDataAccess(_configuration);

			var parameters = new { };

			return sql.LoadData<ProductModel, dynamic>("dbo.spProductGetAll", parameters, "TimCo-Data");
		}

		public ProductModel GetProductById(int productId)
		{
			SqlDataAccess sql = new SqlDataAccess(_configuration);

			var parameters = new { Id = productId };

			return sql.LoadData<ProductModel, dynamic>("dbo.spProductGetById", parameters, "TimCo-Data").FirstOrDefault();
		}
	}
}
