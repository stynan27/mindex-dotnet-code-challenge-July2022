using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;

        // TODO: Helper method to get number of reports
        // Similar to JavaScript Challenge - Breadth First Search of Employee Hierarchy
        private int GetTotalReports(Employee firstEmployee)
        {
          // queue of employees to check until - until all have been visited
          Queue<Employee> employeeQueue = new Queue<Employee>();
          // initalize with the first employee to check
          employeeQueue.Enqueue(firstEmployee);
          // Dictionary of all reporting employees found
          Dictionary<string, Employee> allReports = new Dictionary<string, Employee>();

          // employee to check
          Employee currentEmployee = null;

          // continue to find reports while queue is not empty
          while(employeeQueue.Count != 0)
          {
              currentEmployee = employeeQueue.Dequeue();
              //_logger.LogInformation(currentEmployee.DirectReports[0].FirstName);
              if (currentEmployee.DirectReports != null) 
              {
                  foreach (Employee reportEmployee in currentEmployee.DirectReports)
                  {
                      if (!(allReports.ContainsKey(reportEmployee.EmployeeId)))
                      {
                          // Fetch report employee here to get its directReports.
                          // Problem: Can become expensive to fetch, but only as needed.
                          // Employee reportEmployee = _employeeRepository.GetById(directReport.EmployeeId);
                          employeeQueue.Enqueue(reportEmployee);
                          _logger.LogInformation("Employee Added... " + reportEmployee.FirstName + ' ' + reportEmployee.LastName);
                          allReports.Add(reportEmployee.EmployeeId, reportEmployee);
                      }
                  }
              }
          }

          // return total number found
          return allReports.Count;
        }

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        // TODO: Create compensation
        public Compensation CreateCompensation(Compensation compensation)
        {
            if(compensation != null)
            {
                _employeeRepository.Add(compensation);
                _employeeRepository.SaveAsync().Wait();
            }

            return compensation;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public ReportingStructure GetReportStructureById(string id)
        {
            // First check for a valid employee id
            if (String.IsNullOrEmpty(id))
            {
              return null;
            }
            
            // Attempt to find the employee by the id provided
            Employee reportEmployee = _employeeRepository.GetById(id);
            if (reportEmployee == null)
            {
              return null;
            }

            // Find total reports (number of direct and indirect to return)
            int numReports = GetTotalReports(reportEmployee);

            return new ReportingStructure
            {
                Employee = reportEmployee,
                NumberOfReports = numReports
            };
        }

        // TODO: Define Get request
        // public Employee GetCompensationById(string id)
        // {
        //     if(!String.IsNullOrEmpty(id))
        //     {
        //         return _employeeRepository.GetById(id);
        //     }

        //     return null;
        // }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
