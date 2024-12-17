using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ATEVersions_Management.Models.HelperModels
{
	public static class AuthHelper
	{
		public static string GetName(this IIdentity identity)
		{
			return identity.GetUserData().Name;
		}
        public static string GetUserName(this IIdentity identity)
        {
            return identity.GetUserData().UserName;
        }
        public static string GetAvatar(this IIdentity identity)
		{
			return identity.GetUserData().Avatar;
		}
		public static int GetUserId(this IIdentity identity)
		{
			return identity.GetUserData().UserId;
		}
		public static string GetEmail(this IIdentity identity)
		{
			return identity.GetUserData().Email;
		}
		public static string GetPhoneNumber(this IIdentity identity)
		{
			return identity.GetUserData().PhoneNumber;
		}
		public static int GetRoleCode(this IIdentity identity)
		{
			return identity.GetUserData().RoleCode;
		}
		public static string GetRoleName(this IIdentity identity)
		{
			return identity.GetUserData().Rolename;
		}
		public static bool Permiss_Create(this IIdentity identity)
		{
			return identity.GetUserData().Permission_create;
		}
		public static bool Permiss_View(this IIdentity identity)
		{
			return identity.GetUserData().Permission_view;
		}
		public static bool Permiss_Update(this IIdentity identity)
		{
			return identity.GetUserData().Permission_update;
		}
		public static bool Permiss_Check(this IIdentity identity)
		{
			return identity.GetUserData().Permission_check;
		}
		public static bool Permiss_Delete(this IIdentity identity)
		{
			return identity.GetUserData().Permission_delete;
		}
		public static bool Permiss_Approve(this IIdentity identity)
		{
			return identity.GetUserData().Permission_approve;
		}
		public static LoggedUserData GetUserData(this IIdentity identity)
		{
			var jsonUserData = HttpContext.Current.User.Identity.Name;
			var userData = JsonConvert.DeserializeObject<LoggedUserData>(jsonUserData);
			return userData;
		}
	}
}