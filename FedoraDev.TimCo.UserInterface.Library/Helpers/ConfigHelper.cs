using System;
using System.Configuration;

namespace FedoraDev.TimCo.UserInterface.Library.Helpers
{
	public class ConfigHelper : IConfigHelper
	{
		public decimal GetTaxRate()
		{
			bool isValid = decimal.TryParse(ConfigurationManager.AppSettings["taxRate"], out decimal taxRate);

			if (!isValid)
				throw new ConfigurationErrorsException("The tax rate is not a valid double value. It should be in the form of '0.00'");

			return taxRate / 100m;
		}
	}
}
