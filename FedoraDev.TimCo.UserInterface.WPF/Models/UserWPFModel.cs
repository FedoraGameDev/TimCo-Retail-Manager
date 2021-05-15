using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FedoraDev.TimCo.UserInterface.WPF.Models
{
	public class UserWPFModel : INotifyPropertyChanged
	{
		public string Id { get; set; }
		public string EmailAddress { get; set; }
		public Dictionary<string, string> Roles { get; set; }
		public string RoleList => string.Join(", ", Roles.Select(role => role.Value));

		public event PropertyChangedEventHandler PropertyChanged;

		public void CallPropertyChanged(string propertyName) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		public void AddRole(string roleId, string roleName)
		{
			Roles.Add(roleId, roleName);
			CallPropertyChanged(nameof(Roles));
			CallPropertyChanged(nameof(RoleList));
		}

		public void RemoveRole(string roleId)
		{
			Roles.Remove(roleId);
			CallPropertyChanged(nameof(Roles));
			CallPropertyChanged(nameof(RoleList));
		}
	}
}
