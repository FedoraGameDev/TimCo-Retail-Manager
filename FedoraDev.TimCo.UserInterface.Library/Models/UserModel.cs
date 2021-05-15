using System.Collections.Generic;
using System.Linq;

namespace FedoraDev.TimCo.UserInterface.Library.Models
{
	public class UserModel
	{
		public string Id { get; set; }
		public string EmailAddress { get; set; }
		public Dictionary<string, string> Roles { get; set; }
		public string RoleList => string.Join(", ", Roles.Select(role => role.Value));
	}
}
