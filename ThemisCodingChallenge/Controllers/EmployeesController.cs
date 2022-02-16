using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EnsekCodingChallenge.Interfaces;
using EnsekCodingChallenge.Models;

namespace EnsekCodingChallenge.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeesWebServices _employeesWebServices;
        public EmployeesController(IEmployeesWebServices employeesWebServices)
        {
            _employeesWebServices = employeesWebServices;
        }

        // GET: Employees
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View("EmployeeForm");
        }

        public async Task<ActionResult> Edit(int ID)
        {
            var result = await _employeesWebServices.GetEmployee(ID);
            if (result.code == StatusCode.NotFound)
                return HttpNotFound();
            return View("EmployeeForm", result.employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Save(Employee employee)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (!ModelState.IsValid)
                return View("EmployeeForm", employee);
            if (employee.ID == 0)
                await _employeesWebServices.PostEmployee(employee);
            else
                await _employeesWebServices.PutEmployee(employee.ID, employee);
            return RedirectToAction("Index", "Employees");
        }
    }
}