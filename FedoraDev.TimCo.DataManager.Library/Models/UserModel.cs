﻿using System;

namespace FedoraDev.TimCo.DataManager.Library.Models
{
	public class UserModel
	{
		public string Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public DateTime CreatedDate { get; set; }
	}
}
