using ATEVersions_Management.Models.ATEVersionModels;
using ATEVersions_Management.Models.DTOModels;
using ATEVersions_Management.Models.DTOModels.TestMonitorDTOs;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace ATEVersions_Management.Models.DAOModels
{    
    public class ATEVersionsDAO
    {
        readonly static string sqlConString = "data source=10.220.99.252,4000;initial catalog=ATE_VERSION;persist security info=True;user id=sa;password=allan;MultipleActiveResultSets=True;App=EntityFramework";
        //readonly static string sqlConString = "data source=10.220.99.252,4000;initial catalog=SmartFactory;persist security info=True;user id=sa;password=allan;MultipleActiveResultSets=True;App=EntityFramework";
        #region ========= ATE Versions data access section =========
        // ====== ====================================== ======
        //          ATE Versions data access section
        // ====== ====================================== ======
        // ====== ====================================== ======
        //          Get data directly from database
        // ====== ====================================== ======

        static readonly ATEVersionContext db = new ATEVersionContext();
        #region ========= PROGRAM Table =========
        static public List<string> GET_ListProjectType()
        {
            return new List<string>() { "CABLE", "GPON", "WIRELESS" };
        }
        static public List<ProgramDTO> GetAllProgram()
        {
            return (from program in db.PROGRAMs
                    orderby program.ModelName
                    select new ProgramDTO
                    {
                        ProgramID = program.ProgramID,
                        ModelName = program.ModelName,
                        ProgramName = program.ProgramName,
                        DevelopTool = program.DevelopTool,
                        Status = program.Status,
                        CreatedAt = program.CreatedAt,
                        UpdatedAt = program.UpdatedAt,
                        UpdatedBy = program.UpdatedBy
                    }).DistinctBy(prg => prg.ModelName).ToList();
        }
        static public List<string> GetListATEModel()
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from program in db.PROGRAMs
                            group program by program.ModelName into grpPrograms
                            orderby grpPrograms.Key
                            select grpPrograms.Key
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }
        static public List<string> GET_ListATEModelByProjectType(string projectType)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    string sqlCommand = "SELECT ModelName FROM PROGRAM WHERE ProjectType LIKE '%"+ projectType + "%'";
                    return db.Database.SqlQuery<string>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        static public int GetUncheckedALSTVersions(string model)
        {
            try
            {
                string sqlCommand = "SELECT COUNT(VRS.VersionID) FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID JOIN ATE_CHECKLIST ALST ON VRS.VersionID = ALST.VersionID WHERE PRG.ModelName LIKE '"+model+"' AND ALST.Status = 1";
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return db.Database.SqlQuery<int>(sqlCommand).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public int GetNoALSTVersions(string model)
        {
            try
            {
                int allVersions = 0, alstVersions = 0;
                string sqlCommand = "SELECT COUNT(VRS.VersionID) FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID WHERE PRG.ModelName LIKE '"+model+"'";
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    allVersions = db.Database.SqlQuery<int>(sqlCommand).SingleOrDefault();
                }
                sqlCommand = "SELECT COUNT(VRS.VersionID) FROM PROGRAM PRG JOIN VERSION VRS ON PRG.ProgramID = VRS.ProgramID JOIN ATE_CHECKLIST ALST ON VRS.VersionID = ALST.VersionID  WHERE PRG.ModelName LIKE '" + model + "'";
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    alstVersions = db.Database.SqlQuery<int>(sqlCommand).SingleOrDefault();
                }
                return allVersions - alstVersions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ========= VERSION Table =========
        static public List<VersionDTO> GetVersionList()
        {
            return (from vrs in db.VERSIONs
                    where vrs.Status != 0
                    orderby vrs.ReleaseTime descending
                    select new VersionDTO
                    {
                        VersionID = vrs.VersionID,
                        VersionName = vrs.VersionName,
                        ProgramName = vrs.PROGRAM.ProgramName,
                        ModelName = vrs.PROGRAM.ModelName,
                        Engineer = vrs.Engineer,
                        BuildTime = vrs.BuildTime,
                        ReleaseTime = vrs.ReleaseTime,
                        ReleaseNote = vrs.ReleaseNote,
                    }).ToList();
        }
        static public List<VersionDTO> GetVersionListByModel(string model)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from vrs in db.VERSIONs
                            where vrs.Status != 0 &&
                                  vrs.PROGRAM.ModelName.ToLower() == model.ToLower()
                            orderby vrs.ReleaseTime descending
                            select new VersionDTO
                            {
                                VersionID = vrs.VersionID,
                                VersionName = vrs.VersionName,
                                ProgramName = vrs.PROGRAM.ProgramName,
                                ModelName = vrs.PROGRAM.ModelName,
                                ProjectType = vrs.PROGRAM.ProjectType,
                                Engineer = vrs.Engineer,
                                BuildTime = vrs.BuildTime,
                                ReleaseTime = vrs.ReleaseTime,
                                ReleaseNote = vrs.ReleaseNote,
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public List<VersionDTO> GetVersionListWithSearchKey(string searchStr)
        {
            return (from vrs in db.VERSIONs
                    where vrs.Status != 0 &&
                        (vrs.VersionName.ToLower().Contains(searchStr.ToLower())
                        || vrs.PROGRAM.ProgramName.ToLower().Contains(searchStr.ToLower())
                        || vrs.PROGRAM.ModelName.ToLower().Contains(searchStr.ToLower())
                        || vrs.Engineer.ToLower().Contains(searchStr.ToLower()))
                    orderby vrs.ReleaseTime descending
                    select new VersionDTO
                    {
                        VersionID = vrs.VersionID,
                        VersionName = vrs.VersionName,
                        ProgramName = vrs.PROGRAM.ProgramName,
                        ModelName = vrs.PROGRAM.ModelName,
                        Engineer = vrs.Engineer,
                        BuildTime = vrs.BuildTime,
                        ReleaseTime = vrs.ReleaseTime,
                        ReleaseNote = vrs.ReleaseNote,
                    }).ToList();
        }
        static public List<VersionDTO> GetVersionsWaitForChecklist()
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from v in db.VERSIONs
                            where !(from av in db.ATE_CHECKLIST
                                    select av.VersionID).Contains(v.VersionID)
                            orderby v.PROGRAM.ModelName, v.VersionName
                            select new VersionDTO
                            {
                                VersionID = v.VersionID,
                                VersionName = v.PROGRAM.ModelName + "_" + v.VersionName,
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<VersionLatestDTO> GetListLatestATEVersion()
        {
            try
            {
                string sqlCommand = "SELECT * FROM funcGetListLatestModelVersion() ORDER BY MODELNAME ,VERSIONLATEST DESC";
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return db.Database.SqlQuery<VersionLatestDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<VersionLatestDTO> GET_ListLatesATEtVersionComparisionMethod()
        {
            try
            {
                List<VersionLatestDTO> listLatestVersion = new List<VersionLatestDTO>();
                List<string> listATEModel = GetListATEModel();
                foreach (string model in listATEModel)
                {
                    List<VersionDTO> listVersionOfModel = GetVersionListByModel(model);
                    string strLatestVersionOfModel = "";

                    if (listVersionOfModel.Count > 0)
                    {
                        strLatestVersionOfModel = listVersionOfModel[0].VersionName;
                        for (int i = 0; i < listVersionOfModel.Count; i++)
                        {
                            if (FoxconnVersionCodeCompare(strLatestVersionOfModel, listVersionOfModel[i].VersionName) < 0)
                            {
                                strLatestVersionOfModel = listVersionOfModel[i].VersionName;
                            }
                        }
                    }
                    
                    VersionLatestDTO versionLatestDTO = new VersionLatestDTO
                    {
                        ModelName = model,
                        VersionLatest = strLatestVersionOfModel
                    };
                    listLatestVersion.Add(versionLatestDTO);
                }
                return listLatestVersion;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<VersionLatestDTO> GET_ListLatesATEtVersionComparisionMethodByProjectType(string projectType)
        {
            try
            {
                List<VersionLatestDTO> listLatestVersion = new List<VersionLatestDTO>();
                List<string> listATEModel = GET_ListATEModelByProjectType(projectType);
                foreach (string model in listATEModel)
                {
                    List<VersionDTO> listVersionOfModel = GetVersionListByModel(model);
                    string strLatestVersionOfModel = "";

                    if (listVersionOfModel.Count > 0)
                    {
                        strLatestVersionOfModel = listVersionOfModel[0].VersionName;
                        for (int i = 0; i < listVersionOfModel.Count; i++)
                        {
                            if (FoxconnVersionCodeCompare(strLatestVersionOfModel, listVersionOfModel[i].VersionName) < 0)
                            {
                                strLatestVersionOfModel = listVersionOfModel[i].VersionName;
                            }
                        }
                    }

                    VersionLatestDTO versionLatestDTO = new VersionLatestDTO
                    {
                        ModelName = model,
                        VersionLatest = strLatestVersionOfModel
                    };
                    listLatestVersion.Add(versionLatestDTO);
                }
                return listLatestVersion;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ========= ATELIST Table =========
        static public List<ATEListDTO> GetAllATEList()
        {
            return (from ate in db.ATE_CHECKLIST             
                    orderby ate.UpdatedAt descending
                    select new ATEListDTO
                    {
                        CheckListID = ate.CheckListID,
                        VersionName = ate.VERSION.VersionName,
                        ModelName = ate.VERSION.PROGRAM.ModelName,
                        Status = ate.Status,
                        PreparedBy = ate.PreparedBy,
                        PreparerNote = ate.PreparerNote,
                        PreparedAt = ate.PreparedAt,
                        CheckerNote = ate.CheckerNote,
                        CheckedBy = ate.CheckedBy,
                        CheckedAt = ate.CheckedAt,
                        ApproverNote = ate.ApproverNote,
                        ApprovedBy = ate.ApprovedBy,
                        ApprovedAt = ate.ApprovedAt,
                        UpdatedAt = ate.UpdatedAt,
                        UpdatedBy = ate.UpdatedBy,
                    }).ToList();
        }
        static public ATEListDTO GetATEListByVersionID(int verID)
        {
            return (from ate in db.ATE_CHECKLIST
                    where ate.VersionID == verID                    
                    select new ATEListDTO
                    {
                        CheckListID = ate.CheckListID,
                        VersionName = ate.VERSION.VersionName,
                        ModelName = ate.VERSION.PROGRAM.ModelName,
                        Status = ate.Status,
                        PreparedBy = ate.PreparedBy,
                        PreparerNote = ate.PreparerNote,
                        PreparedAt = ate.PreparedAt,
                        CheckerNote = ate.CheckerNote,
                        CheckedBy = ate.CheckedBy,
                        CheckedAt = ate.CheckedAt,
                        ApproverNote = ate.ApproverNote,
                        ApprovedBy = ate.ApprovedBy,
                        ApprovedAt = ate.ApprovedAt,
                        UpdatedAt = ate.UpdatedAt,
                        UpdatedBy = ate.UpdatedBy,
                    }).SingleOrDefault();
        }
        static public List<ATEListDTO> GetPreparedATEList()
        {
            return (from ate in db.ATE_CHECKLIST
                    where ate.Status == 1
                    orderby ate.UpdatedAt descending
                    select new ATEListDTO
                    {
                        CheckListID = ate.CheckListID,
                        VersionName = ate.VERSION.VersionName,
                        ModelName = ate.VERSION.PROGRAM.ModelName,
                        Status = ate.Status,
                        PreparedBy = ate.PreparedBy,
                        PreparerNote = ate.PreparerNote,
                        PreparedAt = ate.PreparedAt,
                        CheckerNote = ate.CheckerNote,
                        CheckedBy = ate.CheckedBy,
                        CheckedAt = ate.CheckedAt,
                        ApproverNote = ate.ApproverNote,
                        ApprovedBy = ate.ApprovedBy,
                        ApprovedAt = ate.ApprovedAt,
                        UpdatedAt = ate.UpdatedAt,
                        UpdatedBy = ate.UpdatedBy,
                    }).ToList();
        }

        #endregion

        #region ========= TEST_PLAN Table =========
        static public List<string> GetListModelOfTestPlan()
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from testplan in db.TEST_PLANs
                            where testplan.Status != 0 
                            group testplan by testplan.ModelName into modelTestPlan
                            orderby modelTestPlan.Key ascending
                            select modelTestPlan.Key).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        static public List<TestPlanDTO> GetTestPlanList()
        {
            return (from testPlan in db.TEST_PLANs
                    where testPlan.Status != 0
                    select new TestPlanDTO
                    {
                        TestPlanID = testPlan.TestPlanID,
                        UserID = testPlan.UserID,
                        UserName = testPlan.USER.FullName,
                        ModelName = testPlan.ModelName,
                        TestPlanVersion = testPlan.TestPlanVersion,
                        Author = testPlan.Author,
                        ModifiedAt = testPlan.ModifiedAt,
                        ModifyNote = testPlan.ModifyNote,
                        StoredDir = testPlan.StoredDir,
                        Status = testPlan.Status,
                        CreatedAt = testPlan.CreatedAt,
                        UpdatedAt = testPlan.UpdatedAt,
                        UpdatedBy = testPlan.UpdatedBy,
                    }
                   ).ToList();
        }
        static public List<TestPlanDTO> GetTestPlanByModel(string model)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from testplan in db.TEST_PLANs
                            where testplan.Status != 0 &&
                                  testplan.ModelName.Trim().ToLower() == model.Trim().ToLower()
                            orderby testplan.ModifiedAt.Value descending
                            select new TestPlanDTO
                            {
                                TestPlanID = testplan.TestPlanID,
                                UserID = testplan.UserID,
                                UserName = testplan.USER.FullName,
                                ModelName = testplan.ModelName,
                                TestPlanVersion = testplan.TestPlanVersion,
                                Author = testplan.Author,
                                ModifiedAt = testplan.ModifiedAt,
                                ModifyNote = testplan.ModifyNote,
                                StoredDir = testplan.StoredDir,
                                Status = testplan.Status,
                                CreatedAt = testplan.CreatedAt,
                                UpdatedAt = testplan.UpdatedAt,
                                UpdatedBy = testplan.UpdatedBy,
                            }).ToList();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        static public TestPlanDTO GetTestPlanByID(int testPlanID)
        {
            return (from testPlan in db.TEST_PLANs
                    where testPlan.Status != 0 &&
                          testPlan.TestPlanID == testPlanID
                    select new TestPlanDTO
                    {
                        TestPlanID = testPlan.TestPlanID,
                        UserID = testPlan.UserID,
                        UserName = testPlan.USER.FullName,
                        ModelName = testPlan.ModelName,
                        TestPlanVersion = testPlan.TestPlanVersion,
                        Author = testPlan.Author,
                        ModifiedAt = testPlan.ModifiedAt,
                        ModifyNote = testPlan.ModifyNote,
                        StoredDir = testPlan.StoredDir,
                        Status = testPlan.Status,
                        CreatedAt = testPlan.CreatedAt,
                        UpdatedAt = testPlan.UpdatedAt,
                        UpdatedBy = testPlan.UpdatedBy,
                    }).SingleOrDefault();
        }
        static public bool ExistTestPlanVer(int? testPlanId, string model, string ver)
        {
            TEST_PLAN tmpTestPlan = db.TEST_PLANs.FirstOrDefault(v => v.ModelName.ToLower() == model.ToLower() && v.TestPlanVersion.ToLower() == ver.ToLower());
            if (testPlanId.HasValue)
            {
                tmpTestPlan = db.TEST_PLANs.FirstOrDefault(v => v.TestPlanID != testPlanId && v.ModelName.ToLower() == model.ToLower() && v.TestPlanVersion.ToLower() == ver.ToLower());
            }
            return tmpTestPlan != null;
        }
        static public List<TestPlanPreviewDTO> GetListTestPlanPreview()
        {
            try
            {
                string sqlCommand = "SELECT * FROM funcGetListLatestTestPlanVersion() ORDER BY MODELNAME,VERSIONLATEST DESC";
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return db.Database.SqlQuery<TestPlanPreviewDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public List<TestPlanPreviewDTO> GET_ListTestPlanPreviewByProjectType(string projectType)
        {
            try
            {
                string sqlCommand = "SELECT * FROM funcGetListLatestTestPlanVersionOfProjectType('" + projectType + "') ORDER BY MODELNAME,VERSIONLATEST DESC";
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return db.Database.SqlQuery<TestPlanPreviewDTO>(sqlCommand).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region ========= USER Table =========
        static public List<UserDTO> GetUserList()
        {
            return (from user in db.USERS
                    group user by new {user.UserID, user.FullName} into grpUser
                    orderby grpUser.Key.FullName
                    select new UserDTO
                    {
                        UserID = grpUser.Key.UserID,
                        FullName = grpUser.Key.FullName,
                    }).ToList();
        }
        static public List<UserDTO> GetUserListByRoleID(int roleID)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from user in db.USERS
                            where user.RoleID == roleID
                            group user by new { user.UserID, user.FullName } into grpUser
                            orderby grpUser.Key.FullName
                            select new UserDTO
                            {
                                UserID = grpUser.Key.UserID,
                                FullName = grpUser.Key.FullName,
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        static public List<UserDTO> GetUserListByRoleName(string roleName)
        {
            try
            {
                using (ATEVersionContext db = new ATEVersionContext())
                {
                    return (from user in db.USERS
                            where user.ROLE.RoleName.ToLower() == roleName.ToLower()                            
                            orderby user.FullName
                            select new UserDTO
                            {
                                UserID = user.UserID,
                                FullName = user.FullName,
                                Email = user.Email,
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static public string GetListEmailFromListUsers(List<UserDTO> listUsers)
        {
            try
            {
                string strListEmail = "";
                foreach (UserDTO user in listUsers)
                {
                    strListEmail += user.Email + ",";
                }
                return strListEmail;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        // To be added...
        #region ========= GRR_TABLE =========
        static public List<GRRTableDTO> GetGRRList()
        {
            return (from grr in db.GRR_TABLE
                    where grr.Status != 0
                    orderby grr.GageModel, grr.GageName, grr.PartName
                    select new GRRTableDTO
                    {
                        GRR_ID = grr.GRR_ID,
                        UserID = grr.UserID,
                        Dept = grr.Dept,
                        GageModel = grr.GageModel,
                        GageName = grr.GageName,
                        GageNo = grr.GageNo,
                        PartName = grr.PartName,
                        Specification = grr.Specification,
                        Characteristic = grr.Characteristic,
                        JSON_OperTestResult = grr.JSON_OperTestResult,
                        PreparedBy = grr.PreparedBy,
                        PreparedAt = grr.PreparedAt,
                        PreparedNote = grr.PreparedNote,
                        ApprovedBy = grr.ApprovedBy,
                        ApprovedAt = grr.ApprovedAt,
                        ApproverNote = grr.ApproverNote,
                        Status = grr.Status,
                        CreatedAt = grr.CreatedAt,
                        UpdatedAt = grr.UpdatedAt,
                        UpdatedBy = grr.UpdatedBy,
                    }).ToList();
        }
        static public List<GRRTableDTO> GetGRRRejectedList()
        {
            return (from grr in db.GRR_TABLE
                    where grr.Status == 0
                    orderby grr.GageModel, grr.GageName, grr.PartName
                    select new GRRTableDTO
                    {
                        GRR_ID = grr.GRR_ID,
                        UserID = grr.UserID,
                        Dept = grr.Dept,
                        GageModel = grr.GageModel,
                        GageName = grr.GageName,
                        GageNo = grr.GageNo,
                        PartName = grr.PartName,
                        Specification = grr.Specification,
                        Characteristic = grr.Characteristic,
                        JSON_OperTestResult = grr.JSON_OperTestResult,
                        PreparedBy = grr.PreparedBy,
                        PreparedAt = grr.PreparedAt,
                        PreparedNote = grr.PreparedNote,
                        ApprovedBy = grr.ApprovedBy,
                        ApprovedAt = grr.ApprovedAt,
                        ApproverNote = grr.ApproverNote,
                        Status = grr.Status,
                        CreatedAt = grr.CreatedAt,
                        UpdatedAt = grr.UpdatedAt,
                        UpdatedBy = grr.UpdatedBy,
                    }).ToList();
        }
        static public GRRTableDTO GetGRRByID(int grrID)
        {
            return (from grr in db.GRR_TABLE
                    where grr.GRR_ID == grrID
                    select new GRRTableDTO
                    {
                        GRR_ID = grr.GRR_ID,
                        UserID = grr.UserID,
                        Dept = grr.Dept,
                        GageModel = grr.GageModel,
                        GageName = grr.GageName,
                        GageNo = grr.GageNo,
                        PartName = grr.PartName,
                        Specification = grr.Specification,
                        Characteristic = grr.Characteristic,
                        JSON_OperTestResult = grr.JSON_OperTestResult,
                        PreparedBy = grr.PreparedBy,
                        PreparedAt = grr.PreparedAt,
                        PreparedNote = grr.PreparedNote,
                        ApprovedBy = grr.ApprovedBy,
                        ApprovedAt = grr.ApprovedAt,
                        ApproverNote = grr.ApproverNote,
                        Status = grr.Status,
                        CreatedAt = grr.CreatedAt,
                        UpdatedAt = grr.UpdatedAt,
                        UpdatedBy = grr.UpdatedBy,
                    }).SingleOrDefault();
        }
        #endregion

        #endregion


        #region Other Data Process Functions
        // ====== ====================================== ======
        //              Process data from DTOs
        // ====== ====================================== ======
        static public List<VersionDTO> SetAdditionalVersionsData(List<VersionDTO> listVersionDTOs)
        {
            
            int versionsNum = listVersionDTOs.Count;
            if(versionsNum > 0)
            {
                string firstVername = listVersionDTOs[0].VersionName.Trim();
                double maxVerCode = double.Parse(firstVername.Substring(firstVername.IndexOf('.') + 1));
                int maxVerPos = 0, i = 0;
                if (versionsNum == 1)
                {
                    listVersionDTOs[0].IsLastestDBVersion = 1;
                }


                Dictionary<string, string> dicLatestModelVersions = TETestDataDAO.DictionaryLastestVersionOnline();
                foreach (var version in listVersionDTOs)
                {
                    // Set lastest version of ATE_VERSION database
                    version.LastestVersionName = "No info";
                    if (dicLatestModelVersions.ContainsKey(version.ModelName))
                    {
                        version.LastestVersionName = dicLatestModelVersions[version.ModelName];
                        if (dicLatestModelVersions[version.ModelName] == version.VersionName)
                        {
                            version.IsLastest = 1;
                        }
                        else
                        {
                            version.IsLastest = 2;
                        }

                    }
                    if (version.VersionName == TETestDataDAO.GetInUseVersion(version.ModelName))
                    {
                        version.Usage = 1;
                    }
                    // Set latest version getted from table TE_TEST_DATA
                    string thisVername = version.VersionName.Trim();
                    double thisVerCode = double.Parse(thisVername.Substring(thisVername.IndexOf('.') + 1));
                    /*if (maxVerCode <= thisVerCode)
                    {
                        maxVerCode = thisVerCode;
                        maxVerPos = i;
                    }*/
                    if (FoxconnVersionCodeCompare(firstVername,thisVername) < 0)
                    {
                        firstVername = thisVername;
                        maxVerPos = i;
                    }
                    i++;
                    // Set ATEList status data of version
                    ATEListDTO tmpATEList = GetATEListByVersionID(version.VersionID);
                    if (tmpATEList != null)
                    {
                        version.IsATEListAvailable = 1;

                        if (tmpATEList.Status > 1)
                        {
                            version.IsATEListChecked = 1;
                        }
                    }
                    /*else
                    {
                        version.IsATEListChecked = 2;
                    }*/

                }
                listVersionDTOs[maxVerPos].IsLastestDBVersion = 1;
                if(listVersionDTOs[maxVerPos].LastestVersionName == "No info")
                {
                    listVersionDTOs[maxVerPos].LastestVersionName = listVersionDTOs[maxVerPos].VersionName;
                    listVersionDTOs[maxVerPos].IsLastest = 1;
                    listVersionDTOs[maxVerPos].Usage = 1;
                }
            }
            
            

            return listVersionDTOs;
        }
        static public List<VersionLatestDTO> GetListLatestVersionsFullInfo()
        {
            List<VersionLatestDTO> listLatestVersion = GET_ListLatesATEtVersionComparisionMethod();
            Dictionary<string, string> dictLatestVersionOnline = TETestDataDAO.DictionaryLastestVersionOnline();

            foreach (VersionLatestDTO version in listLatestVersion)
            {
                // Set current on-line using ate version
                version.VersionOnline = version.VersionLatest;
                if (dictLatestVersionOnline.ContainsKey(version.ModelName))
                {
                    version.VersionOnline = dictLatestVersionOnline[version.ModelName]; ;
                }
                // Set number of unchecking atelists
                version.UncheckCount = GetUncheckedALSTVersions(version.ModelName);
                // Set number of no atelists
                version.NoATEListCount = GetNoALSTVersions(version.ModelName);
            }

            return listLatestVersion;
        }
        static public List<VersionLatestDTO> GET_ListLatestVersionsFullInfoByProjectType(string projectType)
        {
            List<VersionLatestDTO> listLatestVersion = GET_ListLatesATEtVersionComparisionMethodByProjectType(projectType);            
            Dictionary<string, string> dictLatestVersionOnline = TETestDataDAO.DictionaryLastestVersionOnline();

            foreach (VersionLatestDTO version in listLatestVersion)
            {
                // Set current on-line using ate version
                version.VersionOnline = version.VersionLatest;
                if (dictLatestVersionOnline.ContainsKey(version.ModelName))
                {
                    version.VersionOnline = dictLatestVersionOnline[version.ModelName]; ;
                }
                // Set number of unchecking atelists
                version.UncheckCount = GetUncheckedALSTVersions(version.ModelName);
                // Set number of no atelists
                version.NoATEListCount = GetNoALSTVersions(version.ModelName);
            }

            return listLatestVersion.OrderBy(version => version.ModelName).ToList();
        }
        static private int FoxconnVersionCodeCompare(string verComp1, string verComp2)
        {
            string subStrVerComp1 = verComp1.Trim().Substring(verComp1.IndexOf("V")+1),
                   subStrVerComp2 = verComp2.Trim().Substring(verComp2.IndexOf("V")+1);
            string[] strArrVerComp1 = subStrVerComp1.Split('.'),
                     strArrVerComp2 = subStrVerComp2.Split('.');

            int higherMark = 0, equalMark = 0, lowerMark = 0;

            if(strArrVerComp1.Length == strArrVerComp2.Length)
            {
                for (int i = 0; i < strArrVerComp1.Length; i++)
                {
                    double dVerComp1PosVal = double.Parse(strArrVerComp1[i].Trim()),
                           dVerComp2PosVal = double.Parse(strArrVerComp2[i].Trim());
                    if(dVerComp1PosVal > dVerComp2PosVal)
                    {
                        higherMark++;
                    }
                    if (dVerComp1PosVal == dVerComp2PosVal)
                    {
                        equalMark++;
                    }
                    if (dVerComp1PosVal < dVerComp2PosVal)
                    {
                        lowerMark++;
                    }
                }
            }
            
            if(higherMark > equalMark && higherMark > lowerMark)
            {
                return 1;
            }
            if (equalMark > higherMark && equalMark > lowerMark)
            {
                return 0;
            }
            if (lowerMark > higherMark && lowerMark > equalMark)
            {
                return -1;
            }
            return 0;
        }
        #endregion
    }
}