using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;

namespace FedoraDev.TimCo.DataManager.Library.DataAccess
{
	public interface IProductData
	{
		ProductModel GetProductById(int productId);
		List<ProductModel> GetProducts();
	}
}