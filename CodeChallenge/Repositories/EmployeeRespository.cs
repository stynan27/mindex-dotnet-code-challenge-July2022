using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            // TODO: directReports property is empty??
            // Problem: Previously we were only loading the data table, "Lazy Loading", which doesn't 
            // ... retrieve related complex structures for performance.

            // Solution: Filter for current employee first (Where) then include only its directReports (lazy load recursive employees)
            // Could have applied ToList() to materialize as list of employees before querying by id, 
            // ... but this would make the query slower as our indirect reports grow.
            // Include() allows you to indicate which related entities should be read from the database as part of the same query.
            return _employeeContext.Employees
              .Where(e => e.EmployeeId == id) 
              .Include("DirectReports")
              .SingleOrDefault();
              // .ToList()
              // .SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }
    }
}
