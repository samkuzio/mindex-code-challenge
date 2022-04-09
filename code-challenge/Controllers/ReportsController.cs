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
    [Route("api/employee/reports")]
    public class ReportsController : Controller
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;

        public ReportsController(ILogger<ReportsController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getReportsById")]
        public IActionResult GetReportsById(String id)
        {
            _logger.LogDebug($"Received reports get request for '{id}'");

            var reports = _employeeService.GetReportsById(id);

            if (reports == null)
                return NotFound();

            return Ok(reports);
        }
    }
}
