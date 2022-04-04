using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IOperationService _opeService;
        private readonly IGroupService _groupService;
        private readonly ILogger _logger;


        private OperationController()
        {

        }
        public OperationController(
            IOperationService service,
            IGroupService groupService,
            ILogger<OperationController> logger)
        {
            _opeService = service;
            _groupService = groupService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("add-operation")]
        public async Task<ActionResult<Compte>> AddAsync([FromBody] Operation value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            try
            {
                using var opTask = _opeService.AddAsync(value);
                await opTask;
                if (opTask.IsCompletedSuccessfully)
                {
                    DateTime now = DateTime.Now;
                    using var compteTask = _groupService.GetCompteAsync(value.CompteId, (byte)now.Month, (ushort)now.Year);
                    await compteTask;
                    return Ok(compteTask.Result);
                }
                else
                {
                    _logger.LogError("Could not procees task");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.InnerException?.Message);
                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("remove-operation")]
        public async Task<ActionResult<Compte>> RemoveAsync([FromBody] Operation value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            try
            {
                using var opTask = _opeService.RemoveAsync(value);
                await opTask;
                if (opTask.IsCompletedSuccessfully)
                {
                    DateTime now = DateTime.Now;
                    using var compteTask = _groupService.GetCompteAsync(value.CompteId, (byte)now.Month, (ushort)now.Year);
                    await compteTask;
                    return Ok(compteTask.Result);
                }
                else
                {
                    _logger.LogError("Could not procees task");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.InnerException?.Message);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idOperation"></param>
        /// <returns></returns>
        [HttpDelete("{idOperation}")]
        public async Task<ActionResult> DeleteAsync(Guid idOperation)
        {
            if (idOperation == Guid.Empty)
            {
                return BadRequest();
            }
            try
            {

                using var opTask = _opeService.DeleteAsync(idOperation);
                await opTask;
                if (opTask.IsCompletedSuccessfully)
                {
                    return Ok();
                }
                else
                {
                    _logger.LogError("Could not procees task");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogDebug(ex.InnerException?.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
