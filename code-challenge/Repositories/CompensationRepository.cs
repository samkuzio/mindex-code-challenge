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
    public class CompensationRepository : ICompensationRepository
    {
        private readonly ILogger<ICompensationRepository> _logger;
        private readonly CompensationContext _compensationContext;

        public CompensationRepository(ILogger<ICompensationRepository> logger, CompensationContext compensationContext)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation comp)
        {
            _compensationContext.Compensations.Add(comp);
            return comp;
        }

        public Compensation GetByEmployeeId(string id)
        {
            return _compensationContext.Compensations.SingleOrDefault(c => c.Employee == id);
        }

        public Compensation Remove(Compensation comp)
        {
            return _compensationContext.Remove(comp).Entity;
        }

        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
    }
}