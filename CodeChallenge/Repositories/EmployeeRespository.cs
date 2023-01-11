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

        // Add a new compensation to DB context.
        public Compensation Add(Compensation compensation)
        {
            compensation.CompensationId = Guid.NewGuid().ToString();
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Employee GetById(string id)
        {
            // TODO: directReports property is empty??
            // Problem: Previously we were only loading the data table, "Lazy Loading", which doesn't 
            // ... retrieve related complex structures for performance.

            // Solution: Applied ToList() to materialize as list of employees before querying by id, 
            // ... but this would make the query slower as our indirect reports grow.
            // Filter for current employee first (Where) then include only its directReports (lazy load recursive employees)
            // Include() allows you to indicate which related entities should be read from the database as part of the same query.

            // NOTE: One of the slower parts of database queries is the transfer of data from the database to your local (C#) process. 
            // Therefore it is very wise not to select any properties you don't plan to use.
            // From: https://stackoverflow.com/questions/48852875/entity-framework-6-include-field-from-parent
            return _employeeContext.Employees
              // Could perform the following to only get parent directReports
              // .Where(e => e.EmployeeId == id) 
              // .Include("DirectReports")
              // .SingleOrDefault();
              .ToList()
              .SingleOrDefault(e => e.EmployeeId == id);
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
