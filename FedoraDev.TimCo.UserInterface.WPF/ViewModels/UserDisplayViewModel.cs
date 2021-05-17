using Caliburn.Micro;
using FedoraDev.TimCo.UserInterface.Library.Api;
using FedoraDev.TimCo.UserInterface.Library.Models;
using System;
using System.Linq;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.Generic;
using FedoraDev.TimCo.UserInterface.WPF.Models;
using AutoMapper;

namespace FedoraDev.TimCo.UserInterface.WPF.ViewModels
{
	public class UserDisplayViewModel : Screen
	{
		#region Fields
		private BindingList<UserWPFModel> _users;
		private UserWPFModel _selectedUser;
		private string _selectedUserName;
		private BindingList<string> _selectedUserRoles;
		private BindingList<string> _availableRoles;
		private string _roleToAdd;
		private string _roleToRemove;
		private Dictionary<string, string> _allRoles;
		#endregion

		#region Properties
		public bool CanAddSelectedRole => SelectedUser != null && RoleToAdd != null;
		public bool CanRemoveSelectedRole => SelectedUser != null && RoleToRemove != null;

		public BindingList<UserWPFModel> Users
		{
			get => _users;
			set
			{
				_users = value;
				NotifyOfPropertyChange(() => Users);
			}
		}

		public UserWPFModel SelectedUser
		{
			get => _selectedUser;
			set
			{
				_selectedUser = value;
				SelectedUserName = value.EmailAddress;
				SelectedUserRoles = new BindingList<string>(value.Roles.Select(role => role.Value).ToList());
				AvailableRoles = new BindingList<string>(_allRoles.Where(role => !SelectedUserRoles.Contains(role.Value)).Select(role => role.Value).ToList());
				NotifyOfPropertyChange(() => SelectedUser);
				NotifyOfPropertyChange(() => CanAddSelectedRole);
				NotifyOfPropertyChange(() => CanRemoveSelectedRole);
			}
		}

		public string SelectedUserName
		{
			get => _selectedUserName;
			set
			{
				_selectedUserName = value;
				NotifyOfPropertyChange(() => SelectedUserName);
			}
		}

		public BindingList<string> SelectedUserRoles
		{
			get => _selectedUserRoles;
			set
			{
				_selectedUserRoles = value;
				NotifyOfPropertyChange(() => SelectedUserRoles);
			}
		}

		public BindingList<string> AvailableRoles
		{
			get => _availableRoles;
			set
			{
				_availableRoles = value;
				NotifyOfPropertyChange(() => AvailableRoles);
			}
		}

		public string RoleToAdd
		{
			get => _roleToAdd;
			set
			{
				_roleToAdd = value;
				NotifyOfPropertyChange(() => RoleToAdd);
				NotifyOfPropertyChange(() => CanAddSelectedRole);
			}
		}

		public string RoleToRemove
		{
			get => _roleToRemove;
			set
			{
				_roleToRemove = value;
				NotifyOfPropertyChange(() => RoleToRemove);
				NotifyOfPropertyChange(() => CanRemoveSelectedRole);
			}
		}
		#endregion

		#region Life Cycle
		protected override async void OnViewLoaded(object view)
		{
			try
			{
				await LoadUsers();
				await LoadRoles();
			}
			catch (Exception ex)
			{
				StatusInfoViewModel infoViewModel = IoC.Get<StatusInfoViewModel>();
				dynamic settings = new ExpandoObject();
				settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
				settings.ResizeMode = ResizeMode.NoResize;

				if (ex.Message == "Unauthorizedd")
				{
					settings.Title = "Authorization Error";

					infoViewModel.UpdateMessage("Unauthorized Access", "You are not authorized to view the users list.");
					await IoC.Get<IWindowManager>().ShowDialogAsync(infoViewModel, null, settings);
					await TryCloseAsync();
				}
				else
				{
					settings.Title = "Fatal Exception";

					infoViewModel.UpdateMessage("Fatal Exception", ex.Message);
					await IoC.Get<IWindowManager>().ShowDialogAsync(infoViewModel, null, settings);
				}
			}
		}

		private async Task LoadUsers()
		{
			IUserEndpoint userEndpoint = IoC.Get<IUserEndpoint>();

			List<UserModel> userList = new List<UserModel>(await userEndpoint.GetAll());
			Users = IoC.Get<IMapper>().Map<BindingList<UserWPFModel>>(userList);
		}

		private async Task LoadRoles()
		{
			IUserEndpoint userEndpoint = IoC.Get<IUserEndpoint>();

			_allRoles = new Dictionary<string, string>(await userEndpoint.GetAllRoles());
		}
		#endregion

		#region Buttons
		public async Task AddSelectedRole()
		{
			IUserEndpoint userEndpoint = IoC.Get<IUserEndpoint>();

			await userEndpoint.AddUserToRole(SelectedUser.Id, RoleToAdd);
			KeyValuePair<string, string> rollData = _allRoles.Where(role => role.Value == RoleToAdd).First();

			SelectedUserRoles.Add(RoleToAdd);
			SelectedUser.AddRole(rollData.Key, rollData.Value);
			AvailableRoles.Remove(RoleToAdd);
		}

		public async Task RemoveSelectedRole()
		{
			IUserEndpoint userEndpoint = IoC.Get<IUserEndpoint>();

			await userEndpoint.RemoveUserFromRole(SelectedUser.Id, RoleToRemove);
			KeyValuePair<string, string> rollData = _allRoles.Where(role => role.Value == RoleToRemove).First();

			AvailableRoles.Add(RoleToRemove);
			SelectedUser.RemoveRole(rollData.Key);
			SelectedUserRoles.Remove(RoleToRemove);
		}
		#endregion
	}
}
