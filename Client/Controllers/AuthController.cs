using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.Models;
using NETCore.ViewModels;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public class AuthController : Controller
    {
        readonly HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:44343/api/")
        };

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public JsonResult RegisterEmp(EmployeeVM employeeVM)
        {
            var myContent = JsonConvert.SerializeObject(employeeVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var result = client.PostAsync("Account/Register", byteContent).Result;
            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM loginVM)
        {
            var myContent = JsonConvert.SerializeObject(loginVM);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = client.PostAsync("Account/Login", byteContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var data = result.Content.ReadAsStringAsync().Result;
                var handler = new JwtSecurityTokenHandler();
                var tokens = handler.ReadJwtToken(data);

                var token = "Bearer " + data;
                string id = tokens.Claims.First(claim => claim.Type == "Id").Value;
                string username = tokens.Claims.First(claim => claim.Type == "UserName").Value;
                string email = tokens.Claims.First(claim => claim.Type == "Email").Value;
                string role = tokens.Claims.First(claim => claim.Type == "Role").Value;


                HttpContext.Session.SetString("Token", token);
                HttpContext.Session.SetString("Id", id);
                HttpContext.Session.SetString("Role", role);

                if (role == "Admin")
                {
                    return RedirectToAction("Index", "Department");
                }

                else if (role == "User")
                {
                    return RedirectToAction("Index", "User");
                }

                return View(result);

            }

            else
            {
                return View(result);
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Role");
            HttpContext.Session.Remove("Email");
            HttpContext.Session.Remove("Token");

            return RedirectToAction("Login", "Account");
        }

    }
}