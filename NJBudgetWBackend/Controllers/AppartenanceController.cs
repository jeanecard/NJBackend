using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Commun;
using NJBudgetWBackend.Services.Interface;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NJBudgetWBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppartenanceController : ControllerBase
    {
        private IAppartenanceService _apService = null;
        private AppartenanceController()
        {

        }

        public AppartenanceController(IAppartenanceService ser)
        {
            _apService = ser;
        }
        // GET: api/<AppartenanceController>
        [HttpGet]
        public async Task<IEnumerable<Appartenance>> GetAsync()
        {
            await Task.Delay(1);
            var appartenances = _apService.Get();
            return appartenances;
        }
    }
}
