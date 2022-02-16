using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using EnsekCodingChallenge.Extensions;
using EnsekCodingChallenge.Interfaces;
using EnsekCodingChallenge.Models;

namespace EnsekCodingChallenge.Implementations
{
    public class EmployeesWebServicesFromApi : IEmployeesWebServices, IDisposable
    {
        private readonly string BaseUrl = "https://dummy.restapiexample.com/api/v1";
        private HttpClient _client;

        public EmployeesWebServicesFromApi()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            var content = await _client.GetStringAsync(BaseUrl + "/employees");
            var parsedObject = JObject.Parse(content);
            var jsonData = parsedObject["data"].ToString();

            return JsonConvert.DeserializeObject<IEnumerable<Employee>>(jsonData);
        }

        public async Task<(StatusCode code, Employee employee)> GetEmployee(int id)
        {
            var content = await _client.GetStringAsync(BaseUrl + "/employee/" + id);
            var parsedObject = JObject.Parse(content);
            var jsonData = parsedObject["data"].ToString();
            Employee employee = JsonConvert.DeserializeObject<Employee>(jsonData);
            if (employee.ID != 0)
                return (StatusCode.OK, employee);
            else
                return (StatusCode.NotFound, null);
        }

        public async Task<Employee> PostEmployee(Employee employee)
        {
            var content = JsonConvert.SerializeObject(employee);
            await _client.PostAsync(BaseUrl + "/employees", new StringContent(content));
            //if actual api, GetEmployee to get employee with ID
            return employee;
        }

        public async Task<StatusCode> PutEmployee(int id, Employee employee)
        {
            var result = await GetEmployee(id);
            if (result.code == global::StatusCode.NotFound)
                return StatusCode.NotFound;
            var content = JsonConvert.SerializeObject(employee);
            await _client.PutAsync(BaseUrl + "/employee/" + id, new StringContent(content));
            return StatusCode.OK;
        }

        public async Task<StatusCode> DeleteEmployee(int id)
        {
            var result = await GetEmployee(id);
            if (result.code == global::StatusCode.NotFound)
                return StatusCode.NotFound;
            await _client.DeleteAsync(BaseUrl + "/employee/" + id);
            return StatusCode.OK;
        }

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}