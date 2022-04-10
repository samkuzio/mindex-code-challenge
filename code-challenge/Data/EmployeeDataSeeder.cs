using challenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class EmployeeDataSeeder
    {
        private EmployeeContext _employeeContext;
        private CompensationContext _compensationContext;
        private const String EMPLOYEE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";
        private const String COMPENSATION_SEED_DATA_FILE = "resources/CompensationSeedData.json";

        public EmployeeDataSeeder(EmployeeContext employeeContext, CompensationContext compensationContext)
        {
            _employeeContext = employeeContext;
            _compensationContext = compensationContext;
        }

        public async Task Seed()
        {
            if(!_employeeContext.Employees.Any())
            {
                List<Employee> employees = LoadDataFile<Employee>(EMPLOYEE_SEED_DATA_FILE);
                FixUpReferences(employees);
                _employeeContext.Employees.AddRange(employees);
                await _employeeContext.SaveChangesAsync();

                List<Compensation> compensations = LoadDataFile<Compensation>(COMPENSATION_SEED_DATA_FILE);
                _compensationContext.Compensations.AddRange(compensations);
                await _compensationContext.SaveChangesAsync();
            }
        }

        private List<T> LoadDataFile<T>(string dataFile)
        {
            using (FileStream fs = new FileStream(dataFile, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<T> entries = serializer.Deserialize<List<T>>(jr);

                return entries;
            }
        }

        private void FixUpReferences(List<Employee> employees)
        {
            var employeeIdRefMap = from employee in employees
                                select new { Id = employee.EmployeeId, EmployeeRef = employee };

            employees.ForEach(employee =>
            {
                
                if (employee.DirectReports != null)
                {
                    var referencedEmployees = new List<Employee>(employee.DirectReports.Count);
                    employee.DirectReports.ForEach(report =>
                    {
                        var referencedEmployee = employeeIdRefMap.First(e => e.Id == report.EmployeeId).EmployeeRef;
                        referencedEmployees.Add(referencedEmployee);
                    });
                    employee.DirectReports = referencedEmployees;
                }
            });
        }
    }
}
