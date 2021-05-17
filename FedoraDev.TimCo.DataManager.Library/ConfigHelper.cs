using System.Configuration;

namespace FedoraDev.TimCo.DataManager.Library
{
	public class ConfigHelper
	{
		public static decimal GetTaxRate()
		{
			bool isValid = decimal.TryParse(ConfigurationManager.AppSettings.Get("TaxRate"), out decimal taxRate);

			if (!isValid)
				throw new ConfigurationErrorsException("The tax rate is not a valid double value. It should be in the form of '0.00'");

			return taxRate / 100m;
		}
	}
}
