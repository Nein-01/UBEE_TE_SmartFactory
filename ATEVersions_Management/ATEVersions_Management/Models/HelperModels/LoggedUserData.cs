using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
	/// <summary>
	/// Conatain user information after logged in
	/// </summary>
	public class LoggedUserData
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public bool Permission_create { get; set; }
		public bool Permission_view { get; set; }
		public bool Permission_update { get; set; }
		public bool Permission_delete { get; set; }
		public bool Permission_check { get; set; }
		public bool Permission_approve { get; set; }
		public int RoleCode { get; set; }
		public string Rolename { get; set; }
		public string Avatar { get; set; }
		public string PhoneNumber { get; set; }
	}
}