using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.Context;
using NETCore.Models;
using NETCore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, MyContext>
    {

        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;

        //Constructor
        public EmployeeRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this._configuration = configuration;
            _myContext = myContext;
        }

        //REPO GET ALL
        public async Task<IEnumerable<EmployeeVM>> GetAll()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_GetAll_TB_M_Employee";

                var data = await connection.QueryAsync<EmployeeVM>(procName, commandType: CommandType.StoredProcedure);
                return data;
            }
        }


           //REPO GET BY EMAIL
           public async Task<Employee> GetByEmail(string email)
           {
               return await _myContext.Set<Employee>().FindAsync(email);
           }

           /*
           //REPO GET BY ID FOR UPDATE
           public async Task<IEnumerable<EmployeeVM>> GetIdEmployee(int Id)
           {
               using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
               {
                   var procName = "SP_GetEmployeeById";
                   parameters.Add("@pEmpId", Id);
                   var data = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                   return data;
               }
           }
           */

        //REPO GET BY EMAIL FOR UPDATE
        public async Task<IEnumerable<EmployeeVM>> GetEmailEmp(string email)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
            {
                var procName = "SP_RetrieveByEmail_TB_M_Employee";
                parameters.Add("@Email", email);
                var data = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                return data;
            }
        }

        //REPO DELETE
        public async Task<Employee> Delete(string email)
        {
            var entity = await GetByEmail(email);
            if (entity == null)
            {
                return entity;
            }
            entity.IsDelete = true;
            entity.DeleteDate = DateTimeOffset.Now;
            _myContext.Entry(entity).State = EntityState.Modified;
            await _myContext.SaveChangesAsync();
            return entity;
        }






       




    }
}