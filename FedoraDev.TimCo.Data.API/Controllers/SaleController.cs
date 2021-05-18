using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class SaleController : ControllerBase
	{
		private readonly IServiceProvider _serviceProvider;

		public SaleController(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

        [HttpPost]
        [Authorize(Roles = Roles.CASHIER)]
        public void Post(SaleModel sale)
        {
            ISaleData saleData = _serviceProvider.GetRequiredService<ISaleData>();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            saleData.SaveSale(sale, userId);
        }

        [HttpGet, Route("GetSalesReport")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public List<SaleReportModel> GetSalesReport()
        {
            ISaleData saleData = _serviceProvider.GetRequiredService<ISaleData>();

            return saleData.GetSaleReport();
        }

        [HttpGet, Route("GetTaxRate")]
        [AllowAnonymous]
        public decimal GetTaxRate()
		{
            ISaleData saleData = _serviceProvider.GetRequiredService<ISaleData>();

            return saleData.GetTaxRate();
		}
    }
}
