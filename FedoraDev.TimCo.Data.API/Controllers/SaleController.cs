using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Security.Claims;

namespace FedoraDev.TimCo.Data.API.Controllers
{
	[Authorize]
    [Route("api/[controller]")]
	[ApiController]
	public class SaleController : ControllerBase
	{
		private readonly IConfiguration _configuration;

		public SaleController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

        [HttpPost]
        [Authorize(Roles = Roles.CASHIER)]
        public void Post(SaleModel sale)
        {
            SaleData saleData = new SaleData(_configuration);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            saleData.SaveSale(sale, userId);
        }

        [HttpGet, Route("GetSalesReport")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public List<SaleReportModel> GetSalesReport()
        {
            SaleData saleData = new SaleData(_configuration);

            return saleData.GetSaleReport();
        }
    }
}
