using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.HelperModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATEVersions_Management.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult AllNotifyIndex()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            
            ATESignNotifyDTO ATENotify = new ATESignNotifyDTO()
            {
                ATEListNotify = ATESignNotifyDAO.ATEListSignNotify(User.Identity.GetRoleName()),
                GRRNotify = ATESignNotifyDAO.GRRReportSignNotify(User.Identity.GetRoleName())
            };
            ViewBag.NotifyATEList = ATENotify;

            return View();
        }

        // Notify to user incharge of signing when ATEList status is changed
        [HttpGet]
        public JsonResult NotifyATEList()
        {
            ATESignNotifyDTO data = new ATESignNotifyDTO();
            if (User.Identity.IsAuthenticated)
            {
                data.ATEListNotify = ATESignNotifyDAO.ATEListSignNotify(User.Identity.GetRoleName());
                data.GRRNotify = ATESignNotifyDAO.GRRReportSignNotify(User.Identity.GetRoleName());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        // Notify to user incharge of signing when GRR Report is prepared
        [HttpGet]
        public JsonResult NotifyGRRReport()
        {
            List<GRRTableDTO> data = new List<GRRTableDTO>();
            if (User.Identity.IsAuthenticated)
            {
                data = ATESignNotifyDAO.GRRReportSignNotify(User.Identity.GetRoleName());
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}