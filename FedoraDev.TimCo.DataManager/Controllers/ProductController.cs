using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace FedoraDev.TimCo.DataManager.Controllers
{
	[Authorize]
	public class ProductController : ApiController
	{
		[HttpGet]
		public IEnumerable<ProductModel> Get()
		{
			ProductData productData = new ProductData();

			return productData.GetProducts();
		}
	}
}