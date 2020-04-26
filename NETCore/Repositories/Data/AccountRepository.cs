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
    public class AccountRepository : GeneralRepository<Employee, MyContext>
    {
        DynamicParameters parameters = new DynamicParameters();
        IConfiguration _configuration { get; }
        private readonly MyContext _myContext;
        public AccountRepository(MyContext myContext, IConfiguration configuration) : base(myContext)
        {
            this._configuration = configuration;
            _myContext = myContext;
        }


        //REPO INSERT ACCOUNT WHEN INSERT EMPLOYEE
        public async Task<IEnumerable<EmployeeVM>> InsertEmployee(EmployeeVM employeeVM)
             {
                 using (var connection = new SqlConnection(_configuration.GetConnectionString("MyNetCoreConnection")))
                 {
                     var procName = "SP_InsertEmployee";
                     parameters.Add("@Email", employeeVM.Email);
                     parameters.Add("@First", employeeVM.FirstName);
                     parameters.Add("@Last", employeeVM.LastName);
                     parameters.Add("@Birth", employeeVM.BirthDate);
                     parameters.Add("@Phone", employeeVM.PhoneNumber);
                     parameters.Add("@Address", employeeVM.Address);
                     parameters.Add("@Dept_Id", employeeVM.Department_Id);
                     var create = await connection.QueryAsync<EmployeeVM>(procName, parameters, commandType: CommandType.StoredProcedure);
                     return create;
                 }
             }

    }
}
