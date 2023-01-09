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

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        // TODO: Add another GET here for reporting structure
        // I could have created another controller instead for ReportingStructure, 
        // but I opted to tie this into an existing endpoint of employee.
        [HttpGet("{id}/reportingStructure", Name = "getReportingStructure")]
        public IActionResult GetReportingStructure(String id)
        {
            _logger.LogDebug($"Received getReportingStructure get request for '{id}'");

            var reportingStructure = _employeeService.GetReportStructure(id);

            // if no report structure was found
            if (reportingStructure == null)
                return NotFound();

            return Ok(reportingStructure);
        }

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
