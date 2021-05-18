using System.ComponentModel.DataAnnotations;

namespace FedoraDev.TimCo.UserInterface.Portal.Models
{
	public class AuthenticationUserModel
	{
		[Required(ErrorMessage = "Email Address is required.")]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }
	}
}
