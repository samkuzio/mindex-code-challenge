using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using challenge.Data;

namespace challenge.Repositories
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
            return _employeeContext.Employees.SingleOrDefault(e => e.EmployeeId == id);
        }

        public ReportingStructure GetReportsById(string id)
        {
            ReportingStructure reports = new ReportingStructure();

            // The number of reports is determined to be the number of directReports
            // for an employee and all of their direct reports
            reports.NumberOfReports = CountReports(id, 2);
            reports.Employee = GetById(id);

            return reports;
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        private int CountReports(string employeeId, int maxDepth)
        {
            return CountReports(employeeId, 0, maxDepth);
        }

        private int CountReports(string employeeId, int currentDepth, int maxDepth)
        {
            // Evaluate depth, allowing infinite traversal iff maxDepth == 0
            if (maxDepth != 0 && currentDepth > maxDepth)
                return 0;

            // Ensure direct reports are available in the current context
            var rootEmployee = _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == employeeId);

            var numDirectReports = rootEmployee.DirectReports.Count;

            // Skip level reports are counted with a recursive depth-first traversal of the org tree
            var numSkipLevelReports = rootEmployee.DirectReports
                    .Select(report => CountReports(report.EmployeeId, currentDepth + 1, maxDepth))
                    .Aggregate(0, (sum, val) => sum + val);

            return numDirectReports + numSkipLevelReports;
        }
    }
}
