using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Models;
using NETCore.Repositories.Data;
using NETCore.ViewModels;

namespace NETCore.Controllers
{
  //  [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BasesController<Employee, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;
        
        
        //Constructor
        public EmployeeController(EmployeeRepository employeeRepository) : base(employeeRepository)
        {
            this._repository = employeeRepository;
        }



        // API GET ALL
        [HttpGet]
        public async Task<IEnumerable<EmployeeVM>> Get()
        {
            return await _repository.GetAll();
        }



        // API GET BY EMAIL

        [HttpGet("{email}")]
        public async Task<ActionResult<EmployeeVM>> Get(string email)
        {
            var get = await _repository.GetByEmail(email);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(new { data = get });
        }


        // API PUT

        [HttpPut("{email}")]
        public async Task<ActionResult<Employee>> Put(string email, Employee entity)
        {
            var put = await _repository.GetByEmail(email);
            if (put == null)
            {
                return BadRequest();
            }

            if (entity.FirstName != null)
            {
                put.FirstName = entity.FirstName;
            }

            if (entity.LastName != null)
            {
                put.LastName = entity.LastName;
            }

            if (entity.Email != null)
            {
                put.Email = entity.Email;
            }

            if (entity.Address != null)
            {
                put.Address = entity.Address;
            }

            if (entity.BirthDate != default(DateTime))
            {
                put.BirthDate = entity.BirthDate;
            }

            if (entity.PhoneNumber != null)
            {
                put.PhoneNumber = entity.PhoneNumber;
            }

            if (entity.Department_Id != null)
            {
                put.Department_Id = entity.Department_Id;
            }

            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Update Succesfull");
        }

        //API DELETE

        [HttpDelete("{email}")]
        public async Task<ActionResult> Delete(string email)
        {
            var delete = await _repository.Delete(email);
            if (delete == null)
            {
                return NotFound();
            }
            return Ok(delete);
        }

        /*
    [HttpPut("{Id}")]
    public async Task<ActionResult<Employee>> Put(int Id, Employee model)
    {

        var update = await _repository.Get(Id);
        if (update == null)
        {
            return NotFound();
        }

        update.UpdateDate = DateTimeOffset.Now;
        update.FirstName = model.FirstName;
        update.LastName = model.LastName;
        update.Email = model.Email;
        update.BirthDate = model.BirthDate;
        update.PhoneNumber = model.PhoneNumber;
        update.Address = model.Address;
        update.IsDelete = model.IsDelete;
        update.Department_Id = model.Department_Id;
        await _repository.Put(update);
        return Ok("Update Successfully");
    }
        */
    }
}
