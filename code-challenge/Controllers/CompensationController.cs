using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation comp)
        {
            _logger.LogDebug($"Received compensation create request for employee with id = '{comp.Employee}'");

            comp = _compensationService.Create(comp);

            if (comp == null)
                return NotFound();

            return CreatedAtRoute("getCompensationByEmployeeId", new { id = comp.Employee }, comp);
        }

        [HttpGet("{id}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String id)
        {
            _logger.LogDebug($"Received compensation get request for employee {id}");

            var comp = _compensationService.GetByEmployeeId(id);

            if (comp == null)
                return NotFound();

            return Ok(comp);
        }
    }
}
