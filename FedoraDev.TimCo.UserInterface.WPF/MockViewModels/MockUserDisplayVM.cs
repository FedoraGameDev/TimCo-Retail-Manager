using FedoraDev.TimCo.UserInterface.WPF.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace FedoraDev.TimCo.UserInterface.WPF.MockViewModels
{
	public class MockUserDisplayVM
	{
		public ObservableCollection<UserWPFModel> Users { get; set; }

		public MockUserDisplayVM()
		{
			UserWPFModel mock1 = new UserWPFModel() {
				EmailAddress = "some@guy.com",
				Roles = new Dictionary<string, string>() {
					{ "0", "Cashier" },
					{ "1", "Admin" }
				}
			};

			UserWPFModel mock2 = new UserWPFModel() {
				EmailAddress = "whata@man.com",
				Roles = new Dictionary<string, string>() {
					{ "2", "Manager" }
				}
			};

			Users = new ObservableCollection<UserWPFModel>() { mock1, mock2 };
		}
	}
}
