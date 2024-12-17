using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Areas.Admin.Controllers
{
    public class AdmCheckItemController : DashboardController
    {
        readonly ATEVersionContext ateContext = new ATEVersionContext();
        // GET: Admin/AdmCheckItem
        public ActionResult CheckItemIndex()
        {
            List<CHECKLIST_ITEM> itmList = ateContext.CHECKLIST_ITEM.ToList();
            return View(itmList);
        }

        // Detail infor of check item 
        public ActionResult ItemDetail(int id)
        {
            CHECKLIST_ITEM dtlItem = ateContext.CHECKLIST_ITEM.Find(id);

            return View(dtlItem);
        }

        // Create new item info
        public ActionResult ItemCreate()
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                return View();
            }
            return RedirectToAction("CheckItemIndex");
        }

        [HttpPost]
        public async Task<ActionResult> ItemCreate(CHECKLIST_ITEM item)
        {
            try
            {
                item.Status = 1;
                item.CreatedAt = DateTime.Now;
                item.UpdatedAt = DateTime.Now;
                item.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();

                ateContext.CHECKLIST_ITEM.Add(item);
                await ateContext.SaveChangesAsync();
                return RedirectToAction("CheckItemIndex");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Console.WriteLine(ex.ToString());
            }
            return View();
        }
        
        // Update item info
        public ActionResult ItemEdit(int id)
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                CHECKLIST_ITEM edtItem = ateContext.CHECKLIST_ITEM.Find(id);

                return View(edtItem);
            }
            return RedirectToAction("CheckItemIndex");
        }

        [HttpPost]
        public async Task<ActionResult> ItemEdit(CHECKLIST_ITEM item)
        {
            CHECKLIST_ITEM edtItem = ateContext.CHECKLIST_ITEM.Find(item.ItemID);
            try
            {
                edtItem.ItemName = item.ItemName;
                edtItem.CheckMethod = item.CheckMethod;
                edtItem.UpdatedAt = DateTime.Now;
                edtItem.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();

                await ateContext.SaveChangesAsync();
                return Redirect("~/Admin/AdmCheckItem/ItemDetail/" + item.ItemID);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex;
                Console.WriteLine(ex.ToString());
            }
            return View(item);
        }
        // Delete program info (change status to [0:disable] not delete record)
        [HttpPost]
        public async Task<ActionResult> ItemDelete(int id)
        {
            if (User.Identity.GetRoleName() == "Admin" || User.Identity.GetRoleName() == "Preparer")
            {
                CHECKLIST_ITEM dltItem = ateContext.CHECKLIST_ITEM.Find(id);
                try
                {
                    dltItem.Status = 0;
                    dltItem.UpdatedAt = DateTime.Now;
                    dltItem.UpdatedBy = User.Identity.GetUserName() + " | " + User.Identity.GetName();

                    await ateContext.SaveChangesAsync();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
            return RedirectToAction("CheckItemIndex");
        }
    }
}