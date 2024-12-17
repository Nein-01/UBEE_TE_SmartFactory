using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmTestPlanController : DashboardController
    {
        private readonly ATEVersionContext db = new ATEVersionContext();
        // GET: Admin/AdmTestPlan
        public ActionResult TestPlanIndex()
        {
            List<TestPlanDTO> listTestPlan = ATEVersionsDAO.GetTestPlanList();
            return View(listTestPlan);
        }

        // GET: View for creating new test plan infor
        public ActionResult TestPlanCreate()
        {
            ViewBag.listProjectType = new SelectList(ATEVersionsDAO.GET_ListProjectType());
            ViewBag.ListModel = ATEVersionsDAO.GetListATEModel();
            return View();
        }
        // POST: Post action to create new test plan information
        [HttpPost]
        public async Task<ActionResult> TestPlanCreate(TestPlanDTO testPlanDTO) 
        {
            ViewBag.ListModel = ATEVersionsDAO.GetListATEModel();
            if(testPlanDTO.FileUpload == null)
            {
                ViewBag.ErrorMessage = "Must attach a file!";
                return View();
            }
            try
            {
                                
                if(!ATEVersionsDAO.ExistTestPlanVer(testPlanDTO.TestPlanID, testPlanDTO.ModelName, testPlanDTO.TestPlanVersion))
                {
                    // Get data to TEST_PLAN model
                    TEST_PLAN createTestPlan = new TEST_PLAN
                    {
                        UserID = User.Identity.GetUserId(),
                        ModelName = testPlanDTO.ModelName,
                        ProjectType = testPlanDTO.ProjectType,
                        TestPlanVersion = testPlanDTO.TestPlanVersion,
                        Author = testPlanDTO.Author,
                        ModifyNote = testPlanDTO.ModifyNote,
                        ModifiedAt = testPlanDTO.ModifiedAt,
                        StoredDir = TestPlanFileProcess(testPlanDTO.ModelName, testPlanDTO.TestPlanVersion, testPlanDTO.FileUpload),
                        Status = 1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName(),
                    };
                    // Save data to database
                    db.TEST_PLANs.Add(createTestPlan);
                    await db.SaveChangesAsync();
                    // Finishing up
                    Notification.setFlash1s("Add new test plan " + createTestPlan.ModelName + "_" + createTestPlan.TestPlanVersion + " successfully!", "success");
                    return RedirectToAction("TestPlanIndex");
                }

                Notification.setFlash1s("Test plan " + testPlanDTO.ModelName + "_" + testPlanDTO.TestPlanVersion + " is existed!","danger");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.ErrorMessage = ex.Message;
            }
            
            return View();
        }
        // GET: View for viewing test plan detail information
        public ActionResult TestPlanDetail(int id)
        {            
            TestPlanDTO detailTestPlan = ATEVersionsDAO.GetTestPlanByID(id);
            return View(detailTestPlan);
        }

        // GET: View for editing test plan infor
        public ActionResult TestPlanEdit(int id)
        {
            ViewBag.listProjectType = new SelectList(ATEVersionsDAO.GET_ListProjectType());
            ViewBag.ListUserSameRole = new SelectList(ATEVersionsDAO.GetUserListByRoleID(User.Identity.GetRoleCode()),"UserID","FullName");
            TestPlanDTO editTestPlan = ATEVersionsDAO.GetTestPlanByID(id);
            return View(editTestPlan);
        }
        // POST: Post action to edit test plan information
        [HttpPost]
        public async Task<ActionResult> TestPlanEdit(TestPlanDTO editTestPlanDTO)
        {
            ViewBag.ListUserSameRole = new SelectList(ATEVersionsDAO.GetUserListByRoleID(User.Identity.GetRoleCode()), "UserID", "FullName");            

            try
            {
                // Get edited data to db model
                
                if (!ATEVersionsDAO.ExistTestPlanVer(editTestPlanDTO.TestPlanID, editTestPlanDTO.ModelName, editTestPlanDTO.TestPlanVersion))
                {
                    TEST_PLAN editTestPlan = db.TEST_PLANs.Find(editTestPlanDTO.TestPlanID);
                    editTestPlan.TestPlanVersion = editTestPlanDTO.TestPlanVersion;
                    editTestPlan.UserID = editTestPlanDTO.UserID;
                    editTestPlan.ProjectType = editTestPlanDTO.ProjectType;
                    editTestPlan.Author = editTestPlanDTO.Author;
                    editTestPlan.ModifyNote = editTestPlanDTO.ModifyNote;
                    editTestPlan.ModifiedAt = editTestPlanDTO.ModifiedAt;
                    if(editTestPlanDTO.FileUpload != null)
                    {
                        editTestPlan.StoredDir = TestPlanFileProcess(editTestPlanDTO.ModelName, editTestPlanDTO.TestPlanVersion, editTestPlanDTO.FileUpload);
                    }
                    editTestPlan.UpdatedAt = DateTime.Now;
                    editTestPlan.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                    // Save change to database
                    
                    await db.SaveChangesAsync();
                    // Finishing up
                    Notification.setFlash1s("Test plan " + editTestPlan.ModelName + "_" + editTestPlan.TestPlanVersion + " edited successfully!", "success");
                    return RedirectToAction("TestPlanDetail", new { id = editTestPlan.TestPlanID });
                }

                Notification.setFlash1s("Test plan " + editTestPlanDTO.ModelName + "_" + editTestPlanDTO.TestPlanVersion + " is existed!", "danger");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }

            return View(editTestPlanDTO);
        }
        // POST: Post action to delete test plan
        [HttpPost]
        public async Task<ActionResult> TestPlanDelete(int id)
        {
            try
            {
                TEST_PLAN deleteTestPlan = db.TEST_PLANs.Find(id);
                DeleteFileOnRemovingTestPlan(deleteTestPlan.StoredDir);
                db.TEST_PLANs.Remove(deleteTestPlan);
                // save change to database
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Notification.setFlash1s("Error on deleting test plan: " + ex.Message,"danger");
                Console.WriteLine("Error: " + ex);
            }
            return RedirectToAction("TestPlanIndex");
        }

        #region Helper
        private string TestPlanFileProcess(string model, string ver, HttpPostedFileBase testPlanFile)
        {
            try
            {
                // Check directory
                string FormatTestPlanDir = "/Client_Data/TestPlans/" + model + "/";
                string ServerTestPlanDirPath = Server.MapPath("~" + FormatTestPlanDir);
                if (!Directory.Exists(ServerTestPlanDirPath))
                {
                    Directory.CreateDirectory(ServerTestPlanDirPath);
                }
                // Save file to TestPlan folder
                string fileExtension = Path.GetExtension(testPlanFile.FileName);
                string FormatFileName = model + "_Test_Plan_" + ver + fileExtension;
                string SavedFilePath = FormatTestPlanDir + FormatFileName;
                string ServerTestPlanFilePath = Path.Combine(Server.MapPath("~" + SavedFilePath));                
                testPlanFile.SaveAs(ServerTestPlanFilePath);

                return SavedFilePath;
            }
            catch(Exception ex)
            {
                return "Error: " + ex;
            }
            
        }
        private void DeleteFileOnRemovingTestPlan(string filePath)
        {
            // Delete testplan file
            string ServerFilePath = Server.MapPath("~" + filePath);
            if (System.IO.File.Exists(ServerFilePath))
            {
                System.IO.File.Delete(ServerFilePath);
            }
            // Delete testplan folder if empty
            string testplanFolderPath = ServerFilePath.Substring(0, ServerFilePath.LastIndexOf("\\"));
            if(Directory.GetFiles(testplanFolderPath).Length == 0)
            {
                Directory.Delete(testplanFolderPath);
            }            
        }
        #endregion
    }
}