﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NETCore.Repositories.Interface;

namespace NETCore.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasesController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>

    {
        private readonly TRepository _repository;

        //Constructor
        public BasesController(TRepository repository)
        {
            this._repository = repository;
        }

        // API GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<IEntity>> Get(int id)
        {
            var get = await _repository.Get(id);
            if (get == null)
            {
                return NotFound();
            }
            return Ok(get);
        }


        // API POST
        [HttpPost]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
         entity.CreateDate = DateTimeOffset.Now;
         var post = await _repository.Post(entity);
         if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }


        //API DELETE
        /*       [HttpDelete("{id}")]
               public async Task<ActionResult<TEntity>> Delete(int id)
               {
                   var delete = await _repository.Delete(id);
                   if (delete == null)
                   {
                       return NotFound();
                   }
                   return delete;
               }


                              // API GET ALL
                                public async Task<ActionResult<TEntity>> Get()
                                {
                                    var get = await _repository.Get();
                                    return Ok(new { data = get });
                                }

                            */
    }
}