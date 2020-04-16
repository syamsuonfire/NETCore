using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Models;
using NETCore.Repositories.Data;
using NETCore.ViewModels;

namespace NETCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BasesController<Employee, EmployeeRepository>
    {
        private readonly EmployeeRepository _repository;
        public EmployeeController(EmployeeRepository employeeRepository) : base (employeeRepository)
        {
            this._repository = employeeRepository;
        }

        // API GET ALL

        [HttpGet]
        public async Task<IEnumerable<EmployeeVM>> Get()
        {
            return await _repository.GetAll();
        }


        // API GET BY ID

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeVM>> Get(int id)
        {
            var get = await _repository.GetById(id);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }



        // API UPDATE Employee

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> Put(int id, Employee entity)
        {
            var put = await _repository.Get(id);

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
            return Ok ("Update Succesfull");
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
 