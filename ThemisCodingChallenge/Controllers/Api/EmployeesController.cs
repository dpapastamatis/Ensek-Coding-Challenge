using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EnsekCodingChallenge.Implementations;
using EnsekCodingChallenge.Interfaces;
using EnsekCodingChallenge.Models;

namespace EnsekCodingChallenge.Controllers.Api
{
    public class EmployeesController : ApiController
    {
        private readonly IEmployeesWebServices _employeesWebServices;

        public EmployeesController(IEmployeesWebServices employeesWebServices)
        {
            _employeesWebServices = employeesWebServices;
        }

        // GET: api/Employees
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await _employeesWebServices.GetEmployees();
        }

        // GET: api/Employees/5
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> GetEmployee(int id)
        {
            var result = await _employeesWebServices.GetEmployee(id);
            if (result.code == global::StatusCode.NotFound)
                return NotFound();
            return Ok(result.employee);
        }

        // PUT: api/Employees/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmployee(int id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.ID)
            {
                return BadRequest();
            }

            var result = await _employeesWebServices.PutEmployee(id, employee);
            if (result == global::StatusCode.NotFound)
                return NotFound();
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Employees
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employeeCreated = await _employeesWebServices.PostEmployee(employee);
            return CreatedAtRoute("DefaultApi", new { id = employeeCreated.ID }, employeeCreated);

        }

        // DELETE: api/Employees/5
        [ResponseType(typeof(Employee))]
        public async Task<IHttpActionResult> DeleteEmployee(int id)
        {
            var result = await _employeesWebServices.DeleteEmployee(id);
            if (result == global::StatusCode.NotFound)
                return NotFound();
            return Ok();
        }
    }
}