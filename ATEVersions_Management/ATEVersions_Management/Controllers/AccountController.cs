using ATEVersions_Management.Models.AccountModels;
using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.HelperModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using static System.Data.Entity.Infrastructure.Design.Executor;
using System.Web.UI.WebControls;

namespace ATEVersions_Management.Controllers
{
    public class AccountController : Controller
    {
        // ====== Connect to database ======
        readonly ATEVersionContext db = new ATEVersionContext();
        //
        // ====== =================== ======

        // ====== Handling requests ======
        // GET: Account
        public ActionResult AccountIndex()
        {
            return View();
        }
        // Login view
        public ActionResult Login(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null && Request.UrlReferrer.ToString().Length > 0)
            {
                return RedirectToAction("Login", new { returnUrl = Request.UrlReferrer.ToString() });
            }
            if (User.Identity.IsAuthenticated)
            {                
                return RedirectToAction("Index","Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
            
        }
        // POST: Login action
        [HttpPost]
        public ActionResult Login(MyLogin lgModel, string returnUrl)
        {
            bool view = false, create = false, update = false, delete = false, check = false, approve = false;     
            string notify = "Invalid user, please check username and password or contact administrator!";
            try
            {
                //Handle login data
                string regex = @"^(V)(?=.*?[0-9]).{7}(?:[a-zA-Z])?$";
                string userName = lgModel.Username;
                if(!Regex.IsMatch(userName,regex)) userName = userName.ToLower();
                string hashPw = Crypto.Hash(lgModel.Password + userName);
                USER loggedUser = db.USERS.FirstOrDefault(u => u.UserName == userName && u.Password == hashPw && u.Status != 0);
                if (loggedUser != null)
                {
                    List<PERMISSION> permisses = loggedUser.ROLE.PERMISSIONs.ToList();
                    foreach (PERMISSION permiss in permisses)
                    {
                        switch(permiss.PermissionName)
                        {
                            case "View": view = true; break;
                            case "Create": create = true; break;
                            case "Update": update = true; break;
                            case "Delete": delete = true; break;
                            case "Check": check = true; break;
                            case "Approve": approve = true; break;

                        }
                    }
                    LoggedUserData userData = new LoggedUserData
                    {
                        UserId = loggedUser.UserID,
                        UserName = loggedUser.UserName,
                        Name = loggedUser.FullName,                        
                        Permission_create = create,
                        Permission_view = view,
                        Permission_update = update,                       
                        Permission_delete = delete,
                        Permission_check = check,
                        Permission_approve = approve,
                        RoleCode = loggedUser.RoleID,
                        Rolename = loggedUser.ROLE.RoleName,
                        Avatar = loggedUser.Avatar,
                        PhoneNumber = loggedUser.PhoneNumber,
                    };
                    string jsonUserData = JsonConvert.SerializeObject(userData);
                    FormsAuthentication.SetAuthCookie(jsonUserData, false);
                    //Finishing up
                    Notification.setFlash1s("You have logged in!", "success");
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }                
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error at: " + ex.Message;
            }
            
            ViewBag.Notify = notify;
            return View(lgModel);            
        }
        // Logout action
        public ActionResult Logout()
        {
            //Signout part
            FormsAuthentication.SignOut();
            Notification.setFlash1s("You have signed out!", "succes");
            return RedirectToAction("Login");
        }
        

        // Info user view
        public ActionResult UserInfor(int userID)
        {
            USER inforUser = db.USERS.Find(userID);

            return View(inforUser);
        }
        // POST: Client side update infor
        [HttpPost]
        public async Task<ActionResult> UserInfor(USER user)
        {
            USER edtUser = db.USERS.Find(user.UserID);            
            try
            {

                //Infor change                
                edtUser.FullName = user.FullName;
                edtUser.Email = user.Email;
                edtUser.PhoneNumber = user.PhoneNumber;                
                edtUser.UpdatedAt = DateTime.Now;                
                LoggedUserData userData = new LoggedUserData
                {
                    UserId = edtUser.UserID,
                    UserName = edtUser.UserName,
                    Name = edtUser.FullName,                    
                    RoleCode = edtUser.RoleID,
                    Rolename = edtUser.ROLE.RoleName,
                    Avatar = edtUser.Avatar,
                    PhoneNumber = edtUser.PhoneNumber,
                };
                string jsonUserData = JsonConvert.SerializeObject(userData);
                FormsAuthentication.SetAuthCookie(jsonUserData, false);
                edtUser.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                //Save to database
                await db.SaveChangesAsync();

                //Finishing up
                Notification.setFlash1s("Your infomation has been changed!", "success");                                
                return RedirectToAction("UserInfor", new { userID = edtUser.UserID });

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                Notification.setFlash1s("Error on changing your information", "error");
                Console.WriteLine(ex.Message);

            }

            return View(user);
        }

        // Change password view
        public ActionResult ChangePW()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            return RedirectToAction("Login");
        }
        // POST: Change password action
        [HttpPost]
        public async Task<ActionResult> ChangePW(MyChangePassword cpwModel)
        {
            USER user = db.USERS.Find(User.Identity.GetUserId());
            string hashPW = Crypto.Hash(cpwModel.OldPassword + User.Identity.GetUserName());
            string notify = "Old and new password cannot match!";
            if (cpwModel.OldPassword != cpwModel.NewPassword)
            {                
                notify = "Old password is not correct!";

                if (user.Password.Equals(hashPW))
                {
                    //Change password
                    hashPW = Crypto.Hash(cpwModel.NewPassword + User.Identity.GetUserName());
                    user.Password = hashPW;
                    //Save to database
                    await db.SaveChangesAsync();
                    //Finishing up
                    Notification.setFlash("Change password successfully, please login again!", "success");
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Login");

                }
            }
            ViewBag.Notify = notify;
            return View();

        }        
    }
    #region Use Later
    /*
         // Register view
        public ActionResult Register()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();

        }
        // POST: Register action
        [HttpPost]
        public async Task<ActionResult> Register(Register rgsModel, USER rgsUser)
        {
            string notify = "";
            bool existUser = ateContext.USERS.Any(u => u.UserName == rgsModel.Username);
            try
            {
                if (existUser)
                {
                    notify = "This username has been used, please choose another!";
                }
                else
                {
                    rgsUser.RoleID = 2;
                    rgsUser.UserName = rgsModel.Username;
                    rgsUser.FullName = rgsModel.Name;
                    string hashPw = Crypto.Hash(rgsModel.Password.Trim() + rgsModel.Username);
                    rgsUser.Password = hashPw;
                    rgsUser.Avatar = "/Images/Samples/undraw_profile.svg";
                    rgsUser.Status = 1;
                    rgsUser.CreatedAt = DateTime.Now;
                    rgsUser.UpdatedAt = DateTime.Now;
                    rgsUser.UpdatedBy = rgsModel.Username + " | " + rgsModel.Name;
                    ateContext.USERS.Add(rgsUser);
                    ateContext.Configuration.ValidateOnSaveEnabled = false;
                    await ateContext.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error at: " + ex.Message;
            }

            ViewBag.Notify = notify;
            return View(rgsModel);
        }
    */
    #endregion
}