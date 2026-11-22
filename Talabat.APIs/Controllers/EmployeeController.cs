using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Specifications.Employee_Specs;
using Talabat.Repository.Interfaces;

namespace Talabat.APIs.Controllers;

public class EmployeeController : BaseApiController
{
    private readonly IGenaricRepository<Employee> _employeeRepo;

    public EmployeeController(IGenaricRepository<Employee> employeeRepo)
    {
        _employeeRepo = employeeRepo;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
    {
     var spec = new  EmployeeWithDepartmentSpecifications();
        var employees = await _employeeRepo.GetAllWithSpecAsync(spec);
        return Ok(employees);
    }
    [HttpGet(template: "{id}")]
    public async Task<ActionResult<Employee>> GetById(int id)
    {
        var spec = new EmployeeWithDepartmentSpecifications(id);
        var employee = await _employeeRepo.GetByIdWithSpecAsync(spec);
        return Ok(employee);
    }


}
