using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ILogger<CompensationService> _logger;
        private readonly ICompensationRepository _compensationRepository;
        private readonly IEmployeeService _employeeService;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository, IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationRepository = compensationRepository;
            _employeeService = employeeService;
        }

        public Compensation Create(Compensation comp)
        {
            if (comp == null)
                return null;

            // Assume a 1:1 Employee:Compensation relationship and enforce POST idempotence
            // if a compensation already exists
            var existingComp = _compensationRepository.GetByEmployeeId(comp.Employee);
            if (existingComp != null)
                return Replace(existingComp, comp);

            var employee = _employeeService.GetById(comp.Employee);
            if (employee == null)
                return null;

            _compensationRepository.Add(comp);
            _compensationRepository.SaveAsync().Wait();

            return comp;
        }

        public Compensation GetByEmployeeId(string id)
        {
            return _compensationRepository.GetByEmployeeId(id);
        }

        public Compensation Replace(Compensation oldComp, Compensation newComp)
        {
            if (oldComp == null)
                return newComp;

            _compensationRepository.Remove(oldComp);
            _compensationRepository.SaveAsync().Wait();

            if (newComp != null)
            {
                _compensationRepository.Add(newComp);
                _compensationRepository.SaveAsync().Wait();
            }

            return newComp;
        }
    }
}
