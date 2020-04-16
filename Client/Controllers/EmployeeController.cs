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
            var responseTask = client.GetAsync("Employee/"); // Akses data dari Employee API
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

        // Create/Update Employee

        public JsonResult InsertOrUpdate(EmployeeVM employee)
        {
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            if (employee.Id == 0) //insert
            {
                var result = client.PostAsync("Employee/", byteContent).Result;
                return Json(result);
            }
            else //update
            {
                var result = client.PutAsync("Employee/" + employee.Id, byteContent).Result;
                return Json(result);
            }
        }

        // GETBYID Employee
        public JsonResult GetById(int Id)
        {
            IEnumerable<EmployeeVM> employee = null;
            var responseTask = client.GetAsync("Employee/" + Id);
            responseTask.Wait();
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



        // DELETE: Employee
        public JsonResult Delete(int Id)
        {
            var result = client.DeleteAsync("Employee/" + Id).Result;
            return Json(result);
        }
    }
}