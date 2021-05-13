using FedoraDev.TimCo.UserInterface.Library.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FedoraDev.TimCo.UserInterface.Library.Api
{
	public interface IProductEndpoint
	{
		Task<List<ProductModel>> GetAll();
	}
}