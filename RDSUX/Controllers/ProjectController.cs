using RDSUX.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace RDSUX.Controllers
{
    public class ProjectController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public IHttpActionResult GetProjectType()
        {
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetProjectType");
            ProjectDetailsModel pdm = new ProjectDetailsModel();
            List<ProjectType> lstProjectType = new List<ProjectType>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ProjectType pt = new ProjectType();
                    pt.ProjectName = dr[1].ToString();
                    pt.ProjectTypeId = Convert.ToInt32(dr[0].ToString());
                    lstProjectType.Add(pt);
                }
                // pdm.ProjectType = lstProjectType;
            }
            else
            {
                return NotFound();
            }
            return Ok(lstProjectType);
        }

        [HttpGet]
        public IHttpActionResult GetBarCode(string projectId)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };

            sd.Add("@ProejctId", projectId.ToString());
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetBarCode", sd);
            List<BarCode> lstBarcode = new List<BarCode>();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var barCode = new BarCode()
                    {
                        BarCodeId = Convert.ToInt32(dr[0]),
                        StockLength = Convert.ToInt32(dr[1]),
                        BarCodeGrade = dr[2].ToString(),
                        StandardSplice = Convert.ToInt32(dr[3]),
                        MachanicSplice = Convert.ToInt32(dr[4])
                    };
                    lstBarcode.Add(barCode);
                }
            }
            return Ok(lstBarcode);
        }

        [HttpGet]
        public IHttpActionResult GetScopeOfWork()
        {
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetScopeOfWork");
            ProjectDetailsModel pdm = new ProjectDetailsModel();
            List<ScopeOfWork> lstSow = new List<ScopeOfWork>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    ScopeOfWork sow = new ScopeOfWork();
                    sow.ScopeOfWorkId = Convert.ToInt32(dr[0].ToString());
                    sow.ScopeOfWorkName = dr[1].ToString();
                    lstSow.Add(sow);
                }
                // pdm.ScopeOfWork = lstSow;
            }
            else
            {
                return NotFound();
            }
            return Ok(lstSow);
        }

        [HttpPost]
        public IHttpActionResult CreateProject(Project project)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };

            sd.Add("@ProjectName", project.ProjectName);
            sd.Add("@Notes", project.Notes);
            sd.Add("@PurchaseOrder", project.PurchaseOrder);
            sd.Add("@AssignDate", project.AssignDate.ToString("yyyyMMddHHmmss"));
            sd.Add("@CreateDate", project.CreateDate.ToString("yyyyMMddHHmmss"));
            sd.Add("@CreatedBy", project.CreateBy);
            sd.Add("@JobNumber", project.JobNumber.ToString());
            sd.Add("@ProjectType_ProjectTypeId", project.ProjetTypeId.ToString());
            sd.Add("@ScopeOfWork_ScopeOfWorkId", project.ScopeOfWorkId.ToString());
            sd.Add("@Status_StatusId", project.StatusId.ToString());
            sd.Add("@StockLength", project.StockLength.ToString());
            sd.Add("@BarCodeGrade", project.BarCodeGrade);
            sd.Add("@StandardSplice", project.StandardSplice.ToString());
            sd.Add("@MachanicSplice", project.MechanicSplice.ToString());
            sd.Add("@JobSheetName", project.JobSheetName);

            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_InserProject", "ProejctId", sd);
            var projectId = retvalue.Tables[0].Rows[0][0].ToString();
            return Ok(projectId);
        }

        [HttpPost]
        public IHttpActionResult AddShopDrawings(ShopDrawings shopDrawings)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", shopDrawings.ProjectId);
            sd.Add("@fileName", shopDrawings.FileName);
            sd.Add("@timeStamp", shopDrawings.TImeStamp);

            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_InsertShopDrawings", "ShopDrawingId", sd);
            var shopDrawingId = retvalue.Tables[0].Rows[0][0].ToString();
            return Ok(shopDrawingId);
        }

        [HttpPost]
        public IHttpActionResult AddContractDWGS(ContractDWGS contractDWGS)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", contractDWGS.ProjectId);
            sd.Add("@fileName", contractDWGS.FileName);
            sd.Add("@timeStamp", contractDWGS.TImeStamp);

            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_InsertContractDWGS", "ContractDWGSId", sd);
            var ContractDWGSId = retvalue.Tables[0].Rows[0][0].ToString();
            return Ok(ContractDWGSId);
        }

        [HttpGet]
        public IHttpActionResult GetContractDWGS(string projectId)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", projectId.ToString());
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetContractDWGS", sd);
            var contractDwgs = new List<ContractDWGS>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var drawings = new ContractDWGS { ProjectId = dr[0].ToString(), ContractDrawingId = dr[1].ToString(), FileName = dr[2].ToString(), TImeStamp = dr[3].ToString() };
                    contractDwgs.Add(drawings);
                }
            }
            return Ok(contractDwgs);
        }

        [HttpGet]
        public IHttpActionResult GetShopDrawings(string projectId)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", projectId.ToString());
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetShopDrawings", sd);
            var shopDrawings = new List<ShopDrawings>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var drawings = new ShopDrawings { ProjectId = dr[0].ToString(), ShopDrawingId = dr[1].ToString(), FileName = dr[2].ToString(), TImeStamp = dr[3].ToString() };
                    shopDrawings.Add(drawings);
                }
            }
            return Ok(shopDrawings);
        }

        [HttpGet]
        public IHttpActionResult GetRFIResponses(string projectId)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", projectId.ToString());
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetRFIResponses", sd);
            var rfiResponses = new List<RFIResponse>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var rfiResponse = new RFIResponse { ProjectId = dr[0].ToString(), RfiResponseId = dr[1].ToString(), FileName = dr[2].ToString(), TImeStamp = dr[3].ToString() };
                    rfiResponses.Add(rfiResponse);
                }
            }
            return Ok(rfiResponses);
        }

        [HttpGet]
        public IHttpActionResult GetEngineeringReviewedDrawings(string projectId)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", projectId.ToString());
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetEngineeringReviewedDrawings", sd);
            var engineerReviewedDrawings = new List<EngineerReviewedDrawings>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    var engReviewedDrawing = new EngineerReviewedDrawings { ProjectId = dr[0].ToString(), EngineeringDrawingId = dr[1].ToString(), FileName = dr[2].ToString(), TImeStamp = dr[3].ToString() };
                    engineerReviewedDrawings.Add(engReviewedDrawing);
                }
            }
            return Ok(engineerReviewedDrawings);
        }

        [HttpPost]
        public IHttpActionResult DeleteEngineeringDrawings(EngineerReviewedDrawings engDrawings)
        {
            if (engDrawings == null)
                return BadRequest();
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", engDrawings.ProjectId);
            sd.Add("@EngReviewdDrawingId", engDrawings.EngineeringDrawingId);
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_DeleteEngineeringReviewedDrawings", sd);
            return Ok(retvalue);
        }

        [HttpPost]
        public IHttpActionResult DeleteContractDWGS(ContractDWGS contractDWGS)
        {
            if (contractDWGS == null)
                return BadRequest();
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", contractDWGS.ProjectId);
            sd.Add("@ContractId", contractDWGS.ContractDrawingId);
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_DeleteContractDWGS", sd);
            return Ok(retvalue);
        }

        [HttpPost]
        public IHttpActionResult DeleteRFIResponses(RFIResponse rfiResponse)
        {
            if (rfiResponse == null)
                return BadRequest();
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", rfiResponse.ProjectId);
            sd.Add("@RFiResponseId", rfiResponse.RfiResponseId);
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_DeleteRFIResponses", sd);
            return Ok(retvalue);
        }
       
       [HttpPost]
        public IHttpActionResult DeleteShopDrawings(ShopDrawings shopDrawings)
        {
            if (shopDrawings == null)
                return BadRequest();
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", shopDrawings.ProjectId);
            sd.Add("@shopDrawingId", shopDrawings.ShopDrawingId);
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_DeleteShopDrawings", sd);
            return Ok(retvalue);
        }

        [HttpPost]
        public IHttpActionResult DeleteJobSheet([FromBody]int projectId)
        {
            if (projectId <= 0)
                return BadRequest();
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", projectId.ToString());

            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_Delete_JobSheet", sd);
            return Ok(retvalue);
        }

        [HttpPost]
        public IHttpActionResult AddRFIResponse(RFIResponse rfiresponse)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", rfiresponse.ProjectId);
            sd.Add("@fileName", rfiresponse.FileName);
            sd.Add("@timeStamp", rfiresponse.TImeStamp);

            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_InsertRFIResponse", "RFIResponseId", sd);

            var ContractDWGSId = retvalue.Tables[0].Rows[0][0].ToString();
            return Ok(ContractDWGSId);
        }

        [HttpPost]
        public IHttpActionResult AddEngineeringReview(EngineerReviewedDrawings engineerReviewDrawings)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };
            sd.Add("@ProejctId", engineerReviewDrawings.ProjectId);
            sd.Add("@fileName", engineerReviewDrawings.FileName);
            sd.Add("@timeStamp", engineerReviewDrawings.TImeStamp);

            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_InsertEngineeringReviewed", "EngineeringReviewId", sd);
            var ContractDWGSId = retvalue.Tables[0].Rows[0][0].ToString();
            return Ok(ContractDWGSId);
        }

        [HttpPost]
        public IHttpActionResult UpdateProject(Project project)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };

            sd.Add("@ProjectName", project.ProjectName);
            sd.Add("@Notes", project.Notes);
            sd.Add("@PurchaseOrder", project.PurchaseOrder);
            sd.Add("@AssignDate", project.AssignDate.ToString("yyyyMMddHHmmss"));
            sd.Add("@CreateDate", project.CreateDate.ToString("yyyyMMddHHmmss"));
            sd.Add("@CreatedBy", project.CreateBy);
            sd.Add("@JobNumber", project.JobNumber.ToString());
            sd.Add("@ProjectType_ProjectTypeId", project.ProjetTypeId.ToString());
            sd.Add("@ScopeOfWork_ScopeOfWorkId", project.ScopeOfWorkId.ToString());
            sd.Add("@Status_StatusId", project.StatusId.ToString());
            sd.Add("@ProejctId", project.ProejctId.ToString());

            sd.Add("@StockLength", project.StockLength.ToString());
            sd.Add("@BarCodeGrade", project.BarCodeGrade);
            sd.Add("@StandardSplice", project.StandardSplice.ToString());
            sd.Add("@MachanicSplice", project.MechanicSplice.ToString());
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_UpdateProject", sd);
            return Ok(retvalue);
        }

        [HttpPost]
        public IHttpActionResult DeleteProject([FromBody] string Id)
        {
            SortedDictionary<string, string> sd = new SortedDictionary<string, string>() { };

            sd.Add("@ProejctId", Id);
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_Delete_Project", sd);
            return Ok(retvalue);
        }

        [HttpGet]
        public IHttpActionResult GetProjectList()
        {
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet ds = rdsService.SelectList("USP_GetProjects");

            List<Project> lstProjects = new List<Project>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Project project = new Project();
                    project.ProjectName = dr["ProjectName"].ToString();
                    project.AssignDate = Convert.ToDateTime(dr["AssignDate"]);
                    project.CreateBy = dr["CreatedBy"].ToString();
                    project.CreateDate = Convert.ToDateTime(dr["CreateDate"]);
                    project.JobNumber = Convert.ToInt32(dr["JobNumber"].ToString());
                    project.Notes = dr["Notes"].ToString();
                    project.ProejctId = Convert.ToInt32(dr["ProejctId"].ToString());
                    project.PurchaseOrder = dr["PurchaseOrder"].ToString();
                    project.ProjetTypeId = Convert.ToInt32(dr["ProjectType_ProjectTypeId"].ToString());
                    project.ScopeOfWorkId = Convert.ToInt32(dr["ScopeOfWork_ScopeOfWorkId"].ToString());
                    project.StatusId = Convert.ToInt32(dr["Status_StatusId"].ToString());
                    project.JobSheetName = dr["JobSheetName"].ToString();
                    lstProjects.Add(project);
                }
            }
            else
            {
                return NotFound();
            }
            return Ok(lstProjects);
        }
    }
}