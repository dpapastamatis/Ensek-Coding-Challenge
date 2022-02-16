using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EnsekCodingChallenge.Models;

namespace EnsekCodingChallenge.Interfaces
{
    public interface IEmployeesWebServices
    {
        Task<IEnumerable<Employee>> GetEmployees();
        Task<(StatusCode code, Employee employee)> GetEmployee(int id);
        Task<StatusCode> PutEmployee(int id, Employee employee);
        Task<Employee> PostEmployee(Employee employee);
        Task<StatusCode> DeleteEmployee(int id);
    }
}
