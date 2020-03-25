using Newtonsoft.Json;
using RDSUX.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace RDSUX.Controllers
{
    public class UserController : Controller
    {
        // GET: User/Details/5
        [HttpGet]
        public async Task<ActionResult> Login()
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            var login = new UserLogin();

            return View(login);
        }

        private async Task<List<UserType>> GetUserTypes()
        {
            var userTypesList = new List<UserType>();
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Project/GetUserTypes");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    userTypesList = JsonConvert.DeserializeObject<List<UserType>>(result);
                }

                return userTypesList.Where(e => e.Id != (int)enumUserType.Admin).ToList();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLogin login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            return View();
        }

        // GET: User/Create
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var user = new User();
            user.UserTypes = await GetUserTypes();
            return View(user);
        }

        // POST: User/Create
        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    user.UserTypes = await GetUserTypes();
                    return View(user);
                }
                // TODO: Add insert logic here

                string baseURL = WebConfigurationManager.AppSettings["baseurl"];
                using (var client = new HttpClient())
                {
                    var model = new UserModel();
                    model.Name = user.Name;
                    model.UserName = user.UserName;
                    model.Password = user.Password;
                    model.Photo = user.Photo != null ? user.Photo.FileName : string.Empty;
                    model.Role = user.Role != null ? user.Role : string.Empty;
                    model.UserType = user.UserType;
                    model.DateOfBirth = user.DateofBirth?.ToShortDateString() ?? string.Empty;
                    model.JoiningDate = user.DateofJoin?.ToShortDateString() ?? string.Empty;
                    model.CompanyName = user.CompanyName;
                    model.Experience = user.Expereince;
                    model.Email = user.Email;
                    model.Address = user.Address;

                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsJsonAsync("/api/Project/AddUser", model);

                    if (response.IsSuccessStatusCode == true)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var userId = JsonConvert.DeserializeObject<string>(result);
                        if (user.Photo != null)
                        {
                            var PhotosPath = "\\SourceFiles\\Photos\\" + userId;
                            if (System.IO.Directory.Exists(Server.MapPath("~") + PhotosPath) == false)
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + PhotosPath);
                            user.Photo.SaveAs(Server.MapPath("~") + PhotosPath + "\\" + string.Format("{0:yyyyMMddHHmmss}", DateTime.Now) + "_" + user.Photo.FileName);
                        }
                    }
                }

                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Users()
        {
            var users = new List<User>();
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync("/api/Project/GetUsers").Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<User>>(result);
                }
            }
            return PartialView(users);
        }

        public async Task<ActionResult> CheckUserName(string name)
        {
            string baseURL = WebConfigurationManager.AppSettings["baseurl"];
            List<User> listUsers = new List<User>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("/api/Project/GetUsers");

                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    listUsers = JsonConvert.DeserializeObject<List<User>>(result);
                    if (listUsers.Any(e => e.UserName.Equals(name, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        return Json(true);
                    }
                    else return Json(false);
                }
                return Json(false);
            }
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}