using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize(Roles = Roles.CASHIER)]
	[Route("api/[controller]")]
	[ApiController]
	public class ProductController : ControllerBase
	{
		private readonly IServiceProvider _serviceProvider;

		public ProductController(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		[HttpGet]
		public IEnumerable<ProductModel> Get()
		{
			IProductData productData = _serviceProvider.GetRequiredService<IProductData>();

			return productData.GetProducts();
		}
	}
}
