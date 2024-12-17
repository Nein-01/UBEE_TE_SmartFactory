using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmProgramController : DashboardController
    {
        readonly ATEVersionContext ateContext = new ATEVersionContext();
        // GET: Admin/AdProgram
        [Route("Admin/Program/Index")]
        public ActionResult ProgramIndex()
        {
            List<PROGRAM> prgList = ateContext.PROGRAMs.Where(p => p.Status != 0).ToList();
            return View(prgList);
        }

        // Detail of program infor
        public ActionResult ProgramDetail(int id)
        {
            PROGRAM detailProgram = ateContext.PROGRAMs.Find(id);

            return View(detailProgram);
        }

        // Create new program info
        public ActionResult ProgramCreate()
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {                                
                ViewBag.listProjectType = new SelectList(ATEVersionsDAO.GET_ListProjectType());
                return View();
            }
            return RedirectToAction("ProgramIndex");
        }

        [HttpPost]
        public async Task<ActionResult> ProgramCreate(PROGRAM createProgram)
        {
            if (!ExistProgram(createProgram.ProgramID, createProgram.ModelName))
            {
                try
                {
                    createProgram.Status = 1;
                    createProgram.CreatedAt = DateTime.Now;
                    createProgram.UpdatedAt = DateTime.Now;
                    createProgram.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                    ateContext.PROGRAMs.Add(createProgram);
                    await ateContext.SaveChangesAsync();
                    Notification.setFlash1s("Create info for"+ createProgram.ModelName +" successfully!","success");
                    return RedirectToAction("ProgramIndex");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex;
                    Console.WriteLine(ex.ToString());
                }
            }
            Notification.setFlash1s(createProgram.ModelName+" is already exist!", "danger");
            return View();
        }
       
        // Update program info
        public ActionResult ProgramEdit(int id)
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                PROGRAM edtProgram = ateContext.PROGRAMs.Find(id);
                ViewBag.listProjectType = new SelectList(ATEVersionsDAO.GET_ListProjectType());
                return View(edtProgram);
            }
            return RedirectToAction("ProgramIndex");
        }

        [HttpPost]
        public async Task<ActionResult> ProgramEdit(PROGRAM editProgram)
        {
            PROGRAM edtProgram = ateContext.PROGRAMs.Find(editProgram.ProgramID);
            try
            {
                if (!ExistProgram(editProgram.ProgramID, editProgram.ModelName))
                {
                    edtProgram.ProgramName = editProgram.ProgramName;
                    edtProgram.ModelName = editProgram.ModelName;
                    edtProgram.DevelopTool = editProgram.DevelopTool;
                    edtProgram.ProjectType = editProgram.ProjectType;
                    edtProgram.UpdatedAt = DateTime.Now;
                    edtProgram.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                    await ateContext.SaveChangesAsync();

                    Notification.setFlash1s("Edit " + edtProgram.ModelName + " successfully!", "success");
                    return Redirect("~/Admin/AdmProgram/ProgramDetail/" + editProgram.ProgramID);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Console.WriteLine(ex.ToString());
                Notification.setFlash1s("Fail to update program " + editProgram.ModelName + "!\n" + "Error: " + ex.ToString(), "danger");
            }
            Notification.setFlash1s(editProgram.ModelName + " is already exist!", "danger");
            return View(editProgram);
        }
        // Delete program info
        [HttpPost]
        public async Task<ActionResult> ProgramDelete(int id)
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                PROGRAM dltProgram = ateContext.PROGRAMs.Find(id);
                try
                {
                   
                    dltProgram.Status = 0;
                    dltProgram.UpdatedAt = DateTime.Now;
                    dltProgram.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();

                    await ateContext.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }                
            }
            return RedirectToAction("ProgramIndex");
        } 
        // To version list by program id
        public ActionResult VerProgList(int prgId)
        {            

            return Redirect("~/Admin/AdmVersion/VersionIndex/"+ prgId);
        }
        #region Hepler
        public bool ExistProgram(int? prgId , string model)
        {
            PROGRAM tmp = ateContext.PROGRAMs.FirstOrDefault(p => p.ModelName.ToLower() == model.ToLower());
            if(prgId.HasValue)
                tmp = ateContext.PROGRAMs.FirstOrDefault(p => p.ProgramID != prgId && p.ModelName.ToLower() == model.ToLower());
            return tmp != null;
        }
        #endregion
    }
}