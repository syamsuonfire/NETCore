using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Base;
using NETCore.Models;
using NETCore.Repositories.Data;
using NETCore.ViewModels;

namespace NETCore.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : BasesController<Department, DepartmentRepository>
    {
        private readonly DepartmentRepository _repository;
        
        //Constructor
        public DepartmentController(DepartmentRepository repository) : base(repository)
        {
            this._repository = repository;

        }

        // API GET ALL
        [HttpGet]
        public async Task<ActionResult<DepartmentVM>> Get()
        {
            var get = await _repository.Get();
            return Ok(new { data = get });
        }




        // API PUT

        [HttpPut("{id}")]
        public async Task<ActionResult<Department>> Put(int id, Department entity)
        {
            entity.Id = id;
            if (id != entity.Id)
            {
                return BadRequest();
            }
            var put = await _repository.Get(id);
            put.Name = entity.Name;
            put.UpdateDate = DateTimeOffset.Now;
            await _repository.Put(put);
            return Ok("Update Successfully");
        }


        // API DELETE
        [HttpDelete("{id}")]
        public async Task<ActionResult<Department>> Delete(int id)
        {
            var delete = await _repository.Delete(id);
            if (delete == null)
            {
                return NotFound();
            }
            return delete;
        }
    }
}