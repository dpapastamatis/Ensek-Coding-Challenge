using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using EnsekCodingChallenge.Interfaces;
using EnsekCodingChallenge.Models;

namespace EnsekCodingChallenge.Implementations
{
    public class EmployeesWebServicesFromDB : IEmployeesWebServices, IDisposable
    {
        private ApplicationDbContext _context;

        public EmployeesWebServicesFromDB()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            //return await _context.Employees.Where(e => e.Linked_User.Email == System.Web.HttpContext.Current.User.Identity.Name).ToListAsync();
            return await _context.Employees.ToListAsync();
        }

        [ResponseType(typeof(Employee))]
        public async Task<(StatusCode code, Employee employee)> GetEmployee(int id)
        {
            Employee employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return (StatusCode.NotFound, null);
            }

            return (StatusCode.OK, employee);
        }


        [ResponseType(typeof(Employee))]
        public async Task<Employee> PostEmployee(Employee employee)
        {
            //var currentUser = _context.Users.FirstOrDefault(u => u.Email == System.Web.HttpContext.Current.User.Identity.Name);
            //employee.Linked_User = currentUser;
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return employee;
        }

        [ResponseType(typeof(void))]
        public async Task<StatusCode> PutEmployee(int id, Employee employee)
        {

            _context.Entry(employee).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return StatusCode.NotFound;
                }
                else
                {
                    throw;
                }
            }
            return StatusCode.OK;
        }

        [ResponseType(typeof(Employee))]
        public async Task<StatusCode> DeleteEmployee(int id)
        {
            Employee employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return StatusCode.NotFound;
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return StatusCode.OK;
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Count(e => e.ID == id) > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            _context = null;
        }
    }
}