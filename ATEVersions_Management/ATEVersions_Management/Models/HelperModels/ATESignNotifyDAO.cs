using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DTOModels;

namespace ATEVersions_Management.Models.HelperModels
{
    public class ATESignNotifyDAO
    {
        static readonly ATEVersionContext db = new ATEVersionContext();
        static public List<ATEListDTO> ATEListSignNotify(string role)
        {
            List<ATEListDTO> data = new List<ATEListDTO>();
            switch (role)
            {
                case "Preparer":
                    data = (from ate in db.ATE_CHECKLIST
                            where (ate.Status == 1) && (!string.IsNullOrEmpty(ate.CheckerNote) || !string.IsNullOrEmpty(ate.ApproverNote))
                            orderby ate.UpdatedAt descending
                            select new ATEListDTO
                            {
                                CheckListID = ate.CheckListID,
                                VersionName = ate.VERSION.VersionName,
                                ModelName = ate.VERSION.PROGRAM.ModelName,
                                Status = ate.Status,
                                PreparedBy = ate.PreparedBy,
                                PreparerNote = ate.PreparerNote,
                                CheckerNote = ate.CheckerNote,
                                CheckedBy = ate.CheckedBy,
                                ApproverNote = ate.ApproverNote,
                                ApprovedBy = ate.ApprovedBy,
                                UpdatedAt = ate.UpdatedAt,
                                UpdatedBy = ate.UpdatedBy,
                            }).ToList();
                    break;
                case "Checker":
                    data = (from ate in db.ATE_CHECKLIST
                            where (ate.Status == 1) || (ate.Status == 2 && !string.IsNullOrEmpty(ate.ApproverNote))
                            orderby ate.UpdatedAt descending
                            select new ATEListDTO
                            {
                                CheckListID = ate.CheckListID,
                                VersionName = ate.VERSION.VersionName,
                                ModelName = ate.VERSION.PROGRAM.ModelName,
                                Status = ate.Status,
                                PreparedBy = ate.PreparedBy,
                                PreparerNote = ate.PreparerNote,
                                CheckerNote = ate.CheckerNote,
                                CheckedBy = ate.CheckedBy,
                                ApproverNote = ate.ApproverNote,
                                ApprovedBy = ate.ApprovedBy,
                                UpdatedAt = ate.UpdatedAt,
                                UpdatedBy = ate.UpdatedBy,
                            }).ToList();
                    break;
                case "Approver":
                    data = (from ate in db.ATE_CHECKLIST
                            where (ate.Status != 3)
                            orderby ate.UpdatedAt descending
                            select new ATEListDTO
                            {
                                CheckListID = ate.CheckListID,
                                VersionName = ate.VERSION.VersionName,
                                ModelName = ate.VERSION.PROGRAM.ModelName,
                                Status = ate.Status,
                                PreparedBy = ate.PreparedBy,
                                PreparerNote = ate.PreparerNote,
                                CheckerNote = ate.CheckerNote,
                                CheckedBy = ate.CheckedBy,
                                ApproverNote = ate.ApproverNote,
                                ApprovedBy = ate.ApprovedBy,
                                UpdatedAt = ate.UpdatedAt,
                                UpdatedBy = ate.UpdatedBy,
                            }).ToList();
                    break;
            }
            return data;
        }
        static public List<GRRTableDTO> GRRReportSignNotify(string role)
        {
            List<GRRTableDTO> data = new List<GRRTableDTO>();
            switch (role)
            {                
                case "Approver":
                    data = (from grr in db.GRR_TABLE
                            where (grr.Status == 1)
                            orderby grr.UpdatedAt descending
                            select new GRRTableDTO
                            {
                                GRR_ID = grr.GRR_ID,
                                GageModel = grr.GageModel,
                                GageName = grr.GageName,
                                PartName = grr.PartName,
                                Status = grr.Status,
                                PreparedBy = grr.PreparedBy,
                                PreparedNote = grr.PreparedNote,
                                ApproverNote = grr.ApproverNote,
                                ApprovedBy = grr.ApprovedBy,
                                UpdatedAt = grr.UpdatedAt,
                                UpdatedBy = grr.UpdatedBy,
                            }).ToList();
                    break;
            }
            return data;
        }
    }
}