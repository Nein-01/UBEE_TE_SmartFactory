using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DAOModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmATEListController : DashboardController
    {
        readonly ATEVersionContext ateContext = new ATEVersionContext();
        // GET: Admin/AdmATEList
        public ActionResult ATEListIndex(int? versionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }    
            
            List<ATE_CHECKLIST> ateList = ateContext.ATE_CHECKLIST.ToList();
            if (versionId.HasValue)
            {
                ateList = ateContext.ATE_CHECKLIST.Where(v => v.Status != 0 && v.VersionID == versionId).ToList();
                if (!ateList.Any())
                {
                    return RedirectToAction("ATEListCreate");
                }
            }

            return View(ateList);
                        
        }

        // Detail of ATEList
        public ActionResult ATEListDetail(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                ATE_CHECKLIST dtlAte = ateContext.ATE_CHECKLIST.Find(id);
                ViewBag.ChecklistItems = ateContext.CHECKLIST_ITEM.ToList();           
                return View(dtlAte);
            }

            return RedirectToAction("Login", "Account", new { area = "" });
        }

        // Create new atelist info
        public ActionResult ATEListCreate(int? versionId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account", new { area = "" });
            }
            
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {                
                //Select versions                
                List<VersionDTO> listVersionWaitForChecklist = ATEVersionsDAO.GetVersionsWaitForChecklist();
                versionId = versionId ?? 0;
                ViewBag.vrsSelector = new SelectList(listVersionWaitForChecklist, "VersionID", "VersionName", versionId);
                //Checklist Items            
                ViewBag.checkItems = ateContext.CHECKLIST_ITEM.ToList();
                return View();
            }
            return RedirectToAction("ATEListIndex");
        }

        [HttpPost]
        public async Task<ActionResult> ATEListCreate(ATE_CHECKLIST ate)
        {
            //Select versions           
            var listVersionWaitForChecklist = ATEVersionsDAO.GetVersionsWaitForChecklist();
            ViewBag.vrsSelector = new SelectList(listVersionWaitForChecklist, "VersionID", "VersionName", 0);
            //Checklist Items                      
            List<CHECKLIST_ITEM> checklistItems = ateContext.CHECKLIST_ITEM.ToList();
            ViewBag.checkItems = checklistItems;
            int ateID = ate.CheckListID;
            ate.CHECKLIST_DETAIL = new List<CHECKLIST_DETAIL>();

            try
            {
                foreach (var item in checklistItems)
                {
                    CHECKLIST_DETAIL tmp = new CHECKLIST_DETAIL
                    {
                        CheckListID = ateID,
                        ItemID = item.ItemID,
                        Result = 1
                    };
                    ate.CHECKLIST_DETAIL.Add(tmp);

                }

                ate.Status = 1;
                ate.CreatedAt = DateTime.Now;
                ate.UpdatedAt = DateTime.Now;
                ate.PreparedBy = User.Identity.GetName();
                ate.IsPrepared = 1;
                ate.PreparedAt = DateTime.Now;
                ate.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();
                // Save data to DB
                ateContext.ATE_CHECKLIST.Add(ate);
                await ateContext.SaveChangesAsync();

                // Send mail ask for checking to checker users
                /*FoxconnSMTPModel mailInfo = new FoxconnSMTPModel()
                {
                    ToMail = ATEVersionsDAO.GetListEmailFromListUsers(ATEVersionsDAO.GetUserListByRoleName("checker")),
                    FromMail = "nein.wg.ruan@mail.foxconn.com",
                    CC = "",
                    Subject = "ATEList checking request from SmartFactory!",
                    Message = "Please check these new ATELists at: " + WebInfoModel.GetWebPageURL("/Notification/AllNotifyIndex")
                };
                SendMailViaFoxconnSMTP(mailInfo);*/


                // 
                return RedirectToAction("ATEListIndex");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Console.WriteLine(ex.ToString());
            }
            return View();
        }

        // Update ATEList info
        public ActionResult ATEListEdit(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.Identity.GetRoleCode() != 2)
                {
                    ATE_CHECKLIST edtAte = ateContext.ATE_CHECKLIST.Find(id);
                    ViewBag.ChecklistItems = ateContext.CHECKLIST_ITEM.ToList();
                    return View(edtAte);
                }
                return RedirectToAction("ATEListIndex");
            }

            return RedirectToAction("Login", "Account", new { area = "" });
        }

        [HttpPost]
        public async Task<ActionResult> ATEListEdit(ATE_CHECKLIST ate)
        {
            ViewBag.ChecklistItems = ateContext.CHECKLIST_ITEM.ToList();
            ATE_CHECKLIST edtAte = ateContext.ATE_CHECKLIST.Find(ate.CheckListID);

            try
            {
                               
                switch (User.Identity.GetRoleName())
                {
                    case "Admin":
                        edtAte.ProductHW_SW = ate.ProductHW_SW;
                        edtAte.StoredTime = ate.StoredTime;
                        edtAte.PreparerNote = ate.PreparerNote;
                        if(ate.Status == 1)
                        {
                            edtAte.IsPrepared = 1;
                            edtAte.IsChecked = 0;
                            edtAte.IsApproved = 0;
                            edtAte.PreparedBy = User.Identity.GetName();
                            edtAte.PreparedAt = DateTime.Now;
                        }
                        if(ate.Status == 2)
                        {
                            edtAte.IsChecked = 1;
                            edtAte.IsApproved = 0;
                            edtAte.CheckedBy = User.Identity.GetName();
                            edtAte.CheckerNote = ate.CheckerNote;
                            edtAte.CheckedAt = DateTime.Now;
                        }
                        if (ate.Status == 3)
                        {
                            edtAte.IsPrepared = 1;
                            edtAte.IsChecked = 1;
                            edtAte.IsApproved = 1;
                            edtAte.ApprovedBy = User.Identity.GetName();
                            edtAte.ApproverNote = ate.ApproverNote;
                            edtAte.ApprovedAt = DateTime.Now;
                        }
                        break;
                    case "Preparer":                        
                        edtAte.Status = 1;
                        edtAte.ProductHW_SW = ate.ProductHW_SW;
                        edtAte.StoredTime = ate.StoredTime;
                        edtAte.PreparedBy = User.Identity.GetName();
                        edtAte.IsPrepared = 1;
                        edtAte.PreparerNote = ate.PreparerNote;
                        edtAte.PreparedAt = DateTime.Now;
                        break;
                    case "Checker":
                        if (ate.Status == 1)
                        {
                            edtAte.IsPrepared = 0;
                            edtAte.PreparerNote = "";
                            edtAte.IsChecked = 0;
                        }                             
                        if (ate.Status == 2) 
                        {                             
                            edtAte.IsChecked = 1; 
                        }
                        edtAte.CheckedBy = User.Identity.GetName();
                        edtAte.CheckerNote = ate.CheckerNote;
                        edtAte.CheckedAt = DateTime.Now;
                        edtAte.Status = ate.Status;
                        // Send mail ask for approving to approver users
                       /* FoxconnSMTPModel mailInfo = new FoxconnSMTPModel()
                        {
                            ToMail = ATEVersionsDAO.GetListEmailFromListUsers(ATEVersionsDAO.GetUserListByRoleName("approver")),
                            FromMail = "nein.wg.ruan@mail.foxconn.com",
                            CC = "",
                            Subject = "ATEList approving request from SmartFactory!",
                            Message = "Please approve these new ATELists at: " + WebInfoModel.GetWebPageURL("/Notification/AllNotifyIndex")
                        };
                        SendMailViaFoxconnSMTP(mailInfo);*/
                        break;
                    case "Approver":
                        if (ate.Status == 1)
                        {
                            edtAte.IsPrepared = 0;
                            edtAte.PreparerNote = "";
                            edtAte.IsChecked = 0;
                            edtAte.CheckerNote = "";
                            edtAte.IsApproved = 0;                            
                        }
                        if (ate.Status == 2)
                        {
                            edtAte.IsChecked= 0;
                            edtAte.CheckerNote = "";
                            edtAte.IsApproved = 0;                            
                        }
                        if (ate.Status == 3) 
                        { 
                            edtAte.IsApproved = 1; 
                        }
                        edtAte.ApprovedBy = User.Identity.GetName();
                        edtAte.ApproverNote = ate.ApproverNote;
                        edtAte.ApprovedAt = DateTime.Now;
                        edtAte.Status = ate.Status;

                        break;
                }
                
                edtAte.UpdatedAt = DateTime.Now;
                edtAte.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();

                await ateContext.SaveChangesAsync();
                return Redirect("~/Admin/AdmATEList/ATEListDetail/" + ate.CheckListID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Console.WriteLine(ex.ToString());
            }
            return View(ate);
        }
        // ATEList delete action
        [HttpPost]
        public async Task<ActionResult> ATEListDelete(int id)
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                ATE_CHECKLIST dltAte = ateContext.ATE_CHECKLIST.Find(id);
                try
                {
                    ateContext.ATE_CHECKLIST.Remove(dltAte);

                    await ateContext.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
            return RedirectToAction("ATEListIndex");
        }

        // Change result of each check item in ATEList
        [HttpPost]
        public async Task<ActionResult> ChangeResult(int ateId, int itemId, int eResult)
        {
            try
            {
                CHECKLIST_DETAIL editATE = ateContext.CHECKLIST_DETAIL.SingleOrDefault(a => a.CheckListID == ateId && a.ItemID == itemId);
                editATE.Result = eResult;

                await ateContext.SaveChangesAsync();

                return Json(new { response = "Success", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { response = "Fail: " + ex, JsonRequestBehavior.AllowGet });
            }

        }
        // Change result of all check items in ATEList
        [HttpPost]
        public async Task<ActionResult> ChangeResultAll(int ateId, int eResult)
        {
            try
            {
                List<CHECKLIST_DETAIL> editATE = ateContext.CHECKLIST_DETAIL.Where(a => a.CheckListID == ateId).ToList();
                foreach (CHECKLIST_DETAIL item in editATE)
                {
                    item.Result = eResult;
                }

                await ateContext.SaveChangesAsync();

                return Json(new { response = "Success", JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {
                return Json(new { response = "Fail: " + ex, JsonRequestBehavior.AllowGet });
            }

        }

        #region Support Functions
        private bool SendMailViaFoxconnSMTP(FoxconnSMTPModel mailInfo)
        {
            try
            {
                return FoxconnSMTPModel.Foxconn_SMTP_SendMail(mailInfo.ToMail, mailInfo.FromMail, mailInfo.CC, mailInfo.Subject, mailInfo.Message);
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        #endregion

    }
}