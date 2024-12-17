using ATEVersions_Management.Models.ATEVersionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.IO;
using System.Security.Principal;
using ATEVersions_Management.Models.HelperModels;
using static ATEVersions_Management.Models.HelperModels.ManualSelectors;
using ATEVersions_Management.Models.AccountModels;
using System.Web.Helpers;
using System.Web.Security;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmUserController : DashboardController
    {
        readonly ATEVersionContext ateContext = new ATEVersionContext();
        // GET: Admin/User
        public ActionResult UserIndex()
        {
            List<USER> userList = ateContext.USERS.ToList();
            return View(userList);
        }
        // User detail view
        public ActionResult UserDetail(int id)
        {
            USER dtlUser = ateContext.USERS.Find(id);
            return View(dtlUser);
        }
        // Create user view
        public ActionResult UserCreate()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            ViewBag.RoleSelector = new SelectList(ateContext.ROLES, "RoleID", "RoleName");            
            return View();
        }
        // POST: Create user action
        [HttpPost]
        public async Task<ActionResult> UserCreate(MyRegister rgsModel, USER user)
        {
            ViewBag.RoleSelector = new SelectList(ateContext.ROLES, "RoleID", "RoleName");            
            bool existUser = ateContext.USERS.Any(u => u.UserName == rgsModel.Username);
            string notify = "This username has been used, please choose another!";
            //
            try
            {
                if (!existUser)
                {
                    user.RoleID = rgsModel.RoleID;
                    user.UserName = rgsModel.Username;
                    user.FullName = rgsModel.Name;
                    string hashPw = Crypto.Hash(rgsModel.Password.Trim() + rgsModel.Username);
                    user.Password = hashPw;
                    user.Department = rgsModel.Department;
                    /*rgsUser.Avatar = "/Images/Samples/undraw_profile.svg";*/
                    user.Status = 1;
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    user.UpdatedBy = rgsModel.Username + " | " + rgsModel.Name;
                    ateContext.USERS.Add(user);
                    ateContext.Configuration.ValidateOnSaveEnabled = false;                    
                    await ateContext.SaveChangesAsync();
                    //Finishing up
                    Notification.setFlash1s("Create user "+rgsModel.Username+" successfully","success");
                    return RedirectToAction("UserIndex");
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                notify = "Error at: " + ex.Message;
            }
            //
            ViewBag.Notify = notify;
            return View(rgsModel);
        }
        // User edit view
        public ActionResult UserEdit(int id)
        {
            if (User.Identity.GetRoleName() == "Admin")
            {
                USER edtUser = ateContext.USERS.Find(id);
                ViewBag.RoleSelector = new SelectList(ateContext.ROLES, "RoleID", "RoleName");
                List<StatusSelector> statusSelectors = GetStatusSelectors();
                ViewBag.StatusSelector = new SelectList(statusSelectors, "StatusCode", "StatusValue");

                return View(edtUser);
            }
            return RedirectToAction("AdminIndex","Dasboard");
        }
        // User edit action
        [HttpPost]
        public async Task<ActionResult> UserEdit(USER user)
        {
            USER edtUser = ateContext.USERS.Find(user.UserID);
            ViewBag.RoleSelector = new SelectList(ateContext.ROLES, "RoleID", "RoleName");
            try
            {
                
                //infor change
                edtUser.RoleID = user.RoleID;
                edtUser.FullName = user.FullName;
                edtUser.Department = user.Department;
                edtUser.Email = user.Email;
                edtUser.PhoneNumber = user.PhoneNumber;
                edtUser.Status = user.Status;
                edtUser.UpdatedAt = DateTime.Now;
                edtUser.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                
                await ateContext.SaveChangesAsync();

                Notification.setFlash1s("Edit user " + edtUser.UserName + " successfully", "success");
                return Redirect("~/Admin/AdmUser/UserDetail/" + edtUser.UserID);

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                Console.WriteLine(ex.Message);

            }

            return View(user);
        }
        // Reset password view
        public ActionResult ResetPW(int userID)
        {
            if (User.Identity.GetRoleName() == "Admin")
            {
                USER user = ateContext.USERS.Find(userID);
                ViewBag.User = user;
                return View();
            }
            return RedirectToAction("AdminIndex", "Dasboard");
        }
        // POST: Reset password action
        [HttpPost]
        public async Task<ActionResult> ResetPW(int userID, MyResetPassword rpwModel)
        {
            USER user = ateContext.USERS.Find(userID);
            string hashPW = Crypto.Hash(rpwModel.NewPassword + user.UserName);
            
            //Change password            
            user.Password = hashPW;
            //Save to database
            await ateContext.SaveChangesAsync();
            //Finishing up
            Notification.setFlash("Reset password successfully!", "success");
                   
            return RedirectToAction("UserIndex");

        }

        #region UseLater
        /*
            // Upload image to Images folder 
            //image file handle
                if (user.FileUpload == null)
                {
                    edtUser.Avatar = edtUser.Avatar;
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(user.FileUpload.FileName);
                    string extension = Path.GetExtension(user.FileUpload.FileName);
                    string folderLocate = "/Images/ImagesAvatar/";
                    fileName = DateTime.Now.ToString("dd-MM-yyyy") + "_" + fileName + extension;
                    edtUser.Avatar =  folderLocate + fileName;
                    fileName = Path.Combine(Server.MapPath("~" + folderLocate), fileName);
                    user.FileUpload.SaveAs(fileName);
                }
         */
        #endregion
    }
}