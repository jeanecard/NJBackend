using Microsoft.AspNetCore.Mvc;
using NJBudgetBackEnd.Models;
using NJBudgetWBackend.Services.Interface;
using NJBudgetWBackend.Services.Interface.Interface;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NJBudgetWBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {
        private IOperationService _opeService = null;
        private IGroupService _groupService = null;

        private OperationController()
        {

        }
        public OperationController(
            IOperationService service,
            IGroupService groupService)
        {
            _opeService = service;
            _groupService = groupService;
        }
        // POST api/<OperationController>
        [HttpPost("add-operation")]
        public async Task<Compte> Add([FromBody] Operation value)
        {
            if(value == null )
            {
                return null;
            }
            using var opTask = _opeService.AddAsync(value);
            await opTask;
            if(opTask.IsCompletedSuccessfully)
            {
                DateTime now = DateTime.Now;
                using var compteTask = _groupService.GetCompteAsync(value.CompteId, (byte)now.Month, (ushort)now.Year);
                await compteTask;
                return compteTask.Result;
            }
            else
            {
                throw new Exception("Moi toutes ces histoires de merde, ça me fout la tête comme une callebasse !");
            }
        }

        // POST api/<OperationController>
        [HttpPost("remove-operation")]

        public async Task<Compte> Remove([FromBody] Operation value)
        {
            if (value == null)
            {
                return null;
            }
            using var opTask = _opeService.RemoveAsync(value);
            await opTask;
            if (opTask.IsCompletedSuccessfully)
            {
                DateTime now = DateTime.Now;
                using var compteTask = _groupService.GetCompteAsync(value.CompteId, (byte)now.Month, (ushort)now.Year);
                await compteTask;
                return compteTask.Result;
            }
            else
            {
                throw new Exception("Tu connais quelqu'un qui tire plus vite ? ... non Personne :-)");
            }
        }

        // DELETE api/<OperationController>/5
        [HttpDelete("{idOperation}")]
        public async Task<Compte> Delete(Guid idOperation)
        {
            if (idOperation == Guid.Empty)
            {
                return null;
            }
            using var opTask = _opeService.DeleteAsync(idOperation);
            await opTask;
            if (opTask.IsCompletedSuccessfully)
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new Exception("Les années ne font pas des sages, elles ne font que des viellards");
            }
        }
    }
}
