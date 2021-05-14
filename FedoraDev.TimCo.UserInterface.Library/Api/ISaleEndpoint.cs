using FedoraDev.TimCo.UserInterface.Library.Models;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Api
{
	public interface ISaleEndpoint
	{
		Task PostSale(SaleModel sale);
	}
}