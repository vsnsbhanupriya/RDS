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
            login.UserTypes = await GetUserTypes();

            return View(login);
        }

        public async Task<List<SelectListItem>> GetUserTypes()
        {
            var userTypesList = new List<SelectListItem>();
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
                    var userTypes = JsonConvert.DeserializeObject<List<UserType>>(result);
                    foreach (var userType in userTypes)
                    {
                        userTypesList.Add(new SelectListItem() { Text = userType.Name, Value = userType.Id.ToString() });
                    }
                }

                return userTypesList;
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(UserLogin login)
        {
            if (!ModelState.IsValid)
            {
                login.UserTypes = await GetUserTypes();
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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