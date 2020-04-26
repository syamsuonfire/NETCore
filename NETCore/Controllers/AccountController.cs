using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.Base;
using NETCore.Context;
using NETCore.Models;
using NETCore.Repositories.Data;
using NETCore.ViewModels;

namespace NETCore.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        DynamicParameters param = new DynamicParameters();

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        public IConfiguration _configuration;
        private readonly AccountRepository _repository;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
           RoleManager<Role> roleManager,
            IConfiguration configuration,
            AccountRepository accountRepository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._repository = accountRepository;
        }

        // API POST Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User { };
                    user.Email = employeeVM.Email;
                    user.UserName = employeeVM.Email;
                    user.Id = user.Email;
                    user.PasswordHash = employeeVM.Password;

                    var result = await _userManager.CreateAsync(user, employeeVM.Password); //Membuat Account 
                    result = await _userManager.AddToRoleAsync(user, "Admin"); //Memasukkan ke dalam role Employee
                    if (result.Succeeded)
                    {
                        var post = _repository.InsertEmployee(employeeVM); //Sekalian Memasukkan ke tabel employee 
                        if (post != null)
                        {
                            return Ok("Register Success");
                        }

                    }
                    return BadRequest("Failed to Register");
                }


                catch (Exception)
                {
                    throw;
                }

            }
            return BadRequest(ModelState);
        }


        // API POST Login
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            var result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
            if (result.Succeeded)
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
                {
                    var procName = "SP_Retrieve_AspNetUsers_AspNetRoles";  //SP melihat role dari ID User
                    param.Add("@IdUser", loginVM.UserName); //menambahkan parameter SP
                    var data = connection.Query<UserVM>(procName, param, commandType: CommandType.StoredProcedure);

                    UserVM userVM = new UserVM(); // buat nampung

                    foreach (UserVM row in data)
                    {
                        userVM.Id = row.Id;
                        userVM.UserName = row.UserName;
                        userVM.Email = row.Email;
                        userVM.Role = row.Role;
                    }
                    if (data != null)
                    {
                        var claims = new List<Claim>
                        { //Create claims details based on the user information
                         //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                         //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", userVM.Id),
                        new Claim("UserName", userVM.UserName),
                        new Claim("Email", userVM.Email),
                        new Claim("Role", userVM.Role)
                    };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],
                            _configuration["Jwt:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(1),
                            signingCredentials: signIn);

                        var jwt_token = new JwtSecurityTokenHandler().WriteToken(token);

                        return Ok(jwt_token); //return token

                    }
                    return BadRequest(new { message = "Username or Password is Invalid" });
                }
               
            }

            return BadRequest("Failed");
        }
    


        //API POST Insert Role
        [HttpPost]
        [Route("InsertRole")]
        public async Task<IActionResult> InsertRole(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var role = new Role { };
                    role.Name = userVM.Role;

                    var result = await _roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return Ok("Insert Role Success");
                    }

                    return BadRequest("Failed to Insert Role");
                }

                catch (Exception)
                {
                    throw;
                }

            }
            return BadRequest(ModelState);

        }

        // API Logout
        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logout Success");
        }

        
    }
}