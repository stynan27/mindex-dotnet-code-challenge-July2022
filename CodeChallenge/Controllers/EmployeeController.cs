using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    //   [ApiController]
    // [Route("api/test")]
    // public class TestController : ControllerBase
    // {
    //     private readonly ILogger _logger;

    //     // constructor
    //     public TestController(ILogger<EmployeeController> logger)
    //     {
    //         _logger = logger;
    //     }

    //     [HttpGet("{id}")]
    //     public IActionResult GetTestResult(String id)
    //     {
    //         _logger.LogWarning("Testing...");
    //         _logger.LogInformation("Testing... inform");
    //         _logger.LogDebug($"Received employee get request for '{id}'");

    //         //Example of a console log/print
    //         //Console.WriteLine("This is C#");

    //         //var employee = _employeeService.GetById(id);

    //         // if (employee == null)
    //         //     return NotFound();

    //         return Ok("Hello World");
    //     }
    // }

    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        // CreateCompensation
        [HttpPost("{employeeId}/compensation", Name="createCompensation")]
        public IActionResult CreateCompensation(string employeeId, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for employee '{employeeId}'");
            // Log compensation body
            _logger.LogDebug($"Received compensation of Salary '{compensation.Salary}' and EffectiveDate '{compensation.EffectiveDate}'.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = _employeeService.GetById(employeeId);
            if (employee == null)
            {
                return NotFound();
            }

            // Update the EmployeeId property of the new compensation.
            compensation.EmployeeId = employeeId;

            // Create a new compensation (save to DB Context)
            _employeeService.CreateCompensation(compensation);

            return Ok();
            //return CreatedAtRoute("getCompensationById", new { id = compensation.CompensationId }, compensation);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // I could have created another controller instead for ReportingStructure, 
        // but I opted to tie this into an existing endpoint of employee (HAS-A Employee type).
        [HttpGet("{id}/reportingStructure", Name = "getReportingStructure")]
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received getReportingStructure get request for '{id}'");

            var reportingStructure = _employeeService.GetReportStructureById(id);

            // if no report structure was found
            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }

        // [HttpGet("{id}/compensation", Name = "getCompensationById")]
        // public IActionResult GetCompensationById(String id)
        // {
        //     _logger.LogDebug($"Received getCompensation get request for '{id}'");

        //     var compensation = _employeeService.GetCompensationById(id);

        //     // if no compensation structure was found
        //     if (compensation == null)
        //         return NotFound();

        //     return Ok(compensation);
        // }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Recieved employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }
    }
}
