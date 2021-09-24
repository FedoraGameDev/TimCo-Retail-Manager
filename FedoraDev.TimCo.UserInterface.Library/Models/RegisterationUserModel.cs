using System.ComponentModel.DataAnnotations;

namespace FedoraDev.TimCo.UserInterface.Library.Models
{
	public class RegisterationUserModel
	{
		[Required(ErrorMessage = "First Name is required.")]
		public string FirstName { get; set; }

		[Required(ErrorMessage = "Last Name is required.")]
		public string LastName { get; set; }

		[Required(ErrorMessage = "Email Address is required.")]
		[EmailAddress]
		public string EmailAddress { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password.")]
		[Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
		public string ConfirmPassword { get; set; }
	}
}
