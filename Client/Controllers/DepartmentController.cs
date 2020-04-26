using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Models;
using NETCore.ViewModels;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class DepartmentController : Controller
    {
    //    readonly HttpClient client = new HttpClient();



        // Get Department
        public IActionResult Index()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44343/api/")
            };
            // var role = HttpContext.Session.GetString("Role");
          //  if (role == "Admin")
            { 
                return View(); // Menampilkan data berdasarkan fungsi loaddepartment
            }
        //   return RedirectToAction("User Denied");
        }

        public JsonResult LoadDepartment()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44343/api/")
            };

        //    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
       //     client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            DepartmentJson departmentVM = null;
            var responseTask = client.GetAsync("Department/"); // Akses data dari Department API
            responseTask.Wait(); // Wait for the task to complete execution
            var result = responseTask.Result;
            if (result.IsSuccessStatusCode) // If access success
            {
                var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                departmentVM = JsonConvert.DeserializeObject<DepartmentJson>(json); // Tampung setiap data di dalam department
            }
            else
            {
                ModelState.AddModelError(string.Empty, "server error, try after some time");
            }
            return Json(departmentVM);
        }

        // Create/Update Department

        public JsonResult InsertOrUpdate(Department department)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44343/api/")
            };

         //   client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var myContent = JsonConvert.SerializeObject(department);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            if (department.Id == 0) //insert
            {
                var result = client.PostAsync("Department/", byteContent).Result;
                return Json(result);
            }
            else //update
            {
                var result = client.PutAsync("Department/" + department.Id, byteContent).Result;
                return Json(result);
            }
        }

        // DELETE: Department

        public JsonResult Delete(int id)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44343/api/")
            };

          //  client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            var result = client.DeleteAsync("Department/" + id).Result;
            return Json(result);
        }


        // GETBYID: Department

          public JsonResult GetById(int Id)
              {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44343/api/")
            };

        //    client.DefaultRequestHeaders.Add("Authorization", HttpContext.Session.GetString("Token"));
            DepartmentVM departmentVM = null;
                  var responseTask = client.GetAsync("Department/" + Id);
                  responseTask.Wait();
                  var result = responseTask.Result;
                  if (result.IsSuccessStatusCode)
                  {
                      var json = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
                      departmentVM = JsonConvert.DeserializeObject<DepartmentVM>(json);
                  }
                  else
                  {
                      ModelState.AddModelError(string.Empty, "Server error try after some time.");
                  }
                  return Json(departmentVM);
              }

    }
}