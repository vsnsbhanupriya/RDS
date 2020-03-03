using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Newtonsoft.Json;
using RDSUX.Models;

namespace RDSUX.Controllers
{
    public class HomeController : Controller
    {
       
       public async Task<ActionResult> Index(string redirectResult="")
        {
            ViewBag.Title = "Home Page";
            ViewBag.result = redirectResult;
            ProjectDetailsModel pdm = new ProjectDetailsModel();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];

            using (var client = new HttpClient())
            {



                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/Project/GetProjectType");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<ProjectType> projectTypes = JsonConvert.DeserializeObject<List<ProjectType>>(result);
                    if (result != null)
                    {
                        pdm.ProjectTypeList = projectTypes.Select(x => new SelectListItem { Value = x.ProjectTypeId.ToString(), Text = x.ProjectName, Selected = x.ProjectTypeId == pdm.ProjectTypeId });

                       
                    }
                }
             //   ViewBag.ProjectTypeModel = pdm.ProjectType;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Project/GetScopeOfWork");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<ScopeOfWork> scopeOfWorks = JsonConvert.DeserializeObject<List<ScopeOfWork>>(result);

                    if (result != null)
                        pdm.ScopeOfWork = scopeOfWorks.Select(x => new SelectListItem { Value = x.ScopeOfWorkId.ToString(), Text = x.ScopeOfWorkName, Selected = x.ScopeOfWorkId == pdm.ScopeOfWorkId }); ;
                }
                ViewBag.SOWModel = pdm.ScopeOfWork;
            }
            pdm.ProjectName = "";
            pdm.Notes = "";
            pdm.JobNumber = "";
            pdm.PurchaseOrder = "";
            List<BarCode> lstBarCode = new List<BarCode>();
            BarCode barCode = new BarCode();
            lstBarCode.Add(barCode);
            ViewBag.BarCodeModel = lstBarCode;
            pdm.BarCode = lstBarCode;

            ViewBag.ProjectDetailsModel = pdm;
            return View(pdm);
        //    return View();
        }
       
        public async Task<ActionResult> ProjectList()
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            List<Project> lisProject = new List<Project>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Project/GetProjectList");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                     lisProject = JsonConvert.DeserializeObject<List<Project>>(result);

                    if (result != null)
                        ViewBag.ProjectList = lisProject;
                }
                
            }
            return View(lisProject);
        }

        public async Task<ActionResult> CreateProject(ProjectDetailsModel projectDetailsModel, FormCollection formCollection)
        {
            try
            {
                string baseURL = WebConfigurationManager.AppSettings["baseurl"];
                if (ModelState.IsValid)
                {                    
                       using (var client = new HttpClient())
                        {
                        string projectType = formCollection["ProjectTypeModel"].ToString();
                        string sow = formCollection["SOWModel"].ToString();
                        string notes = formCollection["Notes"].ToString();
                        string JobNumber = formCollection["JobNumber"].ToString();
                        string StockLength = formCollection["StockLength"].ToString();
                        string BarCodeGrade = formCollection["BarCodeGrade"].ToString();
                        string StandardSplice = formCollection["StandardSplice"].ToString();
                        string MechanicSplice = formCollection["MachanicSplice"].ToString();
                        var jobsheet = formCollection["fileUploadJobsheet"];
                        Project project = new Project();
                        project.AssignDate = DateTime.Now;
                        project.CreateBy = "System";
                        project.CreateDate =  DateTime.Now;
                        project.JobNumber = Convert.ToInt32(JobNumber);
                        project.Notes = notes;
                        project.PurchaseOrder = string.Empty;
                        project.ProjectName = projectDetailsModel.ProjectName;
                        project.ProjetTypeId = Convert.ToInt32(projectType);
                        project.ScopeOfWorkId = Convert.ToInt32(sow);
                        project.StatusId = 1;
                        project.StockLength = Convert.ToInt32(StockLength);
                        project.BarCodeGrade = BarCodeGrade;
                        project.StandardSplice = Convert.ToInt32(StandardSplice);
                        project.MechanicSplice = Convert.ToInt32(MechanicSplice);
                        project.JobSheetName = projectDetailsModel.JobSheet?.FileName??string.Empty ;
                        project.RFIResponsesName = projectDetailsModel.RFIResponses?.FileName ?? string.Empty;
                        project.ContractDWGSName = projectDetailsModel.ContractDWGS?.FileName ?? string.Empty;
                        project.EngineerReviewDrawingsName = projectDetailsModel.EngineerReviewDrawings?.FileName ?? string.Empty;
                        client.BaseAddress = new Uri(baseURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/CreateProject", project);

                        if (response.IsSuccessStatusCode == true)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            var projectId = JsonConvert.DeserializeObject<string>(result);
                            var path = "\\SourceFiles\\" + projectId ;
                            
                            if(System.IO.Directory.Exists(Server.MapPath("~") + path) ==false)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + path);
                            if(projectDetailsModel.JobSheet!=null)
                            projectDetailsModel.JobSheet.SaveAs(Server.MapPath("~") + path+"\\"+ projectDetailsModel.JobSheet.FileName);
                            if (projectDetailsModel.EngineerReviewDrawings != null)
                                projectDetailsModel.EngineerReviewDrawings.SaveAs(Server.MapPath("~") + path + "\\" + projectDetailsModel.EngineerReviewDrawings.FileName);
                            if (projectDetailsModel.ContractDWGS != null)
                                projectDetailsModel.ContractDWGS.SaveAs(Server.MapPath("~") + path + "\\" + projectDetailsModel.ContractDWGS.FileName);
                            if (projectDetailsModel.RFIResponses != null)
                                projectDetailsModel.RFIResponses.SaveAs(Server.MapPath("~") + path + "\\" + projectDetailsModel.RFIResponses.FileName);
                            ModelState.Clear();
                            ViewBag.result = "Record Inserted Successfully!";
                            return RedirectToAction("Index",new { redirectResult=ViewBag.result }) ;
                        }

                        return RedirectToAction("Index");

                    }
                }

                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> UpdateProject(ProjectDetailsModel projectDetailsModel, FormCollection formCollection)
        {
            try
            {
                string baseURL = WebConfigurationManager.AppSettings["baseurl"];
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        string projectType = formCollection["ProjectTypeModel"].ToString();
                        string sow = formCollection["SOWModel"].ToString();
                        string notes = formCollection["Notes"].ToString();
                        string JobNumber = formCollection["JobNumber"].ToString();
                        string StockLength = formCollection["StockLength"].ToString();
                        string BarCodeGrade = formCollection["BarCodeGrade"].ToString();
                        string StandardSplice = formCollection["StandardSplice"].ToString();
                        string MachanicSplice = formCollection["MachanicSplice"].ToString();
                        string ProjectId = formCollection["ProejctId"].ToString();
                        string PurchaseOrder = formCollection["PurchaseOrder"].ToString();
                    
                        Project project = new Project();
                        project.AssignDate = DateTime.Now;
                        project.CreateBy = "System";
                        project.CreateDate = DateTime.Now;
                        project.JobNumber = Convert.ToInt32(JobNumber);
                        project.Notes = notes;
                        project.PurchaseOrder = PurchaseOrder;
                        project.ProjectName = projectDetailsModel.ProjectName;
                        project.ProjetTypeId = Convert.ToInt32(projectType);
                        project.ScopeOfWorkId = Convert.ToInt32(sow);
                        project.StatusId = 1;
                        project.ProejctId =  Convert.ToInt32(ProjectId);
                        project.StockLength = Convert.ToInt32(StockLength);
                        project.BarCodeGrade = BarCodeGrade;
                        project.StandardSplice = Convert.ToInt32(StandardSplice);
                        project.MechanicSplice = Convert.ToInt32(MachanicSplice);

                        client.BaseAddress = new Uri(baseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/UpdateProject", project);

                        if (response.IsSuccessStatusCode == true)
                        {
                            ModelState.Clear();
                            ViewBag.result = "Record Updated Successfully!";
                            return RedirectToAction("ProjectList");
                        }

                        return RedirectToAction("ProjectList");

                    }
                }

                return RedirectToAction("ProjecList");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> EditProject(string Id)
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            ProjectDetailsModel pdm = new ProjectDetailsModel();
            List<Project> lisProject = new List<Project>();
            Project selectedProject = new Project();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Project/GetProjectList");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    lisProject = JsonConvert.DeserializeObject<List<Project>>(result);

                    if (result != null)
                        ViewBag.ProjectList = lisProject;
                    selectedProject = lisProject.Where(s => s.ProejctId == Convert.ToInt32(Id)).FirstOrDefault();

                    pdm.JobNumber = selectedProject.JobNumber.ToString();
                    pdm.Notes = selectedProject.Notes;
                    pdm.ProjectName = selectedProject.ProjectName;
                    pdm.PurchaseOrder = selectedProject.PurchaseOrder;
                    pdm.ProjectTypeId = selectedProject.ProjetTypeId;
                    pdm.ScopeOfWorkId = selectedProject.ScopeOfWorkId;
                    pdm.ProejctId = selectedProject.ProejctId;
                }

            }
            using (var client = new HttpClient())
            {



                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/Project/GetProjectType");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<ProjectType> projectTypes = JsonConvert.DeserializeObject<List<ProjectType>>(result);
                    if (result != null && projectTypes.Any() )
                    {
                        pdm.ProjectTypeList = projectTypes.Select(x => new SelectListItem { Value = x.ProjectTypeId.ToString(), Text = x.ProjectName, Selected = x.ProjectTypeId == pdm.ProjectTypeId });

                       // pdm.ProjectType = projectTypes;
                    }
                }
              //  ViewBag.ProjectTypeModel = pdm.ProjectType;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Project/GetScopeOfWork");
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<ScopeOfWork> scopeOfWorks = JsonConvert.DeserializeObject<List<ScopeOfWork>>(result);

                    if (result != null)
                        pdm.ScopeOfWork = scopeOfWorks.Select(x => new SelectListItem { Value = x.ScopeOfWorkId.ToString(), Text = x.ScopeOfWorkName, Selected = x.ScopeOfWorkId == pdm.ScopeOfWorkId }); 
                }
             //   ViewBag.SOWModel = pdm.ScopeOfWork;
            }
            List<BarCode> lstBarCode = new List<BarCode>();
            BarCode barCode = new BarCode();
            using (var client=new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("/api/Project/GetBarCode?projectId="+Id);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    List<BarCode> barCodes = JsonConvert.DeserializeObject<List<BarCode>>(result);
                    if (result != null && barCodes.Any())
                    {
                        pdm.BarCode = barCodes;
                        // pdm.ProjectType = projectTypes;
                    }
                }

            }


               // lstBarCode.Add(barCode);
            //ViewBag.BarCodeModel = lstBarCode;
            //pdm.BarCode = lstBarCode;

            ViewBag.ProjectDetailsModel = pdm;
            return View(pdm);
        }
        
        public async Task<ActionResult> DeleteProject(string Id)
        {
            var project = ViewData["ProjectModel"];
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/DeleteProject",Id);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ProjectList");
                }
            }
            return RedirectToAction("ProjectList");
        }

    }
}
