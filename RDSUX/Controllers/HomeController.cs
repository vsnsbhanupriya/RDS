using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
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

        public ActionResult GetContractDwgs(int id)
        {
            var drawings = new List<ContractDWGS>();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage contrctDWGSResponse =  client.GetAsync("/api/Project/GetContractDWGS?projectId="  +id.ToString()).Result;
                if (contrctDWGSResponse.IsSuccessStatusCode)
                {
                    var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                     drawings = JsonConvert.DeserializeObject<List<ContractDWGS>>(result);

                }

                }
            return PartialView(drawings);
        }

        public ActionResult GetEngineeringReviewedDwgs(int id)
        {
            var drawings = new List<EngineerReviewedDrawings>();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage EngineeringReviewedDrawingsResponse = client.GetAsync("/api/Project/GetEngineeringReviewedDrawings?projectId=" + id.ToString()).Result;
                if (EngineeringReviewedDrawingsResponse.IsSuccessStatusCode)
                {
                    var result = EngineeringReviewedDrawingsResponse.Content.ReadAsStringAsync().Result;
                    drawings = JsonConvert.DeserializeObject<List<EngineerReviewedDrawings>>(result);

                }

            }
            return PartialView(drawings);
        }



        public ActionResult GeetRFIResponses(int id)
        {
            var rfiResponses = new List<RFIResponse>();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage contrctDWGSResponse = client.GetAsync("/api/Project/GetRFIResponses?projectId=" + id.ToString()).Result;
                if (contrctDWGSResponse.IsSuccessStatusCode)
                {
                    var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                    rfiResponses = JsonConvert.DeserializeObject<List<RFIResponse>>(result);

                }

            }
            return PartialView(rfiResponses);


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
                       
                        client.BaseAddress = new Uri(baseURL);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/CreateProject", project);

                        if (response.IsSuccessStatusCode == true)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            var projectId = JsonConvert.DeserializeObject<string>(result);
                           
                            if (projectDetailsModel.JobSheet != null)
                            {
                                var jobSheetPath = "\\SourceFiles\\JobSheet\\" + projectId;

                                if (System.IO.Directory.Exists(Server.MapPath("~") + jobSheetPath) == false)
                                    System.IO.Directory.CreateDirectory(Server.MapPath("~") + jobSheetPath);
                                projectDetailsModel.JobSheet.SaveAs(Server.MapPath("~") + jobSheetPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_"+ projectDetailsModel.JobSheet.FileName);
                            }

                            //if (projectDetailsModel.EngineerReviewDrawings != null)
                            //{
                            //    var EngineerReviewDrawingsPath = "\\SourceFiles\\EngineeringDWGS\\" + projectId;

                            //    if (System.IO.Directory.Exists(Server.MapPath("~") + EngineerReviewDrawingsPath) == false)
                            //        System.IO.Directory.CreateDirectory(Server.MapPath("~") + EngineerReviewDrawingsPath);
                            //    projectDetailsModel.EngineerReviewDrawings.SaveAs(Server.MapPath("~") + EngineerReviewDrawingsPath + "\\" + string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + "_" + projectDetailsModel.EngineerReviewDrawings.FileName);
                            //}

                            //if (projectDetailsModel.ContractDWGS != null)
                            //{
                            //    var contractDWGSPath = "\\SourceFiles\\ContractDWGS\\" + projectId;

                            //    if (System.IO.Directory.Exists(Server.MapPath("~") + contractDWGSPath) == false)
                            //        System.IO.Directory.CreateDirectory(Server.MapPath("~") + contractDWGSPath);
                            //    projectDetailsModel.ContractDWGS.SaveAs(Server.MapPath("~") + contractDWGSPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + projectDetailsModel.ContractDWGS.FileName);
                            //}

                            //if (projectDetailsModel.RFIResponses != null)
                            //{
                            //    var RFIResponsesPath = "\\SourceFiles\\RFIResponses\\" + projectId;

                            //    if (System.IO.Directory.Exists(Server.MapPath("~") + RFIResponsesPath) == false)
                            //        System.IO.Directory.CreateDirectory(Server.MapPath("~") + RFIResponsesPath);
                            //    projectDetailsModel.RFIResponses.SaveAs(Server.MapPath("~") + RFIResponsesPath + "\\" + string.Format("{0:yyyy-MM-dd_HH-mm-ss-fff}", DateTime.Now) + "_" + projectDetailsModel.RFIResponses.FileName);
                            //}

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
                        project.JobSheetName = projectDetailsModel.JobSheetName;
                        client.BaseAddress = new Uri(baseURL);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/UpdateProject", project);

                        if (response.IsSuccessStatusCode == true)
                        {
                            if (projectDetailsModel.ContractDWGS != null)
                            {
                                using(var client1 = new HttpClient())
                                {
                                    client1.BaseAddress = new Uri(baseURL);
                                    client1.DefaultRequestHeaders.Accept.Clear();
                                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    HttpResponseMessage contrctDWGSResponse = await client1.PostAsJsonAsync("/api/Project/AddContractDWGS", new ContractDWGS(projectDetailsModel.ContractDWGS.FileName, projectDetailsModel.ProejctId.ToString()));
                                    if (contrctDWGSResponse.IsSuccessStatusCode)
                                    {
                                        var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                                        var contractDrawingId = JsonConvert.DeserializeObject<string>(result);
                                        var projectId = projectDetailsModel.ProejctId;
                                        var contractDWGSPath = "\\SourceFiles\\ContractDWGS\\" + projectId+"_"+contractDrawingId;
                                        if (System.IO.Directory.Exists(Server.MapPath("~") + contractDWGSPath) == false)
                                   System.IO.Directory.CreateDirectory(Server.MapPath("~") + contractDWGSPath);
                                projectDetailsModel.ContractDWGS.SaveAs(Server.MapPath("~") + contractDWGSPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + projectDetailsModel.ContractDWGS.FileName);


                                    }

                                }                            
                                

                            }
                            //rfiResponse
                            if (projectDetailsModel.RFIResponses != null)
                            {
                                using (var client2 = new HttpClient())
                                {
                                    client2.BaseAddress = new Uri(baseURL);
                                    client2.DefaultRequestHeaders.Accept.Clear();
                                    client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    HttpResponseMessage rfiresponse = await client2.PostAsJsonAsync("/api/Project/AddRFIResponse", new RFIResponse(projectDetailsModel.RFIResponses.FileName, projectDetailsModel.ProejctId.ToString()));
                                    if (rfiresponse.IsSuccessStatusCode)
                                    {
                                        var result = rfiresponse.Content.ReadAsStringAsync().Result;
                                        var rfiResponseId = JsonConvert.DeserializeObject<string>(result);
                                        var projectId = projectDetailsModel.ProejctId;
                                        var RFIPath = "\\SourceFiles\\RFIResponses\\" + projectId + "_" + rfiResponseId;
                                        if (System.IO.Directory.Exists(Server.MapPath("~") + RFIPath) == false)
                                            System.IO.Directory.CreateDirectory(Server.MapPath("~") + RFIPath);
                                        projectDetailsModel.RFIResponses.SaveAs(Server.MapPath("~") + RFIPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + projectDetailsModel.RFIResponses.FileName);


                                    }

                                }


                            }
                            //enginerrreview
                            if (projectDetailsModel.EngineerReviewDrawings != null)
                            {
                                using (var client3 = new HttpClient())
                                {
                                    client3.BaseAddress = new Uri(baseURL);
                                    client3.DefaultRequestHeaders.Accept.Clear();
                                    client3.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                    HttpResponseMessage engieeringReviewDrawings = await client3.PostAsJsonAsync("/api/Project/AddEngineeringReview", new EngineerReviewedDrawings(projectDetailsModel.EngineerReviewDrawings.FileName, projectDetailsModel.ProejctId.ToString()));
                                    if (engieeringReviewDrawings.IsSuccessStatusCode)
                                    {
                                        var result = engieeringReviewDrawings.Content.ReadAsStringAsync().Result;
                                        var engineeeringDrawingId = JsonConvert.DeserializeObject<string>(result);
                                        var projectId = projectDetailsModel.ProejctId;
                                        var engPath = "\\SourceFiles\\EngineeringDrawings\\" + projectId + "_" + engineeeringDrawingId;
                                        if (System.IO.Directory.Exists(Server.MapPath("~") + engPath) == false)
                                            System.IO.Directory.CreateDirectory(Server.MapPath("~") + engPath);
                                        projectDetailsModel.EngineerReviewDrawings.SaveAs(Server.MapPath("~") + engPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + projectDetailsModel.EngineerReviewDrawings.FileName);


                                    }

                                }


                            }



                            ModelState.Clear();
                            ViewBag.result = "Record Updated Successfully!";
                            return RedirectToAction("ProjectList");
                        }

                        return RedirectToAction("ProjectList");

                    }
                }

                return RedirectToAction("ProjecList");
            }
            catch(Exception ex)
            {
                return RedirectToAction("Index");
            }
        }
        
        public ActionResult Download(string downloadmodel)
        {
            var model = downloadmodel.Split('_');
            var folder = model[0];
            var contractId = model[1];
            var projectId = model[2];
            var drawings = new List<ContractDWGS>();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage contrctDWGSResponse = client.GetAsync("/api/Project/GetContractDWGS?projectId=" + projectId).Result;
                if (contrctDWGSResponse.IsSuccessStatusCode)
                {
                    var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                    drawings = JsonConvert.DeserializeObject<List<ContractDWGS>>(result);

                }

            }
            var selectedDrawing = drawings.FirstOrDefault(e => e.ContractDrawingId.Equals( contractId, StringComparison.InvariantCultureIgnoreCase));
            if (selectedDrawing != null)
            {
                var fileName = selectedDrawing.FileName;

                var path = "\\SourceFiles\\" + folder + "\\" + projectId + "_" + contractId;
                if (System.IO.Directory.Exists(Server.MapPath("~") + path))
                {
                    
                    var directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~") + path);
                    var fileinfo = directoryInfo.GetFiles().FirstOrDefault();
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileinfo.FullName);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    throw new Exception("File NOt found");
                }
            }
            else
            {
                throw new Exception("File NOt found");
            }
          
            
        }

        public ActionResult DownloadRFIResponse(string downloadmodel)
        {
            var model = downloadmodel.Split('_');
            var folder = model[0];
            var rfiResponseId = model[1];
            var projectId = model[2];
            var rFIResponses = new List<RFIResponse>();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage contrctDWGSResponse = client.GetAsync("/api/Project/GetRFIResponses?projectId=" + projectId).Result;
                if (contrctDWGSResponse.IsSuccessStatusCode)
                {
                    var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                    rFIResponses = JsonConvert.DeserializeObject<List<RFIResponse>>(result);

                }

            }
            var selectedDrawing = rFIResponses.FirstOrDefault(e => e.RfiResponseId.Equals(rfiResponseId, StringComparison.InvariantCultureIgnoreCase));
            if (selectedDrawing != null)
            {
                var fileName = selectedDrawing.FileName;

                var path = "\\SourceFiles\\" + folder + "\\" + projectId + "_" + rfiResponseId;
                if (System.IO.Directory.Exists(Server.MapPath("~") + path))
                {

                    var directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~") + path);
                    var fileinfo = directoryInfo.GetFiles().FirstOrDefault();
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileinfo.FullName);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    throw new Exception("File NOt found");
                }
            }
            else
            {
                throw new Exception("File NOt found");
            }


        }

        public ActionResult DownloadEngineeringRiewedDrawings(string downloadmodel)
        {
            var model = downloadmodel.Split('_');
            var folder = model[0];
            var engDrawingId = model[1];
            var projectId = model[2];
            var rFIResponses = new List<EngineerReviewedDrawings>();

            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage contrctDWGSResponse = client.GetAsync("/api/Project/GetEngineeringReviewedDrawings?projectId=" + projectId).Result;
                if (contrctDWGSResponse.IsSuccessStatusCode)
                {
                    var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                    rFIResponses = JsonConvert.DeserializeObject<List<EngineerReviewedDrawings>>(result);

                }

            }
            var selectedDrawing = rFIResponses.FirstOrDefault(e => e.EngineeringDrawingId.Equals(engDrawingId, StringComparison.InvariantCultureIgnoreCase));
            if (selectedDrawing != null)
            {
                var fileName = selectedDrawing.FileName;

                var path = "\\SourceFiles\\" + folder + "\\" + projectId + "_" + engDrawingId;
                if (System.IO.Directory.Exists(Server.MapPath("~") + path))
                {

                    var directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~") + path);
                    var fileinfo = directoryInfo.GetFiles().FirstOrDefault();
                    byte[] fileBytes = System.IO.File.ReadAllBytes(fileinfo.FullName);

                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
                else
                {
                    throw new Exception("File NOt found");
                }
            }
            else
            {
                throw new Exception("File NOt found");
            }


        }



        public async Task<ActionResult> DeleteDocument(string downloadmodel)
        {

            var model = downloadmodel.Split('_');
            var folder = model[0];
            var contractId = model[1];
            var projectId = model[2];
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var contrctdwgs = new ContractDWGS { ProjectId = projectId, ContractDrawingId = contractId };
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/DeleteContractDWGS", contrctdwgs);

                if (response.IsSuccessStatusCode)
                {
                    var path = "\\SourceFiles\\" + folder + "\\" + projectId + "_" + contractId;

                    if (System.IO.Directory.Exists(Server.MapPath("~") + path))
                    {

                        var directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~") + path);
                        foreach (var file in directoryInfo.GetFiles())
                        {
                            file.Delete();
                        }
                        directoryInfo.Delete();
                        return RedirectToAction("EditProject", new RouteValueDictionary(
        new { controller = "Home", action = "EditProject", Id = projectId }));
                    }
                    
                }
            }




            throw new Exception("Something went wrong.");
        }

        public async Task<ActionResult> DeleteRFIDocument(string downloadmodel)
        {

            var model = downloadmodel.Split('_');
            var folder = model[0];
            var RfiId = model[1];
            var projectId = model[2];
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var rfiResponse = new RFIResponse { ProjectId = projectId, RfiResponseId = RfiId };
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/DeleteRFIResponses", rfiResponse);

                if (response.IsSuccessStatusCode)
                {
                    var path = "\\SourceFiles\\" + folder + "\\" + projectId + "_" + RfiId;

                    if (System.IO.Directory.Exists(Server.MapPath("~") + path))
                    {

                        var directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~") + path);
                        foreach (var file in directoryInfo.GetFiles())
                        {
                            file.Delete();
                        }
                        directoryInfo.Delete();
                        return RedirectToAction("EditProject", new RouteValueDictionary(
        new { controller = "Home", action = "EditProject", Id = projectId }));
                    }

                }
            }




            throw new Exception("Something went wrong.");
        }

        public async Task<ActionResult> DeleteEngineeringReviewedDocument(string downloadmodel)
        {

            var model = downloadmodel.Split('_');
            var folder = model[0];
            var engineeredReviewedDocId = model[1];
            var projectId = model[2];
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var engineeringReviewedDrawring = new EngineerReviewedDrawings { ProjectId = projectId, EngineeringDrawingId = engineeredReviewedDocId };
                HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/DeleteEngineeringDrawings", engineeringReviewedDrawring);

                if (response.IsSuccessStatusCode)
                {
                    var path = "\\SourceFiles\\" + folder + "\\" + projectId + "_" + engineeredReviewedDocId;

                    if (System.IO.Directory.Exists(Server.MapPath("~") + path))
                    {

                        var directoryInfo = new System.IO.DirectoryInfo(Server.MapPath("~") + path);
                        foreach (var file in directoryInfo.GetFiles())
                        {
                            file.Delete();
                        }
                        directoryInfo.Delete();
                        return RedirectToAction("EditProject", new RouteValueDictionary(
        new { controller = "Home", action = "EditProject", Id = projectId }));
                    }

                }
            }




            throw new Exception("Something went wrong.");
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
                    pdm.JobSheetName = selectedProject.JobSheetName;
                    
                   
                    

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

        [HttpPost]
        public  async Task<ActionResult> UploadDocuments(Project project)
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var documentfile = System.Web.HttpContext.Current.Request.Files["contractDWGSFile"];
                    var projectId = System.Web.HttpContext.Current.Request.Form["projectId"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(documentfile);
                    var fileName = Path.GetFileName(filebase.FileName);

                    using (var client1 = new HttpClient())
                    {
                        client1.BaseAddress = new Uri(baseURL);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage contrctDWGSResponse = await client1.PostAsJsonAsync("/api/Project/AddContractDWGS", new ContractDWGS(fileName, projectId));
                        if (contrctDWGSResponse.IsSuccessStatusCode)
                        {
                            var result = contrctDWGSResponse.Content.ReadAsStringAsync().Result;
                            var contractDrawingId = JsonConvert.DeserializeObject<string>(result);
                          
                            var contractDWGSPath = "\\SourceFiles\\ContractDWGS\\" + projectId + "_" + contractDrawingId;
                            if (System.IO.Directory.Exists(Server.MapPath("~") + contractDWGSPath) == false)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + contractDWGSPath);
                            filebase.SaveAs(Server.MapPath("~") + contractDWGSPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + fileName);

                            return Json("OK");
                        }
                        return Json("Unable to Upload");

                    }



                }
                else { return Json("No File Saved."); }
            }
            catch (Exception ex) { return Json("Error While Saving."); }
        }

        [HttpPost]
        public async Task<ActionResult> UploadEngineerReviewDocuments(Project project)
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var documentfile = System.Web.HttpContext.Current.Request.Files["EngReviewDwgFile"];
                    var projectId = System.Web.HttpContext.Current.Request.Form["projectId"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(documentfile);
                    var fileName = Path.GetFileName(filebase.FileName);

                    using (var client1 = new HttpClient())
                    {
                        client1.BaseAddress = new Uri(baseURL);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage engieeringReviewDrawings = await client1.PostAsJsonAsync("/api/Project/AddEngineeringReview", new EngineerReviewedDrawings(fileName, projectId));

                        if (engieeringReviewDrawings.IsSuccessStatusCode)
                        {
                            var result = engieeringReviewDrawings.Content.ReadAsStringAsync().Result;
                            var engneerReviewDrawingId = JsonConvert.DeserializeObject<string>(result);
                            var engPath = "\\SourceFiles\\EngineeringDrawings\\" + projectId + "_" + engneerReviewDrawingId;
                            if (System.IO.Directory.Exists(Server.MapPath("~") + engPath) == false)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + engPath);
                            filebase.SaveAs(Server.MapPath("~") + engPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + fileName);

                            return Json("OK");
                        }
                        return Json("Unable to Upload");

                    }



                }
                else { return Json("No File Saved."); }
            }
            catch (Exception ex) { return Json("Error While Saving."); }
        }


        [HttpPost]
        public async Task<ActionResult> UploadRFIDocuments(Project project)
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            try
            {
                if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    var documentfile = System.Web.HttpContext.Current.Request.Files["RFIResponseFile"];
                    var projectId = System.Web.HttpContext.Current.Request.Form["projectId"];
                    HttpPostedFileBase filebase = new HttpPostedFileWrapper(documentfile);
                    var fileName = Path.GetFileName(filebase.FileName);

                    using (var client1 = new HttpClient())
                    {
                        client1.BaseAddress = new Uri(baseURL);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage rfiresponse = await client1.PostAsJsonAsync("/api/Project/AddRFIResponse", new RFIResponse(fileName, projectId));

                        if (rfiresponse.IsSuccessStatusCode)
                        {
                            var result = rfiresponse.Content.ReadAsStringAsync().Result;
                            var rfiResponseId = JsonConvert.DeserializeObject<string>(result);
                            var RFIPath = "\\SourceFiles\\RFIResponses\\" + projectId + "_" + rfiResponseId;
                            
                            if (System.IO.Directory.Exists(Server.MapPath("~") + RFIPath) == false)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + RFIPath);
                            filebase.SaveAs(Server.MapPath("~") + RFIPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + fileName);

                            return Json("OK");
                        }
                        return Json("Unable to Upload");

                    }



                }
                else { return Json("No File Saved."); }
            }
            catch (Exception ex) { return Json("Error While Saving."); }
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
                    ViewBag.result = "Record Deleted Successfully!";
                    return RedirectToAction("ProjectList", new { redirectResult = ViewBag.result });
                }
            }
            return RedirectToAction("ProjectList");
        }

    }
}
