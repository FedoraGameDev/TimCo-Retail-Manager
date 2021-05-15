using FedoraDev.TimCo.UserInterface.Library.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FedoraDev.TimCo.UserInterface.WPF.MockViewModels
{
	public class MockUserDisplayVM
	{
		public ObservableCollection<UserModel> Users { get; set; }

		public MockUserDisplayVM()
		{
			UserModel mock1 = new UserModel() {
				EmailAddress = "some@guy.com",
				Roles = new Dictionary<string, string>() {
					{ "0", "Cashier" },
					{ "1", "Admin" }
				}
			};

			UserModel mock2 = new UserModel() {
				EmailAddress = "whata@man.com",
				Roles = new Dictionary<string, string>() {
					{ "2", "Manager" }
				}
			};

			Users = new ObservableCollection<UserModel>() { mock1, mock2 };
		}
	}
}
