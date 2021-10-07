using Microsoft.AspNetCore.Mvc;
using NJBudgetWBackend.Models;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NJBudgetWBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyntheseController : ControllerBase
    {
        private ISyntheseService _synthService = null;
        private SyntheseController()
        {
            //Dummy for DI.
        }

        public SyntheseController(ISyntheseService service)
        {
            _synthService = service;
        }
        //getExpenseGroupByAppartenance
        // GET: api/<SyntheseController>
        [HttpGet("ByAppartenance")]
        public async Task<SyntheseDepenseGlobalModel> GetByAppartenanceAsync()
        {
           
            using var getTask = _synthService.GetSyntheseByAppartenanceAsync((byte)DateTime.Now.Month);
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return getTask.Result;
            }
            else
            {
                throw new Exception("C'est pas faux");
            }
        }

        //getExpenseGroupByAppartenance
        // GET: api/<SyntheseController>
        [HttpGet("ForAppartenance/{appartenanceId}")]
        public async Task<SyntheseDepenseByAppartenanceModel> GetByGroupAsync(Guid appartenanceId)
        {
            using Task<SyntheseDepenseByAppartenanceModel> getTask = _synthService.GetSyntheseForAppartenanceAsync(appartenanceId, (byte)DateTime.Now.Month);
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return getTask.Result;
            }
            else
            {
                throw new Exception("Vous m'avez pris pour un enseignant !!");
            }
        }

        [HttpGet("SyntheseMois")]
        public async Task<SyntheseMoisModel> GetGlobalAsync()
        {
            using Task<SyntheseMoisModel> getTask = _synthService.GetSyntheseGlobal((byte)DateTime.Now.Month);
            await getTask;
            if (getTask.IsCompletedSuccessfully)
            {
                return getTask.Result;
            }
            else
            {
                throw new Exception("Le gras c'est la vie.");
            }
        }



        // GET api/<SyntheseController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SyntheseController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SyntheseController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<SyntheseController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
