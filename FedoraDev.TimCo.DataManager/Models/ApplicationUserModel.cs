using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FedoraDev.TimCo.DataManager.Models
{
	public class ApplicationUserModel
	{
		public string Id { get; set; }
		public string EmailAddress { get; set; }
		public Dictionary<string, string> Roles { get; set; } = new Dictionary<string, string>();
	}
}