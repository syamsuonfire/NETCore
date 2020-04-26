using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NETCore.ViewModels;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44343/api/")
        };

        // Get Employee

        public IActionResult Index()
        {
            return View(); // Menampilkan data berdasarkan fungsi loademployee
        }

        public JsonResult LoadEmployee()
        {
            IEnumerable<EmployeeVM> employee = null;
            var responseTask = client.GetAsync("Employee");
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var readTask = result.Content.ReadAsAsync<IList<EmployeeVM>>();
                readTask.Wait();
                employee = readTask.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Server Error");
            }
            return Json(employee);
        }

        // Update Employee

        public JsonResult Update(EmployeeVM employeeVM)
        {
            var myContent = JsonConvert.SerializeObject(employeeVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PutAsync("Employee/" + employeeVM.Email, byteContent).Result;
            return Json(result);
        }

        // Create Employee

        public JsonResult Insert(EmployeeVM employeeVM)
        {
            var myContent = JsonConvert.SerializeObject(employeeVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Account/Register/", byteContent).Result;
            return Json(result);
        }

        // GETBYID Employee
        public JsonResult GetByEmail(string Email)
        {
            object data = null;
            var responseTask = client.GetAsync("Employee/" + Email);
            responseTask.Wait();
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode)
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                data = JsonConvert.SerializeObject(json);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sorry Server Error, Try Again");
            }
            return Json(data);
        }



        // DELETE: Employee
        public JsonResult Delete(string Email)
        {
            var result = client.DeleteAsync("Employee/" + Email).Result;
            return Json(result);
        }
    }
}