using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize(Roles = Roles.CASHIER)]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public ProductController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		[HttpGet]
		public IEnumerable<ProductModel> Get()
		{
			ProductData productData = new ProductData(_configuration);

			return productData.GetProducts();
		}
	}
}
