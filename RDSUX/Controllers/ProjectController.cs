using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RDSUX.Models;
using RDSService;
using System.Data;

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
                foreach(DataRow dr in ds.Tables[0].Rows)
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
            if (ds != null && ds.Tables[0].Rows.Count>0)
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
            sd.Add("@DWGSName", project.ContractDWGSName);
            sd.Add("@RFIResponseName", project.RFIResponsesName);
            sd.Add("@EnggDrawingsName", project.EngineerReviewDrawingsName);
            RDSService.RDSService rdsService = new RDSService.RDSService();
            DataSet retvalue = rdsService.SelectList("USP_InserProject", "ProejctId", sd );
            var projectId = retvalue.Tables[0].Rows[0][0].ToString();
            return Ok(projectId);
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