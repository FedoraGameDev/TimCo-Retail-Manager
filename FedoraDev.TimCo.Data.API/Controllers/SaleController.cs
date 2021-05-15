using FedoraDev.TimCo.DataManager.Library.Const;
using FedoraDev.TimCo.DataManager.Library.DataAccess;
using FedoraDev.TimCo.DataManager.Library.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web.Http;

namespace FedoraDev.TimCo.DataManager.Controllers
{
	[Authorize]
	public class SaleController : ApiController
    {
        [HttpPost]
        [Authorize(Roles = Roles.CASHIER)]
        public void Post(SaleModel sale)
        {
            SaleData saleData = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();

            saleData.SaveSale(sale, userId);
		}

        [HttpGet, Route("GetSalesReport")]
        [Authorize(Roles = Roles.ADMIN_AND_MANAGER)]
        public List<SaleReportModel> GetSalesReport()
		{
            SaleData saleData = new SaleData();

            return saleData.GetSaleReport();
		}
    }
}
